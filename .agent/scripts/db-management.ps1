# db-management.ps1
# Advanced EF Core Database Management Script.

param(
    [Parameter(Mandatory=$false)]
    [switch]$Reset,
    [Parameter(Mandatory=$false)]
    [string]$MigrationName
)

$ErrorActionPreference = "Stop"

$INFRA_PROJECT = "src/TicketsPlease.Infrastructure"
$WEB_PROJECT = "src/TicketsPlease.Web"

if ($Reset) {
    Write-Host "⚠️ DESTRUCTIVE ACTION: Dropping & Recreating Database..." -ForegroundColor Red
    dotnet ef database drop --project $INFRA_PROJECT --startup-project $WEB_PROJECT --force --context AppDbContext
    dotnet ef database update --project $INFRA_PROJECT --startup-project $WEB_PROJECT --context AppDbContext
}
elseif ($MigrationName) {
    Write-Host "➕ Adding new migration: $MigrationName..." -ForegroundColor Cyan
    dotnet ef migrations add $MigrationName --project $INFRA_PROJECT --startup-project $WEB_PROJECT --context AppDbContext
}
else {
    Write-Host "🚀 Updating Database to latest migration..." -ForegroundColor Cyan
    dotnet ef database update --project $INFRA_PROJECT --startup-project $WEB_PROJECT --context AppDbContext
}

Write-Host "✅ Database operation successful." -ForegroundColor Green
