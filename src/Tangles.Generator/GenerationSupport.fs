namespace Tangles.Generator

open System

type CutStyle = RandomCuts | VariedCuts

module CutStyle =
    let name = function RandomCuts -> "random" | VariedCuts -> "varied"

/// Explicit counters; Attempts remains in JSON for compatibility with older packs.
type GenerationWork =
    { BoardAttempts: int
      TraversalEvaluations: int
      RefinementEvaluations: int
      StructuredStarts: int }

module GenerationWork =
    let zero = { BoardAttempts = 0; TraversalEvaluations = 0; RefinementEvaluations = 0; StructuredStarts = 0 }
    let add a b =
        { BoardAttempts = a.BoardAttempts + b.BoardAttempts
          TraversalEvaluations = a.TraversalEvaluations + b.TraversalEvaluations
          RefinementEvaluations = a.RefinementEvaluations + b.RefinementEvaluations
          StructuredStarts = a.StructuredStarts + b.StructuredStarts }

type internal FailureKind = InvalidOptions | SearchExhausted | InternalFailure

type internal GenerationFailure =
    { Kind: FailureKind; Message: string; Work: GenerationWork }

module internal GenerationFailure =
    let create kind message work = { Kind = kind; Message = message; Work = work }

module internal GenerationSupport =
    let shufflePrefix (random: Random) (items: 'a array) length =
        for i in length - 1 .. -1 .. 1 do
            let j = random.Next(i + 1)
            let value = items[i]
            items[i] <- items[j]
            items[j] <- value

    let shuffle random items = shufflePrefix random items items.Length

    let label index = if index < 26 then string (char (int 'A' + index)) else string (index + 1)

    let neighbours width height =
        Array.init (width * height) (fun cell ->
            [| if cell % width > 0 then cell - 1
               if cell % width + 1 < width then cell + 1
               if cell >= width then cell - width
               if cell + width < width * height then cell + width |])

/// Counter-based batch seeds have a separate domain from Random(seed) retries.
/// Single requests and their retry streams keep their existing positive-seed mapping.
module internal SeedStreams =
    let batchAlgorithm = "splitmix64-batch-v1"

    let batchSeed (seed: int) request =
        if request = 0 then seed
        else
            let mutable value = uint64 seed + 0x54414E474C455342UL + uint64 request * 0x9E3779B97F4A7C15UL
            value <- (value ^^^ (value >>> 30)) * 0xBF58476D1CE4E5B9UL
            value <- (value ^^^ (value >>> 27)) * 0x94D049BB133111EBUL
            value <- value ^^^ (value >>> 31)
            int (value &&& 0x7FFFFFFFUL)
