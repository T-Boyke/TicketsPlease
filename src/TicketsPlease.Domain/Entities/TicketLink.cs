// <copyright file="TicketLink.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Domain.Entities;

using System;
using TicketsPlease.Domain.Common;
using TicketsPlease.Domain.Enums;

/// <summary>
/// Repräsentiert eine gerichtete Verknüpfung zwischen zwei Tickets (z.B. "Ticket A blockiert Ticket B").
/// </summary>
public class TicketLink : BaseEntity
{
  /// <summary>
  /// Initializes a new instance of the <see cref="TicketLink"/> class.
  /// </summary>
  /// <param name="sourceTicketId">Das Quell-Ticket.</param>
  /// <param name="targetTicketId">Das Ziel-Ticket.</param>
  /// <param name="linkType">Der Typ der Verknüpfung.</param>
  public TicketLink(Guid sourceTicketId, Guid targetTicketId, TicketLinkType linkType)
  {
    this.SourceTicketId = sourceTicketId;
    this.TargetTicketId = targetTicketId;
    this.LinkType = linkType;
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="TicketLink"/> class.
  /// Required for EF Core.
  /// </summary>
  private TicketLink()
  {
  }

  /// <summary>
  /// Gets die ID des Quell-Tickets.
  /// </summary>
  public Guid SourceTicketId { get; private set; }

  /// <summary>
  /// Gets das Quell-Ticket.
  /// </summary>
  public Ticket? SourceTicket { get; }

  /// <summary>
  /// Gets die ID des Ziel-Tickets.
  /// </summary>
  public Guid TargetTicketId { get; private set; }

  /// <summary>
  /// Gets das Ziel-Ticket.
  /// </summary>
  public Ticket? TargetTicket { get; }

  /// <summary>
  /// Gets den Typ der Verknüpfung.
  /// </summary>
  public TicketLinkType LinkType { get; private set; }
}
