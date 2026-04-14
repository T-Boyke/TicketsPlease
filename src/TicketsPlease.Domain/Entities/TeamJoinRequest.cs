// <copyright file="TeamJoinRequest.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Domain.Entities;

using System;
using TicketsPlease.Domain.Common;
using TicketsPlease.Domain.Enums;

/// <summary>
/// Repräsentiert eine Anfrage eines Benutzers, einem Team beizutreten.
/// </summary>
public class TeamJoinRequest : BaseEntity
{
  /// <summary>
  /// Gets or sets die ID des Teams.
  /// </summary>
  public Guid TeamId { get; set; }

  /// <summary>
  /// Gets or sets das zugehörige Team.
  /// </summary>
  public Team? Team { get; set; }

  /// <summary>
  /// Gets or sets die ID des anfragenden Benutzers.
  /// </summary>
  public Guid UserId { get; set; }

  /// <summary>
  /// Gets or sets der anfragende Benutzer.
  /// </summary>
  public User? User { get; set; }

  /// <summary>
  /// Gets or sets der Status der Anfrage.
  /// </summary>
  public JoinRequestStatus Status { get; set; } = JoinRequestStatus.Pending;

  /// <summary>
  /// Gets or sets der Zeitpunkt der Anfrage.
  /// </summary>
  public DateTime RequestedAt { get; set; } = DateTime.UtcNow;

  /// <summary>
  /// Gets or sets der Zeitpunkt der Entscheidung.
  /// </summary>
  public DateTime? DecidedAt { get; set; }

  /// <summary>
  /// Gets or sets die ID des Benutzers, der die Entscheidung getroffen hat.
  /// </summary>
  public Guid? DecidedByUserId { get; set; }

  /// <summary>
  /// Gets or sets der Benutzer, der die Entscheidung getroffen hat.
  /// </summary>
  public User? DecidedByUser { get; set; }
}
