| Option                             | Purpose and default                                                        |
| ---------------------------------- | -------------------------------------------------------------------------- |
| `--mode MODE`                      | `connect-only` (default) or `full-coverage`.                               |
| `--width N`, `--height N`          | Board dimensions, each 1–64. Default **8×8**.                              |
| `--pairs N`                        | Number of endpoint pairs. Default **6**.                                   |
| `--min-length N`, `--max-length N` | Path lengths, **including endpoints**. Defaults **4–16**.                  |
| `--count N`                        | Puzzles to produce. Default **1**, or **12 for a ramp**.                   |
| `--seed N`                         | Reproducible seed, **0–2147483647**. Random when omitted.                  |
| `--difficulty D`                   | Exact target: `any`, `easy`, `medium`, `hard`, `expert`. Default **any**.  |
| `--ramp RANGE`                     | Graded sections, such as `easy..hard`; chooses recipes and pair counts.    |
| `--cut-style STYLE`                | `random` (default) or experimental `varied` weighting.                     |
| `--max-attempts N`                 | Board attempts or evaluations per fresh traversal search. Default **500**. |
| `--candidates N`                   | Selection proposals per request. Default **100**.                          |
| `--improve-steps N`                | Further proposals after finding a matching puzzle. Default **100**.        |
| `--solver-work N`                  | Work budget per solver assessment. Default **2,000,000**.                  |
| `--min-trials N`                   | Minimum contradiction eliminations for hard/expert. Default **0**.         |
| `--max-trials N`                   | Optional maximum contradiction eliminations. **No default ceiling.**       |
| `--min-depth N`                    | Expert: minimum branch depth before the first solution. Default **0**.     |
| `--min-search-nodes N`             | Expert: minimum search nodes before the first solution. Default **0**.     |
| `--assess`                         | Include solver reasoning and solution-count evidence.                      |
| `--require-unique`                 | Accept only puzzles proved to have one solution.                           |
| `--avoid-shortest`                 | Connect-only: reject puzzles solved by the greedy shortest-route screen.   |
| `--output PATH`                    | Write a JSON puzzle pack. Otherwise, console output only.                  |
| `--show-solution`                  | Also print the stored solution.                                            |
| `--force`                          | Permit replacing an existing output file.                                  |
| `--help`, `-h`                     | Show complete help.                                                        |

Difficulty targets, ramps and varied cuts require **full coverage**. Reasoning-based selection supports boards up to **16×16**. A ramp cannot be combined with `--pairs` or `--difficulty`; its larger-board budgets increase automatically unless explicitly overridden.

For example, create twelve 8×8 puzzles progressing from easy to hard:

```powershell
dotnet run --project C:\Source\Tangles\src\Tangles.Generator -c Release -- --mode full-coverage --ramp easy..hard --count 12 --seed 42 --output C:\Source\Tangles\puzzles\my-ramp.json
```

Batches automatically exclude rotated and reflected duplicates. All grades remain provisional estimates of human difficulty.