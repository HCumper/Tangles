"""Check the SSB loading layer on the real SMSQ/E CPU, optionally with Turbo.

Compares named records with independently decoded JSON. External toolchain/ROM
files are copied into an isolated work directory; no user drives are mounted.
"""
import argparse
import copy
import datetime
import hashlib
import json
import os
from pathlib import Path
import re
import shutil
import subprocess
import time

ROOT = Path(__file__).resolve().parents[1]
PARSER = ROOT / "Parser"


def sample():
    return {
        "formatVersion": 1,
        "rules": {"orthogonalOnly": True, "noSharedCells": True,
                  "noSelfTouching": True, "requireFullCoverage": False},
        "puzzles": [{"id": "test-board", "width": 3, "height": 3, "pairs": [
            {"label": "A", "start": {"x": 0, "y": 0}, "finish": {"x": 1, "y": 0}},
            {"label": "B", "start": {"x": 0, "y": 2}, "finish": {"x": 2, "y": 2}},
        ]}],
    }


def fixtures(packs):
    cases = []

    def add(name, value, error=0, capacity=4096, json_error=0):
        raw = value if isinstance(value, bytes) else json.dumps(value, ensure_ascii=True).encode()
        cases.append(dict(name=name, raw=raw, error=error, capacity=capacity, json_error=json_error))

    def change(name, path, value=None, error=2, remove=False):
        doc = sample()
        owner = doc
        for key in path[:-1]:
            owner = owner[key]
        if remove:
            del owner[path[-1]]
        else:
            owner[path[-1]] = value
        add(name, doc, error)

    add("connect_only", sample())
    change("full_coverage", ["rules", "requireFullCoverage"], True, 0)
    add("syntax", b'{"formatVersion":', 1, json_error=3)
    add("root_array", [], 2)
    for field in ["formatVersion", "rules", "puzzles"]:
        change("missing_" + field, [field], remove=True)
    change("unsupported_version", ["formatVersion"], 2, 3)
    change("version_string", ["formatVersion"], "1")
    change("rules_null", ["rules"], None)
    for field in ["orthogonalOnly", "noSharedCells", "noSelfTouching"]:
        change("missing_" + field, ["rules", field], remove=True)
        change("false_" + field, ["rules", field], False, 4)
        change("type_" + field, ["rules", field], 1)
    change("missing_coverage", ["rules", "requireFullCoverage"], remove=True)
    change("coverage_string", ["rules", "requireFullCoverage"], "false")
    change("puzzles_object", ["puzzles"], {})
    change("empty_pack", ["puzzles"], [], 6)
    add("too_many_puzzles", {**sample(), "puzzles": [None] * 10001}, 6, 12000)
    change("null_puzzle", ["puzzles", 0], None)
    puzzle = ["puzzles", 0]
    pair = puzzle + ["pairs", 0]
    for field in ["id", "width", "height", "pairs"]:
        change("missing_" + field, puzzle + [field], remove=True)
    for name, value in [("null", None), ("number", 10), ("empty", ""),
                        ("space", " \t\n"), ("unicode_space", "\u2003\u00a0")]:
        change("id_" + name, puzzle + ["id"], value)
    change("id_over_limit", puzzle + ["id"], "x" * 129, 6)
    for name, value in [("string", "3"), ("null", None), ("boolean", True),
                        ("fraction", 3.5), ("integral_float", 3.0),
                        ("zero", 0), ("negative", -1), ("large", 65),
                        ("huge", 10**100)]:
        change("width_" + name, puzzle + ["width"], value)
    change("height_zero", puzzle + ["height"], 0)
    change("height_large", puzzle + ["height"], 65)
    change("pairs_object", puzzle + ["pairs"], {})
    change("no_pairs", puzzle + ["pairs"], [], 5)
    change("too_many_pairs", puzzle + ["pairs"], sample()["puzzles"][0]["pairs"] * 3, 5)
    change("null_pair", pair, None)
    for field in ["label", "start", "finish"]:
        change("missing_" + field, pair + [field], remove=True)
    for name, value in [("empty", ""), ("space", " \r\n"), ("null", None), ("number", 1)]:
        change("label_" + name, pair + ["label"], value)
    change("label_over_limit", pair + ["label"], "x" * 129, 6)
    doc = sample()
    doc["puzzles"][0]["pairs"][0]["label"] = "x" * 4097
    add("label_getter_limit", doc, 1, json_error=9)
    change("duplicate_label", puzzle + ["pairs", 1, "label"], "A", 5)
    change("case_sensitive_labels", puzzle + ["pairs", 1, "label"], "a", 0)
    change("unicode_label", pair + ["label"], "\u00e9\U0001f600", 0)
    change("max_utf8_text", pair + ["label"], "\u00e9" * 64, 0)
    change("utf8_text_over_limit", pair + ["label"], "\u00e9" * 65, 6)
    change("embedded_nul", pair + ["label"], "A\0B", 0)
    for point in ["start", "finish"]:
        change(point + "_array", pair + [point], [])
        for axis in ["x", "y"]:
            change(point + "_missing_" + axis, pair + [point, axis], remove=True)
            change(point + "_negative_" + axis, pair + [point, axis], -1)
            change(point + "_outside_" + axis, pair + [point, axis], 3)
    change("coordinate_string", pair + ["start", "x"], "0")
    change("coordinate_fraction", pair + ["start", "x"], 0.5)
    change("same_endpoints", pair + ["finish"], {"x": 0, "y": 0}, 5)
    change("shared_endpoints", puzzle + ["pairs", 1, "start"], {"x": 1, "y": 0}, 5)
    doc = sample()
    doc["puzzles"].append(copy.deepcopy(doc["puzzles"][0]))
    add("identical_id", doc)
    doc = copy.deepcopy(doc)
    doc["puzzles"][1]["width"] = 4
    add("conflicting_id", doc, 5)
    doc["puzzles"][1]["id"] = "TEST-BOARD"
    add("case_sensitive_ids", doc)
    for name, path, value in [
        ("height", ["height"], 4),
        ("label", ["pairs", 0, "label"], "C"),
        ("start_x", ["pairs", 0, "start", "x"], 2),
        ("start_y", ["pairs", 0, "start", "y"], 1),
        ("finish_x", ["pairs", 0, "finish", "x"], 2),
        ("finish_y", ["pairs", 0, "finish", "y"], 1),
        ("pair_count", ["pairs"], sample()["puzzles"][0]["pairs"][:1]),
        ("pair_order", ["pairs"], sample()["puzzles"][0]["pairs"][::-1]),
    ]:
        doc = sample()
        other = copy.deepcopy(doc["puzzles"][0])
        owner = other
        for key in path[:-1]:
            owner = owner[key]
        owner[path[-1]] = value
        doc["puzzles"].append(other)
        add("conflicting_id_" + name, doc, 5)
    doc = sample()
    doc["puzzles"][0].update(width=64, height=64)
    doc["puzzles"][0]["pairs"][0]["finish"] = {"x": 63, "y": 63}
    add("max_board", doc)
    doc = sample()
    doc["puzzles"][0].update(width=1, height=2, pairs=[
        {"label": "A", "start": {"x": 0, "y": 0}, "finish": {"x": 0, "y": 1}}])
    add("one_column", doc)
    doc = sample()
    doc["generation"] = {"seed": 1555655117}
    doc["verification"] = "not trusted"
    doc["puzzles"][0]["solution"] = "not a playable route"
    add("ignore_non_clues", doc)
    doc["puzzles"][0]["solution"] = "x" * 4097
    add("ignore_large_solution_text", doc)
    raw = json.dumps(sample()).encode()
    add("last_duplicate_valid", raw.replace(b'"width": 3', b'"width": 99, "width": 3'))
    add("last_duplicate_invalid", raw.replace(b'"width": 3', b'"width": 3, "width": 99'), 2)
    add("escaped_field", raw.replace(b'"width"', b'"w\\u0069dth"'))
    add("negative_zero", raw.replace(b'"x": 0', b'"x": -0'))
    add("exponent", raw.replace(b'"width": 3', b'"width": 3e0'), 2)
    add("huge_exponent", raw.replace(b'"width": 3', b'"width": 1e999999'), 2)
    add("byte_limit", b" " * (1048576 + 1), 1, json_error=1)
    add("depth_limit", raw[:-1] + b',"extra":' + b"[" * 33 + b"0" + b"]" * 33 + b"}", 1, json_error=7)
    add("node_limit", sample(), 1, capacity=4, json_error=6)
    add("bad_capacity", sample(), 1, capacity=0, json_error=1)
    for path in sorted(packs.glob("*.json")):
        add("pack_" + path.stem, path.read_bytes())
    add("reload_after_errors", sample())
    return cases


