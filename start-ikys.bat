@echo off
echo ===============================================
echo   IKYS - Insan Kaynaklari Yonetim Sistemi
echo   Simple Startup Script
echo ===============================================

echo 1. Stopping any running processes...
taskkill /f /im dotnet.exe >nul 2>&1
taskkill /f /im MVC.exe >nul 2>&1
timeout /t 2 >nul

echo 2. Cleaning build artifacts...
if exist bin rmdir /s /q bin >nul 2>&1
if exist obj rmdir /s /q obj >nul 2>&1
dotnet clean >nul 2>&1

echo 3. Building application...
dotnet build --no-restore --verbosity minimal

if %ERRORLEVEL% NEQ 0 (
    echo Build failed! Please check errors above.
    pause
    exit /b 1
)

echo 4. Starting IKYS application...
echo.
echo ===============================================
echo   Application URL: http://localhost:8080
echo   Welcome Page: http://localhost:8080/Home/Welcome
echo   Login Page: http://localhost:8080/Auth/Login
echo ===============================================
echo   Test Credentials:
echo   Admin: admin@ikys.com / Admin123!
echo   Manager: manager@ikys.com / Manager123!
echo   Employee: employee@ikys.com / Employee123!
echo ===============================================
echo.
echo Opening browser in 3 seconds...
timeout /t 3 >nul

start http://localhost:8080/Home/Welcome
echo Starting application... (Press Ctrl+C to stop)
dotnet run --project MVC/MVC.csproj --urls "http://localhost:8080"
