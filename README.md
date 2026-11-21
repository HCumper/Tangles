# Tangles

A .NET 11 / F# puzzle generator and Bolero WebAssembly player. The generator constructs
non-overlapping paths, hides their interiors, and retains an independently
validated solution. Choose connect-only puzzles, which allow unused cells,
or full-coverage puzzles, which require every square to be filled.

## Build and run

Requires .NET SDK 11.0.100 or a later patch in the same feature band,
as selected by `global.json`. From `C:\Source\Tangles`:

```powershell
dotnet build Tangles.slnx
dotnet run --project src/Tangles.Generator -- --help
dotnet run --project src/Tangles.Generator -- --seed 42 --show-solution
dotnet run --project src/Tangles.Generator -- --width 7 --height 5 --pairs 4 --seed 42
dotnet run --project src/Tangles.Generator -- --count 20 --seed 42 --output puzzles/batch.json
dotnet run --project src/Tangles.Generator -- --mode full-coverage --seed 42 --show-solution
dotnet run --project src/Tangles.Generator -- --mode full-coverage --count 20 --seed 42 --output puzzles/full.json

dotnet run --project src/Tangles.Generator -- --mode full-coverage --difficulty hard --count 20 --seed 42 --output puzzles/hard.json
dotnet test Tangles.slnx
```

With no options, generate an 8x8 puzzle with six pairs and a random seed.

The [generator options and latest six-pack recipes](src/Tangles.Generator/Generation%20Options.md)
document lists every CLI option, defaults and restrictions, plus the exact seeds,
selection settings and script commands used for the current collections.

Pack-quality options include `--max-trials N` (optional), experimental
`--cut-style varied`, and `--ramp easy..hard`. Batches reject rotated and
reflected duplicates. See [pack quality and measured results](docs/pack-quality.md)
for recipes, comparison data and the sample ramp packs.
`--mode connect-only` is the default and keeps the original generation method.
Use `--mode full-coverage` to require every square in a final solution.
Use `--width` and `--height` independently for rectangular boards (1..64 each).
The pair count must fit the board and minimum path length. Small boards will
usually need fewer pairs and a shorter minimum length.

`--min-length` (default 4) and `--max-length` (default 16) count cells including
endpoints. The maximum is a limit, not a required length. In connect-only mode
it is capped by the space available while reserving the minimum for remaining
paths; a failed attempt to grow a longer path may fall back to the minimum.
For full coverage, the board size must lie between `pairs * min-length` and
`pairs * max-length`. Every returned path still obeys both length bounds.

Existing output files are preserved unless `--force` is supplied. The parent
directory is created if necessary. A batch is written only after every puzzle
has been generated and validated. Ctrl+C cancels generation.

`--difficulty easy|medium|hard|expert` enables solver-guided full-coverage
selection. Hard puzzles require contradiction trials in the grader and are
solved without branching; expert puzzles require branching in this strategy.
The default `--difficulty any` retains the fast generation mode. `--assess`
adds solver evidence without requiring a particular tier. Use `--require-unique`
to enforce a proved unique answer, or `--avoid-shortest` to screen connect-only
puzzles against 32 greedy shortest-route trials. See
[solver-guided generation](docs/solver-guided-generation.md) for commands,
budgets, measurements and limitations.
Use `--min-trials` to remove weak hard/expert candidates, or expert-only
`--min-depth` and `--min-search-nodes` to require deeper search to a first answer.
The latter measurements exclude subsequent work proving uniqueness.

Exit codes: 0 success/help, 1 generation or file failure, 2 invalid arguments
or an existing output file, 130 cancellation.

## Rules

- Each logical label has two distinct endpoints.
- Connect matching endpoints with a continuous horizontal/vertical path.
- Paths cannot cross, share cells, branch, or revisit cells.
- Orthogonally adjacent cells of the same path must be consecutive along it:
  no self-touching. Diagonal contact is allowed.
- Different paths may run alongside one another.
- Connect-only mode allows unused cells; full coverage is also a valid answer.
- Full-coverage mode requires every square to be occupied in the final answer.
  Partial routes may leave gaps while the puzzle is being played.
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

