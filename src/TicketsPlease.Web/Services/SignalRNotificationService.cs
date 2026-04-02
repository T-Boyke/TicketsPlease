// <copyright file="SignalRNotificationService.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Web.Services;

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using TicketsPlease.Application.Common.Dtos;
using TicketsPlease.Application.Common.Interfaces;
using TicketsPlease.Web.Hubs;

/// <summary>
/// Implementierung des Benachrichtigungs-Dienstes via SignalR Hub.
/// </summary>
public class SignalRNotificationService : INotificationService
{
    private readonly IHubContext<NotificationHub> hubContext;

    /// <summary>
    /// Initialisiert eine neue Instanz des <see cref="SignalRNotificationService"/>.
    /// </summary>
    /// <param name="hubContext">Der Hub-Kontext.</param>
    public SignalRNotificationService(IHubContext<NotificationHub> hubContext)
    {
        this.hubContext = hubContext;
    }

    /// <inheritdoc/>
    public async Task SendNotificationToUserAsync(Guid userId, string title, string message, string? link = null)
    {
        await this.hubContext.Clients.User(userId.ToString()).SendAsync("ReceiveNotification", new { title, message, link }).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task SendNotificationToAllAsync(string title, string message)
    {
        await this.hubContext.Clients.All.SendAsync("ReceiveGlobalAlert", new { title, message }).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task NotifyTicketUpdateAsync(Guid ticketId, string message)
    {
        await this.hubContext.Clients.Group($"ticket_{ticketId}").SendAsync("TicketUpdated", new { ticketId, message }).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task NotifyNewCommentAsync(Guid ticketId, CommentDto comment)
    {
        await this.hubContext.Clients.Group($"ticket_{ticketId}").SendAsync("NewComment", new { ticketId, comment }).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task NotifyNewMessageAsync(Guid receiverUserId, MessageDto message)
    {
        // Wir senden die Nachricht direkt an den spezifischen User (SignalR User ID = Identity Name)
        // Hinweis: Standardmäßig nutzt SignalR ClaimsTypes.NameIdentifier oder Name
        await this.hubContext.Clients.User(receiverUserId.ToString()).SendAsync("ReceiveMessage", message).ConfigureAwait(false);
    }
}
