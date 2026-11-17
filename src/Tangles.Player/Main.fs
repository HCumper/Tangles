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

type App() as this =
    inherit Component()

    let mutable pack: PlayablePack option = None
    let mutable game: PlayState option = None
    let mutable current = 0
    let mutable packName = "Starter collection"
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

    member private _.Load(json: string, name: string) = task {
        match PuzzlePacks.read json with
        | Error error ->
            notice <- error
            busy <- false
            this.Refresh()
        | Ok loaded ->
            do! this.Save()
            pack <- Some loaded
            packName <- name
            game <- None
            busy <- false
            do! this.Select(0)
    }

    override _.OnInitializedAsync() = task {
        try
            let requested =
                (Uri this.Navigation.Uri).Query.TrimStart('?').Split('&', StringSplitOptions.RemoveEmptyEntries)
                |> Array.tryPick (fun parameter ->
                    let parts = parameter.Split('=', 2)
                    if parts.Length = 2 && parts[0] = "pack" then Some (Uri.UnescapeDataString parts[1]) else None)
            let source = defaultArg requested "packs/starter.json"
            let target = Uri(this.Http.BaseAddress, source)
            if target.Authority <> this.Http.BaseAddress.Authority || target.Scheme <> this.Http.BaseAddress.Scheme then
                failwith "Puzzle pack URLs must be on the same site."
            let! json = this.Http.GetStringAsync target
            let name = if requested.IsNone then "Starter collection" else target.Segments |> Array.last |> Uri.UnescapeDataString
            do! this.Load(json, name)
        with _ ->
            busy <- false
            notice <- "The puzzle pack could not be loaded. Choose a generator JSON pack to begin."
    }

    override _.OnAfterRenderAsync(_) = task {
        if reference.IsNone then reference <- Some (DotNetObjectReference.Create this)
        do! this.JS.InvokeVoidAsync("tangles.attach", [| box reference.Value |]).AsTask()
    }

    [<JSInvokable>]
    member _.ImportPack(json: string, name: string) = this.Load(json, name)

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
                let result =
                    match action with
                    | "begin" -> Play.start { X = x; Y = y } state
                    | "move" -> Play.move { X = x; Y = y } state
                    | "end" -> Ok (Play.finish state)
                    | "undo" -> Ok (Play.undo state)
                    | "reset" -> Ok (Play.reset state)
                    | _ -> Error "Unknown board action."
                match result with
                | Ok next -> game <- Some next; notice <- ""; accepted <- true
                | Error message -> notice <- message
                if action = "end" || action = "undo" || action = "reset" then do! this.Save()
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
        div {
            attr.``class`` "shell"
            header {
                attr.``class`` "masthead"
                a { attr.href "./"; attr.``class`` "brand"; span { attr.``class`` "brand-mark"; "t" }; "tangles" }
                span { attr.``class`` "masthead-note"; "A little room to think." }
            }
            main {
                div {
                    attr.``class`` "intro"
                    div {
                        p { attr.``class`` "eyebrow"; "THE CONNECTION GAME" }
                        h1 { "Find a way through." }
                        p { attr.``class`` "lede"; "Connect each matching pair. Empty squares can stay empty." }
                    }
                    div { attr.``class`` "collection-tag"; span { attr.``class`` "small-dot" }; packName }
                }
                div {
                    attr.``class`` "play-layout"
                    section {
                        attr.``class`` "board-panel"
                        div {
                            attr.``class`` "board-heading"
                            div { span { attr.``class`` "eyebrow"; "PUZZLE" }; strong { if total > 0 then $"{current + 1:D2} / {total:D2}" else "—" } }
                            span { attr.``class`` "board-size"; match game with Some g -> $"{g.Puzzle.Width} × {g.Puzzle.Height}" | None -> "" }
                        }
                        div {
                            attr.``class`` "board-frame"
                            match game with
                            | Some state -> this.Board state
                            | None -> div { attr.``class`` "board-placeholder"; if busy then "Getting your puzzle ready…" else "Load a puzzle pack to play." }
                        }
                        div {
                            attr.``class`` "board-toolbar"
                            div {
                                attr.``class`` "edit-actions"
                                this.Button("↶ Undo", busy || (game |> Option.forall (fun s -> s.History.IsEmpty && s.BeforeStroke.IsNone)), fun () -> this.BoardInput("undo", 0, 0) |> ignore)
                                this.Button("Reset", busy || (game |> Option.forall _.Paths.IsEmpty), fun () -> this.BoardInput("reset", 0, 0) |> ignore)
                            }
                            span { attr.``class`` "save-note"; "Progress stays in this browser" }
                        }
                    }
                    aside {
                        attr.``class`` "sidebar"
                        section {
                            attr.``class`` (if solved then "progress-card solved" else "progress-card")
                            p { attr.``class`` "eyebrow"; if solved then "NICELY UNTANGLED" else "YOUR CONNECTIONS" }
                            div { attr.``class`` "progress-number"; string completed; span { $" / {pairCount}" } }
                            p { if solved then "Every pair has found its way." else "One connection at a time." }
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
                        section {
                            attr.``class`` "how-card"
                            h2 { "Make the connection" }
                            ol {
                                li { "Drag from a letter to its matching partner." }
                                li { "Keep paths apart. A path cannot touch its own sides." }
                                li { "Retrace to shorten a path, or start again from either endpoint." }
                            }
                            p { attr.id "board-help"; attr.``class`` "keyboard-help"; "Keyboard: focus the board, use arrows to move, Space to start or stop drawing. Ctrl+Z undoes." }
                        }
                        section {
                            attr.``class`` "pack-card"
                            h2 { "Bring your own tangles" }
                            p { "Open a JSON puzzle pack from the Tangles generator." }
                            label { attr.``for`` "pack-file"; attr.``class`` "file-button"; "↑ Load puzzle pack" }
                            input { attr.id "pack-file"; attr.``type`` "file"; "accept" => ".json,application/json"; attr.disabled busy }
                            span { attr.``class`` "file-note"; "Stays on your device · up to 5 MB" }
                        }
                    }
                }
                div { attr.``class`` "notice"; "role" => "status"; "aria-live" => "polite"; if notice <> "" then notice elif solved then "Puzzle complete. Any valid route counts." else "" }
            }
            footer { "No timer. No need to fill the board. Just find your way." }
        }

    interface IDisposable with
        member _.Dispose() = reference |> Option.iter _.Dispose()
