"""Build real SSB/Turbo QL code using the local SMSQmulator toolchain.

Toolchain and ROM binaries remain external. TPTR_EXT is a runtime dependency;
pass a local copy or allow the pinned official archive to be downloaded.
"""
import argparse
import hashlib
import io
import json
import re
import shutil
import subprocess
import time
import urllib.request
import zipfile
from pathlib import Path
from urllib.parse import urlsplit

ROOT = Path(__file__).resolve().parent
URL = "https://sinclairql.net/djw/turbo/tptrp20.zip"
SHA256 = "930d4f9740d0e1d4d03abea4386f502aee49b0447f0410e06a1f6b75c6d6f51c"


def main():
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("--ssb-root", type=Path, default=Path("C:/Source/Structured Super Basic"))
    parser.add_argument("--turboptr", type=Path, help="Local TPTR_EXT binary (otherwise download pinned archive)")
    parser.add_argument("--timeout", type=int, default=360)
    parser.add_argument("--check", action="store_true", help="Also run the 800x600 high-colour pointer smoke test")
    parser.add_argument("--work", type=Path, default=Path("C:/Source/codex-workspace/.scratch/tangles-generator-fixes/ql-gui-build"))
    args = parser.parse_args()
    tool = args.ssb_root.resolve()
    run = args.work.resolve()
    run.mkdir(parents=True, exist_ok=True)
    for name in ("report", "errorlog", "done", "tangles_exe", "output_bas"):
        (run / name).unlink(missing_ok=True)
    for name in ("ssb280_exe", "parser_task", "codegen_task", "ttk", "env_bin"):
        shutil.copy2(tool / "build/compiled-app" / name, run / name)
    if args.turboptr:
        shutil.copy2(args.turboptr, run / "tptr_ext")
    else:
        raw = urllib.request.urlopen(URL, timeout=30).read()
        if hashlib.sha256(raw).hexdigest() != SHA256:
            raise ValueError("TurboPTR archive digest differs from the reviewed distribution")
        with zipfile.ZipFile(io.BytesIO(raw)) as archive:
            name = next(n for n in archive.namelist() if n.lower().endswith("tptr.ext"))
            (run / "tptr_ext").write_bytes(archive.read(name))
    for source, target in (("build.ssb", "input_ssb"), ("tangles.ssb", "tangles_ssb"),
                           ("play.ssb", "play_ssb"), ("progress.ssb", "progress_ssb"), ("hints.ssb", "hints_ssb"),
                           ("version.ssb", "version_ssb")):
        shutil.copy2(ROOT / source, run / target)
    for name in ("pack", "json"):
        shutil.copy2(ROOT.parent / "Parser" / (name + ".ssb"), run / (name + "_ssb"))
    sources = [ROOT / n for n in ("tangles.ssb", "play.ssb", "progress.ssb", "hints.ssb", "version.ssb", "build.ssb")]
    sources += [ROOT.parent / "Parser" / n for n in ("pack.ssb", "json.ssb")]
    source_hashes = {str(p.relative_to(ROOT.parent)): hashlib.sha256(p.read_bytes()).hexdigest() for p in sources}
    wrapper = (run / "input_ssb").read_text("ascii")
    (run / "input_ssb").write_text(wrapper.replace('"ram1_tangles_err"', '"nfa1_report"'), encoding="ascii", newline="\n")
    boot = '''100 LRESPR "nfa1_env_bin"
110 LRESPR "nfa1_ttk"
120 LRESPR "nfa1_tptr_ext"
130 SETENV "SSBWORK=nfa1_"
140 SETENV "SSBWORK2=ram1_"
150 SETENV "SSBRECWORK=ram1_"
160 SETENV "SSBSTART=1"
170 SETENV "SSBINC=1"
180 SETENV "SSBOVER=Y"
190 SETENV "SSBRECLOG=nfa1_errorlog"
200 DEFAULT_DEVICE "sfa1_"
210 EXECUTE_W "sfa1_ssb280_exe";"input"
220 IF FTEST("ram1_input_bas")<>0 THEN OPEN_NEW #9,"nfa1_done":PRINT #9,"ERROR":CLOSE #9:STOP
230 COPY "ram1_input_bas" TO "nfa1_output_bas"
240 LOAD "ram1_input_bas"
'''
    (run / "boot").write_text(boot, encoding="ascii", newline="\n")
    commands = '\n\nLRUN nfa1_boot\n\x03\nCHARGE \\\n\x02\nCOPY "ram1_tangles_exe" TO "sfa1_tangles_exe"\nOPEN_NEW #9,"nfa1_done":PRINT #9,"OK":CLOSE #9\n'
    (run / "commands").write_text(commands, encoding="ascii", newline="\n")
    cp = str(tool / "build/newclasses") + ";" + str(tool / "build/emulator-lib/*")
    command = ["java", "-Dql.ramMiB=8", "-cp", cp, "QlRun", str(tool / "build/SMSQE"), str(run), str(args.timeout), str(run / "commands")]
    print("Building in " + str(run), flush=True)
    with (run / "build.log").open("wb") as log:
        process = subprocess.Popen(command, stdout=log, stderr=subprocess.STDOUT)
        deadline = time.monotonic() + args.timeout + 30
        while process.poll() is None:
            report_path = run / "report"
            if report_path.exists():
                try:
                    text = report_path.read_text("latin1")
                except OSError:  # The QL compiler may hold an exclusive host file handle.
                    text = ""
                if "**** ERROR" in text or re.search(r"Parsing done, [1-9][0-9]* errors", text):
                    process.terminate()
                    process.wait(timeout=10)
                    raise RuntimeError("Turbo rejected the source:\n" + text)
            if time.monotonic() > deadline:
                process.kill()
                process.wait()
                raise TimeoutError("Build timed out: " + str(run))
            time.sleep(0.5)
    report = (run / "report").read_text("latin1") if (run / "report").exists() else ""
    if process.returncode or not (run / "tangles_exe").exists() or "Code generation completed" not in report:
        raise RuntimeError("SSB/Turbo build failed; inspect " + str(run))
    if (run / "errorlog").exists():
        raise RuntimeError((run / "errorlog").read_text("latin1"))
    artifact = ROOT / "build" / "latest"
    artifact.mkdir(exist_ok=True)
    for name in ("tangles_exe", "tptr_ext", "ttk", "output_bas", "report", "build.log"):
        shutil.copy2(run / name, artifact / name)
    manifest = {"compiled": True, "sourceSha256": source_hashes, "executableSha256": hashlib.sha256((artifact / "tangles_exe").read_bytes()).hexdigest(), "toolchain": "SSB 2.8.0 / Turbo 5.09 / SMSQmulator SMSQ/E", "run": str(run)}
    (artifact / "build.json").write_text(json.dumps(manifest, indent=2))
    repo = ROOT.parents[1]
    webroot = repo / "src/Tangles.Player/wwwroot"
    entries = json.loads((webroot / "packs.json").read_text(encoding="utf-8"))["packs"]
    catalogue = []
    for i, entry in enumerate(entries):
        relative = urlsplit(entry["path"]).path
        path = webroot / relative
        if not path.exists():
            path = repo / "puzzles" / Path(relative).name
        pack = json.loads(path.read_text(encoding="utf-8"))
        dimensions = sorted({(p["width"], p["height"]) for p in pack["puzzles"]})
        size = ",".join(f"{w}x{h}" for w, h in dimensions)
        mode = "full coverage" if pack["rules"]["requireFullCoverage"] else "connections only"
        label = entry["name"].replace("\u2014", "-").replace("\u00d7", "x")
        name = f"{label} / {size} / {mode} / {len(pack['puzzles'])} puzzles"
        target = f"pack{i:02d}_json"
        shutil.copy2(path, artifact / target)
        catalogue.extend([name[:128], target])
    (artifact / "tangles_catalog").write_text(str(len(catalogue)//2) + "\n" + "\n".join(catalogue) + "\n", encoding="ascii")
    # Only remove obsolete generated pack copies, never user imports/progress.
    current_files = set(catalogue[1::2])
    for old in artifact.glob("pack[0-9][0-9]_json"):
        if old.name not in current_files:
            old.unlink()
    (artifact / "boot").write_text('100 DATA_USE "nfa1_"\n110 LRESPR "nfa1_ttk"\n120 LRESPR "nfa1_tptr_ext"\n130 EXEC_W "sfa1_tangles_exe"\n', encoding="ascii")
    print("\n".join(line for line in report.splitlines() if "errors and" in line or "Code generation completed" in line), flush=True)
    print("Executable: " + str(artifact / "tangles_exe"), flush=True)
    if args.check:
        (artifact / "smoke.json").unlink(missing_ok=True)
        subprocess.run(["javac", "-cp", cp, "-d", str(artifact), str(ROOT / "GuiCheck.java")], check=True)
        subprocess.run(["java", "-Dql.testDir=" + str(run / "gui-check"), "-cp", str(artifact) + ";" + cp, "GuiCheck", str(tool / "build/SMSQE"), str(artifact)], check=True, timeout=480)
        smoke = json.loads((artifact / "smoke.json").read_text())
        smoke["executableSha256"] = manifest["executableSha256"]
        (artifact / "smoke.json").write_text(json.dumps(smoke, indent=2) + "\n")
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
