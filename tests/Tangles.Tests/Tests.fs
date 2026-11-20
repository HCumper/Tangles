module Tangles.Tests

open System
open System.Text.Json
open System.Threading
open Tangles.Core
open Tangles.Generator
open Xunit

let private cell x y = { X = x; Y = y }

let private withPath width height cells =
    let pair = { Label = "A"; Start = List.head cells; Finish = List.last cells }
    { Width = width; Height = height; RequireFullCoverage = false; Pairs = [ pair ] }, [ { Label = "A"; Cells = cells } ]

let private generate options =
    match Generation.generate CancellationToken.None options with
    | Ok generated -> generated
    | Error message -> failwith message

[<Fact>]
let ``unused cells and diagonal contact are allowed`` () =
    let board, paths = withPath 4 4 [ cell 0 0; cell 1 0; cell 1 1 ]
    Assert.Empty(Validation.solution board paths)

[<Fact>]
let ``separate paths may run alongside each other`` () =
    let a = [ cell 0 0; cell 1 0 ]
    let b = [ cell 0 1; cell 1 1 ]
    let board =
        { Width = 2; Height = 2; RequireFullCoverage = false
          Pairs =
            [ { Label = "A"; Start = a.Head; Finish = List.last a }
              { Label = "B"; Start = b.Head; Finish = List.last b } ] }
    let paths = [ { Label = "A"; Cells = a }; { Label = "B"; Cells = b } ]
    Assert.Empty(Validation.solution board paths)

[<Fact>]
let ``nonconsecutive same-path contact is rejected`` () =
    let board, paths = withPath 3 2 [ cell 0 0; cell 1 0; cell 2 0; cell 2 1; cell 1 1 ]
    Assert.Contains("Path A touches itself.", Validation.solution board paths)

[<Fact>]
let ``diagonal steps are rejected`` () =
    let board, paths = withPath 3 3 [ cell 0 0; cell 1 1 ]
    Assert.NotEmpty(Validation.solution board paths)

[<Fact>]
let ``revisiting a cell is rejected`` () =
    let board, paths = withPath 3 3 [ cell 0 0; cell 1 0; cell 0 0; cell 0 1 ]
    Assert.NotEmpty(Validation.solution board paths)

[<Fact>]
let ``shared cells are rejected`` () =
    let board =
        { Width = 3; Height = 3; RequireFullCoverage = false
          Pairs =
            [ { Label = "A"; Start = cell 0 1; Finish = cell 2 1 }
              { Label = "B"; Start = cell 1 0; Finish = cell 1 2 } ] }
    let paths =
        [ { Label = "A"; Cells = [ cell 0 1; cell 1 1; cell 2 1 ] }
          { Label = "B"; Cells = [ cell 1 0; cell 1 1; cell 1 2 ] } ]
    Assert.NotEmpty(Validation.solution board paths)

[<Fact>]
let ``out of bounds paths are rejected`` () =
    let board, _ = withPath 3 3 [ cell 0 0; cell 1 0 ]
    Assert.NotEmpty(Validation.solution board [ { Label = "A"; Cells = [ cell 0 0; cell -1 0; cell 1 0 ] } ])

[<Fact>]
let ``wrong endpoints missing paths and duplicate labels are rejected`` () =
    let board, paths = withPath 4 4 [ cell 0 0; cell 1 0; cell 2 0 ]
    Assert.NotEmpty(Validation.solution board [])
    Assert.NotEmpty(Validation.solution board [ { Label = "A"; Cells = [ cell 0 0; cell 0 1 ] } ])
    Assert.NotEmpty(Validation.solution board [ { paths.Head with Label = "B" } ])
    Assert.NotEmpty(Validation.solution board (paths @ paths))
    Assert.NotEmpty(Validation.puzzle { board with Pairs = board.Pairs @ board.Pairs })

[<Fact>]
let ``valid alternatives and reversed paths are accepted`` () =
    let board, _ = withPath 4 4 [ cell 0 0; cell 1 0; cell 1 1 ]
    let alternative = { Label = "A"; Cells = [ cell 0 0; cell 0 1; cell 1 1 ] }
    Assert.Empty(Validation.solution board [ alternative ])
    Assert.Empty(Validation.solution board [ { alternative with Cells = List.rev alternative.Cells } ])

[<Fact>]
let ``extreme invalid coordinates cannot wrap into adjacent cells`` () =
    let board, paths = withPath 3 3 [ cell Int32.MinValue 0; cell Int32.MaxValue 0 ]
    Assert.NotEmpty(Validation.solution board paths)
    Assert.False(Cell.areAdjacent (cell Int32.MinValue 0) (cell Int32.MaxValue 0))

