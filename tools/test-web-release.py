"""Exercise the packaged site in an isolated headless Chrome profile.

No existing browser windows, profiles or user storage are touched. Serve the
verified web extraction locally before running; screenshots stay in scratch.
"""
import argparse
import html
import json
from pathlib import Path
import re
import subprocess
import atexit
from http.server import SimpleHTTPRequestHandler, ThreadingHTTPServer
import threading

ROOT=Path(__file__).resolve().parents[1]
SCRATCH=Path('C:/Source/codex-workspace/.scratch/tangles-release')
HARNESS='''<!doctype html><meta charset="utf-8"><title>Tangles release test</title>
<style>html,body{margin:0}iframe{display:block;border:0}pre{position:fixed;right:0;bottom:0;max-width:90vw;white-space:pre-wrap;word-break:break-all;background:white;color:#123;font:10px monospace}</style>
<iframe id="game"></iframe><pre id="release-result">Running release checks</pre>
<script>
const f=document.getElementById('game'),r=document.getElementById('release-result');
const viewport=new URLSearchParams(location.search);const width=Number(viewport.get('width')),height=Number(viewport.get('height'));
f.style.width=width+'px';f.style.height=height+'px';
const wait=async(test)=>{for(let i=0;i<900;i++){if(test())return;await new Promise(x=>setTimeout(x,100));}throw Error('Timed out waiting for player');};
const result={passed:false,checks:[]};
const check=(v,name)=>{if(!v)throw Error(name);result.checks.push(name);};
(async()=>{try{
 localStorage.clear();f.src='index.html?pack=packs/quick-start.json';
 const doc=()=>f.contentDocument;
 await wait(()=>doc()?.querySelector('#hint-button')&&!doc().querySelector('#hint-button').disabled);
 check(doc().body.innerText.includes('Tangles VERSION'),'release version displayed after WebAssembly startup');
 check(f.contentWindow.innerWidth===width,'requested viewport width is used by the player');
 check(doc().querySelector('#pack-choice').options.length===12,'all twelve bundled packs are available');
 const connected=()=>doc().querySelectorAll('.pair.connected').length;
 const button=(name)=>Array.from(doc().querySelectorAll('button')).find(b=>b.textContent.includes(name));
 const before=connected();doc().querySelector('#hint-button').click();await wait(()=>connected()===before+1);
 check(true,'Hint connects exactly one colour');button('Undo').click();await wait(()=>connected()===before);
 check(true,'Undo reverses the whole hint');
 for(let i=0;i<20&&!doc().querySelector('.progress-card.solved');i++){doc().querySelector('#hint-button').click();await new Promise(x=>setTimeout(x,150));}
 await wait(()=>doc().querySelector('.progress-card.solved'));
 check(true,'quick-start puzzle completes with valid hints');
 button('Reset').click();await wait(()=>connected()===0);button('Undo').click();await wait(()=>doc().querySelector('.progress-card.solved'));
 check(true,'Reset is undoable');
 const old=doc();f.contentWindow.location.reload();await wait(()=>doc()!==old&&doc().querySelector('.progress-card.solved'));
 check(true,'completed progress survives reload');
 const select=doc().querySelector('#pack-choice');
 select.value=Array.from(select.options).find(o=>o.textContent.includes('Full coverage collection')).value;
 select.dispatchEvent(new f.contentWindow.Event('change',{bubbles:true}));
 await wait(()=>doc().querySelector('.lede')?.textContent.includes('fill every square'));
 check(doc().querySelector('.progress-card').textContent.includes('squares filled'),'full-coverage rule and fill count are shown');
 check(doc().documentElement.scrollWidth<=f.contentWindow.innerWidth+1,'layout fits the viewport without horizontal overflow');
 check(doc().querySelector('a[href="release-notes.txt"]')!==null,'release notes are linked');
 check(doc().querySelector('a[href="third-party/notices.txt"]')!==null,'third-party notices are linked');
 result.passed=true;
}catch(e){result.error=String(e);}r.textContent=JSON.stringify(result);r.dataset.finished='yes';if(result.passed)r.style.display='none';})();
</script>'''


def main():
    p=argparse.ArgumentParser(description=__doc__)
    p.add_argument('--url',help='Existing preview URL; otherwise serve the extraction at /Tangles/ on a temporary loopback port')
    p.add_argument('--browser',type=Path,default=Path('C:/Program Files/Google/Chrome/Application/chrome.exe'))
    a=p.parse_args();version=(ROOT/'VERSION').read_text().strip()
    web=SCRATCH/'verify-web';(web/'__release-smoke.html').write_text(HARNESS.replace('VERSION',version),encoding='utf-8')
    if not a.url:
        class Handler(SimpleHTTPRequestHandler):
            def __init__(self,*args,**kwargs):super().__init__(*args,directory=str(web),**kwargs)
            def do_GET(self):
                if not self.path.startswith('/Tangles/'):
                    self.send_error(404);return
                self.path=self.path[len('/Tangles'):]
                super().do_GET()
            def log_message(self,*args):pass
        server=ThreadingHTTPServer(('127.0.0.1',0),Handler)
        threading.Thread(target=server.serve_forever,daemon=True).start()
        atexit.register(lambda:(server.shutdown(),server.server_close()))
        a.url=f'http://127.0.0.1:{server.server_port}/Tangles/'
    results=[]
    for name,size in [('desktop','1365,900'),('mobile','390,844')]:
        with (SCRATCH/f'web-{name}.log').open('w') as log:
            run=subprocess.run([str(a.browser),'--headless=new','--no-first-run',
                '--user-data-dir='+str(SCRATCH/'browser-profile'), '--window-size='+size,
                '--screenshot='+str(SCRATCH/f'web-{name}.png'),'--dump-dom','--virtual-time-budget=120000',
                a.url+'__release-smoke.html?width='+size.split(',')[0]+'&height='+size.split(',')[1]],stdout=subprocess.PIPE,stderr=log,timeout=150,check=True)
        dom=run.stdout.decode('utf-8','replace');(SCRATCH/f'web-{name}.html').write_text(dom,encoding='utf-8')
        match=re.search(r'<pre id="release-result"[^>]*>(.*?)</pre>',dom,re.S)
        if not match:raise AssertionError('Missing browser result; see '+str(SCRATCH/f'web-{name}.html'))
        result=json.loads(html.unescape(match[1]));result['viewport']=size;result['screenshot']=str(SCRATCH/f'web-{name}.png');result['deploymentPath']='/Tangles/' if '/Tangles/' in a.url else '/'
        if not result['passed']:raise AssertionError(result)
        results.append(result);print(name,'PASS',len(result['checks']),'browser checks',flush=True)
    reportfile=ROOT/'artifacts/release'/version/'release-verification.json'
    report=json.loads(reportfile.read_text());report['browserSmoke']=results
    reportfile.write_text(json.dumps(report,indent=2)+'\n')
    print(json.dumps(results,indent=2))


if __name__=='__main__':main()