def driver(cases):
    lines = ['#INCLUDE pack_ssb', 'DIM testString$(128)', 'OPEN_NEW #9,"nfa1_result"']
    for index, case in enumerate(cases):
        lines += [f'pack_load "nfa1_f{index:03d}",{case["capacity"]}',
                  f'test_case {index}']
    memory = json.dumps(sample(), separators=(",", ":"))
    lines += [f"pack_parse '{memory}',4096", 'test_case -1',
              'pack_load "nfa1_missing",4096', 'test_case -2',
              'json_parse "true",8,4', 'pack_free',
              'PRINT #9,"FREE|";packDoc.error%;"|";packDoc.puzzleCount%;"|";packDoc.pairCount%;"|";jsonDoc.used%',
              'json_free', 'CLOSE #9',
              'OPEN_NEW #9,"nfa1_done":PRINT #9,"OK":CLOSE #9', 'STOP',
              'DEFine PROCedure test_case(VAL tcCase%)',
              '  LOCal tcPuzzle%,tcPair%,tcSlot%',
              '  PRINT #9,"C|";tcCase%;"|";packDoc.error%;"|";packDoc.jsonError%;"|";packDoc.errorPosition;"|";packDoc.puzzleCount%;"|";packDoc.pairCount%;"|";packDoc.formatVersion%;"|";packDoc.requireFullCoverage%;"|";jsonDoc.used%;"|";jsonArena%',
              '  IF packDoc.error% THEN',
              '    PRINT #9,"E|";tcCase%;"|";packDoc.message$',
              '    RETurn', '  END IF',
              '  ## Prove that no fields depend on the now-discarded JSON tree.',
              '  json_parse "null",8,4', '  json_free',
              '  FOR tcPuzzle% = 1 TO packDoc.puzzleCount%',
              '    PRINT #9,"P|";tcCase%;"|";tcPuzzle%;"|";test_hex$(packPuzzles(tcPuzzle%).id$);"|";packPuzzles(tcPuzzle%).width%;"|";packPuzzles(tcPuzzle%).height%;"|";packPuzzles(tcPuzzle%).requireFullCoverage%;"|";packPuzzles(tcPuzzle%).firstPair%;"|";packPuzzles(tcPuzzle%).pairCount%',
              '    FOR tcPair% = 0 TO packPuzzles(tcPuzzle%).pairCount%-1',
              '      tcSlot% = packPuzzles(tcPuzzle%).firstPair%+tcPair%',
              '      PRINT #9,"R|";tcCase%;"|";tcSlot%;"|";test_hex$(packPairs(tcSlot%).label$);"|";packPairs(tcSlot%).start.x%;"|";packPairs(tcSlot%).start.y%;"|";packPairs(tcSlot%).finish.x%;"|";packPairs(tcSlot%).finish.y%',
              '    END FOR tcPair%', '  END FOR tcPuzzle%', 'END DEFine',
              'DEFine FuNction test_hex$(VAL thText$)',
              '  LOCal thIndex%,thResult$(256)', '  thResult$ = ""',
              '  FOR thIndex% = 1 TO LEN(thText$)',
              '    thResult$ = thResult$ & HEX$(CODE(thText$(thIndex%)),8)',
              '  END FOR thIndex%', '  RETurn thResult$', 'END DEFine']
    return "\n".join(lines) + "\n"


