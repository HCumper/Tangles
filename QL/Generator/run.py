"""Run the compiled native generator in an isolated SMSQ/E, not on the host CPU.

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


def run(tool,work,arguments,timeout=600,release=None,cancel=False):
    release=release or ROOT/'build/latest'
    work.mkdir(parents=True,exist_ok=True)
    for name in ['generator_exe','ttk']:
        shutil.copy2(release/name,work/name)
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
170 EXEC_W "sfa1_generator_exe";'''+ql_string(command)+'''
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
    p.add_argument('options',nargs=argparse.REMAINDER)
    a=p.parse_args();options=a.options[1:] if a.options[:1]==['--'] else a.options
    status=run(a.ssb_root,a.work.resolve(),options,a.timeout)
    print('Native QDOS job return:',status)
    print('Mounted files:',a.work.resolve())
    return abs(status)


if __name__=='__main__':raise SystemExit(main())
