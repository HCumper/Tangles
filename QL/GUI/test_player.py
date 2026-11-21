"""Differential tests: actual F# Play versus actual SSB/Turbo under SMSQ/E.

All emulator files are confined to the test directory; no user disks mount.
"""
import argparse
import hashlib
import json
import os
from pathlib import Path
import shutil
import subprocess
import time

ROOT = Path(__file__).resolve().parent
REPO = ROOT.parents[1]


def main():
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument('--ssb-root', type=Path, default=Path('C:/Source/Structured Super Basic'))
    parser.add_argument('--oracle', type=Path)
    parser.add_argument('--work', type=Path)
    parser.add_argument('--core-dll', type=Path, default=Path('C:/Source/codex-workspace/.scratch/tangles-generator-fixes/artifacts/bin/Tangles.Core/release/Tangles.Core.dll'))
    parser.add_argument('--timeout', type=int, default=600)
    args = parser.parse_args()
    work = (args.work or Path('C:/Source/codex-workspace/.scratch/tangles-generator-fixes/ql-player-tests')).resolve()
    work.mkdir(parents=True, exist_ok=True)
    for name in ('done', 'report', 'result', 'errorlog', 'target_exe'):
        (work/name).unlink(missing_ok=True)
    oracle = args.oracle or work / 'oracle.json'
    if not args.oracle:
        subprocess.run(['dotnet', 'fsi', '--reference:'+str(args.core_dll.resolve()), str(ROOT/'PlayerOracle.fsx'), '--', str(oracle)], check=True, cwd=REPO)
    cases = json.loads(oracle.read_text())
    codes = {'begin': 0, 'move': 1, 'end': 2, 'undo': 3, 'reset': 4, 'save': 5, 'reload': 6, 'hint': 7}
    for name in ('ssb280_exe', 'parser_task', 'codegen_task', 'ttk', 'env_bin'):
        shutil.copy2(args.ssb_root/'build/compiled-app'/name, work/name)
    for name in ('play', 'progress', 'hints'):
        shutil.copy2(ROOT/(name+'.ssb'), work/(name+'_ssb'))
    for name in ('json', 'pack'):
        shutil.copy2(ROOT.parent/'Parser'/(name+'.ssb'), work/(name+'_ssb'))
    for i, case in enumerate(cases):
        (work/f'case{i}').write_text(case['json'], encoding='utf-8')
        (work/f'ops{i}').write_text(''.join(f'{codes[s["action"]]}\n{s["cell"]}\n' for s in case['steps']), encoding='ascii')
    shutil.copy2(REPO/'puzzles/hard-12x12-100.json', work/'large')
    minimal = json.loads(cases[0]['json'])
    invalid = json.dumps(minimal)[:-1] + ',"ignored":{"nested":[1,]}}'
    (work/'bad_metadata').write_text(invalid)
    escaped = json.dumps(minimal).replace('"pairs"', '"pa\\u0069rs"')
    (work/'escaped').write_text(escaped)
    lines = ['TURBO_objfil "ram1_target_exe"', 'TURBO_repfil "nfa1_report"',
             'TURBO_windo 0', 'TURBO_objdat 6144', 'TURBO_buffersz 64',
             'TURBO_list 0', 'TURBO_model ">"', 'TURBO_optim 0',
             '#INCLUDE hints_ssb', 'OPEN_NEW #9,"nfa1_result"']
    for i, case in enumerate(cases):
        lines += [f'testCase%={i}', f'pack_load_player "nfa1_case{i}",256',
                  'PRINT #9,"LOAD|";testCase%;"|";packDoc.error%',
                  'IF packDoc.error% THEN CLOSE #9 : STOP', 'play_open 1',
                  f'hint_load "nfa1_case{i}",1',
                  f'PRINT #9,"HINT{i}|";hintAvailable%',
                  f'testInput%=FOP_IN("nfa1_ops{i}")',
                  f'FOR testStep%=1 TO {len(case["steps"])}',
                  'testAction%=progress_number(testInput%)', 'testCell%=progress_number(testInput%)',
                  'testOK%=1', 'SELect ON testAction%',
                  '=0: testOK%=play_begin%(testCell%)', '=1: testOK%=play_step%(testCell%)',
                  '=2: play_finish', '=3: play_undo', '=4: play_reset', '=5: progress_save',
                  '=6: play_open 1 : progress_load', '=7: testOK%=hint_reveal%(testCell%)', 'END SELect', 'test_dump',
                  'END FOR testStep%', 'CLOSE #testInput%',
                  'PRINT #9,"VALID|";testCase%;"|";play_valid%']
    # Save rollback: the second slot is intentionally damaged, and the first
    # complete generation must remain available. No production progress touched.
    lines += ['pack_load_player "nfa1_case0",256', 'play_open 1',
              'testOK%=play_begin%(0)', 'testOK%=play_step%(1)', 'progress_save',
              'testOK%=play_begin%(1)', 'testOK%=play_step%(2)', 'progress_save',
              'testBad%=FOP_OVER(progressFile$ & (1+progressSerial-2*INT(progressSerial/2)))',
              'PRINT #testBad%,"INTERRUPTED"', 'CLOSE #testBad%', 'play_open 1', 'progress_load',
              'PRINT #9,"RECOVER|";playUsed%;"|";playOrder%(1)',
              'pack_load_player "nfa1_bad_metadata",256',
              'PRINT #9,"BADMETA|";packDoc.error%;"|";packDoc.puzzleCount%',
              'pack_load_player "nfa1_escaped",256',
              'PRINT #9,"ESCAPED|";packDoc.error%;"|";packDoc.puzzleCount%',
              'pack_load_player "nfa1_large",32766',
              'PRINT #9,"LARGE|";packDoc.error%;"|";packDoc.puzzleCount%;"|";packDoc.pairCount%',
              'IF NOT packDoc.error% THEN',
              'FOR testI%=1 TO packDoc.puzzleCount%',
              'PRINT #9,"P|";testI%;"|";progress_hex$(packPuzzles(testI%).id$);"|";packPuzzles(testI%).width%;"|";packPuzzles(testI%).height%;"|";packPuzzles(testI%).pairCount%',
              'END FOR testI%', 'FOR testI%=1 TO packDoc.pairCount%',
              'PRINT #9,"E|";testI%;"|";progress_hex$(packPairs(testI%).label$);"|";packPairs(testI%).start.x%;"|";packPairs(testI%).start.y%;"|";packPairs(testI%).finish.x%;"|";packPairs(testI%).finish.y%',
              'END FOR testI%',
              'play_open 100 : hint_load "nfa1_large",100',
              'PRINT #9,"LATEHINT|";hintAvailable%;"|";packDoc.error%',
              'IF hintAvailable% THEN', 'FOR testI%=1 TO playPairCount%',
              'testOK%=hint_reveal%(testI%)', 'END FOR testI%',
              'PRINT #9,"LATESOLVED|";playSolved%;"|";play_valid%', 'END IF',
              'END IF', 'CLOSE #9',
              'OPEN_NEW #9,"nfa1_done":PRINT #9,"OK":CLOSE #9', 'STOP',
              'DEFine PROCedure test_dump',
              'PRINT #9,"S|";testCase%;"|";testStep%;"|";testOK%;"|";playActive%;"|";playHistoryCount%;"|";playConnected%;"|";playUsed%;"|";playSolved%;"|";',
              'FOR testI%=0 TO playSize%-1',
              'PRINT #9,playOwner%(testI%);":";playOrder%(testI%);",";',
              'END FOR testI%', 'PRINT #9',
              'testChoice%=hint_choose% : PRINT #9,"CAND|";testCase%;"|";testStep%;"|";testChoice%;"|";',
              'IF hint_current% AND NOT playSolved% THEN',
              'FOR testI%=1 TO playPairCount%', 'IF hintClear%(testI%) THEN PRINT #9,testI%;",";',
              'END FOR testI%', 'END IF', 'PRINT #9', 'END DEFine']
    (work/'input_ssb').write_text('\n'.join(lines)+'\n', encoding='ascii')
    (work/'boot').write_text('''100 LRESPR "nfa1_env_bin"
110 LRESPR "nfa1_ttk"
120 SETENV "SSBWORK=nfa1_"
121 SETENV "SSBSTART=1"
122 SETENV "SSBINC=1"
130 SETENV "SSBWORK2=ram1_"
140 SETENV "SSBRECWORK=ram1_"
150 SETENV "SSBOVER=Y"
160 SETENV "SSBRECLOG=nfa1_errorlog"
170 DEFAULT_DEVICE "sfa1_"
180 EXECUTE_W "sfa1_ssb280_exe";"input"
190 COPY "ram1_input_bas" TO "nfa1_output_bas"
200 LOAD "ram1_input_bas"
''', encoding='ascii')
    commands = '\n\nLRUN nfa1_boot\n\x03\nCHARGE \\\n\x02\nCOPY "ram1_target_exe" TO "sfa1_target_exe"\nEXECUTE_W "ram1_target_exe"\n'
    (work/'commands').write_text(commands, encoding='ascii')
    cp = str(args.ssb_root/'build/newclasses')+';'+str(args.ssb_root/'build/emulator-lib/*')
    print(f'QL differential tests: {work}', flush=True)
    with (work/'emulator.log').open('wb') as log:
        process = subprocess.Popen(['java','-Dql.ramMiB=16','-cp',cp,'QlRun',str(args.ssb_root/'build/SMSQE'),str(work),str(args.timeout),str(work/'commands')], stdout=log, stderr=subprocess.STDOUT)
        until=time.monotonic()+args.timeout+30
        while process.poll() is None:
            if (work/'errorlog').exists():
                process.terminate(); process.wait(timeout=10)
                raise RuntimeError((work/'errorlog').read_text('latin1'))
            try:
                report=(work/'report').read_text('latin1')
                if '**** ERROR' in report:
                    process.terminate(); process.wait(timeout=10)
                    raise RuntimeError(report)
            except (FileNotFoundError,PermissionError):
                pass
            if time.monotonic()>until:
                process.kill();process.wait();raise TimeoutError(str(work))
            time.sleep(0.5)
    errors=[]
    if process.returncode or not (work/'done').exists(): errors.append('Emulator did not complete')
    expected={(i,j+1):s for i,c in enumerate(cases) for j,s in enumerate(c['steps'])}
    seen=set();large_p=[];large_e=[];checks={}
    if (work/'result').exists():
        for line in (work/'result').read_text('latin1').splitlines():
            row=[s.strip() for s in line.split('|')]
            if row[0]=='S':
                key=int(row[1]),int(row[2]); seen.add(key);s=expected[key]
                values=[int(v) for v in row[3:9]]
                want=[int(s['accepted']),s['active'],s['history'],s['connected'],s['used'],int(s['solved'])]
                cells=[tuple(map(int,v.split(':'))) for v in row[9].split(',') if v]
                if values!=want or cells!=list(zip(s['owners'],s['orders'])):
                    errors.append(f'{cases[key[0]]["name"]} step {key[1]} {s["action"]} {s["cell"]}: {values} != {want}; cells match={cells==list(zip(s["owners"],s["orders"]))}')
            elif row[0]=='VALID' and row[2]!='1': errors.append('Restored path validation: '+line)
            elif row[0]=='P': large_p.append(row)
            elif row[0]=='E': large_e.append(row)
            elif row[0]=='CAND':
                key=int(row[1]),int(row[2]); want=expected[key]['candidates']
                candidates=[int(v) for v in row[4].split(',') if v]
                choice=int(row[3])
                if candidates!=want or (choice not in want if want else choice!=0):
                    errors.append(f'{cases[key[0]]["name"]} step {key[1]} hint candidates {candidates}/{choice} != {want}')
            else: checks[row[0]]=row[1:]
    if seen!=set(expected): errors.append(f'Missing transitions: {len(seen)}/{len(expected)}')
    for name,want in [('RECOVER',['2','2']),('BADMETA',['1','0']),('ESCAPED',['0','1']),('LARGE',['0','100','1200'])]:
        if checks.get(name)!=want: errors.append(f'{name}: {checks.get(name)} != {want}')
    for i,case in enumerate(cases):
        if checks.get(f'HINT{i}')!=[str(int(case['hintAvailable']))]: errors.append(f'{case["name"]}: hint availability {checks.get(f"HINT{i}")}')
    for name,want in [('LATEHINT',['1','0']),('LATESOLVED',['1','1'])]:
        if checks.get(name)!=want: errors.append(f'{name}: {checks.get(name)} != {want}')
    large=json.loads((work/'large').read_text())
    wanted_p=[['P',str(i+1),p['id'].encode().hex().upper(),str(p['width']),str(p['height']),str(len(p['pairs']))] for i,p in enumerate(large['puzzles'])]
    pairs=[pair for p in large['puzzles'] for pair in p['pairs']]
    wanted_e=[['E',str(i+1),p['label'].encode().hex().upper(),str(p['start']['x']),str(p['start']['y']),str(p['finish']['x']),str(p['finish']['y'])] for i,p in enumerate(pairs)]
    if large_p!=wanted_p or large_e!=wanted_e: errors.append('Large-pack projection changed the clues')
    report={'passed':not errors,'compiled':True,'reference':'Tangles.Core.Play','transitions':len(seen),'cases':len(cases),'largePackBytes':(work/'large').stat().st_size,'errors':errors[:30],'folder':str(work)}
    (work/'verification.json').write_text(json.dumps(report,indent=2))
    print(json.dumps(report,indent=2))
    return 0 if report['passed'] else 1

if __name__=='__main__':
    raise SystemExit(main())
