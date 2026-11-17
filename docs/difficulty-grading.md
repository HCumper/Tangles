# Initial heuristic difficulty model

The generator automatically grades each puzzle, using `Difficulty.assess` in
`src/Tangles.Generator/Difficulty.fs`. No additional command-line option is needed.
The console prints the label and score; each JSON puzzle includes the full report
under `difficulty`. Grades are **provisional estimates**, not measured human
difficulty, independent solvability proofs, or uniqueness claims.

The method is `geometric-heuristic-v1`. Its profiles are `connect-only-v1` and
`full-coverage-v1`. Changes to formulas, weights or thresholds require a new
method/profile version if ratings are to remain comparable.

## Inputs and scope

Only dimensions, labelled endpoint positions and `requireFullCoverage` are used.
The stored solution, generation attempts, random seed and runtime are not inputs.
The same clues get the same report even when a different valid witness is stored.
Pair order, endpoint direction, reflection and rotation do not affect the report.

Boards are limited to the generator's 1..64 dimensions. Clues are checked with
the shared validator; individually disconnected endpoint pairs are rejected.
This does not solve the joint routing problem. Export still validates the stored
witness separately before marking `solutionValid: true`.

## Measurements

For each pair, breadth-first searches find shortest routes while treating other
pairs' fixed endpoints as obstacles. Future paths are not reserved. A shortest
route cannot touch itself nonconsecutively: that contact would provide a shortcut.
Two searches and one board scan per pair keep this bounded at approximately
`O(pairCount * boardArea)` work, without enumerating paths. Cancellation is
checked during the searches and between puzzles.

`pairMeasurements` contains:

- `label`, `manhattanDistance`, `shortestDistance` (distances count steps).
- `shortestRouteCount`, capped at **1024**, and `shortestRouteCountCapped`, true
  only when the actual count exceeds that cap. These are individual shortest
  routes, **not the number of complete puzzle solutions**.
- `minEndpointExits`, `maxEndpointExits`: locally available first steps. Other
  clues block exits. Adjacent matching endpoints have only one legal connection
  under the no-self-touching rule. Counts above one can include moves that later
  prove unhelpful; this is not a continuation search.
- `shortestRouteInteriorCells`: the union of non-endpoint cells on any shortest
  route for that pair.
- `sharedRouteInteriorCells`: how many of those cells also occur in another
  pair's shortest-route union. This measures potential competition, not proven
  mandatory bottlenecks.

`measurements` records:

| Fields | Definition |
|---|---|
| `boardCellCount`, `pairCount`, `endpointCount` | Board area, number of pairs, twice the pair count. |
| `cornerEndpointCount`, `edgeEndpointCount` | Clues in corners; clues on edges excluding corners. Degenerate boards use distinct corner cells. |
| `meanCornerDistance`, `meanEdgeDistance` | Mean Manhattan distance to the nearest corner; mean distance to the nearest edge. |
| `meanAvailableExits`, `singleExitEndpointCount` | Mean local exit count and number of endpoints with one exit. |
| `adjacentPairCount`, `alignedPairCount` | Matching endpoints one step apart; matching endpoints sharing a row or column. |
| `meanManhattanDistance`, `meanShortestDistance` | Mean pair distances before and after considering other clues as obstacles. |
| `minimumRequiredCells` | Sum of each pair's shortest distance plus one endpoint cell. |
| `minimumCoverageRatio` | `minimumRequiredCells / boardCellCount`; an individual-route lower bound. |
| `coverageSlackCells` | Board area minus `minimumRequiredCells`. |
| `requiredExtraCoverageCells` | Slack cells that must also be used in full coverage; zero in connect-only mode. |
| `forcedCornerTurnCount` | Unclued corners on a two-dimensional full-coverage board; zero otherwise. Such a corner must connect both neighbours. |
| `meanLog2ShortestRouteCount` | Mean base-2 logarithm of the capped individual route counts. |
| `shortestRouteCountLimit`, `cappedRoutePairCount` | The cap and how many pairs exceed it. |
| `routeInteriorOccurrences`, `sharedRouteInteriorOccurrences` | Sum of pair corridor sizes; sum of their shared sizes. A cell used by three corridors contributes three occurrences. |
| `routeOverlapRatio`, `maxRouteOverlap` | Shared occurrences divided by all occurrences (zero when none); maximum number of corridors containing a free cell. |
| `crossLabelEndpointAdjacencies`, `clueCrowdingRatio` | Orthogonally adjacent endpoints of different pairs; twice that edge count divided by four times the endpoint count. |