def verify(folder, cases):
    errors = []
    rows = [line.split("|") for line in (folder / "result").read_text("latin1").splitlines()]
    expected = {i: c for i, c in enumerate(cases)}
    expected[-1] = dict(raw=json.dumps(sample()).encode(), error=0, json_error=0)
    expected[-2] = dict(error=1, json_error=2)
    seen = set()
    for row in rows:
        if row[0] != "C":
            continue
        index = int(row[1])
        case = expected[index]
        name = case.get("name", str(index))
        seen.add(index)
        actual = [int(value) for value in row[2:]]
        error, json_error, position, count, pairs, version, coverage, used, arena = actual
        if (error, json_error) != (case["error"], case["json_error"]):
            errors.append(f"{name}: error {(error, json_error)} != {(case['error'], case['json_error'])}")
        if used or arena:
            errors.append(f"{name}: JSON storage was not released")
        if case["error"]:
            if count or pairs or version or coverage:
                errors.append(f"{name}: failure exposed a partial model")
            if position < 1 or not any(r[0] == "E" and int(r[1]) == index and r[2].strip() for r in rows):
                errors.append(f"{name}: missing diagnostic")
            continue
        doc = json.loads(case["raw"])
        full = int(doc["rules"]["requireFullCoverage"])
        total = sum(len(p["pairs"]) for p in doc["puzzles"])
        if (count, pairs, version, coverage) != (len(doc["puzzles"]), total, 1, full):
            errors.append(f"{name}: incorrect pack fields")
        want_puzzles, want_pairs, slot = [], [], 1
        for pi, puzzle in enumerate(doc["puzzles"], 1):
            want_puzzles.append([str(pi), puzzle["id"].encode().hex().upper(), str(puzzle["width"]),
                                 str(puzzle["height"]), str(full), str(slot), str(len(puzzle["pairs"]))])
            for pair in puzzle["pairs"]:
                want_pairs.append([str(slot), pair["label"].encode().hex().upper(),
                                   str(pair["start"]["x"]), str(pair["start"]["y"]),
                                   str(pair["finish"]["x"]), str(pair["finish"]["y"])])
                slot += 1
        for kind, want in [("P", want_puzzles), ("R", want_pairs)]:
            got = [[v.strip() for v in r[2:]] for r in rows if r[0] == kind and int(r[1]) == index]
            if got != want:
                errors.append(f"{name}: {kind} records differ from independently decoded clues")
    if seen != set(expected):
        errors.append(f"Missing cases: {set(expected) - seen}")
    if not any([v.strip() for v in r] == ["FREE", "0", "0", "0", "1"] for r in rows):
        errors.append("pack_free did not clear the model or altered an independent JSON document")
    return errors


