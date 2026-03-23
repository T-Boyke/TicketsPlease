a$repo = "BitLC-NE-2025-2026/TicketsPlease"
$gh = "C:\Program Files\GitHub CLI\gh.exe"
$assignee = "T-Boyke"

function Update-Issue($number, $labels, $body_extra, $priority_label) {
    Write-Host "Updating Issue #$number..."

    # 1. Update basic metadata (Assignee, Labels)
    $lbl_str = ($labels + $priority_label) -join ","
    & $gh issue edit $number --repo $repo --add-assignee $assignee --add-label $lbl_str

    # 2. Update Body if relationships are provided
    if ($body_extra) {
        $tmp = "$env:TEMP\issue_$number.txt"
        & $gh issue view $number --repo $repo --json body --template "{{.body}}" | Out-File -FilePath $tmp -Encoding UTF8

        $relationships = "`n`n---`n**Relationships:** " + $body_extra
        Add-Content -Path $tmp -Value $relationships -Encoding UTF8

        & $gh issue edit $number --repo $repo --body-file $tmp
        Remove-Item $tmp -ErrorAction SilentlyContinue
    }
}

# Ensure Type/Priority labels exist (just in case)
& $gh label create "priority/high" --repo $repo --color "B60205" --description "High priority" --force 2>$null
& $gh label create "priority/medium" --repo $repo --color "FBCA04" --description "Medium priority" --force 2>$null
& $gh label create "priority/low" --repo $repo --color "0E8A16" --description "Low priority" --force 2>$null
& $gh label create "Feature" --repo $repo --color "0E8A16" --description "New functionality" --force 2>$null

# F1: Identity
Update-Issue 23 @("area:domain", "size/S", "Feature") "Blocks #24, #25" "priority/high"
Update-Issue 24 @("area:infrastructure", "size/S", "Feature") "Blocked by #23" "priority/high"
Update-Issue 25 @("area:web", "size/M", "Feature") "Blocked by #24" "priority/medium"

# F2: Projects
Update-Issue 26 @("area:domain", "size/S", "Feature") "Blocks #27, #28" "priority/high"
Update-Issue 27 @("area:application", "size/S", "Feature") "Blocked by #26" "priority/medium"
Update-Issue 28 @("area:web", "size/M", "Feature") "Blocked by #27" "priority/medium"

# F3: Tickets
Update-Issue 29 @("area:domain", "size/S", "Feature") "Blocks #30, #31" "priority/high"
Update-Issue 30 @("area:application", "size/S", "Feature") "Blocked by #29" "priority/medium"
Update-Issue 31 @("area:web", "size/M", "Feature") "Blocked by #30" "priority/medium"

# F5: Comments
Update-Issue 32 @("area:domain", "size/S", "Feature") "Blocks #33" "priority/low"
Update-Issue 33 @("area:web", "size/S", "Feature") "Blocked by #32" "priority/low"

# F7: Blockers
Update-Issue 34 @("area:domain", "size/S", "Feature") "Blocks #35" "priority/medium"
Update-Issue 35 @("area:application", "size/S", "Feature") "Blocked by #34" "priority/medium"

# F8: Workflows
Update-Issue 36 @("area:domain", "size/S", "Feature") "" "priority/medium"

# F9: Messaging
Update-Issue 37 @("area:domain", "size/S", "Feature") "Blocks #38" "priority/low"
Update-Issue 38 @("area:web", "size/M", "Feature") "Blocked by #37" "priority/low"

# Enterprise
Update-Issue 39 @("area:web", "size/XL", "Feature") "" "priority/medium"
Update-Issue 40 @("area:web", "size/S", "Feature") "" "priority/low"
Update-Issue 41 @("area:infrastructure", "size/L", "Feature") "" "priority/low"

Write-Host "Done!"
