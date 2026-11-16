namespace Tangles.Core

open System
open System.Collections.Generic

module Validation =
    /// Validate the clues without making any claim about solvability.
    let puzzle (board: Puzzle) =
        let errors = ResizeArray<string>()

        if board.Width < 1 || board.Height < 1 then
            errors.Add "Board dimensions must be positive."

        if List.isEmpty board.Pairs then
            errors.Add "At least one endpoint pair is required."

        let labels = HashSet<string>()
        let endpoints = HashSet<Cell>()

        for pair in board.Pairs do
            if String.IsNullOrWhiteSpace pair.Label then
                errors.Add "Pair labels must not be empty."

            if not (labels.Add pair.Label) then
                errors.Add $"Duplicate pair label: {pair.Label}."

            if pair.Start = pair.Finish then
                errors.Add $"Pair {pair.Label} needs two distinct endpoints."

            for cell in [ pair.Start; pair.Finish ] do
                if not (Cell.isInside board.Width board.Height cell) then
                    errors.Add $"An endpoint of pair {pair.Label} is outside the board."

                if not (endpoints.Add cell) then
                    errors.Add "Endpoint cells must not be shared."

        List.ofSeq errors

    /// Check an explicit solution under the connect-only, no-self-touching rules.
    /// An empty result means valid. Unused cells and either path orientation are allowed.
    let solution (board: Puzzle) (paths: SolutionPath list) =
        let errors = ResizeArray<string>(puzzle board)
        let pairs = board.Pairs |> List.map (fun p -> p.Label, p) |> Map.ofList
        let labels = HashSet<string>()
        let occupied = HashSet<Cell>()

        for path in paths do
            if not (labels.Add path.Label) then
                errors.Add $"Duplicate solution path: {path.Label}."

            match Map.tryFind path.Label pairs with
            | None -> errors.Add $"Unknown solution label: {path.Label}."
            | Some pair ->
                match path.Cells with
                | [] | [ _ ] -> errors.Add $"Path {path.Label} needs at least two cells."
                | cells ->
                    let first, last = List.head cells, List.last cells

                    if not ((first = pair.Start && last = pair.Finish)
                            || (first = pair.Finish && last = pair.Start)) then
                        errors.Add $"Path {path.Label} does not join its matching endpoints."

            let positions = Dictionary<Cell, int>()

            for index, cell in List.indexed path.Cells do
                if not (Cell.isInside board.Width board.Height cell) then
                    errors.Add $"Path {path.Label} leaves the board."

                if not (positions.TryAdd(cell, index)) then
                    errors.Add $"Path {path.Label} revisits a cell."

                if not (occupied.Add cell) then
                    errors.Add "Solution paths must not share or revisit cells."

            for a, b in List.pairwise path.Cells do
                if not (Cell.areAdjacent a b) then
                    errors.Add $"Path {path.Label} contains a non-orthogonal step."

            for KeyValue(cell, index) in positions do
                for neighbour in Cell.neighbours cell do
                    match positions.TryGetValue neighbour with
                    | true, other when abs (index - other) <> 1 ->
                        errors.Add $"Path {path.Label} touches itself."
                    | _ -> ()

        for pair in board.Pairs do
            if not (labels.Contains pair.Label) then
                errors.Add $"Missing solution path: {pair.Label}."

        errors |> Seq.distinct |> List.ofSeq
