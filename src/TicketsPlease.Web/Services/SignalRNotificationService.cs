// <copyright file="SignalRNotificationService.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Web.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using TicketsPlease.Application.Common.Dtos;
using TicketsPlease.Application.Common.Interfaces;
using TicketsPlease.Domain.Entities;
using TicketsPlease.Web.Hubs;

/// <summary>
/// Implementierung des Benachrichtigungs-Dienstes via SignalR Hub.
/// </summary>
public class SignalRNotificationService : INotificationService
{
  private readonly IHubContext<NotificationHub> hubContext;
  private readonly INotificationRepository repository;

  /// <summary>
  /// Initializes a new instance of the <see cref="SignalRNotificationService"/> class.
  /// Initialisiert eine neue Instanz des <see cref="SignalRNotificationService"/>.
  /// </summary>
  /// <param name="hubContext">Der Hub-Kontext.</param>
  /// <param name="repository">Das Benachrichtigungs-Repository.</param>
  public SignalRNotificationService(IHubContext<NotificationHub> hubContext, INotificationRepository repository)
  {
    this.hubContext = hubContext;
    this.repository = repository;
  }

  /// <inheritdoc/>
  public async Task SendNotificationToUserAsync(Guid userId, string title, string message, string? link = null)
  {
    var notification = new Notification
    {
      UserId = userId,
      Title = title,
      Content = message,
      TargetUrl = link,
      IsRead = false,
      CreatedAt = DateTime.UtcNow,
    };

    await this.repository.AddAsync(notification).ConfigureAwait(false);
    await this.repository.SaveChangesAsync().ConfigureAwait(false);

    await this.hubContext.Clients.User(userId.ToString()).SendAsync("ReceiveNotification", new { id = notification.Id, title, message, link }).ConfigureAwait(false);
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

  /// <inheritdoc/>
  public async Task<List<NotificationDto>> GetNotificationsForUserAsync(Guid userId, int limit = 20, int offset = 0)
  {
    var notifications = await this.repository.GetByUserIdAsync(userId, limit, offset).ConfigureAwait(false);
    return notifications.Select(n => new NotificationDto
    {
      Id = n.Id,
      Title = n.Title,
      Content = n.Content,
      TargetUrl = n.TargetUrl,
      IsRead = n.IsRead,
      CreatedAt = n.CreatedAt,
    }).ToList();
  }

  /// <inheritdoc/>
  public async Task MarkAsReadAsync(Guid notificationId)
  {
    var notification = await this.repository.GetByIdAsync(notificationId).ConfigureAwait(false);
    if (notification != null)
    {
      notification.IsRead = true;
      await this.repository.SaveChangesAsync().ConfigureAwait(false);
    }
  }

  /// <inheritdoc/>
  public async Task MarkAllAsReadAsync(Guid userId)
  {
    var notifications = await this.repository.GetByUserIdAsync(userId, limit: int.MaxValue).ConfigureAwait(false);
    foreach (var notification in notifications.Where(n => !n.IsRead))
    {
      notification.IsRead = true;
    }

    await this.repository.SaveChangesAsync().ConfigureAwait(false);
  }
}
