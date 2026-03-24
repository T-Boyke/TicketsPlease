// <copyright file="TicketAssignment.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Domain.Entities;

using System;
using TicketsPlease.Domain.Common;

/// <summary>
/// Repräsentiert die Zuweisung eines Tickets an einen bestimmten Benutzer oder ein Team.
/// </summary>
public class TicketAssignment : BaseEntity
{
  /// <summary>
  /// Gets or sets die ID des zugewiesenen Tickets.
  /// </summary>
  public Guid TicketId { get; set; }

  /// <summary>
  /// Gets or sets das Navigation-Property für das zugewiesene Ticket.
  /// </summary>
  public Ticket? Ticket { get; set; }

  /// <summary>
  /// Gets or sets die (optionale) ID des zugewiesenen Benutzers.
  /// </summary>
  public Guid? UserId { get; set; }

  /// <summary>
  /// Gets or sets das Navigation-Property für den zugewiesenen Benutzer.
  /// </summary>
  public User? User { get; set; }

  /// <summary>
  /// Gets or sets die (optionale) ID des zugewiesenen Teams.
  /// </summary>
  public Guid? TeamId { get; set; }

  /// <summary>
  /// Gets or sets das Navigation-Property für das zugewiesene Team.
  /// </summary>
  public Team? Team { get; set; }

  /// <summary>
  /// Gets or sets den Zeitpunkt (UTC), an dem die Zuweisung stattfand.
  /// </summary>
  public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
}
