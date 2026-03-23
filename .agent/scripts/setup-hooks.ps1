# Setup Git Hooks for TicketsPlease
$hooksPath = ".githooks"

Write-Host "Configuring Git to use shared hooks in $hooksPath..." -ForegroundColor Cyan

git config core.hooksPath $hooksPath

if ($LASTEXITCODE -eq 0) {
    Write-Host "✅ Git hooks configured successfully!" -ForegroundColor Green
} else {
    Write-Host "❌ Failed to configure Git hooks." -ForegroundColor Red
}
