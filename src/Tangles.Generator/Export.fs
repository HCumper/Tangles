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

    type PuzzleRecipe =
        { PairCount: int; MinPathLength: int; MaxPathLength: int; MaxAttempts: int; CutStyle: string }

    type RampSectionDocument = { Target: string; Count: int; Recipe: PuzzleRecipe }

    type BatchStatistics =
        { Requests: int; RejectedSearches: int; RejectedDuplicates: int
          RequestLimit: int option; CandidateBudget: int option; CandidatesConsidered: int }

    type PuzzleDocument =
        { Id: string
          Width: int
          Height: int
          Seed: int
          Attempts: int
          Work: GenerationWork
          Recipe: PuzzleRecipe option
          Pairs: EndpointPair array
          Solution: PathDocument array
          Difficulty: Difficulty.Assessment
          Selection: GenerationPolicy.Statistics option
          ShortestRoutes: Solver.ShortestRouteScreen option
          Verification: Verification }

    type SelectionSettings =
        { PolicyVersion: string; Target: string; CandidateLimit: int; ImprovementLimit: int; SolverWorkLimit: int
          MinTrials: int; MaxTrials: int option; MinBranchDepth: int; MinFirstSolutionNodes: int
          Assess: bool; RequireUnique: bool; AvoidShortest: bool
          BatchRequests: int option; RejectedSearches: int option; RejectedDuplicates: int option
          BatchCandidateBudget: int option; BatchCandidatesConsidered: int option }

    type GenerationSettings =
        { Algorithm: string
          Mode: string
          Seed: int
          BatchSeedAlgorithm: string
          Deduplication: string
          Width: int
          Height: int
          PairCount: int option
          MinPathLength: int option
          MaxPathLength: int option
          MaxAttempts: int option
          CutStyle: string
          Ramp: string option
          RampSections: RampSectionDocument array option
          Batch: BatchStatistics option
          Selection: SelectionSettings option }

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

    type internal PreparedPuzzle = { Generated: GeneratedPuzzle; Difficulty: Difficulty.Assessment }

    let internal prepareWithCancellation cancellation (generated: GeneratedPuzzle) =
        let geometry = Difficulty.assessWithCancellation cancellation generated.Puzzle
        let reasoning = generated.Quality |> Option.bind _.Reasoning
        let difficulty = reasoning |> Option.map (fun a -> Difficulty.withReasoning a geometry) |> Option.defaultValue geometry
        { Generated = generated; Difficulty = difficulty }

    let internal createPreparedPackWithCancellation (cancellation: CancellationToken) (options: GenerationOptions) (prepared: PreparedPuzzle array) =
        cancellation.ThrowIfCancellationRequested()
        let generated = prepared |> Array.map _.Generated
        let settings (quality: GenerationPolicy.Report option) =
            quality |> Option.map (fun q ->
                { PolicyVersion = q.Selection.PolicyVersion; Target = q.Selection.Target; CandidateLimit = q.Selection.CandidateLimit; ImprovementLimit = q.Selection.ImprovementLimit
                  SolverWorkLimit = q.Selection.SolverWorkLimit; Assess = q.Selection.Assess; RequireUnique = q.Selection.RequireUnique; AvoidShortest = q.Selection.AvoidShortest
                  MinTrials = q.Selection.MinTrials; MaxTrials = q.Selection.MaxTrials; MinBranchDepth = q.Selection.MinBranchDepth; MinFirstSolutionNodes = q.Selection.MinFirstSolutionNodes
                  BatchRequests = None; RejectedSearches = None; RejectedDuplicates = None
                  BatchCandidateBudget = None; BatchCandidatesConsidered = None })
        let selection = generated |> Array.tryHead |> Option.bind (fun p -> settings p.Quality)
        let algorithm (item: GeneratedPuzzle) =
            item.Quality |> Option.map (fun q -> q.Selection.Algorithm)
            |> Option.defaultValue (Generation.algorithm options)
        let algorithmName = generated |> Array.tryHead |> Option.map algorithm |> Option.defaultValue (Generation.algorithm options)
        for item in generated do
            cancellation.ThrowIfCancellationRequested()
            if item.Puzzle.RequireFullCoverage <> (options.Mode = FullCoverage) then
                invalidArg (nameof generated) "Puzzle coverage must match the generation mode of the pack."
            match Validation.solution item.Puzzle item.Solution with
            | [] -> ()
            | errors -> invalidArg (nameof generated) (String.concat " " errors)
            if settings item.Quality <> selection || algorithm item <> algorithmName then
                invalidArg (nameof generated) "All puzzles in a pack must use the same selection policy and generation algorithm."
            match item.Quality |> Option.bind _.Reasoning with
            | Some report when report.BoardKey <> Solver.boardKey item.Puzzle ->
                invalidArg (nameof generated) "Solver assessment belongs to different clues."
            | _ -> ()

        { FormatVersion = 1
          Rules =
            { OrthogonalOnly = true
              NoSharedCells = true
              NoSelfTouching = true
              RequireFullCoverage = options.Mode = FullCoverage }
          Generation =
            { Algorithm = algorithmName
              Mode = if options.Mode = FullCoverage then "full-coverage" else "connect-only"
              Seed = options.Seed
              BatchSeedAlgorithm = SeedStreams.batchAlgorithm
              Deduplication = "board-symmetry-v1"
              Width = options.Width
              Height = options.Height
              PairCount = Some options.PairCount
              MinPathLength = Some options.MinPathLength
              MaxPathLength = Some options.MaxPathLength
              MaxAttempts = Some options.MaxAttempts
              CutStyle = CutStyle.name options.CutStyle
              Ramp = None; RampSections = None; Batch = None
              Selection = selection }
          Puzzles =
            prepared
            |> Array.map (fun entry ->
                let item, difficulty = entry.Generated, entry.Difficulty
                let reasoning = item.Quality |> Option.bind _.Reasoning
                { Id = puzzleId item.Puzzle
                  Width = item.Puzzle.Width
                  Height = item.Puzzle.Height
                  Seed = item.Seed
                  Attempts = item.Attempts
                  Work = item.Work
                  Recipe = None
                  Pairs = Array.ofList item.Puzzle.Pairs
                  Solution = item.Solution |> List.map (fun p -> { Label = p.Label; Cells = Array.ofList p.Cells }) |> Array.ofList
                  Difficulty = difficulty
                  Selection = item.Quality |> Option.map _.Selection
                  ShortestRoutes = item.Quality |> Option.bind _.ShortestRoutes
                  Verification =
                    { SolutionValid = true
                      Uniqueness = reasoning |> Option.map _.Uniqueness |> Option.defaultValue "notChecked"
                      Difficulty = if difficulty.Method = "cell-domain-reasoning-v1" then "reasoningEstimate" else "heuristicEstimate" } }) }

    let private recipeDocument (options: GenerationOptions) =
        { PairCount = options.PairCount; MinPathLength = options.MinPathLength
          MaxPathLength = options.MaxPathLength; MaxAttempts = options.MaxAttempts
          CutStyle = CutStyle.name options.CutStyle }

    let internal createRampPackWithCancellation cancellation (options: GenerationOptions) spec (sections: RampSection array) (entries: (GenerationOptions * PreparedPuzzle) array) =
        if entries.Length = 0 then invalidOp "A ramp must contain puzzles."
        let expected = sections |> Array.collect (fun section -> Array.create section.Count section)
        if entries.Length <> expected.Length then invalidOp "Ramp counts do not match the planned sections."
        for i in 0..entries.Length-1 do
            let recipe, entry = entries[i]
            let section = expected[i]
            let goal = GenerationPolicy.targetName section.Target
            let matches =
                match entry.Generated.Quality |> Option.bind _.Reasoning with
                | Some analysis -> analysis.Grade = goal && GenerationPolicy.meetsMinimums section.Policy analysis
                | None -> false
            if not matches || entry.Generated.Quality.Value.Selection.Target <> goal then
                invalidOp "A ramp puzzle did not achieve its section's exact grade and bounds."
            if recipe.PairCount <> section.Generation.PairCount || recipe.MinPathLength <> section.Generation.MinPathLength
               || recipe.MaxPathLength <> section.Generation.MaxPathLength || recipe.MaxAttempts <> section.Generation.MaxAttempts
               || recipe.CutStyle <> section.Generation.CutStyle then
                invalidOp "A ramp puzzle does not match its planned recipe."
        // Reuse normal export validation independently for every actual recipe.
        let singles = entries |> Array.map (fun (recipe, puzzle) ->
            createPreparedPackWithCancellation cancellation recipe [|puzzle|])
        let recipes = entries |> Array.map fst
        let common get = recipes |> Array.map get |> Array.distinct |> function [|value|] -> Some value | _ -> None
        let template = singles[0]
        { template with
            Generation = { template.Generation with
                            Algorithm = "difficulty-ramp-v1"; Seed = options.Seed
                            PairCount = common _.PairCount; MinPathLength = common _.MinPathLength
                            MaxPathLength = common _.MaxPathLength; MaxAttempts = common _.MaxAttempts
                            Selection = None; Ramp = Some (Ramp.name spec)
                            RampSections = Some (sections |> Array.map (fun section ->
                                { Target = GenerationPolicy.targetName section.Target; Count = section.Count
                                  Recipe = recipeDocument section.Generation })) }
            Puzzles = singles |> Array.mapi (fun i pack -> { pack.Puzzles[0] with Recipe = Some (recipeDocument recipes[i]) }) }

    let createPackWithCancellation cancellation options generated =
        generated |> Array.map (prepareWithCancellation cancellation) |> createPreparedPackWithCancellation cancellation options

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

    let internal printPreparedPuzzle showSolution (entry: PreparedPuzzle) =
        let generated, difficulty = entry.Generated, entry.Difficulty
        let board = generated.Puzzle
        let reasoning = generated.Quality |> Option.bind _.Reasoning
        printfn "\n%dx%d, %d pairs, seed %d" board.Width board.Height board.Pairs.Length generated.Seed
        printfn "Work: %d board attempts; %d traversal evaluations (%d refinement); %d structured starts."
            generated.Work.BoardAttempts generated.Work.TraversalEvaluations generated.Work.RefinementEvaluations generated.Work.StructuredStarts
        printfn "Mode: %s" (if board.RequireFullCoverage then "full-coverage (every square must be filled)" else "connect-only (unused squares allowed)")
        printfn "Difficulty: %s (%d/100; %s; provisional)" difficulty.Grade difficulty.Score difficulty.Method
        match reasoning with
        | Some a ->
            printfn "Solver: %s; %d trial eliminations; uniqueness %s; %s (%d work units)" a.Tier a.TrialSteps a.Uniqueness a.Status a.Work
            if a.SearchNodes > 0 then
                printfn "First answer: depth %d, %d search nodes; total proof search: %d nodes." a.FirstSolutionBranchDepth a.FirstSolutionSearchNodes a.SearchNodes
        | None -> ()
        match generated.Quality |> Option.bind _.ShortestRoutes with
        | Some s -> printfn "Shortest-route screen: %s after %d trial(s)" (if s.Solved then "solved" else "no solution found by this screen") s.Trials
        | None -> ()
        let endpoints = board.Pairs |> Seq.collect (fun p -> [ p.Start, p.Label; p.Finish, p.Label ])
        printGrid "Puzzle (. = empty):" board.Width board.Height endpoints

        if showSolution then
            let entries = generated.Solution |> Seq.collect (fun p -> p.Cells |> Seq.map (fun cell -> cell, p.Label))
            printGrid "Known solution (. = unused):" board.Width board.Height entries

    let printPuzzleWithCancellation cancellation showSolution generated =
        prepareWithCancellation cancellation generated |> printPreparedPuzzle showSolution

    let printPuzzle showSolution generated = printPuzzleWithCancellation CancellationToken.None showSolution generated
