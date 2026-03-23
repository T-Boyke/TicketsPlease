// <copyright file="WorkflowState.cs" company="TicketsPlease">
// Copyright (c) TicketsPlease. All rights reserved.
// </copyright>

namespace TicketsPlease.Domain.Entities;

using TicketsPlease.Domain.Common;

/// <summary>
/// Repräsentiert einen Zustand innerhalb des Ticket-Workflows (z.B. Offen, In Bearbeitung, Geschlossen).
/// </summary>
public class WorkflowState : BaseEntity
{
    /// <summary>
    /// Gets or sets den Namen des Zustands.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets die Reihenfolgenummer für die logische Anzeige im Kanban-Board.
    /// </summary>
    public int OrderIndex { get; set; }

    /// <summary>
    /// Gets or sets den Hexadezimal-Farbcode des Zustands für die UI-Darstellung.
    /// </summary>
    public string ColorHex { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether dieser Zustand der Endzustand (Terminal State) ist.
    /// </summary>
    public bool IsTerminalState { get; set; }
}