Some diagnostic measurements are not assigned a separate score weight: for
example, edge effects already influence exit counts. There is no claim that
all these features independently predict human performance.

## Score calculation

Each component in the JSON has `name`, normalized `value`, signed `weight` and
`points`. Values are clamped to 0..1 and rounded to six decimal places. Points
are `value * weight`, rounded to six decimals. `rawScore` is their rounded sum;
`score` rounds that sum to an integer (midpoints away from zero), then clamps
it to 0..100. Negative contributions are credits for apparent constraints.

The following are **hand-set initial weights**, not fitted coefficients:

| Component | Normalized value | Connections only | Full coverage |
|---|---|---:|---:|
| `boardScale` | `log2(board area) / 12` (4096 cells gives 1) | 10 | 10 |
| `pairLoad` | Pair count / 16 | 10 | 8 |
| `endpointFreedom` | `(mean available exits - 1) / 3` | 18 | 16 |
| `shortRouteAmbiguity` | Mean log2 route count / log2(1024) | 12 | 8 |
| `routeOverlap` | Route overlap ratio | 22 | 24 |
| `clueCrowding` | Clue crowding ratio | 6 | 6 |
| `cornerDistance` | Mean corner distance / `max(1, floor((width-1)/2) + floor((height-1)/2))` | 6 | 6 |
| `minimumCoveragePressure` | Minimum coverage ratio | 16 | Not used |
| `requiredCoverageDetours` | Nonnegative coverage slack / area | Not used | 22 |
| `forcedEndpointCredit` | Single-exit endpoints / all endpoints | -20 | -18 |
| `forcedCornerCredit` | Forced corner turns / 4 | Not used | -6 |

Connect-only mode treats a large minimum space requirement as routing pressure;
unused slack is legal. Full coverage instead accounts for the extra cells that
must be incorporated, and credits forced turns through unclued corners. It does
not assume that full coverage is always harder.

| Profile | Easy | Medium | Hard |
|---|---|---|---|
| `connect-only-v1` | 0..29 | 30..54 | 55..100 |
| `full-coverage-v1` | 0..34 | 35..59 | 60..100 |

These boundaries also appear in each report as `thresholds.easyBelow` and
`thresholds.hardAtLeast`. Compare scores within a profile, not as a universal
cross-mode scale.

## JSON and compatibility

Each puzzle's `difficulty` object includes `method`, `profile`, `mode`,
`provisional: true`, `grade`, `score`, `rawScore`, `thresholds`, `measurements`,
`pairMeasurements` and `components`. For example, the default connect-only
8x8/six-pair puzzle with seed 42 currently receives `medium`, score 48.

`verification.difficulty` is now `"heuristicEstimate"`. Format version stays 1;
the added metadata does not affect IDs, clues, solutions or the coverage rules.
Existing packs without this object remain valid. The web and QL loading layers
ignore unknown metadata. Large packs may require a larger QL JSON node capacity
because measurements also consume parser nodes.

## Limits of the estimate

This first model does not implement human reasoning chains, count full
solutions, prove route bottlenecks, or assess the quality of a puzzle. Several
possible routes can make a puzzle flexible and easy rather than confusing;
geometric competition is only a proxy. A long stored witness does not make the
puzzle harder because it is not an input to the grade.

The current generator may produce batches concentrated in one category. Do not
force a grade distribution or present these labels as calibrated ratings.
Playtests and a reasoning solver should inform a later version of the model.
