# TicketsPlease - NATIVE RELATIONSHIP SYNC (GRAPHQL V2)
$repo = "BitLC-NE-2025-2026/TicketsPlease"
$gh = "C:\Program Files\GitHub CLI\gh.exe"

# Load IDs
$issueData = Get-Content -Raw d:\DEV\Tickets\issue_ids.json | ConvertFrom-Json
$ids = @{}
foreach ($node in $issueData.data.repository.issues.nodes) {
    $ids[$node.number.ToString()] = $node.id
}

function Add-SubIssue($parentId, $childId) {
    Write-Host "# Linking Parent $parentId -> Child $childId"
    $query = 'mutation($issueId: ID!, $subIssueId: ID!) { addSubIssue(input: {issueId: $issueId, subIssueId: $subIssueId}) { clientMutationId } }'
    & $gh api graphql -f query=$query -f issueId=$parentId -f subIssueId=$childId | Out-Null
}

function Add-BlockedBy($blockedId, $blockingId) {
    Write-Host "# Linking $blockedId BLOCKED BY $blockingId"
    $query = 'mutation($issueId: ID!, $blockingIssueId: ID!) { addBlockedBy(input: {issueId: $issueId, blockingIssueId: $blockingIssueId}) { clientMutationId } }'
    & $gh api graphql -f query=$query -f issueId=$blockedId -f blockingIssueId=$blockingId | Out-Null
}

# --- PARENT: F1 (31) ---
foreach ($c in @("32", "33", "49", "50", "51", "52", "53", "54", "55", "56", "57", "58")) {
    if ($ids[$c] -and $ids["31"]) { Add-SubIssue $ids["31"] $ids[$c] }
}

# --- PARENT: F2 (34) ---
foreach ($c in @("35", "36", "59", "60", "61", "62", "63", "64", "65", "66")) {
    if ($ids[$c] -and $ids["34"]) { Add-SubIssue $ids["34"] $ids[$c] }
}

# --- PARENT: F3 (37) ---
foreach ($c in @("38", "39", "67", "68", "69", "70", "71", "72", "73", "74")) {
    if ($ids[$c] -and $ids["37"]) { Add-SubIssue $ids["37"] $ids[$c] }
}

# --- PARENT: ENT-2 (45) ---
foreach ($c in @("75", "76", "77", "78")) {
    if ($ids[$c] -and $ids["45"]) { Add-SubIssue $ids["45"] $ids[$c] }
}

# --- DEPENDENCIES: LAYER FLOW (F1-F3) ---
# Domain blocks Application
if ($ids["49"] -and $ids["31"]) { Add-BlockedBy $ids["49"] $ids["31"] }
# Application blocks Infrastructure
if ($ids["52"] -and $ids["49"]) { Add-BlockedBy $ids["52"] $ids["49"] }
# Infrastructure blocks Web
if ($ids["54"] -and $ids["52"]) { Add-BlockedBy $ids["54"] $ids["52"] }

Write-Host "Native relationships synchronized!"
