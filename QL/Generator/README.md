# Native QL Tangles generator

An SSB implementation of the ordinary F# generator's command-line specification.
Generation, solving, difficulty assessment, deduplication and JSON writing run
inside the QL executable. Python and .NET are development/test tools only.

The target is an expanded QDOS/SMSQ/E machine with **16 MiB RAM**, Toolkit II and
Turbo Toolkit. No Pointer Environment or high-colour display is required.
The tested compiler is **SSB 2.8 / Turbo 5.09**. A stock 128 KiB QL is not a target.

## Build

On this Windows development machine:

```powershell
Set-Location C:\Source\Tangles
python QL/Generator/build.py
```

Requires Python, a JDK and the already prepared external SSB/SMSQmulator toolchain.
The external toolchain defaults to `C:\Source\Structured Super Basic`; override
it with `--ssb-root`. Disposable compiler files are reused under
`C:\Source\codex-workspace\.scratch\tangles-ql-generator\build`; `--work` selects
another build directory. The script runs the actual translator and Turbo inside
SMSQ/E, using the wide-code model and brief optimisation. It does not translate
the puzzle algorithms to Python or call the F# generator for answers.

The current release is `build/latest/generator_exe`, with `ttk`, translated BASIC,
compiler diagnostics and a manifest containing source/executable hashes.
Build outputs and external runtimes are ignored by Git. Turbo's local-string
diagnostic warnings are expected; a successful build has zero errors and
completed code generation.

For a manual QL build, copy the source modules as `model_ssb`, `maths_ssb`,
`construction_ssb`, `identity_ssb`, `solver_ssb`, `difficulty_ssb`, `selection_ssb`,
`cli_ssb`, `export_ssb`, `batch_ssb` and `generator_ssb`. The build script supplies
the wrapper directives, SSB line numbering starting at 1/incrementing by 1,
8 MiB application dataspace and the external Turbo compiler tasks.

## Run in SMSQ/E or QDOS

Copy `generator_exe` and `ttk` from the release directory to a mounted QL drive.
Preserve the executable's SFA/QemuLator header and dataspace when copying it,
using a QL-aware drive or transfer tool. With the supplied SMSQmulator host
directory use `sfa1_generator_exe`, as the development runner does. Load Turbo
Toolkit once in that emulator session:

```basic
LRESPR win1_tangles_ttk
EW win1_tangles_generator_exe;"--help"
EW win1_tangles_generator_exe;"--seed 42 --count 10 --output win1_new_pack_json"
```

Full coverage and an exact easy target, on a deliberately small board:

```basic
EW win1_tangles_generator_exe;"--mode full-coverage --width 3 --height 3 --pairs 3 --min-length 3 --max-length 3 --seed 17 --difficulty easy --candidates 1 --improve-steps 0 --output win1_small_pack_json"
```

Use native QL filenames, not Windows paths. For a filename containing spaces,
quote it inside the option string (use single quotes around the BASIC string).
An output directory/device must already exist. The program opens its own standard 512x256 console. Escape or Ctrl+C requests
cancellation. A nonzero portable exit code is returned to the owner as the
corresponding **negative QDOS job error**: success 0, failure -1, argument/output
conflict -2, cancellation -130.

For an isolated headless development run:

```powershell
python QL/Generator/run.py -- --seed 42 --output nfa1_pack_json
```

Only the selected `--work` directory is mounted as `nfa1_`/`sfa1_`; by default
this is the generator scratch `run` directory. The generated host file is
`pack_json` in that directory. The script executes the compiled QL program.
It never generates a puzzle on the host.

Both front ends accept the version-1 JSON unchanged. Import its QL filename
with L in the native player, or copy the file to Windows and import it on the
web. Stored solutions support the same undoable colour hints.

## Options and behaviour

The ordinary F# specification is documented in
[Generation Options.md](../../src/Tangles.Generator/Generation%20Options.md).
All its generator flags are provided:

| Group | Options |
|---|---|
| Board/rules | `--mode connect-only\|full-coverage`, `--width`, `--height`, `--pairs` |
| Construction | `--min-length`, `--max-length`, `--max-attempts`, `--cut-style random\|varied` |
| Batch/replay | `--seed`, `--count` |
| Selection | `--difficulty any\|easy\|medium\|hard\|expert`, `--ramp`, `--candidates`, `--improve-steps` |
| Evidence/effort | `--solver-work`, `--min-trials`, `--max-trials`, `--min-depth`, `--min-search-nodes` |
| Optional filters | `--assess`, `--require-unique`, `--avoid-shortest` |
| Output | `--output`, `--force`, `--show-solution`, `--help` / `-h` |

Defaults are 8x8, six pairs, connections-only, path lengths 4..16, 500 attempts,
one puzzle, 100 selection proposals, 100 improvement proposals and 2,000,000
solver work units. Dimensions support 1..64; count supports 1..10,000. Duplicate
flags, invalid/missing values, negative seeds and incompatible policy combinations
are rejected. A ramp defaults to 12 puzzles and plans balanced exact-grade
sections, including the F# automatic pair/length/attempt/solver-budget recipes.

