// Run with references to Release Tangles.Core.dll and Tangles.Generator.dll.
// Arguments: mode tier count output-directory work-directory
// Example: connect-only hard 100 C:/Source/Tangles/puzzles <scratch>/recalibration
// Stored solver/geometry grades remain unchanged. Collection tiers record the
// separate, provisional calibration prompted by player feedback.
open System
open System.IO
open System.Text
open System.Text.Json
open System.Text.Json.Nodes
open System.Threading
open System.Diagnostics
open System.Collections.Generic
open Tangles.Core
open Tangles.Generator

let args = fsi.CommandLineArgs |> Array.skip 1
if args.Length <> 5 then failwith "Expected: mode tier count output-directory work-directory"
let mode, tier = args[0], args[1]
if not (List.contains mode ["connect-only";"full-coverage"]) || not (List.contains tier ["easy";"medium";"hard"]) then
    failwith "Expected a supported coverage mode and easy, medium or hard."
let count = Int32.Parse args[2]
if count < 1 || count > 1000 then failwith "Count must be between 1 and 1000."
let output = Path.GetFullPath args[3]
let work = Path.Combine(Path.GetFullPath args[4], $"{mode}-{tier}")
Directory.CreateDirectory output |> ignore
Directory.CreateDirectory work |> ignore
let destination = Path.Combine(output, $"{mode}-12x12-{tier}-{count}-20261120-v2.json")
if File.Exists destination then failwith $"Preserving existing pack: {destination}"
let seed = 202611300 + (if mode="full-coverage" then 10 else 0) + (match tier with "easy"->1 | "medium"->2 | _->3)
let pairs, minimum, maximum =
    match mode,tier with
    | "connect-only","easy" -> 8,6,24
    | "connect-only",_ -> 10,8,24
    | _,"easy" -> 12,4,48
    | _ -> 8,3,48
let generation =
    { Generation.defaults seed with Mode=(if mode="full-coverage" then FullCoverage else ConnectOnly)
                                    Width=12;Height=12;PairCount=pairs;MinPathLength=minimum;MaxPathLength=maximum
                                    MaxAttempts=(if mode="connect-only" then 1000 elif tier="easy" then 5000 else 10000) }
let policy =
    if tier="easy" then
        {GenerationPolicy.defaults with Target=Hard;MinTrials=1;MaxCandidates=60;ImproveSteps=0;SolverWork=20000000}
    else
        {GenerationPolicy.defaults with Target=Expert;MinBranchDepth=(if tier="hard" then 8 else 2)
                                        MinFirstSolutionNodes=(if tier="hard" then 512 else 8)
                                        MaxCandidates=40;ImproveSteps=0;SolverWork=100000000}
let screenLimit, screenLower = match tier with "easy"->256,32 | "medium"->2048,256 | _->8192,8192
let jsonOptions = JsonSerializerOptions(PropertyNamingPolicy=JsonNamingPolicy.CamelCase,WriteIndented=true)
let save path (node:JsonNode) =
    let temporary = path + ".tmp"
    File.WriteAllText(temporary,node.ToJsonString(jsonOptions),UTF8Encoding(false))
    File.Move(temporary,path,true)
let stateFile = Path.Combine(work,"state.json")
let previous = if File.Exists stateFile then JsonNode.Parse(File.ReadAllText stateFile) else null
let previousInt (name:string) = if isNull previous then 0 else previous[name].GetValue<int>()
let mutable requests, failures, rejections, duplicates = previousInt "requests",previousInt "failures",previousInt "rejections",previousInt "duplicates"
let mutable generatorSeconds = if isNull previous then 0. else previous["generatorSeconds"].GetValue<float>()
let seen=HashSet<string>()
// Preserve earlier collections and avoid serving rotated/reflected repeats.
for path in Directory.GetFiles(output,"*.json") do
    match PuzzlePacks.read(File.ReadAllText path) with
    | Ok pack -> for entry in pack.Puzzles do seen.Add(PuzzleSymmetry.key entry.Puzzle) |> ignore
    | Error error -> failwith $"Could not check previous pack {path}: {error}"
let selected = ResizeArray<JsonNode>()
for path in Directory.GetFiles(work,"selected-*.json") |> Array.sort do
    let root=JsonNode.Parse(File.ReadAllText path)
    let playable=PuzzlePacks.read(root.ToJsonString()) |> Result.defaultWith failwith
    if not (seen.Add(PuzzleSymmetry.key playable.Puzzles[0].Puzzle)) then failwith "Checkpoint duplicates another puzzle."
    selected.Add root
let checkpoint () =
    JsonSerializer.SerializeToNode({|requests=requests;failures=failures;rejections=rejections;duplicates=duplicates;generatorSeconds=generatorSeconds;selected=selected.Count|},jsonOptions)
    |> save stateFile
let batchSeed request =
    if request=0 then seed else
    let mutable value=uint64 seed+0x54414E474C455342UL+uint64 request*0x9E3779B97F4A7C15UL
    value <- (value ^^^ (value >>> 30))*0xBF58476D1CE4E5B9UL
    value <- (value ^^^ (value >>> 27))*0x94D049BB133111EBUL
    value <- value ^^^ (value >>> 31)
    int(value &&& 0x7FFFFFFFUL)
let timer=Stopwatch.StartNew()
let cancellation=new CancellationTokenSource(TimeSpan.FromMinutes 60.)
let ct=cancellation.Token
let limit=if mode="connect-only" then 100000 else 2000
let workers=
    if mode="connect-only" then 1
    elif tier="hard" then min 8 (max 1 (Environment.ProcessorCount/3))
    else min 4 (max 1 (Environment.ProcessorCount/4))
