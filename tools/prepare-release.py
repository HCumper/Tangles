"""Build and package both players; never commit, tag, push or publish.

Outputs use the repository's existing artifacts tree. Temporary tests/builds
reuse the task scratch directory. Native ZIP entries retain QDOS attributes;
the companion SFA executable supports ordinary host-folder extraction.
"""
import argparse
import hashlib
import json
from pathlib import Path
import re
import shutil
import struct
import subprocess
import zipfile
from urllib.parse import urlsplit
import xml.etree.ElementTree as ET

ROOT = Path(__file__).resolve().parents[1]
SCRATCH = Path('C:/Source/codex-workspace/.scratch/tangles-release')
STAMP = (2026, 11, 21, 0, 0, 0)
ARCHIVES = {
    'turbo-toolkit.zip': '4590458257ebfd2690dfc9581acdf098e62b546b361fb34bebdde993c98f3e27',
    'turboptr.zip': '930d4f9740d0e1d4d03abea4386f502aee49b0447f0410e06a1f6b75c6d6f51c',
}


def digest(data):
    return hashlib.sha256(data).hexdigest()


def run(*args):
    subprocess.run(list(map(str, args)), cwd=ROOT, check=True)


def qdos_extra(name, data, attributes=bytes(10)):
    encoded = name.encode('ascii')
    if len(encoded) > 36:
        raise ValueError('QL filename exceeds 36 bytes: ' + name)
    header = bytearray(64)
    struct.pack_into('>I', header, 0, len(data))
    header[4:14] = attributes
    struct.pack_into('>H', header, 14, len(encoded))
    header[16:16 + len(encoded)] = encoded
    return struct.pack('<HH', 0xFB4A, 72) + b'QDOS02\0\0' + header


def zip_files(target, entries, qdos=False):
    with zipfile.ZipFile(target, 'w') as archive:
        for name, data, attributes in sorted(entries):
            info = zipfile.ZipInfo(name, STAMP)
            info.compress_type = zipfile.ZIP_DEFLATED
            if qdos:
                info.create_system = 12
                info.extra = qdos_extra(name, data, attributes)
            archive.writestr(info, data)
    with zipfile.ZipFile(target) as archive:
        if archive.testzip() is not None:
            raise ValueError('ZIP CRC verification failed: ' + str(target))


