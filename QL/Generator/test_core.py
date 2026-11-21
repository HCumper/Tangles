"""Compile native SSB and compare it with the existing F# solver and grader."""
import argparse
import json
from pathlib import Path
from build import ROOT, SCRATCH, compile_source, emulate


def fields(type_name, file):
    text=(ROOT/file).read_text().split('TYPE '+type_name+'\n')[1].split('END TYPE')[0]
    return [s.strip().split('(')[0] for s in text.splitlines() if s.strip()]


def main():
    p=argparse.ArgumentParser(description=__doc__)
    p.add_argument('--ssb-root',type=Path,default=Path('C:/Source/Structured Super Basic'))
    p.add_argument('--work',type=Path,default=SCRATCH/'core')
    p.add_argument('--oracle',type=Path,default=SCRATCH/'oracle.json')
    p.add_argument('--quick',action='store_true')
    p.add_argument('--runtime-only',action='store_true')
    args=p.parse_args();args.work.mkdir(parents=True,exist_ok=True)
    cases=json.loads(args.oracle.read_text())
    if args.quick:cases=cases[:8]+cases[18:26]+cases[-3:]
    report_fields=fields('SolverReport','model.ssb')
    geometry_fields=fields('GeometryMeasurements','difficulty.ssb')
    pair_fields=fields('PairMeasurements','difficulty.ssb')
    source=['#INCLUDE model_ssb','#INCLUDE maths_ssb','#INCLUDE construction_ssb',
            '#INCLUDE identity_ssb','#INCLUDE solver_ssb','#INCLUDE difficulty_ssb',
            'OPEN_NEW #9,"nfa1_result"','WHEN ERRor', 'PRINT #9,"NATIVE_ERROR|";ERNUM%;"|";ERLIN%', 'CLOSE #9', 'OPEN_NEW #9,"nfa1_done":PRINT #9,"ERROR":CLOSE #9', 'STOP', 'END WHEN', 'gen_defaults']
    for i,case in enumerate(cases):
        b=case['board'];ps=b['pairs']
        source += [f'opt.width%={b["width"]} : opt.height%={b["height"]} : opt.pairs%={len(ps)} : opt.full%={int(b["requireFullCoverage"])}','gen_board']
        for j,pair in enumerate(ps,1):
            a=pair['start']['y']*b['width']+pair['start']['x'];z=pair['finish']['y']*b['width']+pair['finish']['x']
            source += [f'gStart%({j})={a} : gFinish%({j})={z}']
        source += ['gen_endpoints',f'solver_analyse {case["work"]},{case["nodes"]}']
        source += [f'PRINT #9,"R|{i}|";'+(';"|";'.join('sr.'+f for f in report_fields))]
        if case['geometry'] is not None:
            source += ['geo_assess',f'PRINT #9,"G|{i}|";'+(';"|";'.join('gm.'+f for f in geometry_fields)),
                       f'PRINT #9,"SCORE|{i}|";geoScore%;"|";geoRaw',
                       'FOR oracleP%=1 TO opt.pairs%',f'PRINT #9,"P|{i}|";oracleP%;"|";'+(';"|";'.join('geoPair(oracleP%).'+f for f in pair_fields)),
                       'END FOR oracleP%']
    source += ['opt.width%=8 : opt.height%=8 : opt.pairs%=6 : opt.full%=0','opt.minimum%=4 : opt.maximum%=16 : opt.attempts%=500','gen_board',
               'grow_generate 42','gen_validate','PRINT #9,"GROW|";gSuccess%;"|";gValid%',
               'opt.width%=4 : opt.height%=4 : opt.pairs%=4 : opt.full%=1','opt.minimum%=2 : opt.maximum%=8 : opt.attempts%=500','gen_board',
               'rng_seed 0,42 : gRngStream%=0 : cover_init : cover_find 128,64,372,1','gen_validate',
               'PRINT #9,"COVER|";covFound%;"|";gValid%','CLOSE #9','OPEN_NEW #9,"nfa1_done":PRINT #9,"OK":CLOSE #9','STOP']
    wrapper=args.work/'core.ssb';wrapper.write_text('\n'.join(source)+'\n',encoding='ascii')
    if not args.runtime_only:compile_source(wrapper,args.ssb_root,args.work,600)
    (args.work/'runtimeboot').write_text('100 LRESPR "nfa1_ttk"\n110 EXEC_W "sfa1_target_exe"\n',encoding='ascii')
    (args.work/'result').unlink(missing_ok=True)
    emulate(args.ssb_root,args.work,'\n\nLRUN nfa1_runtimeboot\n',1800)
    errors=[];seen=set()
    mapping={'logical':'solvedByReasoning','firstNodes':'firstSolutionSearchNodes','firstDepth':'firstSolutionBranchDepth','maxDepth':'maxSearchDepth','exhaustive':'solutionCountComplete'}
    for line in (args.work/'result').read_text('latin1').splitlines():
        row=[s.strip() for s in line.split('|')]
        if row[0] in ['GROW','COVER']:
            if row[1:]!=['1','1']:errors.append(line)
            seen.add((row[0],-1))
            continue
        if row[0]=='NATIVE_ERROR':errors.append(line);continue
        kind,i=row[0],int(row[1]);case=cases[i];seen.add((kind,i))
        if kind=='R':
            for field,actual in zip(report_fields,row[2:]):
                name=field.rstrip('%').split('$')[0];want=case['solver'][mapping.get(name,name)]
                if isinstance(want,bool):want=int(want)
                if isinstance(want,str):ok=actual==want
                else:ok=float(actual)==want
                if not ok:errors.append(f'{case["name"]} {name}: {actual} != {want}')
        elif kind=='G':
            for field,actual in zip(geometry_fields,row[2:]):
                want=case['geometry']['measurements'][field]
                if abs(float(actual)-want)>0.00001:errors.append(f'{case["name"]} geometry {field}: {actual} != {want}')
        elif kind=='SCORE':
            want=case['geometry']
            if int(row[2])!=want['score'] or abs(float(row[3])-want['rawScore'])>0.00002:errors.append(f'{case["name"]} score {row[2:]} != {want["score"]}/{want["rawScore"]}')
        elif kind=='P':
            want=case['geometry']['pairMeasurements'][int(row[2])-1]
            for field,actual in zip(pair_fields,row[3:]):
                value=want[field.rstrip('%')]
                if int(actual)!=int(value):errors.append(f'{case["name"]} pair {field}: {actual} != {value}')
    if sum(1 for tag,i in seen if tag=='R')!=len(cases):errors.append('Missing solver reports')
    if not {('GROW',-1),('COVER',-1)}.issubset(seen):errors.append('Missing constructor smoke checks')
    report={'passed':not errors,'compiled':True,'cases':len(cases),'reference':'Tangles.Generator.Solver / Difficulty','errors':errors[:40]}
    (args.work/'verification.json').write_text(json.dumps(report,indent=2));print(json.dumps(report,indent=2))
    return bool(errors)


if __name__=='__main__':raise SystemExit(main())
