// <copyright file="UserListViewModel.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Web.Controllers;

using System;
using System.Collections.Generic;

/// <summary>
/// ViewModel für die Benutzerübersicht im Administrationsbereich.
/// </summary>
internal class UserListViewModel
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
  /// Gets or sets die zugewiesenen Rollen.
  /// </summary>
  public List<string> Roles { get; set; } = new();

  /// <summary>
  /// Gets or sets a value indicating whether der Benutzer aktiv ist.
  /// </summary>
  public bool IsActive { get; set; }
}
