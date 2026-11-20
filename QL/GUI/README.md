# Tangles QL player

Native SSB 2.8 / TurboPTR player for SMSQ/E at **800x600 or larger, GD2 high
colour**. This replaces the paint-grid proof with a playable game. It uses the
same generator JSON as the web player and ports `Tangles.Core.Play`'s rules;
`PlayerOracle.fsx` checks the compiled QL engine against that actual F# library.

- Endpoints, labelled colours, orthogonal dragging with interpolation and
  keyboard play. Start at either endpoint or resume an existing path; retrace to
  shorten. Occupied cells, foreign endpoints and self-touching are rejected.
- Both connections-only and full-coverage rules, taken from the JSON. Any valid
  solution counts. Stored witnesses and difficulty/verification claims are ignored.
- Pack chooser with board-size/coverage descriptions, local JSON import, previous,
  next and jump-to-puzzle controls. Failed imports preserve the current game.
- Whole-stroke undo (100 entries), undoable reset, connection/fill counts and
  distinct completion feedback for the two coverage modes.
- Automatic local progress on release, navigation and quit; last pack/puzzle
  selection is restored on restart. Progress is checked against the complete
  clues and validated with the game rules before use.
- Boards 1..64 on either axis. Sizes through 16x16 fit the board area; use zoom
  and keyboard cursor panning to inspect larger boards. Sixteen colours repeat
  for additional pairs, with labels identifying them as in the web player.

Drawing updates only changed path segments and changed text fields. Extending
a route keeps endpoint colours/letters and unrelated legend entries in place;
it does not clear squares or the sidebar first. Pointer motion within the same
square is coalesced, and keyboard cursor movement updates its old/new squares.
Full redraws are reserved for changing the board/view or requesting R. Ordinary
path extension checks only the four neighbouring squares and maintains counts
directly, avoiding repeated whole-board rule scans.

## Build on Windows

Requires Python, JDK, and the external SSB/SMSQmulator toolchain prepared at
`C:\Source\Structured Super Basic`. No emulator, ROM or proprietary QL extension
is included in this repository.

```powershell
cd C:\Source\Tangles
python QL/GUI/build.py --check
```

An existing TurboPTR copy avoids downloading it again:

```powershell
python QL/GUI/build.py --turboptr QL/GUI/build/latest/tptr_ext --check
```

The real SSB translator and Turbo compiler run inside SMSQ/E. `build/latest/`
contains the executable, dependencies, translated BASIC, build manifest and
diagnostics. The build also copies the three web packs and all JSON files from
`puzzles/`, unchanged, into a native `tangles_catalog` with dimension/coverage
descriptions. The executable reserves 10 MiB dataspace; use **16 MiB RAM** for
the supplied player setup and larger pack imports. A stock 128 KB QL is not a
target. Turbo's existing "only exists as a LOCAL" string warnings are diagnostic;
successful builds must have zero errors and complete code generation.

```powershell
.\QL\GUI\run.ps1
# Optional: choose a catalogue file and puzzle when launching.
.\QL\GUI\run.ps1 -Pack pack09_json -Puzzle 1
```

This opens a visible SMSQmulator with an isolated 800x600, mode-32, 16 MiB setup.
The launcher sets `SCREEN-UPDATE-RATE=8` (milliseconds), replacing the emulator's
50 ms default. This lets the display present pointer changes more frequently;
SMSQ/E's own pointer sampling and host scheduling still determine actual motion.
For an already running emulator, change its screen update interval to 8 ms in
the Config / Screen menu; no game restart is needed.
The 8 ms interval reduces the wait between an emulated pointer update and the
next display request. An isolated 90-move probe measured median request latency
of 16 ms, compared with 22 ms at a 16 ms interval (95th percentile: 29 vs 36 ms).
These are host-dependent emulator measurements, excluding Windows compositor
and monitor latency. The emulator's interrupt cycle remains 20 ms; this setting
does not give the guest a 125 Hz pointer. Idle CPU throttling stays enabled:
disabling it gave no clear further improvement in the same probe.
At its QL prompt enter `LRUN nfa1_boot`. Desktop files and persistent progress
are in `QL/GUI/build/desktop/`; rebuilding does not delete them. Close the old
emulator before relaunching the same desktop to avoid concurrent save writers.

## Native SMSQ/E launch

