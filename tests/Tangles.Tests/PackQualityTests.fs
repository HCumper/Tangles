module Tangles.PackQualityTests

open System
open System.IO
open System.Text.Json
open System.Threading
open Tangles.Core
open Tangles.Generator
open Xunit

let private require = function Ok value -> value | Error message -> failwith message
let private cell x y = { X=x; Y=y }
let private transform f (puzzle: Puzzle) =
    { puzzle with Pairs=puzzle.Pairs |> List.map (fun pair -> { pair with Start=f pair.Start; Finish=f pair.Finish }) }

[<Fact>]
let ``symmetry keys ignore rotations reflections names and pair direction without changing assessment identity`` () =
    let board =
        { Width=4; Height=4; RequireFullCoverage=true
          Pairs=[{Label="A";Start=cell 0 0;Finish=cell 1 2};{Label="B";Start=cell 3 0;Finish=cell 2 3}] }
    let rotated = transform (fun c -> cell (3-c.Y) c.X) board
    let reflected = transform (fun c -> cell (3-c.X) c.Y) board
    let renamed = { rotated with Pairs=rotated.Pairs |> List.rev |> List.mapi (fun i pair -> {pair with Label=string i;Start=pair.Finish;Finish=pair.Start}) }
    Assert.Equal(PuzzleSymmetry.key board,PuzzleSymmetry.key rotated)
    Assert.Equal(PuzzleSymmetry.key board,PuzzleSymmetry.key reflected)
    Assert.Equal(PuzzleSymmetry.key board,PuzzleSymmetry.key renamed)
    Assert.NotEqual<string>(Solver.boardKey board,Solver.boardKey rotated)
    Assert.NotEqual<string>(PuzzleSymmetry.key board,PuzzleSymmetry.key {board with RequireFullCoverage=false})

[<Fact>]
let ``rectangular symmetry preserves dimensions`` () =
    let board = { Width=3;Height=4;RequireFullCoverage=false;Pairs=[{Label="A";Start=cell 0 0;Finish=cell 1 2}] }
    Assert.Equal(PuzzleSymmetry.key board, PuzzleSymmetry.key (transform (fun c -> cell (2-c.X) (3-c.Y)) board))
    let rotated = { (transform (fun c -> cell (3-c.Y) c.X) board) with Width=4;Height=3 }
    Assert.NotEqual<string>(PuzzleSymmetry.key board, PuzzleSymmetry.key rotated)

[<Fact>]
let ``fast batches reject rotated copies and export bounded retry counts`` () =
    let path=Path.Combine(Path.GetTempPath(),"tangles-symmetry-"+Guid.NewGuid().ToString("N")+".json")
    let mutable calls=0
    let generate _ (options: GenerationOptions) _ =
        calls <- calls+1
        let cells = match calls with 1 -> [|0;1|] | 2 -> [|2;5|] | _ -> [|0;1;2|]
        Generation.fromPaths options 1 {GenerationWork.zero with BoardAttempts=1} [|cells|]
    try
        Assert.Equal(0,Program.mainWith generate [|"--seed";"42";"--width";"3";"--height";"3";"--pairs";"1";"--min-length";"2";"--max-length";"3";"--count";"2";"--output";path|])
        Assert.Equal(3,calls)
        use document=JsonDocument.Parse(File.ReadAllText path)
        Assert.Equal(1,document.RootElement.GetProperty("generation").GetProperty("batch").GetProperty("rejectedDuplicates").GetInt32())
        let loaded=File.ReadAllText path |> PuzzlePacks.read |> require
        Assert.Equal(2,loaded.Puzzles |> Array.map (fun p -> PuzzleSymmetry.key p.Puzzle) |> Array.distinct |> Array.length)
    finally
        if File.Exists path then File.Delete path

