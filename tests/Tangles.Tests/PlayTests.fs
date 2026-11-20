module Tangles.PlayTests

open System.Threading
open System.Text.Json.Nodes
open Tangles.Core
open Tangles.Generator
open Xunit

let private c x y = { X = x; Y = y }
let private board =
    { Width = 4; Height = 3; RequireFullCoverage = false
      Pairs = [ { Label = "A"; Start = c 0 0; Finish = c 2 0 }
                { Label = "B"; Start = c 0 2; Finish = c 2 2 } ] }
let private require = function Ok value -> value | Error error -> failwith error
let private draw cells state =
    let active = Play.start (List.head cells) state |> require
    List.fold (fun state cell -> Play.move cell state |> require) active (List.tail cells) |> Play.finish

[<Fact>]
let ``player completes a board with unused squares`` () =
    let state = Play.create board |> draw [ c 0 0; c 1 0; c 2 0 ] |> draw [ c 0 2; c 1 2; c 2 2 ]
    Assert.True(Play.isComplete state)
    Assert.Equal(2, Play.completedCount state)
    Assert.Equal(6, Play.paths state |> List.sumBy _.Cells.Length)

[<Fact>]
let ``undo restores an entire stroke rather than individual squares`` () =
    let state = Play.create board |> draw [ c 0 0; c 1 0; c 2 0 ]
    Assert.Single(state.History) |> ignore
    Assert.True((Play.undo state).Paths.IsEmpty)

[<Fact>]
let ``retracing shortens a path and can be undone`` () =
    let state = Play.create board |> draw [ c 0 0; c 1 0; c 2 0 ]
    let trimmed = Play.start (c 1 0) state |> require |> Play.finish
    Assert.Equal(2, trimmed.Paths["A"].Length)
    Assert.Equal(0, Play.completedCount trimmed)
    Assert.True((Play.undo trimmed).Paths = state.Paths)

[<Fact>]
let ``either endpoint can start a replacement path`` () =
    let state = Play.create board |> draw [ c 0 0; c 1 0; c 2 0 ]
    let reversed = state |> draw [ c 2 0; c 1 0; c 0 0 ]
    Assert.Equal(1, Play.completedCount reversed)
    Assert.True((Play.undo reversed).Paths = state.Paths)

[<Fact>]
let ``self contact is rejected while drawing`` () =
    let puzzle = { Width = 4; Height = 4; RequireFullCoverage = false; Pairs = [ { Label = "A"; Start = c 0 0; Finish = c 3 3 } ] }
    let state = Play.create puzzle |> Play.start (c 0 0) |> require
    let state = [ c 1 0; c 2 0; c 2 1 ] |> List.fold (fun state cell -> Play.move cell state |> require) state
    Assert.True(Play.move (c 1 1) state |> Result.isError)
    Assert.Equal(c 2 1, List.last state.Paths["A"])

[<Fact>]
let ``drawing cannot skip squares enter another endpoint or share a path`` () =
    let state = Play.create board |> Play.start (c 0 0) |> require
    Assert.True(Play.move (c 1 1) state |> Result.isError)
    let state = state |> Play.move (c 0 1) |> require
    Assert.True(Play.move (c 0 2) state |> Result.isError)
    let state = state |> Play.finish |> Play.start (c 0 2) |> require
    Assert.True(Play.move (c 0 1) state |> Result.isError)

[<Fact>]
let ``a completed path cannot extend past its endpoint`` () =
    let state = Play.create board |> Play.start (c 0 0) |> require |> Play.move (c 1 0) |> require |> Play.move (c 2 0) |> require
    Assert.True(Play.move (c 3 0) state |> Result.isError)
    Assert.True(Play.move (c 1 0) state |> Result.isOk)

[<Fact>]
let ``reset is undoable and an empty reset creates no history`` () =
    let empty = Play.create board
    Assert.Empty((Play.reset empty).History)
    let state = empty |> draw [ c 0 0; c 1 0; c 2 0 ]
    let reset = Play.reset state
    Assert.True(reset.Paths.IsEmpty)
    Assert.True((Play.undo reset).Paths = state.Paths)

[<Fact>]
let ``progress round trips and invalid saved routes are rejected`` () =
    let state = Play.create board |> draw [ c 0 0; c 1 0 ]
    let json = PuzzlePacks.writeProgress (Play.paths state)
    let saved = PuzzlePacks.readProgress board json |> require
    let restored = Play.restore saved (Play.create board) |> require
    Assert.True(restored.Paths = state.Paths)
    Assert.True(PuzzlePacks.readProgress board "not json" |> Result.isError)
    Assert.True(Play.restore [ { Label = "A"; Cells = [ c 1 1 ] } ] (Play.create board) |> Result.isError)

let private generatedPack () =
    let options = Generation.defaults 42
    let generated = Generation.generate CancellationToken.None options |> require
    Export.createPack options [| generated |] |> Export.toJson

