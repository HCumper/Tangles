# Stronger 12×12 collections

The first 100-puzzle collections were too easy in human play. In particular,
their geometric connect-only grades did not reject simple routes: the greedy
shortest-route player solved all 100 Easy and Medium puzzles, and 93 of 100 Hard
puzzles, within 256 attempts. Within 32 attempts it solved 100, 99 and 71
respectively. The full-coverage collections had 24, 20 and 18 pairs: their stored
paths averaged only 6, 7.2 and 8 squares.

The replacement collections keep the same rules, board size and count. They
use new clues, exclude previously served puzzles and their board symmetries,
and apply the following selection rules.

| Collection | Pairs | Selection requirement |
|---|---:|---|
| Connections only — Easy | 8 | Greedy shortest routes fail for the first 32 attempts, then succeed within 256. |
| Connections only — Medium | 10 | Greedy shortest routes fail for the first 256 attempts, then succeed within 2,048. |
| Connections only — Hard | 10 | Greedy shortest routes fail for all 8,192 attempts. |
| Full coverage — Easy | 12 | Completed solver assessment requires at least one trial elimination; no branching is needed. |
| Full coverage — Medium | 8 | Completed solver assessment requires branching: depth at least 2, and 8–255 nodes before the first answer. |
| Full coverage — Hard | 8 | Completed solver assessment requires depth at least 8 and at least 512 nodes before the first answer. |

Full-coverage witness lengths now average 12, 18 and 18 squares. All three
full-coverage requirements include a completed uniqueness proof, because the
existing solver only assigns its reasoning grades after that proof. The rules
still accept any valid solution. Connect-only selection does not require a
unique answer.

These are provisional collection tiers calibrated to the player's feedback.
They are separate from the original geometric or solver grade in each
`difficulty` record. In particular, the new full-coverage Easy collection has
the solver's `hard` grade, while both higher collections have its `expert`
grade. The original assessments remain intact. Pack metadata under
`generation.collectionSelection` records the displayed tier, exact thresholds,
rejections and generation time. Each connections-only puzzle also records its
actual `shortestRoutes` screen result.

Each collection opens with a representative puzzle and shuffles the remainder
with a recorded seed. It does not put the weakest puzzles at the front. The
separate earlier ramp samples still provide an explicitly rising sequence.

A failed greedy screen demonstrates that those tested strategies failed; it
does not prove that a human will find the puzzle difficult. Likewise, search
depth is a property of this solver rather than a universal human difficulty
scale. The stricter tests remove the measured weakness of the previous packs
and provide a stronger starting point for further playtesting.

The completed batch passed shared loading, path validation, drawing, hint
completion, duplication checks and a repeat of every shortest-route screen.
The full-coverage Medium median is 74 search nodes before the first answer;
Hard is 1,196, with a minimum of 520. Full counts and ranges are recorded in
[the comparison results](collection-calibration-results.json).

## Regeneration

`tools/regenerate-collections.fsx` uses the existing generator and solver; it
does not modify the game rules or overwrite a previously generated output.
Run it with references to the Release Core and Generator assemblies, followed
by these five arguments:

```text
mode tier count output-directory work-directory
```

For example, use `full-coverage hard 100` followed by the puzzle output folder
and a scratch work folder. Supported modes are `connect-only` and
`full-coverage`; tiers are `easy`, `medium` and `hard`. New files initially have
a `-v2.json` suffix. After validating all six complete packs, replace the
previous collections at their original published filenames so existing pack
links load the new puzzles. The catalogue adds `?edition=2` to avoid serving a
cached earlier pack; older unversioned bookmarks resolve to the listed edition.
Earlier versions remain available in Git history.

The scratch folder holds accepted-puzzle checkpoints. A cancelled run can
resume from that folder; it never publishes an incomplete collection. Full
coverage uses independent candidate searches in parallel, consuming results
in seed order. Each run is limited to 60 minutes and a bounded request count;
it retains checkpoints if those limits are reached. Individual clue/witness
IDs and assessments remain available for replay. Use a fresh work folder when
intentionally starting another selection against a different excluded corpus.