[<Fact>]
let ``selection skips a rotated ambiguous candidate without reusing its solver assessment`` () =
    let mutable calls=0
    let generate _ options =
        calls <- calls+1
        let cells=match calls with 1 -> [|0;1;2|] | 2 -> [|2;5;8|] | _ -> [|0;1|]
        Generation.fromPaths options 1 {GenerationWork.zero with BoardAttempts=1} [|cells|]
    let options={Generation.defaults 42 with Width=3;Height=3;PairCount=1;MinPathLength=2;MaxPathLength=5}
    let result=GuidedGeneration.generateWith generate CancellationToken.None options {GenerationPolicy.defaults with RequireUnique=true;MaxCandidates=3}
    match result with
    | Error failure -> failwith failure.Message
    | Ok generated ->
        Assert.Equal(1,generated.Quality.Value.Selection.DuplicatesRejected)
        Assert.Equal(2,generated.Quality.Value.Selection.SolverAssessments)
        Assert.Equal(Solver.boardKey generated.Puzzle,generated.Quality.Value.Reasoning.Value.BoardKey)

[<Fact>]
let ``optional effort band is satisfied and retained in JSON`` () =
    let options={Generation.defaults 42 with Mode=FullCoverage}
    let policy={GenerationPolicy.defaults with Target=Hard;MinTrials=5;MaxTrials=Some 20;ImproveSteps=0}
    let generated=GuidedGeneration.generate CancellationToken.None options policy |> require
    Assert.InRange(generated.Quality.Value.Reasoning.Value.TrialSteps,5,20)
    Assert.Equal("hard",generated.Quality.Value.Reasoning.Value.Grade)
    let pack=Export.createPack options [|generated|]
    Assert.Equal(Some 20,pack.Generation.Selection.Value.MaxTrials)
    Assert.Equal(GenerationPolicy.version,pack.Puzzles[0].Selection.Value.PolicyVersion)
    use json=JsonDocument.Parse(Export.toJson pack)
    Assert.Equal(20,json.RootElement.GetProperty("generation").GetProperty("selection").GetProperty("maxTrials").GetInt32())
    Assert.True(GenerationPolicy.defaults.MaxTrials.IsNone)

[<Fact>]
let ``effort limits never turn an unmatched candidate into an accepted hard puzzle`` () =
    let options={Generation.defaults 42 with Mode=FullCoverage}
    let policy={GenerationPolicy.defaults with Target=Hard;MaxTrials=Some 0;MaxCandidates=3;ImproveSteps=0}
    Assert.True(GuidedGeneration.generate CancellationToken.None options policy |> Result.isError)

[<Fact>]
let ``new CLI options validate effort bands cut styles and ramp conflicts`` () =
    for args in
        [ [|"--mode";"full-coverage";"--difficulty";"hard";"--max-trials";"-1"|]
          [|"--mode";"full-coverage";"--difficulty";"hard";"--min-trials";"20";"--max-trials";"10"|]
          [|"--max-trials";"10"|]
          [|"--cut-style";"varied"|]
          [|"--mode";"full-coverage";"--cut-style";"unknown"|]
          [|"--ramp";"easy..hard"|]
          [|"--mode";"full-coverage";"--ramp";"hard..easy"|]
          [|"--mode";"full-coverage";"--ramp";"easy..hard";"--count";"2"|]
          [|"--mode";"full-coverage";"--ramp";"easy..hard";"--pairs";"8"|]
          [|"--mode";"full-coverage";"--ramp";"easy..hard";"--difficulty";"hard"|] ] do
        Assert.True(CommandLine.parse 42 args |> Result.isError, String.concat " " args)
    match CommandLine.parse 42 [|"--mode";"full-coverage";"--ramp";"easy..hard";"--max-trials";"20"|] |> require with
    | Generate options ->
        Assert.Equal(12,options.Count)
        Assert.Equal(Some 20,options.Selection.MaxTrials)
    | _ -> failwith "Expected a ramp command."

[<Theory>]
[<InlineData(0)>]
[<InlineData(1)>]
[<InlineData(42)>]
let ``varied cuts preserve rules length bounds and replay`` seed =
    let options={Generation.defaults seed with Mode=FullCoverage;CutStyle=VariedCuts}
    let first=Generation.generate CancellationToken.None options |> require
    Assert.Equal(first,Generation.generate CancellationToken.None options |> require)
    Assert.Empty(Validation.solution first.Puzzle first.Solution)
    Assert.All(first.Solution,fun path -> Assert.InRange(path.Cells.Length,4,16))
    let pack=Export.createPack options [|first|]
    Assert.Equal("varied",pack.Generation.CutStyle)
    Assert.Equal("hamiltonian-path-partition-varied-v1",pack.Generation.Algorithm)
    Assert.Equal(first.Puzzle,(Export.toJson pack |> PuzzlePacks.read |> require).Puzzles[0].Puzzle)

