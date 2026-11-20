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
          if policy.MaxTrials |> Option.exists (fun upper -> upper < policy.MinTrials || upper > 1_000_000) then
              "Maximum trials must be between the minimum trials and 1000000."
          if policy.MaxTrials.IsSome && policy.Target = AnyDifficulty then
              "--max-trials requires an explicit difficulty target or a ramp."
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

    let internal generateWith (generator: CancellationToken -> GenerationOptions -> Result<GeneratedPuzzle, GenerationFailure>) (cancellation: CancellationToken) generation policy =
        cancellation.ThrowIfCancellationRequested()
        match validateOptions generation policy with
        | _::_ as errors -> Error (GenerationFailure.create InvalidOptions (String.concat " " errors) GenerationWork.zero)
        | [] when not (GenerationPolicy.active policy) -> generator cancellation generation
        | [] ->
            let target = GenerationPolicy.targetName policy.Target
            let limits = { Solver.defaults with MaxWork = policy.SolverWork }
            let random = Random generation.Seed
            let seen = HashSet<string>()
            let mutable considered, evaluated, traversalEvaluations, solverAssessments = 0, 0, 0, 0
            let mutable work = GenerationWork.zero
            let mutable duplicates, budgetRejections = 0, 0
            let mutable startingTier, startingTrials = "notAssessed", 0
            let mutable failure: GenerationFailure option = None
            let mutable firstAccepted: int option = None
            let mutable accepted: (GeneratedPuzzle * Solver.Analysis option * Solver.ShortestRouteScreen option) option = None
            let mutable acceptedScore = Int32.MinValue, 0, 0, 0, 0

            let score (analysis: Solver.Analysis option) =
                match analysis with
                | Some a when a.Status = "complete" && a.Grade <> "unrated" ->
                    let rank = gradeRank a.Grade
                    let desired = if policy.Target = AnyDifficulty then 2 else gradeRank target
                    let priority = if rank <= desired then rank else -10 - rank
                    let distance =
                        match policy.MaxTrials with
                        | None -> 0
                        | Some upper -> max 0 (max (policy.MinTrials-a.TrialSteps) (a.TrialSteps-upper))
                    if GenerationPolicy.depthSelection policy then
                        priority, -distance, a.FirstSolutionBranchDepth, a.FirstSolutionSearchNodes, a.TrialSteps
                    else priority, -distance, a.TrialSteps, a.RouteDeductions, 0
                | _ -> -100, 0, 0, 0, 0

            let consider (candidate: GeneratedPuzzle) =
                let key = PuzzleSymmetry.key candidate.Puzzle
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
                                solverAssessments <- solverAssessments + 1
                                Some (Solver.analyse cancellation limits candidate.Puzzle)
                            else None
                        match reasoning with
                        | Some a when a.Uniqueness = "none" ->
                            failure <- Some (GenerationFailure.create InternalFailure "Internal solver contradiction: the candidate has an independently validated witness." work)
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
                                            generation.MinPathLength, generation.MaxPathLength, random, cancellation, generation.CutStyle)
                let makeCandidate paths = Generation.fromPaths generation traversalEvaluations work paths

                let mutable structuredProposal = false
                let findLayout () =
                    let cells = generation.Width * generation.Height
                    let restartEvery = if cells <= 64 then 128 else 16 * cells
                    // Give mixed searches their full budget. Exact lengths get
                    // an early structured proposal; otherwise reserve the last
                    // proposal only when mixing has found no assessable layout.
                    let tight = generation.MinPathLength * generation.PairCount = cells
                                || int64 generation.MaxPathLength * int64 generation.PairCount = int64 cells
                    structuredProposal <- (tight && considered = 1)
                                          || (evaluated = 0 && considered = policy.MaxCandidates)
                    let found = search.Find(generation.MaxAttempts, restartEvery, 0, (if structuredProposal then 0 else -1), false)
                    found |> Option.map (fun layout -> layout.Order, layout.Positions, layout.Paths)

                let mutable current: (int array * int array * (int * int * int * int * int)) option = None
                let mutable stagnant = 0
                while considered < policy.MaxCandidates && failure.IsNone
                      && (firstAccepted.IsNone || considered - firstAccepted.Value < policy.ImproveSteps) do
                    cancellation.ThrowIfCancellationRequested()
                    considered <- considered + 1
                    if stagnant >= 32 then current <- None; stagnant <- 0
                    let proposed =
                        match current with
                        | None -> findLayout ()
                        | Some (order, positions, _) ->
                            structuredProposal <- false
                            let candidate, nextPositions = Array.copy order, Array.copy positions
                            for _ in 1 .. random.Next(1,5) do search.Backbite(candidate, nextPositions)
                            let ends, needed = search.Analyse(candidate, nextPositions)
                            if needed > generation.PairCount then None
                            else search.Partition(candidate, ends) |> Option.map (fun paths -> candidate, nextPositions, paths)
                    work <- search.Work
                    traversalEvaluations <- work.TraversalEvaluations
                    match proposed with
                    | None -> stagnant <- stagnant + 1
                    | Some (order, positions, paths) ->
                        match makeCandidate paths with
                        | Error error -> failure <- Some error
                        | Ok candidate ->
                            match consider candidate with
                            | Some qualityScore when current.IsNone || qualityScore >= (let _,_,s = current.Value in s) ->
                                if current |> Option.exists (fun (_,_,s) -> qualityScore > s) then stagnant <- 0
                                else stagnant <- stagnant + 1
                                let rank,_,_,_,_ = qualityScore
                                // An easy structured witness must not trap a
                                // harder target in repeated stripe mutations.
                                if not structuredProposal || rank = gradeRank target then
                                    current <- Some (order, positions, qualityScore)
                            | _ -> stagnant <- stagnant + 1
            else
                // Selection retries use a deterministic seed stream. The output
                // keeps the requested seed so the complete selection is replayable.
                while accepted.IsNone && considered < policy.MaxCandidates && failure.IsNone do
                    cancellation.ThrowIfCancellationRequested()
                    let seed = if considered = 0 then generation.Seed else random.Next()
                    considered <- considered + 1
                    match generator cancellation { generation with Seed = seed } with
                    | Error error ->
                        work <- GenerationWork.add work error.Work
                        if error.Kind <> SearchExhausted then failure <- Some error
                    | Ok candidate ->
                        work <- GenerationWork.add work candidate.Work
                        traversalEvaluations <- work.TraversalEvaluations
                        consider { candidate with Seed = generation.Seed } |> ignore

            match failure, accepted with
            | Some error, _ -> Error error
            | None, None ->
                let goal = if policy.AvoidShortest then "puzzle that defeats the shortest-route screen" else $"{target} puzzle meeting the selection policy"
                let detail = $"{considered} candidates ({evaluated} distinct layouts checked, {solverAssessments} solver assessments, {budgetRejections} solver limits; {work.TraversalEvaluations} traversal evaluations)"
                let message =
                    if evaluated = 0 then
                        $"No valid layout found in {detail}. Increase --max-attempts, try another seed or adjust pair/path-length bounds. This does not prove impossibility."
                    elif solverAssessments > 0 && budgetRejections = solverAssessments then
                        $"No {goal} found in {detail}. All solver assessments reached a limit; increase --solver-work or simplify the board."
                    else
                        $"No {goal} found in {detail}. Valid layouts did not meet the policy; try more --candidates, another seed or different pair/path-length bounds."
                Error (GenerationFailure.create SearchExhausted (message + " No easier puzzle was substituted.") work)
            | None, Some (candidate, reasoning, shortest) ->
                let statistics: GenerationPolicy.Statistics =
                    { Algorithm = if guided && generation.CutStyle = VariedCuts then (if GenerationPolicy.strengthened policy then "solver-guided-varied-cuts-v2" else "solver-guided-varied-cuts-v1") elif guided then (if GenerationPolicy.strengthened policy then "solver-guided-hamiltonian-partition-v6" else "solver-guided-hamiltonian-partition-v5") else Generation.algorithm generation
                      PolicyVersion = GenerationPolicy.version
                      Target = target; CandidateLimit = policy.MaxCandidates; CandidatesConsidered = considered; CandidatesEvaluated = evaluated
                      TraversalEvaluations = work.TraversalEvaluations; SolverAssessments = solverAssessments; ImprovementLimit = policy.ImproveSteps; SolverWorkLimit = policy.SolverWork
                      MinTrials = policy.MinTrials; MaxTrials = policy.MaxTrials; MinBranchDepth = policy.MinBranchDepth; MinFirstSolutionNodes = policy.MinFirstSolutionNodes
                      Assess = policy.Assess; RequireUnique = policy.RequireUnique; AvoidShortest = policy.AvoidShortest; DuplicatesRejected = duplicates
                      BudgetRejections = budgetRejections; StartingTier = startingTier; StartingTrialSteps = startingTrials }
                Ok { candidate with Attempts = work.BoardAttempts + work.TraversalEvaluations; Work = work; Quality = Some { Reasoning = reasoning; ShortestRoutes = shortest; Selection = statistics } }

    let internal generateResult cancellation generation policy =
        generateWith Generation.generateResult cancellation generation policy

    let generate cancellation generation policy =
        generateResult cancellation generation policy |> Result.mapError _.Message
