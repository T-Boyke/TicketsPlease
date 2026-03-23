// <copyright file="Team.cs" company="TicketsPlease">
// Copyright (c) TicketsPlease. All rights reserved.
// </copyright>

namespace TicketsPlease.Domain.Entities;

using System;
using TicketsPlease.Domain.Common;

/// <summary>
/// Repräsentiert ein Support- oder Bearbeitungsteam zur Gruppierung von Tickets und Agenten.
/// </summary>
public class Team : BaseEntity
{
    /// <summary>
    /// Gets or sets den Namen des Teams.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets die Beschreibung der Zuständigkeiten des Teams.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets den spezifischen Farbcode des Teams für Dashboards und UI.
    /// </summary>
    public string ColorCode { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets den Zeitpunkt (UTC), an dem das Team erstellt wurde.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets die ID des Benutzers, der das Team erstellt hat.
    /// </summary>
    public Guid CreatedByUserId { get; set; }

    /// <summary>
    /// Gets or sets das Navigation-Property für den Benutzer, der das Team angelegt hat.
    /// </summary>
    public User? CreatedByUser { get; set; }
}
