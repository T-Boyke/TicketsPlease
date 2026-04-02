// <copyright file="NotificationHub.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Web.Hubs;

using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

/// <summary>
/// SignalR Hub für Echtzeit-Benachrichtigungen und Kollaboration.
/// </summary>
[Authorize]
internal class NotificationHub : Hub
{
  private static readonly System.Collections.Concurrent.ConcurrentDictionary<string, System.Collections.Generic.HashSet<string>> PresenceTracker = new();

  /// <summary>
  /// Tritt einer Gruppe für ein spezifisches Ticket bei (für Live-Updates/Präsenz).
  /// </summary>
  /// <param name="ticketId">Die Ticket-ID.</param>
  /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
  public async Task JoinTicketGroup(string ticketId)
  {
    var groupName = $"ticket_{ticketId}";
    await this.Groups.AddToGroupAsync(this.Context.ConnectionId, groupName).ConfigureAwait(false);

    var username = this.Context.User?.Identity?.Name ?? "Unbekannt";
    var groupUsers = PresenceTracker.GetOrAdd(groupName, _ => new System.Collections.Generic.HashSet<string>());
    lock (groupUsers)
    {
      groupUsers.Add(username);
    }

    await this.Clients.Group(groupName).SendAsync("PresenceUpdated", groupUsers).ConfigureAwait(false);
  }

  /// <summary>
  /// Verlässt die Gruppe für ein spezifisches Ticket.
  /// </summary>
  /// <param name="ticketId">Die Ticket-ID.</param>
  /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
  public async Task LeaveTicketGroup(string ticketId)
  {
    var groupName = $"ticket_{ticketId}";
    await this.Groups.RemoveFromGroupAsync(this.Context.ConnectionId, groupName).ConfigureAwait(false);

    var username = this.Context.User?.Identity?.Name ?? "Unbekannt";
    if (PresenceTracker.TryGetValue(groupName, out var groupUsers))
    {
      lock (groupUsers)
      {
        groupUsers.Remove(username);
      }

      await this.Clients.Group(groupName).SendAsync("PresenceUpdated", groupUsers).ConfigureAwait(false);
    }
  }

  /// <inheritdoc/>
  public override async Task OnDisconnectedAsync(System.Exception? exception)
  {
    var username = this.Context.User?.Identity?.Name;
    if (username != null)
    {
      foreach (var group in PresenceTracker)
      {
        lock (group.Value)
        {
          if (group.Value.Remove(username))
          {
            _ = this.Clients.Group(group.Key).SendAsync("PresenceUpdated", group.Value);
          }
        }
      }
    }

    await base.OnDisconnectedAsync(exception).ConfigureAwait(false);
  }
}
