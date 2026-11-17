module Tangles.DifficultyTests

open System
open System.Text.Json
open System.Text.Json.Nodes
open System.Threading
open Tangles.Core
open Tangles.Generator
open Xunit

let private c x y = { X = x; Y = y }
let private pair label start finish = { Label = label; Start = start; Finish = finish }
let private board width height pairs =
    { Width = width; Height = height; RequireFullCoverage = false; Pairs = pairs }
let private require = function Ok value -> value | Error error -> failwith error

[<Fact>]
let ``diagonal endpoints have six shortest routes on a three by three board`` () =
    let result = board 3 3 [ pair "A" (c 0 0) (c 2 2) ] |> Difficulty.assess
    let m = result.Measurements
    Assert.Equal(9, m.BoardCellCount)
    Assert.Equal(2, m.CornerEndpointCount)
    Assert.Equal(0, m.EdgeEndpointCount)
    Assert.Equal(0.0, m.MeanCornerDistance)
    Assert.Equal(2.0, m.MeanAvailableExits)
    Assert.Equal(5, m.MinimumRequiredCells)
    Assert.Equal(4, m.CoverageSlackCells)
    Assert.Equal(4, result.PairMeasurements[0].ShortestDistance)
    Assert.Equal(6, result.PairMeasurements[0].ShortestRouteCount)
    Assert.False(result.PairMeasurements[0].ShortestRouteCountCapped)
    Assert.Equal(7, result.PairMeasurements[0].ShortestRouteInteriorCells)
    Assert.Equal(0.0, m.RouteOverlapRatio)

[<Fact>]
let ``short route competition counts shared candidate cells rather than a stored solution`` () =
    let puzzle = board 5 5 [ pair "A" (c 0 1) (c 3 3); pair "B" (c 1 0) (c 4 2) ]
    // A can go down the left edge then right, while B goes right then down.
    // Both pairs also have shortest-route alternatives through six shared cells.
    let result = Difficulty.assess puzzle
    Assert.All(result.PairMeasurements, fun p ->
        Assert.Equal(10, p.ShortestRouteCount)
        Assert.Equal(10, p.ShortestRouteInteriorCells)
        Assert.Equal(6, p.SharedRouteInteriorCells))
    Assert.Equal(20, result.Measurements.RouteInteriorOccurrences)
    Assert.Equal(12, result.Measurements.SharedRouteInteriorOccurrences)
    Assert.Equal(0.6, result.Measurements.RouteOverlapRatio)
    Assert.Equal(2, result.Measurements.MaxRouteOverlap)
    Assert.Equal(3, result.Measurements.EdgeEndpointCount)
    Assert.Equal(0, result.Measurements.CornerEndpointCount)
    Assert.Equal(1.5, result.Measurements.MeanCornerDistance)
    Assert.Equal(0.25, result.Measurements.MeanEdgeDistance)

[<Fact>]
let ``adjacent mates count as a forced connection despite other empty neighbours`` () =
    let result = board 3 3 [ pair "A" (c 1 1) (c 2 1) ] |> Difficulty.assess
    Assert.Equal(1, result.Measurements.AdjacentPairCount)
    Assert.Equal(2, result.Measurements.SingleExitEndpointCount)
    Assert.Equal(1.0, result.Measurements.MeanAvailableExits)
    Assert.Equal("easy", result.Grade)

[<Fact>]
let ``other clues can force exits even for nonadjacent mates`` () =
    let result =
        board 2 3 [ pair "A" (c 0 0) (c 0 2); pair "B" (c 1 0) (c 1 2) ]
        |> Difficulty.assess
    Assert.Equal(0, result.Measurements.AdjacentPairCount)
    Assert.Equal(4, result.Measurements.SingleExitEndpointCount)
    Assert.Equal(6, result.Measurements.MinimumRequiredCells)
    Assert.Equal(0, result.Measurements.CoverageSlackCells)
    Assert.Equal("easy", result.Grade)

[<Fact>]
let ``shortest route counts saturate and identify the cap`` () =
    let under = board 7 7 [ pair "A" (c 0 0) (c 6 6) ] |> Difficulty.assess
    Assert.Equal(924, under.PairMeasurements[0].ShortestRouteCount)
    Assert.False(under.PairMeasurements[0].ShortestRouteCountCapped)
    let over = board 8 8 [ pair "A" (c 0 0) (c 7 7) ] |> Difficulty.assess
    Assert.Equal(1024, over.PairMeasurements[0].ShortestRouteCount)
    Assert.True(over.PairMeasurements[0].ShortestRouteCountCapped)
    Assert.Equal(1, over.Measurements.CappedRoutePairCount)