[<Fact>]
let ``player loads the generator format without depending on its solution`` () =
    let json = generatedPack ()
    let parsed = PuzzlePacks.read json |> require
    Assert.Single(parsed.Puzzles) |> ignore
    Assert.Equal(8, parsed.Puzzles[0].Puzzle.Width)
    Assert.Equal(6, parsed.Puzzles[0].Puzzle.Pairs.Length)
    Assert.True(parsed.Puzzles[0].Solution.IsSome)
    let document = JsonNode.Parse json
    (document["puzzles"][0]).AsObject().Remove("solution") |> ignore
    (document["puzzles"][0]).AsObject().Remove("verification") |> ignore
    let cluesOnly = PuzzlePacks.read (document.ToJsonString()) |> require
    Assert.True(cluesOnly.Puzzles[0].Solution.IsNone)

[<Theory>]
[<InlineData("null")>]
[<InlineData("false")>]
[<InlineData("{}")>]
[<InlineData("[]")>]
[<InlineData("[{\"label\":\"A\",\"cells\":[] }]")>]
let ``invalid optional solutions disable hints without preventing play`` invalidSolution =
    let document = JsonNode.Parse(generatedPack ())
    document.["puzzles"].[0].["solution"] <- JsonNode.Parse(invalidSolution: string)
    let pack = PuzzlePacks.read (document.ToJsonString()) |> require
    Assert.True(pack.Puzzles[0].Solution.IsNone)
    Assert.Empty(Validation.puzzle pack.Puzzles[0].Puzzle)

[<Fact>]
let ``hint witnesses are validated rather than trusting verification metadata`` () =
    let document = JsonNode.Parse(generatedPack ())
    document.["puzzles"].[0].["solution"].[0].["cells"].[0].["x"] <- JsonValue.Create(-1)
    let pack = PuzzlePacks.read (document.ToJsonString()) |> require
    Assert.True(pack.Puzzles[0].Solution.IsNone)

[<Fact>]
let ``a hint replaces a partial path preserves other colours saves and undoes`` () =
    let before = Play.create board |> draw [ c 0 0; c 1 0 ] |> draw [ c 2 2; c 1 2; c 0 2 ]
    let route = { Label = "A"; Cells = [ c 0 0; c 1 0; c 2 0 ] }
    let hinted = Play.reveal route before |> require
    Assert.True(hinted.Paths["A"] = route.Cells)
    Assert.True(hinted.Paths["B"] = before.Paths["B"])
    Assert.True(Play.isComplete hinted)
    Assert.True(hinted.Active.IsNone)
    let saved = PuzzlePacks.writeProgress (Play.paths hinted) |> PuzzlePacks.readProgress board |> require
    let restored = Play.restore saved (Play.create board) |> require
    Assert.True(Play.isComplete restored)
    Assert.True((Play.undo hinted).Paths = before.Paths)

[<Fact>]
let ``a hint clears intersecting colours and one undo restores the entire change`` () =
    let before = Play.create board |> draw [ c 0 2; c 0 1; c 1 1; c 1 0 ] |> Play.start (c 0 0) |> require
    let route = { Label = "A"; Cells = [ c 0 0; c 1 0; c 2 0 ] }
    let hinted = Play.reveal route before |> require
    Assert.False(hinted.Paths.ContainsKey "B")
    Assert.True(hinted.Paths["A"] = route.Cells)
    Assert.Empty(Validation.partialSolution board (Play.paths hinted))
    Assert.True((Play.undo hinted).Paths = before.Paths)

[<Fact>]
let ``hints prefer unfinished colours and stop after any valid completion`` () =
    let solution = [ { Label = "A"; Cells = [ c 0 0; c 1 0; c 2 0 ] }
                     { Label = "B"; Cells = [ c 0 2; c 1 2; c 2 2 ] } ]
    let empty = Play.create board
    Assert.Equal(2, (Play.hintCandidates solution empty).Length)
    let withA = empty |> draw [ c 2 0; c 1 0; c 0 0 ]
    Assert.Equal("B", (Play.hintCandidates solution withA |> List.exactlyOne).Label)
    // B takes a valid alternative route instead of the stored one.
    let solved = withA |> draw [ c 0 2; c 0 1; c 1 1; c 2 1; c 2 2 ]
    Assert.True(Play.isComplete solved)
    Assert.Empty(Play.hintCandidates solution solved)

[<Fact>]
let ``invalid hint paths cannot bypass the shared rules`` () =
    let state = Play.create board
    for route in [ { Label = "A"; Cells = [] }
                   { Label = "A"; Cells = [ c 0 0; c 1 0 ] }
                   { Label = "A"; Cells = [ c 0 0; c 2 0 ] }
                   { Label = "missing"; Cells = [ c 0 0; c 1 0; c 2 0 ] } ] do
        Assert.True(Play.reveal route state |> Result.isError)
    Assert.True(state.Paths.IsEmpty)

