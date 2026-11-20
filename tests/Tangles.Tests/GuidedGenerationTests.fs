module Tangles.GuidedGenerationTests

open System
open System.IO
open System.Text.Json
open System.Threading
open Tangles.Core
open Tangles.Generator
open Xunit

let private require = function Ok value -> value | Error message -> failwith message
let private hard seed =
    GuidedGeneration.generate CancellationToken.None { Generation.defaults seed with Mode = FullCoverage }
        { GenerationPolicy.defaults with Target = Hard } |> require

[<Theory>]
[<InlineData(0)>]
[<InlineData(1)>]
[<InlineData(3)>]
[<InlineData(42)>]
let ``hard selection returns trial reasoning without guesses and respects witness bounds`` seed =
    let puzzle = hard seed
    let report = puzzle.Quality.Value.Reasoning.Value
    Assert.Equal("hard", report.Grade)
    Assert.Equal("trial", report.Tier)
    Assert.True(report.SolvedByReasoning)
    Assert.True(report.TrialSteps > 0)
    Assert.Equal(0, report.SearchNodes)
    Assert.Empty(Validation.solution puzzle.Puzzle puzzle.Solution)
    Assert.All(puzzle.Solution, fun path -> Assert.InRange(path.Cells.Length, 4, 16))
    Assert.InRange(puzzle.Quality.Value.Selection.CandidatesConsidered, 1, 100)

[<Theory>]
[<InlineData("easy",10,16)>]
[<InlineData("medium",8,16)>]
[<InlineData("expert",4,32)>]
let ``difficulty targets are exact and expert explicitly permits branching`` grade pairs maximum =
    let target = match grade with "easy" -> Easy | "medium" -> Medium | _ -> Expert
    let options = { Generation.defaults 42 with Mode = FullCoverage; PairCount = pairs; MaxPathLength = maximum }
    let puzzle = GuidedGeneration.generate CancellationToken.None options { GenerationPolicy.defaults with Target = target } |> require
    Assert.Equal(grade, puzzle.Quality.Value.Reasoning.Value.Grade)
    Assert.Empty(Validation.solution puzzle.Puzzle puzzle.Solution)
    if target = Expert then Assert.False(puzzle.Quality.Value.Reasoning.Value.SolvedByReasoning)

[<Fact>]
let ``hard generation and assessments replay exactly from seed and policy`` () =
    let first, second = hard 42, hard 42
    Assert.Equal(first, second)
    let settings = { Generation.defaults 42 with Mode = FullCoverage }
    Assert.Equal(Export.toJson (Export.createPack settings [|first|]), Export.toJson (Export.createPack settings [|second|]))

[<Fact>]
let ``larger mixed layouts have enough search time to produce checked hard puzzles`` () =
    let options =
        { Generation.defaults 42 with Mode = FullCoverage; Width = 12; Height = 12; PairCount = 12
                                      MinPathLength = 3; MaxPathLength = 32; MaxAttempts = 5000 }
    let policy = { GenerationPolicy.defaults with Target = Hard; MaxCandidates = 10; ImproveSteps = 0; SolverWork = 20_000_000 }
    let puzzle = GuidedGeneration.generate CancellationToken.None options policy |> require
    Assert.Equal("trial", puzzle.Quality.Value.Reasoning.Value.Tier)
    Assert.True(puzzle.Quality.Value.Reasoning.Value.SolvedByReasoning)
    Assert.Empty(Validation.solution puzzle.Puzzle puzzle.Solution)
    Assert.All(puzzle.Solution, fun path -> Assert.InRange(path.Cells.Length,3,32))

[<Fact>]
let ``an unmet target and an exhausted solver budget never fall back to easy output`` () =
    let options = { Generation.defaults 0 with Mode = FullCoverage; Width = 1; Height = 4; PairCount = 2; MinPathLength = 2; MaxPathLength = 2 }
    for work in [1; Solver.defaults.MaxWork] do
        let policy = { GenerationPolicy.defaults with Target = Hard; MaxCandidates = 2; SolverWork = work }
        match GuidedGeneration.generate CancellationToken.None options policy with
        | Ok _ -> failwith "This board cannot meet the hard target."
        | Error message -> Assert.Contains("No easier puzzle was substituted", message)

[<Fact>]
let ``unfiltered generation remains identical and connect-only screening is separate`` () =
    let options = Generation.defaults 42
    Assert.Equal(Generation.generate CancellationToken.None options,
                 GuidedGeneration.generate CancellationToken.None options GenerationPolicy.defaults)
    let screened = GuidedGeneration.generate CancellationToken.None options { GenerationPolicy.defaults with AvoidShortest = true } |> require
    Assert.False(screened.Quality.Value.ShortestRoutes.Value.Solved)
    Assert.Equal(32, screened.Quality.Value.ShortestRoutes.Value.Trials)
    Assert.True(screened.Quality.Value.Reasoning.IsNone)
    Assert.Empty(Validation.solution screened.Puzzle screened.Solution)
    Assert.Equal(screened, GuidedGeneration.generate CancellationToken.None options { GenerationPolicy.defaults with AvoidShortest = true } |> require)

