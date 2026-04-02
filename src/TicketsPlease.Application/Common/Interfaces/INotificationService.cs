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
    Task SendNotificationToUserAsync(Guid userId, string title, string message, string? link = null);

    /// <summary>
    /// Sendet eine Benachrichtigung an alle verbundenen Benutzer.
    /// </summary>
    Task SendNotificationToAllAsync(string title, string message);

    /// <summary>
    /// Informiert Teilnehmer eines Tickets über eine Aktualisierung.
    /// </summary>
    Task NotifyTicketUpdateAsync(Guid ticketId, string message);

    /// <summary>
    /// Informiert Teilnehmer eines Tickets über einen neuen Kommentar.
    /// </summary>
    Task NotifyNewCommentAsync(Guid ticketId, CommentDto comment);

    /// <summary>
    /// Sendet eine Benachrichtigung über eine neue Privatnachricht.
    /// </summary>
    /// <param name="receiverUserId">Die ID des Empfängers.</param>
    /// <param name="message">Das Nachrichten-DTO.</param>
    /// <returns>Ein Task.</returns>
    Task NotifyNewMessageAsync(Guid receiverUserId, MessageDto message);
}
