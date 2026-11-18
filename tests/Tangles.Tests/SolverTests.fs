module Tangles.SolverTests

open System
open System.Threading
open Tangles.Core
open Tangles.Generator
open Xunit

let private cell x y = { X = x; Y = y }
let private pair name a b = { Label = name; Start = a; Finish = b }
let private board full width height pairs = { Width = width; Height = height; RequireFullCoverage = full; Pairs = pairs }

// An independent oracle enumerates entire induced paths and combines disjoint
// paths. It shares neither the cell-domain model nor any solver rule.
let private oracle (puzzle: Puzzle) =
    let adjacent a b = abs (a.X-b.X) + abs (a.Y-b.Y) = 1
    let allCells = [for y in 0 .. puzzle.Height-1 do for x in 0 .. puzzle.Width-1 -> cell x y]
    let endpoints = puzzle.Pairs |> List.collect (fun p -> [p.Start;p.Finish]) |> Set.ofList
    let routes (p: EndpointPair) =
        let result = ResizeArray<Set<Cell>>()
        let rec extend path =
            let current = List.head path
            if current = p.Finish then result.Add (Set.ofList path)
            else
                for next in allCells do
                    if adjacent current next && not (List.contains next path)
                       && (next = p.Finish || not (Set.contains next endpoints))
                       && (List.tail path |> List.forall (fun previous -> not (adjacent next previous))) then
                        extend (next::path)
        extend [p.Start]
        List.ofSeq result
    let mutable found = 0
    let rec combine occupied remaining =
        if found < 2 then
            match remaining with
            | [] when not puzzle.RequireFullCoverage || Set.count occupied = allCells.Length -> found <- found + 1
            | [] -> ()
            | choices::rest ->
                for path in choices do
                    if Set.isEmpty (Set.intersect occupied path) then combine (Set.union occupied path) rest
    combine Set.empty (List.map routes puzzle.Pairs)
    found

[<Theory>]
[<InlineData(false)>]
[<InlineData(true)>]
let ``solver counts match exhaustive independent paths on every two-pair 3x3 clue layout`` full =
    let cells = [|for y in 0..2 do for x in 0..2 -> cell x y|]
    for a in 0..5 do
        for b in a+1..6 do
            for c in b+1..7 do
                for d in c+1..8 do
                    for (p,q,r,s) in [(a,b,c,d);(a,c,b,d);(a,d,b,c)] do
                        let puzzle = board full 3 3 [pair "A" cells[p] cells[q];pair "B" cells[r] cells[s]]
                        let expected = oracle puzzle
                        let result = Solver.solve CancellationToken.None Solver.defaults puzzle
                        Assert.Equal("complete", result.Analysis.Status)
                        Assert.Equal(expected, result.Analysis.SolutionCount)
                        if expected = 1 then Assert.Equal("unique", result.Analysis.Uniqueness)
                        if expected = 0 then Assert.Equal("none", result.Analysis.Uniqueness)
                        if expected = 2 then Assert.Equal("multiple", result.Analysis.Uniqueness)
                        for paths in result.Solutions do Assert.Empty(Validation.solution puzzle paths)

[<Fact>]
let ``single paths and optional empty cells agree with independent enumeration`` () =
    for full in [false;true] do
        for a in 0..7 do
            for b in a+1..8 do
                let puzzle = board full 3 3 [pair "A" (cell (a%3) (a/3)) (cell (b%3) (b/3))]
                let report = Solver.analyse CancellationToken.None Solver.defaults puzzle
                Assert.Equal(oracle puzzle, report.SolutionCount)
                Assert.Equal("complete", report.Status)

let hardFixture =
    board true 8 8
        [ pair "A" (cell 7 3) (cell 1 5); pair "B" (cell 1 6) (cell 3 6)
          pair "C" (cell 2 0) (cell 3 7); pair "D" (cell 3 0) (cell 5 6)
          pair "E" (cell 2 3) (cell 6 6); pair "F" (cell 6 1) (cell 3 3) ]

[<Fact>]
let ``hard reference puzzle needs contradiction trials and has a checked unique solution`` () =
    let result = Solver.solve CancellationToken.None { Solver.defaults with MaxWork = 20_000_000 } hardFixture
    Assert.Equal("complete", result.Analysis.Status)
    Assert.Equal("trial", result.Analysis.Tier)
    Assert.Equal("hard", result.Analysis.Grade)
    Assert.True(result.Analysis.TrialSteps > 0)
    Assert.True(result.Analysis.SolvedByReasoning)
    Assert.Equal("unique", result.Analysis.Uniqueness)
    Assert.Single(result.Solutions) |> ignore
    Assert.Empty(Validation.solution hardFixture result.Solutions[0])

