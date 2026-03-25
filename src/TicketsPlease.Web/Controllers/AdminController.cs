// <copyright file="AdminController.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Web.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Haupt-Controller für den Administrationsbereich.
/// Erfordert die Admin-Rolle.
/// </summary>
[Authorize(Roles = "Admin")]
internal sealed class AdminController : Controller
{
  /// <summary>
  /// Leitet zur Benutzerverwaltung weiter.
  /// </summary>
  /// <returns>Ein Redirect auf die Benutzerliste.</returns>
  [HttpGet]
  public IActionResult Index()
  {
    return this.RedirectToAction(nameof(this.Users));
  }

  /// <summary>
  /// Zeigt die Benutzerverwaltung an.
  /// </summary>
  /// <returns>Die Users-View.</returns>
  [HttpGet]
  public IActionResult Users()
  {
    return this.View();
  }

  /// <summary>
  /// Zeigt die Systemeinstellungen an.
  /// </summary>
  /// <returns>Die Settings-View.</returns>
  [HttpGet]
  public IActionResult Settings()
  {
    return this.View();
  }
}