def main():
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("--ssb-root", type=Path, default=Path("C:/Source/Structured Super Basic"))
    parser.add_argument("--packs", type=Path, default=ROOT.parent / "src/Tangles.Player/wwwroot/packs")
    parser.add_argument("--work", type=Path)
    parser.add_argument("--compile", action="store_true")
    parser.add_argument("--runtime-only", type=Path, help="Run a previously compiled suite in a fresh emulator")
    parser.add_argument("--ram-mib", type=int, default=8)
    parser.add_argument("--timeout", type=int, default=180)
    args = parser.parse_args()
    folder = (args.work or Path(os.environ["TEMP"]) / "tangles-pack-tests" / str(time.time_ns())).resolve()
    folder.mkdir(parents=True)
    cases = fixtures(args.packs)
    loader_hash = hashlib.sha256((PARSER / "pack.ssb").read_bytes()).hexdigest()
    parser_hash = hashlib.sha256((PARSER / "json.ssb").read_bytes()).hexdigest()
    suite_hash = hashlib.sha256(json.dumps([
        {**{key: value for key, value in case.items() if key != "raw"},
         "rawSha256": hashlib.sha256(case["raw"]).hexdigest()} for case in cases
    ], sort_keys=True).encode()).hexdigest()
    driver_hash = hashlib.sha256(driver(cases).encode()).hexdigest()
    if args.runtime_only:
        previous = json.loads((args.runtime_only / "verification.json").read_text())
        if not previous["passed"] or not previous["compiled"] or any([
            previous.get("loaderSha256") != loader_hash,
            previous.get("parserSha256") != parser_hash,
            previous.get("suiteSha256") != suite_hash,
            previous.get("driverSha256") != driver_hash,
        ]):
            parser.error("Runtime-only target must be a successful build of the current sources and fixtures")
    for name in ["ssb280_exe", "parser_task", "codegen_task", "ttk", "env_bin"]:
        shutil.copy2(args.ssb_root / "build/compiled-app" / name, folder / name)
    for name in ["json", "pack"]:
        shutil.copy2(PARSER / (name + ".ssb"), folder / (name + "_ssb"))
    for i, case in enumerate(cases):
        (folder / f"f{i:03d}").write_bytes(case["raw"])
    source = driver(cases)
    if args.compile:
        source = ('TURBO_objfil "ram1_target_exe"\nTURBO_repfil "nfa1_report"\n'
                  'TURBO_windo 0\nTURBO_objdat 1024\nTURBO_buffersz 64\n'
                  'TURBO_list 0\nTURBO_model ">"\nTURBO_optim 0\n') + source
    (folder / "input_ssb").write_text(source, encoding="ascii", newline="\n")
    if args.runtime_only:
        shutil.copy2(args.runtime_only / "target_exe", folder / "target_exe")
        boot = '100 LRESPR "nfa1_ttk"\n110 EXECUTE_W "sfa1_target_exe"\n'
    else:
        boot = '''100 LRESPR "nfa1_env_bin"
110 LRESPR "nfa1_ttk"
120 SETENV "SSBWORK=nfa1_"
130 SETENV "SSBWORK2=ram1_"
140 SETENV "SSBSTART=1"
150 SETENV "SSBINC=1"
160 SETENV "SSBRECWORK=ram1_"
170 SETENV "SSBOVER=Y"
175 SETENV "SSBRECLOG=nfa1_errorlog"
180 DEFAULT_DEVICE "sfa1_"
190 EXECUTE_W "sfa1_ssb280_exe";"input"
200 IF FTEST("ram1_input_bas")<>0 THEN OPEN_NEW #9,"nfa1_done":PRINT #9,"ERROR":CLOSE #9:STOP
210 COPY "ram1_input_bas" TO "nfa1_output_bas"
220 ''' + ('LOAD' if args.compile else 'LRUN') + ' "ram1_input_bas"\n'
    (folder / "boot").write_text(boot, encoding="ascii")
    commands = ["", "", "LRUN nfa1_boot"]
    if args.compile and not args.runtime_only:
        commands += [chr(3), "CHARGE " + chr(92), chr(2),
                     'COPY "ram1_target_exe" TO "sfa1_target_exe"', 'EXECUTE_W "ram1_target_exe"']
    (folder / "commands").write_text("\n".join(commands) + "\n", encoding="ascii")
    cp = str(args.ssb_root / "build/newclasses") + ";" + str(args.ssb_root / "build/emulator-lib/*")
    command = ["java", "-Dql.ramMiB=" + str(args.ram_mib), "-cp", cp, "QlRun",
               str(args.ssb_root / "build/SMSQE"), str(folder), str(args.timeout), str(folder / "commands")]
    print("Running real SMSQ/E pack loader: " + str(folder), flush=True)
    run = subprocess.run(command, capture_output=True, timeout=args.timeout + 30)
    (folder / "emulator.log").write_bytes(run.stdout + run.stderr)
    errors = []
    if run.returncode:
        errors.append("Emulator returned " + str(run.returncode))
    if (folder / "errorlog").exists():
        errors.append((folder / "errorlog").read_text("latin1"))
    if not (folder / "done").exists() or (folder / "done").read_text().strip() != "OK":
        errors.append("No successful completion marker")
    if (folder / "result").exists():
        errors += verify(folder, cases)
    else:
        errors.append("No result file")
    if not args.runtime_only:
        expected = sum(len(re.findall(r"^MANIFEST\s*:", (PARSER / (n + ".ssb")).read_text("ascii"), re.M))
                       for n in ["json", "pack"])
        translated = (folder / "output_bas").read_text("latin1") if (folder / "output_bas").exists() else ""
        if len(re.findall(r"^\d+\s+MANIFEST\s*:", translated, re.M)) != expected:
            errors.append("MANIFEST declarations did not survive SSB translation")
    if args.compile and not args.runtime_only:
        report = (folder / "report").read_text("latin1") if (folder / "report").exists() else ""
        if "Code generation completed" not in report or "*** Error" in report:
            errors.append("Turbo compilation did not complete cleanly")
    report = dict(passed=not errors, compiled=args.compile or bool(args.runtime_only),
                  runtimeOnly=bool(args.runtime_only), ramMiB=args.ram_mib, caseCount=len(cases) + 2,
                  loaderSha256=loader_hash, parserSha256=parser_hash,
                  suiteSha256=suite_hash, driverSha256=driver_hash,
                  errors=errors[:25], folder=str(folder),
                  testedAt=datetime.datetime.now(datetime.timezone.utc).isoformat())
    (folder / "verification.json").write_text(json.dumps(report, indent=2))
    print(json.dumps(report, indent=2))
    return 0 if report["passed"] else 1


if __name__ == "__main__":
    raise SystemExit(main())
