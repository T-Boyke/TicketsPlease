// <copyright file="NotificationsController.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Web.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Controller for viewing in-app notifications.
/// </summary>
[Authorize]
public sealed class NotificationsController : Controller
{
    /// <summary>
    /// Displays the notifications center.
    /// </summary>
    /// <returns>The notifications index view.</returns>
    [HttpGet]
    public IActionResult Index()
    {
        return this.View();
    }
}
