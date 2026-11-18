namespace Tangles.Generator

open System
open System.Threading

/// Hamiltonian traversal operations shared by the fast and solver-guided searches.
type internal CoverageSearch(width: int, height: int, pairCount: int, minLength: int, maxLength: int,
                             random: Random, cancellation: CancellationToken) =
    let shuffle (random: Random) (items: 'a array) =
        for i in items.Length - 1 .. -1 .. 1 do
            let j = random.Next(i + 1)
            let value = items[i]
            items[i] <- items[j]
            items[j] <- value

    let count = width * height
    let neighbours =
        Array.init count (fun cell ->
            [| if cell % width > 0 then cell - 1
               if cell % width + 1 < width then cell + 1
               if cell >= width then cell - width
               if cell + width < count then cell + width |])
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
            for finish in start + minLength .. ends[start] do
                needed[start] <- min needed[start] (1 + needed[finish])
        ends, needed[0]

    let partition (order: int array) (ends: int array) =
        // Prefix sums make each range-of-possible-cuts query constant-time.
        let possible = Array.init (pairCount + 1) (fun _ -> Array.zeroCreate<bool> (count + 1))
        possible[0][count] <- true
        let prefix = Array.zeroCreate<int> (count + 2)
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
            let paths = ResizeArray<int array>()
            let mutable start = 0
            for remaining in pairCount .. -1 .. 1 do
                cancellation.ThrowIfCancellationRequested()
                let cuts =
                    [| for finish in start + minLength .. ends[start] do
                           if possible[remaining - 1][finish] then yield finish |]
                let finish = cuts[random.Next cuts.Length]
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
        let _, score = analyse order positions
        order, positions, score

    member _.Fresh(mix) = freshTraversal mix
    member _.Analyse(order, positions) = analyse order positions
    member _.Partition(order, ends) = partition order ends
    member _.Backbite(order, positions) = backbite order positions
