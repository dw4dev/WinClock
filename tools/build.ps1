#Requires -Version 5.1
<#
.SYNOPSIS
    一鍵編譯本專案的方案檔（只編譯，不執行測試）。

.DESCRIPTION
    通用腳本：自動尋找 repo 內的 .sln／.slnx（先找根目錄，再往下找 2 層）。
    找到多個方案檔時，使用最靠近根目錄的那一個；要指定時用 -Solution。
    測試請用 run-tests.ps1。

.EXAMPLE
    .\tools\build.ps1                          # Debug 編譯
    .\tools\build.ps1 -Configuration Release   # Release 編譯
    .\tools\build.ps1 -Clean                   # 先 dotnet clean 再編譯
    .\tools\build.ps1 -Solution src\App.sln    # 指定方案檔（相對於 repo 根目錄）

.PARAMETER Configuration Debug（預設）或 Release
.PARAMETER Clean         編譯前先 dotnet clean
.PARAMETER Solution      方案檔路徑（相對於 repo 根目錄，或絕對路徑）
#>
param(
    [ValidateSet('Debug', 'Release')]
    [string]$Configuration = 'Debug',
    [switch]$Clean,
    [string]$Solution = ''
)

$ErrorActionPreference = 'Stop'
$root = Split-Path -Parent (Split-Path -Parent $MyInvocation.MyCommand.Path)

if ($Solution) {
    if ([System.IO.Path]::IsPathRooted($Solution)) { $sln = $Solution } else { $sln = Join-Path $root $Solution }
    if (-not (Test-Path -LiteralPath $sln)) {
        Write-Host "找不到方案檔：$sln" -ForegroundColor Red
        exit 1
    }
} else {
    $found = @(Get-ChildItem -LiteralPath $root -Recurse -Depth 2 -File -ErrorAction SilentlyContinue |
        Where-Object { ($_.Extension -eq '.sln' -or $_.Extension -eq '.slnx') -and $_.FullName -notmatch '\\(bin|obj|\.git|\.vs)\\' } |
        Sort-Object @{ Expression = { $_.FullName.Split('\').Count } }, FullName)

    if ($found.Count -eq 0) {
        Write-Host "找不到方案檔（.sln／.slnx）：$root" -ForegroundColor Red
        exit 1
    }
    if ($found.Count -gt 1) {
        Write-Host '找到多個方案檔，使用第 1 個。要指定時用 -Solution：' -ForegroundColor Yellow
        foreach ($f in $found) { Write-Host ("  " + $f.FullName.Substring($root.Length + 1)) -ForegroundColor Yellow }
    }
    $sln = $found[0].FullName
}

Write-Host "==> 方案檔：$sln" -ForegroundColor Cyan

# 切到方案檔所在資料夾，讓 dotnet 找到 global.json 與 Directory.Build.props。
Push-Location -LiteralPath (Split-Path -Parent $sln)
try {
    if ($Clean) {
        Write-Host "== dotnet clean ($Configuration) ==" -ForegroundColor Cyan
        dotnet clean $sln -c $Configuration -v q --nologo
        if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }
    }

    Write-Host "== dotnet build ($Configuration) ==" -ForegroundColor Cyan
    dotnet build $sln -c $Configuration -v minimal --nologo
    $code = $LASTEXITCODE
} finally {
    Pop-Location
}

Write-Host ''
if ($code -eq 0) {
    Write-Host "==> 編譯成功（$Configuration）" -ForegroundColor Green
} else {
    Write-Host "==> 編譯失敗（exit $code）" -ForegroundColor Red
}
exit $code
