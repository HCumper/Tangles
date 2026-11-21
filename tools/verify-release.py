"""Check ZIPs, extract clean test copies and launch the actual QL package."""
import argparse
import hashlib
import json
from pathlib import Path
import shutil
import struct
import subprocess
import zipfile

ROOT=Path(__file__).resolve().parents[1]
SCRATCH=Path('C:/Source/codex-workspace/.scratch/tangles-release')


def extract_owned(archive, target):
    target=target.resolve()
    if not target.is_relative_to(SCRATCH.resolve()):
        raise ValueError('Verification output must stay in the release scratch directory')
    marker=target/'.tangles-release-verification'
    if target.exists():
        if not marker.exists():
            raise ValueError('Refusing to clean an unowned directory: '+str(target))
        shutil.rmtree(target)
    target.mkdir(parents=True)
    marker.touch()
    with zipfile.ZipFile(archive) as z:
        if z.testzip() is not None:
            raise ValueError('ZIP CRC failure')
        for entry in z.infolist():
            if not (target/entry.filename).resolve().is_relative_to(target):
                raise ValueError('Archive path escapes its extraction directory')
        z.extractall(target)


def main():
    parser=argparse.ArgumentParser(description=__doc__)
    parser.add_argument('--skip-ql',action='store_true')
    parser.add_argument('--ssb-root',type=Path,default=Path('C:/Source/Structured Super Basic'))
    a=parser.parse_args()
    version=(ROOT/'VERSION').read_text().strip()
    out=ROOT/'artifacts/release'/version
    for line in (out/'SHA256SUMS.txt').read_text().splitlines():
        expected,name=line.split('  ',1)
        if hashlib.sha256((out/name).read_bytes()).hexdigest()!=expected:
            raise ValueError('Checksum mismatch: '+name)
    ql=SCRATCH/'verify-ql';web=SCRATCH/'verify-web'
    extract_owned(out/f'tangles-{version}-ql.zip',ql)
    extract_owned(out/f'tangles-{version}-web.zip',web)
    with zipfile.ZipFile(out/f'tangles-{version}-ql.zip') as z:
        for entry in z.infolist():
            if entry.create_system!=12 or entry.extra[4:12]!=b'QDOS02\0\0':
                raise ValueError('Missing QDOS metadata: '+entry.filename)
            if struct.unpack_from('>I',entry.extra,12)[0]!=entry.file_size:
                raise ValueError('Wrong QDOS file length: '+entry.filename)
        attributes=z.getinfo('tangles_exe').extra[16:26]
        host=z.read('tangles_sfa_exe')
        offset=struct.unpack_from('>H',host,18)[0]*2
        if attributes!=host[20:30] or z.read('tangles_exe')!=host[offset:]:
            raise ValueError('Native and host executable forms differ')
    for path in ['release-notes.txt','third-party/notices.txt','COPYRIGHT.txt','LICENSE.txt']:
        if not (web/path).is_file():raise ValueError('Missing web release document: '+path)
    core=SCRATCH/'dotnet/bin/Tangles.Core/release/Tangles.Core.dll'
    subprocess.run(['dotnet','fsi','--reference:'+str(core),str(ROOT/'tools/ValidateReleasePacks.fsx'),
                    '--',str(web)],cwd=ROOT,check=True)
    report={'version':version,
            'artifacts':{p.name:hashlib.sha256(p.read_bytes()).hexdigest() for p in out.glob('*.zip')},
            'zipChecksums':True,'qdosMetadata':True,'matchingExecutableForms':True,
            'allPackWitnesses':True,'qlPackageSmoke':None,'browserSmoke':None}
    if not a.skip_ql:
        classes=SCRATCH/'classes';classes.mkdir(exist_ok=True)
        cp=str(ROOT/'QL/GUI/build/latest')+';'+str(a.ssb_root/'build/newclasses')+';'+str(a.ssb_root/'build/emulator-lib/*')
        subprocess.run(['javac','-cp',cp,'-d',str(classes),str(ROOT/'tools/ReleaseCheck.java')],check=True)
        with (SCRATCH/'package-smoke.log').open('w') as log:
            subprocess.run(['java','-cp',str(classes)+';'+cp,'ReleaseCheck',str(a.ssb_root/'build/SMSQE'),str(ql)],
                           stdout=log,stderr=subprocess.STDOUT,check=True,timeout=240)
        report['qlPackageSmoke']=json.loads((ql/'package-smoke.json').read_text())
    (out/'release-verification.json').write_text(json.dumps(report,indent=2)+'\n')
    print(json.dumps(report,indent=2))
    print('Extracted web preview:',web)


if __name__=='__main__':main()