[<Fact>]
let ``limits never turn an unfinished search into a unique or difficult verdict`` () =
    let report = Solver.analyse CancellationToken.None { Solver.defaults with MaxWork = 1 } hardFixture
    Assert.Equal("budgetExceeded", report.Status)
    Assert.Equal("notEstablished", report.Uniqueness)
    Assert.False(report.SolutionCountComplete)
    Assert.Equal("unrated", report.Grade)
    Assert.Equal(1, report.Work)

[<Fact>]
let ``a search node cutoff does not certify uniqueness`` () =
    let puzzle = board false 3 3 [pair "A" (cell 0 0) (cell 2 2)]
    let report = Solver.analyse CancellationToken.None { Solver.defaults with MaxSearchNodes = 1 } puzzle
    Assert.Equal("budgetExceeded",report.Status)
    Assert.Equal("searchNodes",report.LimitReason)
    Assert.Equal("notEstablished",report.Uniqueness)
    Assert.False(report.SolutionCountComplete)

[<Theory>]
[<InlineData(true,64)>]
[<InlineData(false,63)>]
let ``highest domain bit works for 64 full-coverage labels or an empty-cell value`` full pairs =
    let clues =
        [for i in 0 .. pairs-1 do
            let a,b=2*i,2*i+1
            yield pair (string i) (cell (a%16) (a/16)) (cell (b%16) (b/16))]
    let puzzle = board full 16 8 clues
    let result = Solver.solve CancellationToken.None Solver.defaults puzzle
    Assert.Equal("complete",result.Analysis.Status)
    Assert.Equal("unique",result.Analysis.Uniqueness)
    Assert.Single(result.Solutions) |> ignore
    Assert.Empty(Validation.solution puzzle result.Solutions[0])

[<Fact>]
let ``reasoning is invariant to label renaming pair order and endpoint direction`` () =
    let renamed = { hardFixture with Pairs = hardFixture.Pairs |> List.rev |> List.mapi (fun i p -> { p with Label = string i; Start = p.Finish; Finish = p.Start }) }
    let limits = { Solver.defaults with MaxWork = 20_000_000 }
    Assert.Equal(Solver.analyse CancellationToken.None limits hardFixture, Solver.analyse CancellationToken.None limits renamed)

[<Fact>]
let ``solver and shortest-route screen respect cancellation`` () =
    use cancellation = new CancellationTokenSource()
    cancellation.Cancel()
    Assert.Throws<OperationCanceledException>(fun () -> Solver.analyse cancellation.Token Solver.defaults hardFixture |> ignore) |> ignore
    Assert.Throws<OperationCanceledException>(fun () -> Solver.shortestRouteScreen cancellation.Token 32 { hardFixture with RequireFullCoverage = false } |> ignore) |> ignore

[<Fact>]
let ``shortest-route wins are real solutions and connect-only ambiguity is not labelled expert`` () =
    let puzzle = board false 3 3 [pair "A" (cell 0 0) (cell 2 2)]
    let screen = Solver.shortestRouteScreen CancellationToken.None 32 puzzle
    Assert.True(screen.Solved)
    Assert.Equal(Some 1, screen.FirstSuccessTrial)
    let report = Solver.analyse CancellationToken.None Solver.defaults puzzle
    Assert.Equal("multiple", report.Uniqueness)
    Assert.Equal("unrated", report.Grade)

[<Fact>]
let ``multiple full-coverage answers are ambiguity rather than an expert grade`` () =
    let puzzle =
        board true 8 8
            [pair "A" (cell 5 6) (cell 5 1); pair "B" (cell 6 1) (cell 5 3)
             pair "C" (cell 6 3) (cell 0 1); pair "D" (cell 6 6) (cell 1 2)
             pair "E" (cell 2 2) (cell 3 5); pair "F" (cell 1 1) (cell 2 5)]
    let result = Solver.solve CancellationToken.None Solver.defaults puzzle
    Assert.Equal("complete",result.Analysis.Status)
    Assert.Equal("multiple",result.Analysis.Uniqueness)
    Assert.Equal("unrated",result.Analysis.Grade)
    Assert.Equal(2,result.Solutions.Length)
    for solution in result.Solutions do Assert.Empty(Validation.solution puzzle solution)
