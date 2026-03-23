// <copyright file="SubTicket.cs" company="TicketsPlease">
// Copyright (c) TicketsPlease. All rights reserved.
// </copyright>

namespace TicketsPlease.Domain.Entities;

using System;
using TicketsPlease.Domain.Common;

/// <summary>
/// Repräsentiert ein Unterticket (Teilaufgabe), das einem Hauptticket zugeordnet ist.
/// </summary>
public class SubTicket : BaseEntity
{
    /// <summary>
    /// Gets or sets die ID des übergeordneten Haupttickets.
    /// </summary>
    public Guid ParentTicketId { get; set; }

    /// <summary>
    /// Gets or sets das Navigation-Property für das Hauptticket.
    /// </summary>
    public Ticket? ParentTicket { get; set; }

    /// <summary>
    /// Gets or sets den Titel der Teilaufgabe.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether die Teilaufgabe abgeschlossen ist.
    /// </summary>
    public bool IsCompleted { get; set; }

    /// <summary>
    /// Gets or sets den Zeitpunkt (UTC) der Erstellung des Untertickets.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets die ID des Benutzers, der die Teilaufgabe erstellt hat.
    /// </summary>
    public Guid CreatorId { get; set; }

    /// <summary>
    /// Gets or sets das Navigation-Property für den ersteller-Benutzer.
    /// </summary>
    public User? Creator { get; set; }
}
