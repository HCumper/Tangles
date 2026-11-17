"""Exercise the actual SSB translator and 68000 runtime, not a host parser port.

Uses an existing local SSB 2.8.0 / Turbo / SMSQmulator installation. Fixtures,
ROM and toolchain binaries stay outside the repository. No user drives mount.
"""
import argparse,datetime,hashlib,json,os,re,shutil,subprocess,time
from pathlib import Path

ROOT=Path(__file__).resolve().parents[1]
PARSER=ROOT/'Parser'
class Obj(list): pass
class Num(str): pass
def decoded(raw):
    return json.loads(raw.decode('utf-8-sig'),object_pairs_hook=Obj,parse_int=Num,parse_float=Num)
def expected_nodes(raw):
    nodes=[]
    def visit(value,key=''):
        index=len(nodes)+1
        kind=1 if isinstance(value,Obj) else 2 if isinstance(value,list) else 4 if isinstance(value,Num) else 3 if isinstance(value,str) else 5 if value is True else 6 if value is False else 7
        text=str(value) if kind in (3,4) else ''
        node={'handle':index,'kind':kind,'first':0,'next':0,'count':0,'key':key.encode().hex().upper(),'text':text.encode().hex().upper(),'value':value};nodes.append(node)
        children=value if kind==1 else [(None,v) for v in value] if kind==2 else []
        previous=0
        for k,v in children:
            child=visit(v,k if kind==1 else '')
            if previous:nodes[previous-1]['next']=child
            else:node['first']=child
            previous=child;node['count']+=1
        return index
    visit(decoded(raw));return nodes
def fixtures():
    cases=[]
    def add(name,data,error=0,capacity=256,depth=32,maxbytes=1048576):
        cases.append(dict(name=name,data=data.encode() if isinstance(data,str) else data,error=error,capacity=capacity,depth=depth,maxbytes=maxbytes))
    for name,text in [('empty_object','{}'),('empty_array','[]'),('null','null'),('true','true'),('false','false'),('zero','0'),('negative','-123'),('fraction','-0.125e+2'),('text','"hello"'),('empty_text','""'),('nested','{"a":[{},[],true,false,null,-1,2.5E-2]}'),('duplicates','{"A":1,"a":2,"A":3,"":4}'),('escaped_keys','{"\\u0061":1,"a":2,"x\\ny":3}'),('escapes','"\\\"\\\\\\/\\b\\f\\n\\r\\t\\u0000"'),('unicode','"caf\u00e9 \U0001f600"'),('unicode_escape','"\\u00e9 \\uD83D\\uDE00"'),('bom',b'\xef\xbb\xbf{}'),('crlf','\r\n {"a":\r\n 1} \t')]:add(name,text)
    for name,text in [('blank',''),('space',' \n\t'),('comma_object','{"x":1,}'),('comma_array','[1,]'),('colon','{"x" 1}'),('comma','[1 2]'),('key','{x:1}'),('mismatch','[}'),('open','['),('trailing','{} []'),('leading_zero','01'),('plus','+1'),('dot','.5'),('minus','-'),('fraction_end','1.'),('exponent','1e'),('exponent_sign','1e+'),('literal','True'),('literal_end','tru'),('single_quotes',"'a'"),('comment','/*a*/{}')]:add(name,text,3 if name not in ['minus','fraction_end','exponent','exponent_sign'] else 5)
    for name,text in [('unterminated','"abc'),('escape','"\\x"'),('hex','"\\u00xz"'),('short_hex','"\\u12"'),('high','"\\ud800"'),('low','"\\udc00"'),('bad_pair','"\\ud800\\u0041"'),('newline','"a\nb"'),('nul',b'"a\x00b"'),('utf8_overlong',b'"\xc0\x80"'),('utf8_cont',b'"\xe2A\x80"'),('utf8_short',b'"\xe2\x82"'),('utf8_surrogate',b'"\xed\xa0\x80"'),('utf8_range',b'"\xf4\x90\x80\x80"')]:add(name,text,4)
    add('capacity','[1,2,3]',6,capacity=3)
    add('depth','[[[]]]',7,depth=2)
    add('at_depth','[[[]]]',0,depth=3)
    add('byte_limit','{"a":1}',1,maxbytes=3)
    add('chunk_boundary',' '*510+'{"\\u0061":"'+('z'*1000)+'","n":1555655117}')
    add('long_string','"'+('x'*4097)+'"')
    add('huge_number','1e9999')
    return cases

