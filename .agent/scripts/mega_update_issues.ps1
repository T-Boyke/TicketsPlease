# TicketsPlease - MEGA Lifecycle Update (V4 - ASCII ONLY)
$repo = "BitLC-NE-2025-2026/TicketsPlease"
$gh = "C:\Program Files\GitHub CLI\gh.exe"
$assignee = "T-Boyke"

function Update-Issue($number, $title, $labels, $rel, $is_closed) {
    Write-Host ("Syncing #" + $number)
    $lbl_str = $labels -join ","
    & $gh issue edit $number --repo $repo --title $title --add-assignee $assignee --add-label $lbl_str

    $body = "Objective: Detailed in documentation. Relationships: " + $rel
    & $gh issue edit $number --repo $repo --body $body

    if ($is_closed) {
        & $gh issue close $number --repo $repo --reason "completed"
    } else {
        & $gh issue reopen $number --repo $repo 2>$null
    }
}

function Create-Issue($title, $labels, $rel) {
    Write-Host ("Creating: " + $title)
    $body = "Objective: Detailed in roadmap. Relationships: " + $rel
    & $gh issue create --repo $repo --title $title --body $body --label ($labels -join ",") --assignee $assignee
}

# --- Phase 0: DONE ---
Update-Issue 23 "FOUNDATION: Clean Architecture Scaffolding" @("area:github", "size/L", "Task") "Parent: #CORE | Blocking: MVP" $true
Update-Issue 24 "FOUNDATION: Quality Gate Setup" @("area:github", "size/S", "Task") "Parent: #CORE" $true
Update-Issue 25 "FOUNDATION: CI/CD Hardening" @("area:github", "size/M", "Task") "Parent: #CORE" $true
Update-Issue 26 "UI: OKLCH Design Tokens" @("area:web", "size/M", "Task") "Parent: #UI" $true
Update-Issue 27 "UI: Local Font Integration" @("area:web", "size/S", "Task") "Parent: #UI" $true
Update-Issue 28 "UI: Global Layout" @("area:web", "size/S", "Task") "Parent: #UI" $true
Update-Issue 29 "DOCS: Chapter 1-2" @("area:docs", "size/M", "Task") "Parent: #DOCS" $true
Update-Issue 30 "DOCS: 11 MVP ADRs" @("area:docs", "size/M", "Task") "Parent: #DOCS" $true

# --- Phase 1: MVP ---
Update-Issue 31 "F1: [Domain] AppUser entity extensions" @("area:domain", "size/S", "Feature") "Parent: #F1 | Blocking: #32" $false
Update-Issue 32 "F1: [Infrastructure] Identity Setup" @("area:infrastructure", "size/S", "Feature") "Parent: #F1 | Blocked by: #31" $false
Update-Issue 33 "F1: [Web] AccountController" @("area:web", "size/M", "Feature") "Parent: #F1 | Blocked by: #32" $false
Update-Issue 34 "F2: [Domain] Project Entity" @("area:domain", "size/S", "Feature") "Parent: #F2 | Blocking: #35" $false
Update-Issue 35 "F2: [Application] Project CRUD" @("area:application", "size/S", "Feature") "Parent: #F2 | Blocked by: #34" $false
Update-Issue 36 "F2: [Web] Project UI" @("area:web", "size/M", "Feature") "Parent: #F2 | Blocked by: #35" $false
Update-Issue 37 "F3: [Domain] Ticket Model" @("area:domain", "size/S", "Feature") "Parent: #F3 | Blocking: #38" $false
Update-Issue 38 "F3: [Application] Ticket Commands" @("area:application", "size/S", "Feature") "Parent: #F3 | Blocked by: #37" $false
Update-Issue 39 "F3: [Web] Ticket Explorer" @("area:web", "size/M", "Feature") "Parent: #F3" $false
Update-Issue 40 "F7: [Domain] Ticket Dependencies" @("area:domain", "size/S", "Feature") "Parent: #F7 | Blocking: #41" $false
Update-Issue 41 "F7: [Application] Close Validation" @("area:application", "size/S", "Feature") "Parent: #F7 | Blocked by: #40" $false

# --- Phase 2-5: FUTURE ---
Create-Issue "F5: [Domain] Comment Entity" @("area:domain", "size/S", "Feature") "Parent: #F5"
Create-Issue "F8: [Shared] Workflow Engine" @("area:domain", "size/S", "Feature") "Parent: #F8"
Create-Issue "F9: [Web] Messaging Center" @("area:web", "size/M", "Feature") "Parent: #F9"
Create-Issue "ENT-2: [Web] Kanban Board" @("area:web", "size/XL", "Feature") "Parent: Phase 2 | Blocked by: F3"
Create-Issue "ENT-3: [Infra] Audit Trail" @("area:infrastructure", "size/L", "Feature") "Parent: Phase 3"
Create-Issue "ENT-4: [Infra] Plugin System" @("area:infrastructure", "size/XL", "Feature") "Parent: Phase 4"
Create-Issue "ENT-5: [Infra] Redis Caching" @("area:infrastructure", "size/M", "Feature") "Parent: Phase 5"

Write-Host "Done!"
