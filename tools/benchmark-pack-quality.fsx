// Run with --reference to the built Tangles.Core and Tangles.Generator DLLs.
// Output is diagnostic data; witnesses are measured separately from solver grades.
open System
open System.IO
open System.Diagnostics
open System.Text.Json
open System.Threading
open Tangles.Core
open Tangles.Generator

let output = Path.GetFullPath(fsi.CommandLineArgs[1])
Directory.CreateDirectory output |> ignore
let count = fsi.CommandLineArgs |> Array.skip 2 |> Array.tryPick (fun value -> match Int32.TryParse value with true,n -> Some n | _ -> None) |> Option.defaultValue 150
let doCuts = not (fsi.CommandLineArgs |> Array.contains "--recipes-only")
let save name data = File.WriteAllText(Path.Combine(output,name),JsonSerializer.Serialize(data,JsonSerializerOptions(WriteIndented=true)))

type CutRow =
    { Style: string; Seed: int; Generated: bool; Error: string
      GenerationMs: float; Evaluations: int; Grade: string; Uniqueness: string; Status: string
      Turns: int; Paths: int; StraightPaths: int; BorderEndpoints: int; LengthDeviation: float }

let measure style seed =
    let watch = Stopwatch.StartNew()
    let result = Generation.generate CancellationToken.None { Generation.defaults seed with Mode=FullCoverage; CutStyle=style }
    let elapsed = watch.Elapsed.TotalMilliseconds
    match result with
    | Error error ->
        { Style=CutStyle.name style; Seed=seed; Generated=false; Error=error; GenerationMs=elapsed
          Evaluations=500; Grade="notGenerated"; Uniqueness="notEstablished"; Status="notGenerated"
          Turns=0; Paths=0; StraightPaths=0; BorderEndpoints=0; LengthDeviation=0.0 }
    | Ok generated ->
        let turns path =
            path.Cells |> List.windowed 3 |> List.sumBy (fun triple ->
                let a,b,c=triple[0],triple[1],triple[2]
                if b.X-a.X <> c.X-b.X || b.Y-a.Y <> c.Y-b.Y then 1 else 0)
        let bends = generated.Solution |> List.map turns
        let lengths = generated.Solution |> List.map (fun path -> float path.Cells.Length)
        let average = List.average lengths
        let boundary cell = cell.X=0 || cell.Y=0 || cell.X=7 || cell.Y=7
        let analysis = Solver.analyse CancellationToken.None Solver.defaults generated.Puzzle
        { Style=CutStyle.name style; Seed=seed; Generated=true; Error=""; GenerationMs=elapsed
          Evaluations=generated.Work.TraversalEvaluations; Grade=analysis.Grade; Uniqueness=analysis.Uniqueness; Status=analysis.Status
          Turns=List.sum bends; Paths=bends.Length; StraightPaths=bends |> List.filter ((=)0) |> List.length
          BorderEndpoints=generated.Puzzle.Pairs |> List.sumBy (fun pair -> (if boundary pair.Start then 1 else 0)+(if boundary pair.Finish then 1 else 0))
          LengthDeviation=lengths |> List.averageBy (fun length -> abs(length-average)) }

// Warm both paths; alternate timing order to reduce systematic warmup bias.
if doCuts then
    for style in [RandomCuts;VariedCuts] do measure style 1000 |> ignore
let rows =
    [|for seed in 0..(if doCuts then count-1 else -1) do
        for style in (if seed%2=0 then [RandomCuts;VariedCuts] else [VariedCuts;RandomCuts]) do
            yield measure style seed|]
if doCuts then save "cut-rows.json" rows
type CutSummary =
    { Style: string; Requested: int; Generated: int; MeanGenerationMs: float; MeanEvaluations: float
      TurnsPerPath: float; StraightPathPercent: float; BorderEndpointPercent: float
      MeanLengthDeviation: float; Grades: Map<string,int>; Uniqueness: Map<string,int>; Status: Map<string,int> }
