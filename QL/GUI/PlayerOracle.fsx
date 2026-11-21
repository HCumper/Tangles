// Golden traces from the actual shared F# player, not a second rules model.
// Supply the current Tangles.Core assembly with dotnet fsi --reference.
open System
open System.IO
open System.Text.Json
open Tangles.Core

type Snapshot =
    { Action: string; Cell: int; Accepted: bool; Active: int; History: int
      Connected: int; Used: int; Solved: bool; Owners: int array; Orders: int array; Candidates: int array }
type Case = { Name: string; Json: string; HintAvailable: bool; Steps: Snapshot array }
let options = JsonSerializerOptions(PropertyNamingPolicy=JsonNamingPolicy.CamelCase)
let cases = ResizeArray<Case>()
let cell x y : Cell = {X=x;Y=y}
let pair label a b : EndpointPair = {Label=label;Start=a;Finish=b}
let packJson (board: Puzzle) id =
    JsonSerializer.Serialize(
        {| formatVersion=1
           rules={|orthogonalOnly=true;noSharedCells=true;noSelfTouching=true;requireFullCoverage=board.RequireFullCoverage|}
           puzzles=[|{|id=id;width=board.Width;height=board.Height;pairs=Array.ofList board.Pairs|}|] |},options)
let makeCaseWith name (board: Puzzle) (solution: SolutionPath list option) json (produce: PlayState -> int -> string * int) count =
    let mutable state = Play.create board
    let mutable saved = []
    let steps = ResizeArray<Snapshot>()
    for i in 0..count-1 do
        let action,index = produce state i
        let at = {X=index%board.Width;Y=index/board.Width}
        let result =
            match action with
            | "begin" -> Play.start at state
            | "move" -> Play.move at state
            | "end" -> Ok(Play.finish state)
            | "undo" -> Ok(Play.undo state)
            | "reset" -> Ok(Play.reset state)
            | "save" -> let next=Play.finish state in saved<-Play.paths next; Ok next
            | "reload" -> Play.restore saved (Play.create board)
            | "hint" ->
                match solution with
                | Some paths when index>0 && index<=board.Pairs.Length -> Play.reveal (paths |> List.find(fun p -> p.Label=board.Pairs[index-1].Label)) state
                | _ -> Error "No valid hint"
            | _ -> failwith action
        let accepted =
            match result with
            | Ok next -> state<-next;true
            | Error _ -> false
        let owners,orders=Array.zeroCreate(board.Width*board.Height),Array.zeroCreate(board.Width*board.Height)
        for path in Play.paths state do
            let p=1+List.findIndex(fun (p: EndpointPair) -> p.Label=path.Label) board.Pairs
            for j,c in List.indexed path.Cells do owners[c.Y*board.Width+c.X]<-p;orders[c.Y*board.Width+c.X]<-j+1
        steps.Add {Action=action;Cell=index;Accepted=accepted
                   Active=state.Active |> Option.map(fun label -> 1+List.findIndex(fun (p: EndpointPair) -> p.Label=label) board.Pairs) |> Option.defaultValue 0
                   History=state.History.Length;Connected=Play.completedCount state
                   Used=owners |> Array.filter((<>)0) |> Array.length;Solved=Play.isComplete state;Owners=owners;Orders=orders
                   Candidates=solution |> Option.map(fun paths -> Play.hintCandidates paths state |> List.map(fun p -> 1+List.findIndex(fun (q:EndpointPair) -> q.Label=p.Label) board.Pairs) |> List.sort |> Array.ofList) |> Option.defaultValue [||]}
    cases.Add {Name=name;Json=json;HintAvailable=solution.IsSome;Steps=steps.ToArray()}
let makeCase name board produce count = makeCaseWith name board None (packJson board name) produce count

