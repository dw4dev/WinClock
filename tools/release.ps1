# Publish WinClock as a single-file, self-contained exe (Release / win-x64) to build\publish.
# Keep this file ASCII-only so the console codepage never affects it.
# Top-level *.json files in build\publish (window-state.json, alarms.json) are user state and are kept.

$repoDir    = Split-Path -Parent (Split-Path -Parent $MyInvocation.MyCommand.Path)
$outputDir  = Join-Path $repoDir "build\publish"
$projFile   = Join-Path $repoDir "src\WinClock.csproj"

Write-Host ""
Write-Host "===================================="
Write-Host "   WinClock Release Builder"
Write-Host "===================================="
Write-Host ""

Write-Host "[1/2] Cleaning output directory (keeping *.json user state)..."
if (Test-Path -LiteralPath $outputDir) {
    Get-ChildItem -LiteralPath $outputDir -Force |
        Where-Object { $_.PSIsContainer -or $_.Extension -ne '.json' } |
        Remove-Item -Recurse -Force
}

Write-Host "[2/2] Publishing single-file executable (Release / win-x64)..."
dotnet publish $projFile -c Release -r win-x64 --self-contained true "-p:PublishSingleFile=true" "-p:EnableCompressionInSingleFile=true" "-p:DebugType=none" "-p:DebugSymbols=false" -o $outputDir

if ($LASTEXITCODE -ne 0) {
    Write-Host ""
    Write-Host "[ERROR] Publish failed." -ForegroundColor Red
    exit 1
}

Write-Host ""
Write-Host "Done! Output directory:" -ForegroundColor Green
Write-Host "  $outputDir" -ForegroundColor Cyan
foreach ($e in @(Get-ChildItem -LiteralPath $outputDir -File -Filter *.exe)) {
    Write-Host ("  {0}  {1}  {2:yyyy-MM-dd HH:mm}" -f $e.Name, $e.VersionInfo.ProductVersion, $e.LastWriteTime)
}
Write-Host ""
exit 0