[<Fact>]
let ``reports are invariant under pair order reversal reflection and rotation`` () =
    let puzzle = board 7 5 [ pair "A" (c 0 1) (c 3 3); pair "B" (c 1 0) (c 6 2) ]
    let original = Difficulty.assess puzzle
    let reordered = { puzzle with Pairs = puzzle.Pairs |> List.rev |> List.map (fun p -> { p with Start = p.Finish; Finish = p.Start }) }
    Assert.Equal(original, Difficulty.assess reordered)
    let transform width height change =
        { puzzle with Width = width; Height = height
                      Pairs = puzzle.Pairs |> List.map (fun p -> { p with Start = change p.Start; Finish = change p.Finish }) }
    let reflected = transform puzzle.Width puzzle.Height (fun cell -> c (puzzle.Width - 1 - cell.X) cell.Y)
    let rotated = transform puzzle.Height puzzle.Width (fun cell -> c (puzzle.Height - 1 - cell.Y) cell.X)
    Assert.Equal(original, Difficulty.assess reflected)
    Assert.Equal(original, Difficulty.assess rotated)

[<Fact>]
let ``coverage modes use different profiles and only full coverage credits empty corners`` () =
    let generated = Generation.generate CancellationToken.None { Generation.defaults 42 with Mode = FullCoverage } |> require
    let full = Difficulty.assess generated.Puzzle
    let connect = Difficulty.assess { generated.Puzzle with RequireFullCoverage = false }
    Assert.Equal("full-coverage-v1", full.Profile)
    Assert.Equal("connect-only-v1", connect.Profile)
    Assert.Equal(0, connect.Measurements.RequiredExtraCoverageCells)
    Assert.Equal(0, connect.Measurements.ForcedCornerTurnCount)
    Assert.True(full.Measurements.RequiredExtraCoverageCells > 0)
    Assert.Equal(2, full.Measurements.ForcedCornerTurnCount)
    let names (report: Difficulty.Assessment) = report.Components |> Array.map _.Name
    Assert.Contains("requiredCoverageDetours", names full)
    Assert.Contains("forcedCornerCredit", names full)
    Assert.DoesNotContain("minimumCoveragePressure", names full)
    Assert.Contains("minimumCoveragePressure", names connect)
    Assert.DoesNotContain("requiredCoverageDetours", names connect)
    Assert.DoesNotContain("forcedCornerCredit", names connect)

[<Fact>]
let ``different valid witnesses for identical clues receive identical grades`` () =
    let puzzle = board 4 4 [ pair "A" (c 0 0) (c 2 1) ]
    let short = [ c 0 0; c 1 0; c 2 0; c 2 1 ]
    let long = [ c 0 0; c 0 1; c 0 2; c 1 2; c 2 2; c 2 1 ]
    let options = { Generation.defaults 0 with Width = 4; Height = 4; PairCount = 1 }
    let export cells =
        let generated = { Puzzle = puzzle; Solution = [ { Label = "A"; Cells = cells } ]; Seed = 0; Attempts = 1 }
        Export.createPack options [| generated |]
    let first, second = export short, export long
    Assert.Equal(first.Puzzles[0].Id, second.Puzzles[0].Id)
    Assert.Equal(first.Puzzles[0].Difficulty, second.Puzzles[0].Difficulty)

[<Fact>]
let ``a valid layout with many competing short routes reaches the hard score band`` () =
    let puzzle = board 8 8 [ pair "A" (c 1 1) (c 6 6); pair "B" (c 1 6) (c 6 1) ]
    let a =
        [ yield c 1 1
          for y in 1 .. 7 do yield c 0 y
          for x in 1 .. 6 do yield c x 7
          yield c 6 6 ]
    let b =
        [ for y in 6 .. -1 .. 2 do yield c 1 y
          for x in 2 .. 6 do yield c x 2
          yield c 6 1 ]
    Assert.Empty(Validation.solution puzzle [ { Label = "A"; Cells = a }; { Label = "B"; Cells = b } ])
    let result = Difficulty.assess puzzle
    Assert.Equal(1.0, result.Measurements.RouteOverlapRatio)
    Assert.All(result.PairMeasurements, fun p -> Assert.Equal(250, p.ShortestRouteCount))
    Assert.Equal(63, result.Score)
    Assert.Equal("hard", result.Grade)