def driver(cases,packs):
    lines=['#INCLUDE json_ssb','DIM testText$(4096),testKey$(4096),testResult$(4096)','OPEN_NEW #9,"nfa1_result"']
    for i,c in enumerate(cases):
        lines += [f'json_load "nfa1_f{i:03d}",{c["capacity"]},{c["maxbytes"]},{c["depth"]}',f'PRINT #9,"C|{c["name"]}|";jsonDoc.error%;"|";jsonDoc.errorPosition;"|";jsonDoc.errorLine;"|";jsonDoc.errorColumn;"|";jsonDoc.used%;"|";jsonDoc.root%']
        if c['error']==0 and c['name'] not in ['long_string','huge_number']:
            lines += [f'PRINT #9,"BEGIN|{c["name"]}"','test_dump','PRINT #9,"END"']
    lines += ['json_load "nfa1_missing",256,1048576,32','PRINT #9,"MISSING|";jsonDoc.error%','json_parse "[1,true,null]",20,8','PRINT #9,"MEMORY|";jsonDoc.error%;"|";jsonDoc.used%']
    for name in packs:
        lines += [f'json_load "nfa1_{name}",4096,1048576,32',f'PRINT #9,"PACK|{name}|";jsonDoc.error%;"|";jsonDoc.bytes;"|";jsonDoc.used%',f'PRINT #9,"BEGIN|{name}"','test_dump','PRINT #9,"END"']
    lines += ['json_parse \'{"A":1,"a":2,"A":3,"":4,"\\u0062":5}\',64,8','testRoot% = jsonDoc.root%','testA% = json_member%(testRoot%,"A")','testB% = json_member%(testRoot%,"a")','testEmpty% = json_member%(testRoot%,"")','testEscaped% = json_member%(testRoot%,"b")','testAbsent% = json_member%(testRoot%,"absent")','PRINT #9,"LOOKUP|";json_number(testA%);"|";json_number(testB%);"|";json_number(testEmpty%);"|";json_number(testEscaped%);"|";testAbsent%','json_parse "[0,1555655117,-0.125e+2,true,false]",64,8','testRoot% = jsonDoc.root%','testA% = json_at%(testRoot%,1)','testB% = json_at%(testRoot%,2)','testTrue% = json_at%(testRoot%,3)','testFalse% = json_at%(testRoot%,4)','PRINT #9,"VALUES|";json_number(testA%);"|";json_number(testB%);"|";json_boolean%(testTrue%);"|";json_boolean%(testFalse%);"|";json_at%(testRoot%,9)','json_load "nfa1_f%03d",256,1048576,32' % next(i for i,c in enumerate(cases) if c['name']=='long_string'),'testResult$ = json_string$(jsonDoc.root%)','PRINT #9,"LONG|";jsonDoc.error%','json_load "nfa1_f%03d",256,1048576,32' % next(i for i,c in enumerate(cases) if c['name']=='huge_number'),'testValue = json_number(jsonDoc.root%)','PRINT #9,"HUGE|";jsonDoc.error%','json_parse "{}",8,8','testResult$ = json_string$(jsonDoc.root%)','PRINT #9,"TYPE|";jsonDoc.error%','CLOSE #9','OPEN_NEW #9,"nfa1_done":PRINT #9,"OK":CLOSE #9','STOP']
    lines += ['DEFine PROCedure test_dump','  LOCal tdNode%,tdIndex%,tdKind%,tdStart,tdLength','  FOR tdNode% = 1 TO jsonDoc.used%','    tdKind% = jsonNodes(tdNode%).kind%','    testKey$ = json_decode$(jsonNodes(tdNode%).keyStart,jsonNodes(tdNode%).keyLength)','    testText$ = ""','    IF tdKind% = 3 THEN testText$ = json_string$(tdNode%)','    IF tdKind% = 4 THEN testText$ = json_raw$(tdNode%)','    PRINT #9,"N|";tdNode%;"|";tdKind%;"|";jsonNodes(tdNode%).first%;"|";jsonNodes(tdNode%).next%;"|";jsonNodes(tdNode%).count%;"|";jsonNodes(tdNode%).start;"|";jsonNodes(tdNode%).length;"|";jsonNodes(tdNode%).keyStart;"|";jsonNodes(tdNode%).keyLength;"|";','    FOR tdIndex% = 1 TO LEN(testKey$)','      PRINT #9,HEX$(CODE(testKey$(tdIndex%)),8);','    END FOR tdIndex%','    PRINT #9,"|";','    FOR tdIndex% = 1 TO LEN(testText$)','      PRINT #9,HEX$(CODE(testText$(tdIndex%)),8);','    END FOR tdIndex%','    PRINT #9','  END FOR tdNode%','END DEFine']
    exact=next(i for i,l in enumerate(lines) if l.startswith('PRINT #9,"VALUES|"'))
    lines.insert(exact+1,'PRINT #9,"EXACT|";(json_number(testA%)=1555655117)')
    return '\n'.join(lines)+'\n'

