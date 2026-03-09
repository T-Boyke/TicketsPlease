# db-management.ps1
# Hilfsskript für EF Core Datenbank-Operationen.

param(
    [Parameter(Mandatory=$false)]
    [switch]$Reset
)

$ErrorActionPreference = "Stop"

$INFRA_PROJECT = "src/TicketsPlease.Infrastructure"
$WEB_PROJECT = "src/TicketsPlease.Web"

if ($Reset) {
    Write-Host "⚠️ Datenbank-Reset angefordert..." -ForegroundColor Yellow
    # Hier könnte ein Drop-Database Befehl stehen, falls nötig.
    # Für das MVP beschränken wir uns auf ein sauberes Update.
}

Write-Host "🚀 Führe EF Core Database Update aus..." -ForegroundColor Cyan

dotnet ef database update --project $INFRA_PROJECT --startup-project $WEB_PROJECT

Write-Host "✅ Datenbank ist auf dem neuesten Stand." -ForegroundColor Green
