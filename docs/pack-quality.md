# Pack quality and difficulty ramps

The generator now removes rotated and reflected duplicates, supports an optional
upper trial-elimination bound, and can compose packs containing exact graded
sections. None of these changes adds a game rule or changes saved-progress IDs.
The grades remain estimates from the current solver, not calibrated human ratings.

## Symmetry and effort bounds

Selection compares all eight symmetries of a square and the four
dimension-preserving symmetries of a rectangle. Labels, pair order and endpoint
direction do not distinguish duplicates. This key is separate from the solver's
assessment key: rotation can change its scan order and workload. Fast batches
also reject symmetric duplicates, with at most `10 * count` requests. Selected
batches retain their existing bounded proposal budget and atomic output.

There is no default effort ceiling. For example, request a band explicitly:

```powershell
dotnet run --project src/Tangles.Generator -c Release -- --mode full-coverage --difficulty hard --min-trials 5 --max-trials 20 --seed 42
```

`--max-trials` bounds successful contradiction eliminations, matching the
existing `--min-trials` measurement. It does not bound all attempted hypotheses,
search nodes, elapsed time or human effort. `--solver-work` still bounds solver
work. The hill-climber favours moving towards the permitted interval when a
candidate is outside it; it does not keep rewarding counts above the ceiling.
The bound and `policyVersion: "symmetry-effort-v1"` are retained in JSON.

## Optional cut weighting and measured result

`--cut-style random` remains the default. `--cut-style varied` gives modest,
positive weights to DP-feasible cuts using turns, straight segments, border-cell
fraction and departure from the mean length. It preserves all path-length and
solvability checks. These are local preferences, not a global optimisation of
the complete partition or a guarantee of difficulty.

The paired comparison used seeds 0..149, full coverage, 8x8, six pairs, lengths
4..16, at most 500 traversal evaluations and the same solver budgets:

| Measurement | Random | Varied |
|---|---:|---:|
| Puzzles generated | 150 | 150 |
| Witness turns per path | 1.949 | 1.962 |
| Straight witness paths | 14.22% | 12.44% |
| Border endpoints | 52.44% | 52.22% |
| Mean absolute path-length deviation | 3.529 | 3.483 |
| Easy / medium / hard / expert | 107 / 0 / 40 / 0 | 107 / 0 / 40 / 0 |
| Unrated | 3 | 3 |
| Proved unique / multiple / unknown | 147 / 2 / 1 | 147 / 2 / 1 |

This is a small reduction in straight paths, with no improvement in the grade
distribution and no increase in length variation in this sample. Varied cuts
remain experimental and opt-in. Witness shape is distinct from the difficulty
of finding any accepted solution. Timing is host-dependent and was not used as
an acceptance criterion.

## Ramps made from achieved grades

```powershell
dotnet run --project src/Tangles.Generator -c Release -- --mode full-coverage --ramp easy..hard --count 12 --seed 42 --max-trials 20 --improve-steps 20 --output puzzles/ramp-8x8-easy-hard.json
```

This creates four easy, four medium and four hard puzzles. Ascending ranges
such as `easy..medium`, `medium..hard` and `easy..expert` are also accepted.
Without `--count`, ramps request 12 puzzles. Every requested grade receives at
least one puzzle; extra puzzles go to earlier sections. Within each section,
output is ordered by the solver's measured score and effort. A candidate must
achieve its exact section grade and optional bounds; an unmet target never
receives an easier substitute or a partial output file.

Ramps choose pair counts, so do not combine `--ramp` with `--pairs` or
`--difficulty`. Explicit length and work budgets remain authoritative. Without
overrides, larger boards receive longer layout and solver budgets. Initial
easy/medium/hard recipes are:

| Board | Easy pairs | Medium pairs | Hard pairs | Layout budget | Solver work |
|---|---:|---:|---:|---:|---:|
| 8x8 | 10 | 9 | 8 | 500 | 2,000,000 |
| 12x12 | 24 | 20 | 18 | 5,000 | 20,000,000 |
| 16x16 | 43 | 36 | 32 | 10,000 | 100,000,000 |

The medium-recipe survey illustrates why pair counts were changed: with 100
candidate proposals, nine pairs selected medium on 9/10 tested 8x8 seeds,
compared with 7/10 at eight pairs. On 12x12, 20 pairs succeeded on 3/3 tested
seeds; 12 pairs succeeded on none. On 16x16, 36 pairs succeeded on 2/2 tested
seeds, while 20..32 pairs missed medium on those seeds. These are small samples
of solver selection success, not guarantees or human playtesting. The larger
recipes intentionally contain many endpoints; players repeat their colour
palette and distinguish additional pairs by label.

The pack records `generation.ramp`, `rampSections`, actual per-puzzle `recipe`
values and each puzzle's complete selection policy. A pack-level parameter is
null when recipes differ, rather than claiming one pair count for the whole
pack. Existing format-v1 players continue reading the unchanged rules and clues.
Replay a puzzle using its stored seed, board dimensions, recipe and per-puzzle
selection options. The default random fast generator retains the captured
positive-seed clue/witness mapping; new sampling/selection behaviour is versioned.

Sample packs, with an explicit 20-elimination ceiling, are checked in under
`puzzles/ramp-8x8-easy-hard.json` (12 puzzles),
`puzzles/ramp-12x12-easy-hard.json` (six) and
`puzzles/ramp-16x16-easy-hard.json` (six). Each stored solution has been completed
through the shared player rules. These estimates do not promise equal jumps in
human difficulty between puzzles or grades.

## Reproducing the measurements

Use `tools/benchmark-pack-quality.fsx` with references to built Core/Generator
assemblies and an output directory. The default count is 150. Add
`--full-recipes` for the denser medium/hard surveys, or `--recipes-only` to skip
the cut comparison. The checked-in `pack-quality-results.json` contains summary
counts, budgets, fixed seeds, sample-pack verification and source hashes. Raw
diagnostics use the reusable scratch directory, outside source and releases.
