@echo off
rem Wrapper for run-tests.ps1 (bypasses PowerShell execution policy).
rem Usage: run-tests.bat [-Filter <name>] [-Configuration Debug|Release]
rem Keep this file ASCII-only to avoid codepage issues; chcp 65001 makes
rem the UTF-8 Chinese output from the ps1 display correctly when redirected.

chcp 65001 >nul
powershell -NoProfile -ExecutionPolicy Bypass -File "%~dp0run-tests.ps1" %*
set EXITCODE=%ERRORLEVEL%

rem Pause only when launched by double-click (so the window doesn't vanish).
echo %cmdcmdline% | find /i "%~f0" >nul && pause

exit /b %EXITCODE%
