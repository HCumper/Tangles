# JSON for QL Structured SuperBASIC

`json.ssb` is a JSON reader written in SSB using the **SSB 2.8.0 record facility**.
It translates to ordinary S*BASIC for interpreted SMSQ/E SBASIC or Turbo 5.09
compilation. This is QL code, not an F#/.NET parser.

`pack.ssb` is the application loading layer, similar to `PuzzlePacks.read` in
F#: it maps the temporary JSON tree into named puzzle/pair/cell records, validates
the clues, then frees the tree. `pack_example.ssb` uses only those records to
display puzzle IDs, board sizes and endpoint pairs. It does not implement the
game or certify solvability, a stored solution, or uniqueness. The playable
TurboPTR interface in [`QL/GUI`](../GUI/README.md) uses these records and has its
own rule engine, checked against the shared F# player.

## Files and prerequisites

- `json.ssb`: reusable parser and accessors.
- `pack.ssb`: Tangles v1 schema-specific loader and game records.
- `pack_example.ssb`: example for `src/Tangles.Player/wwwroot/packs/starter.json`.
- `build_example.ssb`: Turbo build wrapper with the tested compiler settings.
- `tools/test_json.py`: host-side fixture creation and real-emulator tests.
- `tools/test_packs.py`: independent checks of loaded game records and invalid packs.
- SSB **2.8.0**, including records and the MANIFEST-line preservation fix;
  Toolkit II (`FOP_IN`, `FLEN`); Turbo Toolkit
  when compiling or interpreting the source's `MANIFEST` declarations.
  The compiled parser needs Toolkit II, but no manifest runtime support.
  The library itself does not need BGET, machine-code helpers,
  a record runtime extension, a network connection or JSON preprocessing on a PC.

Copy the sources to QL filenames `json_ssb`, `pack_ssb` and `pack_example_ssb`,
keeping LF line endings. Copy the JSON unchanged as `starter_json`. Set the example's data
filename and include/work prefix to the mounted QL device. The supplied example
uses `nfa1_` for the data file; that is a host-folder drive in SMSQmulator.

For example, with the sources and `ssb280_exe` available on `nfa1_`/`sfa1_`:

```basic
SETENV "SSBWORK=nfa1_"
SETENV "SSBWORK2=ram1_"
SETENV "SSBRECWORK=ram1_"
SETENV "SSBOVER=Y"
DEFAULT_DEVICE "sfa1_"
EXECUTE_W "sfa1_ssb280_exe";"pack_example"
LRUN "ram1_pack_example_bas"
```

SSB prefixes `#INCLUDE json_ssb` with `SSBWORK`; do not repeat the device in both.
For Turbo, also copy `build_example.ssb` as `build_example_ssb`, preprocess
`build_example`, then `LOAD "ram1_build_example_bas"` and `CHARGE` it. The wrapper
uses the tested wide-code model, brief optimization (`TURBO_optim 0`), a 64 KB
compiler buffer and 256 KB application dataspace. A 2 KB compiler buffer caused
a code-generation phase failure; Fast optimization produced an invalid runtime
in this local Turbo build. Use the supplied settings for this library.
The original `starter.json` is 46,091 bytes and produces 1,784 value nodes. It is
larger than one QL string. An expanded QL is needed for this pack and the example's
4,096-node capacity; this is not a claim that the full pack fits a stock 128 KB QL.
Smaller documents can use smaller capacities.

## Puzzle-pack loading API

Applications include `pack_ssb` (which includes `json_ssb`) and use named records:

```basic
#INCLUDE pack_ssb
pack_load "nfa1_starter_json",4096
IF packDoc.error% THEN PRINT packDoc.message$ : STOP
PRINT packDoc.puzzleCount%
PRINT packPuzzles(1).id$
PRINT packPuzzles(1).width%;" by ";packPuzzles(1).height%
firstPair% = packPuzzles(1).firstPair%
PRINT packPairs(firstPair%).label$
PRINT packPairs(firstPair%).start.x%;",";packPairs(firstPair%).start.y%
PRINT packPairs(firstPair%).finish.x%;",";packPairs(firstPair%).finish.y%
PRINT "Full coverage: ";packPuzzles(1).requireFullCoverage%
pack_free
STOP
```

