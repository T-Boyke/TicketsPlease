// <copyright file="TicketTag.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Domain.Entities;

using System;
using TicketsPlease.Domain.Common;

/// <summary>
/// Repräsentiert die Zuordnung (Many-to-Many) zwischen einem Ticket und einem Tag.
/// </summary>
public class TicketTag : BaseEntity
{
  /// <summary>
  /// Gets or sets die ID des zugehörigen Tickets.
  /// </summary>
  public Guid TicketId { get; set; }

  /// <summary>
  /// Gets or sets das Navigation-Property für das Ticket.
  /// </summary>
  public Ticket? Ticket { get; set; }

  /// <summary>
  /// Gets or sets die ID des zugeordneten Tags.
  /// </summary>
  public Guid TagId { get; set; }

  /// <summary>
  /// Gets or sets das Navigation-Property für das Tag.
  /// </summary>
  public Tag? Tag { get; set; }
}
