@echo off
rem Wrapper for release.ps1 (bypasses PowerShell execution policy).
rem Publishes a single-file, self-contained exe (Release / win-x64) to build\publish.
rem Top-level *.json files in build\publish (user state) are kept.

chcp 65001 >nul
powershell -NoProfile -ExecutionPolicy Bypass -File "%~dp0release.ps1" %*
set EXITCODE=%ERRORLEVEL%

rem Open the output folder and pause only when launched by double-click.
echo %cmdcmdline% | find /i "%~f0" >nul && (
    rem if %EXITCODE%==0 explorer "%~dp0..\build\publish"
    pause
)

exit /b %EXITCODE%