Full-coverage mode uses `hamiltonian-path-partition-v3`. It starts with a traversal
visiting every square once, then changes it with endpoint backbite moves (joining
an endpoint to another neighbouring square and reversing the affected section).
Dynamic programming splits that traversal into exactly the requested number of
paths within the length limits. A segment is allowed only if its nonconsecutive
cells never touch orthogonally, preserving the no-self-touching rule. The segments
therefore cover the board without sharing cells.

Before testing a split, an unguided backbite walk mixes the traversal: 48 moves
per board cell, with a minimum of 256 and a maximum of 16,384 (3,072 on 8x8).
The search favours traversals needing fewer segments. After 128 unsuccessful
candidates it creates a fresh randomly oriented, reflected and mixed traversal.
Every eighth search proposal uses several backbites to help leave local plateaux.
Once a witness is found, it is retained while up to `max(64, board area)` further
candidates are explored. One-dimensional boards skip this ineffective refinement.
With attempt limits of at least 128, up to the final
128 candidates are reserved for a fresh structured start if mixing has not
found any witness; this preserves support for tightly constrained lengths.

`--max-attempts` bounds all candidate split evaluations, including refinement
and fallback. The bounded mixing moves are additional work and check cancellation
throughout; one-dimensional boards skip mixing because backbites cannot change
their traversal. Valid settings can still exhaust that search; another seed,
a larger attempt limit or different pair/length settings may help. One path cannot fill a two-dimensional
rectangle under the no-self-touching rule, and is rejected in this mode.
See [the measured v1/v2 comparison](docs/full-coverage-comparison.md) for the
improvements in variety, generation costs and remaining limitations.

The shared validator separately checks the returned witness before it is
displayed or saved. Fast generation also receives an initial
**provisional heuristic difficulty grade** (`easy`, `medium` or `hard`) and a
0..100 score. The two coverage modes use separate profiles. Grades depend on
the clues, not on the shape of the stored solution, and are not calibrated
against human play. Solver-assessed full-coverage puzzles use a reasoning grade
and retain the geometry estimate as diagnostic data. Selected packs exclude
duplicate clue layouts; full-coverage selections are ordered by solver difficulty.
Their bounded candidate pool skips failed searches. Plain fast generation retains its original
batch behaviour. Unique solutions are recorded when established and can be
required explicitly; uniqueness is never assumed during solving. Playtesting is
still needed. See [the geometry model](docs/difficulty-grading.md) and
[the reasoning model](docs/solver-guided-generation.md).

Seeds must be in `0..2147483647`. An explicit seed reproduces generation with the same options, algorithm and
.NET runtime implementation. A batch starts generation with the supplied seed
and uses deterministically derived seeds for further requests. Each puzzle stores
its own seed so it can also be regenerated individually with the same settings.

## JSON pack format (version 1)

The pack contains `formatVersion`, explicit `rules`, `generation` settings,
and a `puzzles` array. Every puzzle includes:

- `id`: SHA-256-based identifier of its labelled, oriented endpoint layout;
  full-coverage IDs also include the coverage rule. Existing connect-only IDs
  are preserved. Rotations, reflections and label permutations are not normalised.
- `width`, `height`, `seed`, and candidate `attempts`.
- `pairs`: labels with `start` and `finish` coordinates.
- `solution`: labels with ordered `cells` arrays, including endpoints.
- `difficulty`: provisional grade, score, versioned mode-specific profile,
  thresholds, aggregate and per-pair measurements, and weighted score components.
  Assessed puzzles also include `reasoning`; a completed reasoning grade retains
  the original score/components in `geometricEstimate`.
- `selection`: optional per-puzzle candidate/search statistics; `shortestRoutes`
  records an optional connect-only screen.
- `verification`: `solutionValid: true`, `uniqueness` (`notChecked`, `unique`,
  `multiple` or `notEstablished`), and `difficulty` (`heuristicEstimate` or
  `reasoningEstimate`). Grades remain provisional estimates of human difficulty.

Coordinates are zero-based `{ "x": 0, "y": 0 }`, with the origin at the top
left. Puzzle clues are the dimensions and endpoint pairs; solution cells are
separate and should not be exposed during play. No timestamp is included, so
the same batch settings produce identical JSON on the same runtime.