/// This checker deliberately does not call the production validator or its
/// adjacency helpers: it checks the returned witness directly against the rules.
let private independentlyCheck options (generated: GeneratedPuzzle) =
    Assert.Equal(options.Width, generated.Puzzle.Width)
    Assert.Equal(options.Height, generated.Puzzle.Height)
    Assert.Equal(options.PairCount, generated.Puzzle.Pairs.Length)
    Assert.Equal(options.PairCount, generated.Solution.Length)
    Assert.Equal(options.Mode = FullCoverage, generated.Puzzle.RequireFullCoverage)
    let occupied = Collections.Generic.HashSet<int * int>()

    for path in generated.Solution do
        let cells = Array.ofList path.Cells
        Assert.InRange(cells.Length, options.MinPathLength, options.MaxPathLength)
        let pair = generated.Puzzle.Pairs |> List.find (fun pair -> pair.Label = path.Label)
        Assert.Equal(pair.Start, cells[0])
        Assert.Equal(pair.Finish, cells[cells.Length - 1])

        for i in 0 .. cells.Length - 1 do
            let a = cells[i]
            Assert.InRange(a.X, 0, options.Width - 1)
            Assert.InRange(a.Y, 0, options.Height - 1)
            Assert.True(occupied.Add(a.X, a.Y), "Paths shared or revisited a cell.")

            for j in i + 1 .. cells.Length - 1 do
                let b = cells[j]
                let distance = abs (a.X - b.X) + abs (a.Y - b.Y)
                if j = i + 1 then Assert.Equal(1, distance)
                else Assert.NotEqual(1, distance)

    if options.Mode = FullCoverage then Assert.Equal(options.Width * options.Height, occupied.Count)

[<Theory>]
[<InlineData(1, 12, 2, 3, 6)>]
[<InlineData(2, 2, 1, 2, 4)>]
[<InlineData(3, 5, 3, 3, 7)>]
[<InlineData(6, 6, 4, 4, 10)>]
[<InlineData(8, 8, 6, 4, 16)>]
[<InlineData(10, 7, 6, 4, 16)>]
[<InlineData(15, 15, 12, 4, 16)>]
let ``generated witnesses satisfy an independent rule checker across seeds`` width height pairs minimum maximum =
    for seed in 0 .. 39 do
        let options =
            { Generation.defaults seed with
                Width = width; Height = height; PairCount = pairs
                MinPathLength = minimum; MaxPathLength = maximum }
        let generated = generate options
        independentlyCheck options generated
        Assert.Empty(Validation.solution generated.Puzzle generated.Solution)

[<Fact>]
let ``generation and export are reproducible from a seed`` () =
    let options = Generation.defaults 123
    let first = generate options
    let second = generate options
    Assert.True((first = second))
    Assert.Equal(Export.toJson (Export.createPack options [| first |]), Export.toJson (Export.createPack options [| second |]))

[<Fact>]
let ``generator rejects impossible capacity and invalid sizes before allocating`` () =
    let invalid =
        [ { Generation.defaults 0 with Width = 0 }
          { Generation.defaults 0 with Width = Int32.MaxValue }
          { Generation.defaults 0 with PairCount = Int32.MaxValue }
          { Generation.defaults 0 with PairCount = 0 }
          { Generation.defaults 0 with MinPathLength = 1 }
          { Generation.defaults 0 with MaxPathLength = 3 }
          { Generation.defaults 0 with MaxAttempts = 0 } ]
    for options in invalid do
        Assert.True(Generation.generate CancellationToken.None options |> Result.isError)

[<Fact>]
let ``search exhaustion is bounded and reported without an impossibility claim`` () =
    let options =
        { Generation.defaults 0 with
            Width = 2; Height = 2; PairCount = 1
            MinPathLength = 4; MaxPathLength = 4; MaxAttempts = 1 }
    match Generation.generate CancellationToken.None options with
    | Ok _ -> failwith "A four-cell path cannot occupy a 2x2 grid without self-contact."
    | Error message -> Assert.Contains("does not prove impossibility", message)

[<Fact>]
let ``generation respects cancellation`` () =
    use cancellation = new CancellationTokenSource()
    cancellation.Cancel()
    Assert.Throws<OperationCanceledException>(fun () -> Generation.generate cancellation.Token (Generation.defaults 0) |> ignore)
    |> ignore

