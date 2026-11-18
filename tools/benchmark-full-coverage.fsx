// Build the generator in Release, then run in a fresh FSI process per version:
// dotnet fsi --reference:<dir>/Tangles.Core.dll --reference:<dir>/Tangles.Generator.dll tools/benchmark-full-coverage.fsx <output.json> [seedCount]
// Uses individual seeds 0..seedCount-1, not the CLI's derived batch seeds.
open System
open System.Collections.Generic
open System.Diagnostics
open System.IO
open System.Runtime.InteropServices
open System.Security.Cryptography
open System.Text.Json
open System.Threading
open Tangles.Core
open Tangles.Generator

type Trial =
    { Seed: int
      Milliseconds: float
      Success: bool
      Error: string
      Attempts: int
      StraightPaths: int
      Turns: int
      AllPathsAtMostTwoTurns: bool
      ClueKey: string
      PuzzleId: string
      SolutionSha256: string
      Grade: string
      DifficultyScore: int
      Puzzle: Export.PuzzleDocument option }

type Profile = { Name: string; Algorithm: string; Options: GenerationOptions; Trials: Trial array }
type Report =
    { Runtime: string
      OS: string
      Architecture: string
      GeneratorSha256: string
      SeedCount: int
      Profiles: Profile array }

let arguments = fsi.CommandLineArgs |> Array.skip 1
if arguments.Length < 1 || arguments.Length > 2 then
    failwith "Usage: benchmark-full-coverage.fsx <output.json> [seedCount]"
let seedCount = if arguments.Length = 2 then int arguments[1] else 100
if seedCount < 1 || seedCount > 10000 then failwith "seedCount must be 1..10000."

// Independent witness check: no shared production validator/adjacency helper.
let check (options: GenerationOptions) (generated: GeneratedPuzzle) =
    let occupied = HashSet<Cell>()
    if generated.Solution.Length <> options.PairCount then failwith "Wrong path count."
    for path in generated.Solution do
        let cells = List.toArray path.Cells
        let pair = generated.Puzzle.Pairs |> List.find (fun p -> p.Label = path.Label)
        if cells.Length < options.MinPathLength || cells.Length > options.MaxPathLength then failwith "Wrong path length."
        if cells[0] <> pair.Start || cells[cells.Length - 1] <> pair.Finish then failwith "Wrong endpoints."
        for i in 0 .. cells.Length - 1 do
            let a = cells[i]
            if a.X < 0 || a.Y < 0 || a.X >= options.Width || a.Y >= options.Height || not (occupied.Add a) then
                failwith "Out of bounds, shared or repeated cell."
            for j in i + 1 .. cells.Length - 1 do
                let b = cells[j]
                let distance = abs (a.X - b.X) + abs (a.Y - b.Y)
                if (j = i + 1 && distance <> 1) || (j > i + 1 && distance = 1) then failwith "Invalid adjacency."
    if options.Mode = FullCoverage && occupied.Count <> options.Width * options.Height then failwith "Incomplete coverage."

let turns (path: SolutionPath) =
    path.Cells
    |> List.windowed 3
    |> List.sumBy (function
        | [a; b; c] when (b.X - a.X, b.Y - a.Y) <> (c.X - b.X, c.Y - b.Y) -> 1
        | _ -> 0)

// Ignore labels, pair ordering and endpoint direction; keep board orientation.
let clueKey (puzzle: Puzzle) =
    puzzle.Pairs
    |> List.map (fun p ->
        let a = p.Start.Y * puzzle.Width + p.Start.X
        let b = p.Finish.Y * puzzle.Width + p.Finish.X
        min a b, max a b)
    |> List.sort
    |> List.map (fun (a, b) -> $"{a}:{b}")
    |> String.concat ";"

