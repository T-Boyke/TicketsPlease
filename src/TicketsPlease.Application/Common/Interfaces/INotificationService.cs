// <copyright file="INotificationService.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Application.Common.Interfaces;

using System;
using System.Threading.Tasks;
using TicketsPlease.Application.Common.Dtos;

/// <summary>
/// Domain-neutraler Dienst für Echtzeit-Benachrichtigungen.
/// </summary>
public interface INotificationService
{
  /// <summary>
  /// Sendet eine Benachrichtigung an einen spezifischen Benutzer.
  /// </summary>
  /// <param name="userId">Die ID des Benutzers.</param>
  /// <param name="title">Der Titel der Benachrichtigung.</param>
  /// <param name="message">Die Nachricht.</param>
  /// <param name="link">Optionaler Link.</param>
  /// <returns>Ein Task.</returns>
  public Task SendNotificationToUserAsync(Guid userId, string title, string message, string? link = null);

  /// <summary>
  /// Sendet eine Benachrichtigung an alle verbundenen Benutzer.
  /// </summary>
  /// <param name="title">Der Titel der Benachrichtigung.</param>
  /// <param name="message">Die Nachricht.</param>
  /// <returns>Ein Task.</returns>
  public Task SendNotificationToAllAsync(string title, string message);

  /// <summary>
  /// Informiert Teilnehmer eines Tickets über eine Aktualisierung.
  /// </summary>
  /// <param name="ticketId">Die ID des Tickets.</param>
  /// <param name="message">Die Update-Nachricht.</param>
  /// <returns>Ein Task.</returns>
  public Task NotifyTicketUpdateAsync(Guid ticketId, string message);

  /// <summary>
  /// Informiert Teilnehmer eines Tickets über einen neuen Kommentar.
  /// </summary>
  /// <param name="ticketId">Die ID des Tickets.</param>
  /// <param name="comment">Der neue Kommentar.</param>
  /// <returns>Ein Task.</returns>
  public Task NotifyNewCommentAsync(Guid ticketId, CommentDto comment);

  /// <summary>
  /// Sendet eine Benachrichtigung über eine neue Privatnachricht.
  /// </summary>
  /// <param name="receiverUserId">Die ID des Empfängers.</param>
  /// <param name="message">Das Nachrichten-DTO.</param>
  /// <returns>Ein Task.</returns>
  public Task NotifyNewMessageAsync(Guid receiverUserId, MessageDto message);
}