Copy `tangles_exe`, `ttk`, `tptr_ext`, `tangles_catalog` and the `packNN_json`
files to a QL-aware mounted directory. Preserve the executable's SFA/QemuLator
header and dataspace; do not prepend another header. For example:

```basic
DATA_USE "win1_tangles_"
LRESPR "win1_tangles_ttk"
LRESPR "win1_tangles_tptr_ext"
EXEC_W "win1_tangles_tangles_exe"
```

Load the extensions once per emulator session. Set the desktop resolution and
GD2 mode before launching; the player checks them and does not change them.
The catalogue and session file are read from `DATAD$`. Catalogue format is a
count, followed by alternating display-name and relative JSON-filename lines.
At most 64 pack choices are retained in a session. Imported packs join the chooser.

`tangles_config` is optional: its first line is the QL device/directory prefix
for progress files, e.g. `win1_tangles_saves_`. Otherwise progress is written to
the data directory. Imports accept a complete QL filename, e.g.
`nfa2_hard-12x12-100.json`, without converting the JSON on the PC.

## Controls

| Action | Control |
|---|---|
| Draw/retrace | Hold the left button from an endpoint or existing path |
| Keyboard drawing | Arrows move the cursor; Space/Enter starts/stops |
| End a keyboard stroke | Esc |
| Undo / reset | U or Ctrl+Z / C; reset can be undone |
| Previous / next / jump | P / N / J |
| Packs / import JSON | K / L |
| Zoom | + / -; arrows pan the cursor into view |
| Redraw / move window | R / M or the title bar |
| Save and quit | Q or the title-bar Quit button |

Click a legend entry to place the keyboard cursor at that pair. Paths remain
editable after completing a puzzle. All connected pairs with unfilled squares
produce an amber indicator in full-coverage mode; completed games show green.

## Persistence and bounds

Each puzzle has two alternating progress files. Writes replace the older slot;
a truncated latest save falls back to the previous valid generation. The full
ID, dimensions, coverage and ordered endpoint pairs are checked, so a filename
hash collision cannot apply another puzzle's routes. Invalid routes are discarded.
Undo history is session-local, matching the web player's restored progress.

The player import uses `pack_load_player`: up to **5 MiB of JSON bytes**, depth
32, at most 32766 retained clue nodes and pair records, with 128-byte UTF-8
IDs/labels. Ignored solutions/metadata still undergo syntax, Unicode and nesting
validation but consume no value nodes. The original 2.46 MB, 100-puzzle 12x12
pack is tested unchanged. These explicit QL limits can reject larger or more
densely populated packs that the web loader accepts.

The native QL font cannot render arbitrary UTF-8 labels. The loader retains
their exact bytes; endpoints use a numeric legend index if a label is not
displayable or does not fit. At small zoom levels endpoint text is omitted;
zoom in and use the cursor/legend to identify it.

## Verification

```powershell
dotnet build Tangles.slnx -c Release --disable-build-servers
python QL/GUI/test_player.py
python QL/GUI/build.py --turboptr QL/GUI/build/latest/tptr_ext --check
python QL/tools/test_json.py --compile
python QL/tools/test_packs.py --compile
```

`test_player.py` obtains reference traces from the actual shared F# engine,
then compiles and runs the QL engine under SMSQ/E and compares every accepted
action, active path, cell/order map, undo depth, connection count and completion
flag. It also exercises save/reload, interrupted-save fallback, rejected malformed
metadata, escaped required names and every clue in the 100-puzzle pack.

`GuiCheck.java` drives the compiled executable using SMSQmulator's emulated
mouse/keyboard linkage and checks rendered pixels, rather than calling BASIC
game routines directly. Its isolated test folder and screenshots are recorded
in `build/latest/smoke.json`. Other display backends and overlapping QL jobs
still require their own integration checks.

The renderer probe observes every native block fill during a short partial
drag, checks that an endpoint is never temporarily erased and an unrelated
legend entry is never repainted, and compares incremental images with a complete
redraw after extensions, corners, undo/reset, retracing and a 16x16 stroke.
`build/latest/render-metrics.json` records the result. On the same 3x3 test case,
the previous cached renderer used 34 block fills covering 216,469 pixels; the
incremental renderer uses 3 fills covering 600 pixels. This measures screen I/O,
not a guaranteed frame rate on every QL or emulator.
