# TicketsPlease - NATIVE RELATIONSHIP SYNC (GRAPHQL)
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
    $query = "mutation { addSubIssue(input: {issueId: `"$parentId`", subIssueId: `"$childId`"}) { clientMutationId } }"
    & $gh api graphql -f query=$query | Out-Null
}

function Add-BlockedBy($blockedId, $blockingId) {
    Write-Host "# Linking $blockedId BLOCKED BY $blockingId"
    $query = "mutation { addBlockedBy(input: {issueId: `"$blockedId`", blockingIssueId: `"$blockingId`"}) { clientMutationId } }"
    & $gh api graphql -f query=$query | Out-Null
}

# --- PARENT: F1 (31) ---
foreach ($c in @("32", "33", "49", "50", "51", "52", "53", "54", "55", "56", "57", "58")) {
    if ($ids[$c]) { Add-SubIssue $ids["31"] $ids[$c] }
}

# --- PARENT: F2 (34) ---
foreach ($c in @("35", "36", "59", "60", "61", "62", "63", "64", "65", "66")) {
    if ($ids[$c]) { Add-SubIssue $ids["34"] $ids[$c] }
}

# --- PARENT: F3 (37) ---
foreach ($c in @("38", "39", "67", "68", "69", "70", "71", "72", "73", "74")) {
    if ($ids[$c]) { Add-SubIssue $ids["37"] $ids[$c] }
}

# --- DEPENDENCIES (Examples) ---
if ($ids["32"] -and $ids["31"]) { Add-BlockedBy $ids["32"] $ids["31"] }
if ($ids["33"] -and $ids["32"]) { Add-BlockedBy $ids["33"] $ids["32"] }
if ($ids["41"] -and $ids["40"]) { Add-BlockedBy $ids["41"] $ids["40"] }

Write-Host "Native relationships synchronized!"
