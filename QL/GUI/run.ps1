param(
    [string]$SsbRoot = 'C:\Source\Structured Super Basic',
    [string]$Pack,
    [ValidateRange(1,10000)][int]$Puzzle = 1
)
$ErrorActionPreference = 'Stop'
$qlArtifact = Join-Path $PSScriptRoot 'build\latest'
if (-not (Test-Path -LiteralPath (Join-Path $qlArtifact 'tangles_exe'))) {
    throw 'Build the player first: python QL/GUI/build.py --check'
}
$qlDesktop = Join-Path $PSScriptRoot 'build\desktop'
New-Item -ItemType Directory -Path $qlDesktop -Force | Out-Null
foreach ($qlName in @('tangles_exe','ttk','tptr_ext','boot','tangles_catalog')) {
    Copy-Item -LiteralPath (Join-Path $qlArtifact $qlName) -Destination (Join-Path $qlDesktop $qlName) -Force
}
foreach ($qlData in (Get-ChildItem -LiteralPath $qlArtifact -File -Filter 'pack??_json')) {
    Copy-Item -LiteralPath $qlData.FullName -Destination (Join-Path $qlDesktop $qlData.Name) -Force
}
if ($Pack) {
    if ($Pack -notmatch '^pack[0-9]{2}_json$' -or -not (Test-Path -LiteralPath (Join-Path $qlDesktop $Pack))) {
        throw 'Pack must name an available packNN_json file in the built catalogue.'
    }
    [IO.File]::WriteAllText((Join-Path $qlDesktop 'tangles_session'), "nfa1_$Pack`n$Puzzle`n", [Text.UTF8Encoding]::new($false))
}
$qlRom = (Join-Path $SsbRoot 'build\SMSQE').Replace('\','/')
$qlMount = $qlDesktop.Replace('\','/') + '/'
$qlIni = Join-Path $qlDesktop 'tangles.ini'
$qlSettings = @"
ROM_IMAGE_FILE=$qlRom
WDW_XSIZE=800
WDW_YSIZE=600
WDW-XPOS=80
WDW-YPOS=80
SCREEN-MODE=3
SCREEN-UPDATE-RATE=8
MEM_SIZE=16
WINDOW-MODE=0
MONITOR-VISIBLE=0
MENUBAR-VISIBLE=1
DOUBLE-SIZE=0
FAST-MODE=1
LANGUAGE=2
SOUND-VOLUME=0
LESS-CPU-WHEN-IDLE=1
DISABLE-WIN-DEVICE=1
DISABLE-FLP-DEVICE=1
DISABLE-NFA-DEVICE=0
DISABLE-SFA-DEVICE=0
NFA1=$qlMount
SFA1=$qlMount
NFA-FILENAME-CHANGE=0
SFA-FILENAME-CHANGE=0
"@
[IO.File]::WriteAllText($qlIni, $qlSettings, [Text.UTF8Encoding]::new($false))
$qlClassPath = (Join-Path $SsbRoot 'build\newclasses') + ';' + (Join-Path $SsbRoot 'build\emulator-lib\*') + ';' + (Join-Path $SsbRoot 'build\emulator-src')
$qlJava = (Get-Command javaw.exe -ErrorAction Stop).Source
$qlArguments = @('-cp',('"'+$qlClassPath+'"'),'smsqmulator.SMSQmulator',('"'+$qlIni+'"'))
$qlRunning = Start-Process -FilePath $qlJava -ArgumentList $qlArguments -WorkingDirectory $qlDesktop -WindowStyle Normal -RedirectStandardOutput (Join-Path $qlDesktop 'stdout.log') -RedirectStandardError (Join-Path $qlDesktop 'stderr.log') -PassThru
Write-Output "SMSQmulator process: $($qlRunning.Id)"
Write-Output 'At the QL prompt, enter: LRUN nfa1_boot'
Write-Output "Desktop files and progress: $qlDesktop"