def verify(folder,cases,packs):
    errors=[];lines=(folder/'result').read_text('latin1').splitlines();documents={c['name']:c['data'] for c in cases};documents.update(packs);seen=set();block=None;actual=[]
    for line in lines:
        parts=[x.strip() for x in line.split('|')]
        if parts[0]=='C':
            c=next(c for c in cases if c['name']==parts[1]);seen.add(c['name']);code=int(parts[2])
            if code!=c['error']:errors.append(f'{c["name"]}: expected error{c["error"]}, got{code}')
            if code and int(parts[-1])!=0:errors.append(f'{c["name"]}: invalid document exposes root')
            if code:
                position=int(parts[3]);prefix=c['data'][:position-1].replace(b'\r\n',b'\n').replace(b'\r',b'\n')
                if not 1<=position<=len(c['data'])+1 or [int(parts[4]),int(parts[5])]!=[prefix.count(b'\n')+1,len(prefix.rsplit(b'\n',1)[-1])+1]:errors.append(f'{c["name"]}: inconsistent error position/line/column')
        elif parts[0]=='PACK':
            if int(parts[2])!=0:errors.append(f'pack {parts[1]} parse failed')
        elif parts[0]=='BEGIN':block=parts[1];actual=[]
        elif parts[0]=='N':actual.append(parts)
        elif parts[0]=='END':
            raw=documents[block];expected=expected_nodes(raw)
            if len(actual)!=len(expected):errors.append(f'{block}: node count {len(actual)} != {len(expected)}')
            for a,e in zip(actual,expected):
                values=[int(x) for x in a[1:10]]
                if values[:5]!=[e[k] for k in ['handle','kind','first','next','count']] or a[10]!=e['key'] or a[11]!=e['text']:errors.append(f'{block}: node mismatch {e["handle"]}: {a} != {e}')
                start,length,ks,kl=values[5:];piece=raw[start-1:start-1+length]
                try:
                    value=decoded(piece)
                    if value!=e['value']:raise ValueError('value span differs from independent decoded subtree')
                    kind=e['kind']
                    if kind==3 and value.encode().hex().upper()!=e['text']:raise ValueError('string span differs')
                    if kind==4 and str(value).encode().hex().upper()!=e['text']:raise ValueError('number span differs')
                except Exception as ex:errors.append(f'{block}: bad value span at {e["handle"]}: {ex}')
                if ks:
                    try:key=json.loads(b'"'+raw[ks-1:ks-1+kl]+b'"');assert key.encode().hex().upper()==e['key']
                    except Exception as ex:errors.append(f'{block}: bad key span at {e["handle"]}: {ex}')
            block=None
    for c in cases:
        if c['name'] not in seen:errors.append('missing case '+c['name'])
    for marker in ['MISSING|2','MEMORY|0|4','LOOKUP|1|2|4|5|0','EXACT|1','LONG|9','HUGE|9','TYPE|8']:
        if not any(''.join(l.split())==marker for l in lines):errors.append('missing/incorrect '+marker)
    values=next((l.split('|')[1:] for l in lines if l.startswith('VALUES|')),None)
    # QL PRINT rounds large numbers to its default display precision.
    if values is None or abs(float(values[0])-1555655117)>1000 or [float(x) for x in values[1:]]!=[-12.5,1,0,0]:errors.append('number/boolean accessor values differ')
    return errors

