"""Translate and Turbo-compile the native generator under real SMSQ/E.

Python orchestrates the external compiler/emulator; generation runs in SSB.
"""
import argparse
import hashlib
import json
from pathlib import Path
import re
import shutil
import subprocess
import time

ROOT = Path(__file__).resolve().parent
SCRATCH = Path('C:/Source/codex-workspace/.scratch/tangles-ql-generator')


def emulate(tool, work, commands, timeout=360, ram=16):
    (work/'commands').write_text(commands, encoding='ascii', newline='\n')
    (work/'done').unlink(missing_ok=True)
    cp = str(tool/'build/newclasses')+';'+str(tool/'build/emulator-lib/*')
    with (work/'emulator.log').open('wb') as log:
        process = subprocess.Popen(['java', f'-Dql.ramMiB={ram}', '-cp', cp, 'QlRun',
                                    str(tool/'build/SMSQE'), str(work), str(timeout), str(work/'commands')],
                                   stdout=log, stderr=subprocess.STDOUT)
        end = time.monotonic()+timeout+25
        while process.poll() is None:
            for name in ['errorlog', 'report']:
                try:
                    text = (work/name).read_text('latin1')
                except OSError:
                    continue
                if name=='errorlog' or '**** ERROR' in text or re.search(r'Parsing done, [1-9]\d* errors', text):
                    process.terminate(); process.wait(timeout=10)
                    raise RuntimeError(text)
            if time.monotonic()>end:
                process.kill(); process.wait(); raise TimeoutError(str(work))
            time.sleep(.5)
    if process.returncode or not (work/'done').exists():
        raise RuntimeError('SMSQ/E did not finish; see '+str(work/'emulator.log'))


def compile_source(entry, tool, work, timeout=360):
    work.mkdir(parents=True, exist_ok=True)
    for name in ['errorlog','report','done','target_exe','output_bas']:
        (work/name).unlink(missing_ok=True)
    for name in ['ssb280_exe','parser_task','codegen_task','ttk','env_bin']:
        shutil.copy2(tool/'build/compiled-app'/name,work/name)
    sources={}
    for source in ROOT.glob('*.ssb'):
        shutil.copy2(source,work/(source.stem+'_ssb'))
        sources[source.name]=hashlib.sha256(source.read_bytes()).hexdigest()
    # A test wrapper may live outside the product directory.
    shutil.copy2(entry,work/'entry_ssb')
    (work/'input_ssb').write_text('''TURBO_objfil "ram1_target_exe"
TURBO_repfil "nfa1_report"
TURBO_taskn "Tangles generator"
TURBO_windo 0
TURBO_objdat 8192
TURBO_buffersz 64
TURBO_list 0
TURBO_model ">"
TURBO_optim 0
#INCLUDE entry_ssb
''',encoding='ascii',newline='\n')
    (work/'boot').write_text('''100 LRESPR "nfa1_env_bin"
110 LRESPR "nfa1_ttk"
120 SETENV "SSBWORK=nfa1_"
130 SETENV "SSBWORK2=ram1_"
140 SETENV "SSBRECWORK=ram1_"
150 SETENV "SSBSTART=1"
160 SETENV "SSBINC=1"
170 SETENV "SSBOVER=Y"
180 SETENV "SSBRECLOG=nfa1_errorlog"
190 DEFAULT_DEVICE "sfa1_"
200 EXECUTE_W "sfa1_ssb280_exe";"input"
210 IF FTEST("ram1_input_bas")<>0 THEN OPEN_NEW #9,"nfa1_done":PRINT #9,"ERROR":CLOSE #9:STOP
220 COPY "ram1_input_bas" TO "nfa1_output_bas"
230 LOAD "ram1_input_bas"
''',encoding='ascii',newline='\n')
    print('Compiling '+str(entry)+' in '+str(work),flush=True)
    emulate(tool,work,'\n\nLRUN nfa1_boot\n\x03\nCHARGE \\\n\x02\nCOPY "ram1_target_exe" TO "sfa1_target_exe"\nOPEN_NEW #9,"nfa1_done":PRINT #9,"OK":CLOSE #9\n',timeout)
    report=(work/'report').read_text('latin1')
    if 'Code generation completed' not in report or not (work/'target_exe').exists():
        raise RuntimeError('Compilation incomplete: '+str(work))
    return sources,report


def main():
    p=argparse.ArgumentParser(description=__doc__)
    p.add_argument('--ssb-root',type=Path,default=Path('C:/Source/Structured Super Basic'))
    p.add_argument('--work',type=Path,default=SCRATCH/'build')
    p.add_argument('--entry',type=Path,default=ROOT/'generator.ssb')
    p.add_argument('--timeout',type=int,default=360)
    p.add_argument('--run',action='store_true',help='Run the entry as a native test, expecting nfa1_done')
    args=p.parse_args()
    sources,report=compile_source(args.entry.resolve(),args.ssb_root,args.work.resolve(),args.timeout)
    if args.run:
        (args.work/'result').unlink(missing_ok=True)
        (args.work/'runtimeboot').write_text('100 LRESPR "nfa1_ttk"\n110 EXEC_W "sfa1_target_exe"\n',encoding='ascii')
        emulate(args.ssb_root,args.work,'\n\nLRUN nfa1_runtimeboot\n',args.timeout)
        if (args.work/'done').read_text().strip()!='OK':raise RuntimeError('Native test reported failure; see '+str(args.work/'result'))
        print((args.work/'result').read_text('latin1') if (args.work/'result').exists() else 'Native test completed.')
    elif args.entry.resolve()==(ROOT/'generator.ssb').resolve():
        release=ROOT/'build/latest';release.mkdir(parents=True,exist_ok=True)
        for name,target in [('target_exe','generator_exe'),('ttk','ttk'),('output_bas','output_bas'),('report','report')]:
            shutil.copy2(args.work/name,release/target)
        (release/'build.json').write_text(json.dumps({'compiled':True,'sourceSha256':sources,
            'executableSha256':hashlib.sha256((release/'generator_exe').read_bytes()).hexdigest(),
            'toolchain':'SSB 2.8 / Turbo 5.09 / SMSQmulator SMSQ/E'},indent=2))
        print('Executable: '+str(release/'generator_exe'))
    print('\n'.join(s for s in report.splitlines() if 'errors and' in s or 'Code generation completed' in s))


if __name__=='__main__':main()

