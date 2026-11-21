"""Exercise compiled or interpreted CLI execution and validate packs in F#."""
import argparse
import json
from pathlib import Path
import subprocess
import hashlib
import re
import shutil
from build import ROOT,SCRATCH
from run import run


def main():
    p=argparse.ArgumentParser(description=__doc__)
    p.add_argument('--ssb-root',type=Path,default=Path('C:/Source/Structured Super Basic'))
    p.add_argument('--work',type=Path,default=SCRATCH/'integration')
    p.add_argument('--artifacts',type=Path,default=SCRATCH/'artifacts')
    p.add_argument('--timeout',type=int,default=600)
    p.add_argument('--interpreted',action='store_true',help='Run every CLI case through SBASIC rather than the Turbo executable')
    args=p.parse_args();args.work.mkdir(parents=True,exist_ok=True)
    checks=[];packs=[]
    def case(name,argv,want=0,pack=None,cancel=False):
        print('Native CLI: '+name,flush=True)
        if pack:(args.work/pack).unlink(missing_ok=True)
        code=abs(run(args.ssb_root,args.work,argv,args.timeout,cancel=cancel,interpreted=args.interpreted))
        if code!=want:raise AssertionError(f'{name}: exit {code}, expected {want}')
        if pack:
            path=args.work/pack
            if not path.exists():raise AssertionError(name+': missing output')
            packs.append(path)
        checks.append(name)
    case('help',['--help'])
    case('negative seed',['--seed','-1'],2)
    case('duplicate flag',['--width','8','--width','8'],2)
    case('connect target rejected',['--difficulty','hard'],2)
    case('ramp pair override rejected',['--mode','full-coverage','--ramp','easy..hard','--pairs','6'],2)
    common=['--width','4','--height','4','--pairs','3','--min-length','2','--max-length','6','--seed','42','--count','2']
    collision=args.work/'connect_json_spool_0'
    collision.write_bytes(b'PREEXISTING FILE MUST SURVIVE\n')
    case('connections batch',common+['--show-solution','--output','nfa1_connect_json'],pack='connect_json')
    assert collision.read_bytes()==b'PREEXISTING FILE MUST SURVIVE\n'
    collision.unlink()
    checks.append('temporary filename collision preserves existing file')
    # The default 8x8 identities exercise the signed-word boundary that failed
    # in SMSQ/E 3.13, unlike the smaller connection fixtures above.
    case('default 8x8 ten-puzzle batch',['--seed','42','--count','10','--output','nfa1_default_json'],pack='default_json')
    case('deterministic replay',common+['--output','nfa1_replay_json'],pack='replay_json')
    assert (args.work/'connect_json').read_bytes()==(args.work/'replay_json').read_bytes()
    checks.append('identical native replay bytes')
    replay=json.loads((args.work/'connect_json').read_text())
    assert [p['seed'] for p in replay['puzzles']]==[42,1388897097]
    checks.append('portable SplitMix64 batch seed sequence')
    full=['--mode','full-coverage','--width','3','--height','3','--pairs','3','--min-length','3','--max-length','3','--max-attempts','20','--seed','17']
    case('full varied cuts',full+['--cut-style','varied','--output','nfa1_full_json'],pack='full_json')
    case('exact easy proof',full+['--difficulty','easy','--candidates','1','--improve-steps','0','--solver-work','50000','--output','nfa1_easy_json'],pack='easy_json')
    case('assessed full batch ordering',['--mode','full-coverage','--width','4','--height','4','--pairs','4','--min-length','2','--max-length','8','--seed','2147483647','--count','2','--assess','--solver-work','1','--candidates','1','--improve-steps','0','--output','nfa1_ordered_json'],pack='ordered_json')
    seeds=[p['seed'] for p in json.loads((args.work/'ordered_json').read_text())['puzzles']]
    assert seeds==sorted(seeds)
    checks.append('assessed full-coverage ordering is ascending')
    case('honest solver limit',common[:-2]+['--assess','--solver-work','1','--output','nfa1_limited_json'],pack='limited_json')
    unique=['--width','2','--height','2','--pairs','2','--min-length','2','--max-length','2','--seed','5','--candidates','1','--improve-steps','0']
    case('optional uniqueness',unique+['--require-unique','--output','nfa1_unique_json'],pack='unique_json')
    case('maximum dimension and seed',['--mode','full-coverage','--width','64','--height','1','--pairs','32','--min-length','2','--max-length','2','--seed','2147483647','--max-attempts','1','--output','nfa1_boundary_json'],pack='boundary_json')
    case('quoted output filename',unique+['--output','nfa1_quoted pack_json'],pack='quoted pack_json')
    case('shortest screen rejects trivial board',unique+['--avoid-shortest','--output','nfa1_rejected_json'],1)
    assert not (args.work/'rejected_json').exists()
    sentinel=b'ORIGINAL OUTPUT MUST SURVIVE\n';(args.work/'protected_json').write_bytes(sentinel)
    case('existing output protected',full+['--output','nfa1_protected_json'],2)
    assert (args.work/'protected_json').read_bytes()==sentinel
    case('keyboard cancellation preserves output',['--mode','full-coverage','--width','16','--height','16','--pairs','8','--min-length','4','--max-length','64','--difficulty','expert','--seed','1','--max-attempts','10000','--candidates','100','--solver-work','100000000','--output','nfa1_protected_json','--force'],130,cancel=True)
    assert (args.work/'protected_json').read_bytes()==sentinel
    case('failed exact grade preserves forced output',full+['--difficulty','hard','--candidates','1','--improve-steps','0','--output','nfa1_protected_json','--force'],1)
    assert (args.work/'protected_json').read_bytes()==sentinel
    case('successful force replacement',full+['--output','nfa1_protected_json','--force'])
    packs.append(args.work/'protected_json')
    if args.interpreted:
        # An invalid channel exercises the actual native WHEN ERRor handler,
        # rather than the CLI's ordinary validation/error-return paths.
        fault_release=args.work/'runtime-fault'
        fault_release.mkdir(exist_ok=True)
        shutil.copy2(ROOT/'build/latest/ttk',fault_release/'ttk')
        source=(ROOT/'build/latest/output_bas').read_text('ascii')
        source,replacements=re.subn(r'(?m)^\d+ gInput% = 0$',
            '6000 PRINT #32767,"Injected invalid channel"',source)
        if replacements!=1:raise AssertionError('Runtime fault injection did not match the driver')
        # Preserve increasing source line numbers for the SBASIC loader.
        source='\n'.join(str(n)+' '+line.split(' ',1)[1]
                         for n,line in enumerate(source.splitlines(),1))+'\n'
        (fault_release/'output_bas').write_text(source,encoding='ascii',newline='\n')
        code=abs(run(args.ssb_root,args.work,['--help'],args.timeout,
                     release=fault_release,interpreted=True))
        if code!=1:raise AssertionError(f'Interpreted runtime error: exit {code}, expected 1')
        checks.append('native interpreter error trap returns failure to EXEC_W')
    if list(args.work.glob('*_spool_*')) or list(args.work.glob('*_console_*')) or list(args.work.glob('*_previous_*')):
        raise AssertionError('Successful/failed batches left disposable files')
    core=args.artifacts/'bin/Tangles.Core/release/Tangles.Core.dll'
    generator=args.artifacts/'bin/Tangles.Generator/release/Tangles.Generator.dll'
    subprocess.run(['dotnet','fsi','--reference:'+str(core),'--reference:'+str(generator),str(ROOT/'Validate.fsx'),'--',*[str(x) for x in packs]],check=True)
    manifest=json.loads((ROOT/'build/latest/build.json').read_text())
    report={'passed':True,'compiled':not args.interpreted,'checks':checks,'packs':[{'name':p.name,'sha256':hashlib.sha256(p.read_bytes()).hexdigest()} for p in packs],
            'executableSha256':manifest['executableSha256'],'reference':'Actual shared F# loader, rules, hints, solver and grader'}
    (args.work/'verification.json').write_text(json.dumps(report,indent=2));print(json.dumps(report,indent=2))


if __name__=='__main__':main()
