namespace Tangles.Generator

open System
open System.Threading
open Tangles.Core

type GenerationOptions =
    { Width: int
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
        { Width = 8
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
              "The requested pairs and minimum path length do not fit on the board." ]

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

                    let board =
                        { Width = options.Width
                          Height = options.Height
                          Pairs =
                            solution
                            |> List.map (fun path ->
                                { Label = path.Label
                                  Start = List.head path.Cells
                                  Finish = List.last path.Cells }) }

                    result <- Some { Puzzle = board; Solution = solution; Seed = options.Seed; Attempts = attempt }

            match result with
            | None ->
                Error $"No candidate found after {options.MaxAttempts} attempts (seed {options.Seed}). Try another seed or fewer/shorter paths. This does not prove impossibility."
            | Some generated ->
                match Validation.solution generated.Puzzle generated.Solution with
                | [] -> Ok generated
                | errors -> Error ("Internal solution validation failed: " + String.concat " " errors)
