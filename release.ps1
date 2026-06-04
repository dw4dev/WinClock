$projectDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$outputDir  = Join-Path $projectDir "build\publish"
$projFile   = Join-Path $projectDir "src\WinClock.csproj"

Write-Host ""
Write-Host "===================================="
Write-Host "   WinClock Release Builder"
Write-Host "===================================="
Write-Host ""

Write-Host "[1/2] Cleaning output directory..."
if (Test-Path $outputDir) {
    Remove-Item $outputDir -Recurse -Force
}

Write-Host "[2/2] Publishing single-file executable (Release / win-x64)..."
dotnet publish $projFile -c Release -r win-x64 --self-contained true "-p:PublishSingleFile=true" "-p:EnableCompressionInSingleFile=true" "-p:DebugType=none" "-p:DebugSymbols=false" -o $outputDir

if ($LASTEXITCODE -ne 0) {
    Write-Host ""
    Write-Host "[ERROR] Publish failed." -ForegroundColor Red
    Read-Host "Press Enter to exit"
    exit 1
}

Write-Host ""
Write-Host "Done! Output directory:" -ForegroundColor Green
Write-Host "  $outputDir" -ForegroundColor Cyan
Write-Host ""
Start-Process explorer.exe $outputDir
