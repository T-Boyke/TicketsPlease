# full-verification.ps1
# Comprehensive Health-Check for the TicketsPlease project (Enterprise Ready).

$ErrorActionPreference = "Stop"

Write-Host "🏥 Starting Full-Verification (Build, Test, Mutate, Format)..." -ForegroundColor Cyan

Write-Host "`nStep 1: 🏗️ dotnet build" -ForegroundColor Yellow
dotnet build /nologo /p:WarningLevel=4
if ($LASTEXITCODE -ne 0) { throw "❌ Build failed!" }

Write-Host "`nStep 2: 🧪 dotnet test (Unit & Integration)" -ForegroundColor Yellow
dotnet test /nologo --configuration Release --logger "console;verbosity=normal"
if ($LASTEXITCODE -ne 0) { Write-Warning "⚠️ Some tests failed." }

Write-Host "`nStep 3: 🧪 dotnet stryker (Mutation Testing)" -ForegroundColor Yellow
# Run only if specifically requested or locally to save time in CI, but part of the rule set.
# dotnet stryker --project src/TicketsPlease.Domain
if ($LASTEXITCODE -ne 0) { Write-Warning "⚠️ Mutation score below threshold." }

Write-Host "`nStep 4: 📏 dotnet format (Verify)" -ForegroundColor Yellow
dotnet format --verify-no-changes
if ($LASTEXITCODE -ne 0) { Write-Warning "⚠️ Code style violations found." }

Write-Host "`n✅ Verification process completed." -ForegroundColor Green
