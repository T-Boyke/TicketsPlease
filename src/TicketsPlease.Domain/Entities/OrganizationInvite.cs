// <copyright file="OrganizationInvite.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Domain.Entities;

using System;
using TicketsPlease.Domain.Common;

/// <summary>
/// Repräsentiert eine Einladung zu einer Organisation via Token.
/// </summary>
public class OrganizationInvite : BaseAuditableEntity
{
  /// <summary>
  /// Gets or sets den eindeutigen Einladungs-Token (GUID).
  /// </summary>
  public Guid Token { get; set; } = Guid.NewGuid();

  /// <summary>
  /// Gets or sets die ID der Organisation, zu der eingeladen wird.
  /// </summary>
  public Guid OrganizationId { get; set; }

  /// <summary>
  /// Gets or sets das Navigation-Property für die Organisation.
  /// </summary>
  public virtual Organization? Organization { get; set; }

  /// <summary>
  /// Gets or sets den Zeitpunkt, an dem die Einladung abläuft.
  /// </summary>
  public DateTime ExpiresAt { get; set; }

  /// <summary>
  /// Gets or sets a value indicating whether die Einladung bereits verwendet wurde.
  /// </summary>
  public bool IsUsed { get; set; }

  /// <summary>
  /// Gets or sets die ID des Benutzers, der die Einladung verwendet hat.
  /// </summary>
  public Guid? UsedByUserId { get; set; }

  /// <summary>
  /// Gets or sets die optionale E-Mail-Adresse, für die die Einladung bestimmt ist.
  /// </summary>
  public string? TargetedEmail { get; set; }
}