`rules.requireFullCoverage` selects the completion rule for the whole pack.
`generation.mode` is `connect-only` or `full-coverage`, and `generation.algorithm`
records the corresponding algorithm. Both modes use format version 1: the
coverage flag was already part of that format. Each pack uses one rule set.
`generation.selection` records requested policies, work limits and observed
batch retries. Solver-guided full coverage records
`solver-guided-hamiltonian-partition-v5` as its generation algorithm, or `v6`
when explicit reasoning minimums are enabled.
`generation.batchSeedAlgorithm` records the independent batch seed derivation;
per-puzzle `work` separates board attempts, traversal evaluations and refinement.
See [generator reliability and replay](docs/generator-reliability.md) for the
seed policy, structured proposals, diagnostics and compatibility details.
The difficulty object is also an additive format-v1 field; old packs without
it remain valid. Existing players ignore this extra metadata. Difficulty does
not change puzzle IDs or the generated clues/solutions. Previously generated
packs are not modified automatically; new runs include assessments.

## Layout

- `Tangles.slnx`: solution.
- `src/Tangles.Core`: shared puzzle types, complete/partial rules validation, pack reading and player state.
- `src/Tangles.Generator`: generation, clue-only reasoning/solution counting,
  solver-guided selection, geometric grading, command line and JSON export.
- `src/Tangles.Player`: standalone Bolero WebAssembly player and static assets.
- `tests/Tangles.Tests`: rule regressions, independent multi-seed witness checks,
  reproducibility, input validation, cancellation and export checks.
- `global.json`: .NET SDK selection.

## Bolero web player

The player targets .NET 11 and uses Bolero 0.25.65 with the .NET 11 Blazor
WebAssembly runtime. It runs entirely in the browser, without an application
backend, server-side rendering, or a database. The generator stays a separate
console application in the same solution.

Start the development preview from the repository root:

```powershell
dotnet run --project src/Tangles.Player -- --urls http://localhost:5094
```

Open the displayed URL and use **Choose a pack**. The bundled choices are the
eight-puzzle 8x8 starter collection, an eight-puzzle 8x8 full-coverage collection,
and a small 3x3 quick-start pack. Each option describes its dimensions, coverage
rule and puzzle count. Descriptions are derived from the validated pack data;
packs with several board sizes list each distinct size.

### Playing

- Drag with a mouse or finger from either lettered endpoint to its match.
- Retrace to shorten a path, or drag from an existing path to trim and extend it.
- Starting at an endpoint replaces that pair's current path.
- Other routes and endpoints cannot be crossed or overwritten. Self-touching
  and diagonal steps are rejected by the shared F# validator.
- Undo restores one drawing stroke. Reset clears the board and is undoable.
- Hint fills a random unfinished colour using the pack's stored solution. It
  clears any drawn paths that cross that route; one Undo restores the whole
  change. Hints are unavailable when the puzzle is complete or its pack has no
  valid stored solution. If all pairs connect but full coverage has gaps, Hint
  replaces a route that differs from the stored solution.
- Keyboard: Tab to the board; arrows move the cursor; Space/Enter starts or
  stops drawing. Escape stops drawing; Ctrl+Z/Cmd+Z undoes.
- Any valid connection of every pair completes a connect-only puzzle.
- Full-coverage packs also require every square to be filled. The player displays
  the coverage rule and a filled-square count, and keeps the puzzle incomplete
  if all pairs are connected but gaps remain.
- The pack description says **Full coverage required** or **Connections only**, using the JSON
  `rules.requireFullCoverage` flag. The introduction, instructions and footer
  explain the selected rule, including when another pack is imported.
- Completion accepts any valid solution, independently of the stored hint paths.

Valid partial progress is saved in this browser's local storage per puzzle.
Undo history is not persisted. Storage can be unavailable or cleared by the
browser; progress does not synchronise between devices. Custom packs must be
loaded again after a reload unless supplied as a static pack URL.

### Loading generator packs

