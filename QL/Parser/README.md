# JSON for QL Structured SuperBASIC

`json.ssb` is a JSON reader written in SSB using the **SSB 2.8.0 record facility**.
It translates to ordinary S*BASIC and is intended for interpreted QDOS/SMSQ/E
programs or Turbo 5.09 compilation. This is QL code, not an F#/.NET parser.

`pack_example.ssb` reads the Tangles generator's JSON format and displays all
puzzle IDs, board sizes and endpoint pairs. It does not implement the game,
validate all game rules, or certify a stored solution/uniqueness claim.

## Files and prerequisites

- `json.ssb`: reusable parser and accessors.
- `pack_example.ssb`: example for `src/Tangles.Player/wwwroot/packs/starter.json`.
- `build_example.ssb`: Turbo build wrapper with the tested compiler settings.
- `tools/test_json.py`: host-side fixture creation and real-emulator tests.
- SSB **2.8.0**, including records; Toolkit II (`FOP_IN`, `FLEN`); Turbo Toolkit
  when compiling. The library itself does not need BGET, machine-code helpers,
  a record runtime extension, a network connection or JSON preprocessing on a PC.

Copy the sources to QL filenames `json_ssb` and `pack_example_ssb`, keeping LF
line endings. Copy the JSON unchanged as `starter_json`. Set the example's data
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

## Basic API

```basic
#INCLUDE json_ssb
json_load "nfa1_starter_json",4096,1048576,32
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

Kinds: **1 object, 2 array, 3 string, 4 number, 5 true, 6 false, 7 null**.
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

`jsonDoc.error%` is zero on success. The first error is retained with
`message$`, `errorPosition`, `errorLine` and `errorColumn`. Positions and columns
count input bytes, not Unicode display cells; CR, LF and CRLF are handled.
After an accessor error (8 or 9), call `json_clear` to clear the error and its
diagnostics while retaining the parsed tree and valid handles. Errors 1..7 cannot
be cleared this way: load/parse again to replace the failed document.

Call `json_free` when finished to reset the document and shrink its arrays back to
their placeholders. All handles/spans then become invalid. Otherwise, arrays keep
their largest allocated bounds and are reused by subsequent loads/parses.

| Code | Meaning |
|---|---|
| 1 | Invalid configuration or file byte limit exceeded |
| 2 | File open failure or short read |
| 3 | JSON structure/literal/trailing input error |
| 4 | String escape, Unicode or UTF-8 error |
| 5 | Number syntax error |
| 6 | Node capacity exhausted |
| 7 | Nesting limit exceeded |
| 8 | Wrong accessor type or invalid node handle |
| 9 | Getter/conversion size or range limit |

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