def main():
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument('--skip-build', action='store_true', help='Package already verified, matching current builds')
    args = parser.parse_args()
    version = (ROOT/'VERSION').read_text().strip()
    if not re.fullmatch(r'\d+\.\d+(?:\.\d+)?(?:-[a-zA-Z0-9.]+)?', version):
        raise ValueError('Invalid release version')
    if f'guiRelease$="{version}"' not in (ROOT/'QL/GUI/version.ssb').read_text():
        raise ValueError('QL/GUI/version.ssb does not match VERSION')
    out = ROOT/'artifacts/release'/version
    out.mkdir(parents=True, exist_ok=True)
    if not args.skip_build:
        run('dotnet', 'test', 'Tangles.slnx', '-c', 'Release', '--artifacts-path', SCRATCH/'dotnet',
            '--disable-build-servers', '--logger', 'trx', '--results-directory', SCRATCH/'tests')
        run('dotnet', 'publish', 'src/Tangles.Player/Tangles.Player.fsproj', '-c', 'Release',
            '--artifacts-path', SCRATCH/'dotnet', '--output', SCRATCH/'web-publish', '--disable-build-servers')
        run('python', 'QL/GUI/build.py', '--turboptr', 'QL/GUI/build/latest/tptr_ext', '--check')
    web = SCRATCH/'web-publish/wwwroot'
    ql = ROOT/'QL/GUI/build/latest'
    build = json.loads((ql/'build.json').read_text())
    for name, expected in build['sourceSha256'].items():
        source = ROOT/'QL'/Path(name.replace('\\', '/'))
        if digest(source.read_bytes()) != expected:
            raise ValueError('QL build is stale: ' + str(source))
    exe = (ql/'tangles_exe').read_bytes()
    if digest(exe) != build['executableSha256']:
        raise ValueError('QL executable does not match its build manifest')
    if not exe.startswith(b']!QDOS File Header'):
        raise ValueError('QL executable lacks its Qemu/SFA header')
    offset = struct.unpack_from('>H', exe, 18)[0]*2
    if offset < 30 or offset >= len(exe) or exe[21] != 1:
        raise ValueError('Invalid QL executable header')
    native, attributes = exe[offset:], exe[20:30]
    if struct.unpack_from('>I', attributes, 2)[0] <= 0:
        raise ValueError('QL executable has no reserved dataspace')
    smoke = json.loads((ql/'smoke.json').read_text())
    if not smoke.get('passed') or smoke.get('executableSha256') != build['executableSha256']:
        raise ValueError('QL pointer integration did not pass')
    for name, expected in ARCHIVES.items():
        if digest((ROOT/'third-party/ql'/name).read_bytes()) != expected:
            raise ValueError('Third-party archive differs: ' + name)
    for archive_name, original, runtime in [('turbo-toolkit.zip', 'turbo_sms.code', 'ttk'),
                                             ('turboptr.zip', 'tptr.ext', 'tptr_ext')]:
        with zipfile.ZipFile(ROOT/'third-party/ql'/archive_name) as archive:
            if archive.read(original) != (ql/runtime).read_bytes():
                raise ValueError('Runtime differs from its original distribution: ' + runtime)
    for name in ['index.html', '_framework/blazor.webassembly.js', 'release-notes.txt', 'LICENSE.txt', 'COPYRIGHT.txt',
                 'third-party/notices.txt', 'third-party/Elmish-LICENSE.txt']:
        if not (web/name).is_file():
            raise ValueError('Missing published web file: ' + name)
    web_catalogue = json.loads((web/'packs.json').read_text())['packs']
    catalogue = (ql/'tangles_catalog').read_text().splitlines()
    count = int(catalogue[0])
    if len(catalogue) != 1+2*count or count != len(web_catalogue):
        raise ValueError('Pack catalogues differ')
    packs = []
    for i, item in enumerate(web_catalogue):
        name = catalogue[2+2*i]
        data = (ql/name).read_bytes()
        if data != (web/urlsplit(item['path']).path).read_bytes():
            raise ValueError('Web/QL pack content differs: ' + name)
        pack = json.loads(data)
        packs.append({'name': item['name'], 'qlFile': name, 'webFile': item['path'],
                      'puzzles': len(pack['puzzles']), 'sha256': digest(data)})
    zero = bytes(10)
    entries = [('tangles_exe', native, attributes), ('tangles_sfa_exe', exe, zero)]
    entries += [(n, (ql/n).read_bytes(), zero) for n in ['ttk', 'tptr_ext', 'tangles_catalog']+catalogue[2::2]]
    for source, target in [('QL/GUI/boot.bas', 'boot'), ('QL/GUI/start.bas', 'start'),
                           ('docs/QL-Install.txt', 'readme_txt'),
                           (f'docs/releases/{version}.md', 'release_notes_txt'),
                           ('COPYRIGHT.txt', 'copyright_txt'), ('THIRD-PARTY-NOTICES.md', 'notices_txt'),
                           ('LICENSE', 'licence_txt'),
                           ('third-party/ql/turbo-toolkit.zip', 'turbo_toolkit_zip'),
                           ('third-party/ql/turboptr.zip', 'turboptr_zip')]:
        entries.append((target, (ROOT/source).read_bytes(), zero))
    zip_files(out/f'tangles-{version}-ql.zip', entries, qdos=True)
    web_entries = [(p.relative_to(web).as_posix(), p.read_bytes(), zero) for p in web.rglob('*') if p.is_file()]
    web_entries += [('README.txt', (ROOT/'docs/Web-Install.txt').read_bytes(), zero)]
    zip_files(out/f'tangles-{version}-web.zip', web_entries)
    names = subprocess.check_output(['git', '-C', str(ROOT), 'ls-files', '-z', '--cached', '--others', '--exclude-standard'])
    allowed = {'src', 'tests', 'tools', 'docs', 'QL', 'puzzles', '.github', 'third-party'}
    source_entries = []
    for name in sorted(set(names.decode().split('\0'))-{''}):
        p = ROOT/name
        if p.is_file() and (Path(name).parts[0] in allowed or '/' not in name):
            source_entries.append((name, p.read_bytes(), zero))
    zip_files(out/f'tangles-{version}-source.zip', source_entries)
    tree_hash = digest('\n'.join(n+' '+digest(d) for n,d,_ in source_entries).encode())
    trx = max((SCRATCH/'tests').glob('*.trx'), key=lambda p: p.stat().st_mtime)
    counters = ET.parse(trx).getroot().find('.//{*}Counters').attrib
    tests = {name: int(counters[name]) for name in ['total', 'passed', 'failed']}
    if tests['failed'] or tests['total'] == 0:
        raise ValueError('Shared test results did not pass')
    manifest = {'version': version, 'status': 'release', 'releaseTag': 'v'+version,
                'sourceCommit': subprocess.check_output(['git', '-C', str(ROOT), 'rev-parse', 'HEAD']).decode().strip(),
                'sourceHasUncommittedChanges': bool(subprocess.check_output(['git', '-C', str(ROOT), 'status', '--porcelain'])),
                'sourceTreeSha256': tree_hash, 'qlBuild': build,
                'qlPointerCheck': smoke, 'packs': packs,
                'qlFiles': [{'name': n, 'sha256': digest(d), 'bytes': len(d), 'qdosType': a[1],
                             'dataspace': struct.unpack_from('>I', a, 2)[0]} for n,d,a in entries],
                'licence': 'MIT for Tangles code, documentation and bundled puzzle packs; separate third-party terms apply',
                'sharedTests': tests,
                'tests': 'Shared F# test counters and current QL pointer check attached. See release-verification.json for package checks.'}
    (out/'release-manifest.json').write_text(json.dumps(manifest, indent=2)+'\n')
    shutil.copy2(ROOT/f'docs/releases/{version}.md', out/'RELEASE-NOTES.md')
    assets = sorted(p for p in out.iterdir() if p.suffix == '.zip' or p.name in ['release-manifest.json', 'RELEASE-NOTES.md'])
    (out/'SHA256SUMS.txt').write_text(''.join(digest(p.read_bytes())+'  '+p.name+'\n' for p in assets))
    print(f'Prepared {version}: {count} packs, {sum(p["puzzles"] for p in packs)} puzzles')
    print(out)


if __name__ == '__main__':
    main()