[<Fact>]
let ``full coverage hints can repair connected routes with gaps and eventually solve`` () =
    let solution =
        [ { Label = "A"; Cells = [ c 1 3; c 2 3; c 2 2; c 2 1; c 1 1; c 0 1 ] }
          { Label = "B"; Cells = [ c 4 1; c 4 2; c 3 2; c 3 3 ] }
          { Label = "C"; Cells = [ c 2 0; c 1 0; c 0 0 ] }
          { Label = "D"; Cells = [ c 4 3; c 4 4; c 3 4; c 2 4; c 1 4; c 0 4; c 0 3; c 0 2; c 1 2 ] }
          { Label = "E"; Cells = [ c 4 0; c 3 0; c 3 1 ] } ]
    let puzzle =
        { Width = 5; Height = 5; RequireFullCoverage = true
          Pairs = solution |> List.map (fun path -> { Label = path.Label; Start = path.Cells.Head; Finish = List.last path.Cells }) }
    Assert.Empty(Validation.solution puzzle solution)
    let before =
        Play.create puzzle
        |> draw [ c 0 1; c 0 2; c 0 3; c 1 3 ]
        |> draw solution[1].Cells
        |> draw (List.rev solution[2].Cells)
        |> draw [ c 1 2; c 2 2; c 2 3; c 2 4; c 3 4; c 4 4; c 4 3 ]
        |> draw solution[4].Cells
    Assert.Equal(5, Play.completedCount before)
    Assert.False(Play.isComplete before)
    let candidates = Play.hintCandidates solution before
    Assert.True((candidates |> List.map _.Label) = [ "A"; "D" ])
    let hinted = Play.reveal candidates.Head before |> require
    Assert.True((Play.undo hinted).Paths = before.Paths)
    let completed = Play.reveal (Play.hintCandidates solution hinted |> List.exactlyOne) hinted |> require
    Assert.True(Play.isComplete completed)
    Assert.Empty(Play.hintCandidates solution completed)

[<Theory>]
[<InlineData(false)>]
[<InlineData(true)>]
let ``repeated hints solve generated packs in either mode`` fullCoverage =
    for seed in 0 .. 9 do
        let options = { Generation.defaults seed with Mode = if fullCoverage then FullCoverage else ConnectOnly }
        let generated = Generation.generate CancellationToken.None options |> require
        let loaded = Export.createPack options [| generated |] |> Export.toJson |> PuzzlePacks.read |> require
        let entry = loaded.Puzzles[0]
        let solution = entry.Solution.Value
        let mutable state = Play.create entry.Puzzle
        for index in 0 .. entry.Puzzle.Pairs.Length - 1 do
            let candidates = Play.hintCandidates solution state
            Assert.Equal(entry.Puzzle.Pairs.Length - index, candidates.Length)
            state <- Play.reveal candidates[index % candidates.Length] state |> require
        Assert.True(Play.isComplete state)

[<Fact>]
let ``all generated witnesses are playable using the shared player operations`` () =
    for seed in 0 .. 19 do
        let generated = Generation.generate CancellationToken.None (Generation.defaults seed) |> require
        let state = generated.Solution |> List.fold (fun state path -> draw path.Cells state) (Play.create generated.Puzzle)
        Assert.True(Play.isComplete state)

[<Fact>]
let ``pack reader rejects unsupported rules versions malformed clues and conflicting IDs`` () =
    Assert.True(PuzzlePacks.read "[]" |> Result.isError)
    Assert.True(PuzzlePacks.read "{" |> Result.isError)
    let version = JsonNode.Parse(generatedPack ())
    version["formatVersion"] <- JsonValue.Create(2)
    Assert.True(PuzzlePacks.read (version.ToJsonString()) |> Result.isError)
    let rules = JsonNode.Parse(generatedPack ())
    rules["rules"]["noSelfTouching"] <- JsonValue.Create(false)
    Assert.True(PuzzlePacks.read (rules.ToJsonString()) |> Result.isError)
    let clues = JsonNode.Parse(generatedPack ())
    let cluePuzzle = (clues["puzzles"]).AsArray().[0]
    let cluePair = (cluePuzzle["pairs"]).AsArray().[0]
    let start = cluePair["start"]
    start["x"] <- JsonValue.Create(-1)
    Assert.True(PuzzlePacks.read (clues.ToJsonString()) |> Result.isError)
    let repeated = JsonNode.Parse(generatedPack ())
    let duplicate = (repeated["puzzles"][0]).DeepClone()
    repeated["puzzles"].AsArray().Add(duplicate)
    Assert.True(PuzzlePacks.read (repeated.ToJsonString()) |> Result.isOk)
    duplicate["width"] <- JsonValue.Create(9)
    Assert.True(PuzzlePacks.read (repeated.ToJsonString()) |> Result.isError)

