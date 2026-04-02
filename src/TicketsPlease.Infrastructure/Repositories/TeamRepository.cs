// <copyright file="TeamRepository.cs" company="BitLC-NE-2025-2026">
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
/// Repository-Implementierung für den Datenzugriff auf Teams.
/// </summary>
public class TeamRepository : ITeamRepository
{
    private readonly AppDbContext context;

    /// <summary>
    /// Initializes a new instance of the <see cref="TeamRepository"/> class.
    /// </summary>
    /// <param name="context">Der Datenbankkontext.</param>
    public TeamRepository(AppDbContext context)
    {
        this.context = context;
    }

    /// <inheritdoc/>
    public async Task<Team?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await this.context.Teams
            .Include(t => t.Members)
            .ThenInclude(m => m.User)
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Team>> GetTeamsByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await this.context.Teams
            .Include(t => t.Members)
            .Where(t => t.Members.Any(m => m.UserId == userId))
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Team>> GetAllTeamsAsync(CancellationToken cancellationToken = default)
    {
        return await this.context.Teams
            .Include(t => t.Members)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task AddAsync(Team team, CancellationToken cancellationToken = default)
    {
        await this.context.Teams.AddAsync(team, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task UpdateAsync(Team team, CancellationToken cancellationToken = default)
    {
        this.context.Teams.Update(team);
        await Task.CompletedTask.ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task DeleteAsync(Team team)
    {
        this.context.Teams.Remove(team);
        await Task.CompletedTask.ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await this.context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }
}
