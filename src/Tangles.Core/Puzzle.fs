namespace Tangles.Core

[<Struct>]
type Cell = { X: int; Y: int }

type EndpointPair =
    { Label: string
      Start: Cell
      Finish: Cell }

type Puzzle =
    { Width: int
      Height: int
      RequireFullCoverage: bool
      Pairs: EndpointPair list }

type SolutionPath =
    { Label: string
      Cells: Cell list }

module Cell =
    let isInside width height cell =
        cell.X >= 0 && cell.Y >= 0 && cell.X < width && cell.Y < height

    let areAdjacent a b =
        abs (int64 a.X - int64 b.X) + abs (int64 a.Y - int64 b.Y) = 1L

    let neighbours cell =
        [ { cell with X = cell.X - 1 }
          { cell with X = cell.X + 1 }
          { cell with Y = cell.Y - 1 }
          { cell with Y = cell.Y + 1 } ]
