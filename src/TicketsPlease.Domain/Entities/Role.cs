// <copyright file="Role.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TicketsPlease.Domain.Entities;

using System;
using Microsoft.AspNetCore.Identity;

/// <summary>
/// Repräsentiert eine Benutzerrolle im System zur Berechtigungssteuerung.
/// Erbt von <see cref="IdentityRole{TKey}"/> für ASP.NET Core Identity Integration.
/// </summary>
public class Role : IdentityRole<Guid>
{
  /// <summary>
  /// Gets or sets die detaillierte Beschreibung der Rolle und ihrer Berechtigungen.
  /// </summary>
  public string Description { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets a value indicating whether die Entität gelöscht wurde (Soft Delete).
  /// </summary>
  public bool IsDeleted { get; set; }

  /// <summary>
  /// Gets or sets die Version für die Nebenläufigkeitskontrolle.
  /// </summary>
  public byte[] RowVersion { get; set; } = Array.Empty<byte>();
}
