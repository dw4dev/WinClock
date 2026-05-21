@echo off
echo.
echo ====================================
echo   WinClock Release Build Tool
echo ====================================
echo.

set "OUTPUT_DIR=%~dp0build\publish"
set "PROJ=%~dp0WinClock.csproj"

echo [1/2] Cleaning old publish files...
if exist "%OUTPUT_DIR%" rmdir /s /q "%OUTPUT_DIR%"

echo [2/2] Publishing single-file executable (Release / win-x64)...
dotnet publish "%PROJ%" -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:EnableCompressionInSingleFile=true -p:DebugType=none -p:DebugSymbols=false -o "%OUTPUT_DIR%"

if %ERRORLEVEL% neq 0 (
    echo.
    echo [ERROR] Publish failed! Please check for compilation errors.
    pause
    exit /b 1
)

echo.
echo Publish completed successfully! Output directory:
echo   %OUTPUT_DIR%
echo.
explorer "%OUTPUT_DIR%"