let profiles =
    [ "8x8-six", { Generation.defaults 0 with Mode = FullCoverage }
      "8x8-twelve", { Generation.defaults 0 with Mode = FullCoverage; PairCount = 12; MinPathLength = 3; MaxPathLength = 10 }
      "8x8-five", { Generation.defaults 0 with Mode = FullCoverage; PairCount = 5 }
      "7x5-four", { Generation.defaults 0 with Mode = FullCoverage; Width = 7; Height = 5; PairCount = 4 }
      "10x10-ten", { Generation.defaults 0 with Mode = FullCoverage; Width = 10; Height = 10; PairCount = 10; MaxPathLength = 20 }
      "16x16-sixteen", { Generation.defaults 0 with Mode = FullCoverage; Width = 16; Height = 16; PairCount = 16 }
      "16x16-looser-lengths", { Generation.defaults 0 with Mode = FullCoverage; Width = 16; Height = 16; PairCount = 16; MaxPathLength = 32 }
      "3x3-two", { Generation.defaults 0 with Mode = FullCoverage; Width = 3; Height = 3; PairCount = 2; MinPathLength = 2; MaxPathLength = 7 }
      "connect-only-control", Generation.defaults 0 ]
    |> List.map (fun (name, options) ->
        // Warm JIT/allocators before timing; grading and export are outside timing.
        for seed in [-1001; -1002; -1003] do
            Generation.generate CancellationToken.None { options with Seed = seed } |> ignore
        GC.Collect()
        let trials =
            Array.init seedCount (fun seed ->
                let settings = { options with Seed = seed }
                let timer = Stopwatch.StartNew()
                let result = Generation.generate CancellationToken.None settings
                timer.Stop()
                match result with
                | Error error ->
                    { Seed = seed; Milliseconds = timer.Elapsed.TotalMilliseconds; Success = false; Error = error
                      Attempts = 0; StraightPaths = 0; Turns = 0; AllPathsAtMostTwoTurns = false; ClueKey = ""
                      PuzzleId = ""; SolutionSha256 = ""; Grade = ""; DifficultyScore = 0; Puzzle = None }
                | Ok puzzle ->
                    check settings puzzle
                    let bends = puzzle.Solution |> List.map turns
                    let document = (Export.createPack settings [|puzzle|]).Puzzles[0]
                    { Seed = seed; Milliseconds = timer.Elapsed.TotalMilliseconds; Success = true; Error = ""
                      Attempts = puzzle.Attempts; StraightPaths = bends |> List.filter ((=) 0) |> List.length
                      Turns = List.sum bends; AllPathsAtMostTwoTurns = bends |> List.forall (fun n -> n <= 2)
                      ClueKey = clueKey puzzle.Puzzle; PuzzleId = document.Id
                      SolutionSha256 = JsonSerializer.SerializeToUtf8Bytes(document.Solution) |> SHA256.HashData |> Convert.ToHexString
                      Grade = document.Difficulty.Grade; DifficultyScore = document.Difficulty.Score
                      // Keep two reproducible examples, rather than thousands of witnesses.
                      Puzzle = if seed = 0 || seed = 42 then Some document else None })
        let successes = trials |> Array.filter _.Success
        let paths = successes.Length * options.PairCount
        let straight = successes |> Array.sumBy _.StraightPaths
        let totalTurns = successes |> Array.sumBy _.Turns
        let duplicates = successes.Length - (successes |> Array.map _.ClueKey |> Array.distinct |> Array.length)
        printfn "%s: %d/%d successes; straight %d/%d; %.3f turns/path; %d duplicates; %.2f ms/seed"
            name successes.Length seedCount straight paths
            (if paths = 0 then 0. else float totalTurns / float paths) duplicates (trials |> Array.averageBy _.Milliseconds)
        { Name = name; Algorithm = (Export.createPack options [||]).Generation.Algorithm; Options = options; Trials = trials })
    |> List.toArray

let assemblyPath = typeof<GenerationOptions>.Assembly.Location
let report =
    { Runtime = RuntimeInformation.FrameworkDescription; OS = RuntimeInformation.OSDescription
      Architecture = string RuntimeInformation.ProcessArchitecture
      GeneratorSha256 = SHA256.HashData(File.ReadAllBytes assemblyPath) |> Convert.ToHexString
      SeedCount = seedCount; Profiles = profiles }
// Serialize the F# mode explicitly; System.Text.Json does not encode DUs.
let output =
    {| runtime = report.Runtime; os = report.OS; architecture = report.Architecture
       generatorSha256 = report.GeneratorSha256; seedCount = report.SeedCount
       profiles = report.Profiles |> Array.map (fun profile ->
           {| name = profile.Name
              algorithm = profile.Algorithm
              mode = if profile.Options.Mode = FullCoverage then "full-coverage" else "connect-only"
              width = profile.Options.Width; height = profile.Options.Height; pairCount = profile.Options.PairCount
              minPathLength = profile.Options.MinPathLength; maxPathLength = profile.Options.MaxPathLength
              maxAttempts = profile.Options.MaxAttempts; trials = profile.Trials |}) |}
let destination = Path.GetFullPath arguments[0]
Directory.CreateDirectory(Path.GetDirectoryName destination) |> ignore
File.WriteAllText(destination, JsonSerializer.Serialize(output, JsonSerializerOptions(PropertyNamingPolicy = JsonNamingPolicy.CamelCase, WriteIndented = true)))
