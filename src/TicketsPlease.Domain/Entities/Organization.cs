// <copyright file="Organization.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Domain.Entities;

using TicketsPlease.Domain.Common;

/// <summary>
/// Repräsentiert eine Kundenorganisation oder ein Mandantenunternehmen.
/// </summary>
public class Organization : BaseAuditableEntity
{
  /// <summary>
  /// Gets or sets den Namen der Organisation.
  /// </summary>
  public string Name { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets das aktuelle Abonnement-Level der Organisation (z.B. Trial, Basic, Premium).
  /// </summary>
  public string SubscriptionLevel { get; set; } = "Trial";

  /// <summary>
  /// Gets or sets a value indicating whether die Organisation aktiv ist und Zugriff auf das System hat.
  /// </summary>
  public bool IsActive { get; set; } = true;

  /// <summary>
  /// Gets or sets das Intervall für SLA-Prüfungen in Minuten.
  /// </summary>
  public int SlaCheckIntervalMinutes { get; set; } = 15;

  /// <summary>
  /// Gets or sets den Beginn der Ruhezeit (HH:mm).
  /// </summary>
  public TimeSpan? QuietHoursStart { get; set; }

  /// <summary>
  /// Gets or sets das Ende der Ruhezeit (HH:mm).
  /// </summary>
  public TimeSpan? QuietHoursEnd { get; set; }

  /// <summary>
  /// Gets or sets die lokale Zeitzone der Organisation (z.B. "W. Europe Standard Time").
  /// </summary>
  public string TimeZoneId { get; set; } = "UTC";

  /// <summary>
  /// Gets or sets a value indicating whether bei Low-Priorität benachrichtigt werden soll.
  /// </summary>
  public bool NotifyOnLow { get; set; } = true;

  /// <summary>
  /// Gets or sets a value indicating whether bei Medium-Priorität benachrichtigt werden soll.
  /// </summary>
  public bool NotifyOnMedium { get; set; } = true;

  /// <summary>
  /// Gets or sets a value indicating whether bei High-Priorität benachrichtigt werden soll.
  /// </summary>
  public bool NotifyOnHigh { get; set; } = true;

  /// <summary>
  /// Gets or sets a value indicating whether bei Blocker-Priorität benachrichtigt werden soll.
  /// </summary>
  public bool NotifyOnBlocker { get; set; } = true;
}