| Operation/storage | Meaning |
|---|---|
| `pack_load file$, nodeCapacity%` | Load and validate a generator v1 pack. |
| `pack_load_player file$, nodeCapacity%` | Player import: up to 5 MiB; retain clues and rules only while validating all JSON bytes. |
| `pack_parse text$, nodeCapacity%` | Same operation for JSON already in a QL string. |
| `pack_free` | Release the game records and clear pack diagnostics. |
| `packDoc.error%`, `.message$`, `.errorPosition` | First failure and source byte position. Zero error means success. |
| `packDoc.jsonError%` | Underlying JSON error class for `PACK_ERR_JSON%`; otherwise zero. |
| `packDoc.formatVersion%`, `.requireFullCoverage%` | Validated format/rule settings. |
| `packDoc.puzzleCount%`, `.pairCount%` | Number of published puzzles and total pairs. |
| `packPuzzles(1..puzzleCount%)` | `TanglesPuzzle` records with `id$`, `width%`, `height%`, `requireFullCoverage%`, `firstPair%`, `pairCount%`, and real-valued `sourceStart`/`sourceLength` byte spans. |
| `packPairs(1..pairCount%)` | `TanglesPair` records with `label$`, nested `start.x%`/`start.y%` and `finish.x%`/`finish.y%`. |

Each puzzle's pairs occupy the contiguous range starting at `firstPair%`, of
length `pairCount%`. These are indices of game records, not JSON node handles.
The JSON source, tree, frame stack and temporary endpoint occupancy map are
released before either public loading routine returns. Game records remain
usable after independent subsequent JSON parsing.

One pack is held globally. A new load invalidates the previous model; any failure
releases partial records and leaves both counts zero. Check `packDoc.error%`
before accessing arrays. A caller retaining an old board across a failed import
must copy its game records before loading. Reserve the `pack_` prefix and
`packDoc`, `packPuzzles`, `packPairs`, `packOccupied%`, `packContext$`, `packAt`.
`pack_free` leaves an independently loaded JSON document alone.

The loader checks required fields and exact value types; format version 1;
the three required true geometry flags; Boolean `requireFullCoverage`; 1..10000
puzzles; integer board dimensions 1..64; at least one pair per puzzle; nonblank
IDs/labels; distinct labels; integer endpoints inside the board; distinct,
unshared endpoint cells; and conflicting reuse of puzzle IDs. Identical boards
with the same ID are allowed. Pair order and endpoint direction form part of
that identity, as in the F# records. Fields are case-sensitive, can be reordered
or escaped, and the last duplicate member wins, matching `JsonElement` lookup.
Integer fields reject fractions and exponent notation without floating-point
rounding. Both coverage modes are retained; loading clues does not prove either
mode solvable. Unknown metadata, stored solutions and verification claims are
ignored after JSON syntax validation.

QL limits are **1 MiB of input, 32 container levels**, the supplied JSON node
capacity (maximum 32766), and **128 UTF-8 bytes** per ID/label. F# allows larger
packs and counts its text limit in .NET characters; the byte limit here avoids
silently truncating QL record strings. Decoded strings/keys also have the generic
parser's 4096-byte accessor limit. The pair-record count is bounded at 32766.
Unexpected operating-system errors and insufficient physical memory remain BASIC
runtime errors; node capacity alone does not guarantee RAM is available.

`pack_load_player` is an opt-in alternative for larger generator packs. It raises
the file limit to **5 MiB of bytes**, retaining only required rule/clue fields in
the temporary tree. Unknown metadata and stored solutions still pass the full
JSON grammar, string/Unicode, number and nesting checks; their descendants use
scratch node zero and stack frames instead of consuming value handles. Only
this entry point enables the projection, and it resets the mode before returning.
The original generic parser and `pack_load`/`pack_parse` contracts are unchanged.
Required-name escapes, duplicate last-member behaviour and schema validation
are preserved. The 32766 retained-node/pair limits still apply. The GUI uses a
larger Turbo dataspace and 16 MiB emulator configuration for these imports.

