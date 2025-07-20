@echo off
echo.
echo ===============================================
echo  Syncler Windows - Build Script
echo ===============================================
echo.

REM Check if .NET 8.0 SDK is installed
dotnet --version >nul 2>&1
if %errorlevel% neq 0 (
    echo ERROR: .NET 8.0 SDK is not installed or not in PATH
    echo Please install .NET 8.0 SDK from: https://dotnet.microsoft.com/download/dotnet/8.0
    pause
    exit /b 1
)

echo [1/4] Checking .NET SDK version...
dotnet --version

echo.
echo [2/4] Restoring NuGet packages...
dotnet restore
if %errorlevel% neq 0 (
    echo ERROR: Failed to restore NuGet packages
    pause
    exit /b 1
)

echo.
echo [3/4] Building application (Release)...
dotnet build --configuration Release --no-restore
if %errorlevel% neq 0 (
    echo ERROR: Build failed
    pause
    exit /b 1
)

echo.
echo [4/4] Build completed successfully!
echo.
echo You can now run the application with:
echo   dotnet run --project SynclerWindows --configuration Release
echo.
echo Or open the solution in Visual Studio 2022:
echo   start SynclerWindows.sln
echo.

pause