module Tangles.Generator.Program

open System
open System.IO
open System.Threading
open Tangles.Generator

let private run (cancellation: CancellationToken) options =
    match options.OutputPath with
    | Some path when File.Exists path && not options.Force ->
        eprintfn "Output file already exists. Use --force to replace it: %s" path
        2
    | _ ->
        let seeds = Random options.Generation.Seed
        let generated = ResizeArray<GeneratedPuzzle>()
        let mutable error = None

        while generated.Count < options.Count && error.IsNone do
            cancellation.ThrowIfCancellationRequested()
            let seed = if generated.Count = 0 then options.Generation.Seed else seeds.Next()

            match Generation.generate cancellation { options.Generation with Seed = seed } with
            | Ok puzzle -> generated.Add puzzle
            | Error message -> error <- Some $"Puzzle {generated.Count + 1}: {message}"

        match error with
        | Some message ->
            eprintfn "%s" message
            1
        | None ->
            cancellation.ThrowIfCancellationRequested()

            match options.OutputPath with
            | Some path ->
                let pack = Export.createPackWithCancellation cancellation options.Generation (generated.ToArray())
                cancellation.ThrowIfCancellationRequested()
                Export.writeFile path options.Force pack
                printfn "Saved %d puzzle(s) to %s" generated.Count (Path.GetFullPath path)
            | None -> ()

            for puzzle in generated do Export.printPuzzleWithCancellation cancellation options.ShowSolution puzzle
            printfn "\nKnown solutions validated; uniqueness unchecked; difficulty is a provisional heuristic estimate."
            0

[<EntryPoint>]
let main args =
    match CommandLine.parse (Random.Shared.Next()) args with
    | Error message ->
        eprintfn "%s" message
        2
    | Ok Help ->
        printf "%s" CommandLine.help
        0
    | Ok (Generate options) ->
        use cancellation = new CancellationTokenSource()
        let handler = ConsoleCancelEventHandler(fun _ event -> event.Cancel <- true; cancellation.Cancel())
        Console.CancelKeyPress.AddHandler handler

        try
            try run cancellation.Token options
            with
            | :? OperationCanceledException ->
                eprintfn "Generation cancelled."
                130
            | :? IOException as error ->
                eprintfn "File operation failed: %s" error.Message
                1
            | :? UnauthorizedAccessException as error ->
                eprintfn "File access denied: %s" error.Message
                1
            | :? ArgumentException as error ->
                eprintfn "Invalid argument: %s" error.Message
                2
        finally
            Console.CancelKeyPress.RemoveHandler handler
