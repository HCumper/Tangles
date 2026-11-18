# Solver-guided puzzle generation

Use `--difficulty hard` with full coverage to select puzzles that the clue-only
grader solves using contradiction trials, without branching. The stored answer
is never an input to the solver. All grades remain provisional until playtested.

## Running it

From the repository root:

```powershell
dotnet run --project src/Tangles.Generator -c Release -- --mode full-coverage --difficulty hard --count 20 --seed 42 --output puzzles/hard.json
```

This keeps the usual 8x8 board, six pairs and stored path lengths of 4..16.
Load the result using the web player's **Load puzzle pack** button. Existing
format-v1 readers continue to load the rules and clues.

Other useful starting points:

```powershell
# Assess the fast generator's output without selecting a difficulty.
dotnet run --project src/Tangles.Generator -c Release -- --mode full-coverage --assess --seed 42

# Easier, more densely clued 8x8 puzzles.
dotnet run --project src/Tangles.Generator -c Release -- --mode full-coverage --pairs 10 --difficulty easy --seed 42

# Route deductions, without contradiction trials.
dotnet run --project src/Tangles.Generator -c Release -- --mode full-coverage --pairs 8 --difficulty medium --seed 42

# Expert permits branching. Fewer pairs need more room for long stored paths.
dotnet run --project src/Tangles.Generator -c Release -- --mode full-coverage --pairs 4 --max-length 32 --difficulty expert --seed 42

# Connect-only: reject 32 greedy shortest-route attempts.
dotnet run --project src/Tangles.Generator -c Release -- --avoid-shortest --seed 42

# A verified 12x12/twelve-pair example, with explicit longer search and paths.
dotnet run --project src/Tangles.Generator -c Release -- --mode full-coverage --width 12 --height 12 --pairs 12 --min-length 3 --max-length 32 --max-attempts 5000 --difficulty hard --solver-work 20000000 --candidates 10 --improve-steps 0 --seed 42 --output puzzles/hard-12x12.json
```

The last recipe yields a puzzle requiring 73 contradiction eliminations in this
grader. Its stored path bounds are 3..32. Larger boards and fewer pairs can need
more search; these are starting points, not guarantees that every seed succeeds.
Increase `--solver-work` when candidates hit the assessment limit. Increasing
`--candidates` or `--max-attempts` instead gives the layout search more chances.

For a stronger 12x12 playtest, use fewer pairs and explicit expert floors:

```powershell
dotnet run --project src/Tangles.Generator -c Release -- --mode full-coverage --width 12 --height 12 --pairs 6 --min-length 3 --max-length 48 --max-attempts 10000 --difficulty expert --min-depth 10 --min-search-nodes 5000 --solver-work 100000000 --candidates 80 --improve-steps 20 --seed 42 --output puzzles/expert-12x12-6pairs-challenge.json
```

This example reaches its first answer after 10 nested cell-colour branch
decisions and 10,712 search nodes. An eight-pair variant with `--min-depth 8`,
`--min-search-nodes 700` and `--candidates 40` reaches its first answer at depth
8 after 723 nodes. Both complete the uniqueness proof within the limits. These
are strategy-specific measurements, not guarantees of human difficulty.

Pair count is a useful coarse dial: the supplied experiment suggests roughly
area/6 for easy, sqrt(area) around medium/hard, and sqrt(area)/2 for expert.
Actual selection uses the solver; it does not assume a pair count establishes
difficulty. Pair counts and length bounds remain explicit user choices.

## Reasoning model

`Solver.fs` assigns each board cell a domain of possible colours. Connect-only
boards also have an empty-cell value. Under the no-self-touching rule, adjacent
cells of one colour must be joined, so endpoints need exactly one same-colour
neighbour and path interiors need exactly two.

The grader applies these stages in order:

| Tier | Reasoning | Full-coverage grade |
|---|---|---|
| `local` | Remove colours with insufficient/excess matching neighbours; force or exclude neighbours when degree requirements determine them. | Easy |
| `routes` | Remove unreachable colour regions; force cells through which every remaining endpoint-to-endpoint route must pass. | Medium |
| `trial` | Assign one possible colour to one cell, run local and route deductions to a fixed point, and eliminate the assumption if it contradicts the rules. | Hard |
| `search` | Every available single assumption has been tried without finding another elimination; continue with branching search. | Expert, if uniqueness is established |

Route deductions use a low-link DFS to find all endpoint-separating vertices in
one pass per colour. They do not enumerate every possible route. Trial scans
return to cheaper deductions after each successful elimination.

