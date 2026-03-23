// <copyright file="TicketHistory.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TicketsPlease.Domain.Entities;

using System;
using TicketsPlease.Domain.Common;

/// <summary>
/// Repräsentiert einen Audit-Log-Eintrag für eine Änderung an einem Ticket.
/// </summary>
public class TicketHistory : BaseEntity
{
  /// <summary>
  /// Gets or sets die ID des geänderten Tickets.
  /// </summary>
  public Guid TicketId { get; set; }

  /// <summary>
  /// Gets or sets das Navigation-Property für das geänderte Ticket.
  /// </summary>
  public Ticket? Ticket { get; set; }

  /// <summary>
  /// Gets or sets die ID des Benutzers, der die Änderung vorgenommen hat.
  /// </summary>
  public Guid ActorUserId { get; set; }

  /// <summary>
  /// Gets or sets das Navigation-Property für den agierenden Benutzer.
  /// </summary>
  public User? ActorUser { get; set; }

  /// <summary>
  /// Gets or sets den Namen des geänderten Feldes (z.B. Status, Priorität, Zuweisung).
  /// </summary>
  public string FieldName { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets den alten Wert vor der Änderung als String.
  /// </summary>
  public string? OldValue { get; set; }

  /// <summary>
  /// Gets or sets den neuen Wert nach der Änderung als String.
  /// </summary>
  public string? NewValue { get; set; }

  /// <summary>
  /// Gets or sets den Zeitpunkt (UTC) der Änderung.
  /// </summary>
  public DateTime ChangedAt { get; set; } = DateTime.UtcNow;
}
