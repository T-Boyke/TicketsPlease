// <copyright file="IMessageService.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Application.Common.Interfaces;

using TicketsPlease.Application.Common.Dtos;

/// <summary>
/// Definiert die Geschäftslogik für die Nachrichtenverwaltung.
/// </summary>
public interface IMessageService
{
  /// <summary>
  /// Ruft alle Nachrichten für den aktuellen Benutzer ab.
  /// </summary>
  /// <param name="userId">Die ID des Benutzers.</param>
  /// <param name="ct">Das Abbruchsignal.</param>
  /// <returns>Eine Liste von Nachrichten-DTOs.</returns>
  public Task<List<MessageDto>> GetUserMessagesAsync(Guid userId, CancellationToken ct = default);

  /// <summary>
  /// Sendet eine neue Nachricht.
  /// </summary>
  /// <param name="senderId">Die ID des Absenders.</param>
  /// <param name="dto">Das DTO mit den Nachrichtendaten.</param>
  /// <param name="ct">Das Abbruchsignal.</param>
  /// <returns>Das DTO der erstellten Nachricht.</returns>
  public Task<MessageDto> SendMessageAsync(Guid senderId, CreateMessageDto dto, CancellationToken ct = default);

  /// <summary>
  /// Lädt einen Anhang für eine Nachricht hoch.
  /// </summary>
  /// <param name="messageId">Die ID der Nachricht.</param>
  /// <param name="file">Die hochzuladende Datei.</param>
  /// <returns>Ein Task.</returns>
  public Task UploadAttachmentAsync(Guid messageId, Microsoft.AspNetCore.Http.IFormFile file);

  /// <summary>
  /// Ruft die Konversation zwischen zwei Benutzern ab.
  /// </summary>
  /// <param name="userId">Die ID des ersten Benutzers.</param>
  /// <param name="otherUserId">Die ID des zweiten Benutzers.</param>
  /// <param name="ct">Das Abbruchsignal.</param>
  /// <returns>Eine Liste von Nachrichten-DTOs.</returns>
  public Task<List<MessageDto>> GetConversationAsync(Guid userId, Guid otherUserId, CancellationToken ct = default);
}
