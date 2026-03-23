// <copyright file="User.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TicketsPlease.Domain.Entities;

using System;
using TicketsPlease.Domain.Common;

/// <summary>
/// Repräsentiert einen Benutzer im Ticketsystem.
/// Erbt von <see cref="BaseEntity"/> für die ID und Nebenläufigkeitskontrolle.
/// </summary>
public class User : BaseEntity
{
  /// <summary>
  /// Gets or sets den Anzeigenamen des Benutzers.
  /// </summary>
  public string DisplayName { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets den eindeutigen Login-Namen.
  /// </summary>
  public string Username { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets die E-Mail-Adresse des Benutzers.
  /// </summary>
  public string Email { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets den Passwort-Hash.
  /// </summary>
  public string PasswordHash { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets den Erstellungszeitpunkt.
  /// </summary>
  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

  /// <summary>
  /// Gets or sets den Zeitpunkt des letzten Logins.
  /// </summary>
  public DateTime? LastLoginAt { get; set; }

  /// <summary>
  /// Gets or sets a value indicating whether der Benutzer aktiv ist.
  /// </summary>
  public bool IsActive { get; set; } = true;

  /// <summary>
  /// Gets or sets a value indicating whether der das den Online-Status angibt.
  /// </summary>
  public bool IsOnline { get; set; }

  /// <summary>
  /// Gets or sets die ID der zugewiesenen Rolle.
  /// </summary>
  public Guid RoleId { get; set; }

  /// <summary>
  /// Gets or sets das Navigation-Property zur Rolle.
  /// </summary>
  public Role? Role { get; set; }
}