Both modes require orthogonal, disjoint paths with no self-touching. Only full
coverage requires every cell. Path-length bounds constrain the stored answer;
they do not become player length clues. Uniqueness is an optional generator
filter, not a player rule. Every candidate exported as a puzzle is checked by a
separate final validator.

Connections-only uses bounded depth-first induced-path growth. Full coverage
uses a Hamiltonian traversal, a long random backbite mixing phase, varied
restarts, feasible-cut dynamic programming and bounded annealing/refinement.
Varied cuts use the same modest turn/border/length preferences. Targeted full
coverage searches retain and mutate layouts according to their solver effort.

The clue-only solver implements local degree deductions, Tarjan route and
connectivity deductions, contradiction trials and bounded branching. It counts
up to two answers and distinguishes uniqueness proofs, ambiguity, unsatisfiable
clues and incomplete budgets. It supports boards up to 16x16 and 64 full-coverage
or 63 connections-only labels. Unsupported/incomplete evidence never establishes
uniqueness or an expert grade. The geometric grader preserves the separate
coverage profiles and underlying per-pair/board measurements.

Explicit grades are exact and require a completed full-coverage uniqueness
proof. Easy uses local deductions, medium additional route deductions, hard
contradiction trials and expert branching. Effort floors/ceilings have the same
meaning as F#. The connections-only shortest-route screen tries 32 deterministic
greedy orders/routes. A screen failure is not a solvability or hardness proof.
Difficulty grades remain provisional.

Batches reject rotations/reflections as well as identical clues, and selected
full-coverage packs are sorted by the same grade/effort ordering. Request,
candidate, search-node and work budgets remain separate. Failed exact targets
do not fall back to easier puzzles. Complete output is staged before publication;
an existing file needs `--force`. QDOS has no portable atomic replacement call:
publication copies the completed staged pack to its destination. With `--force`,
a closed backup of the original is kept until the replacement closes successfully
and is restored after a trapped publication failure. This also works with Windows
NFA devices whose rename operation fails on open host files. A crash during the
final copy can expose a partial destination; media failures can require recovering
the retained backup. It is not an atomic filesystem replacement.

## Native differences and performance

- This is a port of the ordinary CLI. The separate Windows collection
  recalibration/checkpoint script is not a QL command-line feature. Its larger
  shortest-route trial recipes can be scripted using `shortest_screen` in SSB.
- SHA-256 clue/report/progress identities and the `splitmix64-batch-v1` seed
  derivation are portable. Native algorithms have `ql-` names and explicitly
  record `ql-subtractive-int31-v1` randomness. Repeating the same native recipe
  and version is deterministic; matching an F# seed does not promise the same
  puzzle, because QL numeric precision and random interval arithmetic differ.
- Geometry values are rounded to six decimals, as in F#, but their last decimal
  places can differ slightly because QL floating-point arithmetic has less
  precision. The reports retain actual reasoning/work, never elapsed-time grades.
- Numeric array limits are avoided for domain frames, coverage DP and batch
  ordering by using owned heap blocks. Puzzle/console output is spooled to disk,
  so the full pack does not have to fit in memory. Keep free space on the output
  device for staged output and, with `--force`, the original backup.
- The option string is limited to 4096 bytes/64 arguments; output filenames to
  200 bytes, leaving room for safe staging names. Classic QDOS device limits
  still apply. A supplied maximum path length above 4096 is retained in metadata
  and effectively bounded by board size during construction.
- Native QL solving and hard-puzzle selection can be much slower than .NET.
  Start with small boards and modest budgets. A valid configuration can exhaust
  its limits without finding a candidate; that does not prove impossibility.

## Verification

The tests execute real compiled 68000 code under SMSQ/E. Reference reports come
from the actual F# solver/grader; exported packs are checked by the shared F#
loader, rules, player hints, identities and difficulty logic.

```powershell
$qlScratch = 'C:/Source/codex-workspace/.scratch/tangles-ql-generator'
dotnet build src/Tangles.Generator -c Release --artifacts-path "$qlScratch/artifacts"
dotnet fsi "--reference:$qlScratch/artifacts/bin/Tangles.Core/release/Tangles.Core.dll" "--reference:$qlScratch/artifacts/bin/Tangles.Generator/release/Tangles.Generator.dll" QL/Generator/Oracle.fsx -- "$qlScratch/oracle.json"
python QL/Generator/test_core.py
python QL/Generator/build.py
python QL/Generator/test_policy.py
python QL/Generator/build.py --entry QL/Generator/tests/construction.ssb --work "$qlScratch/construction" --run
python QL/Generator/test_generator.py
```

`test_core.py --quick` uses a smaller cross-section, including high domain bits
and budget limits. `test_policy.py` checks a hard stored witness, exact-grade and
effort filters, symmetry identities, seed derivation and 12x12 ramp planning.
The construction test covers fully populated DP words and mixed 8x8/12x12
full-coverage boards. Compiler diagnostics and native test reports are retained in
the reused scratch directories; `validation.json` records verified source hashes
and the test scope for the checked release.
