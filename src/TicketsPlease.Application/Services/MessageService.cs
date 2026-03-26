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

    /// <summary>
    /// Initializes a new instance of the <see cref="MessageService"/> class.
    /// </summary>
    /// <param name="messageRepository">Das injizierte Repository.</param>
    public MessageService(IMessageRepository messageRepository)
    {
        this.messageRepository = messageRepository;
    }

    /// <inheritdoc />
    public async Task<List<MessageDto>> GetUserMessagesAsync(Guid userId, CancellationToken ct = default)
    {
        var messages = await this.messageRepository.GetUserMessagesAsync(userId, ct).ConfigureAwait(false);
        return messages.Select(m => new MessageDto(
            m.Id,
            m.SenderUserId,
            m.SenderUser?.UserName ?? "Unknown",
            m.ReceiverUserId,
            m.ReceiverUser?.UserName,
            m.BodyMarkdown,
            m.SentAt)).ToList();
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

        // Map back to DTO. In a real project we would fetch to get names, but for now we return what we have or fetch it.
        // Let's fetch it to ensure navigation properties are loaded.
        var savedMessage = await this.messageRepository.GetByIdAsync(message.Id, ct).ConfigureAwait(false);

        return new MessageDto(
            savedMessage!.Id,
            savedMessage.SenderUserId,
            savedMessage.SenderUser?.UserName ?? "Unknown",
            savedMessage.ReceiverUserId,
            savedMessage.ReceiverUser?.UserName,
            savedMessage.BodyMarkdown,
            savedMessage.SentAt);
    }
}
