namespace Tangles.Generator

open System

type RampSpec =
    { First: DifficultyTarget; Last: DifficultyTarget
      AutoMaxLength: bool; AutoMaxAttempts: bool; AutoSolverWork: bool }

type RampSection =
    { Target: DifficultyTarget; Count: int
      Generation: GenerationOptions; Policy: GenerationPolicy.Options }

module Ramp =
    let targets = [| Easy; Medium; Hard; Expert |]
    let rank = function Easy -> 0 | Medium -> 1 | Hard -> 2 | Expert -> 3 | AnyDifficulty -> -1
    let name spec = GenerationPolicy.targetName spec.First + ".." + GenerationPolicy.targetName spec.Last

    let parse (text: string) =
        let parts = text.Split("..", StringSplitOptions.None)
        let find value = targets |> Array.tryFind (fun target -> GenerationPolicy.targetName target = value)
        if parts.Length <> 2 then Error "Ramp must be an ascending range such as easy..hard."
        else
            match find parts[0], find parts[1] with
            | Some first, Some last when rank first < rank last ->
                Ok { First = first; Last = last; AutoMaxLength = true; AutoMaxAttempts = true; AutoSolverWork = true }
            | _ -> Error "Ramp must be an ascending range such as easy..hard."

    let plan (generation: GenerationOptions) (policy: GenerationPolicy.Options) count spec =
        let first, last = rank spec.First, rank spec.Last
        if first < 0 || last <= first then Error "Ramp must contain ascending explicit grades."
        elif generation.Width < 1 || generation.Width > 16 || generation.Height < 1 || generation.Height > 16 then
            Error "Difficulty ramps support boards up to 16x16."
        elif generation.Mode <> FullCoverage then Error "--ramp requires --mode full-coverage."
        elif count < last-first+1 then Error "Ramp count must include at least one puzzle of every requested grade."
        elif policy.MinTrials > 0 && last < rank Hard then Error "Minimum trials require a hard or expert ramp section."
        elif GenerationPolicy.depthSelection policy && last < rank Expert then Error "Depth/search floors require an expert ramp section."
        else
            let area = generation.Width * generation.Height
            let side = sqrt (float area)
            let round (value: float) = int (Math.Round(value, MidpointRounding.AwayFromZero))
            let sections =
                targets[first..last] |> Array.mapi (fun index target ->
                    let pairs =
                        match target with
                        | Easy -> if area <= 64 then round (1.25*side) else round (float area/6.0)
                        | Medium -> round (float area / 7.2)
                        | Hard -> round (float area / 8.0)
                        | _ -> round (0.5*side)
                    let pairs = max 2 (min (area / max 2 generation.MinPathLength) pairs)
                    let maximum =
                        if not spec.AutoMaxLength then generation.MaxPathLength
                        elif target = Expert then max 16 (2 * int (ceil (float area / float pairs)))
                        else max 16 (2 * int (ceil side))
                    let options =
                        { generation with PairCount = pairs; MaxPathLength = maximum
                                          MaxAttempts = if not spec.AutoMaxAttempts then generation.MaxAttempts elif area <= 64 then 500 elif area <= 144 then 5000 else 10000 }
                    let selection =
                        { policy with Target = target
                                      MinTrials = if target = Hard || target = Expert then policy.MinTrials else 0
                                      MinBranchDepth = if target = Expert then policy.MinBranchDepth else 0
                                      MinFirstSolutionNodes = if target = Expert then policy.MinFirstSolutionNodes else 0
                                      SolverWork = if not spec.AutoSolverWork then policy.SolverWork elif area <= 64 then 2_000_000 elif area <= 144 then 20_000_000 else 100_000_000 }
                    // Balanced contiguous sections; extra puzzles go to earlier grades.
                    let length = count / (last-first+1) + (if index < count % (last-first+1) then 1 else 0)
                    { Target = target; Count = length; Generation = options; Policy = selection })
            let errors = sections |> Array.collect (fun section ->
                GuidedGeneration.validateOptions section.Generation section.Policy
                |> List.map (fun error -> GenerationPolicy.targetName section.Target + ": " + error)
                |> Array.ofList)
            if errors.Length > 0 then Error (String.concat " " errors) else Ok sections
