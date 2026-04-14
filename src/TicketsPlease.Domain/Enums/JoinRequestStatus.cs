// <copyright file="JoinRequestStatus.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Domain.Enums;

/// <summary>
/// Status einer Beitrittsanfrage zu einem Team.
/// </summary>
public enum JoinRequestStatus
{
    /// <summary>
    /// Die Anfrage ist noch offen.
    /// </summary>
    Pending,

    /// <summary>
    /// Die Anfrage wurde akzeptiert.
    /// </summary>
    Approved,

    /// <summary>
    /// Die Anfrage wurde abgelehnt.
    /// </summary>
    Rejected
}
