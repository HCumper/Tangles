// Generate independent golden reports with the existing, shared F# implementation.
open System
open System.IO
open System.Text.Json
open System.Threading
open Tangles.Core
open Tangles.Generator
let jsonOptions=JsonSerializerOptions(PropertyNamingPolicy=JsonNamingPolicy.CamelCase,WriteIndented=true)
let cell w i:Cell={X=i%w;Y=i/w}
let make full w h pairs: Puzzle =
    {Width=w;Height=h;RequireFullCoverage=full
     Pairs=pairs |> List.mapi(fun i (a,b)->{Label=string(char(65+i));Start=cell w a;Finish=cell w b})}
let cases=ResizeArray<_>()
let add name work nodes board =
    let solver=Solver.analyse CancellationToken.None {MaxWork=work;MaxSearchNodes=nodes} board
    let geometry=try Some(Difficulty.assess board) with _ -> None
    cases.Add {|name=name;board=board;work=work;nodes=nodes;solver=solver;geometry=geometry|}
for full in [false;true] do
    add $"rows-{full}" 2000000 20000 (make full 3 3 [0,2;3,5;6,8])
    add $"gaps-{full}" 2000000 20000 (make full 3 3 [0,2;6,8])
    add $"single-{full}" 2000000 20000 (make full 3 3 [0,8])
    add $"line-{full}" 2000000 20000 (make full 6 1 [0,2;3,5])
    add $"tiny-budget-{full}" 1 20000 (make full 4 4 [0,15;3,12])
    add $"node-budget-{full}" 2000000 1 (make full 3 3 [0,8])
    let random=Random 113
    for i=1 to 12 do
        let endpoints=[|0..8|] |> Array.sortBy(fun _->random.Next())
        add $"random-{full}-{i}" 30000 20000 (make full 3 3 [endpoints[0],endpoints[1];endpoints[2],endpoints[3]])
for full,pairs in [true,64;false,63] do
    add $"high-bit-{full}" 2000000 20000 (make full 16 8 [for i in 0..pairs-1 -> 2*i,2*i+1])
add "medium-routes" 2000000 20000 (make true 8 8 [7,19;51,57;56,24;31,20;16,53;50,30;54,37;6,0;29,8])
add "hard-trials" 2000000 20000 (make true 8 8 [36,19;51,13;50,9;11,33;29,52;14,8;34,49;37,35])
add "unsupported-width" 2000000 20000 (make false 17 1 [0,2;8,16])
let hard=make true 8 8 [31,41;49,51;2,59;3,53;26,54;14,27]
add "hard-budget" 1000 20000 hard
let destination=fsi.CommandLineArgs |> Array.last
File.WriteAllText(destination,JsonSerializer.Serialize(cases,jsonOptions))
printfn "%d golden solver/geometry cases" cases.Count
