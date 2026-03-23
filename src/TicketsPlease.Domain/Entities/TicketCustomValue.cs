// <copyright file="TicketCustomValue.cs" company="TicketsPlease">
// Copyright (c) TicketsPlease. All rights reserved.
// </copyright>

namespace TicketsPlease.Domain.Entities;

using System;
using TicketsPlease.Domain.Common;

/// <summary>
/// Repräsentiert den konkreten Wert eines benutzerdefinierten Feldes (Custom Field) für ein spezifisches Ticket.
/// </summary>
public class TicketCustomValue : BaseEntity
{
    /// <summary>
    /// Gets or sets die ID des zugehörigen Tickets.
    /// </summary>
    public Guid TicketId { get; set; }

    /// <summary>
    /// Gets or sets das Navigation-Property für das zugehörige Ticket.
    /// </summary>
    public Ticket? Ticket { get; set; }

    /// <summary>
    /// Gets or sets die ID der benutzerdefinierten Felddefinition.
    /// </summary>
    public Guid FieldDefinitionId { get; set; }

    /// <summary>
    /// Gets or sets das Navigation-Property für die Felddefinition.
    /// </summary>
    public CustomFieldDefinition? FieldDefinition { get; set; }

    /// <summary>
    /// Gets or sets den als String gespeicherten Wert des benutzerdefinierten Feldes.
    /// </summary>
    public string Value { get; set; } = string.Empty;
}
