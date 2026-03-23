// <copyright file="Organization.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
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
}
