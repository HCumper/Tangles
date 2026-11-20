module Tangles.GenerationRegressionTests

open System
open System.IO
open System.Text.Json
open System.Threading
open Tangles.Core
open Tangles.Generator
open Xunit

let private require = function Ok value -> value | Error message -> failwith message

[<Theory>]
[<InlineData(-1)>]
[<InlineData(-42)>]
[<InlineData(Int32.MinValue)>]
let ``negative seeds are rejected by CLI fast and selected generation`` seed =
    Assert.True(CommandLine.parse 0 [|"--seed"; string seed|] |> Result.isError)
    for mode in [ConnectOnly; FullCoverage] do
        let options = { Generation.defaults seed with Mode = mode }
        for result in
            [ Generation.generate CancellationToken.None options
              GuidedGeneration.generate CancellationToken.None options { GenerationPolicy.defaults with Assess = true } ] do
            match result with
            | Ok _ -> failwith "Negative seeds must not silently alias positive seeds."
            | Error message -> Assert.Contains("Seed must be between 0 and 2147483647", message)

[<Theory>]
[<InlineData(0)>]
[<InlineData(Int32.MaxValue)>]
let ``nonnegative seed boundaries remain valid`` seed =
    Assert.Empty(Generation.validateOptions (Generation.defaults seed))

[<Fact>]
let ``batch requests do not replay the master's retry prefix`` () =
    let path = Path.Combine(Path.GetTempPath(), "tangles-stream-" + Guid.NewGuid().ToString("N") + ".json")
    let requests = ResizeArray<int>()
    let candidates = ResizeArray<int>()
    let generate cancellation (options: GenerationOptions) policy =
        requests.Add options.Seed
        let mutable calls = 0
        GuidedGeneration.generateWith (fun token child ->
            candidates.Add child.Seed
            calls <- calls + 1
            if calls < 3 then
                Error (GenerationFailure.create SearchExhausted "Deliberate unsuccessful proposal." { GenerationWork.zero with BoardAttempts = 1 })
            else Generation.generateResult token child) cancellation options policy
    try
        let args = [|"--seed";"42";"--count";"3";"--assess";"--candidates";"4";"--output";path|]
        Assert.Equal(0, Program.mainWith generate args)
        Assert.Equal(3, requests.Count)
        Assert.Equal(9, candidates.Count)
        let firstRetries = candidates |> Seq.take 3 |> Set.ofSeq
        Assert.All(requests |> Seq.skip 1, fun seed -> Assert.False(Set.contains seed firstRetries))
        Assert.Equal<int array>([|for i in 0..2 -> SeedStreams.batchSeed 42 i|], requests.ToArray())
        use json = JsonDocument.Parse(File.ReadAllText path)
        Assert.Equal("splitmix64-batch-v1", json.RootElement.GetProperty("generation").GetProperty("batchSeedAlgorithm").GetString())
        let puzzles = json.RootElement.GetProperty("puzzles").EnumerateArray() |> Seq.toArray
        Assert.All(puzzles, fun puzzle -> Assert.True(puzzle.GetProperty("work").GetProperty("boardAttempts").GetInt32() >= 3))
    finally
        if File.Exists path then File.Delete path

[<Fact>]
let ``an internal candidate failure is fatal without relying on its text`` () =
    let mutable calls = 0
    let expected = GenerationFailure.create InternalFailure "Witness invariant failed." GenerationWork.zero
    let generator _ _ = calls <- calls + 1; Error expected
    let result = GuidedGeneration.generateWith generator CancellationToken.None (Generation.defaults 42)
                    { GenerationPolicy.defaults with Assess = true; MaxCandidates = 10 }
    Assert.Equal(1, calls)
    match result with
    | Error actual -> Assert.Equal(expected, actual)
    | Ok _ -> failwith "An internal failure cannot be retried as an ordinary rejected candidate."

[<Fact>]
let ``malformed generated witnesses return classified internal failures`` () =
    let options = { Generation.defaults 42 with PairCount = 1; MinPathLength = 2 }
    for paths in [ [|[||]|]; [|[|0;1;8|]|] ] do
        match Generation.fromPaths options 1 GenerationWork.zero paths with
        | Error failure -> Assert.Equal(InternalFailure, failure.Kind)
        | Ok _ -> failwith "The malformed witness must not be published."

[<Fact>]
let ``CLI invariant failures return one and preserve existing output`` () =
    let path = Path.Combine(Path.GetTempPath(), "tangles-internal-" + Guid.NewGuid().ToString("N") + ".json")
    try
        File.WriteAllText(path, "preserve this pack")
        let args = [|"--seed";"42";"--assess";"--output";path;"--force"|]
        let failure = GenerationFailure.create InternalFailure "Witness invariant failed." GenerationWork.zero
        Assert.Equal(1, Program.mainWith (fun _ _ _ -> Error failure) args)
        Assert.Equal("preserve this pack", File.ReadAllText path)
        Assert.Equal(1, Program.mainWith (fun _ _ _ -> invalidOp "Unexpected invariant failure.") args)
        Assert.Equal("preserve this pack", File.ReadAllText path)
    finally
        if File.Exists path then File.Delete path

let private tightOptions =
    { Generation.defaults 42 with
        Mode = FullCoverage; PairCount = 8; MinPathLength = 8; MaxPathLength = 8; MaxAttempts = 1 }