The hosted pack chooser includes six 12×12 collections with 100 puzzles per
tier and coverage mode. These were regenerated with stronger requirements
after player feedback: every connections-only Medium puzzle defeats 256 greedy
shortest-route attempts, and Hard defeats 8,192. Full-coverage Easy now needs
trial deductions, while Medium and Hard need branching. The displayed collection
tiers are provisional and separate from the raw diagnostic grades in the JSON.
See [collection calibration](docs/collection-calibration.md) for the exact
requirements. Earlier 8×8, 12×12 and 16×16 ramp samples are labelled separately.
The player project links these JSON files from `puzzles/` into the published
`packs/` directory, so the website and repository use the same pack contents.

Use **Load puzzle pack** to select the generator's v1 JSON output (up to 5 MB).
Imported packs join the drop-down list for the current visit, so you can switch
back without selecting the file again. Importing the same filename replaces
that imported entry. Switching packs saves the current puzzle's progress and
restores saved progress when another puzzle is selected.
The file is read locally; it is not uploaded. Unsupported versions/rules and
invalid endpoint layouts are rejected without replacing the current puzzle.
Clues are loaded into game state, and valid stored solutions are retained for
hints. Completion accepts any valid route; verification claims are not trusted.
The pack's coverage rule is retained and enforced. Duplicate
identical puzzles are permitted; conflicting boards with
the same ID are rejected. Imported clues are validated, but arbitrary imported
packs without a valid stored witness are not independently checked for solvability.

Alternatively copy a pack to `src/Tangles.Player/wwwroot/packs/` and open
`?pack=packs/your-pack.json`. Pack URLs must be on the same site.
Selecting a hosted pack also updates this URL so it can be bookmarked or reloaded.

To add a hosted pack to the drop-down list, add its name and path to the `packs`
array in `src/Tangles.Player/wwwroot/packs.json`, for example:

```json
{ "name": "My collection", "path": "packs/my-pack.json" }
```

Only the name and path belong in this catalogue; sizes and coverage rules are
read from each puzzle pack. Available packs are fetched once at startup and
cached for switching during the visit. An unavailable entry does not prevent
playing the other packs. Local imports are kept in memory and are not uploaded
or included in a hosted-pack URL.

### Publish as static files

```powershell
dotnet publish src/Tangles.Player -c Release -o artifacts/player
```

Deploy the **contents of `artifacts/player/wwwroot`** to any static HTTPS host.
There is no server project to deploy. Keep the `_framework` folder and puzzle
packs alongside `index.html`; the host must serve `.wasm` as `application/wasm`
and allow the generated framework file extensions. The relative `<base href="./">`
supports deployment in a subdirectory, such as a GitHub Pages project site;
use its trailing-slash URL. A `.nojekyll` file is included for GitHub Pages so `_framework` is served.

To verify the actual published files locally, if Python is installed:

```powershell
python -m http.server 5094 --bind 127.0.0.1 --directory artifacts/player/wwwroot
```

The WebAssembly runtime is included in the initial download. This initial
version does not install an offline service worker or provide logical hints,
difficulty ratings, or generation in the browser. Hints reveal one complete
stored path rather than explaining a deduction. A
`wasm-tools` workload is not required for this interpreted WebAssembly build;
AOT/native optimisation is disabled. The .NET publish command may suggest
installing that optional workload.

## QL player and puzzle-pack loading layer

`QL/Parser/pack.ssb` loads the generator's v1 JSON packs into named SSB records,
with clue and rule validation similar to the F# loader. Applications use
`packPuzzles` and `packPairs`, including nested endpoint coordinates and the
coverage flag; JSON traversal stays inside the loader and its temporary tree is
freed after loading. `pack_example.ssb` demonstrates this interface. See
[the QL parser and loader guide](QL/Parser/README.md) for APIs, QL limits and
interpreted/Turbo emulator checks.

[`QL/GUI`](QL/GUI/README.md) is the native SMSQ/E player: pack selection/import,
variable boards including 16x16, mouse and keyboard drawing with the same game
rules as the F# player, undo/reset, puzzle navigation, completion and validated
local progress. The compiled engine is checked against reference traces from
`Tangles.Core.Play`. The player import validates up to 5 MiB of original JSON
while retaining only clue/rule nodes; the 100-puzzle 12x12 pack is tested unchanged.
Use SMSQ/E GD2 at 800x600 with 16 MiB RAM; see the GUI guide for the real Turbo
build, launcher, controls and QL-specific limits.
