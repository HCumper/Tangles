module Tangles.Generator.Program

open System
open System.IO
open System.Threading
open Tangles.Generator

let internal runWith generate (cancellation: CancellationToken) options =
    match options.OutputPath with
    | Some path when File.Exists path && not options.Force ->
        eprintfn "Output file already exists. Use --force to replace it: %s" path
        2
    | _ ->
        let sections =
            match options.Ramp with
            | Some spec -> Ramp.plan options.Generation options.Selection options.Count spec |> Result.defaultWith invalidOp
            | None -> [|{ Target = options.Selection.Target; Count = options.Count; Generation = options.Generation; Policy = options.Selection }|]
        let slots = sections |> Array.collect (fun section -> Array.create section.Count section)
        let generated = ResizeArray<GeneratedPuzzle>()
        let recipesUsed = ResizeArray<GenerationOptions>()
        let mutable error = None
        let mutable excluded = Set.empty
        let mutable requests, rejectedSearches, rejectedDuplicates, candidatesConsumed = 0, 0, 0, 0
        let selecting = options.Ramp.IsSome || GenerationPolicy.active options.Selection
        let requestLimit = max options.Count (10 * options.Count)
        let candidateBudget = if selecting then 2 * options.Count * options.Selection.MaxCandidates else requestLimit
        let hasBudget () =
            if selecting then candidatesConsumed + options.Selection.MaxCandidates <= candidateBudget
            else requests < requestLimit

        while generated.Count < options.Count && error.IsNone && hasBudget () do
            cancellation.ThrowIfCancellationRequested()
            let seed = SeedStreams.batchSeed options.Generation.Seed requests
            requests <- requests + 1

            let section = slots[generated.Count]
            let recipe = { section.Generation with Seed = seed }
            match generate cancellation recipe section.Policy with
            | Ok puzzle when Set.contains (PuzzleSymmetry.key puzzle.Puzzle) excluded ->
                candidatesConsumed <- candidatesConsumed + (if selecting then puzzle.Quality.Value.Selection.CandidatesConsidered else 1)
                rejectedDuplicates <- rejectedDuplicates + 1
            | Ok puzzle ->
                generated.Add puzzle
                recipesUsed.Add recipe
                excluded <- Set.add (PuzzleSymmetry.key puzzle.Puzzle) excluded
                if not selecting then candidatesConsumed <- candidatesConsumed + 1
                if selecting then
                    candidatesConsumed <- candidatesConsumed + puzzle.Quality.Value.Selection.CandidatesConsidered
                    eprintfn "Selected puzzle %d/%d (seed %d; %d/%d batch proposals used)." generated.Count options.Count seed candidatesConsumed candidateBudget
            | Error failure when failure.Kind <> SearchExhausted -> error <- Some failure.Message
            | Error failure when selecting ->
                candidatesConsumed <- candidatesConsumed + options.Selection.MaxCandidates
                rejectedSearches <- rejectedSearches + 1
                if rejectedSearches <= 3 || rejectedSearches % 10 = 0 then eprintfn "Skipped seed %d: %s" seed failure.Message
            | Error failure -> error <- Some $"Puzzle {generated.Count + 1}: {failure.Message}"

        if error.IsNone && generated.Count < options.Count then
            error <- Some $"Selected {generated.Count} of {options.Count} puzzles in {requests} requests using {candidatesConsumed}/{candidateBudget} batch proposals ({rejectedSearches} failed selections, {rejectedDuplicates} duplicates). No partial pack was written. Try fewer puzzles, more --candidates, a larger --solver-work budget, fewer pairs or different length limits."

        match error with
        | Some message ->
            eprintfn "%s" message
            1
        | None ->
            cancellation.ThrowIfCancellationRequested()
            let entries = Array.zip (generated.ToArray()) (recipesUsed.ToArray())
            if selecting && options.Generation.Mode = FullCoverage then
                Array.sortInPlaceBy (fun ((p: GeneratedPuzzle),_) ->
                    let grade = if options.Ramp.IsSome then p.Quality.Value.Selection.Target else "any"
                    let gradeOrder = match grade with "easy" -> 0 | "medium" -> 1 | "hard" -> 2 | "expert" -> 3 | _ -> 0
                    match p.Quality |> Option.bind _.Reasoning with
                    | Some a when GenerationPolicy.depthSelection options.Selection ->
                        gradeOrder, 0, a.FirstSolutionBranchDepth, a.FirstSolutionSearchNodes, a.TrialSteps, a.RouteDeductions, p.Seed
                    | Some a -> gradeOrder, a.Score, 0, 0, a.TrialSteps, a.RouteDeductions, p.Seed
                    | None -> gradeOrder,0,0,0,0,0,p.Seed) entries

            let puzzles = entries |> Array.map fst
            let prepared = entries |> Array.map (fun (p,recipe) -> recipe, Export.prepareWithCancellation cancellation p)
            match options.OutputPath with
            | Some path ->
                let pack =
                    match options.Ramp with
                    | Some spec -> Export.createRampPackWithCancellation cancellation options.Generation spec sections prepared
                    | None -> Export.createPreparedPackWithCancellation cancellation options.Generation (prepared |> Array.map snd)
                let selection =
                    pack.Generation.Selection |> Option.map (fun s ->
                        { s with BatchRequests = Some requests; RejectedSearches = Some rejectedSearches; RejectedDuplicates = Some rejectedDuplicates
                                 BatchCandidateBudget = Some candidateBudget; BatchCandidatesConsidered = Some candidatesConsumed })
                let batch: Export.BatchStatistics =
                    { Requests = requests; RejectedSearches = rejectedSearches; RejectedDuplicates = rejectedDuplicates
                      RequestLimit = if selecting then None else Some requestLimit
                      CandidateBudget = (if selecting then Some candidateBudget else None)
                      CandidatesConsidered = candidatesConsumed }
                let pack = { pack with Generation = { pack.Generation with Selection = selection; Batch = Some batch } }
                cancellation.ThrowIfCancellationRequested()
                Export.writeFile path options.Force pack
                printfn "Saved %d puzzle(s) to %s" generated.Count (Path.GetFullPath path)
            | None -> ()

            for _,puzzle in prepared do Export.printPreparedPuzzle options.ShowSolution puzzle
            if selecting then printfn "\nSelected %d distinct puzzles in %d requests; skipped %d failed selections and %d duplicates." puzzles.Length requests rejectedSearches rejectedDuplicates
            printfn "\nKnown solutions validated. Difficulty estimates are provisional and have not been calibrated against human play."
            0

let internal mainWith generate args =
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
            try runWith generate cancellation.Token options
            with
            | :? OperationCanceledException ->
                eprintfn "Generation cancelled."
                130
            | :? InvalidOperationException as error ->
                eprintfn "Internal generation error: %s" error.Message
                1
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

[<EntryPoint>]
let main args = mainWith GuidedGeneration.generateResult args
