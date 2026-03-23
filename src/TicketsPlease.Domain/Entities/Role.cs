// <copyright file="Role.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TicketsPlease.Domain.Entities;

using TicketsPlease.Domain.Common;

/// <summary>
/// Repräsentiert eine Benutzerrolle im System zur Berechtigungssteuerung.
/// </summary>
public class Role : BaseEntity
{
  /// <summary>
  /// Gets or sets den Namen der Rolle (z.B. Admin, Agent, User).
  /// </summary>
  public string Name { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets die detaillierte Beschreibung der Rolle und ihrer Berechtigungen.
  /// </summary>
  public string Description { get; set; } = string.Empty;
}
