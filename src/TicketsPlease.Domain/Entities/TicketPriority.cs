// <copyright file="TicketPriority.cs" company="TicketsPlease">
// Copyright (c) TicketsPlease. All rights reserved.
// </copyright>

namespace TicketsPlease.Domain.Entities;

using TicketsPlease.Domain.Common;

/// <summary>
/// Repräsentiert die Priorität eines Tickets, z.B. Hoch, Mittel, Niedrig.
/// </summary>
public class TicketPriority : BaseEntity
{
    /// <summary>
    /// Gets or sets den Namen der Priorität.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets die Gewichtung der Priorität zur Sortierung (höhere Zahl = wichtiger).
    /// </summary>
    public int LevelWeight { get; set; }

    /// <summary>
    /// Gets or sets den Hexadezimal-Farbcode der Priorität für die UI-Darstellung.
    /// </summary>
    public string ColorHex { get; set; } = string.Empty;
}
