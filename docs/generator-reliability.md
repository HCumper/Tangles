# Generator reliability and replay

The generator now distinguishes invalid options, an exhausted search, and an
internal invariant failure. Only an exhausted search may be retried during
selection. An invalid witness or a solver contradiction stops the batch; it is
never counted as an ordinary rejected puzzle. The CLI returns 1 for an internal
failure and preserves an existing output pack. Invalid command-line options
return 2, and cancellation returns 130.

## Seeds

Seeds must be in `0..2147483647`, in both the CLI and the generation API.
Negative seeds previously aliased their positive counterparts through
`System.Random`. Replay an older `-N` seed as `N`; the old `Int32.MinValue`
special case replays as `2147483647`.

The first request in a batch keeps the supplied seed. Later requests use the
counter-based `splitmix64-batch-v1` scheme, recorded as
`generation.batchSeedAlgorithm`. Candidate retries retain their existing
`Random(requestSeed)` stream, so they no longer systematically replay the batch
request stream. Finite seed and puzzle spaces still allow accidental collisions;
selected batches continue to reject duplicate clue layouts.

Eight captured positive-seed cases (0, 1, 7 and 42 in each coverage mode) retain
their exact fast-generator clues and witnesses after the workspace refactor.
This is a compatibility check, not a promise that different seeds always give
different puzzles. Batch output changes after the first request. Search versions
are now `hamiltonian-path-partition-v3` for fast full coverage and
`solver-guided-hamiltonian-partition-v5` for difficulty targets, or `v6` when
reasoning bounds are requested. Previous JSON packs remain readable.

## Tight lengths and structured starts

Both full-coverage generators share the bounded traversal search. Guided search
tries a structured start early when the requested bounds force exact lengths.
Otherwise, its final proposal may try a structured start if no layout has yet
been assessed. Mixed searches receive their full evaluation budget.

Every structured witness passes the same validation, solver, exact difficulty
target and optional uniqueness/floor checks as a mixed witness. A structured
candidate graded easy cannot silently satisfy a hard request. For example:

```powershell
dotnet run --project src/Tangles.Generator -c Release -- --mode full-coverage --pairs 8 --min-length 8 --max-length 8 --max-attempts 1 --difficulty easy --candidates 1 --improve-steps 0 --seed 42
```

Failure messages distinguish finding no valid layouts, exhausting solver
budgets, and checking layouts that do not meet the requested policy. Search
failure remains inconclusive about mathematical impossibility.

## Work measurements

Each exported puzzle has an additive `work` object:

| Field | Meaning |
|---|---|
| `boardAttempts` | Connect-only board construction attempts, including unsuccessful selection retries. |
| `traversalEvaluations` | Full-coverage traversal proposals analysed, including unsuccessful layout searches and local mutations. |
| `refinementEvaluations` | The subset of traversal evaluations spent refining a fast-generator witness after its first success. |
| `structuredStarts` | Number of structured traversal starts tried. |

Selection statistics separately expose `candidatesConsidered`,
`candidatesEvaluated` (distinct layouts screened), `solverAssessments`, and
`budgetRejections`. Guided improvement is bounded by `--improve-steps`; it is not
the fast generator's traversal refinement counter. The older mode-dependent
`attempts` field remains for compatibility; use the explicit counters to compare
work. Console output uses the same counters. The CLI prepares difficulty once
and shares that assessment between JSON and console output.

## Allocation and regression checks

Growth, traversal partitioning and solver route deductions reuse workspaces.
One-dimensional coverage stops after the first evaluation because backbite
cannot change that traversal. A 64x64, 2048-pair, length-2, 128-evaluation Debug
probe reduced allocated bytes from 1,331,527,336 to approximately 246,268,064
(about 81.5%). This measures allocation, not a guaranteed generation speedup.

Regression tests cover fatal-error propagation and atomic output, separated
batch/retry seeds, negative-seed rejection, structured tight-length selection,
fast fallback execution, work counters, 1-D refinement, and 64x64 generation
with numeric labels through 2048. Fixed-seed randomized three-pair layouts on
4x4 and 5x5 boards are compared with independent whole-path enumeration in both
coverage modes. Incomplete oracle searches produce no correctness verdict.

See [pack quality](pack-quality.md) for symmetry deduplication, optional effort
ceilings, experimental cut weighting and achieved-grade ramp packs.
