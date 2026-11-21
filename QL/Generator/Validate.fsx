// Use the actual web/shared loader, validator, player, solver and grader.
open System
open System.IO
open System.Text
open System.Text.Json
open System.Security.Cryptography
open System.Threading
open Tangles.Core
open Tangles.Generator
let require = function Ok v->v | Error e->failwith e
let options=JsonSerializerOptions(PropertyNamingPolicy=JsonNamingPolicy.CamelCase)
let mutable count=0
for filename in fsi.CommandLineArgs |> Array.skip 1 |> Array.filter ((<>) "--") do
    let json=File.ReadAllText filename
    let pack=PuzzlePacks.read json |> require
    use doc=JsonDocument.Parse json
    let root=doc.RootElement
    let generation=root.GetProperty("generation")
    let entries=root.GetProperty("puzzles").EnumerateArray() |> Array.ofSeq
    let keys=Collections.Generic.HashSet<string>()
    for i in 0..entries.Length-1 do
        let entry=entries[i]
        let loaded=pack.Puzzles[i]
        let board=loaded.Puzzle
        let paths=loaded.Solution |> Option.defaultWith(fun ()->failwith "Missing or invalid native solution")
        let recipe=match entry.TryGetProperty("recipe") with true,r when r.ValueKind=JsonValueKind.Object->r | _->generation
        let minimum=recipe.GetProperty("minPathLength").GetInt32()
        let maximum=recipe.GetProperty("maxPathLength").GetInt32()
        for p in paths do
            if p.Cells.Length<minimum || p.Cells.Length>maximum then failwith "Path length outside its recipe"
        let pairs=board.Pairs |> List.map(fun p -> $"{p.Label}:{p.Start.X},{p.Start.Y}:{p.Finish.X},{p.Finish.Y}") |> String.concat ";"
        let rule=if board.RequireFullCoverage then ":full-coverage" else ""
        let id="tangles-"+(SHA256.HashData(Encoding.UTF8.GetBytes($"tangles-v1{rule}:{board.Width}x{board.Height}:{pairs}")) |> Convert.ToHexString).ToLowerInvariant()
        if id<>loaded.Id then failwith "Native puzzle ID differs from F#"
        if not(keys.Add(Solver.boardKey board)) then failwith "Repeated clue identity in native batch"
        let mutable state=Play.create board
        for route in paths do state<-Play.reveal route state |> require
        if not(Play.isComplete state) then failwith "Native hint paths do not complete the shared player"
        let actual=entry.GetProperty("difficulty")
        let geometry=Difficulty.assess board
        let mutable expected=geometry
        match actual.TryGetProperty("reasoning") with
        | true,r when r.ValueKind=JsonValueKind.Object ->
            let analysis=Solver.analyse CancellationToken.None {Solver.defaults with MaxWork=r.GetProperty("workLimit").GetInt32()} board
            let expectedJson=JsonSerializer.SerializeToElement(analysis,options)
            for property in expectedJson.EnumerateObject() do
                let value=r.GetProperty(property.Name)
                if value.ToString()<>property.Value.ToString() then failwith $"Native reasoning {property.Name}: {value} != {property.Value}"
            expected<-Difficulty.withReasoning analysis geometry
        | _ -> ()
        if actual.GetProperty("grade").GetString()<>expected.Grade then failwith "Native difficulty grade differs from F#"
        if actual.GetProperty("score").GetInt32()<>expected.Score then failwith "Native difficulty score differs from F#"
        let measurements=JsonSerializer.SerializeToElement(geometry.Measurements,options)
        for property in measurements.EnumerateObject() do
            let actualValue=actual.GetProperty("measurements").GetProperty(property.Name).GetDouble()
            let expectedValue=property.Value.GetDouble()
            if abs(actualValue-expectedValue)>0.00001 then failwith $"Measurement {property.Name}: {actualValue} != {expectedValue}"
        count<-count+1
    printfn "PASS: %s (%d puzzles, shared loader/rules/hints/IDs/grading)" filename entries.Length
printfn "%d native puzzles validated against F#" count
