# clean-solution.ps1
# Löscht alle bin- und obj-Verzeichnisse in der Solution für einen sauberen Build.

$ErrorActionPreference = "Stop"

Write-Host "🧼 Starte Deep-Cleaning der Solution..." -ForegroundColor Cyan

$basePath = Resolve-Path ".."
$itemsRemoved = 0

Get-ChildItem -Path $basePath -Include bin,obj -Recurse -Directory | ForEach-Object {
    Write-Host "🧹 Lösche: $($_.FullName)" -ForegroundColor Gray
    Remove-Item -Path $_.FullName -Recurse -Force
    $itemsRemoved++
}

Write-Host "✅ Bereinigung abgeschlossen. $itemsRemoved Verzeichnisse entfernt." -ForegroundColor Green
