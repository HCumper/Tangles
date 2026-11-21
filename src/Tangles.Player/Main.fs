module Tangles.Player.Main

open System
open System.Net.Http
open System.Threading.Tasks
open Bolero
open Bolero.Html
open Microsoft.AspNetCore.Components
open Microsoft.JSInterop
open Tangles.Core

let private palette =
    [| "#79d4eb"; "#ffae80"; "#b5a1f1"; "#a5d986"; "#f6d878"; "#ee9cbd"
       "#82e0ca"; "#e6aaee"; "#c1ca83"; "#8daff5"; "#e99d90"; "#c8c6e8"
       "#e3c49a"; "#a4cbbd"; "#e0d1f5"; "#dedf90" |]

let private coordinate value = string (value * 64 + 32)

type private PackChoice =
    { Key: string
      Name: string
      Source: string option
      Pack: PlayablePack }

let private packDimensions (loaded: PlayablePack) =
    loaded.Puzzles
    |> Array.map (fun entry -> entry.Puzzle.Width, entry.Puzzle.Height)
    |> Array.distinct
    |> Array.sort
    |> Array.map (fun (width, height) -> $"{width} × {height}")
    |> String.concat ", "

let private packDescription (loaded: PlayablePack) =
    let coverage = if loaded.Puzzles[0].Puzzle.RequireFullCoverage then "Full coverage required" else "Connections only"
    let count = if loaded.Puzzles.Length = 1 then "1 puzzle" else $"{loaded.Puzzles.Length} puzzles"
    $"{packDimensions loaded} · {coverage} · {count}"