The GUI's separate `hints.ssb` layer can reopen the source file and seek to
`sourceStart-1` to read `sourceLength` bytes. These are one-based byte spans of
the original puzzle object, not tree handles; they remain valid only for that
unchanged file. The real-valued fields support positions above 32767. An opt-in
`jsonHintOnly%` projection retains only the selected object's optional solution;
the hint layer validates it independently and frees the temporary tree. Neither
normal pack loading nor the generic parser starts retaining witnesses.

Pack failures use named Turbo MANIFEST constants: `PACK_ERR_JSON%` (1),
`PACK_ERR_SCHEMA%` (2), `PACK_ERR_FORMAT%` (3), `PACK_ERR_RULES%` (4),
`PACK_ERR_CLUES%` (5), `PACK_ERR_LIMIT%` (6). Parser diagnostics are copied before
the tree is freed; schema errors identify the puzzle/pair and field where possible.

## Generic JSON API

```basic
#INCLUDE json_ssb
json_load "nfa1_starter_json",4096,JSON_MAX_BYTES,32
IF jsonDoc.error% THEN
  PRINT jsonDoc.message$;" at byte ";jsonDoc.errorPosition
  STOP
END IF
root% = jsonDoc.root%
puzzles% = json_member%(root%,"puzzles")
IF puzzles% = 0 THEN PRINT "No puzzles member" : STOP
firstPuzzle% = json_at%(puzzles%,0)
idNode% = json_member%(firstPuzzle%,"id")
PRINT json_string$(idNode%)
STOP
```

All public routines operate on **one current document**. A subsequent load/parse
invalidates every previous handle and span. The global records/arrays are named
`jsonDoc`, `jsonNodes`, `jsonFrames`; input bytes live in `jsonChunks$`.
Reserve the `json_` prefix and these storage names in a calling program.

| Operation | Behaviour |
|---|---|
| `json_load file$, capacity%, maxBytes, maxDepth%` | Read a file and parse it. Replaces the current document. |
| `json_parse text$, capacity%, maxDepth%` | Parse an existing QL string. Same tree/error semantics. |
| `json_clear` | Clear accessor errors 8/9 without replacing the document; leave errors 1..7 set. |
| `json_free` | Release the document arrays back to their placeholders and invalidate all handles/spans. |
| `jsonDoc.root%` | Root handle on success, zero on every failed load/parse. |
| `jsonDoc.used%`, `.bytes` | Allocated value count and input byte count. |
| `json_member%(object%, name$)` | First exact, case-sensitive decoded key match; zero if absent. |
| `json_at%(array%, index%)` | Zero-based element lookup; zero if index is outside the array. |
| `json_name$(handle%)` | Decoded name of an object member; error 8 for a node that is not an object member. |
| `jsonNodes(handle%).kind%` | Value type; see table below. |
| `.first%`, `.next%`, `.count%` | First child, next sibling and number of direct children. Zero terminates a chain. |
| `json_string$(handle%)` | Decode a string to UTF-8 bytes, including escapes and surrogate pairs. |
| `json_number(handle%)` | Convert a JSON number to QL floating point. |
| `json_boolean%(handle%)` | True = 1; false = 0. |
| `json_raw$(handle%)` | Original JSON bytes for the complete value, including quotes/brackets. |

Node kinds have named Turbo `MANIFEST` constants:

| Constant | Value | Meaning |
|---|---|---|
| `JSON_KIND_OBJECT%` | 1 | Object |
| `JSON_KIND_ARRAY%` | 2 | Array |
| `JSON_KIND_STRING%` | 3 | String |
| `JSON_KIND_NUMBER%` | 4 | Number |
| `JSON_KIND_TRUE%` | 5 | True |
| `JSON_KIND_FALSE%` | 6 | False |
| `JSON_KIND_NULL%` | 7 | Null |