printfn "START %s %s: %d puzzles, %d pairs; resumed %d; %d workers" mode tier count pairs selected.Count workers
let propose request =
    let recipe={generation with Seed=batchSeed request}
    let candidate=if mode="connect-only" then Generation.generate ct recipe else GuidedGeneration.generate ct recipe policy
    candidate |> Result.map(fun generated ->
        let screen=if mode="connect-only" then Some(Solver.shortestRouteScreen ct screenLimit generated.Puzzle) else None
        generated,screen)
while selected.Count<count && requests<limit do
    ct.ThrowIfCancellationRequested()
    // Independent seeds run concurrently; consume results in request order so
    // scheduling cannot change the selected sequence or checkpoint identities.
    let batchSize=min workers (min (count-selected.Count) (limit-requests))
    let proposed=[|requests..requests+batchSize-1|] |> Array.Parallel.map propose
    for candidate in proposed do
        requests <- requests+1
        match candidate with
        | Error error when error.StartsWith("Internal",StringComparison.OrdinalIgnoreCase) -> failwith error
        | Error _ -> failures<-failures+1
        | Ok (generated,screen) ->
            let key=PuzzleSymmetry.key generated.Puzzle
            if seen.Contains key then duplicates<-duplicates+1
            else
                let qualifies =
                    match screen with
                    | Some s when tier="hard" -> not s.Solved
                    | Some s -> s.Solved && s.FirstSuccessTrial.Value>screenLower
                    | None when tier="medium" -> generated.Quality.Value.Reasoning.Value.FirstSolutionSearchNodes<=255
                    | None -> true
                if not qualifies then rejections<-rejections+1
                else
                    seen.Add key |> ignore
                    let root=JsonNode.Parse(Export.createPack generation [|generated|] |> Export.toJson)
                    match screen with
                    | Some s -> root.["puzzles"].[0].["shortestRoutes"] <- JsonSerializer.SerializeToNode(s,jsonOptions)
                    | None -> ()
                    selected.Add root
                    let path=Path.Combine(work,$"selected-{selected.Count:D6}.json")
                    save path root
                    if selected.Count%10=0 || selected.Count=count then
                        printfn "SELECTED %s %s %d/%d after %d requests, %.1fs" mode tier selected.Count count requests (generatorSeconds+timer.Elapsed.TotalSeconds)
        if requests%10=0 || selected.Count=count then
            let elapsed=timer.Elapsed.TotalSeconds
            generatorSeconds<-generatorSeconds+elapsed
            timer.Restart()
            checkpoint()
if selected.Count<>count then failwith $"Selected only {selected.Count}/{count} after {requests} requests; checkpoints retained in {work}."
let root=selected[0].DeepClone()
let entries=JsonArray()
let score (node:JsonNode) =
    let puzzle=node.["puzzles"].[0]
    if mode="connect-only" then
        let screen=puzzle["shortestRoutes"]
        if screen["solved"].GetValue<bool>() then screen["firstSuccessTrial"].GetValue<int>() else 8193
    elif tier="easy" then puzzle.["difficulty"].["reasoning"].["trialSteps"].GetValue<int>()
    else puzzle.["difficulty"].["reasoning"].["firstSolutionSearchNodes"].GetValue<int>()
let ordered=selected |> Seq.sortBy(fun node -> score node,node.["puzzles"].[0].["id"].GetValue<string>()) |> Array.ofSeq
let middle=ordered.Length/2
let representative=ordered[middle]
let rest=ordered |> Array.mapi(fun index node -> index,node) |> Array.choose(fun(index,node)->if index=middle then None else Some node)
let orderingRandom=Random(seed ^^^ 0x43544C32)
for index in rest.Length-1 .. -1 .. 1 do
    let other=orderingRandom.Next(index+1)
    let value=rest[index]
    rest[index]<-rest[other]
    rest[other]<-value
for item in Array.append [|representative|] rest do entries.Add(item.["puzzles"].[0].DeepClone())
root["puzzles"] <- entries
root.["generation"].["seed"] <- JsonValue.Create seed
root.["generation"].["collectionSelection"] <- JsonSerializer.SerializeToNode(
    {|method="player-feedback-v2";tier=tier;provisional=true;ordering="representative-first-then-seeded-shuffle-v1";requests=requests;rejectedSearches=failures;rejectedDifficulty=rejections;rejectedDuplicates=duplicates
      shortestRouteTrialLimit=(if mode="connect-only" then Some screenLimit else None)
      mustDefeatShortestRouteTrials=(if mode="connect-only" then Some screenLower else None)
      mustEventuallyPassScreen=(if mode="connect-only" then Some(tier<>"hard") else None)
      minimumFirstSolutionDepth=(if mode="full-coverage" && tier<>"easy" then Some policy.MinBranchDepth else None)
      minimumFirstSolutionNodes=(if mode="full-coverage" && tier<>"easy" then Some policy.MinFirstSolutionNodes else None)
      maximumFirstSolutionNodes=(if mode="full-coverage" && tier="medium" then Some 255 else None)
      seconds=generatorSeconds|},jsonOptions)
let playable=PuzzlePacks.read(root.ToJsonString()) |> Result.defaultWith failwith
if playable.Puzzles.Length<>count || (playable.Puzzles |> Array.exists(fun p->p.Solution.IsNone)) then failwith "Final pack did not pass the shared loader and solution validation."
save destination root
printfn "SAVED %s: %d puzzles, %d requests, %.1fs" destination count requests generatorSeconds
cancellation.Dispose()
