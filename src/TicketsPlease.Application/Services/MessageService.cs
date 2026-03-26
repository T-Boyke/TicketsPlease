// <copyright file="MessageService.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Application.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TicketsPlease.Application.Common.Dtos;
using TicketsPlease.Application.Common.Interfaces;
using TicketsPlease.Domain.Entities;

/// <summary>
/// Implementiert die Geschäftslogik für die Nachrichtenverwaltung.
/// </summary>
public class MessageService : IMessageService
{
  private readonly IMessageRepository messageRepository;
  private readonly IFileStorageService fileStorageService;
  private readonly IFileAssetRepository fileAssetRepository;

  /// <summary>
  /// Initializes a new instance of the <see cref="MessageService"/> class.
  /// </summary>
  /// <param name="messageRepository">Das injizierte Repository.</param>
  /// <param name="fileStorageService">Der Dienst zur Dateispeicherung.</param>
  /// <param name="fileAssetRepository">Das Repository für Datei-Metadaten.</param>
  public MessageService(
      IMessageRepository messageRepository,
      IFileStorageService fileStorageService,
      IFileAssetRepository fileAssetRepository)
  {
    this.messageRepository = messageRepository;
    this.fileStorageService = fileStorageService;
    this.fileAssetRepository = fileAssetRepository;
  }

  /// <inheritdoc />
  public async Task<List<MessageDto>> GetUserMessagesAsync(Guid userId, CancellationToken ct = default)
  {
    var messages = await this.messageRepository.GetUserMessagesAsync(userId, ct).ConfigureAwait(false);
    return messages.Select(m => MapToDto(m)).ToList();
  }

  /// <inheritdoc />
  public async Task<MessageDto> SendMessageAsync(Guid senderId, CreateMessageDto dto, CancellationToken ct = default)
  {
    ArgumentNullException.ThrowIfNull(dto);

    var message = new Message
    {
      SenderUserId = senderId,
      ReceiverUserId = dto.ReceiverUserId,
      BodyMarkdown = dto.BodyMarkdown,
      SentAt = DateTime.UtcNow,
    };

    await this.messageRepository.AddAsync(message, ct).ConfigureAwait(false);
    await this.messageRepository.SaveChangesAsync(ct).ConfigureAwait(false);

    if (dto.Attachment != null)
    {
      await this.UploadAttachmentAsync(message.Id, dto.Attachment).ConfigureAwait(false);
    }

    // Fetch again to ensure navigation properties (Sender/Receiver/Attachments) are loaded
    var savedMessage = await this.messageRepository.GetByIdAsync(message.Id, ct).ConfigureAwait(false);
    return MapToDto(savedMessage!);
  }

  private static MessageDto MapToDto(Message m)
  {
    var attachments = m.Attachments?.Select(a => new FileAssetDto(
        a.Id,
        a.FileName,
        a.ContentType ?? "application/octet-stream",
        a.SizeBytes,
        a.UploadedAt,
        a.UploadedByUser?.UserName ?? "Unknown")) ?? Enumerable.Empty<FileAssetDto>();

    return new MessageDto(
        m.Id,
        m.SenderUserId,
        m.SenderUser?.UserName ?? "Unknown",
        m.ReceiverUserId,
        m.ReceiverUser?.UserName,
        m.BodyMarkdown,
        m.SentAt,
        attachments);
  }

  /// <inheritdoc />
  public async Task UploadAttachmentAsync(Guid messageId, Microsoft.AspNetCore.Http.IFormFile file)
  {
    ArgumentNullException.ThrowIfNull(file);

    var message = await this.messageRepository.GetByIdAsync(messageId).ConfigureAwait(false);
    if (message == null)
    {
      throw new KeyNotFoundException("Nachricht nicht gefunden.");
    }

    using var stream = file.OpenReadStream();
    var blobPath = await this.fileStorageService.SaveFileAsync(stream, file.FileName).ConfigureAwait(false);

    var asset = new FileAsset
    {
      Id = Guid.NewGuid(),
      FileName = file.FileName,
      ContentType = file.ContentType,
      SizeBytes = file.Length,
      BlobPath = blobPath,
      UploadedByUserId = message.SenderUserId,
      MessageId = messageId,
      UploadedAt = DateTime.UtcNow,
    };

    await this.fileAssetRepository.AddAsync(asset).ConfigureAwait(false);
    await this.fileAssetRepository.SaveChangesAsync().ConfigureAwait(false);
  }

  /// <inheritdoc />
  public async Task<List<MessageDto>> GetConversationAsync(Guid userId, Guid otherUserId, CancellationToken ct = default)
  {
    var messages = await this.messageRepository.GetConversationAsync(userId, otherUserId, ct).ConfigureAwait(false);
    return messages.Select(m => MapToDto(m)).ToList();
  }
}
