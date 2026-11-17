namespace Tangles.Core

open System
open System.Text.Json
open System.IO
open System.Text

type PlayablePuzzle = { Id: string; Puzzle: Puzzle }
type PlayablePack = { Puzzles: PlayablePuzzle array }
type PackCatalogEntry = { Name: string; Path: string }

module PuzzlePacks =
    let private property name (element: JsonElement) =
        match element.TryGetProperty(name: string) with
        | true, value -> value
        | _ -> failwith $"Missing field '{name}'."

    let private integer name element = (property name element).GetInt32()
    let private flag name element =
        match (property name element).ValueKind with
        | JsonValueKind.True -> true
        | JsonValueKind.False -> false
        | _ -> failwith $"Field '{name}' must be true or false."
    let private text name element =
        let value = (property name element).GetString()
        if String.IsNullOrWhiteSpace value || value.Length > 128 then failwith $"Invalid '{name}'."
        value

    let private cell (element: JsonElement) = { X = integer "x" element; Y = integer "y" element }

    /// Static hosts list available packs explicitly; descriptions come from the
    /// validated packs themselves rather than duplicated catalogue metadata.
    let readCatalog (json: string) : Result<PackCatalogEntry array, string> =
        try
            if json.Length > 1024 * 1024 then failwith "The pack list is too large."
            use document = JsonDocument.Parse(json, JsonDocumentOptions(MaxDepth = 8))
            let entries = property "packs" document.RootElement
            if entries.GetArrayLength() < 1 || entries.GetArrayLength() > 1000 then
                failwith "The pack list must contain between 1 and 1,000 entries."
            entries.EnumerateArray()
            |> Seq.map (fun entry -> { Name = text "name" entry; Path = text "path" entry })
            |> Array.ofSeq
            |> Ok
        with error -> Error ("Could not load pack list: " + error.Message)

    /// Load only clues from the generator's v1 format. Stored solutions and
    /// verification claims are deliberately not part of the playable model.
    let read (json: string) : Result<PlayablePack, string> =
        try
            if json.Length > 5 * 1024 * 1024 then failwith "Puzzle packs must be smaller than 5 MB."
            use document = JsonDocument.Parse(json, JsonDocumentOptions(MaxDepth = 32))
            let root = document.RootElement
            if integer "formatVersion" root <> 1 then failwith "This pack version is not supported."
            let rules = property "rules" root
            if not (flag "orthogonalOnly" rules && flag "noSharedCells" rules && flag "noSelfTouching" rules)
               then
                failwith "This player requires orthogonal paths, no shared cells, and no self-touching."
            let requireFullCoverage = flag "requireFullCoverage" rules
            let entries = property "puzzles" root
            if entries.GetArrayLength() < 1 || entries.GetArrayLength() > 10000 then
                failwith "A pack must contain between 1 and 10,000 puzzles."

            let puzzles =
                entries.EnumerateArray()
                |> Seq.map (fun entry ->
                    let width, height = integer "width" entry, integer "height" entry
                    if width < 1 || width > 64 || height < 1 || height > 64 then
                        failwith "Board dimensions must each be between 1 and 64."
                    let pairElements = property "pairs" entry
                    if pairElements.GetArrayLength() > width * height / 2 then failwith "Too many endpoint pairs."
                    let pairs =
                        pairElements.EnumerateArray()
                        |> Seq.map (fun p ->
                            { Label = text "label" p
                              Start = cell (property "start" p)
                              Finish = cell (property "finish" p) })
                        |> List.ofSeq
                    let board = { Width = width; Height = height; RequireFullCoverage = requireFullCoverage; Pairs = pairs }
                    match Validation.puzzle board with
                    | [] -> { Id = text "id" entry; Puzzle = board }
                    | errors -> failwith (String.concat " " errors))
                |> Array.ofSeq
            let conflictingIds =
                puzzles |> Array.groupBy _.Id
                |> Array.exists (fun (_, entries) -> entries |> Array.distinctBy _.Puzzle |> Array.length > 1)
            if conflictingIds then failwith "The same puzzle ID cannot describe different boards."
            Ok { Puzzles = puzzles }
        with error -> Error ("Could not load puzzle pack: " + error.Message)

    let readProgress board (json: string) : Result<SolutionPath list, string> =
        try
            if json.Length > 1024 * 1024 then failwith "Saved progress is too large."
            use document = JsonDocument.Parse(json, JsonDocumentOptions(MaxDepth = 16))
            let paths =
                document.RootElement.EnumerateArray()
                |> Seq.map (fun p ->
                    { Label = text "label" p
                      Cells = (property "cells" p).EnumerateArray() |> Seq.map cell |> List.ofSeq })
                |> List.ofSeq
            match Validation.partialSolution board paths with
            | [] -> Ok paths
            | errors -> Error (String.concat " " errors)
        with error -> Error error.Message

    let writeProgress (paths: SolutionPath list) =
        use stream = new MemoryStream()
        use writer = new Utf8JsonWriter(stream)
        writer.WriteStartArray()
        for path in paths do
            writer.WriteStartObject()
            writer.WriteString("label", path.Label)
            writer.WriteStartArray("cells")
            for cell in path.Cells do
                writer.WriteStartObject()
                writer.WriteNumber("x", cell.X)
                writer.WriteNumber("y", cell.Y)
                writer.WriteEndObject()
            writer.WriteEndArray()
            writer.WriteEndObject()
        writer.WriteEndArray()
        writer.Flush()
        Encoding.UTF8.GetString(stream.ToArray())
