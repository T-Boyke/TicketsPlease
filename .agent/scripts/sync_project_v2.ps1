# TicketsPlease - Project Board Metadata Sync (V4 - MEGA)
$repo = "BitLC-NE-2025-2026/TicketsPlease"
$gh = "C:\Program Files\GitHub CLI\gh.exe"
$projectId = "PVT_kwDODuVgms4BSkUI"

# Field IDs
$fieldStatus = "PVTSSF_lADODuVgms4BSkUIzhAECfg"
$fieldPriority = "PVTSSF_lADODuVgms4BSkUIzhAECtQ"
$fieldSize = "PVTSSF_lADODuVgms4BSkUIzhAECtU"

# Option IDs - Status
$optDone = "98236657"
$optReady = "61e4505c"
$optInProgress = "47fc9ee4"
$optBacklog = "f75ad846"

# Option IDs - Priority
$optP0_High = "79628723"
$optP1_Med = "0a877460"
$optP2_Low = "da944a9c"

# Option IDs - Size
$optXS = "6c6483d2"
$optS = "f784b110"
$optM = "7515a9f1"
$optL = "817d0097"
$optXL = "db339eb2"

function Sync-ProjectMetadata($itemId, $number, $labels) {
    Write-Host ("# Syncing Item: " + $itemId + " (Issue #" + $number + ")")
    
    # 1. Update STATUS
    $statusId = $optBacklog
    if ($number -ge 23 -and $number -le 30) { $statusId = $optDone }
    elseif ($number -ge 31 -and $number -le 41) { $statusId = $optReady }
    elseif ($number -ge 49 -and $number -le 78) { $statusId = $optReady } # Detailed MVP tasks
    & $gh project item-edit --id $itemId --project-id $projectId --field-id $fieldStatus --single-select-option-id $statusId

    # 2. Update PRIORITY
    $priorityId = $optP2_Low
    if ($labels -contains "priority/high") { $priorityId = $optP0_High }
    elseif ($labels -contains "priority/medium") { $priorityId = $optP1_Med }
    & $gh project item-edit --id $itemId --project-id $projectId --field-id $fieldPriority --single-select-option-id $priorityId

    # 3. Update SIZE
    $sizeId = $optS
    if ($labels -contains "size/XS") { $sizeId = $optXS }
    elseif ($labels -contains "size/M") { $sizeId = $optM }
    elseif ($labels -contains "size/L") { $sizeId = $optL }
    elseif ($labels -contains "size/XL") { $sizeId = $optXL }
    & $gh project item-edit --id $itemId --project-id $projectId --field-id $fieldSize --single-select-option-id $sizeId
}

$itemsJson = Get-Content -Raw "project_items_mega.json" | ConvertFrom-Json
foreach ($item in $itemsJson.items) {
    Sync-ProjectMetadata $item.id $item.content.number $item.labels
}

Write-Host "Project board metadata synchronization successful!"
