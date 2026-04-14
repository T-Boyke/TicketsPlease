// <copyright file="TimeLogRepository.cs" company="BitLC-NE-2025-2026">
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
/// Implementierung des ITimeLogRepository für den Zugriff auf Zeiterfassungsdaten.
/// </summary>
public class TimeLogRepository : ITimeLogRepository
{
    private readonly AppDbContext context;

    /// <summary>
    /// Initializes a new instance of the <see cref="TimeLogRepository"/> class.
    /// </summary>
    /// <param name="context">Der Datenbankkontext.</param>
    public TimeLogRepository(AppDbContext context)
    {
        this.context = context;
    }

    /// <inheritdoc/>
    public async Task<List<TimeLog>> GetAllAsync(CancellationToken ct = default)
    {
        return await this.context.TimeLogs
            .AsNoTracking()
            .ToListAsync(ct)
            .ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<List<TimeLog>> GetByTenantAsync(Guid tenantId, CancellationToken ct = default)
    {
        return await this.context.TimeLogs
            .IgnoreQueryFilters()
            .AsNoTracking()
            .Where(l => l.TenantId == tenantId)
            .ToListAsync(ct)
            .ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<List<TimeLog>> GetByUserIdAsync(Guid userId, CancellationToken ct = default)
    {
        return await this.context.TimeLogs
            .AsNoTracking()
            .Where(l => l.UserId == userId)
            .ToListAsync(ct)
            .ConfigureAwait(false);
    }
}