[<Fact>]
let ``export records endpoints ordered solution paths and honest verification status`` () =
    let options = Generation.defaults 42
    let generated = generate options
    let pack = Export.createPack options [| generated |]
    use json = JsonDocument.Parse(Export.toJson pack)
    let root = json.RootElement
    Assert.Equal(1, root.GetProperty("formatVersion").GetInt32())
    Assert.True(root.GetProperty("rules").GetProperty("noSelfTouching").GetBoolean())
    Assert.False(root.GetProperty("rules").GetProperty("requireFullCoverage").GetBoolean())
    let puzzle = root.GetProperty("puzzles")[0]
    Assert.Equal(6, puzzle.GetProperty("pairs").GetArrayLength())
    Assert.Equal(6, puzzle.GetProperty("solution").GetArrayLength())
    Assert.True(puzzle.GetProperty("verification").GetProperty("solutionValid").GetBoolean())
    Assert.Equal("notChecked", puzzle.GetProperty("verification").GetProperty("uniqueness").GetString())
    Assert.Equal("heuristicEstimate", puzzle.GetProperty("verification").GetProperty("difficulty").GetString())

[<Fact>]
let ``invalid witnesses cannot be exported as validated`` () =
    let options = Generation.defaults 0
    let generated = generate options
    let invalid = { generated with Solution = [] }
    Assert.Throws<ArgumentException>(fun () -> Export.createPack options [| invalid |] |> ignore) |> ignore

[<Fact>]
let ``command line handles rectangular boards batches and nonnegative seeds`` () =
    match CommandLine.parse 99 [| "--width"; "7"; "--height"; "5"; "--pairs"; "4"; "--seed"; "10"; "--count"; "3"; "--show-solution" |] with
    | Ok (Generate options) ->
        Assert.Equal(7, options.Generation.Width)
        Assert.Equal(5, options.Generation.Height)
        Assert.Equal(4, options.Generation.PairCount)
        Assert.Equal(10, options.Generation.Seed)
        Assert.Equal(3, options.Count)
        Assert.True(options.ShowSolution)
    | _ -> failwith "Expected a valid generation command."

[<Fact>]
let ``command line rejects malformed and contradictory requests`` () =
    let invalid =
        [ [| "--width" |]
          [| "--width"; "eight" |]
          [| "--width"; "8"; "--width"; "9" |]
          [| "--unknown" |]
          [| "--count"; "0" |]
          [| "--pairs"; "100" |]
          [| "--force" |]
          [| "--output"; "--seed"; "1" |] ]
    for args in invalid do Assert.True(CommandLine.parse 0 args |> Result.isError)

[<Theory>]
[<InlineData(1, 12, 2, 3, 9)>]
[<InlineData(12, 1, 2, 3, 9)>]
[<InlineData(2, 8, 4, 2, 6)>]
[<InlineData(8, 2, 4, 2, 6)>]
[<InlineData(2, 2, 2, 2, 2)>]
[<InlineData(3, 3, 2, 2, 7)>]
[<InlineData(7, 5, 4, 4, 16)>]
[<InlineData(8, 8, 6, 4, 16)>]
[<InlineData(8, 8, 12, 3, 10)>]
[<InlineData(10, 10, 10, 4, 20)>]
[<InlineData(16, 16, 16, 4, 16)>]
let ``full coverage witnesses fill every cell without self contact across seeds`` width height pairs minimum maximum =
    for seed in 0 .. 39 do
        let options =
            { Generation.defaults seed with
                Mode = FullCoverage; Width = width; Height = height; PairCount = pairs
                MinPathLength = minimum; MaxPathLength = maximum }
        let generated = generate options
        independentlyCheck options generated
        Assert.Empty(Validation.solution generated.Puzzle generated.Solution)
        Assert.InRange(generated.Attempts, 1, options.MaxAttempts)

[<Fact>]
let ``full coverage generation is reproducible`` () =
    let options = { Generation.defaults 123 with Mode = FullCoverage }
    let first, second = generate options, generate options
    Assert.Equal(first, second)
    Assert.Equal(Export.toJson (Export.createPack options [| first |]), Export.toJson (Export.createPack options [| second |]))

[<Fact>]
let ``full coverage rejects insufficient capacity and impossible single paths`` () =
    let invalid =
        [ { Generation.defaults 0 with Mode = FullCoverage; Width = 8; Height = 8; PairCount = 3 }
          { Generation.defaults 0 with Mode = FullCoverage; Width = 2; Height = 2; PairCount = 1; MinPathLength = 2 } ]
    for options in invalid do
        Assert.NotEmpty(Generation.validateOptions options)
        Assert.True(Generation.generate CancellationToken.None options |> Result.isError)

[<Fact>]
let ``full coverage bounded search failure is not reported as impossibility`` () =
    let options = { Generation.defaults 0 with Mode = FullCoverage; MaxAttempts = 1 }
    match Generation.generate CancellationToken.None options with
    | Ok _ -> failwith "This one-candidate fixture unexpectedly found a witness."
    | Error message -> Assert.Contains("does not prove impossibility", message)

