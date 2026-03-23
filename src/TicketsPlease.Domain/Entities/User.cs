// <copyright file="User.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TicketsPlease.Domain.Entities;

using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using TicketsPlease.Domain.Common;

/// <summary>
/// Repräsentiert einen Benutzer im Ticketsystem.
/// Erbt von <see cref="IdentityUser{TKey}"/> für ASP.NET Core Identity Integration.
/// </summary>
public class User : IdentityUser<Guid>
{
  /// <summary>
  /// Gets or sets den Login-Namen (Alias for UserName).
  /// </summary>
  [NotMapped]
  public string Username { get => this.UserName ?? string.Empty; set => this.UserName = value; }

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
  /// Gets or sets den Fremdschlüssel zur Organisation (Mandantenfähigkeit).
  /// </summary>
  public Guid TenantId { get; set; }

  /// <summary>
  /// Gets or sets die Version für die Nebenläufigkeitskontrolle.
  /// </summary>
  public byte[] RowVersion { get; set; } = Array.Empty<byte>();

  /// <summary>
  /// Gets or sets die ID der zugewiesenen Rolle.
  /// </summary>
  public Guid RoleId { get; set; }

  /// <summary>
  /// Gets or sets das Navigation-Property zur Rolle.
  /// </summary>
  public Role? Role { get; set; }

  /// <summary>
  /// Gets or sets das zugehörige Benutzerprofil (1:1).
  /// </summary>
  public virtual UserProfile? Profile { get; set; }
}