[<Theory>]
[<InlineData(false)>]
[<InlineData(true)>]
let ``JSON contains an auditable provisional assessment and remains readable by the player`` fullCoverage =
    let options = { Generation.defaults 42 with Mode = if fullCoverage then FullCoverage else ConnectOnly }
    let generated = Generation.generate CancellationToken.None options |> require
    let pack = Export.createPack options [| generated |]
    use json = JsonDocument.Parse(Export.toJson pack)
    let puzzle = json.RootElement.GetProperty("puzzles")[0]
    let report = puzzle.GetProperty("difficulty")
    Assert.Equal("geometric-heuristic-v1", report.GetProperty("method").GetString())
    Assert.True(report.GetProperty("provisional").GetBoolean())
    Assert.Equal((if fullCoverage then "full-coverage" else "connect-only"), report.GetProperty("mode").GetString())
    Assert.Equal(64, report.GetProperty("measurements").GetProperty("boardCellCount").GetInt32())
    Assert.Equal(6, report.GetProperty("pairMeasurements").GetArrayLength())
    let points = report.GetProperty("components").EnumerateArray() |> Seq.sumBy (fun c -> c.GetProperty("points").GetDouble())
    Assert.Equal(points, report.GetProperty("rawScore").GetDouble(), 5)
    Assert.InRange(report.GetProperty("score").GetInt32(), 0, 100)
    Assert.Contains(report.GetProperty("grade").GetString(), [| "easy"; "medium"; "hard" |])
    Assert.True(PuzzlePacks.read (Export.toJson pack) |> Result.isOk)
    let legacy = JsonNode.Parse(Export.toJson pack)
    let legacyPuzzle = legacy["puzzles"].AsArray().[0]
    legacyPuzzle.AsObject().Remove("difficulty") |> ignore
    let legacyVerification = legacyPuzzle["verification"]
    legacyVerification["difficulty"] <- JsonValue.Create("notAssessed")
    Assert.True(PuzzlePacks.read (legacy.ToJsonString()) |> Result.isOk)
    if not fullCoverage then
        Assert.Equal("tangles-5860d9285de33d0007e8e7b55bfbe3614b052cdcd4336ce589fd368fee142328", pack.Puzzles[0].Id)

[<Theory>]
[<InlineData(1, 12)>]
[<InlineData(12, 1)>]
let ``one dimensional boards have finite scores and no full coverage corner turn credit`` width height =
    let puzzle = { board width height [ pair "A" (c 0 0) (c (width - 1) (height - 1)) ] with RequireFullCoverage = true }
    let result = Difficulty.assess puzzle
    Assert.Equal("easy", result.Grade)
    Assert.Equal(0, result.Measurements.ForcedCornerTurnCount)
    Assert.Equal(0, result.Measurements.RequiredExtraCoverageCells)
    Assert.All(result.Components, fun c -> Assert.True(Double.IsFinite c.Points))

[<Fact>]
let ``invalid clue layouts and isolated endpoints cannot be graded`` () =
    let invalid =
        [ board 0 3 [ pair "A" (c 0 0) (c 0 1) ]
          board 65 1 [ pair "A" (c 0 0) (c 1 0) ]
          board 3 3 []
          board 3 3 [ pair "A" (c 0 0) (c 0 0) ]
          board 3 3 [ pair "A" (c 0 0) (c 2 2); pair "B" (c 1 0) (c 0 1) ] ]
    for puzzle in invalid do
        Assert.Throws<ArgumentException>(fun () -> Difficulty.assess puzzle |> ignore) |> ignore

[<Fact>]
let ``grading and export respect cancellation`` () =
    use cancellation = new CancellationTokenSource()
    cancellation.Cancel()
    let puzzle = board 3 3 [ pair "A" (c 0 0) (c 2 2) ]
    Assert.Throws<OperationCanceledException>(fun () -> Difficulty.assessWithCancellation cancellation.Token puzzle |> ignore) |> ignore
    Assert.Throws<OperationCanceledException>(fun () -> Export.createPackWithCancellation cancellation.Token (Generation.defaults 0) [||] |> ignore) |> ignore
