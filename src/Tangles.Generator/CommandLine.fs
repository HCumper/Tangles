namespace Tangles.Generator

open System
open System.Globalization
open System.Collections.Generic

type CommandOptions =
    { Generation: GenerationOptions
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
  --seed N           Reproducible integer seed (random when omitted)
  --count N          Puzzles in this batch, 1..10000 (default 1)
  --max-attempts N   Candidate attempts per puzzle, 1..10000 (default 500)
  --output PATH      Save a versioned JSON pack with endpoints and solutions
  --show-solution    Also display the known solution in the console
  --force            Allow replacing an existing output file
  --help, -h         Show this help

Rules: orthogonal paths, no sharing or self-touching in both modes.
Connect-only allows unused cells; full-coverage requires every square to be used.
Solutions are validated. Difficulty grades are provisional heuristic estimates.
JSON includes the grade, score, measurements and scoring components.
Uniqueness is not checked; grades have not been calibrated against human play.
An attempt limit is not a proof of impossibility. Ctrl+C cancels generation.
"""

    let parse defaultSeed (args: string array) : Result<Command, string> =
        if args |> Array.exists (fun arg -> arg = "--help" || arg = "-h") then
            Ok Help
        else
            let mutable options =
                { Generation = Generation.defaults defaultSeed
                  Count = 1
                  OutputPath = None
                  ShowSolution = false
                  Force = false }

            let seen = HashSet<string>()
            let mutable index = 0
            let mutable error = None
            let numeric = set [ "--width"; "--height"; "--pairs"; "--min-length"; "--max-length"; "--seed"; "--count"; "--max-attempts" ]

            while index < args.Length && error.IsNone do
                let arg = args[index]

                if not (seen.Add arg) then
                    error <- Some $"Option specified more than once: {arg}."
                elif arg = "--show-solution" then
                    options <- { options with ShowSolution = true }
                elif arg = "--force" then
                    options <- { options with Force = true }
                elif arg = "--output" || arg = "--mode" || numeric.Contains arg then
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
                                | _ -> ()
                else
                    error <- Some $"Unknown option: {arg}. Use --help for usage."

                index <- index + 1

            match error with
            | Some message -> Error message
            | None ->
                let errors =
                    [ yield! Generation.validateOptions options.Generation
                      if options.Count < 1 || options.Count > 10000 then "Count must be between 1 and 10000."
                      if options.Force && options.OutputPath.IsNone then "--force requires --output." ]

                match errors with
                | [] -> Ok (Generate options)
                | _ -> Error (String.concat " " errors)
