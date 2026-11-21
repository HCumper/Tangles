// Check every shipped pack with the actual shared loader and completion rules.
open System.IO
open System.Text.Json
open Tangles.Core

let unwrap = function Ok value -> value | Error error -> failwith error
let root = fsi.CommandLineArgs |> Array.last
let catalogue = JsonDocument.Parse(File.ReadAllText(Path.Combine(root, "packs.json")))
let mutable packs = 0
let mutable puzzles = 0
for entry in catalogue.RootElement.GetProperty("packs").EnumerateArray() do
    let relative = entry.GetProperty("path").GetString().Split('?')[0]
    let pack = File.ReadAllText(Path.Combine(root, relative)) |> PuzzlePacks.read |> unwrap
    for puzzle in pack.Puzzles do
        let solution = puzzle.Solution |> Option.defaultWith (fun () -> failwith "Missing valid hint solution")
        let mutable state = Play.create puzzle.Puzzle
        for path in solution do state <- Play.reveal path state |> unwrap
        if not (Play.isComplete state) then failwithf "Incomplete witness: %s" puzzle.Id
        puzzles <- puzzles + 1
    packs <- packs + 1
printfn "PASS: %d packs / %d puzzles: shared loader, stored hints and completion rules" packs puzzles
catalogue.Dispose()
