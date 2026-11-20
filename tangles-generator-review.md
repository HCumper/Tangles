# Tangles puzzle generator review (C:\Source\tangles, HEAD e46d802)

Review only. No source changes, commits or pushes. `dotnet test` rebuilt the
gitignored bin/obj output. Probe scripts: scratchpad probe1-5.fsx, run against
the built Debug DLLs.

Scope: src/Tangles.Generator (all 9 files) plus Tangles.Core Puzzle.fs and
Validation.fs, which the generator uses for its checks.

## Verified sound (no finding)
- Every returned puzzle has a witness that passes `Validation.solution`, and Export
  checks it again, so no unsolvable output.
- CoverageSearch.analyse/partition DP: segment validity (`left` is a cumulative max,
  `ends` uses two pointers), exact-count DP bounds and reconstruction are all correct.
  Backbite reversal ranges are also correct.
- Every loop has a bound: tryGrowPath (2048-node budget), MaxAttempts, MaxCandidates,
  solver MaxWork/MaxSearchNodes, and the batch candidate budget. No unbounded retry.
- Connect-only `random.Next(min, maximum + 1)` can never get min > max, because
  `maximum >= MinPathLength` is an invariant.
- Solver deductions (degree rule, Tarjan separators, trial elimination) preserve all
  solutions, so a "unique" verdict cannot be spurious. The 64-bit domain edge is fine.
- One-line `if c then a; b` (Solver.fs:273, GuidedGeneration.fs:151) runs both
  statements inside the then-branch (checked in fsi), so it is not a bug.

## Findings, most severe first
1. Negative seeds alias positive ones. Generation.fs:202, GuidedGeneration.fs:40,
   Program.fs:14 (CommandLine.fs:120 accepts any int).
   - The cause is `new Random(seed)`, which uses |seed|. It also treats
     Int32.MinValue the same as Int32.MaxValue.
   - Result: `--seed -42` gives the same puzzle as `--seed 42`, in both modes.
   - The batch seed stream is the same too, so `--seed -42 --count N` produces the
     same pack as `--seed 42`. Only the stored seeds differ.
   - Probe: board keys are equal for 42/-42, full-coverage 7/-7 and MinValue/MaxValue.
2. Batch seeds overlap the per-request retry stream (non-guided selection:
   --avoid-shortest, --require-unique or --assess without --difficulty).
   - Program.fs:14/25 and GuidedGeneration.fs:40/176 both use the `Random(S).Next()`
     sequence, so request k's first candidate is request 0's k-th retry.
   - If request 0 accepts retry j, request j returns the same board, which is then
     discarded as a duplicate. Requests 1..j-1 each re-test a seed already rejected.
   - Probe: 23 of 23 seeds (6x6, 5 pairs, min 3, --avoid-shortest).
   - CLI: `--count 10` with seeds 1-3 shows 1 duplicate each time.
   - Output stays correct because of dedup. The cost is wasted proposals and
     correlated streams.
3. Targeted full coverage can never use the structured fallback.
   GuidedGeneration.fs:118-143 always starts from `Fresh true`. docs/solver-guided-generation.md:102
   documents this.
   - With exact lengths, e.g. 8x8, 8 pairs, min=max=8, `--difficulty easy|medium|hard`
     ends with "0 assessed" after 100 x 500 traversal evaluations.
   - Fast mode succeeds on 10 of 10 seeds with the same settings.
   - The error at :187-189 says "Try more --candidates", which cannot help, and it
     does not separate "no layout found" from "no layout reached the target".
4. Internal errors are handled inconsistently.
   - GuidedGeneration.fs:179 discards every `Error _` from Generation.generate,
     including "Internal solution validation failed". In selection mode a generator
     bug therefore becomes a silent skip, while fast mode treats it as fatal.
   - GuidedGeneration.fs:115 raises `invalidOp`. Program.fs:97-109 does not catch
     InvalidOperationException, so the user gets an unhandled crash, not exit 1.
   - Latent: it only triggers if a witness is invalid.
5. `attempts` means different things per mode.
   - Connect-only: board attempts until success.
   - Fast full coverage: all evaluations, including refinement after the first success
     (Generation.fs:186). It is usually min(MaxAttempts, first + max(64, area)).
     For example, 64x64 with 256 pairs reports "500 attempt(s)".

## Cleanup
- Generation.tryCover takes a `neighbours` parameter it never uses (Generation.fs:135,
  passed at :217), and adds four forwarding lambdas (:138-141).
- Duplicated helpers:
  - `shuffle`: Generation.fs:60, CoverageSearch.fs:9, Solver.fs:343.
  - `label`: Generation.fs:67 and GuidedGeneration.fs:107.
  - Neighbour tables: Generation.fs:205, CoverageSearch.fs:17, Solver.fs:61, Difficulty.fs:97.
  - Solution-to-puzzle construction: Generation.fs:188 and GuidedGeneration.fs:104-116.
- The annealing loop is duplicated: Generation.fs:156-185 and GuidedGeneration.fs:118-143
  share the acceptance rule and the 0.4 temperature literal. It belongs in CoverageSearch.
- Generation.generate handles both modes in one body: the full-coverage branch, then
  `while options.Mode = ConnectOnly`, with an `attempt` the full-coverage branch never
  uses (:213-265).
- Hot-loop waste:
  - tryGrowPath allocates and shuffles every free cell, plus a new `local` array, for
    each path (Generation.fs:74-77). On a dense 64x64 board with 2048 domino pairs
    this costs about 75 ms per attempt, so a default 500-attempt failure takes ~40 s.
    The Array.filter/sumBy at :89-93 also allocate on every step.
  - CoverageSearch.analyse allocates three count-sized arrays per call (:45, :52, :58).
  - partition allocates (pairs+1) x (count+1) bools per call (:68): 8.4 MB at
    64x64 with 2048 pairs, on every refinement attempt.
  - Solver.routes allocates three arrays per colour per pass (:161-163), inside
    trial propagation.
- On 1-D full-coverage boards, refinement re-evaluates the same straight traversal up to
  max(64, area) times, because backbite does nothing there.
- Difficulty is assessed twice per puzzle (Export.fs:126 and :178).

## Tests
129 of 129 pass in about 5 s.

Covered:
- Rule validator cases.
- Independent witness checks across many sizes and seeds.
- Seed reproducibility.
- Cancellation.
- CLI validation.
- Attempt limits around 63/64/127/128/129/256.
- Solver counts against exhaustive enumeration for every two-pair 3x3 layout.
- The 64-label domain edge.
- Export/JSON and the player.

Gaps:
- No test that distinct seeds give distinct puzzles (finding 1). The negative-seed
  test only checks parsing.
- No batch test of seed-stream independence (finding 2).
- No test that the structured fallback actually runs or rescues tight lengths. The
  boundary test only checks attempts <= limit.
- No guided test with tight lengths.
- No generation above 16x16, and none with more than 26 pairs, so the "27" labels
  are never produced.
- The solver's soundness, which --require-unique and every --difficulty target rely
  on, is cross-checked only on 3x3 boards with two pairs. There is no randomized
  brute-force comparison on 4x4/5x5 boards with 3 pairs.
