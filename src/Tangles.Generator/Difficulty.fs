namespace Tangles.Generator

open System
open System.Collections.Generic
open System.Threading
open Tangles.Core

/// Provisional, deterministic estimates from clues alone. No stored solution,
/// random choices, timing, uniqueness assumption or full puzzle search is used.
module Difficulty =
    [<Literal>]
    let Method = "geometric-heuristic-v1"

    [<Literal>]
    let ShortestRouteCountLimit = 1024

    type PairMeasurements =
        { Label: string
          ManhattanDistance: int
          ShortestDistance: int
          ShortestRouteCount: int
          ShortestRouteCountCapped: bool
          MinEndpointExits: int
          MaxEndpointExits: int
          ShortestRouteInteriorCells: int
          SharedRouteInteriorCells: int }

    type Measurements =
        { BoardCellCount: int
          PairCount: int
          EndpointCount: int
          CornerEndpointCount: int
          EdgeEndpointCount: int
          MeanCornerDistance: float
          MeanEdgeDistance: float
          MeanAvailableExits: float
          SingleExitEndpointCount: int
          AdjacentPairCount: int
          AlignedPairCount: int
          MeanManhattanDistance: float
          MeanShortestDistance: float
          MinimumRequiredCells: int
          MinimumCoverageRatio: float
          CoverageSlackCells: int
          RequiredExtraCoverageCells: int
          ForcedCornerTurnCount: int
          MeanLog2ShortestRouteCount: float
          ShortestRouteCountLimit: int
          CappedRoutePairCount: int
          RouteInteriorOccurrences: int
          SharedRouteInteriorOccurrences: int
          RouteOverlapRatio: float
          MaxRouteOverlap: int
          CrossLabelEndpointAdjacencies: int
          ClueCrowdingRatio: float }

    type Component = { Name: string; Value: float; Weight: int; Points: float }
    type Thresholds = { EasyBelow: int; HardAtLeast: int }

    type Assessment =
        { Method: string
          Profile: string
          Mode: string
          Provisional: bool
          Grade: string
          Score: int
          RawScore: float
          Thresholds: Thresholds
          Measurements: Measurements
          PairMeasurements: PairMeasurements array
          Components: Component array }

    let private rounded (value: float) = Math.Round(value, 6, MidpointRounding.AwayFromZero)
    let private fraction (numerator: int) (denominator: int) = if denominator = 0 then 0.0 else float numerator / float denominator
    let private unitValue value = Math.Clamp(value, 0.0, 1.0)

    let assessWithCancellation (cancellation: CancellationToken) (board: Puzzle) =
        cancellation.ThrowIfCancellationRequested()
        if board.Width < 1 || board.Width > 64 || board.Height < 1 || board.Height > 64 then
            invalidArg (nameof board) "Difficulty grading supports board dimensions 1..64."
        match Validation.puzzle board with
        | _ :: _ as errors -> invalidArg (nameof board) (String.concat " " errors)
        | [] -> ()

        let width, height = board.Width, board.Height
        let cells = width * height
        let index (cell: Cell) = cell.Y * width + cell.X
        // Canonical order makes the report independent of pair-list order.
        let pairs = board.Pairs |> List.toArray |> Array.sortWith (fun a b -> StringComparer.Ordinal.Compare(a.Label, b.Label))
        let owners = Array.create cells -1
        let neighbours =
            Array.init cells (fun cell ->
                let x, y = cell % width, cell / width
                [| if x > 0 then cell - 1
                   if x + 1 < width then cell + 1
                   if y > 0 then cell - width
                   if y + 1 < height then cell + width |])
        for pairIndex in 0 .. pairs.Length - 1 do
            owners[index pairs[pairIndex].Start] <- pairIndex
            owners[index pairs[pairIndex].Finish] <- pairIndex

        let endpoints = [| for cell in 0 .. cells - 1 do if owners[cell] >= 0 then cell |]
        let corners = [| 0; width - 1; (height - 1) * width; cells - 1 |] |> Array.distinct
        let cornerDistance cell = min (cell % width) (width - 1 - cell % width) + min (cell / width) (height - 1 - cell / width)
        let edgeDistance cell = min (min (cell % width) (width - 1 - cell % width)) (min (cell / width) (height - 1 - cell / width))
        let isEdge cell = cell % width = 0 || cell % width = width - 1 || cell / width = 0 || cell / width = height - 1
        let isCorner cell = Array.contains cell corners

        // Breadth-first distances and a saturating count of shortest routes.
        // Other pairs' endpoints are obstacles. This does not reserve their
        // future paths, and a shortest route is not necessarily forced globally.
        let distances pairIndex source =
            let distance = Array.create cells -1
            let ways = Array.zeroCreate<int> cells
            let queue = Queue<int>()
            distance[source] <- 0
            ways[source] <- 1
            queue.Enqueue source
            while queue.Count > 0 do
                cancellation.ThrowIfCancellationRequested()
                let current = queue.Dequeue()
                for next in neighbours[current] do
                    if owners[next] < 0 || owners[next] = pairIndex then
                        if distance[next] < 0 then
                            distance[next] <- distance[current] + 1
                            queue.Enqueue next
                        if distance[next] = distance[current] + 1 then
                            ways[next] <- min (ShortestRouteCountLimit + 1) (ways[next] + ways[current])
            distance, ways

        let routeUsage = Array.zeroCreate<int> cells
        let corridors = Array.zeroCreate<int array> pairs.Length
        let stats = Array.zeroCreate<PairMeasurements> pairs.Length
        let mutable singleExits = 0
        let mutable aligned = 0
        for pairIndex in 0 .. pairs.Length - 1 do
            cancellation.ThrowIfCancellationRequested()
            let pair = pairs[pairIndex]
            let start, finish = index pair.Start, index pair.Finish
            let manhattan = abs (pair.Start.X - pair.Finish.X) + abs (pair.Start.Y - pair.Finish.Y)
            let fromStart, ways = distances pairIndex start
            if fromStart[finish] < 0 then
                invalidArg (nameof board) $"Pair {pair.Label} has no route even before other paths are drawn."
            let fromFinish, _ = distances pairIndex finish
            let availableExits endpoint =
                // Adjacent matching endpoints must join directly under the
                // no-self-touching rule, rather than taking a longer detour.
                if manhattan = 1 then 1
                else neighbours[endpoint] |> Array.sumBy (fun next -> if owners[next] < 0 || owners[next] = pairIndex then 1 else 0)
            let a, b = availableExits start, availableExits finish
            singleExits <- singleExits + (if a = 1 then 1 else 0) + (if b = 1 then 1 else 0)
            if pair.Start.X = pair.Finish.X || pair.Start.Y = pair.Finish.Y then aligned <- aligned + 1
            let corridor =
                [| for cell in 0 .. cells - 1 do
                       if owners[cell] < 0 && fromStart[cell] >= 0 && fromFinish[cell] >= 0
                          && fromStart[cell] + fromFinish[cell] = fromStart[finish] then
                           yield cell |]
            corridors[pairIndex] <- corridor
            for cell in corridor do routeUsage[cell] <- routeUsage[cell] + 1
            stats[pairIndex] <-
                { Label = pair.Label; ManhattanDistance = manhattan; ShortestDistance = fromStart[finish]
                  ShortestRouteCount = min ShortestRouteCountLimit ways[finish]
                  ShortestRouteCountCapped = ways[finish] > ShortestRouteCountLimit
                  MinEndpointExits = min a b; MaxEndpointExits = max a b
                  ShortestRouteInteriorCells = corridor.Length; SharedRouteInteriorCells = 0 }

        for pairIndex in 0 .. pairs.Length - 1 do
            let shared = corridors[pairIndex] |> Array.sumBy (fun cell -> if routeUsage[cell] > 1 then 1 else 0)
            stats[pairIndex] <- { stats[pairIndex] with SharedRouteInteriorCells = shared }
        let occurrences = stats |> Array.sumBy _.ShortestRouteInteriorCells
        let sharedOccurrences = stats |> Array.sumBy _.SharedRouteInteriorCells
        let minimumCells = stats |> Array.sumBy (fun pair -> pair.ShortestDistance + 1)
        let slack = cells - minimumCells
        let crossAdjacencies =
            endpoints |> Array.sumBy (fun cell ->
                neighbours[cell] |> Array.sumBy (fun next -> if next > cell && owners[next] >= 0 && owners[next] <> owners[cell] then 1 else 0))
        let cornerTurns =
            if board.RequireFullCoverage && width > 1 && height > 1 then
                corners |> Array.sumBy (fun cell -> if owners[cell] < 0 then 1 else 0)
            else 0
        let measurements =
            { BoardCellCount = cells; PairCount = pairs.Length; EndpointCount = endpoints.Length
              CornerEndpointCount = endpoints |> Array.filter isCorner |> Array.length
              EdgeEndpointCount = endpoints |> Array.filter (fun cell -> isEdge cell && not (isCorner cell)) |> Array.length
              MeanCornerDistance = endpoints |> Array.averageBy (cornerDistance >> float) |> rounded
              MeanEdgeDistance = endpoints |> Array.averageBy (edgeDistance >> float) |> rounded
              MeanAvailableExits = stats |> Array.averageBy (fun pair -> float (pair.MinEndpointExits + pair.MaxEndpointExits) / 2.0) |> rounded
              SingleExitEndpointCount = singleExits
              AdjacentPairCount = stats |> Array.filter (fun pair -> pair.ManhattanDistance = 1) |> Array.length
              AlignedPairCount = aligned
              MeanManhattanDistance = stats |> Array.averageBy (fun pair -> float pair.ManhattanDistance) |> rounded
              MeanShortestDistance = stats |> Array.averageBy (fun pair -> float pair.ShortestDistance) |> rounded
              MinimumRequiredCells = minimumCells; MinimumCoverageRatio = fraction minimumCells cells |> rounded
              CoverageSlackCells = slack
              RequiredExtraCoverageCells = if board.RequireFullCoverage then max 0 slack else 0
              ForcedCornerTurnCount = cornerTurns
              MeanLog2ShortestRouteCount = stats |> Array.map (fun pair -> Math.Log2(float pair.ShortestRouteCount)) |> Array.sort |> Array.average |> rounded
              ShortestRouteCountLimit = ShortestRouteCountLimit
              CappedRoutePairCount = stats |> Array.filter _.ShortestRouteCountCapped |> Array.length
              RouteInteriorOccurrences = occurrences; SharedRouteInteriorOccurrences = sharedOccurrences
              RouteOverlapRatio = fraction sharedOccurrences occurrences |> rounded
              MaxRouteOverlap = Array.max routeUsage
              CrossLabelEndpointAdjacencies = crossAdjacencies
              ClueCrowdingRatio = fraction (2 * crossAdjacencies) (4 * endpoints.Length) |> rounded }

        // Explicit, hand-set profiles. Weights and cutoffs are versioned and
        // provisional: these are not calibrated human difficulty measurements.
        let common =
            [| "boardScale", Math.Log2(float cells) / 12.0
               "pairLoad", float pairs.Length / 16.0
               "endpointFreedom", (measurements.MeanAvailableExits - 1.0) / 3.0
               "shortRouteAmbiguity", measurements.MeanLog2ShortestRouteCount / Math.Log2(float ShortestRouteCountLimit)
               "routeOverlap", measurements.RouteOverlapRatio
               "clueCrowding", measurements.ClueCrowdingRatio
               "cornerDistance", measurements.MeanCornerDistance / float (max 1 ((width - 1) / 2 + (height - 1) / 2)) |]
        let mode, thresholds, weighted =
            if board.RequireFullCoverage then
                "full-coverage", { EasyBelow = 35; HardAtLeast = 60 },
                Array.append (Array.map2 (fun (name, value) weight -> name, value, weight) common [| 10; 8; 16; 8; 24; 6; 6 |])
                    [| "requiredCoverageDetours", fraction (max 0 slack) cells, 22
                       "forcedEndpointCredit", fraction singleExits endpoints.Length, -18
                       "forcedCornerCredit", fraction cornerTurns 4, -6 |]
            else
                "connect-only", { EasyBelow = 30; HardAtLeast = 55 },
                Array.append (Array.map2 (fun (name, value) weight -> name, value, weight) common [| 10; 10; 18; 12; 22; 6; 6 |])
                    [| "minimumCoveragePressure", measurements.MinimumCoverageRatio, 16
                       "forcedEndpointCredit", fraction singleExits endpoints.Length, -20 |]
        let components =
            weighted |> Array.map (fun (name, value, weight) ->
                let normalised = value |> unitValue |> rounded
                { Name = name; Value = normalised; Weight = weight; Points = float weight * normalised |> rounded })
        let rawScore = components |> Array.sumBy _.Points |> rounded
        let score = Math.Clamp(int (Math.Round(rawScore, MidpointRounding.AwayFromZero)), 0, 100)
        { Method = Method; Profile = mode + "-v1"; Mode = mode; Provisional = true
          Grade = if score < thresholds.EasyBelow then "easy" elif score < thresholds.HardAtLeast then "medium" else "hard"
          Score = score; RawScore = rawScore; Thresholds = thresholds
          Measurements = measurements; PairMeasurements = stats; Components = components }

    let assess board = assessWithCancellation CancellationToken.None board
