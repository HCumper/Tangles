// Golden traces from the actual shared F# player, not a second rules model.
#r "../../src/Tangles.Core/bin/Release/net11.0/Tangles.Core.dll"
open System
open System.IO
open System.Text.Json
open Tangles.Core

type Snapshot =
    { Action: string; Cell: int; Accepted: bool; Active: int; History: int
      Connected: int; Used: int; Solved: bool; Owners: int array; Orders: int array }
type Case = { Name: string; Json: string; Steps: Snapshot array }
let options = JsonSerializerOptions(PropertyNamingPolicy=JsonNamingPolicy.CamelCase)
let cases = ResizeArray<Case>()
let cell x y : Cell = {X=x;Y=y}
let pair label a b : EndpointPair = {Label=label;Start=a;Finish=b}
let packJson (board: Puzzle) id =
    JsonSerializer.Serialize(
        {| formatVersion=1
           rules={|orthogonalOnly=true;noSharedCells=true;noSelfTouching=true;requireFullCoverage=board.RequireFullCoverage|}
           puzzles=[|{|id=id;width=board.Width;height=board.Height;pairs=Array.ofList board.Pairs|}|] |},options)
let makeCase name (board: Puzzle) (produce: PlayState -> int -> string * int) count =
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
                   Used=owners |> Array.filter((<>)0) |> Array.length;Solved=Play.isComplete state;Owners=owners;Orders=orders}
    cases.Add {Name=name;Json=packJson board name;Steps=steps.ToArray()}

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

File.WriteAllText(fsi.CommandLineArgs |> Array.last,JsonSerializer.Serialize(cases,options))
printfn "%d reference cases, %d state transitions" cases.Count (cases |> Seq.sumBy(fun c -> c.Steps.Length))