let summaries =
    [|for style in (if doCuts then ["random";"varied"] else []) do
        let selected = rows |> Array.filter (fun row -> row.Style=style)
        let valid = selected |> Array.filter _.Generated
        let paths = valid |> Array.sumBy _.Paths
        let counts get = selected |> Array.countBy get |> Map.ofArray
        yield { Style=style; Requested=count; Generated=valid.Length
                MeanGenerationMs=selected |> Array.averageBy _.GenerationMs
                MeanEvaluations=selected |> Array.averageBy (fun row -> float row.Evaluations)
                TurnsPerPath=float (valid |> Array.sumBy _.Turns) / float paths
                StraightPathPercent=100.0 * float (valid |> Array.sumBy _.StraightPaths) / float paths
                BorderEndpointPercent=100.0 * float (valid |> Array.sumBy _.BorderEndpoints) / float (2*paths)
                MeanLengthDeviation=valid |> Array.averageBy _.LengthDeviation
                Grades=counts _.Grade; Uniqueness=counts _.Uniqueness; Status=counts _.Status }|]
if doCuts then save "cut-summary.json" summaries
for summary in summaries do printfn "CUTS %s: %.2f turns/path; %.1f%% straight; %.2f ms; grades %A" summary.Style summary.TurnsPerPath summary.StraightPathPercent summary.MeanGenerationMs summary.Grades

type RecipeRow =
    { Target: string; Width: int; Pairs: int; Seed: int; Selected: bool; ElapsedMs: float; Error: string; Trials: int; Candidates: int }
let recipeRows = ResizeArray<RecipeRow>()
let profiles =
    if fsi.CommandLineArgs |> Array.contains "--full-recipes" then
        [ Medium,8,[8;9],[0;1;2;3;4;5;6;7;8;42],100,500,2_000_000
          Medium,12,[12;14;16;18;20;24],[0;1;42],100,5000,20_000_000
          Medium,16,[20;24;28;32;36],[0;42],100,10000,100_000_000
          Hard,8,[6;7;8],[0;1;42],100,500,2_000_000
          Hard,12,[16;18;20],[0;1;42],100,5000,20_000_000
          Hard,16,[30;32;34],[0;42],100,10000,100_000_000 ]
    else
        [ Medium,8,[6;7;8;9;10],[0;1;2;3;4;5;6;7;8;42],30,500,2_000_000
          Medium,12,[10;12;14;16],[0;1;42],15,5000,20_000_000
          Medium,16,[14;16;18],[42],5,10000,100_000_000 ]
for target,size,pairsList,seeds,candidates,attempts,work in profiles do
    for pairs in pairsList do
        for seed in seeds do
            let options = { Generation.defaults seed with Mode=FullCoverage; Width=size; Height=size; PairCount=pairs; MaxPathLength=2*size; MaxAttempts=attempts }
            let policy = { GenerationPolicy.defaults with Target=target; MaxCandidates=candidates; ImproveSteps=0; SolverWork=work }
            let timer = Stopwatch.StartNew()
            let result = GuidedGeneration.generate CancellationToken.None options policy
            let elapsed = timer.Elapsed.TotalMilliseconds
            let row =
                match result with
                | Ok generated ->
                    { Target=GenerationPolicy.targetName target; Width=size; Pairs=pairs; Seed=seed; Selected=true; ElapsedMs=elapsed; Error=""
                      Trials=generated.Quality.Value.Reasoning.Value.TrialSteps; Candidates=generated.Quality.Value.Selection.CandidatesConsidered }
                | Error error -> { Target=GenerationPolicy.targetName target; Width=size; Pairs=pairs; Seed=seed; Selected=false; ElapsedMs=elapsed; Error=error; Trials=0; Candidates=candidates }
            recipeRows.Add row
        save (if fsi.CommandLineArgs |> Array.contains "--full-recipes" then "recipe-survey-full.json" else "recipe-survey.json") (recipeRows.ToArray())
        let group = recipeRows |> Seq.filter (fun row -> row.Width=size && row.Pairs=pairs && row.Target=GenerationPolicy.targetName target) |> Seq.toArray
        printfn "RECIPE %s %dx%d/%d pairs: %d/%d selected, %.1f ms mean" (GenerationPolicy.targetName target) size size pairs (group |> Array.filter _.Selected |> Array.length) group.Length (group |> Array.averageBy _.ElapsedMs)
