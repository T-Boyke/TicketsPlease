# 🚀 TicketsPlease - GitHub Issue Automation Script
# Prerequisites: Run 'gh auth login' before executing this script.

$repo = "BitLC-NE-2025-2026/TicketsPlease"
$gh = "C:\Program Files\GitHub CLI\gh.exe"

function Ensure-Label($name, $color, $description) {
    Write-Host "Checking label: $name..." -ForegroundColor Gray
    $exists = & $gh label list --repo $repo --search $name --json name | ConvertFrom-Json
    if ($exists.Count -eq 0 -or ($exists | Where-Object { $_.name -eq $name }).Count -eq 0) {
        Write-Host "Creating label: $name..." -ForegroundColor Yellow
        & $gh label create $name --repo $repo --color $color --description $description
    }
}

function Create-Issue($title, $body, $labels) {
    Write-Host "Creating Issue: $title..." -ForegroundColor Cyan
    & $gh issue create --repo $repo --title $title --body $body --label ($labels -join ",")
}

# --- 1. Ensure Labels Exist ---
Write-Host "Ensuring required labels exist..." -ForegroundColor Blue

# Area Labels
Ensure-Label "area:domain" "3182CE" "Domain Layer (Entities, Value Objects)"
Ensure-Label "area:application" "38A169" "Application Layer (MediatR, CQRS)"
Ensure-Label "area:infrastructure" "805AD5" "Infrastructure Layer (EF Core, Services)"
Ensure-Label "area:web" "DD6B20" "Web Layer (Controllers, Views)"
Ensure-Label "area:tests" "E53E3E" "Testing (Unit, Integration, E2E)"
Ensure-Label "area:docs" "718096" "Documentation and ADRs"
Ensure-Label "area:github" "2D3748" "CI/CD and Project Management"

# Type Labels
Ensure-Label "Feature" "0E8A16" "New functionality"
Ensure-Label "Bug" "D93F0B" "Something isn't working"
Ensure-Label "Task" "FBCA04" "Technical task or refactoring"

# Size Labels
Ensure-Label "size/XS" "EDEDED" "Extremely small task"
Ensure-Label "size/S" "D1FAE5" "Small task"
Ensure-Label "size/M" "FEF3C7" "Medium task"
Ensure-Label "size/L" "FEE2E2" "Large task"
Ensure-Label "size/XL" "FCA5A5" "Very large task"

# Additional Labels
Ensure-Label "dependencies" "B60205" "Blockers or dependency issues"
Ensure-Label "diagrams" "2133A1" "Mermaid or architecture diagrams"

# --- 2. Create Issues ---
Write-Host "`nCreating all business issues..." -ForegroundColor Blue

# --- F1: Web App & Identity ---
Create-Issue "F1: [Domain] AppUser Entity extensions" "Implement AppUser with Username, Vorname, and Email requirements." @("area:domain", "size/S", "Feature")
Create-Issue "F1: [Infrastructure] Identity Setup & Seeding" "Configure ASP.NET Core Identity with Admin, Developer, and Tester roles and seed users." @("area:infrastructure", "size/S", "Feature")
Create-Issue "F1: [Web] AccountController & Auth Views" "Implement Login, Logout, and registration flow with Tailwind styling." @("area:web", "size/M", "Feature")

# --- F2: Admin Area & Project CRUD ---
Create-Issue "F2: [Domain] Project Entity" "Create Project entity with Title, Description, and Dates (IHK F2.2)." @("area:domain", "size/S", "Feature")
Create-Issue "F2: [Application] Project CRUD Commands" "Implement MediatR commands for Project management." @("area:application", "size/S", "Feature")
Create-Issue "F2: [Web] Admin Area Dashboard & CRUD" "Implement Admin-only views for Project management." @("area:web", "size/M", "Feature")

# --- F3: Ticket Management ---
Create-Issue "F3: [Domain] Ticket Entity with Close-Rules" "Implement rich domain model for Ticket with status management and SHA1 hash." @("area:domain", "size/S", "Feature")
Create-Issue "F3: [Application] Ticket Management Commands" "Implement Create/Assign/Close commands (MediatR)." @("area:application", "size/S", "Feature")
Create-Issue "F3: [Web] Ticket List & Detail Views" "Implement sortable/filterable ticket list and detailed information view." @("area:web", "size/M", "Feature")

# --- F5: Comments ---
Create-Issue "F5: [Domain] Comment Entity" "Implement Comment entity and Ticket aggregate persistence." @("area:domain", "size/S", "Feature")
Create-Issue "F5: [Web] Ticket Comments Component" "Implement comment display and submission on ticket detail page." @("area:web", "size/S", "Feature")

# --- F7: Blockers ---
Create-Issue "F7: [Domain] Ticket Dependencies" "Implement N:M relationship for ticket blockers." @("area:domain", "size/S", "Feature")
Create-Issue "F7: [Application] Dependency Validation" "Prevent closing tickets with open blockers." @("area:application", "size/S", "Feature")

# --- F8: Workflows ---
Create-Issue "F8: [Domain] Workflow & Status Entities" "Implement Dynamic Workflow system." @("area:domain", "size/S", "Feature")

# --- F9: Messages ---
Create-Issue "F9: [Domain] Message Entity" "Implement Sender/Receiver/Content message system." @("area:domain", "size/S", "Feature")
Create-Issue "F9: [Web] Messaging Inbox View" "Implement grouped message list and composition view." @("area:web", "size/M", "Feature")

# --- Enterprise Add-Ons ---
Create-Issue "ENT: [Web] Kanban Dashboard (Drag & Drop)" "Implement interactive board with SortableJS." @("area:web", "size/XL", "Feature")
Create-Issue "ENT: [Web] Markdown & Mermaid Rendering" "Integrate Marked and Mermaid for descriptions." @("area:web", "size/S", "Feature")
Create-Issue "ENT: [Infrastructure] Audit-Log Engine" "Implement append-only history trail." @("area:infrastructure", "size/L", "Feature")

Write-Host "`n✅ All issues and labels created successfully!" -ForegroundColor Green