[<Fact>]
let ``uniqueness is optional and assessment does not mistake ambiguity for connect-only difficulty`` () =
    let options = { Generation.defaults 42 with Width = 3; Height = 3; PairCount = 1; MinPathLength = 2; MaxPathLength = 2 }
    let selected = GuidedGeneration.generate CancellationToken.None options { GenerationPolicy.defaults with RequireUnique = true } |> require
    Assert.Equal("unique", selected.Quality.Value.Reasoning.Value.Uniqueness)
    let assessment = selected.Quality.Value.Reasoning.Value
    Assert.Equal("unrated", assessment.Grade)
    let pack = Export.createPack options [|selected|]
    Assert.Equal("geometric-heuristic-v1", pack.Puzzles[0].Difficulty.Method)
    Assert.Equal("unique", pack.Puzzles[0].Verification.Uniqueness)

[<Fact>]
let ``solver JSON becomes the primary grade and retains the original geometry estimate`` () =
    let puzzle = hard 3
    let options = { Generation.defaults 3 with Mode = FullCoverage }
    let pack = Export.createPack options [|puzzle|]
    let document = pack.Puzzles[0]
    Assert.Equal("hard", document.Difficulty.Grade)
    Assert.Equal("cell-domain-reasoning-v1", document.Difficulty.Method)
    Assert.Equal("reasoningEstimate", document.Verification.Difficulty)
    Assert.Equal("unique", document.Verification.Uniqueness)
    Assert.Equal("solver-guided-hamiltonian-partition-v5", pack.Generation.Algorithm)
    let old = Difficulty.assess puzzle.Puzzle
    Assert.Equal(old.Score, document.Difficulty.GeometricEstimate.Value.Score)
    Assert.Equal<Difficulty.Component array>(old.Components, document.Difficulty.GeometricEstimate.Value.Components)
    Assert.Equal(float document.Difficulty.Score, document.Difficulty.Components |> Array.sumBy _.Points)
    let loaded = Export.toJson pack |> PuzzlePacks.read |> require
    Assert.Equal(puzzle.Puzzle, loaded.Puzzles[0].Puzzle)
    let stale = { puzzle with Quality = puzzle.Quality |> Option.map (fun q -> { q with Reasoning = q.Reasoning |> Option.map (fun a -> { a with BoardKey = "different" }) }) }
    Assert.Throws<ArgumentException>(fun () -> Export.createPack options [|stale|] |> ignore) |> ignore

[<Fact>]
let ``CLI validates target modes budgets and optional uniqueness`` () =
    let args = [|"--mode";"full-coverage";"--difficulty";"hard";"--candidates";"30";"--improve-steps";"0";"--solver-work";"5000000";"--require-unique"|]
    match CommandLine.parse 42 args |> require with
    | Generate options ->
        Assert.Equal(Hard, options.Selection.Target)
        Assert.Equal(30, options.Selection.MaxCandidates)
        Assert.Equal(0, options.Selection.ImproveSteps)
        Assert.Equal(5_000_000, options.Selection.SolverWork)
        Assert.True(options.Selection.RequireUnique)
    | Help -> failwith "Expected generation."
    for invalid in
        [ [|"--difficulty";"hard"|]
          [|"--mode";"full-coverage";"--avoid-shortest"|]
          [|"--difficulty";"unknown"|]
          [|"--candidates";"0"|]
          [|"--improve-steps";"-1"|]
          [|"--solver-work";"0"|]
          [|"--assess";"--assess"|]
          [|"--mode";"full-coverage";"--width";"17";"--require-unique"|] ] do
        Assert.True(CommandLine.parse 42 invalid |> Result.isError)

[<Fact>]
let ``selected CLI packs are distinct ordered and byte-reproducible`` () =
    let first = Path.Combine(Path.GetTempPath(), $"tangles-hard-{Guid.NewGuid():N}.json")
    let second = Path.Combine(Path.GetTempPath(), $"tangles-hard-{Guid.NewGuid():N}.json")
    try
        for path in [first;second] do
            Assert.Equal(0, Tangles.Generator.Program.main [|"--mode";"full-coverage";"--difficulty";"hard";"--seed";"42";"--count";"4";"--output";path|])
        Assert.Equal(File.ReadAllText first, File.ReadAllText second)
        let loaded = File.ReadAllText first |> PuzzlePacks.read |> require
        Assert.Equal(4, loaded.Puzzles.Length)
        Assert.Equal(4, loaded.Puzzles |> Array.map (fun p -> Solver.boardKey p.Puzzle) |> Array.distinct |> Array.length)
        use document = JsonDocument.Parse(File.ReadAllText first)
        let trials = document.RootElement.GetProperty("puzzles").EnumerateArray() |> Seq.map (fun p -> p.GetProperty("difficulty").GetProperty("reasoning").GetProperty("trialSteps").GetInt32()) |> Seq.toArray
        Assert.Equal<int array>(Array.sort trials, trials)
        let selection = document.RootElement.GetProperty("generation").GetProperty("selection")
        Assert.Equal(800, selection.GetProperty("batchCandidateBudget").GetInt32())
        Assert.InRange(selection.GetProperty("batchCandidatesConsidered").GetInt32(),1,800)
    finally
        if File.Exists first then File.Delete first
        if File.Exists second then File.Delete second