[<Fact>]
let ``ramp export uses achieved grades records recipes and replays byte for byte`` () =
    let first=Path.Combine(Path.GetTempPath(),"tangles-ramp-"+Guid.NewGuid().ToString("N")+".json")
    let second=Path.Combine(Path.GetTempPath(),"tangles-ramp-"+Guid.NewGuid().ToString("N")+".json")
    try
        for path in [first;second] do
            Assert.Equal(0,Program.main [|"--mode";"full-coverage";"--ramp";"easy..hard";"--count";"6";"--seed";"42";"--improve-steps";"0";"--output";path|])
        Assert.Equal(File.ReadAllText first,File.ReadAllText second)
        use json=JsonDocument.Parse(File.ReadAllText first)
        let generation=json.RootElement.GetProperty("generation")
        Assert.Equal(JsonValueKind.Null,generation.GetProperty("pairCount").ValueKind)
        Assert.Equal("easy..hard",generation.GetProperty("ramp").GetString())
        let puzzles=json.RootElement.GetProperty("puzzles").EnumerateArray() |> Seq.toArray
        let grades=puzzles |> Array.map (fun p -> p.GetProperty("difficulty").GetProperty("grade").GetString())
        Assert.Equal<string array>([|"easy";"easy";"medium";"medium";"hard";"hard"|],grades)
        Assert.Equal(6,File.ReadAllText first |> PuzzlePacks.read |> require |> _.Puzzles.Length)
        Assert.All(puzzles,fun p -> Assert.Equal(p.GetProperty("pairs").GetArrayLength(),p.GetProperty("recipe").GetProperty("pairCount").GetInt32()))
        Assert.Equal(6,puzzles |> Array.map (fun p -> p.GetProperty("id").GetString()) |> Array.distinct |> Array.length)
    finally
        if File.Exists first then File.Delete first
        if File.Exists second then File.Delete second

[<Fact>]
let ``ramp recipes use surveyed densities and respect explicit budgets`` () =
    let spec=Ramp.parse "easy..hard" |> require
    for size,medium,hard in [8,9,8;12,20,18;16,36,32] do
        let options={Generation.defaults 42 with Mode=FullCoverage;Width=size;Height=size}
        let sections=Ramp.plan options GenerationPolicy.defaults 6 spec |> require
        Assert.Equal(medium,sections[1].Generation.PairCount)
        Assert.Equal(hard,sections[2].Generation.PairCount)
    match CommandLine.parse 42 [|"--mode";"full-coverage";"--ramp";"easy..hard";"--max-attempts";"123";"--solver-work";"4567";"--max-length";"40"|] |> require with
    | Generate options ->
        let sections=Ramp.plan options.Generation options.Selection options.Count options.Ramp.Value |> require
        Assert.All(sections,fun section ->
            Assert.Equal(123,section.Generation.MaxAttempts)
            Assert.Equal(4567,section.Policy.SolverWork)
            Assert.Equal(40,section.Generation.MaxPathLength))
    | _ -> failwith "Expected a ramp."

[<Fact>]
let ``ramp export refuses a mislabeled achieved grade`` () =
    let options={Generation.defaults 42 with Mode=FullCoverage}
    let spec=Ramp.parse "easy..medium" |> require
    let sections=Ramp.plan options GenerationPolicy.defaults 2 spec |> require
    let easy=GuidedGeneration.generate CancellationToken.None sections[0].Generation {sections[0].Policy with ImproveSteps=0} |> require
    let prepared=Export.prepareWithCancellation CancellationToken.None easy
    Assert.Throws<InvalidOperationException>(fun () ->
        Export.createRampPackWithCancellation CancellationToken.None options spec sections
            [|sections[0].Generation,prepared; sections[1].Generation,prepared|] |> ignore) |> ignore