type App() as this =
    inherit Component()

    let mutable pack: PlayablePack option = None
    let mutable game: PlayState option = None
    let mutable current = 0
    let mutable choices: PackChoice array = [||]
    let mutable selectedPack = ""
    let mutable nextImport = 0
    let mutable notice = ""
    let mutable busy = true
    let mutable reference: DotNetObjectReference<App> option = None

    [<Inject>]
    member val Http = Unchecked.defaultof<HttpClient> with get, set

    [<Inject>]
    member val JS = Unchecked.defaultof<IJSRuntime> with get, set

    [<Inject>]
    member val Navigation = Unchecked.defaultof<NavigationManager> with get, set

    member private _.Refresh() = this.StateHasChanged()

    member private _.CurrentKey =
        match pack with
        | Some loaded -> "tangles.player.v1." + loaded.Puzzles[current].Id
        | None -> ""

    member private _.HintPaths =
        match pack, game with
        | Some loaded, Some state ->
            loaded.Puzzles[current].Solution
            |> Option.map (fun solution -> Play.hintCandidates solution state)
            |> Option.defaultValue []
        | _ -> []

    member private _.Save() = task {
        match game with
        | Some state ->
            try
                let json = PuzzlePacks.writeProgress (Play.paths state)
                let! saved = this.JS.InvokeAsync<bool>("tangles.save", [| box this.CurrentKey; box json |]).AsTask()
                if not saved then notice <- "Browser storage is unavailable. You can keep playing, but progress will not be saved."
            with _ -> notice <- "Progress could not be saved in this browser."
        | None -> ()
    }

    member private _.Select(index: int) = task {
        if not busy then
            busy <- true
            do! this.Save()
            match pack with
            | None -> ()
            | Some loaded ->
                current <- max 0 (min index (loaded.Puzzles.Length - 1))
                let entry = loaded.Puzzles[current]
                let mutable next = Play.create entry.Puzzle
                notice <- ""
                try
                    let! json = this.JS.InvokeAsync<string>("tangles.load", [| box this.CurrentKey |]).AsTask()
                    if not (String.IsNullOrEmpty json) then
                        match PuzzlePacks.readProgress entry.Puzzle json with
                        | Ok paths ->
                            match Play.restore paths next with
                            | Ok restored -> next <- restored
                            | Error _ -> notice <- "Saved progress was invalid; this puzzle has been restarted."
                        | Error _ -> notice <- "Saved progress was invalid; this puzzle has been restarted."
                with _ -> notice <- "Browser storage is unavailable."
                game <- Some next
            busy <- false
            this.Refresh()
    }

    member private _.UsePack(choice: PackChoice, updateUrl: bool) = task {
        busy <- true
        this.Refresh()
        do! this.Save()
        pack <- Some choice.Pack
        selectedPack <- choice.Key
        game <- None
        if updateUrl then
            try
                do! this.JS.InvokeVoidAsync("tangles.selectPack", [| box (defaultArg choice.Source null) |]).AsTask()
            with _ -> ()
        busy <- false
        do! this.Select(0)
    }

    member private _.ChoosePack(key: string) = task {
        if not busy && key <> selectedPack then
            match choices |> Array.tryFind (fun choice -> choice.Key = key) with
            | Some choice -> do! this.UsePack(choice, true)
            | None -> ()
    }

    member private _.Load(json: string, name: string) = task {
        match PuzzlePacks.read json with
        | Error error ->
            notice <- error
            busy <- false
            this.Refresh()
        | Ok loaded ->
            let key =
                match choices |> Array.tryFind (fun choice -> choice.Source.IsNone && choice.Name = name) with
                | Some previous -> previous.Key
                | None -> nextImport <- nextImport + 1; $"import:{nextImport}"
            let choice = { Key = key; Name = name; Source = None; Pack = loaded }
            choices <- Array.append (choices |> Array.filter (fun previous -> previous.Key <> key)) [| choice |]
            do! this.UsePack(choice, true)
    }

    member private _.PackTarget(source: string) =
        let target = Uri(this.Http.BaseAddress, source)
        if target.Authority <> this.Http.BaseAddress.Authority || target.Scheme <> this.Http.BaseAddress.Scheme then
            failwith "Puzzle pack URLs must be on the same site."
        target

    member private _.FetchPack(name: string, source: string) = task {
        let target = this.PackTarget source
        let! json = this.Http.GetStringAsync target
        match PuzzlePacks.read json with
        | Error error -> return failwith error
        | Ok loaded ->
            return
                { Key = target.AbsoluteUri; Name = name
                  Source = Some (this.Http.BaseAddress.MakeRelativeUri(target).ToString()); Pack = loaded }
    }

    override _.OnInitializedAsync() = task {
        try
            let warnings = ResizeArray<string>()
            let! catalog = task {
                try
                    let! json = this.Http.GetStringAsync "packs.json"
                    return PuzzlePacks.readCatalog json
                with _ -> return Error "The pack list could not be loaded."
            }
            let entries =
                match catalog with
                | Ok entries -> entries
                | Error _ ->
                    warnings.Add "The pack list could not be loaded. You can still open a JSON puzzle pack."
                    [| { Name = "Starter collection"; Path = "packs/starter.json" } |]
            let! results =
                entries
                |> Array.map (fun entry -> task {
                    try
                        let! choice = this.FetchPack(entry.Name, entry.Path)
                        return Ok choice
                    with _ -> return Error entry.Name
                })
                |> Task.WhenAll
            choices <- results |> Array.choose (function Ok choice -> Some choice | Error _ -> None) |> Array.distinctBy _.Key
            let unavailable = results |> Array.choose (function Error name -> Some name | Ok _ -> None)
            if unavailable.Length > 0 then warnings.Add ("Some packs are unavailable: " + String.concat ", " unavailable + ".")
            let requested =
                (Uri this.Navigation.Uri).Query.TrimStart('?').Split('&', StringSplitOptions.RemoveEmptyEntries)
                |> Array.tryPick (fun parameter ->
                    let parts = parameter.Split('=', 2)
                    if parts.Length = 2 && parts[0] = "pack" then Some (Uri.UnescapeDataString parts[1]) else None)
            let! requestedChoice = task {
                match requested with
                | None -> return None
                | Some source ->
                    try
                        let target = this.PackTarget source
                        match choices |> Array.tryFind (fun choice -> choice.Key = target.AbsoluteUri) with
                        | Some choice -> return Some choice
                        | None ->
                            let name = target.Segments |> Array.last |> Uri.UnescapeDataString
                            let! choice = this.FetchPack(name, source)
                            choices <- Array.append choices [| choice |]
                            return Some choice
                    with _ ->
                        warnings.Add "The requested pack could not be loaded. Choose another pack or open a JSON file."
                        return None
            }
            let initial =
                requestedChoice
                |> Option.orElseWith (fun () ->
                    let defaultKey = (this.PackTarget "packs/starter.json").AbsoluteUri
                    choices |> Array.tryFind (fun choice -> choice.Key = defaultKey) |> Option.orElse (Array.tryHead choices))
            match initial with
            | Some choice ->
                do! this.UsePack(choice, false)
                if warnings.Count > 0 then notice <- String.concat " " (Seq.append [ notice ] warnings) |> _.Trim()
            | None ->
                busy <- false
                notice <- "No packs could be loaded. Open a JSON puzzle pack to begin."
        with _ ->
            busy <- false
            notice <- "The puzzle pack could not be loaded. Choose a generator JSON pack to begin."
        this.Refresh()
    }

    override _.OnAfterRenderAsync(_) = task {
        if reference.IsNone then reference <- Some (DotNetObjectReference.Create this)
        do! this.JS.InvokeVoidAsync("tangles.attach", [| box reference.Value |]).AsTask()
    }

    [<JSInvokable>]
    member _.ImportPack(json: string, name: string) = task {
        if not busy then do! this.Load(json, name)
    }

    [<JSInvokable>]
    member _.FileError(message: string) =
        notice <- message
        this.Refresh()

    [<JSInvokable>]
    member _.BoardInput(action: string, x: int, y: int) : Task<bool> = task {
        let mutable accepted = false
        if not busy then
            match game with
            | None -> ()
            | Some state ->
                let mutable feedback = ""
                let result =
                    match action with
                    | "begin" -> Play.start { X = x; Y = y } state
                    | "move" -> Play.move { X = x; Y = y } state
                    | "end" -> Ok (Play.finish state)
                    | "undo" -> Ok (Play.undo state)
                    | "reset" -> Ok (Play.reset state)
                    | "hint" ->
                        let candidates = this.HintPaths |> List.toArray
                        if candidates.Length = 0 then Error "No hint is available for this puzzle."
                        else
                            let path = candidates[Random.Shared.Next candidates.Length]
                            Play.reveal path state
                            |> Result.map (fun next ->
                                let cleared = state.Paths |> Map.toList |> List.filter (fun (label, _) -> not (next.Paths.ContainsKey label)) |> List.length
                                feedback <-
                                    if cleared = 0 then $"Filled colour {path.Label}. Undo will restore your previous paths."
                                    elif cleared = 1 then $"Filled colour {path.Label} and cleared one conflicting path. Undo restores both."
                                    else $"Filled colour {path.Label} and cleared {cleared} conflicting paths. Undo restores them."
                                next)
                    | _ -> Error "Unknown board action."
                match result with
                | Ok next -> game <- Some next; notice <- feedback; accepted <- true
                | Error message -> notice <- message
                if action = "end" || action = "undo" || action = "reset" || action = "hint" then do! this.Save()
                this.Refresh()
        return accepted
    }

    member private _.Button(text: string, disabled: bool, callback: unit -> unit) =
        button { attr.``type`` "button"; attr.disabled disabled; on.click (fun _ -> callback()); text }

    member private _.Board(state: PlayState) =
        let board = state.Puzzle
        let colour label =
            let index = board.Pairs |> List.findIndex (fun pair -> pair.Label = label)
            palette[index % palette.Length]
        elt "svg" {
            attr.id "puzzle-board"
            "viewBox" => $"0 0 {board.Width * 64} {board.Height * 64}"
            "data-width" => string board.Width
            "data-height" => string board.Height
            "data-puzzle" => this.CurrentKey
            "tabindex" => "0"
            "role" => "application"
            "aria-label" => $"{board.Width} by {board.Height} puzzle board"
            "aria-describedby" => "board-help"
            for y in 0 .. board.Height - 1 do
                for x in 0 .. board.Width - 1 do
                    elt "rect" {
                        "x" => string (x * 64); "y" => string (y * 64)
                        "width" => "64"; "height" => "64"; attr.``class`` "grid-cell"
                    }
            for path in Play.paths state do
                if path.Cells.Length > 1 then
                    elt "polyline" {
                        "points" => (path.Cells |> List.map (fun cell -> coordinate cell.X + "," + coordinate cell.Y) |> String.concat " ")
                        "fill" => "none"; "stroke" => colour path.Label; "stroke-width" => "15"
                        "stroke-linecap" => "round"; "stroke-linejoin" => "round"
                        attr.``class`` "drawn-path"
                    }
            for pair in board.Pairs do
                for cell in [ pair.Start; pair.Finish ] do
                    elt "circle" {
                        "cx" => coordinate cell.X; "cy" => coordinate cell.Y
                        "r" => "20"; "fill" => colour pair.Label; attr.``class`` "endpoint"
                    }
                    elt "text" {
                        "x" => coordinate cell.X; "y" => coordinate cell.Y
                        "text-anchor" => "middle"; "dy" => ".36em"; attr.``class`` "endpoint-label"
                        pair.Label
                    }
            elt "rect" {
                attr.id "keyboard-cursor"; "x" => "4"; "y" => "4"
                "width" => "56"; "height" => "56"; "rx" => "12"
            }
        }

    override _.Render() =
        let total = pack |> Option.map (fun p -> p.Puzzles.Length) |> Option.defaultValue 0
        let completed = game |> Option.map Play.completedCount |> Option.defaultValue 0
        let pairCount = game |> Option.map (fun g -> g.Puzzle.Pairs.Length) |> Option.defaultValue 0
        let solved = game |> Option.exists Play.isComplete
        let hintAvailable = not this.HintPaths.IsEmpty
        let fullCoverage = game |> Option.exists _.Puzzle.RequireFullCoverage
        let filled = game |> Option.map (Play.paths >> List.sumBy (fun path -> path.Cells.Length)) |> Option.defaultValue 0
        let squareCount = game |> Option.map (fun state -> state.Puzzle.Width * state.Puzzle.Height) |> Option.defaultValue 0
        div {
            attr.``class`` "shell"
            header {
                attr.``class`` "masthead"
                a { attr.href "./"; attr.``class`` "brand"; span { attr.``class`` "brand-mark"; "t" }; "tangles" }
                p {
                    attr.``class`` "lede"
                    if game.IsNone then "Load a puzzle pack to begin."
                    elif fullCoverage then "Connect each matching pair and fill every square."
                    else "Connect each matching pair. Empty squares can stay empty."
                }
            }
            main {
                div {
                    attr.``class`` "play-layout"
                    div {
                        attr.``class`` "pack-picker"
                        label { attr.``class`` "pack-label"; attr.``for`` "pack-choice"; "Choose a pack" }
                        select {
                            attr.id "pack-choice"; attr.value selectedPack; attr.disabled (busy || choices.Length = 0)
                            "aria-describedby" => "pack-description"
                            on.change (fun event -> this.ChoosePack(string event.Value) |> ignore)
                            if choices.Length = 0 then option { attr.value ""; if busy then "Loading packs…" else "Open a JSON puzzle pack" }
                            for choice in choices do
                                option { attr.value choice.Key; $"{choice.Name} — {packDescription choice.Pack}" }
                        }
                        p {
                            attr.id "pack-description"; attr.``class`` "pack-description"; "aria-live" => "polite"
                            match pack with
                            | Some loaded ->
                                span { packDimensions loaded; " · " }
                                span { attr.id "pack-mode"; attr.``class`` "pack-mode"; if fullCoverage then "Full coverage required" else "Connections only" }
                                span { " · "; if loaded.Puzzles.Length = 1 then "1 puzzle" else $"{loaded.Puzzles.Length} puzzles" }
                            | None -> "Choose a collection or open your own JSON pack."
                        }
                    }
                    section {
                        attr.``class`` "board-panel"
                        attr.style (match game with
                                    | Some state -> $"--board-columns:{state.Puzzle.Width};--board-rows:{state.Puzzle.Height}"
                                    | None -> "--board-columns:1;--board-rows:1")
                        div {
                            attr.``class`` "board-heading"
                            div { span { attr.``class`` "eyebrow"; "PUZZLE" }; strong { if total > 0 then $"{current + 1:D2} / {total:D2}" else "—" } }
                            span { attr.``class`` "board-size"; match game with Some g -> $"{g.Puzzle.Width} × {g.Puzzle.Height}" | None -> "" }
                        }
                        div {
                            attr.``class`` "board-stage"
                            div {
                                attr.``class`` "board-frame"
                                match game with
                                | Some state -> this.Board state
                                | None -> div { attr.``class`` "board-placeholder"; if busy then "Getting your puzzle ready…" else "Load a puzzle pack to play." }
                            }
                        }
                        div {
                            attr.``class`` "board-toolbar"
                            div {
                                attr.``class`` "edit-actions"
                                this.Button("↶ Undo", busy || (game |> Option.forall (fun s -> s.History.IsEmpty && s.BeforeStroke.IsNone)), fun () -> this.BoardInput("undo", 0, 0) |> ignore)
                                this.Button("Reset", busy || (game |> Option.forall _.Paths.IsEmpty), fun () -> this.BoardInput("reset", 0, 0) |> ignore)
                                button {
                                    attr.id "hint-button"; attr.``type`` "button"
                                    attr.disabled (busy || not hintAvailable)
                                    "title" => (if solved then "Puzzle complete" elif not hintAvailable then "Hints need a valid stored solution in the puzzle pack" else "Fill a random colour using the stored solution")
                                    on.click (fun _ -> this.BoardInput("hint", 0, 0) |> ignore)
                                    "Hint"
                                }
                            }
                            span { attr.``class`` "save-note"; "Progress stays in this browser" }
                        }
                    }
                    aside {
                        attr.``class`` "sidebar"
                        div { attr.``class`` "notice"; "role" => "status"; "aria-live" => "polite"; if notice <> "" then notice elif solved then "Puzzle complete. Any valid route counts." else "" }
                        section {
                            attr.``class`` (if solved then "progress-card solved" else "progress-card")
                            p { attr.``class`` "eyebrow"; if solved then "NICELY UNTANGLED" else "YOUR CONNECTIONS" }
                            div { attr.``class`` "progress-number"; string completed; span { $" / {pairCount}" } }
                            p {
                                if solved && fullCoverage then "Every pair connected. Every square filled."
                                elif solved then "Every pair has found its way."
                                elif fullCoverage && completed = pairCount then "All pairs connected. Adjust your paths to fill the remaining squares."
                                else "One connection at a time."
                            }
                            if fullCoverage then p { $"{filled} / {squareCount} squares filled" }
                            div {
                                attr.``class`` "pair-list"
                                match game with
                                | None -> ()
                                | Some state ->
                                    for i, pair in List.indexed state.Puzzle.Pairs do
                                        let connected =
                                            Map.tryFind pair.Label state.Paths
                                            |> Option.exists (fun cells -> cells.Length > 1 && (List.last cells = pair.Start || List.last cells = pair.Finish))
                                        span {
                                            attr.``class`` (if connected then "pair connected" else "pair")
                                            attr.style $"--pair-colour:{palette[i % palette.Length]}"
                                            "aria-label" => (pair.Label + if connected then " connected" else " unconnected")
                                            pair.Label
                                            if connected then span { "✓" }
                                        }
                            }
                            div {
                                attr.``class`` "navigation"
                                this.Button("← Previous", busy || current = 0, fun () -> this.Select(current - 1) |> ignore)
                                this.Button("Next →", busy || current + 1 >= total, fun () -> this.Select(current + 1) |> ignore)
                            }
                            label { attr.``class`` "jump-label"; attr.``for`` "puzzle-choice"; "Jump to puzzle" }
                            select {
                                attr.id "puzzle-choice"; attr.disabled (busy || total = 0); attr.value (string current)
                                on.change (fun event ->
                                    match Int32.TryParse(string event.Value) with
                                    | true, index -> this.Select(index) |> ignore
                                    | _ -> ())
                                for index in 0 .. total - 1 do
                                    option { attr.value (string index); $"Puzzle {index + 1}" }
                            }
                        }
                        elt "details" {
                            attr.``class`` "how-card"
                            elt "summary" { "How to play" }
                            ol {
                                li { "Drag from a letter to its matching partner." }
                                li { "Keep paths apart. A path cannot touch its own sides." }
                                li { "Retrace to shorten a path, or start again from either endpoint." }
                                if game.IsSome then
                                    li {
                                        if fullCoverage then "Fill every square to finish the puzzle."
                                        else "Empty squares are allowed once every pair is connected."
                                    }
                            }
                            p { attr.id "board-help"; attr.``class`` "keyboard-help"; "Keyboard: focus the board, use arrows to move, Space to start or stop drawing. Ctrl+Z undoes." }
                        }
                        elt "details" {
                            attr.``class`` "pack-card"
                            elt "summary" { "Open your own puzzle pack" }
                            p { "Open a JSON puzzle pack from the Tangles generator." }
                            label { attr.``for`` "pack-file"; attr.``class`` "file-button"; "↑ Load puzzle pack" }
                            input { attr.id "pack-file"; attr.``type`` "file"; "accept" => ".json,application/json"; attr.disabled busy }
                            span { attr.``class`` "file-note"; "Stays on your device · up to 5 MB" }
                        }
                        footer {
                            if game.IsNone then "No timer. Just find your way."
                            elif fullCoverage then "No timer. Connect every pair and fill the board."
                            else "No timer. No need to fill the board. Just find your way."
                        }
                    }
                }
            }
        }

    interface IDisposable with
        member _.Dispose() = reference |> Option.iter _.Dispose()