For example, compare `jsonNodes(handle%).kind% = JSON_KIND_ARRAY%`.
Null is a real node of kind 7; it is distinct from a missing member (handle 0).
Objects preserve member order and duplicates; lookup returns the first match.
Traverse siblings directly for large arrays rather than repeatedly calling
`json_at%`, whose lookup time is linear in the requested index.

Node `start`/`length` are the original value's byte span (one-based start).
`keyStart`/`keyLength` cover the member name **without** its quotes, retaining
JSON escapes in the source. Only children of objects have a key span.
Source spans and file positions are real-valued integer quantities so offsets
beyond 32,767 are supported. Do not copy seeds/file positions into `%` variables:
QL `%` integers are signed 16-bit. The example keeps dimensions/coordinates as
real values too; applications should check integer/range constraints separately.

## Representation, limits and errors

The parser is iterative. A record-array stack tracks containers, so malicious
nesting does not recurse through BASIC procedures. Nodes retain source spans;
there is no large fixed string field in every node. Input is split into 512-byte
strings and only as much file storage as needed is allocated. File input uses
channel-based ROM `INKEY$` and closes its channel after reading. No fixed channel
number is taken from the caller. Files are opened for input only.

The limits are explicit:

- At most **1,048,576 input bytes**, with a caller-specified smaller file limit.
- **1..32,766 nodes** and **1..256 container levels**, chosen by the caller.
- `json_string$`, `json_decode$` and `json_raw$` return at most **4,096 bytes**.
  Larger tokens can be parsed and inspected via source spans, but these getters
  report an error instead of silently truncating them.
- Number conversion accepts at most 128 source characters and an absolute explicit
  exponent at most 128. More extreme numbers remain valid JSON tokens and retain
  their exact original spelling; conversion reports a limit error. Converted
  values have QL floating-point precision, not arbitrary decimal precision.
- Capacity does not guarantee physical RAM is available: an oversized DIM or
  an operating-system I/O failure can still raise a normal BASIC runtime error.

The declarations at the top of `json.ssb` also name grammar phases, character
codes, UTF-8/UTF-16 boundaries and limits such as `JSON_CHUNK_SIZE%`,
`JSON_MAX_BYTES`, `JSON_MAX_NODES%`, `JSON_MAX_DEPTH%`, `JSON_MAX_TEXT%`,
`JSON_NUMBER_CHARS%` and `JSON_MAX_EXPONENT%`. Turbo substitutes the literal
values in compiled code. With Turbo Toolkit loaded, the interpreted assignments
provide the same values. Treat every `JSON_*` name as read-only. Constants whose
values exceed 32,767 have no `%` suffix, preserving QL real arithmetic for input
sizes and Unicode. The numeric kind/error API and all limits are unchanged.

Each `MANIFEST` and its assignments must remain on one physical BASIC line for
Turbo to recognise the constants. Use the local SSB build with the 17 November
2026 preservation fix; older record-expansion builds split those lines. The
emulator harness verifies all 95 declarations survive translation intact.

`jsonDoc.error%` is zero on success. The first error is retained with
`message$`, `errorPosition`, `errorLine` and `errorColumn`. Positions and columns
count input bytes, not Unicode display cells; CR, LF and CRLF are handled.
After an accessor error (8 or 9), call `json_clear` to clear the error and its
diagnostics while retaining the parsed tree and valid handles. Errors 1..7 cannot
be cleared this way: load/parse again to replace the failed document.

Call `json_free` when finished to reset the document and shrink its arrays back to
their placeholders. All handles/spans then become invalid. Otherwise, arrays keep
their largest allocated bounds and are reused by subsequent loads/parses.