The solver reconstructs and validates every returned answer. A fully forced
assignment proves uniqueness because every deduction preserves all possible
solutions. Otherwise, search counts answers up to two. Finding one answer before
a cutoff does **not** prove uniqueness. Multiple answers do not receive an
expert grade merely because the grader had to branch to choose one.

Inputs are canonicalised by endpoint positions, independently of labels, pair
order and endpoint direction. Trial order is deterministic. Rotations and
reflections can change the strategy's workload; the grade is not a mathematical
invariant or a calibrated human rating.

## Candidate selection and improvement

Full-coverage targets use `solver-guided-hamiltonian-partition-v1`:

1. Mix a fresh Hamiltonian traversal using the shared backbite implementation.
2. Search for an exact split into induced paths within the requested lengths.
   Larger boards get longer uninterrupted descent before restarting. The fast
   generator's structured fallback is not used to satisfy a difficulty target.
3. Assess a valid candidate from its endpoints only.
4. Apply 1..4 backbites and cut again. Keep proposals that improve the target's
   reasoning tier/refutation count, allowing equal-score moves for exploration.
5. Retain the best matching candidate across restarts. Stop with an error when
   the budget expires without reaching the requested tier.

Targets are exact. Hard selection accepts trial-based solutions without
branching. Expert is a separate explicit choice. An easier puzzle is never
silently substituted for an unmet target.

Selected batches exclude duplicate clue layouts, ignoring labels, pair order
and endpoint direction. Rotations/reflections remain distinct. They retry failed
requests with deterministic new seeds, retain each puzzle's replayable request
seed, and sort full-coverage output by score and trial count. Every player still
accepts any valid completion; uniqueness is a generation property, not an added
game rule. `--require-unique` explicitly enforces it for otherwise unfiltered
selection as well.

## Budgets and limits

- `--difficulty any` keeps the fast generator when no other selection flag is
  supplied. This preserves its generation behaviour and seed-to-clue mapping.
- `--max-attempts` bounds each fresh traversal search (default 500).
- `--candidates` bounds selection proposals per request (default 100), including
  unsuccessful layouts and duplicate clues.
- `--improve-steps` bounds proposals after the first matching candidate (default
  100), within the overall candidate limit. Zero returns the first match.
- `--solver-work` limits each assessment deterministically (default 2,000,000
  counted operations). Search also has a 20,000-node limit.
- `--min-trials` sets a minimum successful contradiction count for hard/expert
  targets. Zero, the default, adds no floor.
- `--min-depth` and `--min-search-nodes` apply to expert targets. They set minimum
  branch depth and visited nodes at the **first valid answer**, respectively.
  Both default to zero. Work spent subsequently proving uniqueness cannot
  satisfy these floors. Incomplete solver runs are still rejected.
- A selected batch has at most `2 * count * candidates` proposals in total. A
  new request starts only when enough budget remains for its full candidate
  allowance. Rejected duplicates and failed requests consume this budget.
- Selected batches write only complete packs, using atomic replacement. An
  exhausted batch leaves an existing destination untouched. Progress is printed
  to stderr, and Ctrl+C cancels layout search and solving.
- Reasoning selection supports dimensions up to 16x16, at most 64 full-coverage
  colours, or 63 connect-only colours plus empty. These are supported sizes,
  not promises of fast selection. Fast generation keeps its previous 1..64
  dimension range.

Budget exhaustion is `budgetExceeded` or `notEstablished` evidence. It never
awards hardness or uniqueness. Geometry remains available when reasoning cannot
produce a grade.

## Connect-only policy

Full-coverage reasoning grades are not applied to connect-only puzzles, where
unused space and multiple acceptable answers can make an unresolved domain
model an easy game. `--avoid-shortest` instead screens 32 deterministic attempts
with different pair orders and shortest-route tie choices. A win is validated
as a complete legal answer. Failure means only that this particular screen did
not solve the puzzle; it does not prove human difficulty or uniqueness.

## JSON evidence and scoring

Completed, uniquely solved full-coverage assessments use
`difficulty.method: "cell-domain-reasoning-v1"` and
`profile: "full-coverage-reasoning-v1"`. The geometry measurements remain
diagnostic; their original grade, score, thresholds and components are retained
in `difficulty.geometricEstimate`.

Scores are provisional and deterministic:

- Local: `10 + min(19, floor(localDeductions / boardArea))`.
- Routes: `30 + min(24, routeDeductions)`.
- Trial: `55 + min(24, trialSteps)`.
- Search: `80 + min(20, floor(2 * log2(searchNodes + 1)))`.