def main():
    parser=argparse.ArgumentParser();parser.add_argument('--ssb-root',type=Path,default=Path('C:/Source/Structured Super Basic'));parser.add_argument('--packs',type=Path,default=ROOT.parent/'src/Tangles.Player/wwwroot/packs');parser.add_argument('--work',type=Path);parser.add_argument('--compile',action='store_true');parser.add_argument('--timeout',type=int,default=180);parser.add_argument('--example',action='store_true');parser.add_argument('--ram-mib',type=int,default=8);args=parser.parse_args()
    folder=(args.work or Path(os.environ['TEMP'])/'tangles-json-tests'/str(time.time_ns())).resolve();folder.mkdir(parents=True)
    tool=args.ssb_root
    for name in ['ssb280_exe','parser_task','codegen_task','ttk','env_bin']:shutil.copy2(tool/'build/compiled-app'/name,folder/name)
    shutil.copy2(PARSER/'json.ssb',folder/'json_ssb');cases=fixtures()
    shutil.copy2(PARSER/'pack.ssb',folder/'pack_ssb')
    for i,c in enumerate(cases):(folder/f'f{i:03d}').write_bytes(c['data'])
    packs={}
    for src in sorted(args.packs.glob('*.json')):
        name=src.stem.replace('-','')+'_json';packs[name]=src.read_bytes();(folder/name).write_bytes(packs[name])
    source=driver(cases,packs)
    if args.example:
        example=(PARSER/'pack_example.ssb').read_text(encoding='ascii')
        source=example.replace('#INCLUDE pack_ssb','#INCLUDE pack_ssb\nOPEN_NEW #9,"nfa1_result"').replace('PRINT ','PRINT #9,')
        before,after=source.rsplit('\nSTOP\n',1)
        source=before+'\nCLOSE #9\nOPEN_NEW #9,"nfa1_done":PRINT #9,"OK":CLOSE #9\nSTOP\n'+after
        (folder/'starter_json').write_bytes((args.packs/'starter.json').read_bytes())
    if args.compile:source='TURBO_objfil "ram1_target_exe"\nTURBO_repfil "nfa1_report"\nTURBO_windo 0\nTURBO_objdat 256\nTURBO_buffersz 64\nTURBO_list 0\nTURBO_model ">"\nTURBO_optim 0\n'+source
    (folder/'input_ssb').write_text(source,encoding='ascii',newline='\n')
    boot='''100 LRESPR "nfa1_env_bin"
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
220 '''+('LOAD' if args.compile else 'LRUN')+' "ram1_input_bas"\n'
    (folder/'boot').write_text(boot,encoding='ascii');commands=['','','LRUN nfa1_boot']
    if args.compile:commands += [chr(3),'CHARGE '+chr(92),chr(2),'COPY "ram1_target_exe" TO "sfa1_target_exe"','EXECUTE_W "ram1_target_exe"']
    (folder/'commands').write_text('\n'.join(commands)+'\n',encoding='ascii')
    cp=str(tool/'build/newclasses')+';'+str(tool/'build/emulator-lib/*')
    mainclass='QlRun'
    if os.environ.get('TANGLES_QL_DEBUG_CLASSES'):
        cp=os.environ['TANGLES_QL_DEBUG_CLASSES']+';'+cp;mainclass='TanglesQlRun'
    command=['java','-Dql.ramMiB='+str(args.ram_mib),'-cp',cp,mainclass,str(tool/'build/SMSQE'),str(folder),str(args.timeout),str(folder/'commands')]
    print('Running actual SMSQ/E'+(' + Turbo' if args.compile else ' interpreted')+': '+str(folder),flush=True)
    run=subprocess.run(command,capture_output=True,timeout=args.timeout+30);(folder/'emulator.log').write_bytes(run.stdout+run.stderr)
    errors=[]
    # Turbo's MANIFEST applies only to assignments on the same BASIC line.
    # An older SSB record expander split these into ordinary runtime variables.
    manifest_count=len(re.findall(r'^MANIFEST\s*:',(PARSER/'json.ssb').read_text('ascii'),re.M))
    if args.example:manifest_count+=len(re.findall(r'^MANIFEST\s*:',(PARSER/'pack.ssb').read_text('ascii'),re.M))
    translated=(folder/'output_bas').read_text('latin1') if (folder/'output_bas').exists() else ''
    manifests_preserved=len(re.findall(r'^\d+\s+MANIFEST\s*:',translated,re.M))==manifest_count
    if not manifests_preserved:errors.append('SSB must preserve MANIFEST and its assignments on the same BASIC line; update the local translator')
    if run.returncode:errors.append('emulator returned '+str(run.returncode))
    if (folder/'errorlog').exists():errors.append((folder/'errorlog').read_text('latin1'))
    if not (folder/'done').exists():errors.append('no completion marker')
    elif (folder/'done').read_text().strip()!='OK':errors.append('completion marker is not OK')
    if (folder/'result').exists():
        if args.example:
            pack=decoded((args.packs/'starter.json').read_bytes());pack=dict(pack);puzzles=pack['puzzles'];expected=['Puzzles: '+str(len(puzzles))]
            for puzzle in puzzles:
                puzzle=dict(puzzle);expected += [puzzle['id'],str(puzzle['width'])+' by '+str(puzzle['height'])+', pairs: '+str(len(puzzle['pairs']))]
                for pair in puzzle['pairs']:
                    pair=dict(pair);a=dict(pair['start']);b=dict(pair['finish']);expected += [pair['label']+': '+str(a['x'])+','+str(a['y'])+' -> '+str(b['x'])+','+str(b['y'])]
            actual=(folder/'result').read_text('latin1').splitlines()
            if [''.join(x.split()) for x in expected]!=[''.join(x.split()) for x in actual]:errors.append('Example output differs from starter pack IDs/dimensions/endpoints')
        else:errors+=verify(folder,cases,packs)
    else:errors.append('no result file')
    report={'passed':not errors,'compiled':args.compile,'example':args.example,'ramMiB':args.ram_mib,'caseCount':0 if args.example else len(cases),'packCount':1 if args.example else len(packs),'packNodes':{k:len(expected_nodes(v)) for k,v in packs.items()},'packSha256':{k:hashlib.sha256(v).hexdigest() for k,v in packs.items()},'librarySha256':hashlib.sha256((PARSER/'json.ssb').read_bytes()).hexdigest(),'manifestCount':manifest_count,'manifestsPreserved':manifests_preserved,'errors':errors[:30],'folder':str(folder),'testedAt':datetime.datetime.now(datetime.timezone.utc).isoformat()}
    (folder/'verification.json').write_text(json.dumps(report,indent=2));print(json.dumps(report,indent=2));return 0 if report['passed'] else 1
if __name__=='__main__':raise SystemExit(main())
