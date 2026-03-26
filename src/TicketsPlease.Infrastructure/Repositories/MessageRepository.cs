// <copyright file="MessageRepository.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Infrastructure.Repositories;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TicketsPlease.Application.Common.Interfaces;
using TicketsPlease.Domain.Entities;
using TicketsPlease.Infrastructure.Persistence;

/// <summary>
/// Implementiert den Datenzugriff für Nachrichten unter Verwendung von Entity Framework Core.
/// </summary>
public class MessageRepository : IMessageRepository
{
  private readonly AppDbContext context;

  /// <summary>
  /// Initializes a new instance of the <see cref="MessageRepository"/> class.
  /// </summary>
  /// <param name="context">Der Datenbankkontext.</param>
  public MessageRepository(AppDbContext context)
  {
    this.context = context;
  }

  /// <inheritdoc />
  public async Task<Message?> GetByIdAsync(Guid id, CancellationToken ct = default)
  {
    return await this.context.Messages
        .Include(m => m.SenderUser)
        .Include(m => m.ReceiverUser)
        .FirstOrDefaultAsync(m => m.Id == id, ct).ConfigureAwait(false);
  }

  /// <inheritdoc />
  public async Task<List<Message>> GetUserMessagesAsync(Guid userId, CancellationToken ct = default)
  {
    return await this.context.Messages
        .AsNoTracking()
        .Where(m => m.SenderUserId == userId || m.ReceiverUserId == userId)
        .Include(m => m.SenderUser)
        .Include(m => m.ReceiverUser)
        .OrderByDescending(m => m.SentAt)
        .ToListAsync(ct).ConfigureAwait(false);
  }

  /// <inheritdoc />
  public async Task AddAsync(Message message, CancellationToken ct = default)
  {
    await this.context.Messages.AddAsync(message, ct).ConfigureAwait(false);
  }

  /// <inheritdoc />
  public async Task SaveChangesAsync(CancellationToken ct = default)
  {
    await this.context.SaveChangesAsync(ct).ConfigureAwait(false);
  }
}
