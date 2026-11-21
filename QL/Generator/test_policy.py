"""Check native exact-grade policies, saved witnesses, symmetry, ramps and seeds."""
import json
from pathlib import Path
from build import ROOT, SCRATCH, compile_source, emulate


def main():
    tool=Path('C:/Source/Structured Super Basic');work=SCRATCH/'policy'
    work.mkdir(parents=True,exist_ok=True)
    fixture=json.loads((ROOT/'tests/hard-fixture.json').read_text())
    source=['#INCLUDE '+n+'_ssb' for n in ['model','maths','construction','identity','solver','difficulty','selection','cli','export','batch']]
    source += ['OPEN_NEW #9,"nfa1_result"', 'WHEN_ERROR 0',
               'PRINT #9,"ERROR|";ERNUM%','CLOSE #9',
               'OPEN_NEW #8,"nfa1_done":PRINT #8,"ERROR":CLOSE #8','RJOB -1,-1','END_WHEN',
               'cli_parse "--mode full-coverage --width 8 --height 8 --pairs 8 --min-length 4 --max-length 16 --difficulty hard --min-trials 1 --max-trials 3 --seed 42"',
               'gen_board','DIM gSeen$(100,64),gSavedOwner%(63),gSavedOrder%(63)']
    for p,route in enumerate(fixture['solution'],1):
        cells=[c['y']*8+c['x'] for c in route['cells']]
        source += [f'gLength%({p})={len(cells)} : gStart%({p})={cells[0]} : gFinish%({p})={cells[-1]}']
        source += [f'gOwner%({c})={p} : gOrder%({c})={i}' for i,c in enumerate(cells,1)]
    source += ['gen_endpoints : gen_validate','PRINT #9,"VALID|";gValid%',
               'selection_consider','PRINT #9,"ACCEPT|";selAccepted%;"|";sr.grade$;"|";sr.trialSteps',
               'FOR policyI%=0 TO 63','gOwner%(policyI%)=0 : gOrder%(policyI%)=0','END FOR policyI%',
               'selection_restore : gen_validate','PRINT #9,"RESTORE|";gValid%;"|";sr.grade$;"|";sr.trialSteps',
               'opt.minTrials=0 : opt.maxTrials=0 : selAccepted%=0 : selSeen%=0',
               'selection_consider','PRINT #9,"CEILING|";selAccepted%',
               'opt.maxTrials=-1 : opt.target%=2 : selAccepted%=0 : selSeen%=0',
               'selection_consider','PRINT #9,"EXACT|";selAccepted%',
               'DIM policyKey$(64)','policyKey$=identity_symmetry$',
               'FOR policyP%=1 TO opt.pairs%',
               'gStart%(policyP%)=8*INT(gStart%(policyP%)/8)+7-gStart%(policyP%) MOD 8',
               'gFinish%(policyP%)=8*INT(gFinish%(policyP%)/8)+7-gFinish%(policyP%) MOD 8',
               'END FOR policyP%',
               'PRINT #9,"SYMMETRY|";cli_equal%(policyKey$,identity_symmetry$)',
               'hash_reset : hash_text "abc"','PRINT #9,"SHA|";hash_finish$',
               'FOR policyI%=0 TO 6','PRINT #9,"SEED|";policyI%;"|";gen_int$(batch_seed(42,policyI%))','END FOR policyI%',
               'cli_parse "--mode full-coverage --width 16 --height 16 --pairs 64 --min-length 4 --max-length 4096"',
               'PRINT #9,"CAPACITY|";gExit%;"|";gen_int$(gen_product(64,4096))',
               'cli_parse "--width 64 --height 64 --pairs 2048 --min-length 4096 --max-length 4096"',
               'PRINT #9,"OVERSIZE|";gExit%',
               'cli_parse "--mode full-coverage --width 12 --height 12 --ramp easy..expert --count 10 --min-trials 5 --max-trials 7 --min-depth 2 --min-search-nodes 3"',
               'PRINT #9,"RAMP|";gExit%', 'FOR policyI%=1 TO 4','cli_section policyI%',
               'PRINT #9,"SECTION|";policyI%;"|";opt.pairs%;"|";opt.maximum%;"|";rampCounts%(policyI%);"|";opt.minTrials;"|";opt.minDepth%;"|";opt.minNodes%',
               'END FOR policyI%', 'batch_cleanup','CLOSE #9',
               'OPEN_NEW #8,"nfa1_done":PRINT #8,"OK":CLOSE #8','RJOB -1,0']
    entry=work/'policy.ssb';entry.write_text('\n'.join(source)+'\n',encoding='ascii')
    compile_source(entry,tool,work,600)
    (work/'result').unlink(missing_ok=True)
    (work/'runtimeboot').write_text('100 LRESPR "nfa1_ttk"\n110 EXEC_W "sfa1_target_exe"\n')
    emulate(tool,work,'\n\nLRUN nfa1_runtimeboot\n',600)
    rows=[[s.strip() for s in line.split('|')] for line in (work/'result').read_text().splitlines()]
    by={r[0]:r[1:] for r in rows if r[0] not in ['SEED','SECTION']}
    assert by['VALID']==['1'] and by['ACCEPT']==['1','hard','2'],rows
    assert by['RESTORE']==['1','hard','2'] and by['CEILING']==['0'] and by['EXACT']==['0'],rows
    assert by['SYMMETRY']==['1'] and by['SHA']==['BA7816BF8F01CFEA414140DE5DAE2223B00361A396177A9CB410FF61F20015AD'],rows
    assert [int(r[2]) for r in rows if r[0]=='SEED']==[42,1388897097,2096869710,1323180694,1118539959,651689611,1147992348],rows
    assert by['CAPACITY']==['0','262144'] and by['OVERSIZE']==['2'],rows
    assert by['RAMP']==['0'],rows
    assert [list(map(int,r[1:])) for r in rows if r[0]=='SECTION']==[
        [1,24,24,3,0,0,0],[2,20,24,3,0,0,0],[3,18,24,2,5,0,0],[4,6,48,2,5,2,3]],rows
    report={'passed':True,'checks':['hard exact-grade acceptance','stored witness/report restoration','trial effort ceiling','no grade fallback','reflection deduplication','SHA-256','seven SplitMix64 seeds','balanced 12x12 ramp recipes and section effort floors','large capacity arithmetic and invalid-size rejection']}
    (work/'verification.json').write_text(json.dumps(report,indent=2));print(json.dumps(report,indent=2))


if __name__=='__main__':main()