for full in [false;true] do
    let board={Width=3;Height=3;RequireFullCoverage=full;Pairs=[pair "A" (cell 0 0) (cell 2 2)]}
    let events=[|"begin",0;"move",1;"move",4;"move",3;"move",5;"move",8;"end",0
                 "undo",0;"begin",1;"move",0;"end",0;"undo",0;"reset",0;"undo",0
                 "save",0;"reset",0;"reload",0;"begin",8;"move",7;"move",6;"move",3;"move",0;"end",0|]
    makeCase $"touch-and-coverage-{full}" board (fun _ i -> events[i]) events.Length
    let rng=Random(if full then 49 else 23)
    let board={Width=4;Height=4;RequireFullCoverage=full;Pairs=[pair "A" (cell 0 0) (cell 3 0);pair "B" (cell 0 2) (cell 3 2)]}
    makeCase $"random-{full}" board (fun state i ->
        let n=rng.Next(20)
        if n<5 then
            let endpoints=board.Pairs |> List.collect(fun p -> [p.Start;p.Finish])
            let c=if rng.Next(3)=0 then cell (rng.Next(4)) (rng.Next(4)) else endpoints[rng.Next(endpoints.Length)]
            "begin",c.Y*4+c.X
        elif n<14 then
            let c=
                match state.Active with
                | Some label ->
                    let neighbours=state.Paths[label] |> List.last |> Cell.neighbours |> List.filter(Cell.isInside 4 4)
                    neighbours[rng.Next(neighbours.Length)]
                | None -> cell (rng.Next(4)) (rng.Next(4))
            "move",c.Y*4+c.X
        elif n<16 then "end",0
        elif n<18 then "undo",0
        elif n=18 then "reset",0
        else "save",0) 450

let historyBoard={Width=2;Height=1;RequireFullCoverage=true;Pairs=[pair "A" (cell 0 0) (cell 1 0)]}
let historyEvents=
    [| for i in 1..115 do yield "begin",0;yield "move",1;yield "end",0;yield "reset",0
       for i in 1..105 do yield "undo",0 |]
makeCase "history-ring" historyBoard (fun _ i -> historyEvents[i]) historyEvents.Length

for pairs in [12;14;16] do
    let json=File.ReadAllText(Path.Combine(__SOURCE_DIRECTORY__, $"../../puzzles/sample-16x16-{pairs}pairs.json"))
    let pack=PuzzlePacks.read json |> function Ok p -> p | Error e -> failwith e
    use doc=JsonDocument.Parse json
    let events=ResizeArray<string*int>()
    let entry=doc.RootElement.GetProperty("puzzles").EnumerateArray() |> Seq.head
    for route in entry.GetProperty("solution").EnumerateArray() do
        for i,c in route.GetProperty("cells").EnumerateArray() |> Seq.indexed do
            events.Add((if i=0 then "begin" else "move"),c.GetProperty("y").GetInt32()*16+c.GetProperty("x").GetInt32())
        events.Add("end",0)
    events.Add("save",0);events.Add("reset",0);events.Add("undo",0);events.Add("reload",0)
    makeCase $"witness-16x16-{pairs}" (pack.Puzzles[0].Puzzle) (fun _ i -> events[i]) events.Count

let hintCase name board (paths: SolutionPath list) (mutate: string -> string) events =
    let json =
        JsonSerializer.Serialize(
            {| formatVersion=1
               rules={|orthogonalOnly=true;noSharedCells=true;noSelfTouching=true;requireFullCoverage=board.RequireFullCoverage|}
               puzzles=[|{|id=name;width=board.Width;height=board.Height;pairs=Array.ofList board.Pairs;solution=paths|}|] |},options) |> mutate
    let parsed = PuzzlePacks.read json |> function Ok p -> p | Error e -> failwith e
    makeCaseWith name board parsed.Puzzles[0].Solution json (fun _ i -> events |> List.item i) events.Length

