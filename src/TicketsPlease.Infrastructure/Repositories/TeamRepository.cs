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
    // The entity is already tracked by the context, so EF Core will automatically
    // detect changes during SaveChangesAsync. Calling Update() on a tracked entity
    // with Include() navigations forces the entire graph to Modified, causing ConcurrencyExceptions.
    await Task.CompletedTask.ConfigureAwait(false);
  }

  /// <inheritdoc/>
  public async Task DeleteAsync(Team team)
  {
    this.context.Teams.Remove(team);
    await Task.CompletedTask.ConfigureAwait(false);
  }

  /// <inheritdoc/>
  public async Task AddJoinRequestAsync(TeamJoinRequest request, CancellationToken cancellationToken = default)
  {
    await this.context.TeamJoinRequests.AddAsync(request, cancellationToken).ConfigureAwait(false);
  }

  /// <inheritdoc/>
  public async Task UpdateJoinRequestAsync(TeamJoinRequest request, CancellationToken cancellationToken = default)
  {
    // The entity is already tracked by the context.
    await Task.CompletedTask.ConfigureAwait(false);
  }

  /// <inheritdoc/>
  public async Task<TeamJoinRequest?> GetJoinRequestByIdAsync(Guid requestId, CancellationToken cancellationToken = default)
  {
    return await this.context.TeamJoinRequests
        .Include(r => r.Team)
        .Include(r => r.User)
        .Include(r => r.DecidedByUser)
        .FirstOrDefaultAsync(r => r.Id == requestId, cancellationToken)
        .ConfigureAwait(false);
  }

  /// <inheritdoc/>
  public async Task<IEnumerable<TeamJoinRequest>> GetJoinRequestsByTeamIdAsync(Guid teamId, CancellationToken cancellationToken = default)
  {
    return await this.context.TeamJoinRequests
        .Include(r => r.User)
        .Where(r => r.TeamId == teamId)
        .ToListAsync(cancellationToken)
        .ConfigureAwait(false);
  }

  /// <inheritdoc/>
  public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
  {
    return await this.context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
  }

  /// <inheritdoc/>
  public async Task<IEnumerable<Team>> GetTeamsByTenantAsync(Guid tenantId)
  {
    return await this.context.Teams
        .IgnoreQueryFilters()
        .Include(t => t.Members)
        .Where(t => t.TenantId == tenantId)
        .ToListAsync()
        .ConfigureAwait(false);
  }
}
