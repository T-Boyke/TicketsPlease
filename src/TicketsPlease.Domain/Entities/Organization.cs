// <copyright file="Organization.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TicketsPlease.Domain.Entities;

using System;
using TicketsPlease.Domain.Common;

/// <summary>
/// Repräsentiert eine Kundenorganisation oder ein Mandantenunternehmen.
/// </summary>
public class Organization : BaseEntity
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
  /// Gets or sets den Zeitpunkt (UTC), an dem die Organisation dem System beigetreten ist.
  /// </summary>
  public DateTime JoinedAt { get; set; } = DateTime.UtcNow;

  /// <summary>
  /// Gets or sets a value indicating whether die Organisation aktiv ist und Zugriff auf das System hat.
  /// </summary>
  public bool IsActive { get; set; } = true;
}