[<Fact>]
let ``partial validation forbids passing through an endpoint`` () =
    let paths = [ { Label = "A"; Cells = [ c 0 0; c 1 0; c 2 0; c 3 0 ] } ]
    Assert.NotEmpty(Validation.partialSolution board paths)

[<Fact>]
let ``full coverage does not complete when every pair is connected but squares remain`` () =
    let full = { board with RequireFullCoverage = true }
    let state = Play.create full |> draw [ c 0 0; c 1 0; c 2 0 ] |> draw [ c 0 2; c 1 2; c 2 2 ]
    Assert.Equal(2, Play.completedCount state)
    Assert.False(Play.isComplete state)
    Assert.Contains("Every square must be covered.", Validation.solution full (Play.paths state))
    Assert.Empty(Validation.partialSolution full (Play.paths state))
    let saved = PuzzlePacks.writeProgress (Play.paths state)
    Assert.True(PuzzlePacks.readProgress full saved |> Result.isOk)

[<Fact>]
let ``full coverage packs load and their witnesses can be completed through player operations`` () =
    for seed in 0 .. 19 do
        let options = { Generation.defaults seed with Mode = FullCoverage }
        let generated = Generation.generate CancellationToken.None options |> require
        let pack = Export.createPack options [| generated |] |> Export.toJson |> PuzzlePacks.read |> require
        let puzzle = pack.Puzzles[0].Puzzle
        Assert.True(puzzle.RequireFullCoverage)
        let state = generated.Solution |> List.fold (fun state path -> draw path.Cells state) (Play.create puzzle)
        Assert.True(Play.isComplete state)
        Assert.Equal(puzzle.Width * puzzle.Height, Play.paths state |> List.sumBy (fun path -> path.Cells.Length))

[<Fact>]
let ``existing packs retain optional coverage`` () =
    let pack = generatedPack () |> PuzzlePacks.read |> require
    Assert.False(pack.Puzzles[0].Puzzle.RequireFullCoverage)

[<Theory>]
[<InlineData(false)>]
[<InlineData(true)>]
let ``player coverage rule comes from the JSON rules flag rather than generation metadata`` (fullCoverage: bool) =
    let document = JsonNode.Parse(generatedPack ())
    document["rules"]["requireFullCoverage"] <- JsonValue.Create(fullCoverage)
    document["generation"]["mode"] <- JsonValue.Create(if fullCoverage then "connect-only" else "full-coverage")
    let pack = PuzzlePacks.read (document.ToJsonString()) |> require
    Assert.Equal(fullCoverage, pack.Puzzles[0].Puzzle.RequireFullCoverage)

[<Theory>]
[<InlineData("null")>]
[<InlineData("\"true\"")>]
[<InlineData("0")>]
let ``player rejects an invalid coverage flag with a useful message`` invalidFlag =
    let document = JsonNode.Parse(generatedPack ())
    document["rules"]["requireFullCoverage"] <- JsonNode.Parse(invalidFlag: string)
    match PuzzlePacks.read (document.ToJsonString()) with
    | Ok _ -> failwith "Invalid coverage flag was accepted."
    | Error message -> Assert.Contains("Field 'requireFullCoverage' must be true or false.", message)

[<Fact>]
let ``player requires an explicit coverage rule instead of guessing`` () =
    let document = JsonNode.Parse(generatedPack ())
    document["rules"].AsObject().Remove("requireFullCoverage") |> ignore
    match PuzzlePacks.read (document.ToJsonString()) with
    | Ok _ -> failwith "Missing coverage flag was accepted."
    | Error message -> Assert.Contains("Missing field 'requireFullCoverage'.", message)

[<Fact>]
let ``pack catalogue reads names and paths without copying puzzle metadata`` () =
    let json = """{"packs":[{"name":"Starter collection","path":"packs/starter.json"},{"name":"Full coverage","path":"packs/full-coverage.json"}]}"""
    let entries = PuzzlePacks.readCatalog json |> require
    Assert.Equal(2, entries.Length)
    Assert.Equal("Starter collection", entries[0].Name)
    Assert.Equal("packs/full-coverage.json", entries[1].Path)

[<Theory>]
[<InlineData("not json")>]
[<InlineData("[]")>]
[<InlineData("{\"packs\":[]}")>]
[<InlineData("{\"packs\":{}}")>]
[<InlineData("{\"packs\":[{\"name\":\"Starter\"}]}")>]
[<InlineData("{\"packs\":[{\"name\":\" \",\"path\":\"packs/starter.json\"}]}")>]
let ``malformed pack catalogues are rejected`` json =
    Assert.True(PuzzlePacks.readCatalog json |> Result.isError)
