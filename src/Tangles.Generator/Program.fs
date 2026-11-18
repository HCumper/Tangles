module Tangles.Generator.Program

open System
open System.IO
open System.Threading
open Tangles.Generator

let private run (cancellation: CancellationToken) options =
    match options.OutputPath with
    | Some path when File.Exists path && not options.Force ->
        eprintfn "Output file already exists. Use --force to replace it: %s" path
        2
    | _ ->
        let seeds = Random options.Generation.Seed
        let generated = ResizeArray<GeneratedPuzzle>()
        let mutable error = None
        let mutable excluded = Set.empty
        let mutable requests, rejectedSearches, rejectedDuplicates, candidatesConsumed = 0, 0, 0, 0
        let selecting = GenerationPolicy.active options.Selection
        let candidateBudget = if selecting then 2 * options.Count * options.Selection.MaxCandidates else options.Count
        let hasBudget () = not selecting || candidatesConsumed + options.Selection.MaxCandidates <= candidateBudget

        while generated.Count < options.Count && error.IsNone && hasBudget () do
            cancellation.ThrowIfCancellationRequested()
            let seed = if requests = 0 then options.Generation.Seed else seeds.Next()
            requests <- requests + 1

            match GuidedGeneration.generate cancellation { options.Generation with Seed = seed } options.Selection with
            | Ok puzzle when selecting && Set.contains (Solver.boardKey puzzle.Puzzle) excluded ->
                candidatesConsumed <- candidatesConsumed + puzzle.Quality.Value.Selection.CandidatesConsidered
                rejectedDuplicates <- rejectedDuplicates + 1
            | Ok puzzle ->
                generated.Add puzzle
                excluded <- Set.add (Solver.boardKey puzzle.Puzzle) excluded
                if selecting then
                    candidatesConsumed <- candidatesConsumed + puzzle.Quality.Value.Selection.CandidatesConsidered
                    eprintfn "Selected puzzle %d/%d (seed %d; %d/%d batch proposals used)." generated.Count options.Count seed candidatesConsumed candidateBudget
            | Error message when message.StartsWith("Internal", StringComparison.Ordinal) -> error <- Some message
            | Error message when selecting ->
                candidatesConsumed <- candidatesConsumed + options.Selection.MaxCandidates
                rejectedSearches <- rejectedSearches + 1
                if rejectedSearches <= 3 || rejectedSearches % 10 = 0 then eprintfn "Skipped seed %d: %s" seed message
            | Error message -> error <- Some $"Puzzle {generated.Count + 1}: {message}"

        if error.IsNone && generated.Count < options.Count then
            error <- Some $"Selected {generated.Count} of {options.Count} puzzles in {requests} requests using {candidatesConsumed}/{candidateBudget} batch proposals ({rejectedSearches} failed selections, {rejectedDuplicates} duplicates). No partial pack was written. Try fewer puzzles, more --candidates, a larger --solver-work budget, fewer pairs or different length limits."

        match error with
        | Some message ->
            eprintfn "%s" message
            1
        | None ->
            cancellation.ThrowIfCancellationRequested()
            let puzzles = generated.ToArray()
            if GenerationPolicy.active options.Selection && options.Generation.Mode = FullCoverage then
                Array.sortInPlaceBy (fun (p: GeneratedPuzzle) ->
                    match p.Quality |> Option.bind _.Reasoning with
                    | Some a -> a.Score, a.TrialSteps, a.RouteDeductions, p.Seed
                    | None -> 0,0,0,p.Seed) puzzles

            match options.OutputPath with
            | Some path ->
                let pack = Export.createPackWithCancellation cancellation options.Generation puzzles
                let selection =
                    pack.Generation.Selection |> Option.map (fun s ->
                        { s with BatchRequests = Some requests; RejectedSearches = Some rejectedSearches; RejectedDuplicates = Some rejectedDuplicates
                                 BatchCandidateBudget = Some candidateBudget; BatchCandidatesConsidered = Some candidatesConsumed })
                let pack = { pack with Generation = { pack.Generation with Selection = selection } }
                cancellation.ThrowIfCancellationRequested()
                Export.writeFile path options.Force pack
                printfn "Saved %d puzzle(s) to %s" generated.Count (Path.GetFullPath path)
            | None -> ()

            for puzzle in puzzles do Export.printPuzzleWithCancellation cancellation options.ShowSolution puzzle
            if selecting then printfn "\nSelected %d distinct puzzles in %d requests; skipped %d failed selections and %d duplicates." puzzles.Length requests rejectedSearches rejectedDuplicates
            printfn "\nKnown solutions validated. Difficulty estimates are provisional and have not been calibrated against human play."
            0

[<EntryPoint>]
let main args =
    match CommandLine.parse (Random.Shared.Next()) args with
    | Error message ->
        eprintfn "%s" message
        2
    | Ok Help ->
        printf "%s" CommandLine.help
        0
    | Ok (Generate options) ->
        use cancellation = new CancellationTokenSource()
        let handler = ConsoleCancelEventHandler(fun _ event -> event.Cancel <- true; cancellation.Cancel())
        Console.CancelKeyPress.AddHandler handler

        try
            try run cancellation.Token options
            with
            | :? OperationCanceledException ->
                eprintfn "Generation cancelled."
                130
            | :? IOException as error ->
                eprintfn "File operation failed: %s" error.Message
                1
            | :? UnauthorizedAccessException as error ->
                eprintfn "File access denied: %s" error.Message
                1
            | :? ArgumentException as error ->
                eprintfn "Invalid argument: %s" error.Message
                2
        finally
            Console.CancelKeyPress.RemoveHandler handler