[<Fact>]
let ``structured proposals rescue exact lengths and still meet the target`` () =
    let policy = { GenerationPolicy.defaults with Target = Easy; MaxCandidates = 1; ImproveSteps = 0 }
    let generated = GuidedGeneration.generate CancellationToken.None tightOptions policy |> require
    Assert.Equal("easy", generated.Quality.Value.Reasoning.Value.Grade)
    Assert.Equal(1, generated.Quality.Value.Selection.SolverAssessments)
    Assert.Equal(1, generated.Work.StructuredStarts)
    Assert.Equal(1, generated.Work.TraversalEvaluations)
    Assert.All(generated.Solution, fun path -> Assert.Equal(8, path.Cells.Length))
    Assert.Empty(Validation.solution generated.Puzzle generated.Solution)

[<Fact>]
let ``structured witnesses never silently substitute an easier target`` () =
    let policy = { GenerationPolicy.defaults with Target = Hard; MaxCandidates = 1; ImproveSteps = 0 }
    match GuidedGeneration.generate CancellationToken.None tightOptions policy with
    | Ok _ -> failwith "These straight strips cannot satisfy the hard target."
    | Error message ->
        Assert.Contains("1 solver assessments", message)
        Assert.Contains("Valid layouts did not meet the policy", message)
        Assert.Contains("No easier puzzle was substituted", message)

[<Fact>]
let ``layout and solver exhaustion have different diagnostics`` () =
    let impossible = { tightOptions with PairCount = 1; Width = 2; Height = 2; MinPathLength = 4; MaxPathLength = 4 }
    let noLayout _ _ = Error (GenerationFailure.create SearchExhausted "No witness." GenerationWork.zero)
    match GuidedGeneration.generateWith noLayout CancellationToken.None (Generation.defaults 42)
            { GenerationPolicy.defaults with Assess = true; MaxCandidates = 1 } with
    | Error failure -> Assert.Contains("No valid layout found", failure.Message)
    | Ok _ -> failwith "No layout was supplied."
    Assert.True(Generation.generate CancellationToken.None impossible |> Result.isError)
    let limited = { GenerationPolicy.defaults with Target = Hard; MaxCandidates = 1; SolverWork = 1 }
    match GuidedGeneration.generate CancellationToken.None tightOptions limited with
    | Error message -> Assert.Contains("All solver assessments reached a limit", message)
    | Ok _ -> failwith "A one-unit solver budget cannot establish hardness."

[<Fact>]
let ``fast fallback actually runs within its evaluation bound`` () =
    let generated = Generation.generate CancellationToken.None { tightOptions with MaxAttempts = 128 } |> require
    Assert.True(generated.Work.StructuredStarts > 0)
    Assert.InRange(generated.Work.TraversalEvaluations, 1, 128)
    Assert.Empty(Validation.solution generated.Puzzle generated.Solution)

[<Fact>]
let ``work counters separate board attempts from traversal refinement`` () =
    let connected = Generation.generate CancellationToken.None (Generation.defaults 42) |> require
    Assert.True(connected.Work.BoardAttempts > 0)
    Assert.Equal(0, connected.Work.TraversalEvaluations)
    let options =
        { Generation.defaults 42 with
            Mode = FullCoverage; Width = 4; Height = 4
            PairCount = 8; MinPathLength = 2; MaxPathLength = 2; MaxAttempts = 10 }
    let covered = Generation.generate CancellationToken.None options |> require
    Assert.Equal(0, covered.Work.BoardAttempts)
    Assert.Equal(10, covered.Work.TraversalEvaluations)
    Assert.Equal(9, covered.Work.RefinementEvaluations)
    let prepared = Export.prepareWithCancellation CancellationToken.None covered
    let pack = Export.createPreparedPackWithCancellation CancellationToken.None options [|prepared|]
    Assert.Equal(covered.Work, pack.Puzzles[0].Work)
    Assert.Same(prepared.Difficulty, pack.Puzzles[0].Difficulty)
    Assert.Equal(covered.Puzzle, (Export.toJson pack |> PuzzlePacks.read |> require).Puzzles[0].Puzzle)

[<Fact>]
let ``one dimensional full coverage skips ineffective refinement`` () =
    let options =
        { Generation.defaults 42 with
            Mode = FullCoverage; Width = 1; Height = 64
            PairCount = 32; MinPathLength = 2; MaxPathLength = 2 }
    let generated = Generation.generate CancellationToken.None options |> require
    Assert.Equal(1, generated.Work.TraversalEvaluations)
    Assert.Equal(0, generated.Work.RefinementEvaluations)
    Assert.Contains("27", generated.Puzzle.Pairs |> List.map _.Label)
    Assert.Empty(Validation.solution generated.Puzzle generated.Solution)

[<Fact>]
let ``maximum board and pair count produce independently validated numeric labels`` () =
    let options =
        { Generation.defaults 42 with
            Mode = FullCoverage; Width = 64; Height = 64
            PairCount = 2048; MinPathLength = 2; MaxPathLength = 2; MaxAttempts = 2 }
    let generated = Generation.generate CancellationToken.None options |> require
    Assert.Equal(2048, generated.Puzzle.Pairs.Length)
    Assert.Equal("2048", generated.Puzzle.Pairs |> List.last |> _.Label)
    Assert.Equal(4096, generated.Solution |> List.sumBy _.Cells.Length)
    Assert.Empty(Validation.solution generated.Puzzle generated.Solution)

[<Fact>]
let ``connect only growth reuses its workspace across numeric labelled paths`` () =
    let options =
        { Generation.defaults 42 with
            Width = 32; Height = 4; PairCount = 27; MinPathLength = 2; MaxPathLength = 2 }
    let generated = Generation.generate CancellationToken.None options |> require
    Assert.Equal("27", generated.Puzzle.Pairs |> List.last |> _.Label)
    Assert.All(generated.Solution, fun path -> Assert.Equal(2, path.Cells.Length))
    Assert.Empty(Validation.solution generated.Puzzle generated.Solution)
