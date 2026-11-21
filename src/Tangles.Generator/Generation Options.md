# Tangles puzzle-pack generation

Command-line options and the recipes used for the latest six 12×12 packs.
Checked against the generator source and pack metadata at commit `1b6f9b2`,
dated 20 November 2026. Requires .NET SDK 11.0.100 or a compatible patch
selected by the repository's `global.json`.

**The latest collections used `tools/regenerate-collections.fsx`, which calls
the generator and solver directly and applies additional filters.** They were
not produced simply by running `--difficulty easy`, `medium` and `hard`.

## Running the ordinary generator

From PowerShell:

```powershell
Set-Location C:\Source\Tangles
dotnet run --project src/Tangles.Generator -c Release -- --help
dotnet run --project src/Tangles.Generator -c Release -- --count 20 --seed 42 --output puzzles/my-pack.json
```

The `--` separates `dotnet run` options from the generator's options. With no
generator options, it creates one 8×8 connections-only puzzle with six pairs.

## Board, generation and output options

| Option | Default | Meaning and accepted values |
|---|---|---|
| `--mode MODE` | `connect-only` | `connect-only` allows unused squares; `full-coverage` requires every square in the completed answer. |
| `--width N` | `8` | Board width, 1–64. |
| `--height N` | `8` | Board height, 1–64. Width and height can differ. |
| `--pairs N` | `6` | Number of colours/labels, each with two endpoints. Must be positive and fit the board. Eight pairs means 16 endpoints and eight paths. |
| `--min-length N` | `4` | Minimum cells in a generated path, including both endpoints; at least 2. |
| `--max-length N` | `16` | Maximum cells in a generated path; at least the minimum. |
| `--seed N` | Random | Starting seed, 0–2,147,483,647. Negative seeds are rejected. |
| `--count N` | `1` | Puzzles in the pack, 1–10,000. Defaults to 12 when `--ramp` is supplied. |
| `--max-attempts N` | `500` | 1–10,000. Board attempts for connections-only; traversal evaluations per fresh layout search for full coverage. |
| `--cut-style STYLE` | `random` | `random` or experimental `varied` cuts. `varied` requires full coverage. |
| `--output PATH` | None | Write a JSON puzzle pack. Without this, results are printed to the console. |
| `--show-solution` | Off | Also print the stored solution in the console. Solutions are included in JSON regardless. |
| `--force` | Off | Permit replacing an existing output file. Requires `--output`. |
| `--help`, `-h` | — | Display the built-in help. |

Both modes enforce orthogonal moves, no shared cells and no self-touching.
Path-length bounds constrain the generator's stored solution; they are not
length clues or additional restrictions imposed on the player.

The minimum lengths must fit within the board. For full coverage,
`pairs × min-length ≤ width × height ≤ pairs × max-length` must hold. One path
cannot cover a two-dimensional rectangle under the no-self-touching rule.

## Difficulty and selection options

| Option | Default | Meaning and accepted values |
|---|---|---|
| `--difficulty D` | `any` | Exact solver target: `any`, `easy`, `medium`, `hard` or `expert`. Explicit targets require full coverage. |
| `--ramp RANGE` | None | An ascending range such as `easy..hard` or `medium..expert`. Composes sections of the original solver grades and chooses recipes for each section. |
| `--candidates N` | `100` | Selection proposals per generation request, 1–10,000. |
| `--improve-steps N` | `100` | Additional proposals after first meeting the target, 0–10,000, within the candidate limit. |
| `--solver-work N` | `2000000` | Deterministic work-unit budget per assessment, 1–100,000,000. This is not milliseconds. |
| `--min-trials N` | `0` | Minimum successful contradiction eliminations, 0–1,000,000. Positive values require `hard` or `expert`. |
| `--max-trials N` | Unset | Optional maximum contradiction eliminations, at least `--min-trials` and at most 1,000,000. Requires an explicit target or ramp. There is no default ceiling. |
| `--min-depth N` | `0` | Minimum branch depth before the first answer, 0–256. Positive values require `expert`. |
| `--min-search-nodes N` | `0` | Minimum search nodes before the first answer, 0–20,000. Positive values require `expert`. |
| `--assess` | Off | Record clue-only solver reasoning and solution-count evidence without demanding a grade. |
| `--require-unique` | Off | Accept only puzzles with a proved unique solution. Uniqueness is not a game rule. |
| `--avoid-shortest` | Off | Connections-only: reject puzzles solved by the built-in **32-attempt** greedy shortest-route screen. |

The original solver grades mean:

