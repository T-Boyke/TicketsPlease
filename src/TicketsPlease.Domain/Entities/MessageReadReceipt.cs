// <copyright file="MessageReadReceipt.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TicketsPlease.Domain.Entities;

using System;
using TicketsPlease.Domain.Common;

/// <summary>
/// Repräsentiert eine Lesebestätigung (Read Receipt) für eine Nachricht.
/// </summary>
public class MessageReadReceipt : BaseEntity
{
  /// <summary>
  /// Gets or sets die ID der gelesenen Nachricht.
  /// </summary>
  public Guid MessageId { get; set; }

  /// <summary>
  /// Gets or sets das Navigation-Property für die gelesene Nachricht.
  /// </summary>
  public Message? Message { get; set; }

  /// <summary>
  /// Gets or sets die ID des Benutzers, der die Nachricht gelesen hat.
  /// </summary>
  public Guid UserId { get; set; }

  /// <summary>
  /// Gets or sets das Navigation-Property für den Benutzer, der die Nachricht gelesen hat.
  /// </summary>
  public User? User { get; set; }

  /// <summary>
  /// Gets or sets den Zeitpunkt (UTC), an dem die Nachricht als gelesen markiert wurde.
  /// </summary>
  public DateTime ReadAt { get; set; } = DateTime.UtcNow;
}
