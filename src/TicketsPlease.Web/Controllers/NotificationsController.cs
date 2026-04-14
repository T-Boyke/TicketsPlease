// <copyright file="NotificationsController.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Web.Controllers;

using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketsPlease.Application.Common.Interfaces;
using TicketsPlease.Web.Models;

/// <summary>
/// Controller for viewing in-app notifications.
/// </summary>
[Authorize]
public sealed class NotificationsController : Controller
{
    private readonly INotificationService notificationService;

    /// <summary>
    /// Initializes a new instance of the <see cref="NotificationsController"/> class.
    /// </summary>
    /// <param name="notificationService">Der Benachrichtigungsdienst.</param>
    public NotificationsController(INotificationService notificationService)
    {
        this.notificationService = notificationService;
    }

    /// <summary>
    /// Displays the notifications center.
    /// </summary>
    /// <param name="offset">Paging offset.</param>
    /// <returns>The notifications index view.</returns>
    [HttpGet]
    public async Task<IActionResult> Index(int offset = 0)
    {
        var userId = this.GetUserId();
        const int limit = 20;
        var notifications = await this.notificationService.GetNotificationsForUserAsync(userId, limit + 1, offset).ConfigureAwait(false);

        var model = new NotificationsViewModel
        {
            Notifications = notifications.Take(limit).ToList(),
            HasMore = notifications.Count > limit,
        };

        if (this.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
        {
            return this.PartialView("_NotificationList", model);
        }

        return this.View(model);
    }

    /// <summary>
    /// Markiert eine Benachrichtigung als gelesen.
    /// </summary>
    /// <param name="id">Die ID.</param>
    /// <returns>Redirect.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> MarkAsRead(Guid id)
    {
        await this.notificationService.MarkAsReadAsync(id).ConfigureAwait(false);
        return this.Ok();
    }

    /// <summary>
    /// Markiert alle als gelesen.
    /// </summary>
    /// <returns>Redirect.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> MarkAllAsRead()
    {
        var userId = this.GetUserId();
        await this.notificationService.MarkAllAsReadAsync(userId).ConfigureAwait(false);
        return this.RedirectToAction(nameof(this.Index));
    }

    private Guid GetUserId()
    {
        var userIdClaim = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.TryParse(userIdClaim, out var guid) ? guid : Guid.Empty;
    }
}
