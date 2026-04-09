// <copyright file="EditUserViewModel.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Web.Controllers;

using System;
using System.Collections.Generic;

/// <summary>
/// ViewModel zum Bearbeiten eines Benutzers im Administrationsbereich.
/// </summary>
internal class EditUserViewModel
{
  /// <summary>
  /// Gets or sets die Benutzer-ID.
  /// </summary>
  public Guid Id { get; set; }

  /// <summary>
  /// Gets or sets den Benutzernamen.
  /// </summary>
  public string UserName { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets die E-Mail-Adresse.
  /// </summary>
  public string Email { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets die aktuellen Rollen des Benutzers.
  /// </summary>
  public List<string> UserRoles { get; set; } = new();

  /// <summary>
  /// Gets or sets alle verfügbaren Rollen.
  /// </summary>
  public List<string> AllRoles { get; set; } = new();

  /// <summary>
  /// Gets or sets die ausgewählten Rollen.
  /// </summary>
  public List<string> SelectedRoles { get; set; } = new();

  public string? Position { get; set; }
  public string? TechStack { get; set; }
  public string? Street { get; set; }
  public string? HouseNumber { get; set; }
  public string? City { get; set; }
  public string? Country { get; set; }
}
