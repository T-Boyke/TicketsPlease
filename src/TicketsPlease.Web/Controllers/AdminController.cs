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
public class AdminController : Controller
{
    /// <summary>
    /// Leitet zur Benutzerverwaltung weiter.
    /// </summary>
    /// <returns>Ein Redirect auf die Benutzerliste.</returns>
    public IActionResult Index()
    {
        return RedirectToAction(nameof(Users));
    }

    /// <summary>
    /// Zeigt die Benutzerverwaltung an.
    /// </summary>
    /// <returns>Die Users-View.</returns>
    public IActionResult Users()
    {
        return View();
    }

    /// <summary>
    /// Zeigt die Systemeinstellungen an.
    /// </summary>
    /// <returns>Die Settings-View.</returns>
    public IActionResult Settings()
    {
        return View();
    }
}
