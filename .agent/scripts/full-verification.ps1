# full-verification.ps1
# Kombinierter Gesundheitscheck für das gesamte Projekt.

$ErrorActionPreference = "Continue"

Write-Host "🏥 Starte Full-Verification (Build, Test, Format)..." -ForegroundColor Cyan

Write-Host "`nStep 1: 🏗️ dotnet build" -ForegroundColor Yellow
dotnet build /nologo

if ($LASTEXITCODE -ne 0) {
    Write-Error "❌ Build fehlgeschlagen!"
    exit $LASTEXITCODE
}

Write-Host "`nStep 2: 🧪 dotnet test" -ForegroundColor Yellow
dotnet test /nologo

if ($LASTEXITCODE -ne 0) {
    Write-Warning "⚠️ Einige Tests sind fehlgeschlagen."
}

Write-Host "`nStep 3: 📏 dotnet format (Verify)" -ForegroundColor Yellow
dotnet format --verify-no-changes

Write-Host "`n✅ Verification-Prozess beendet." -ForegroundColor Green
