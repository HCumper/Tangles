"""Run the compiled or interpreted generator in an isolated SMSQ/E.

Arguments after -- are encoded as the generator's native option string.
QL filenames such as nfa1_pack_json refer to --work (the only mounted drive).
"""
import argparse
import json
from pathlib import Path
import shutil
import subprocess
from build import ROOT, SCRATCH, emulate


def ql_string(value):
    return '&CHR$(34)&'.join('"'+part+'"' for part in value.split('"'))


def run(tool,work,arguments,timeout=600,release=None,cancel=False,interpreted=False):
    release=release or ROOT/'build/latest'
    work.mkdir(parents=True,exist_ok=True)
    shutil.copy2(release/'ttk',work/'ttk')
    if interpreted:
        shutil.copy2(release/'output_bas',work/'generator_bas')
        executable='nfa1_generator_bas'
    else:
        shutil.copy2(release/'generator_exe',work/'generator_exe')
        executable='sfa1_generator_exe'
    def argument(value):
        if not any(c.isspace() or c in "\"'" for c in value):return value
        if '"' not in value:return '"'+value+'"'
        if "'" not in value:return "'"+value+"'"
        raise ValueError('The native option-string syntax cannot encode both quote characters in one argument')
    command=' '.join(argument(a) for a in arguments)
    (work/'runtimeboot').write_text('''100 result=0
110 WHEN ERRor
120 result=ERNUM
130 CONTINUE
140 END WHEN
150 LRESPR "nfa1_ttk"
160 DATA_USE "nfa1_"
170 EXEC_W "'''+executable+'";'+ql_string(command)+'''
180 OPEN_NEW #9,"nfa1_result"
190 PRINT #9,result
200 CLOSE #9
210 OPEN_NEW #9,"nfa1_done":PRINT #9,"OK":CLOSE #9
''',encoding='ascii',newline='\n')
    (work/'result').unlink(missing_ok=True)
    commands='\n\nLRUN nfa1_runtimeboot\n'
    # The external harness sends keys 150 ms apart. Spaces allow the child
    # console to open before Escape; no Windows process is killed for this test.
    if cancel:commands+=' '*30+'\x1b'
    emulate(tool,work,commands,timeout)
    return int((work/'result').read_text().strip())


def main():
    p=argparse.ArgumentParser(description=__doc__)
    p.add_argument('--ssb-root',type=Path,default=Path('C:/Source/Structured Super Basic'))
    p.add_argument('--work',type=Path,default=SCRATCH/'run')
    p.add_argument('--timeout',type=int,default=600)
    p.add_argument('--interpreted',action='store_true',help='Execute translated BASIC with SBASIC and CMD$, without the compiled generator')
    p.add_argument('options',nargs=argparse.REMAINDER)
    a=p.parse_args();options=a.options[1:] if a.options[:1]==['--'] else a.options
    status=run(a.ssb_root,a.work.resolve(),options,a.timeout,interpreted=a.interpreted)
    print('Native QDOS job return:',status)
    print('Mounted files:',a.work.resolve())
    return abs(status)


if __name__=='__main__':raise SystemExit(main())
