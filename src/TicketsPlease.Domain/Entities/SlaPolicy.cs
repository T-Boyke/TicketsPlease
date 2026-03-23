// <copyright file="SlaPolicy.cs" company="TicketsPlease">
// Copyright (c) TicketsPlease. All rights reserved.
// </copyright>

namespace TicketsPlease.Domain.Entities;

using System;
using TicketsPlease.Domain.Common;

/// <summary>
/// Repräsentiert eine Service-Level-Agreement (SLA) Richtlinie für bestimmte Ticket-Prioritäten.
/// </summary>
public class SlaPolicy : BaseEntity
{
    /// <summary>
    /// Gets or sets die ID der zugehörigen Ticket-Priorität.
    /// </summary>
    public Guid PriorityId { get; set; }

    /// <summary>
    /// Gets or sets das Navigation-Property für die verknüpfte Ticket-Priorität.
    /// </summary>
    public TicketPriority? Priority { get; set; }

    /// <summary>
    /// Gets or sets die definierte Antwortzeit in Stunden.
    /// </summary>
    public int ResponseTimeHours { get; set; }

    /// <summary>
    /// Gets or sets die definierte Lösungszeit in Stunden.
    /// </summary>
    public int ResolutionTimeHours { get; set; }
}
