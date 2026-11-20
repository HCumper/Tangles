namespace Tangles.Generator

open Tangles.Core

/// Selection identity only: never substitute this for an assessment/progress key.
module PuzzleSymmetry =
    let key (board: Puzzle) =
        let w, h = board.Width, board.Height
        let transforms =
            [ yield fun (c: Cell) -> c
              yield fun c -> { X = w-1-c.X; Y = c.Y }
              yield fun c -> { X = c.X; Y = h-1-c.Y }
              yield fun c -> { X = w-1-c.X; Y = h-1-c.Y }
              if w = h then
                  yield fun c -> { X = c.Y; Y = c.X }
                  yield fun c -> { X = w-1-c.Y; Y = c.X }
                  yield fun c -> { X = c.Y; Y = h-1-c.X }
                  yield fun c -> { X = w-1-c.Y; Y = h-1-c.X } ]
        transforms
        |> List.map (fun transform ->
            let pairs = board.Pairs |> List.map (fun pair ->
                { pair with Start = transform pair.Start; Finish = transform pair.Finish })
            Solver.boardKey { board with Pairs = pairs })
        |> List.min