| Constant | Code | Meaning |
|---|---|---|
| `JSON_ERR_CONFIG%` | 1 | Invalid configuration or file byte limit exceeded |
| `JSON_ERR_FILE%` | 2 | File open failure or short read |
| `JSON_ERR_SYNTAX%` | 3 | JSON structure/literal/trailing input error |
| `JSON_ERR_STRING%` | 4 | String escape, Unicode or UTF-8 error |
| `JSON_ERR_NUMBER%` | 5 | Number syntax error |
| `JSON_ERR_NODES%` | 6 | Node capacity exhausted |
| `JSON_ERR_DEPTH%` | 7 | Nesting limit exceeded |
| `JSON_ERR_ACCESSOR%` | 8 | Wrong accessor type or invalid node handle |
| `JSON_ERR_LIMIT%` | 9 | Getter/conversion size or range limit |

All JSON value types are supported. Whitespace is restricted to JSON's four
ASCII whitespace bytes. Comments, single-quoted strings, trailing commas, invalid
number syntax and trailing documents are rejected. An initial UTF-8 BOM is
accepted. Strings require valid UTF-8 scalar values; `\u` escapes require valid
surrogate pairs (isolated surrogates are rejected). Decoding produces UTF-8,
including embedded NUL where requested; it does not map Unicode to the QL display
character set. Tangles' ASCII IDs, labels and property names need no conversion.

Do not pass individual `jsonNodes(index%)` records by reference when compiling
with Turbo. Pass their handles and access the global array, as this library does.
No record-returning functions or toolkit changes are required.

## Tests

With the existing local SSB build and emulator ROM installed:

```powershell
python C:\Source\Tangles\QL\tools\test_json.py --ssb-root "C:\Source\Structured Super Basic" --packs C:\Source\Tangles\src\Tangles.Player\wwwroot\packs
python C:\Source\Tangles\QL\tools\test_json.py --ssb-root "C:\Source\Structured Super Basic" --packs C:\Source\Tangles\src\Tangles.Player\wwwroot\packs --compile
python C:\Source\Tangles\QL\tools\test_packs.py
python C:\Source\Tangles\QL\tools\test_packs.py --compile
```

The host creates fixtures, invokes the **real QL SSB translator**, and runs its
output in SMSQ/E on SMSQmulator's 68000 CPU, optionally compiling the target with
Turbo. It compares complete trees against Python's independent JSON decoder,
including all source spans, names, strings, exact number spellings, sibling links
and child counts. Both actual Tangles packs are read unchanged. The harness also
checks malformed input, Unicode, chunk boundaries, duplicate/case-sensitive keys,
large seeds, capacity/depth/string limits, missing files and accessor errors.
No emulator/compiler/ROM binaries are redistributed here.

The checked-in `validation.json` records successful interpreted and Turbo-compiled
60-case runs, complete comparison of both packs (66 and 1,784 nodes), and the
compiled example's eight IDs, dimensions and 48 endpoint pairs. Translation and
compilation used an 8 MB SMSQ/E emulator. The resulting compiled parser also
passed the full suite in a fresh **2 MB** SMSQ/E emulator without running SSB or
Turbo there. This is runtime validation, not a claim that SSB can translate this
library with 2 MB. Classic QDOS ROMs and a stock 128 KB QL were not tested.

`test_json.py --example --compile` builds and verifies the converted record-based
example against all eight starter puzzle IDs and 48 endpoint pairs. The pack
suite's 113 cases separately check every loaded record against Python's JSON decoder,
including both supplied packs, both coverage flags, invalid schema/clues,
Unicode, duplicate IDs/members, resource limits, reloads and tree disposal.
Use `--work <new-folder>` to retain a build; local output under this folder's
`build/` is ignored by Git. A compiled pack suite can be run without
translating/compiling again using `test_packs.py --runtime-only <compiled-folder>
--ram-mib 4 --work <new-folder>`. The suite reserves 1 MiB dataspace to exercise
its 12000-node limit fixture. That reservation prevents the full suite launching
in the tested 2 MiB emulator (SMSQ/E reports insufficient memory). The example
wrapper retains its 256 KiB dataspace and successfully loads all eight starter
puzzles and 48 pairs in a fresh 2 MiB emulator without translating or compiling.
`pack-validation.json` records loading-layer verification separately from the
generic parser's existing `validation.json`.
