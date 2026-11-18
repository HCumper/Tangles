// Build Release. Invoke with --reference:<dir>/Tangles.Core.dll and
// --reference:<dir>/Tangles.Generator.dll, then this script and:
// <baseline-pack.json> <report.json> [solver-work=2000000]
// Baseline packs must be full coverage. Each original seed is replayed with the
// fast generator (and checked against the saved clues), then with a hard target.
open System
open System.Collections.Generic
open System.Diagnostics
open System.IO
open System.Security.Cryptography
open System.Text.Json
open System.Threading
open Tangles.Core
open Tangles.Generator

let args = fsi.CommandLineArgs |> Array.skip 1
if args.Length < 2 || args.Length > 3 then failwith "Expected baseline pack, output report, and optional solver-work limit."
let readInt name (element: JsonElement) = element.GetProperty(name: string).GetInt32()
let source = File.ReadAllText args[0]
let input = JsonDocument.Parse source
let generation = input.RootElement.GetProperty "generation"
let policy = { GenerationPolicy.defaults with Target = Hard; SolverWork = if args.Length = 3 then int args[2] else Solver.defaults.MaxWork }
let options =
    { Generation.defaults (readInt "seed" generation) with
        Mode = FullCoverage; Width = readInt "width" generation; Height = readInt "height" generation
        PairCount = readInt "pairCount" generation; MinPathLength = readInt "minPathLength" generation
        MaxPathLength = readInt "maxPathLength" generation; MaxAttempts = readInt "maxAttempts" generation }
let baseline = PuzzlePacks.read source |> Result.defaultWith failwith
if baseline.Puzzles |> Array.exists (fun p -> not p.Puzzle.RequireFullCoverage) then failwith "Full coverage is required."
let documents = input.RootElement.GetProperty("puzzles").EnumerateArray() |> Seq.toArray
let mutable accepted = []

// Independent direct witness check, separate from production validation.
let check (puzzle: GeneratedPuzzle) =
    let occupied = HashSet<int*int>()
    for path in puzzle.Solution do
        let cells = List.toArray path.Cells
        let clues = puzzle.Puzzle.Pairs |> List.find (fun p -> p.Label = path.Label)
        if cells[0] <> clues.Start || cells[cells.Length-1] <> clues.Finish then failwith "Wrong endpoints."
        if cells.Length < options.MinPathLength || cells.Length > options.MaxPathLength then failwith "Wrong path length."
        for i in 0 .. cells.Length-1 do
            let a = cells[i]
            if a.X < 0 || a.Y < 0 || a.X >= options.Width || a.Y >= options.Height || not (occupied.Add(a.X,a.Y)) then failwith "Invalid occupied cell."
            for j in i+1 .. cells.Length-1 do
                let b = cells[j]
                let adjacent = abs(a.X-b.X)+abs(a.Y-b.Y)=1
                if adjacent <> (j=i+1) then failwith "Wrong adjacency or self-touching."
    if occupied.Count <> options.Width*options.Height then failwith "Incomplete coverage."

// Warm both code paths; reporting and baseline analysis are outside timers.
Generation.generate CancellationToken.None options |> ignore
GuidedGeneration.generate CancellationToken.None options policy |> ignore

let cases =
    baseline.Puzzles |> Array.mapi (fun i before ->
        let settings = { options with Seed = readInt "seed" documents[i] }
        let timer = Stopwatch.StartNew()
        let replay = Generation.generate CancellationToken.None settings |> Result.defaultWith failwith
        timer.Stop()
        let beforeMs = timer.Elapsed.TotalMilliseconds
        if replay.Puzzle <> before.Puzzle then failwith $"Fast generator changed the baseline clues for seed {settings.Seed}."
        check replay
        let beforeAnalysis = Solver.analyse CancellationToken.None { Solver.defaults with MaxWork = policy.SolverWork } before.Puzzle
        timer.Restart()
        let result = GuidedGeneration.generate CancellationToken.None settings policy
        timer.Stop()
        let afterMs = timer.Elapsed.TotalMilliseconds
        match result with
        | Error error ->
            printfn "seed %d: %s -> failed (%s)" settings.Seed beforeAnalysis.Tier error
            {| seed = settings.Seed; before = beforeAnalysis; after = None; selection = None; error = error; beforeMs = beforeMs; afterMs = afterMs |}
        | Ok puzzle ->
            check puzzle
            let quality = puzzle.Quality.Value
            let analysis = quality.Reasoning.Value
            accepted <- puzzle::accepted
            printfn "seed %d: %s/%d -> %s/%d; %.1f ms" settings.Seed beforeAnalysis.Tier beforeAnalysis.TrialSteps analysis.Tier analysis.TrialSteps afterMs
            {| seed = settings.Seed; before = beforeAnalysis; after = Some analysis; selection = Some quality.Selection; error = ""; beforeMs = beforeMs; afterMs = afterMs |})

let acceptedPuzzles = accepted |> List.sortBy (fun p -> let a=p.Quality.Value.Reasoning.Value in a.Score,a.TrialSteps,a.RouteDeductions,p.Seed) |> List.toArray
let pack = Export.createPack options acceptedPuzzles
let destination = Path.GetFullPath args[1]
Directory.CreateDirectory(Path.GetDirectoryName destination) |> ignore
File.WriteAllText(destination + ".pack.json", Export.toJson pack)
let report =
    {| baselinePackSha256 = SHA256.HashData(File.ReadAllBytes args[0]) |> Convert.ToHexString
       currentAssemblySha256 = SHA256.HashData(File.ReadAllBytes typeof<GenerationOptions>.Assembly.Location) |> Convert.ToHexString
       width = options.Width; height = options.Height; pairCount = options.PairCount
       minLength = options.MinPathLength; maxLength = options.MaxPathLength; maxAttempts = options.MaxAttempts
       target = "hard"; solverWork = policy.SolverWork; candidateLimit = policy.MaxCandidates; improveSteps = policy.ImproveSteps
       requested = cases.Length; accepted = acceptedPuzzles.Length; cases = cases |}
File.WriteAllText(destination, JsonSerializer.Serialize(report, JsonSerializerOptions(PropertyNamingPolicy=JsonNamingPolicy.CamelCase,WriteIndented=true)))
printfn "Accepted %d/%d; %.1f ms/request." acceptedPuzzles.Length cases.Length (cases |> Array.averageBy _.afterMs)
input.Dispose()
