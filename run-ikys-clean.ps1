# IKYS Clean Startup Script
Write-Host "================================================" -ForegroundColor Green
Write-Host "  IKYS - Insan Kaynaklari Yonetim Sistemi" -ForegroundColor Green  
Write-Host "  Clean Startup Script" -ForegroundColor Green
Write-Host "================================================" -ForegroundColor Green
Write-Host ""

# Step 1: Kill all processes
Write-Host "1. Stopping all running processes..." -ForegroundColor Yellow
Get-Process | Where-Object { $_.ProcessName -like "*dotnet*" -or $_.ProcessName -like "*MVC*" } | Stop-Process -Force -ErrorAction SilentlyContinue
Start-Sleep -Seconds 2

# Step 2: Clean build artifacts
Write-Host "2. Cleaning build artifacts..." -ForegroundColor Yellow
Remove-Item -Path ".\bin" -Recurse -Force -ErrorAction SilentlyContinue
Remove-Item -Path ".\obj" -Recurse -Force -ErrorAction SilentlyContinue
Get-ChildItem -Path . -Recurse -Directory | Where-Object { $_.Name -eq "bin" -or $_.Name -eq "obj" } | Remove-Item -Recurse -Force -ErrorAction SilentlyContinue

# Step 3: Clean dotnet
Write-Host "3. Cleaning dotnet cache..." -ForegroundColor Yellow
dotnet clean | Out-Null

# Step 4: Restore packages
Write-Host "4. Restoring packages..." -ForegroundColor Yellow
dotnet restore | Out-Null

# Step 5: Build
Write-Host "5. Building application..." -ForegroundColor Yellow
dotnet build --no-restore --verbosity quiet
if ($LASTEXITCODE -ne 0) {
    Write-Host "Build failed! Trying with warnings suppressed..." -ForegroundColor Yellow
    dotnet build --no-restore --verbosity minimal
    if ($LASTEXITCODE -ne 0) {
        Write-Host "Build failed! Please check errors above." -ForegroundColor Red
        Read-Host "Press Enter to exit"
        exit 1
    }
}

# Step 6: Update database
Write-Host "6. Updating database..." -ForegroundColor Yellow
Set-Location MVC
dotnet ef database update | Out-Null
Set-Location ..

# Step 7: Start application
Write-Host "7. Starting application..." -ForegroundColor Yellow
Write-Host ""
Write-Host "Application will start on: http://localhost:8080" -ForegroundColor Green
Write-Host "Login credentials:" -ForegroundColor Cyan
Write-Host "  Admin: admin@ikys.com / Admin123!" -ForegroundColor Cyan
Write-Host "  Manager: manager@ikys.com / Manager123!" -ForegroundColor Cyan  
Write-Host "  Employee: employee@ikys.com / Employee123!" -ForegroundColor Cyan
Write-Host ""
Write-Host "Opening browser in 3 seconds..." -ForegroundColor Yellow
Start-Sleep -Seconds 3

# Open browser to welcome page
Start-Process "http://localhost:8080/Home/Welcome"

# Start the application
Write-Host "Starting IKYS application..." -ForegroundColor Green
dotnet run --project MVC/MVC.csproj --urls "http://localhost:8080"
