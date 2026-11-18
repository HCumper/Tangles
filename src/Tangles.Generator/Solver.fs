namespace Tangles.Generator

open System
open System.Collections.Generic
open System.Numerics
open System.Security.Cryptography
open System.Text
open System.Threading
open Tangles.Core

/// A clue-only constraint solver for induced, disjoint paths. It never receives
/// the generator's witness or its path-length preferences.
module Solver =
    type Limits = { MaxWork: int; MaxSearchNodes: int }
    let defaults = { MaxWork = 2_000_000; MaxSearchNodes = 20_000 }

    type Analysis =
        { Method: string
          BoardKey: string
          Mode: string
          Status: string
          LimitReason: string
          Tier: string
          Grade: string
          Score: int
          Provisional: bool
          SolvedByReasoning: bool
          LocalDeductions: int
          RouteDeductions: int
          RoutePasses: int
          TrialSteps: int
          TrialAssumptions: int
          SearchNodes: int
          SolutionCount: int
          SolutionCountLimit: int
          SolutionCountComplete: bool
          Uniqueness: string
          Work: int
          WorkLimit: int }

    type Result = { Analysis: Analysis; Solutions: SolutionPath list array }
    type ShortestRouteScreen =
        { Method: string; Trials: int; TrialLimit: int; Solved: bool; FirstSuccessTrial: int option }

    exception private Contradiction
    exception private LimitReached of string

    /// Ignore labels, pair order and endpoint direction, but retain orientation
    /// and coverage. Used for both report identity and pack deduplication.
    let boardKey (board: Puzzle) =
        let index (c: Cell) = c.Y * board.Width + c.X
        let pairs =
            board.Pairs |> List.map (fun p -> min (index p.Start) (index p.Finish), max (index p.Start) (index p.Finish))
            |> List.sort |> List.map (fun (a,b) -> $"{a}:{b}") |> String.concat ";"
        let text = $"{board.Width}x{board.Height}:{board.RequireFullCoverage}:{pairs}"
        SHA256.HashData(Encoding.UTF8.GetBytes text) |> Convert.ToHexString

    let private neighbours width height =
        Array.init (width * height) (fun c ->
            [| if c % width > 0 then c - 1
               if c % width + 1 < width then c + 1
               if c >= width then c - width
               if c + width < width * height then c + width |])

    let private canonicalPairs (board: Puzzle) =
        let index (c: Cell) = c.Y * board.Width + c.X
        board.Pairs |> List.map (fun p ->
            if index p.Start <= index p.Finish then p else { p with Start = p.Finish; Finish = p.Start })
        |> List.sortBy (fun p -> index p.Start, index p.Finish) |> List.toArray

    let solve (cancellation: CancellationToken) limits (board: Puzzle) =
        cancellation.ThrowIfCancellationRequested()
        if limits.MaxWork < 1 || limits.MaxSearchNodes < 1 then invalidArg (nameof limits) "Solver budgets must be positive."
        match Validation.puzzle board with
        | [] -> ()
        | errors -> invalidArg (nameof board) (String.concat " " errors)
        let supported =
            board.Width <= 16 && board.Height <= 16
            && board.Pairs.Length <= (if board.RequireFullCoverage then 64 else 63)
        let mutable work, localDeductions, routeDeductions, routePasses = 0, 0, 0, 0
        let mutable trialSteps, trialAssumptions, searchNodes = 0, 0, 0
        let mutable tier, status, limitReason = "local", "complete", ""
        let mutable logical, exhaustive = false, false
        let solutions = ResizeArray<SolutionPath list>()

        if not supported then
            status <- "unsupported"
            tier <- "unknown"
            limitReason <- "Solver supports dimensions up to 16x16 and at most 64 full-coverage or 63 connect-only pairs."
        else
            let width, count = board.Width, board.Width * board.Height
            let pairs = canonicalPairs board
            let k = pairs.Length
            let nb = neighbours board.Width board.Height
            let index (c: Cell) = c.Y * width + c.X
            let ends = pairs |> Array.map (fun p -> index p.Start, index p.Finish)
            let emptyBit = if board.RequireFullCoverage then 0UL else 1UL <<< k
            let colours = k + (if board.RequireFullCoverage then 0 else 1)
            let all = if colours = 64 then UInt64.MaxValue else (1UL <<< colours) - 1UL
            let initial = Array.create count all
            let required = Array.create count 2
            for colour in 0 .. k - 1 do
                let a,b = ends[colour]
                initial[a] <- 1UL <<< colour
                initial[b] <- 1UL <<< colour
                required[a] <- 1
                required[b] <- 1

            let spend () =
                if work % 256 = 0 then cancellation.ThrowIfCancellationRequested()
                if work >= limits.MaxWork then raise (LimitReached "work")
                work <- work + 1

            let single d = d <> 0UL && (d &&& (d - 1UL)) = 0UL
            let update (dom: uint64 array) cell value recordLocal recordRoute =
                if value = 0UL then raise Contradiction
                let old = dom[cell]
                if value <> old then
                    let removed = BitOperations.PopCount old - BitOperations.PopCount value
                    if recordLocal then localDeductions <- localDeductions + removed
                    if recordRoute then routeDeductions <- routeDeductions + removed
                    dom[cell] <- value
                    true
                else false

            let local (dom: uint64 array) record =
                let mutable changed = true
                while changed do
                    changed <- false
                    for cell in 0 .. count - 1 do
                        let mutable bits = dom[cell] &&& (~~~emptyBit)
                        while bits <> 0UL do
                            spend ()
                            let bit = 1UL <<< BitOperations.TrailingZeroCount bits
                            bits <- bits &&& (bits - 1UL)
                            if dom[cell] &&& bit <> 0UL then
                                let mutable possible, fixedCount = 0, 0
                                for next in nb[cell] do
                                    if dom[next] &&& bit <> 0UL then possible <- possible + 1
                                    if dom[next] = bit then fixedCount <- fixedCount + 1
                                if possible < required[cell] || fixedCount > required[cell] then
                                    if update dom cell (dom[cell] &&& (~~~bit)) record false then changed <- true
                                elif dom[cell] = bit then
                                    for next in nb[cell] do
                                        if dom[next] &&& bit <> 0UL && dom[next] <> bit then
                                            if possible = required[cell] then
                                                if update dom next bit record false then changed <- true
                                            elif fixedCount = required[cell] then
                                                if update dom next (dom[next] &&& (~~~bit)) record false then changed <- true

            let routes (dom: uint64 array) record =
                let mutable changed = false
                for colour in 0 .. k - 1 do
                    spend ()
                    let bit = 1UL <<< colour
                    let a,b = ends[colour]
                    let discovery = Array.create count -1
                    let low = Array.zeroCreate count
                    let mandatory = Array.create count false
                    let mutable clock = 0
                    // Tarjan low links identify every vertex separating the two
                    // endpoints in one DFS, rather than one flood-fill per cell.
                    let rec visit cell parent =
                        spend ()
                        discovery[cell] <- clock
                        low[cell] <- clock
                        clock <- clock + 1
                        let mutable containsFinish = cell = b
                        for next in nb[cell] do
                            spend ()
                            if dom[next] &&& bit <> 0UL then
                                if discovery[next] < 0 then
                                    let childContainsFinish = visit next cell
                                    low[cell] <- min low[cell] low[next]
                                    if childContainsFinish && low[next] >= discovery[cell] then mandatory[cell] <- true
                                    containsFinish <- containsFinish || childContainsFinish
                                elif next <> parent then low[cell] <- min low[cell] discovery[next]
                        containsFinish
                    if not (visit a -1) then raise Contradiction
                    for cell in 0 .. count - 1 do
                        if dom[cell] &&& bit <> 0UL then
                            if discovery[cell] < 0 then
                                if update dom cell (dom[cell] &&& (~~~bit)) false record then changed <- true
                            elif mandatory[cell] then
                                if update dom cell bit false record then changed <- true
                if changed && record then routePasses <- routePasses + 1
                changed

            let propagate dom =
                let mutable changed = true
                while changed do
                    local dom false
                    changed <- routes dom false

            let materialise (dom: uint64 array) =
                let result =
                    pairs |> Array.mapi (fun colour pair ->
                        let bit = 1UL <<< colour
                        let a,b = ends[colour]
                        let cells = ResizeArray<Cell>()
                        let mutable current, previous = a, -1
                        let mutable finished = false
                        while not finished do
                            spend ()
                            cells.Add { X = current % width; Y = current / width }
                            if current = b then finished <- true
                            else
                                let choices = nb[current] |> Array.filter (fun n -> n <> previous && dom[n] = bit)
                                if choices.Length <> 1 || cells.Count > count then raise Contradiction
                                previous <- current
                                current <- choices[0]
                        if cells.Count <> (dom |> Array.sumBy (fun d -> if d = bit then 1 else 0)) then raise Contradiction
                        { Label = pair.Label; Cells = List.ofSeq cells }) |> List.ofArray
                match Validation.solution board result with
                | [] -> result
                | _ -> raise Contradiction

            let mutable domains = Array.copy initial
            try
                let mutable grading = true
                while grading do
                    local domains true
                    if domains |> Array.forall single then
                        solutions.Add (materialise domains)
                        logical <- true
                        exhaustive <- true
                        grading <- false
                    elif routes domains true then
                        if tier = "local" then tier <- "routes"
                    else
                        // Exhaust all single assumptions before calling this
                        // branching. A bounded/incomplete scan remains ungraded.
                        let mutable eliminated = false
                        let mutable cell = 0
                        while not eliminated && cell < count do
                            let mutable bits = if single domains[cell] then 0UL else domains[cell]
                            while not eliminated && bits <> 0UL do
                                let bit = 1UL <<< BitOperations.TrailingZeroCount bits
                                bits <- bits &&& (bits - 1UL)
                                trialAssumptions <- trialAssumptions + 1
                                let trial = Array.copy domains
                                trial[cell] <- bit
                                let contradictory =
                                    try propagate trial; false
                                    with Contradiction -> true
                                if contradictory then
                                    update domains cell (domains[cell] &&& (~~~bit)) false false |> ignore
                                    trialSteps <- trialSteps + 1
                                    tier <- "trial"
                                    eliminated <- true
                            cell <- cell + 1
                        if not eliminated then
                            tier <- "search"
                            grading <- false

                if not logical then
                    let rec search (dom: uint64 array) =
                        if solutions.Count >= 2 then false
                        else
                            spend ()
                            if searchNodes >= limits.MaxSearchNodes then raise (LimitReached "searchNodes")
                            searchNodes <- searchNodes + 1
                            try
                                propagate dom
                                let mutable best, size = -1, 65
                                for cell in 0 .. count - 1 do
                                    let choices = BitOperations.PopCount dom[cell]
                                    if choices > 1 && choices < size then best <- cell; size <- choices
                                if best < 0 then
                                    solutions.Add (materialise dom)
                                    true
                                else
                                    let mutable bits = dom[best]
                                    let mutable complete = true
                                    while bits <> 0UL && solutions.Count < 2 do
                                        let bit = 1UL <<< BitOperations.TrailingZeroCount bits
                                        bits <- bits &&& (bits - 1UL)
                                        let next = Array.copy dom
                                        next[best] <- bit
                                        complete <- search next && complete
                                    complete && bits = 0UL
                            with Contradiction -> true
                    exhaustive <- search domains
                    if exhaustive && solutions.Count = 0 then tier <- "unsatisfiable"
            with
            | Contradiction -> tier <- "unsatisfiable"; exhaustive <- true
            | LimitReached reason -> status <- "budgetExceeded"; limitReason <- reason

        let uniqueness =
            if solutions.Count >= 2 then "multiple"
            elif exhaustive && solutions.Count = 1 then "unique"
            elif exhaustive then "none"
            else "notEstablished"
        let grade, score =
            // Branching caused by multiple acceptable answers is ambiguity,
            // not evidence of difficulty. Never use it to award an expert grade.
            if not board.RequireFullCoverage || status <> "complete" || uniqueness <> "unique" then "unrated", 0
            else
                match tier with
                | "local" -> "easy", 10 + min 19 (localDeductions / max 1 (board.Width * board.Height))
                | "routes" -> "medium", 30 + min 24 routeDeductions
                | "trial" -> "hard", 55 + min 24 trialSteps
                | _ -> "expert", 80 + min 20 (int (2.0 * Math.Log2(float (searchNodes + 1))))
        { Analysis =
            { Method = "cell-domain-reasoning-v1"; BoardKey = boardKey board
              Mode = if board.RequireFullCoverage then "full-coverage" else "connect-only"
              Status = status; LimitReason = limitReason; Tier = tier; Grade = grade; Score = score; Provisional = true
              SolvedByReasoning = logical; LocalDeductions = localDeductions; RouteDeductions = routeDeductions
              RoutePasses = routePasses; TrialSteps = trialSteps; TrialAssumptions = trialAssumptions; SearchNodes = searchNodes
              SolutionCount = solutions.Count; SolutionCountLimit = 2; SolutionCountComplete = exhaustive
              Uniqueness = uniqueness; Work = work; WorkLimit = limits.MaxWork }
          Solutions = solutions.ToArray() }

    let analyse cancellation limits board = (solve cancellation limits board).Analysis

    /// A failed greedy screen proves only that these particular attempts failed.
    /// A successful result is checked as a real solution before being reported.
    let shortestRouteScreen (cancellation: CancellationToken) trials (board: Puzzle) =
        if trials < 1 then invalidArg (nameof trials) "At least one shortest-route trial is required."
        match Validation.puzzle board with
        | [] -> ()
        | errors -> invalidArg (nameof board) (String.concat " " errors)
        if board.RequireFullCoverage then invalidArg (nameof board) "The shortest-route screen is for connect-only puzzles."
        let nb = neighbours board.Width board.Height
        let pairs = canonicalPairs board
        let index (c: Cell) = c.Y * board.Width + c.X
        let count = board.Width * board.Height
        let owner = Array.create count -1
        for i in 0 .. pairs.Length - 1 do
            owner[index pairs[i].Start] <- i
            owner[index pairs[i].Finish] <- i
        let random = Random 0x54414E47
        let shuffle (a: 'a array) =
            for i in a.Length - 1 .. -1 .. 1 do
                let j = random.Next(i + 1)
                let temp = a[i]
                a[i] <- a[j]
                a[j] <- temp
        let mutable usedTrials = 0
        let mutable success = None
        while usedTrials < trials && success.IsNone do
            cancellation.ThrowIfCancellationRequested()
            let order = [| 0 .. pairs.Length - 1 |]
            if usedTrials = 0 then
                Array.sortInPlaceBy (fun i -> let p = pairs[i] in abs (p.Start.X-p.Finish.X) + abs (p.Start.Y-p.Finish.Y), i) order
            else shuffle order
            let occupied = Array.create count false
            let result = ResizeArray<SolutionPath>()
            let mutable failed = false
            for i in order do
                if not failed then
                    let a,b = index pairs[i].Start, index pairs[i].Finish
                    let previous = Array.create count -1
                    let queue = Queue<int>()
                    previous[a] <- a
                    queue.Enqueue a
                    while queue.Count > 0 && previous[b] < 0 do
                        cancellation.ThrowIfCancellationRequested()
                        let current = queue.Dequeue()
                        let choices = Array.copy nb[current]
                        shuffle choices
                        for next in choices do
                            if previous[next] < 0 && not occupied[next] && (owner[next] < 0 || owner[next] = i) then
                                previous[next] <- current
                                queue.Enqueue next
                    if previous[b] < 0 then failed <- true
                    else
                        let path = ResizeArray<Cell>()
                        let mutable current = b
                        while current <> a do
                            occupied[current] <- true
                            path.Add { X = current % board.Width; Y = current / board.Width }
                            current <- previous[current]
                        occupied[a] <- true
                        path.Add pairs[i].Start
                        result.Add { Label = pairs[i].Label; Cells = path |> Seq.rev |> List.ofSeq }
            usedTrials <- usedTrials + 1
            if not failed && Validation.solution board (List.ofSeq result) |> List.isEmpty then success <- Some usedTrials
        { Method = "greedy-shortest-routes-v1"; Trials = usedTrials; TrialLimit = trials; Solved = success.IsSome; FirstSuccessTrial = success }
