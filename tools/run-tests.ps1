#Requires -Version 5.1
<#
.SYNOPSIS
    一鍵執行本專案的測試（只執行測試專案，和 build.ps1 分開）。

.DESCRIPTION
    通用腳本：自動尋找 repo 內檔名結尾是 Tests.csproj 的測試專案，逐一執行 dotnet test。
    可用 -Filter 只執行符合的測試（對應 dotnet test --filter）。
    repo 內沒有測試專案時，顯示提示並結束（exit 0）。

.EXAMPLE
    .\tools\run-tests.ps1                          # 執行全部測試
    .\tools\run-tests.ps1 -Filter ParserTests      # 只執行名稱符合的測試
    .\tools\run-tests.ps1 -Configuration Release

.PARAMETER Filter        測試名稱過濾字串
.PARAMETER Configuration Debug（預設）或 Release
#>
param(
    [string]$Filter = '',
    [ValidateSet('Debug', 'Release')]
    [string]$Configuration = 'Debug'
)

$ErrorActionPreference = 'Stop'
$root = Split-Path -Parent (Split-Path -Parent $MyInvocation.MyCommand.Path)

$projects = @(Get-ChildItem -LiteralPath $root -Recurse -File -Filter '*Tests.csproj' -ErrorAction SilentlyContinue |
    Where-Object { $_.FullName -notmatch '\\(bin|obj|\.git|\.vs)\\' } |
    Sort-Object FullName)

if ($projects.Count -eq 0) {
    Write-Host "這個專案還沒有測試專案（找不到 *Tests.csproj）：$root" -ForegroundColor Yellow
    exit 0
}

Write-Host "==> Configuration：$Configuration" -ForegroundColor Cyan
if ($Filter) { Write-Host "==> Filter：$Filter" -ForegroundColor Cyan }

$exit = 0
foreach ($project in $projects) {
    Write-Host "==> 測試專案：$($project.Name)" -ForegroundColor Cyan

    $dotnetArgs = @('test', $project.FullName, '-c', $Configuration, '--nologo', '--verbosity', 'minimal')
    if ($Filter) { $dotnetArgs += @('--filter', $Filter) }

    # 切到測試專案所在資料夾，讓 dotnet 找到 global.json 與 Directory.Build.props。
    Push-Location -LiteralPath $project.DirectoryName
    try {
        & dotnet @dotnetArgs
        if ($LASTEXITCODE -ne 0) { $exit = $LASTEXITCODE }
    } finally {
        Pop-Location
    }
}

Write-Host ''
if ($exit -eq 0) {
    Write-Host '==> 測試全數通過' -ForegroundColor Green
} else {
    Write-Host "==> 測試失敗（exit $exit）" -ForegroundColor Red
}
exit $exit
