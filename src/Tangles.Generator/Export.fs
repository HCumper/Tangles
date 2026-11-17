namespace Tangles.Generator

open System
open System.IO
open System.Security.Cryptography
open System.Text
open System.Text.Json
open System.Threading
open Tangles.Core

module Export =
    type Rules =
        { OrthogonalOnly: bool
          NoSharedCells: bool
          NoSelfTouching: bool
          RequireFullCoverage: bool }

    type PathDocument = { Label: string; Cells: Cell array }

    type Verification =
        { SolutionValid: bool
          Uniqueness: string
          Difficulty: string }

    type PuzzleDocument =
        { Id: string
          Width: int
          Height: int
          Seed: int
          Attempts: int
          Pairs: EndpointPair array
          Solution: PathDocument array
          Difficulty: Difficulty.Assessment
          Verification: Verification }

    type GenerationSettings =
        { Algorithm: string
          Mode: string
          Seed: int
          Width: int
          Height: int
          PairCount: int
          MinPathLength: int
          MaxPathLength: int
          MaxAttempts: int }

    type PuzzlePack =
        { FormatVersion: int
          Rules: Rules
          Generation: GenerationSettings
          Puzzles: PuzzleDocument array }

    let private jsonOptions = JsonSerializerOptions(PropertyNamingPolicy = JsonNamingPolicy.CamelCase, WriteIndented = true)

    let private puzzleId (board: Puzzle) =
        let pairs =
            board.Pairs
            |> List.map (fun p -> $"{p.Label}:{p.Start.X},{p.Start.Y}:{p.Finish.X},{p.Finish.Y}")
            |> String.concat ";"

        // Keep existing connect-only IDs; separate full-coverage progress/identity.
        let rule = if board.RequireFullCoverage then ":full-coverage" else ""
        let canonical = $"tangles-v1{rule}:{board.Width}x{board.Height}:{pairs}"
        let hash = SHA256.HashData(Encoding.UTF8.GetBytes canonical)
        "tangles-" + Convert.ToHexString(hash).ToLowerInvariant()

    let createPackWithCancellation (cancellation: CancellationToken) (options: GenerationOptions) (generated: GeneratedPuzzle array) =
        cancellation.ThrowIfCancellationRequested()
        for item in generated do
            cancellation.ThrowIfCancellationRequested()
            if item.Puzzle.RequireFullCoverage <> (options.Mode = FullCoverage) then
                invalidArg (nameof generated) "Puzzle coverage must match the generation mode of the pack."
            match Validation.solution item.Puzzle item.Solution with
            | [] -> ()
            | errors -> invalidArg (nameof generated) (String.concat " " errors)

        { FormatVersion = 1
          Rules =
            { OrthogonalOnly = true
              NoSharedCells = true
              NoSelfTouching = true
              RequireFullCoverage = options.Mode = FullCoverage }
          Generation =
            { Algorithm = if options.Mode = FullCoverage then "hamiltonian-path-partition-v1" else "random-path-growth-v1"
              Mode = if options.Mode = FullCoverage then "full-coverage" else "connect-only"
              Seed = options.Seed
              Width = options.Width
              Height = options.Height
              PairCount = options.PairCount
              MinPathLength = options.MinPathLength
              MaxPathLength = options.MaxPathLength
              MaxAttempts = options.MaxAttempts }
          Puzzles =
            generated
            |> Array.map (fun item ->
                { Id = puzzleId item.Puzzle
                  Width = item.Puzzle.Width
                  Height = item.Puzzle.Height
                  Seed = item.Seed
                  Attempts = item.Attempts
                  Pairs = Array.ofList item.Puzzle.Pairs
                  Solution = item.Solution |> List.map (fun p -> { Label = p.Label; Cells = Array.ofList p.Cells }) |> Array.ofList
                  Difficulty = Difficulty.assessWithCancellation cancellation item.Puzzle
                  Verification = { SolutionValid = true; Uniqueness = "notChecked"; Difficulty = "heuristicEstimate" } }) }

    let createPack options generated = createPackWithCancellation CancellationToken.None options generated

    let toJson pack = JsonSerializer.Serialize<PuzzlePack>(pack, jsonOptions)

    /// Write a complete pack, never a partially generated batch. Existing files
    /// are preserved unless overwrite was explicitly requested.
    let writeFile path overwrite pack =
        let destination = Path.GetFullPath path
        let directory = Path.GetDirectoryName destination
        Directory.CreateDirectory directory |> ignore
        let temporary = Path.Combine(directory, $".tangles-{Guid.NewGuid():N}.tmp")

        try
            File.WriteAllText(temporary, toJson pack, UTF8Encoding(false))
            File.Move(temporary, destination, overwrite)
        finally
            if File.Exists temporary then File.Delete temporary

    let printGrid title width height entries =
        let cells = Array2D.create height width "."
        let mutable labelWidth = 1

        for (cell: Cell), label in entries do
            cells[cell.Y, cell.X] <- label
            labelWidth <- max labelWidth label.Length

        printfn "%s" title

        for y in 0 .. height - 1 do
            [ for x in 0 .. width - 1 -> cells[y, x].PadLeft labelWidth ]
            |> String.concat " "
            |> printfn "%s"

    let printPuzzleWithCancellation cancellation showSolution (generated: GeneratedPuzzle) =
        let board = generated.Puzzle
        let difficulty = Difficulty.assessWithCancellation cancellation board
        printfn "\n%dx%d, %d pairs, seed %d, %d attempt(s)" board.Width board.Height board.Pairs.Length generated.Seed generated.Attempts
        printfn "Mode: %s" (if board.RequireFullCoverage then "full-coverage (every square must be filled)" else "connect-only (unused squares allowed)")
        printfn "Difficulty: %s (%d/100; provisional heuristic)" difficulty.Grade difficulty.Score
        let endpoints = board.Pairs |> Seq.collect (fun p -> [ p.Start, p.Label; p.Finish, p.Label ])
        printGrid "Puzzle (. = empty):" board.Width board.Height endpoints

        if showSolution then
            let entries = generated.Solution |> Seq.collect (fun p -> p.Cells |> Seq.map (fun cell -> cell, p.Label))
            printGrid "Known solution (. = unused):" board.Width board.Height entries

    let printPuzzle showSolution generated = printPuzzleWithCancellation CancellationToken.None showSolution generated