let hintBoard={Width=3;Height=3;RequireFullCoverage=false;Pairs=[pair "A" (cell 0 0) (cell 2 0);pair "B" (cell 0 2) (cell 2 2)]}
let hintPaths:SolutionPath list=[{Label="A";Cells=[cell 0 0;cell 1 0;cell 2 0]};{Label="B";Cells=[cell 0 2;cell 1 2;cell 2 2]}]
hintCase "hint-conflict-undo-save" hintBoard hintPaths id
    ["begin",6;"move",3;"move",4;"move",1;"end",0;"begin",0;"hint",1;"undo",0;"hint",1;"hint",2;"save",0;"reset",0;"reload",0;"undo",0]
hintCase "hint-reversed-alternative" hintBoard hintPaths id
    ["begin",2;"move",1;"move",0;"end",0;"begin",6;"move",3;"move",4;"move",5;"move",8;"end",0;"hint",1;"undo",0]
for name,paths in ["hint-missing-colour",[hintPaths[0]]
                   "hint-duplicate-label",[hintPaths[0];hintPaths[0]]
                   "hint-self-touch",[{hintPaths[0] with Cells=[cell 0 0;cell 0 1;cell 1 1;cell 1 0;cell 2 0]};hintPaths[1]]
                   "hint-wrong-label",[{hintPaths[0] with Label="a"};hintPaths[1]]
                   "hint-outside",[{hintPaths[0] with Cells=[cell 0 0;cell -1 0;cell 2 0]};hintPaths[1]]
                   "hint-disconnected",[{hintPaths[0] with Cells=[cell 0 0;cell 2 0]};hintPaths[1]] ] do
    hintCase name hintBoard paths id ["hint",1;"begin",0;"move",1;"end",0;"undo",0]
for name,mutate in ["hint-null",fun (s:string) -> s.Replace("\"solution\":[", "\"solution\":null,\"ignored\":[")
                    "hint-fraction",fun s -> s.Replace("\"x\":1,", "\"x\":1.0,")
                    "hint-number-type",fun s -> s.Replace("\"cells\":[", "\"cells\":17,\"ignored\":[") ] do
    hintCase name hintBoard hintPaths mutate ["hint",1;"begin",0;"move",1;"end",0]

let fullPaths:SolutionPath list=
    [ {Label="A";Cells=[cell 1 3;cell 2 3;cell 2 2;cell 2 1;cell 1 1;cell 0 1]}
      {Label="B";Cells=[cell 4 1;cell 4 2;cell 3 2;cell 3 3]}
      {Label="C";Cells=[cell 2 0;cell 1 0;cell 0 0]}
      {Label="D";Cells=[cell 4 3;cell 4 4;cell 3 4;cell 2 4;cell 1 4;cell 0 4;cell 0 3;cell 0 2;cell 1 2]}
      {Label="E";Cells=[cell 4 0;cell 3 0;cell 3 1]} ]
let fullBoard={Width=5;Height=5;RequireFullCoverage=true;Pairs=fullPaths |> List.map(fun p -> pair p.Label p.Cells.Head (List.last p.Cells))}
let fullEvents=[for route in [[cell 0 1;cell 0 2;cell 0 3;cell 1 3];fullPaths[1].Cells;List.rev fullPaths[2].Cells;[cell 1 2;cell 2 2;cell 2 3;cell 2 4;cell 3 4;cell 4 4;cell 4 3];fullPaths[4].Cells] do
                    for i,c in List.indexed route do yield (if i=0 then "begin" else "move"),c.Y*5+c.X
                    yield "end",0
                yield "hint",1;yield "undo",0;yield "hint",1;yield "hint",4;yield "save",0;yield "reset",0;yield "reload",0]
hintCase "hint-full-coverage-gaps" fullBoard fullPaths id fullEvents
hintCase "hint-full-incomplete-witness" {hintBoard with RequireFullCoverage=true} hintPaths id ["hint",1;"begin",0;"move",1;"end",0]

File.WriteAllText(fsi.CommandLineArgs |> Array.last,JsonSerializer.Serialize(cases,options))
printfn "%d reference cases, %d state transitions" cases.Count (cases |> Seq.sumBy(fun c -> c.Steps.Length))