`components` and `rawScore` describe this score. The uncapped trial count remains
available and is used to order puzzles whose displayed score has saturated.
There is no calibrated cross-profile comparison with the older geometry scores.

`difficulty.reasoning` records tier, grade, proof status, domains removed by
local/route deductions, changed route passes, successful trial eliminations,
assumptions tested, search nodes, capped solution count, search completeness,
uniqueness, consumed work and the work limit. Local/route deduction counts
exclude deductions inside speculative branches; speculative work is still
charged to the work budget.

`firstSolutionSearchNodes` and `firstSolutionBranchDepth` describe the first
answer found by the search after the initial deduction/trial pass.
`maxSearchDepth` covers the whole search. Logical solutions have zero search
nodes and depth. Depth counts cell-colour decisions along the successful DFS
branch; it is not a minimum over all search orders, or a count of guesses a
human must make. The total `searchNodes` includes subsequent uniqueness work.

When any minimum is enabled, generation records
`solver-guided-hamiltonian-partition-v2`. Expert depth/effort floors also make
hill-climbing and pack ordering prefer first-answer depth, then first-answer
nodes, then trial count. Otherwise v1 generation order is preserved. The
`cell-domain-reasoning-v1` tiers and displayed score formulas are unchanged;
the new measurements are additive. Generation policy and per-puzzle selection
metadata retain all three floors so the selection can be replayed.

`verification.uniqueness` is `unique`, `multiple`, `none`, `notEstablished` or
`notChecked`; valid generated witnesses prevent a legitimate `none` result.
`verification.difficulty` is `reasoningEstimate` when the primary grade comes
from completed reasoning. `selection` records actual proposals, the first assessed
candidate's tier/trials and policy limits. `generation.selection` additionally records batch
requests, skips and the overall proposal budget. Format version remains 1.

## Measured comparison

The baseline was the generator at the start of this change, including its v2
backbite mixing. Forty seeds from the old CLI's seed-42 batch were replayed;
all baseline clues matched the preserved pre-change pack exactly. Both versions
used 8x8, six pairs, lengths 4..16, and 500 layout attempts. The new policy used
100 proposals, 100 improvement steps and the default solver work limit.

| Measurement | Fast baseline | Hard selection |
|---|---:|---:|
| Local deductions sufficient | 26/40 | 0/40 |
| Requires contradiction trials | 13/40 | 40/40 |
| Ambiguous, ungraded | 1/40 | 0/40 |
| Mean trial eliminations | 6.6 | 36.4 |
| Trial elimination range | 0..41 | 8..58 |
| Distinct clue layouts | 38 | 40 |
| Mean generation time | 2.0 ms | 90.5 ms |

All 40 selected puzzles were uniquely solved without branching. Initial mixed
candidates averaged 18.6 eliminations; refinement added an average 17.9. The
selected puzzle for an individual seed need not be harder than that seed's
baseline: selection targets a tier and starts from a different layout.

Timing is one warmed Release run on this Windows/.NET 11 machine. Fast timing
includes construction/validation; guided timing also includes candidate solving
and improvement. JSON, console output, baseline analysis and the independent
checker are outside these timers. This is an indicative cost comparison, not a
controlled throughput study or a human playtest.

All 125 tests pass. Independent whole-path enumeration agrees with the solver
on all 756 two-pair 3x3 layouts across both modes, plus 72 single-pair cases.
Eight selected samples spanning 8..58 F# eliminations were also solved by the
supplied Python reference: all required its trial tier and had exactly one
answer after a completed search. Trial counts can differ with scan order.
Both sample packs (21 puzzles total) also load and complete through the shared
player operations, and their score components audit correctly. Mid-run guided
generation cancellation was observed after 27 ms with a 25 ms cancellation timer.

The checked-in [benchmark summary](solver-guided-benchmark.json) retains results,
sample evidence and hashes. Raw packs/reports are in the ignored
`artifacts/harder-puzzles` directory, including `hard-8x8.json` (20 puzzles) and
`hard-12x12.json` (one puzzle using the larger-board recipe above).

Reproduce the paired comparison with a preserved baseline pack:

```powershell
dotnet build src/Tangles.Generator -c Release --disable-build-servers
dotnet fsi --reference:src/Tangles.Core/bin/Release/net11.0/Tangles.Core.dll --reference:src/Tangles.Generator/bin/Release/net11.0/Tangles.Generator.dll tools/benchmark-reasoning.fsx artifacts/harder-puzzles/before-8x8.json artifacts/harder-puzzles/reasoning-8x8.json
```
