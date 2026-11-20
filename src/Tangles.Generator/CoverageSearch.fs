namespace Tangles.Generator

open System
open System.Threading

type internal CoverageLayout =
    { Order: int array; Positions: int array; Paths: int array array }

/// Hamiltonian traversal operations shared by the fast and solver-guided searches.
type internal CoverageSearch(width: int, height: int, pairCount: int, minLength: int, maxLength: int,
                             random: Random, cancellation: CancellationToken, ?cutStyle: CutStyle) =
    let cutStyle = defaultArg cutStyle RandomCuts
    let shuffle = GenerationSupport.shuffle
    let count = width * height
    let neighbours = GenerationSupport.neighbours width height
    let mutable evaluations, refinementEvaluations, structuredStarts = 0, 0, 0
    // Scratch arrays belong to this search, not to each candidate evaluation.
    let left = Array.zeroCreate count
    let ends = Array.zeroCreate count
    let needed = Array.zeroCreate (count + 1)
    let possible = lazy (Array.init (pairCount + 1) (fun _ -> Array.zeroCreate<bool> (count + 1)))
    let prefix = Array.zeroCreate<int> (count + 2)
    let maximum = min count maxLength
    let initial preferFewerStripes =
        let transpose =
            if preferFewerStripes && width <> height then width < height
            else random.Next(2) = 0
        let flipX, flipY = random.Next(2) = 0, random.Next(2) = 0
        let columns, rows = if transpose then height, width else width, height
        [| for row in 0 .. rows - 1 do
               for offset in 0 .. columns - 1 do
                   let column = if row % 2 = 0 then offset else columns - 1 - offset
                   let x, y = if transpose then row, column else column, row
                   let x = if flipX then width - 1 - x else x
                   let y = if flipY then height - 1 - y else y
                   yield y * width + x |]

    let positionsOf (order: int array) =
        let positions = Array.zeroCreate count
        for i in 0 .. count - 1 do positions[order[i]] <- i
        positions

    let analyse (order: int array) (positions: int array) =
        // left[j] is the earliest permitted start of a segment ending at j.
        evaluations <- evaluations + 1
        let mutable bound = 0
        for j in 0 .. count - 1 do
            cancellation.ThrowIfCancellationRequested()
            for neighbour in neighbours[order[j]] do
                if positions[neighbour] < j - 1 then bound <- max bound (positions[neighbour] + 1)
            left[j] <- bound
        let mutable finish = 0
        for start in 0 .. count - 1 do
            finish <- max finish start
            while finish < count && left[finish] <= start do finish <- finish + 1
            ends[start] <- min finish (start + maximum)
        Array.Fill(needed, count + 1)
        needed[count] <- 0
        for start in count - 1 .. -1 .. 0 do
            cancellation.ThrowIfCancellationRequested()
            for finish in start + minLength .. ends[start] do
                needed[start] <- min needed[start] (1 + needed[finish])
        ends, needed[0]

    let partition (order: int array) (ends: int array) =
        // Prefix sums make each range-of-possible-cuts query constant-time.
        let possible = possible.Value
        // Every row's writable band depends only on these fixed search bounds.
        // That entire band is overwritten below on every call; cells outside it
        // are never written and stay false. No whole-matrix clear is necessary.
        possible[0][count] <- true
        for paths in 1 .. pairCount do
            cancellation.ThrowIfCancellationRequested()
            prefix[0] <- 0
            for i in 0 .. count do prefix[i + 1] <- prefix[i] + (if possible[paths - 1][i] then 1 else 0)
            let first = max 0 (count - paths * maximum)
            let last = count - paths * minLength
            for start in first .. last do
                let minimumEnd = start + minLength
                let maximumEnd = ends[start]
                possible[paths][start] <- minimumEnd <= maximumEnd && prefix[maximumEnd + 1] > prefix[minimumEnd]
        if not (possible[pairCount][0]) then None
        else
            // Bounded local preferences: keep every choice DP-feasible and
            // retain substantial probability for straight/ordinary segments.
            let turns = if cutStyle = VariedCuts then Array.zeroCreate<int> (count + 1) else [||]
            let border = if cutStyle = VariedCuts then Array.zeroCreate<int> (count + 1) else [||]
            if cutStyle = VariedCuts then
                let onBorder cell = cell % width = 0 || cell % width = width-1 || cell / width = 0 || cell / width = height-1
                for i in 0 .. count-1 do
                    let turn = i > 0 && i+1 < count && order[i]-order[i-1] <> order[i+1]-order[i]
                    turns[i+1] <- turns[i] + (if turn then 1 else 0)
                    border[i+1] <- border[i] + (if onBorder order[i] then 1 else 0)
            let paths = ResizeArray<int array>()
            let mutable start = 0
            for remaining in pairCount .. -1 .. 1 do
                cancellation.ThrowIfCancellationRequested()
                let cuts =
                    [| for finish in start + minLength .. ends[start] do
                           if possible[remaining - 1][finish] then yield finish |]
                let finish =
                    if cutStyle = RandomCuts then cuts[random.Next cuts.Length]
                    else
                        let weights = cuts |> Array.map (fun finish ->
                            let length = finish-start
                            let bends = if length < 3 then 0 else turns[finish-1]-turns[start+1]
                            let boundary = float (border[finish]-border[start]) / float length
                            let average = float count / float pairCount
                            let variety = min 1.0 (abs (float length-average) / average)
                            1.0 + 0.3 * float (min 2 bends) - (if bends=0 then 0.2 else 0.0) - 0.1*boundary + 0.15*variety)
                        let target = random.NextDouble() * Array.sum weights
                        let mutable index, cumulative = 0, weights[0]
                        while index+1 < cuts.Length && cumulative <= target do
                            index <- index+1
                            cumulative <- cumulative+weights[index]
                        cuts[index]
                paths.Add order[start .. finish - 1]
                start <- finish
            let result = paths.ToArray()
            shuffle random result
            for path in result do if random.Next(2) = 0 then Array.Reverse path
            Some result

    // Reverse in place and update the inverse map. Mixing does not need the
    // partition DP, or a fresh traversal/position array for every move.
    let backbite (order: int array) (positions: int array) =
        let front = random.Next(2) = 0
        let endpoint, adjacent = if front then order[0], order[1] else order[count - 1], order[count - 2]
        let choices = neighbours[endpoint] |> Array.filter ((<>) adjacent)
        if choices.Length > 0 then
            let join = positions[choices[random.Next choices.Length]]
            let mutable left, right = if front then 0, join - 1 else join + 1, count - 1
            while left < right do
                let a, b = order[left], order[right]
                order[left] <- b
                order[right] <- a
                positions[a] <- right
                positions[b] <- left
                left <- left + 1
                right <- right - 1

    // This is an unguided walk: accepting only improvements here would keep
    // the traversal close to the easy-to-partition serpentine starting shape.
    let mixingMoves = min 16384 (max 256 (48 * count))
    let freshTraversal mix =
        let order = initial (not mix)
        let positions = positionsOf order
        if mix && width > 1 && height > 1 then
            for _ in 1 .. mixingMoves do
                cancellation.ThrowIfCancellationRequested()
                backbite order positions
        if not mix then structuredStarts <- structuredStarts + 1
        order, positions

    member _.Analyse(order, positions) = analyse order positions
    member _.Partition(order, ends) = partition order ends
    member _.Backbite(order, positions) = backbite order positions
    member _.Work =
        { GenerationWork.zero with TraversalEvaluations = evaluations
                                   RefinementEvaluations = refinementEvaluations
                                   StructuredStarts = structuredStarts }

    /// Both generators use this bounded annealing/search loop. A structured
    /// proposal is just another witness; difficulty selection still happens later.
    member _.Find(maxAttempts, restartEvery, refine, fallbackAt, bursts) =
        let mutable order, positions = freshTraversal (fallbackAt <> 0)
        let mutable score = Int32.MaxValue
        let mutable attempt, sinceRestart = 0, 0
        let mutable firstSuccess: int option = None
        let mutable result = None
        let twoDimensional = width > 1 && height > 1
        let refinement = if twoDimensional then refine else 0
        while attempt < maxAttempts
              && (twoDimensional || attempt = 0)
              && (firstSuccess.IsNone || attempt - firstSuccess.Value < refinement) do
            cancellation.ThrowIfCancellationRequested()
            if firstSuccess.IsNone && attempt > 0 && (sinceRestart = restartEvery || attempt = fallbackAt) then
                let a,b = freshTraversal (attempt <> fallbackAt)
                order <- a; positions <- b; score <- Int32.MaxValue
                sinceRestart <- 0
            let candidate, nextPositions = Array.copy order, Array.copy positions
            if sinceRestart > 0 then
                let moves = if bursts && sinceRestart % 8 = 0 then max 2 (count / 4) else 1
                for _ in 1 .. moves do
                    cancellation.ThrowIfCancellationRequested()
                    backbite candidate nextPositions
            if firstSuccess.IsSome then refinementEvaluations <- refinementEvaluations + 1
            attempt <- attempt + 1
            sinceRestart <- sinceRestart + 1
            let ends, nextScore = analyse candidate nextPositions
            if nextScore <= pairCount then
                match partition candidate ends with
                | Some paths ->
                    result <- Some { Order = candidate; Positions = nextPositions; Paths = paths }
                    if firstSuccess.IsNone then firstSuccess <- Some attempt
                | None -> ()
            if nextScore <= score || random.NextDouble() < exp (float (score-nextScore) / 0.4) then
                order <- candidate; positions <- nextPositions; score <- nextScore
        result
