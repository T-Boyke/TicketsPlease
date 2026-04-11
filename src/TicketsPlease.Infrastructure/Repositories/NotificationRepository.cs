// <copyright file="NotificationRepository.cs" company="BitLC-NE-2025-2026">
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
/// Repository-Implementierung für den Zugriff auf <see cref="Notification"/>-Entitäten mit EF Core.
/// </summary>
public class NotificationRepository : INotificationRepository
{
    private readonly AppDbContext context;

    /// <summary>
    /// Initializes a new instance of the <see cref="NotificationRepository"/> class.
    /// </summary>
    /// <param name="context">Der Datenbankkontext.</param>
    public NotificationRepository(AppDbContext context)
    {
        this.context = context;
    }

    /// <inheritdoc />
    public async Task<List<Notification>> GetByUserIdAsync(Guid userId, int limit = 20, int offset = 0, CancellationToken ct = default)
    {
        return await this.context.Notifications
            .AsNoTracking()
            .Where(n => n.UserId == userId)
            .OrderByDescending(n => n.CreatedAt)
            .Skip(offset)
            .Take(limit)
            .ToListAsync(ct)
            .ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<Notification?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await this.context.Notifications
            .FirstOrDefaultAsync(n => n.Id == id, ct)
            .ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        return await this.context.SaveChangesAsync(ct).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task AddAsync(Notification notification, CancellationToken ct = default)
    {
        await this.context.Notifications.AddAsync(notification, ct).ConfigureAwait(false);
    }
}
