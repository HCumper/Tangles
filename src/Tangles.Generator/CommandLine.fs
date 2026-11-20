namespace Tangles.Generator

open System
open System.Globalization
open System.Collections.Generic

type CommandOptions =
    { Generation: GenerationOptions
      Selection: GenerationPolicy.Options
      Ramp: RampSpec option
      Count: int
      OutputPath: string option
      ShowSolution: bool
      Force: bool }

type Command = Help | Generate of CommandOptions

module CommandLine =
    let help = """Tangles puzzle generator (.NET 11 / F#)

Usage: Tangles.Generator [options]

  --mode MODE        connect-only (default) or full-coverage
  --width N          Board width, 1..64 (default 8)
  --height N         Board height, 1..64 (default 8)
  --pairs N          Number of endpoint pairs (default 6)
  --min-length N     Minimum path cells, including endpoints (default 4)
  --max-length N     Maximum path cells, including endpoints (default 16)
  --seed N           Reproducible seed, 0..2147483647 (random when omitted)
  --count N          Puzzles in this batch, 1..10000 (default 1)
  --max-attempts N   Candidate attempts per puzzle, 1..10000 (default 500)
  --difficulty D    Full-coverage target: any (default), easy, medium, hard, expert
  --ramp RANGE      Compose exact graded sections, e.g. easy..hard (default count 12)
  --cut-style STYLE random (default) or modest varied weighting; full coverage only
  --candidates N    Bounded selection proposals, 1..10000 (default 100)
  --improve-steps N Further proposals after reaching the target, 0..10000 (default 100)
  --solver-work N   Deterministic work budget per assessment (default 2000000)
  --min-trials N    Minimum contradiction eliminations for hard/expert (default 0)
  --max-trials N    Optional maximum contradiction eliminations (unset by default)
  --min-depth N     Expert: minimum branch depth to the first answer (default 0)
  --min-search-nodes N  Expert: minimum nodes before the first answer (default 0)
  --assess          Record clue-only solver reasoning and solution-count evidence
  --require-unique  Accept only puzzles proved to have one solution
  --avoid-shortest  Connect-only: reject puzzles solved by 32 shortest-route trials
  --output PATH      Save a versioned JSON pack with endpoints and solutions
  --show-solution    Also display the known solution in the console
  --force            Allow replacing an existing output file
  --help, -h         Show this help

Rules: orthogonal paths, no sharing or self-touching in both modes.
Connect-only allows unused cells; full-coverage requires every square to be used.
Hard uses contradiction trials; expert allows branching. Targets are exact.
Targeted full-coverage search mixes fresh traversals and hill-climbs solver grade.
--max-attempts bounds each fresh layout search; --candidates also bounds retries.
Batch seed derivation: splitmix64-batch-v1; the first request retains --seed.
Selected batches allow at most 2 * count * candidates proposals in total.
Batches reject rotated/reflected clues; fast batches allow at most 10 * count requests.
JSON includes reasoning, explicit work counters and provisional difficulty grades.
The legacy attempts field is retained; use work for board/refinement/search counts.
Uniqueness remains optional; a solver limit never certifies uniqueness or hardness.
Grades have not been calibrated against human play. Any valid route still counts.
An attempt limit is not a proof of impossibility. Ctrl+C cancels generation.
"""

    let parse defaultSeed (args: string array) : Result<Command, string> =
        if args |> Array.exists (fun arg -> arg = "--help" || arg = "-h") then
            Ok Help
        else
            let mutable options =
                { Generation = Generation.defaults defaultSeed
                  Selection = GenerationPolicy.defaults
                  Ramp = None
                  Count = 1
                  OutputPath = None
                  ShowSolution = false
                  Force = false }

            let seen = HashSet<string>()
            let mutable index = 0
            let mutable error = None
            let numeric = set [ "--width"; "--height"; "--pairs"; "--min-length"; "--max-length"; "--seed"; "--count"; "--max-attempts"; "--candidates"; "--improve-steps"; "--solver-work"; "--min-trials"; "--max-trials"; "--min-depth"; "--min-search-nodes" ]

            while index < args.Length && error.IsNone do
                let arg = args[index]

                if not (seen.Add arg) then
                    error <- Some $"Option specified more than once: {arg}."
                elif arg = "--show-solution" then
                    options <- { options with ShowSolution = true }
                elif arg = "--force" then
                    options <- { options with Force = true }
                elif arg = "--assess" then options <- { options with Selection = { options.Selection with Assess = true } }
                elif arg = "--require-unique" then options <- { options with Selection = { options.Selection with RequireUnique = true } }
                elif arg = "--avoid-shortest" then options <- { options with Selection = { options.Selection with AvoidShortest = true } }
                elif arg = "--output" || arg = "--mode" || arg = "--difficulty" || arg = "--ramp" || arg = "--cut-style" || numeric.Contains arg then
                    if index + 1 >= args.Length || args[index + 1].StartsWith("--", StringComparison.Ordinal) then
                        error <- Some $"Missing value for {arg}."
                    else
                        index <- index + 1
                        let value = args[index]

                        if arg = "--mode" then
                            match value with
                            | "connect-only" -> options <- { options with Generation = { options.Generation with Mode = ConnectOnly } }
                            | "full-coverage" -> options <- { options with Generation = { options.Generation with Mode = FullCoverage } }
                            | _ -> error <- Some "Mode must be connect-only or full-coverage."
                        elif arg = "--ramp" then
                            match Ramp.parse value with
                            | Ok spec -> options <- { options with Ramp = Some spec }
                            | Error message -> error <- Some message
                        elif arg = "--cut-style" then
                            match value with
                            | "random" -> options <- { options with Generation = { options.Generation with CutStyle = RandomCuts } }
                            | "varied" -> options <- { options with Generation = { options.Generation with CutStyle = VariedCuts } }
                            | _ -> error <- Some "Cut style must be random or varied."
                        elif arg = "--difficulty" then
                            let target = match value with "any" -> Some AnyDifficulty | "easy" -> Some Easy | "medium" -> Some Medium | "hard" -> Some Hard | "expert" -> Some Expert | _ -> None
                            match target with
                            | Some target -> options <- { options with Selection = { options.Selection with Target = target } }
                            | None -> error <- Some "Difficulty must be any, easy, medium, hard or expert."
                        elif arg = "--output" then
                            if String.IsNullOrWhiteSpace value then error <- Some "Output path must not be empty."
                            else options <- { options with OutputPath = Some value }
                        else
                            match Int32.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture) with
                            | false, _ -> error <- Some $"Expected an integer for {arg}: {value}."
                            | true, number ->
                                let generation = options.Generation

                                match arg with
                                | "--width" -> options <- { options with Generation = { generation with Width = number } }
                                | "--height" -> options <- { options with Generation = { generation with Height = number } }
                                | "--pairs" -> options <- { options with Generation = { generation with PairCount = number } }
                                | "--min-length" -> options <- { options with Generation = { generation with MinPathLength = number } }
                                | "--max-length" -> options <- { options with Generation = { generation with MaxPathLength = number } }
                                | "--max-attempts" -> options <- { options with Generation = { generation with MaxAttempts = number } }
                                | "--seed" -> options <- { options with Generation = { generation with Seed = number } }
                                | "--count" -> options <- { options with Count = number }
                                | "--candidates" -> options <- { options with Selection = { options.Selection with MaxCandidates = number } }
                                | "--improve-steps" -> options <- { options with Selection = { options.Selection with ImproveSteps = number } }
                                | "--solver-work" -> options <- { options with Selection = { options.Selection with SolverWork = number } }
                                | "--min-trials" -> options <- { options with Selection = { options.Selection with MinTrials = number } }
                                | "--max-trials" -> options <- { options with Selection = { options.Selection with MaxTrials = Some number } }
                                | "--min-depth" -> options <- { options with Selection = { options.Selection with MinBranchDepth = number } }
                                | "--min-search-nodes" -> options <- { options with Selection = { options.Selection with MinFirstSolutionNodes = number } }
                                | _ -> ()
                else
                    error <- Some $"Unknown option: {arg}. Use --help for usage."

                index <- index + 1

            match error with
            | Some message -> Error message
            | None ->
                if options.Ramp.IsSome then
                    let spec = options.Ramp.Value
                    options <- { options with
                                    Count = if seen.Contains "--count" then options.Count else 12
                                    Ramp = Some { spec with AutoMaxLength = not (seen.Contains "--max-length")
                                                            AutoMaxAttempts = not (seen.Contains "--max-attempts")
                                                            AutoSolverWork = not (seen.Contains "--solver-work") } }
                let errors =
                    [ match options.Ramp with
                      | None -> yield! GuidedGeneration.validateOptions options.Generation options.Selection
                      | Some spec ->
                          if seen.Contains "--pairs" || seen.Contains "--difficulty" then
                              yield "--ramp chooses pair counts and targets; do not combine it with --pairs or --difficulty."
                          match Ramp.plan options.Generation options.Selection options.Count spec with
                          | Error message -> yield message
                          | Ok _ -> ()
                      if options.Count < 1 || options.Count > 10000 then yield "Count must be between 1 and 10000."
                      if options.Force && options.OutputPath.IsNone then yield "--force requires --output." ]

                match errors with
                | [] -> Ok (Generate options)
                | _ -> Error (String.concat " " errors)
