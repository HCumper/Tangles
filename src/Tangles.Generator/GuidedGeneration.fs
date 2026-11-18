namespace Tangles.Generator

open System
open System.Collections.Generic
open System.Threading
open Tangles.Core

module GuidedGeneration =
    let validateOptions (generation: GenerationOptions) (policy: GenerationPolicy.Options) =
        [ yield! Generation.validateOptions generation
          if policy.MaxCandidates < 1 || policy.MaxCandidates > 10000 then "Candidate limit must be between 1 and 10000."
          if policy.ImproveSteps < 0 || policy.ImproveSteps > 10000 then "Improvement steps must be between 0 and 10000."
          if policy.SolverWork < 1 || policy.SolverWork > 100_000_000 then "Solver work limit must be between 1 and 100000000."
          if policy.MinTrials < 0 || policy.MinTrials > 1_000_000 then "Minimum trials must be between 0 and 1000000."
          if policy.MinBranchDepth < 0 || policy.MinBranchDepth > 256 then "Minimum branch depth must be between 0 and 256."
          if policy.MinFirstSolutionNodes < 0 || policy.MinFirstSolutionNodes > Solver.defaults.MaxSearchNodes then
              "Minimum first-solution nodes must be between 0 and 20000."
          if policy.MinTrials > 0 && policy.Target <> Hard && policy.Target <> Expert then
              "--min-trials requires --difficulty hard or expert."
          if GenerationPolicy.depthSelection policy && policy.Target <> Expert then
              "--min-depth and --min-search-nodes require --difficulty expert."
          if policy.Target <> AnyDifficulty && generation.Mode <> FullCoverage then
              "Reasoning difficulty targets require --mode full-coverage. For connect-only use --avoid-shortest."
          if policy.AvoidShortest && generation.Mode <> ConnectOnly then "--avoid-shortest applies only to connect-only puzzles."
          if (policy.Target <> AnyDifficulty || policy.RequireUnique)
             && (generation.Width > 16 || generation.Height > 16
                 || generation.PairCount > (if generation.Mode = FullCoverage then 64 else 63)) then
              "Solver selection supports up to 16x16 and at most 64 full-coverage or 63 connect-only pairs." ]

    let private gradeRank = function "easy" -> 0 | "medium" -> 1 | "hard" -> 2 | "expert" -> 3 | _ -> -1

    let generate (cancellation: CancellationToken) generation policy =
        cancellation.ThrowIfCancellationRequested()
        match validateOptions generation policy with
        | _::_ as errors -> Error (String.concat " " errors)
        | [] when not (GenerationPolicy.active policy) -> Generation.generate cancellation generation
        | [] ->
            let target = GenerationPolicy.targetName policy.Target
            let limits = { Solver.defaults with MaxWork = policy.SolverWork }
            let random = Random generation.Seed
            let seen = HashSet<string>()
            let mutable considered, evaluated, traversalEvaluations = 0, 0, 0
            let mutable duplicates, budgetRejections = 0, 0
            let mutable startingTier, startingTrials = "notAssessed", 0
            let mutable failure: string option = None
            let mutable firstAccepted: int option = None
            let mutable accepted: (GeneratedPuzzle * Solver.Analysis option * Solver.ShortestRouteScreen option) option = None
            let mutable acceptedScore = Int32.MinValue, 0, 0, 0

            let score (analysis: Solver.Analysis option) =
                match analysis with
                | Some a when a.Status = "complete" && a.Grade <> "unrated" ->
                    let rank = gradeRank a.Grade
                    let desired = if policy.Target = AnyDifficulty then 2 else gradeRank target
                    let priority = if rank <= desired then rank else -10 - rank
                    if GenerationPolicy.depthSelection policy then
                        priority, a.FirstSolutionBranchDepth, a.FirstSolutionSearchNodes, a.TrialSteps
                    else priority, a.TrialSteps, a.RouteDeductions, 0
                | _ -> -100, 0, 0, 0

            let consider (candidate: GeneratedPuzzle) =
                let key = Solver.boardKey candidate.Puzzle
                if not (seen.Add key) then
                    duplicates <- duplicates + 1
                    None
                else
                    evaluated <- evaluated + 1
                    let shortest =
                        if policy.AvoidShortest then Some (Solver.shortestRouteScreen cancellation 32 candidate.Puzzle)
                        else None
                    if shortest |> Option.exists _.Solved then None
                    else
                        let reasoning =
                            if policy.Assess || policy.Target <> AnyDifficulty || policy.RequireUnique then
                                Some (Solver.analyse cancellation limits candidate.Puzzle)
                            else None
                        match reasoning with
                        | Some a when a.Uniqueness = "none" ->
                            failure <- Some "Internal solver contradiction: the candidate has an independently validated witness."
                        | _ -> ()
                        if startingTier = "notAssessed" then
                            match reasoning with
                            | Some a -> startingTier <- a.Tier; startingTrials <- a.TrialSteps
                            | None -> ()
                        let needsProof = policy.Target <> AnyDifficulty || policy.RequireUnique
                        let limited = reasoning |> Option.exists (fun a -> a.Status <> "complete")
                        if needsProof && limited then budgetRejections <- budgetRejections + 1
                        let matches =
                            (policy.Target = AnyDifficulty || (reasoning |> Option.exists (fun a -> a.Status = "complete" && a.Grade = target)))
                            && (not policy.RequireUnique || (reasoning |> Option.exists (fun a -> a.Uniqueness = "unique")))
                            && (not (GenerationPolicy.strengthened policy)
                                || (reasoning |> Option.exists (GenerationPolicy.meetsMinimums policy)))
                        let qualityScore = score reasoning
                        if matches && failure.IsNone && (accepted.IsNone || qualityScore >= acceptedScore) then
                            accepted <- Some (candidate, reasoning, shortest)
                            acceptedScore <- qualityScore
                            if firstAccepted.IsNone then firstAccepted <- Some considered
                        Some qualityScore

            let guided = generation.Mode = FullCoverage && policy.Target <> AnyDifficulty
            if guided then
                let search = CoverageSearch(generation.Width, generation.Height, generation.PairCount,
                                            generation.MinPathLength, generation.MaxPathLength, random, cancellation)
                let makeCandidate paths =
                    let solution =
                        paths |> Array.mapi (fun i indices ->
                            { Label = if i < 26 then string (char (int 'A' + i)) else string (i + 1)
                              Cells = indices |> Array.map (fun cell -> { X = cell % generation.Width; Y = cell / generation.Width }) |> List.ofArray })
                        |> List.ofArray
                    let puzzle =
                        { Width = generation.Width; Height = generation.Height; RequireFullCoverage = true
                          Pairs = solution |> List.map (fun p -> { Label = p.Label; Start = p.Cells.Head; Finish = List.last p.Cells }) }
                    match Validation.solution puzzle solution with
                    | [] -> ()
                    | errors -> invalidOp ("Internal witness validation failed: " + String.concat " " errors)
                    { Puzzle = puzzle; Solution = solution; Seed = generation.Seed; Attempts = traversalEvaluations; Quality = None }

                let findMixed () =
                    // Larger boards need time to descend through segment-count
                    // plateaux. Short fixed restarts repeatedly discard progress.
                    let cells = generation.Width * generation.Height
                    let restartEvery = if cells <= 64 then 128 else 16 * cells
                    let initialOrder, initialPositions, initialScore = search.Fresh true
                    let mutable order, positions, currentScore = initialOrder, initialPositions, initialScore
                    let mutable found = None
                    let mutable attempts = 0
                    while found.IsNone && attempts < generation.MaxAttempts do
                        cancellation.ThrowIfCancellationRequested()
                        if attempts > 0 && attempts % restartEvery = 0 then
                            let a,b,c = search.Fresh true
                            order <- a; positions <- b; currentScore <- c
                        let candidate, nextPositions = Array.copy order, Array.copy positions
                        if attempts % restartEvery > 0 then search.Backbite(candidate, nextPositions)
                        let ends, nextScore = search.Analyse(candidate, nextPositions)
                        attempts <- attempts + 1
                        traversalEvaluations <- traversalEvaluations + 1
                        if nextScore <= generation.PairCount then
                            match search.Partition(candidate, ends) with
                            | Some paths -> found <- Some (candidate, nextPositions, paths)
                            | None -> ()
                        if nextScore <= currentScore || random.NextDouble() < exp (float (currentScore-nextScore) / 0.4) then
                            order <- candidate; positions <- nextPositions; currentScore <- nextScore
                    found

                let mutable current: (int array * int array * (int * int * int * int)) option = None
                let mutable stagnant = 0
                while considered < policy.MaxCandidates && failure.IsNone
                      && (firstAccepted.IsNone || considered - firstAccepted.Value < policy.ImproveSteps) do
                    cancellation.ThrowIfCancellationRequested()
                    considered <- considered + 1
                    if stagnant >= 32 then current <- None; stagnant <- 0
                    let proposed =
                        match current with
                        | None -> findMixed ()
                        | Some (order, positions, _) ->
                            let candidate, nextPositions = Array.copy order, Array.copy positions
                            for _ in 1 .. random.Next(1,5) do search.Backbite(candidate, nextPositions)
                            let ends, needed = search.Analyse(candidate, nextPositions)
                            traversalEvaluations <- traversalEvaluations + 1
                            if needed > generation.PairCount then None
                            else search.Partition(candidate, ends) |> Option.map (fun paths -> candidate, nextPositions, paths)
                    match proposed with
                    | None -> stagnant <- stagnant + 1
                    | Some (order, positions, paths) ->
                        match consider (makeCandidate paths) with
                        | Some qualityScore when current.IsNone || qualityScore >= (let _,_,s = current.Value in s) ->
                            if current |> Option.exists (fun (_,_,s) -> qualityScore > s) then stagnant <- 0
                            else stagnant <- stagnant + 1
                            current <- Some (order, positions, qualityScore)
                        | _ -> stagnant <- stagnant + 1
            else
                // Selection retries use a deterministic seed stream. The output
                // keeps the requested seed so the complete selection is replayable.
                while accepted.IsNone && considered < policy.MaxCandidates && failure.IsNone do
                    cancellation.ThrowIfCancellationRequested()
                    let seed = if considered = 0 then generation.Seed else random.Next()
                    considered <- considered + 1
                    match Generation.generate cancellation { generation with Seed = seed } with
                    | Error _ -> ()
                    | Ok candidate ->
                        traversalEvaluations <- traversalEvaluations + candidate.Attempts
                        consider { candidate with Seed = generation.Seed } |> ignore

            match failure, accepted with
            | Some error, _ -> Error error
            | None, None ->
                let goal = if policy.AvoidShortest then "puzzle that defeats the shortest-route screen" else $"{target} puzzle meeting the selection policy"
                let budgetAdvice = if budgetRejections > 0 then ", a larger --solver-work budget" else ""
                Error $"No {goal} found in {considered} candidates ({evaluated} assessed, {budgetRejections} solver limits). Try more --candidates{budgetAdvice}, fewer pairs or different path-length bounds. No easier puzzle was substituted."
            | None, Some (candidate, reasoning, shortest) ->
                let statistics: GenerationPolicy.Statistics =
                    { Algorithm = if guided then (if GenerationPolicy.strengthened policy then "solver-guided-hamiltonian-partition-v2" else "solver-guided-hamiltonian-partition-v1") elif generation.Mode = FullCoverage then Generation.fullCoverageAlgorithm else "random-path-growth-v1"
                      Target = target; CandidateLimit = policy.MaxCandidates; CandidatesConsidered = considered; CandidatesEvaluated = evaluated
                      TraversalEvaluations = traversalEvaluations; ImprovementLimit = policy.ImproveSteps; SolverWorkLimit = policy.SolverWork
                      MinTrials = policy.MinTrials; MinBranchDepth = policy.MinBranchDepth; MinFirstSolutionNodes = policy.MinFirstSolutionNodes
                      Assess = policy.Assess; RequireUnique = policy.RequireUnique; AvoidShortest = policy.AvoidShortest; DuplicatesRejected = duplicates
                      BudgetRejections = budgetRejections; StartingTier = startingTier; StartingTrialSteps = startingTrials }
                Ok { candidate with Attempts = traversalEvaluations; Quality = Some { Reasoning = reasoning; ShortestRoutes = shortest; Selection = statistics } }