| Solver grade | Required reasoning |
|---|---|
| `easy` | Local deductions. |
| `medium` | Additional route/connectivity deductions. |
| `hard` | Contradiction trials, without branching. |
| `expert` | Branching search. |

These reasoning grades require a completed full-coverage assessment and a
uniqueness proof. Connections-only retains its provisional geometric assessment;
`--difficulty hard` does not select connections-only puzzles.

Selection requiring a grade or uniqueness supports boards up to 16×16, with
at most 64 full-coverage or 63 connections-only pairs. `--assess` can report an
unsupported assessment outside the solver's supported sizes.

A ramp requires full coverage and cannot be combined with `--pairs` or
`--difficulty`. Its count must allow at least one puzzle per requested grade.
It chooses pair counts and can increase maximum lengths, attempt limits and
solver budgets for larger boards unless those values are explicitly set.
Its grades are the original solver grades, not the latest collection tiers.

## Attempts, batches and repeatability

- `--max-attempts` bounds a layout search; `--candidates` bounds selection
  proposals; `--solver-work` bounds each assessment. Raising one does not remove
  the other limits.
- Ordinary selected CLI batches allow at most `2 × count × candidates`
  proposals in total. Fast, unselected batches allow at most `10 × count`
  generation requests. The latest collection script has its own limits below.
- Rotated and reflected duplicate boards are rejected within a CLI batch.
- A complete pack is written only after all requested puzzles are available
  and validated. A failed search is not proof that the settings are impossible.
- Ctrl+C cancels generation. Existing output is preserved unless `--force` is
  supplied. Exit codes are 0 for success/help, 1 for generation/file failure,
  2 for invalid arguments or an existing output file, and 130 for cancellation.
- A batch derives request seeds using `splitmix64-batch-v1`; its first request
  retains the supplied seed. Per-puzzle seeds and selection settings are stored
  in JSON. Replay requires the same code, recipe and selection settings, not
  just the pack's starting seed.

## Settings used for the latest six packs

All six contain **100 new 12×12 puzzles**, use `random` cuts, and retain valid
stored solutions for hints. These tables give the values used by the script,
using corresponding ordinary CLI option names where applicable.

| Mode | Displayed tier | `--pairs` | `--min-length` | `--max-length` | `--max-attempts` | Starting `--seed` |
|---|---|---:|---:|---:|---:|---:|
| `connect-only` | Easy | 8 | 6 | 24 | 1,000 | `202611301` |
| `connect-only` | Medium | 10 | 8 | 24 | 1,000 | `202611302` |
| `connect-only` | Hard | 10 | 8 | 24 | 1,000 | `202611303` |
| `full-coverage` | Easy | 12 | 4 | 48 | 5,000 | `202611311` |
| `full-coverage` | Medium | 8 | 3 | 48 | 10,000 | `202611312` |
| `full-coverage` | Hard | 8 | 3 | 48 | 10,000 | `202611313` |

### Full-coverage selection settings

| Displayed tier | `--difficulty` equivalent | `--candidates` | `--solver-work` | `--min-trials` | `--min-depth` | `--min-search-nodes` | Extra script filter |
|---|---|---:|---:|---:|---:|---:|---|
| Easy | `hard` | 60 | 20,000,000 | 1 | 0 | 0 | None. |
| Medium | `expert` | 40 | 100,000,000 | 0 | 2 | 8 | First-answer search nodes must also be **at most 255**. |
| Hard | `expert` | 40 | 100,000,000 | 0 | 8 | 512 | None. |

For all three, **`--improve-steps` was 0** and **`--max-trials` was unset**.
`--ramp`, `--assess`, `--require-unique` and `--avoid-shortest` were not enabled.
The explicit target already requests assessment. All 300 full-coverage puzzles
nevertheless have completed uniqueness proofs because these are required for
the solver grades used in selection.

The Medium ceiling of 255 nodes is a script filter: there is no ordinary
`--max-search-nodes` option. Search-node and depth measurements concern finding
the **first** answer, excluding subsequent work proving uniqueness.

### Connections-only selection settings

These use `Generation.generate` directly, then run a larger shortest-route
screen. They do not use explicit `--difficulty`, solver grading, or the ordinary
`--avoid-shortest` flag.

| Displayed tier | Attempts tested by the script | Acceptance rule |
|---|---:|---|
| Easy | 256 | No success in attempts 1–32; a valid solution found in attempts 33–256. |
| Medium | 2,048 | No success in attempts 1–256; a valid solution found in attempts 257–2,048. |
| Hard | 8,192 | No success in any of the 8,192 attempts. |

