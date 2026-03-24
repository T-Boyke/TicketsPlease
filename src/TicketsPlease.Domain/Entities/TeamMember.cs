// <copyright file="TeamMember.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Domain.Entities;

using System;
using TicketsPlease.Domain.Common;

/// <summary>
/// Repräsentiert die Mitgliedschaft eines Benutzers in einem spezifischen Team.
/// </summary>
public class TeamMember : BaseEntity
{
  /// <summary>
  /// Gets or sets die ID des Teams.
  /// </summary>
  public Guid TeamId { get; set; }

  /// <summary>
  /// Gets or sets das Navigation-Property für das zugehörige Team.
  /// </summary>
  public Team? Team { get; set; }

  /// <summary>
  /// Gets or sets die ID des Mitglieds-Benutzers.
  /// </summary>
  public Guid UserId { get; set; }

  /// <summary>
  /// Gets or sets das Navigation-Property für den zugeordneten Benutzer.
  /// </summary>
  public User? User { get; set; }

  /// <summary>
  /// Gets or sets den Zeitpunkt (UTC) des Beitritts in das Team.
  /// </summary>
  public DateTime JoinedAt { get; set; } = DateTime.UtcNow;

  /// <summary>
  /// Gets or sets a value indicating whether dieser Benutzer der Teamleiter ist.
  /// </summary>
  public bool IsTeamLead { get; set; }
}
