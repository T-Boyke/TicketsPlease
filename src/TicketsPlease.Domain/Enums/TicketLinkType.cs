// <copyright file="TicketLinkType.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TicketsPlease.Domain.Enums;

/// <summary>
/// Definiert den Typ der Beziehung zwischen zwei Tickets.
/// </summary>
public enum TicketLinkType
{
    /// <summary>
    /// Das Quell-Ticket blockiert das Ziel-Ticket.
    /// </summary>
    Blocks = 0,

    /// <summary>
    /// Das Quell-Ticket steht in Beziehung zum Ziel-Ticket.
    /// </summary>
    RelatesTo = 1,

    /// <summary>
    /// Das Quell-Ticket ist ein Duplikat des Ziel-Tickets.
    /// </summary>
    Duplicates = 2,
}