These larger limits are not ordinary CLI options. In particular,
`--avoid-shortest` alone tests 32 attempts and cannot reproduce these sets.
The script retains a separately validated solution; failure of its greedy
screen does not mean a puzzle is unsolvable, uniquely solvable or proved hard
for a human.

### Collection tiers and JSON evidence

The displayed Easy/Medium/Hard tiers are the provisional `player-feedback-v2`
collection scale. Raw assessments were preserved: full-coverage Easy has the
solver's `hard` grade; full-coverage Medium and Hard have `expert`. The three
connections-only collections contain raw geometric grades of `medium` or `hard`.

`generation.collectionSelection` records the displayed tier and extra filters.
Each puzzle records its raw `difficulty`, solver evidence where applicable,
and its connections-only `shortestRoutes` result. Collection ordering puts a
representative puzzle first and shuffles the remainder with a recorded seed.
Selection also excluded existing puzzles in the output folder and their
rotations/reflections; all six sets were checked together for repeats.

See [the calibration notes](../../docs/collection-calibration.md) and
[the measured results](../../docs/collection-calibration-results.json). All
600 solutions and their hints were checked through the shared player rules.

## Commands for the latest collection recipes

The script accepts exactly five positional arguments:

```text
mode tier count output-directory work-directory
```

Its modes are `connect-only` and `full-coverage`; tiers are `easy`, `medium`
and `hard`; count is 1–1,000. Dimensions, seeds, path bounds and selection
requirements are fixed by the script's recipes. Generator flags such as
`--width` and `--difficulty` cannot be appended to this script command.

The latest run used Release assemblies under
`C:\Source\codex-workspace\.scratch\tangles-generator-fixes\artifacts`, output
to `C:\Source\Tangles\puzzles`, and checkpointed in the scratch
`recalibration` folder. To run the same recipes again, use a **fresh work folder**:

```powershell
Set-Location C:\Source\Tangles
$artifacts = 'C:\Source\codex-workspace\.scratch\tangles-generator-fixes\artifacts'
dotnet build src/Tangles.Generator/Tangles.Generator.fsproj -c Release --artifacts-path $artifacts

$coreDll = Join-Path $artifacts 'bin\Tangles.Core\release\Tangles.Core.dll'
$generatorDll = Join-Path $artifacts 'bin\Tangles.Generator\release\Tangles.Generator.dll'
$output = 'C:\Source\Tangles\puzzles'
$work = 'C:\Source\codex-workspace\.scratch\tangles-generator-fixes\next-collections'

function New-TanglesCollection {
    param([string]$Mode, [string]$Tier)
    dotnet fsi "--reference:$coreDll" "--reference:$generatorDll" tools/regenerate-collections.fsx $Mode $Tier 100 $output $work
    if ($LASTEXITCODE -ne 0) { throw "Collection generation failed: $Mode $Tier" }
}

New-TanglesCollection connect-only easy
New-TanglesCollection connect-only medium
New-TanglesCollection connect-only hard
New-TanglesCollection full-coverage easy
New-TanglesCollection full-coverage medium
New-TanglesCollection full-coverage hard
```

These commands reproduce the recipes, not necessarily identical collections:
the current output folder already contains the latest puzzles, which the script
will exclude. The previous comparison corpus is identified by commit `a1d05d6`
in the results document. Timing metadata also varies between runs.

The script saves accepted puzzles as checkpoints and can resume an incomplete
run using its original work folder. Each invocation has a 60-minute limit and
allows up to 100,000 requests for connections-only or 2,000 for full coverage.
These are script limits, not the ordinary CLI batch budget. Full-coverage
requests run in parallel, with results consumed in seed order.

## Output filenames and publication

The script initially creates:

```text
connect-only-12x12-easy-100-20261120-v2.json
connect-only-12x12-medium-100-20261120-v2.json
connect-only-12x12-hard-100-20261120-v2.json
full-coverage-12x12-easy-100-20261120-v2.json
full-coverage-12x12-medium-100-20261120-v2.json
full-coverage-12x12-hard-100-20261120-v2.json
```

It refuses to replace an existing output of the same name. For the latest
release, the six complete packs were validated and installed in `puzzles/`
at the same filenames **without `-v2`**, replacing the earlier editions. The
website catalogue requests those files with `?edition=2`; old unversioned
bookmarks resolve to the listed edition. Earlier pack contents remain in Git.

Creating a pack does not publish it automatically. The web project links the
published filenames into its static assets; committing and pushing changes
to `main` triggers the GitHub Pages build and deployment.
