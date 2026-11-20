namespace Tangles.Core

type PlayState =
    { Puzzle: Puzzle
      Paths: Map<string, Cell list>
      Active: string option
      History: Map<string, Cell list> list
      BeforeStroke: Map<string, Cell list> option }

module Play =
    let create board = { Puzzle = board; Paths = Map.empty; Active = None; History = []; BeforeStroke = None }
    let paths state : SolutionPath list = state.Paths |> Map.toList |> List.map (fun (label, cells) -> { Label = label; Cells = cells })
    let isComplete state = Validation.solution state.Puzzle (paths state) |> List.isEmpty
    let completedCount state =
        paths state
        |> List.sumBy (fun path ->
            let pair = state.Puzzle.Pairs |> List.find (fun p -> p.Label = path.Label)
            let last = List.last path.Cells
            if path.Cells.Length > 1 && (last = pair.Start || last = pair.Finish) then 1 else 0)

    let finish state =
        let history =
            match state.BeforeStroke with
            | Some before when before <> state.Paths -> before :: state.History |> List.truncate 100
            | _ -> state.History
        { state with Active = None; BeforeStroke = None; History = history }

    let start cell state =
        let state = finish state
        let endpoint = state.Puzzle.Pairs |> List.tryFind (fun p -> p.Start = cell || p.Finish = cell)
        let existing = state.Paths |> Map.toList |> List.tryFind (fun (_, cells) -> List.contains cell cells)
        let selected =
            match endpoint, existing with
            | Some pair, _ -> Some (pair.Label, [ cell ])
            | None, Some (label, cells) ->
                let index = cells |> List.findIndex ((=) cell)
                Some (label, List.take (index + 1) cells)
            | _ -> None
        match selected with
        | None -> Error "Start at a lettered endpoint or an existing path."
        | Some (label, cells) ->
            Ok { state with Paths = Map.add label cells state.Paths; Active = Some label; BeforeStroke = Some state.Paths }

    let move cell state =
        match state.Active with
        | None -> Ok state
        | Some label ->
            let cells = state.Paths[label]
            let last = List.last cells
            if last = cell then Ok state
            elif not (Cell.areAdjacent last cell) then Error "Move one square horizontally or vertically."
            else
                let existingIndex = List.tryFindIndex ((=) cell) cells
                let pair = state.Puzzle.Pairs |> List.find (fun p -> p.Label = label)
                let alreadyConnected = cells.Length > 1 && (last = pair.Start || last = pair.Finish)
                let candidate =
                    match existingIndex with
                    | Some index -> Some (List.take (index + 1) cells)
                    | None when alreadyConnected -> None
                    | None -> Some (cells @ [ cell ])
                match candidate with
                | None -> Error "This pair is connected. Retrace or start another path."
                | Some updated ->
                    let next = { state with Paths = Map.add label updated state.Paths }
                    match Validation.partialSolution state.Puzzle (paths next) with
                    | [] -> Ok next
                    | errors -> Error (List.head errors)

    let undo state =
        let state = finish state
        match state.History with
        | before :: rest -> { state with Paths = before; History = rest }
        | [] -> state

    let reset state =
        let state = finish state
        if state.Paths.IsEmpty then state
        else { state with Paths = Map.empty; History = state.Paths :: state.History |> List.truncate 100 }

    /// Prefer unconnected colours. If full coverage is still incomplete after
    /// all pairs connect, offer routes that differ from the stored solution.
    let hintCandidates (solution: SolutionPath list) state =
        if isComplete state then []
        else
            let unfinished =
                solution |> List.filter (fun path ->
                    let pair = state.Puzzle.Pairs |> List.find (fun pair -> pair.Label = path.Label)
                    state.Paths |> Map.tryFind path.Label
                    |> Option.forall (fun cells ->
                        cells.Length < 2 || (List.last cells <> pair.Start && List.last cells <> pair.Finish)))
            if not unfinished.IsEmpty then unfinished
            else
                solution |> List.filter (fun path ->
                    state.Paths |> Map.tryFind path.Label
                    |> Option.forall (fun cells -> cells <> path.Cells && cells <> List.rev path.Cells))

    /// Reveal one whole route, removing only paths it intersects. The complete
    /// change, including removed paths, is a single undoable action.
    let reveal (path: SolutionPath) state =
        let joinsEndpoints =
            state.Puzzle.Pairs |> List.tryFind (fun pair -> pair.Label = path.Label)
            |> Option.exists (fun pair ->
                match path.Cells with
                | first :: _ when path.Cells.Length > 1 ->
                    let last = List.last path.Cells
                    (first = pair.Start && last = pair.Finish) || (first = pair.Finish && last = pair.Start)
                | _ -> false)
        match Validation.partialSolution state.Puzzle [ path ] with
        | _ :: _ as errors -> Error (String.concat " " errors)
        | [] when not joinsEndpoints -> Error "A hint must connect its matching endpoints."
        | [] ->
            let state = finish state
            let occupied = Set.ofList path.Cells
            let nextPaths =
                state.Paths
                |> Map.filter (fun label cells -> label <> path.Label && not (cells |> List.exists occupied.Contains))
                |> Map.add path.Label path.Cells
            let history =
                if nextPaths = state.Paths then state.History
                else state.Paths :: state.History |> List.truncate 100
            Ok { state with Paths = nextPaths; History = history }

    let restore saved state =
        match Validation.partialSolution state.Puzzle saved with
        | [] -> Ok { state with Paths = saved |> List.map (fun p -> p.Label, p.Cells) |> Map.ofList; Active = None; BeforeStroke = None; History = [] }
        | errors -> Error (String.concat " " errors)