[<Fact>]
let ``duplicate exhaustion and failed selections preserve atomic output`` () =
    let path = Path.Combine(Path.GetTempPath(), $"tangles-exhaust-{Guid.NewGuid():N}.json")
    let common = [|"--mode";"full-coverage";"--width";"1";"--height";"4";"--pairs";"2";"--min-length";"2";"--max-length";"2";"--candidates";"2";"--output";path|]
    try
        Assert.Equal(1, Tangles.Generator.Program.main (Array.append common [|"--assess";"--count";"2"|]))
        Assert.False(File.Exists path)
        File.WriteAllText(path,"keep this")
        Assert.Equal(1, Tangles.Generator.Program.main (Array.append common [|"--difficulty";"hard";"--force"|]))
        Assert.Equal("keep this", File.ReadAllText path)
    finally
        if File.Exists path then File.Delete path

[<Fact>]
let ``guided selection honours cancellation`` () =
    use cancellation = new CancellationTokenSource()
    cancellation.Cancel()
    Assert.Throws<OperationCanceledException>(fun () ->
        GuidedGeneration.generate cancellation.Token { Generation.defaults 42 with Mode = FullCoverage }
            { GenerationPolicy.defaults with Target = Hard } |> ignore) |> ignore

[<Fact>]
let ``trial floors are enforced and exported without changing the game rules`` () =
    let options = { Generation.defaults 42 with Mode = FullCoverage }
    let policy = { GenerationPolicy.defaults with Target = Hard; MinTrials = 25 }
    let puzzle = GuidedGeneration.generate CancellationToken.None options policy |> require
    Assert.True(puzzle.Quality.Value.Reasoning.Value.TrialSteps >= 25)
    let pack = Export.createPack options [|puzzle|]
    Assert.Equal("solver-guided-hamiltonian-partition-v6",pack.Generation.Algorithm)
    Assert.Equal(25,pack.Generation.Selection.Value.MinTrials)
    Assert.Equal(25,pack.Puzzles[0].Selection.Value.MinTrials)
    Assert.Equal(puzzle.Puzzle,(Export.toJson pack |> PuzzlePacks.read |> require).Puzzles[0].Puzzle)
    let impossible = { policy with MinTrials = 1_000_000; MaxCandidates = 2; ImproveSteps = 0 }
    Assert.True(GuidedGeneration.generate CancellationToken.None options impossible |> Result.isError)

[<Fact>]
let ``expert floors select and export first-answer depth and effort`` () =
    let options = { Generation.defaults 42 with Mode = FullCoverage; PairCount = 4; MaxPathLength = 32 }
    let policy = { GenerationPolicy.defaults with Target = Expert; MinBranchDepth = 1; MinFirstSolutionNodes = 1 }
    let first = GuidedGeneration.generate CancellationToken.None options policy |> require
    let second = GuidedGeneration.generate CancellationToken.None options policy |> require
    Assert.Equal(first,second)
    let report = first.Quality.Value.Reasoning.Value
    Assert.Equal("expert",report.Grade)
    Assert.True(report.FirstSolutionBranchDepth >= 1)
    Assert.True(report.FirstSolutionSearchNodes >= 1)
    Assert.Equal("unique",report.Uniqueness)
    let pack = Export.createPack options [|first|]
    Assert.Equal(1,pack.Generation.Selection.Value.MinBranchDepth)
    Assert.Equal(1,pack.Generation.Selection.Value.MinFirstSolutionNodes)
    Assert.Equal("solver-guided-hamiltonian-partition-v6",pack.Generation.Algorithm)

[<Fact>]
let ``CLI validates reasoning floors and restricts depth floors to expert`` () =
    match CommandLine.parse 42 [|"--mode";"full-coverage";"--difficulty";"expert";"--min-depth";"6";"--min-search-nodes";"128";"--min-trials";"25"|] |> require with
    | Generate options ->
        Assert.Equal(6,options.Selection.MinBranchDepth)
        Assert.Equal(128,options.Selection.MinFirstSolutionNodes)
        Assert.Equal(25,options.Selection.MinTrials)
    | _ -> failwith "Expected generation."
    for args in
        [ [|"--min-trials";"1"|]
          [|"--mode";"full-coverage";"--difficulty";"medium";"--min-trials";"1"|]
          [|"--mode";"full-coverage";"--difficulty";"hard";"--min-depth";"1"|]
          [|"--mode";"full-coverage";"--difficulty";"hard";"--min-search-nodes";"1"|]
          [|"--min-trials";"-1"|]
          [|"--min-depth";"257"|]
          [|"--min-search-nodes";"20001"|] ] do
        Assert.True(CommandLine.parse 42 args |> Result.isError)
