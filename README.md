# Tangles

A .NET 11 / F# console generator for connect-the-pairs puzzles. It constructs
non-overlapping paths, hides their interiors, and retains an independently
validated solution. Unused cells are allowed.

## Build and run

Requires .NET SDK 11.0.100 or a later patch in the same feature band,
as selected by `global.json`. From `C:\Source\Tangles`:

```powershell
dotnet build Tangles.slnx
dotnet run --project src/Tangles.Generator -- --help
dotnet run --project src/Tangles.Generator -- --seed 42 --show-solution
dotnet run --project src/Tangles.Generator -- --width 7 --height 5 --pairs 4 --seed 42
dotnet run --project src/Tangles.Generator -- --count 20 --seed 42 --output puzzles/batch.json
dotnet test Tangles.slnx
```

With no options, generate an 8x8 puzzle with six pairs and a random seed.
Use `--width` and `--height` independently for rectangular boards (1..64 each).
The pair count must fit the board and minimum path length. Small boards will
usually need fewer pairs and a shorter minimum length.

`--min-length` (default 4) and `--max-length` (default 16) count cells including
endpoints. The maximum is a limit, not a required length; it is capped by the
space available while reserving the minimum for remaining paths. A failed
attempt to grow a longer path may fall back to the minimum length.

Existing output files are preserved unless `--force` is supplied. The parent
directory is created if necessary. A batch is written only after every puzzle
has been generated and validated. Ctrl+C cancels generation.

Exit codes: 0 success/help, 1 generation or file failure, 2 invalid arguments
or an existing output file, 130 cancellation.

## Rules

- Each logical label has two distinct endpoints.
- Connect matching endpoints with a continuous horizontal/vertical path.
- Paths cannot cross, share cells, branch, or revisit cells.
- Orthogonally adjacent cells of the same path must be consecutive along it:
  no self-touching. Diagonal contact is allowed.
- Different paths may run alongside one another.
- Unused cells are allowed. Full coverage is also valid; it is not required.
- Any solution satisfying the rules is accepted by the shared validator,
  including reversed paths and alternatives to the stored solution.
- Uniqueness is not required.

## Generation and limits

The original `random-path-growth-v1` algorithm grows paths through free cells
using randomized, bounded backtracking. A new cell may touch only its immediate
predecessor on its own path. Paths are placed sequentially; if a candidate
cannot place the requested number, it restarts the board. Each path search has
a 2,048-node budget; `--max-attempts` (default 500) limits board attempts.
Exhaustion is reported as failure to find a candidate, never as proof that
the configuration is impossible. Dense or unusually long requests can fail.

The shared validator separately checks the returned witness before it is
displayed or saved. This initial version does **not** assess difficulty,
establish uniqueness, deduplicate puzzles, or guarantee enjoyable play.
Constructed path length is not a difficulty rating. Solver-assisted selection
and playtesting remain future work.

An explicit seed reproduces generation with the same options, algorithm and
.NET runtime implementation. A batch uses the supplied seed for the first
puzzle and deterministically derived seeds thereafter. Each puzzle stores its
own seed so it can also be regenerated individually with the same settings.

## JSON pack format (version 1)

The pack contains `formatVersion`, explicit `rules`, `generation` settings,
and a `puzzles` array. Every puzzle includes:

- `id`: SHA-256-based identifier of its labelled, oriented endpoint layout;
  rotations, reflections and label permutations are not normalised.
- `width`, `height`, `seed`, and candidate `attempts`.
- `pairs`: labels with `start` and `finish` coordinates.
- `solution`: labels with ordered `cells` arrays, including endpoints.
- `verification`: `solutionValid: true`, `uniqueness: "notChecked"`,
  `difficulty: "notAssessed"`.

Coordinates are zero-based `{ "x": 0, "y": 0 }`, with the origin at the top
left. Puzzle clues are the dimensions and endpoint pairs; solution cells are
separate and should not be exposed during play. No timestamp is included, so
the same batch settings produce identical JSON on the same runtime.

## Layout

- `Tangles.slnx`: solution.
- `src/Tangles.Core`: shared puzzle types and rules validator, without UI or IO.
- `src/Tangles.Generator`: generation, command line, console display and JSON export.
- `tests/Tangles.Tests`: rule regressions, independent multi-seed witness checks,
  reproducibility, input validation, cancellation and export checks.
- `global.json`: .NET SDK selection.
