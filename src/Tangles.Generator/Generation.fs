namespace Tangles.Generator

open System
open System.Threading
open Tangles.Core

type GenerationMode = ConnectOnly | FullCoverage

type GenerationOptions =
    { Mode: GenerationMode
      CutStyle: CutStyle
      Width: int
      Height: int
      PairCount: int
      MinPathLength: int
      MaxPathLength: int
      MaxAttempts: int
      Seed: int }

type GeneratedPuzzle =
    { Puzzle: Puzzle
      Solution: SolutionPath list
      Seed: int
      Attempts: int
      Quality: GenerationPolicy.Report option
      Work: GenerationWork }

type private GrowthWorkspace(count) =
    member val Local = Array.zeroCreate<bool> count
    member val Starts = Array.zeroCreate<int> count
    member val Path = ResizeArray<int>()
    member val Candidates = Array.zeroCreate<int array> (count + 1)

module Generation =
    let fullCoverageAlgorithm = "hamiltonian-path-partition-v3"

    let algorithm (options: GenerationOptions) =
        if options.Mode = ConnectOnly then "random-path-growth-v1"
        elif options.CutStyle = VariedCuts then "hamiltonian-path-partition-varied-v1"
        else fullCoverageAlgorithm

    let defaults seed =
        { Mode = ConnectOnly
          CutStyle = RandomCuts
          Width = 8
          Height = 8
          PairCount = 6
          MinPathLength = 4
          MaxPathLength = 16
          MaxAttempts = 500
          Seed = seed }

    let validateOptions (options: GenerationOptions) =
        [ if options.Seed < 0 then
              "Seed must be between 0 and 2147483647. Negative seeds are not supported; use the corresponding positive seed for older packs."
          if options.Width < 1 || options.Width > 64
             || options.Height < 1 || options.Height > 64 then
              "Width and height must each be between 1 and 64."
          if options.PairCount < 1 then
              "The number of pairs must be positive."
          if options.CutStyle <> RandomCuts && options.Mode <> FullCoverage then
              "--cut-style varied requires full-coverage mode."
          if options.MinPathLength < 2 then
              "Minimum path length must be at least 2, including endpoints."
          if options.MaxPathLength < options.MinPathLength then
              "Maximum path length must not be less than minimum path length."
          if options.MaxAttempts < 1 || options.MaxAttempts > 10000 then
              "Maximum attempts must be between 1 and 10000."
          if int64 options.PairCount * int64 options.MinPathLength
             > int64 options.Width * int64 options.Height then
              "The requested pairs and minimum path length do not fit on the board."
          if options.Mode = FullCoverage then
              if int64 options.PairCount * int64 options.MaxPathLength < int64 options.Width * int64 options.Height then
                  "Full coverage requires enough pairs and maximum path length to cover every square."
              if options.PairCount = 1 && options.Width > 1 && options.Height > 1 then
                  "One path cannot fill a two-dimensional rectangle without touching itself." ]

    /// Search only a bounded number of extensions. Failure is a failed attempt,
    /// not a proof that the requested configuration is impossible.
    let private tryGrowPath (random: Random) (cancellation: CancellationToken)
                            (neighbours: int array array) (occupied: bool array) (scratch: GrowthWorkspace) targetLength =
        let local, path, starts = scratch.Local, scratch.Path, scratch.Starts
        path.Clear()
        let mutable startCount = 0
        for i in 0 .. occupied.Length - 1 do
            if not occupied[i] then
                starts[startCount] <- i
                startCount <- startCount + 1
        GenerationSupport.shufflePrefix random starts startCount
        let mutable budget = 2048

        let rec extend current =
            cancellation.ThrowIfCancellationRequested()
            budget <- budget - 1

            if path.Count = targetLength then
                true
            elif budget <= 0 then
                false
            else
                let depth = path.Count
                if isNull scratch.Candidates[depth] then scratch.Candidates[depth] <- Array.zeroCreate 4
                let candidates = scratch.Candidates[depth]
                let mutable candidateCount = 0
                for next in neighbours[current] do
                    if not occupied[next] && not local[next] then
                        let mutable touches = 0
                        for n in neighbours[next] do if local[n] then touches <- touches + 1
                        if touches = 1 then
                            candidates[candidateCount] <- next
                            candidateCount <- candidateCount + 1
                GenerationSupport.shufflePrefix random candidates candidateCount
                let mutable found = false
                let mutable index = 0

                while not found && index < candidateCount && budget > 0 do
                    let next = candidates[index]
                    local[next] <- true
                    path.Add next
                    found <- extend next

                    if not found then
                        path.RemoveAt(path.Count - 1)
                        local[next] <- false

                    index <- index + 1

                found

        let mutable found = false
        let mutable index = 0

        while not found && index < startCount && budget > 0 do
            let start = starts[index]
            local[start] <- true
            path.Add start
            found <- extend start

            if not found then
                path.Clear()
                local[start] <- false

            index <- index + 1

        if found then
            let cells = path.ToArray()
            for cell in cells do local[cell] <- false
            Some cells
        else None

    let internal makeGenerated options attempt work (solution: SolutionPath list) =
        if solution.Length <> options.PairCount
           || (solution |> List.exists (fun path -> path.Cells.Length < options.MinPathLength || path.Cells.Length > options.MaxPathLength)) then
            Error (GenerationFailure.create InternalFailure "Internal solution validation failed: path count or length does not match the requested bounds." work)
        else
            let board =
                { Width = options.Width
                  Height = options.Height
                  RequireFullCoverage = options.Mode = FullCoverage
                  Pairs =
                    solution |> List.map (fun path ->
                        { Label = path.Label; Start = List.head path.Cells; Finish = List.last path.Cells }) }
            match Validation.solution board solution with
            | [] -> Ok { Puzzle = board; Solution = solution; Seed = options.Seed; Attempts = attempt; Quality = None; Work = work }
            | errors -> Error (GenerationFailure.create InternalFailure ("Internal solution validation failed: " + String.concat " " errors) work)

    let internal fromPaths options attempt work (paths: int array array) =
        let solution =
            paths |> Array.mapi (fun i cells ->
                { Label = GenerationSupport.label i
                  Cells = cells |> Array.map (fun cell -> { X = cell % options.Width; Y = cell / options.Width }) |> List.ofArray })
            |> List.ofArray
        makeGenerated options attempt work solution

    let internal generateResult (cancellation: CancellationToken) (options: GenerationOptions) : Result<GeneratedPuzzle, GenerationFailure> =
        match validateOptions options with
        | _ :: _ as errors -> Error (GenerationFailure.create InvalidOptions (String.concat " " errors) GenerationWork.zero)
        | [] ->
            let random = Random options.Seed
            let cellCount = options.Width * options.Height

            match options.Mode with
            | FullCoverage ->
                let search = CoverageSearch(options.Width, options.Height, options.PairCount, options.MinPathLength, options.MaxPathLength, random, cancellation, options.CutStyle)
                let fallbackAt = if options.MaxAttempts >= 128 then max 64 (options.MaxAttempts - 128) else -1
                match search.Find(options.MaxAttempts, 128, max 64 cellCount, fallbackAt, true) with
                | Some layout -> fromPaths options search.Work.TraversalEvaluations search.Work layout.Paths
                | None ->
                    Error (GenerationFailure.create SearchExhausted
                        $"No candidate found after {search.Work.TraversalEvaluations} attempts (seed {options.Seed}). Try another seed, more attempts, or different pair/path-length settings. This does not prove impossibility."
                        search.Work)
            | ConnectOnly ->
                let neighbours = GenerationSupport.neighbours options.Width options.Height
                let scratch = GrowthWorkspace cellCount
                let mutable attempt = 0
                let mutable result = None
                while result.IsNone && attempt < options.MaxAttempts do
                    cancellation.ThrowIfCancellationRequested()
                    attempt <- attempt + 1
                    let occupied = Array.create cellCount false
                    let paths = ResizeArray<SolutionPath>()
                    let mutable freeCount = cellCount
                    let mutable failed = false

                    while not failed && paths.Count < options.PairCount do
                        let remainingPairs = options.PairCount - paths.Count - 1
                        let maximum = min options.MaxPathLength (freeCount - remainingPairs * options.MinPathLength)
                        let target = random.Next(options.MinPathLength, maximum + 1)

                        let grown =
                            match tryGrowPath random cancellation neighbours occupied scratch target with
                            | Some cells -> Some cells
                            | None when target <> options.MinPathLength ->
                                tryGrowPath random cancellation neighbours occupied scratch options.MinPathLength
                            | None -> None

                        match grown with
                        | None -> failed <- true
                        | Some indices ->
                            for index in indices do
                                occupied[index] <- true

                            freeCount <- freeCount - indices.Length

                            paths.Add
                                { Label = GenerationSupport.label paths.Count
                                  Cells =
                                    indices
                                    |> Array.map (fun index -> { X = index % options.Width; Y = index / options.Width })
                                    |> List.ofArray }

                    if not failed then
                        let solution = List.ofSeq paths

                        result <- Some (makeGenerated options attempt { GenerationWork.zero with BoardAttempts = attempt } solution)

                match result with
                | Some generated -> generated
                | None ->
                    Error (GenerationFailure.create SearchExhausted
                        $"No candidate found after {attempt} attempts (seed {options.Seed}). Try another seed or fewer/shorter paths. This does not prove impossibility."
                        { GenerationWork.zero with BoardAttempts = attempt })

    let generate cancellation options =
        generateResult cancellation options |> Result.mapError _.Message
