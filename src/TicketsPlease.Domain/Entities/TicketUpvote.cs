// <copyright file="TicketUpvote.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TicketsPlease.Domain.Entities;

using System;
using TicketsPlease.Domain.Common;

/// <summary>
/// Repräsentiert einen Upvote (Zustimmung/Unterstützung) eines Benutzers für ein Ticket (z.B. Feature Request).
/// </summary>
public class TicketUpvote : BaseEntity
{
  /// <summary>
  /// Gets or sets die ID des upgevoteten Tickets.
  /// </summary>
  public Guid TicketId { get; set; }

  /// <summary>
  /// Gets or sets das Navigation-Property für das Ticket.
  /// </summary>
  public Ticket? Ticket { get; set; }

  /// <summary>
  /// Gets or sets die ID des Benutzers, der den Upvote abgegeben hat.
  /// </summary>
  public Guid UserId { get; set; }

  /// <summary>
  /// Gets or sets das Navigation-Property für den abstimmenden Benutzer.
  /// </summary>
  public User? User { get; set; }

  /// <summary>
  /// Gets or sets den Zeitpunkt (UTC) des Upvotes.
  /// </summary>
  public DateTime VotedAt { get; set; } = DateTime.UtcNow;
}