[<Fact>]
let ``full coverage search respects cancellation`` () =
    use cancellation = new CancellationTokenSource()
    cancellation.Cancel()
    Assert.Throws<OperationCanceledException>(fun () ->
        Generation.generate cancellation.Token { Generation.defaults 0 with Mode = FullCoverage } |> ignore)
    |> ignore

[<Fact>]
let ``coverage rule changes identity and cannot be mislabelled during export`` () =
    let options = { Generation.defaults 42 with Mode = FullCoverage }
    let full = generate options
    let fullPack = Export.createPack options [| full |]
    Assert.True(fullPack.Rules.RequireFullCoverage)
    Assert.Equal("full-coverage", fullPack.Generation.Mode)
    let connect = { full with Puzzle = { full.Puzzle with RequireFullCoverage = false } }
    let connectPack = Export.createPack { options with Mode = ConnectOnly } [| connect |]
    Assert.NotEqual<string>(fullPack.Puzzles[0].Id, connectPack.Puzzles[0].Id)
    Assert.Throws<ArgumentException>(fun () -> Export.createPack options [| connect |] |> ignore) |> ignore
    Assert.Throws<ArgumentException>(fun () -> Export.createPack { options with Mode = ConnectOnly } [| full |] |> ignore) |> ignore
    let incomplete = { full with Solution = full.Solution.Tail }
    Assert.Throws<ArgumentException>(fun () -> Export.createPack options [| incomplete |] |> ignore) |> ignore

[<Fact>]
let ``coverage modes are explicit on the command line with connect only as default`` () =
    let parse args =
        match CommandLine.parse 42 args with
        | Ok (Generate options) -> options.Generation.Mode
        | other -> failwith $"Unexpected command result: {other}"
    Assert.Equal(ConnectOnly, parse [||])
    Assert.Equal(ConnectOnly, parse [| "--mode"; "connect-only" |])
    Assert.Equal(FullCoverage, parse [| "--mode"; "full-coverage" |])
    for args in [ [| "--mode" |]; [| "--mode"; "invalid" |]; [| "--mode"; "connect-only"; "--mode"; "full-coverage" |] ] do
        Assert.True(CommandLine.parse 42 args |> Result.isError)

[<Theory>]
[<InlineData(6, 4, 16, 1.5)>]
[<InlineData(12, 3, 10, 1.0)>]
let ``full coverage mixing avoids a predominantly striped sample`` pairs minimum maximum minimumTurns =
    // Broad fixed-seed regression bounds for layout variety, not human difficulty.
    let turns =
        [ for seed in 0 .. 31 do
              let options =
                  { Generation.defaults seed with Mode = FullCoverage; PairCount = pairs
                                                  MinPathLength = minimum; MaxPathLength = maximum }
              let puzzle = generate options
              independentlyCheck options puzzle
              for path in puzzle.Solution do
                  yield path.Cells |> List.windowed 3 |> List.sumBy (function
                      | [a; b; c] when (b.X - a.X, b.Y - a.Y) <> (c.X - b.X, c.Y - b.Y) -> 1
                      | _ -> 0) ]
    let mean = turns |> List.averageBy float
    Assert.True(mean >= minimumTurns, $"Too few turns per path: {mean}.")
    Assert.True(float (turns |> List.filter ((=) 0) |> List.length) / float turns.Length < 0.35)

[<Fact>]
let ``exports distinguish the new full coverage algorithm and preserve connect only identity`` () =
    let options = Generation.defaults 42
    let connect = Export.createPack options [|generate options|]
    Assert.Equal("random-path-growth-v1", connect.Generation.Algorithm)
    Assert.Equal("tangles-5860d9285de33d0007e8e7b55bfbe3614b052cdcd4336ce589fd368fee142328", connect.Puzzles[0].Id)
    let fullOptions = { options with Mode = FullCoverage }
    let full = Export.createPack fullOptions [|generate fullOptions|]
    Assert.Equal("hamiltonian-path-partition-v3", full.Generation.Algorithm)

[<Theory>]
[<InlineData(1)>]
[<InlineData(63)>]
[<InlineData(64)>]
[<InlineData(127)>]
[<InlineData(128)>]
[<InlineData(129)>]
[<InlineData(256)>]
let ``full coverage respects candidate limits around restart and fallback boundaries`` limit =
    let options = { Generation.defaults 42 with Mode = FullCoverage; MaxAttempts = limit }
    match Generation.generate CancellationToken.None options with
    | Ok generated ->
        independentlyCheck options generated
        Assert.InRange(generated.Attempts, 1, limit)
    | Error message -> Assert.Contains($"after {limit} attempts", message)
