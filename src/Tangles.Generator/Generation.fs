namespace Tangles.Generator

open System
open System.Threading
open Tangles.Core

type GenerationMode = ConnectOnly | FullCoverage

type GenerationOptions =
    { Mode: GenerationMode
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
      Attempts: int }

module Generation =
    let defaults seed =
        { Mode = ConnectOnly
          Width = 8
          Height = 8
          PairCount = 6
          MinPathLength = 4
          MaxPathLength = 16
          MaxAttempts = 500
          Seed = seed }

    let validateOptions options =
        [ if options.Width < 1 || options.Width > 64
             || options.Height < 1 || options.Height > 64 then
              "Width and height must each be between 1 and 64."
          if options.PairCount < 1 then
              "The number of pairs must be positive."
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

    let private shuffle (random: Random) (items: 'a array) =
        for i in items.Length - 1 .. -1 .. 1 do
            let j = random.Next(i + 1)
            let value = items[i]
            items[i] <- items[j]
            items[j] <- value

    let private label index =
        if index < 26 then string (char (int 'A' + index)) else string (index + 1)

    /// Search only a bounded number of extensions. Failure is a failed attempt,
    /// not a proof that the requested configuration is impossible.
    let private tryGrowPath (random: Random) (cancellation: CancellationToken)
                            (neighbours: int array array) (occupied: bool array) targetLength =
        let local = Array.create occupied.Length false
        let path = ResizeArray<int>()
        let starts = [| for i in 0 .. occupied.Length - 1 do if not occupied[i] then i |]
        shuffle random starts
        let mutable budget = 2048

        let rec extend current =
            cancellation.ThrowIfCancellationRequested()
            budget <- budget - 1

            if path.Count = targetLength then
                true
            elif budget <= 0 then
                false
            else
                let candidates =
                    neighbours[current]
                    |> Array.filter (fun next ->
                        not occupied[next] && not local[next]
                        && (neighbours[next] |> Array.sumBy (fun n -> if local[n] then 1 else 0)) = 1)

                shuffle random candidates
                let mutable found = false
                let mutable index = 0

                while not found && index < candidates.Length && budget > 0 do
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

        while not found && index < starts.Length && budget > 0 do
            let start = starts[index]
            local[start] <- true
            path.Add start
            found <- extend start

            if not found then
                path.Clear()
                local[start] <- false

            index <- index + 1

        if found then Some (path.ToArray()) else None

    /// Each candidate is a Hamiltonian traversal. Split it into disjoint induced
    /// paths: any nonconsecutive neighbouring cells must be in different paths.
    /// Backbite moves preserve the traversal; bounded local search seeks layouts
    /// which can be split into exactly the requested number of paths.
    let private tryCover (random: Random) (cancellation: CancellationToken)
                         (neighbours: int array array) options =
        let count = options.Width * options.Height
        let maximum = min count options.MaxPathLength
        // Begin with fewer stripes on rectangles; randomise either orientation on squares.
        let transpose = if options.Width = options.Height then random.Next(2) = 0 else options.Width < options.Height
        let flipX, flipY = random.Next(2) = 0, random.Next(2) = 0
        let columns, rows = if transpose then options.Height, options.Width else options.Width, options.Height
        let initial =
            [| for row in 0 .. rows - 1 do
                   for offset in 0 .. columns - 1 do
                       let column = if row % 2 = 0 then offset else columns - 1 - offset
                       let x, y = if transpose then row, column else column, row
                       let x = if flipX then options.Width - 1 - x else x
                       let y = if flipY then options.Height - 1 - y else y
                       yield y * options.Width + x |]

        let analyse (order: int array) =
            let positions = Array.zeroCreate count
            for i in 0 .. count - 1 do positions[order[i]] <- i
            // left[j] is the earliest permitted start of a segment ending at j.
            let left = Array.zeroCreate count
            let mutable bound = 0
            for j in 0 .. count - 1 do
                cancellation.ThrowIfCancellationRequested()
                for neighbour in neighbours[order[j]] do
                    if positions[neighbour] < j - 1 then bound <- max bound (positions[neighbour] + 1)
                left[j] <- bound
            let ends = Array.zeroCreate count
            let mutable finish = 0
            for start in 0 .. count - 1 do
                finish <- max finish start
                while finish < count && left[finish] <= start do finish <- finish + 1
                ends[start] <- min finish (start + maximum)
            let needed = Array.create (count + 1) (count + 1)
            needed[count] <- 0
            for start in count - 1 .. -1 .. 0 do
                cancellation.ThrowIfCancellationRequested()
                for finish in start + options.MinPathLength .. ends[start] do
                    needed[start] <- min needed[start] (1 + needed[finish])
            positions, ends, needed[0]

        let partition (order: int array) (ends: int array) =
            // Prefix sums make each range-of-possible-cuts query constant-time.
            let possible = Array.init (options.PairCount + 1) (fun _ -> Array.zeroCreate<bool> (count + 1))
            possible[0][count] <- true
            let prefix = Array.zeroCreate<int> (count + 2)
            for paths in 1 .. options.PairCount do
                cancellation.ThrowIfCancellationRequested()
                prefix[0] <- 0
                for i in 0 .. count do prefix[i + 1] <- prefix[i] + (if possible[paths - 1][i] then 1 else 0)
                let first = max 0 (count - paths * maximum)
                let last = count - paths * options.MinPathLength
                for start in first .. last do
                    let minimumEnd = start + options.MinPathLength
                    let maximumEnd = ends[start]
                    possible[paths][start] <- minimumEnd <= maximumEnd && prefix[maximumEnd + 1] > prefix[minimumEnd]
            if not (possible[options.PairCount][0]) then None
            else
                let paths = ResizeArray<int array>()
                let mutable start = 0
                for remaining in options.PairCount .. -1 .. 1 do
                    cancellation.ThrowIfCancellationRequested()
                    let cuts =
                        [| for finish in start + options.MinPathLength .. ends[start] do
                               if possible[remaining - 1][finish] then yield finish |]
                    let finish = cuts[random.Next cuts.Length]
                    paths.Add order[start .. finish - 1]
                    start <- finish
                let result = paths.ToArray()
                shuffle random result
                for path in result do if random.Next(2) = 0 then Array.Reverse path
                Some result

        let mutate (order: int array) (positions: int array) =
            let candidate = Array.copy order
            let front = random.Next(2) = 0
            let endpoint, adjacent = if front then order[0], order[1] else order[count - 1], order[count - 2]
            let choices = neighbours[endpoint] |> Array.filter ((<>) adjacent)
            if choices.Length > 0 then
                let join = positions[choices[random.Next choices.Length]]
                if front then Array.Reverse(candidate, 0, join)
                else Array.Reverse(candidate, join + 1, count - join - 1)
            candidate

        let mutable order = initial
        let initialPositions, _, initialScore = analyse order
        let mutable positions = initialPositions
        let mutable score = initialScore
        let mutable attempt = 0
        let mutable firstSuccess: int option = None
        let mutable result = None
        // Keep a valid witness while exploring a few further layouts for variety.
        let refinement = min 32 count
        while attempt < options.MaxAttempts
              && (firstSuccess.IsNone || attempt - firstSuccess.Value < refinement) do
            cancellation.ThrowIfCancellationRequested()
            if firstSuccess.IsNone && attempt > 0 && attempt % 64 = 0 then
                order <- initial
                let resetPositions, _, resetScore = analyse order
                positions <- resetPositions
                score <- resetScore
            attempt <- attempt + 1
            let candidate = if attempt = 1 then order else mutate order positions
            let nextPositions, ends, nextScore = analyse candidate
            if nextScore <= options.PairCount then
                match partition candidate ends with
                | Some paths ->
                    result <- Some paths
                    if firstSuccess.IsNone then firstSuccess <- Some attempt
                | None -> ()
            if nextScore <= score || random.NextDouble() < exp (float (score - nextScore) / 0.4) then
                order <- candidate
                positions <- nextPositions
                score <- nextScore
        result |> Option.map (fun paths -> paths, attempt)

    let private makeGenerated options attempt (solution: SolutionPath list) =
        let board =
            { Width = options.Width
              Height = options.Height
              RequireFullCoverage = options.Mode = FullCoverage
              Pairs =
                solution |> List.map (fun path ->
                    { Label = path.Label; Start = List.head path.Cells; Finish = List.last path.Cells }) }
        { Puzzle = board; Solution = solution; Seed = options.Seed; Attempts = attempt }

    let generate (cancellation: CancellationToken) options : Result<GeneratedPuzzle, string> =
        match validateOptions options with
        | _ :: _ as errors -> Error (String.concat " " errors)
        | [] ->
            let random = Random options.Seed
            let cellCount = options.Width * options.Height

            let neighbours =
                Array.init cellCount (fun index ->
                    let x, y = index % options.Width, index / options.Width
                    [| if x > 0 then index - 1
                       if x + 1 < options.Width then index + 1
                       if y > 0 then index - options.Width
                       if y + 1 < options.Height then index + options.Width |])

            let mutable attempt = 0
            let mutable result = None

            if options.Mode = FullCoverage then
                match tryCover random cancellation neighbours options with
                | Some (paths, attempts) ->
                    let solution =
                        paths |> Array.mapi (fun i indices ->
                            { Label = label i
                              Cells = indices |> Array.map (fun index -> { X = index % options.Width; Y = index / options.Width }) |> List.ofArray })
                        |> List.ofArray
                    result <- Some (makeGenerated options attempts solution)
                | None -> ()

            while options.Mode = ConnectOnly && result.IsNone && attempt < options.MaxAttempts do
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
                        match tryGrowPath random cancellation neighbours occupied target with
                        | Some cells -> Some cells
                        | None when target <> options.MinPathLength ->
                            tryGrowPath random cancellation neighbours occupied options.MinPathLength
                        | None -> None

                    match grown with
                    | None -> failed <- true
                    | Some indices ->
                        for index in indices do
                            occupied[index] <- true

                        freeCount <- freeCount - indices.Length

                        paths.Add
                            { Label = label paths.Count
                              Cells =
                                indices
                                |> Array.map (fun index -> { X = index % options.Width; Y = index / options.Width })
                                |> List.ofArray }

                if not failed then
                    let solution = List.ofSeq paths

                    result <- Some (makeGenerated options attempt solution)

            match result with
            | None ->
                let suggestion =
                    if options.Mode = FullCoverage then "Try another seed, more attempts, or different pair/path-length settings."
                    else "Try another seed or fewer/shorter paths."
                Error $"No candidate found after {options.MaxAttempts} attempts (seed {options.Seed}). {suggestion} This does not prove impossibility."
            | Some generated ->
                match Validation.solution generated.Puzzle generated.Solution with
                | [] -> Ok generated
                | errors -> Error ("Internal solution validation failed: " + String.concat " " errors)
