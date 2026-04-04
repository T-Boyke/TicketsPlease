// <copyright file="Message.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Domain.Entities;

using System;
using TicketsPlease.Domain.Common;

/// <summary>
/// Repräsentiert eine Chat-Nachricht oder einen Kommentar zu einem Ticket, an ein Team oder einen direkten Benutzer.
/// </summary>
public class Message : BaseEntity
{
  /// <summary>
  /// Gets or sets die ID des Absender-Benutzers.
  /// </summary>
  public Guid SenderUserId { get; set; }

  /// <summary>
  /// Gets or sets das Navigation-Property für den Absender-Benutzer.
  /// </summary>
  public User? SenderUser { get; set; }

  /// <summary>
  /// Gets or sets die (optionale) Ticket-ID, falls es sich um eine Ticket-Nachricht handelt.
  /// </summary>
  public Guid? TicketId { get; set; }

  /// <summary>
  /// Gets or sets das Navigation-Property für ein zugehöriges Ticket.
  /// </summary>
  public Ticket? Ticket { get; set; }

  /// <summary>
  /// Gets or sets die (optionale) Team-ID, falls es sich um eine Team-Nachricht handelt.
  /// </summary>
  public Guid? TeamId { get; set; }

  /// <summary>
  /// Gets or sets das Navigation-Property für ein zugehöriges Team.
  /// </summary>
  public Team? Team { get; set; }

  /// <summary>
  /// Gets or sets die (optionale) Empfänger-ID, falls es sich um eine direkte Nachricht handelt.
  /// </summary>
  public Guid? ReceiverUserId { get; set; }

  /// <summary>
  /// Gets or sets das Navigation-Property für einen direkten Empfänger-Benutzer.
  /// </summary>
  public User? ReceiverUser { get; set; }

  /// <summary>
  /// Gets or sets den als Markdown formatierten Inhalt der Nachricht.
  /// </summary>
  public string BodyMarkdown { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets den Zeitpunkt (UTC), an dem die Nachricht gesendet wurde.
  /// </summary>
  public DateTime SentAt { get; set; } = DateTime.UtcNow;

  /// <summary>
  /// Gets or sets a value indicating whether die Nachricht nach dem Senden bearbeitet wurde.
  /// </summary>
  public bool IsEdited { get; set; }

  /// <summary>
  /// Gets die Liste der Dateianhänge dieser Nachricht.
  /// </summary>
  public virtual ICollection<FileAsset> Attachments { get; } = new List<FileAsset>();
}
