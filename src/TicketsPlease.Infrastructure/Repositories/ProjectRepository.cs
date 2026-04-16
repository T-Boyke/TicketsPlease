// <copyright file="ProjectRepository.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Infrastructure.Repositories;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TicketsPlease.Application.Common.Interfaces;
using TicketsPlease.Domain.Entities;
using TicketsPlease.Infrastructure.Persistence;

/// <summary>
/// Implementiert den Datenzugriff für Projekte unter Verwendung von Entity Framework Core.
/// </summary>
public class ProjectRepository : IProjectRepository
{
  private readonly AppDbContext context;

  /// <summary>
  /// Initializes a new instance of the <see cref="ProjectRepository"/> class.
  /// </summary>
  /// <param name="context">Der Datenbankkontext.</param>
  public ProjectRepository(AppDbContext context)
  {
    this.context = context;
  }

  /// <inheritdoc/>
  public async Task<Project?> GetByIdAsync(Guid id)
  {
    return await this.context.Projects
        .Include(p => p.Tickets)
        .FirstOrDefaultAsync(p => p.Id == id).ConfigureAwait(false);
  }

  /// <inheritdoc/>
  public async Task<IEnumerable<Project>> GetAllAsync(Guid tenantId)
  {
    return await this.context.Projects
        .IgnoreQueryFilters()
        .AsNoTracking()
        .Where(p => p.TenantId == tenantId)
        .OrderByDescending(p => p.StartDate)
        .ToListAsync().ConfigureAwait(false);
  }

  /// <inheritdoc/>
  public async Task AddAsync(Project project)
  {
    await this.context.Projects.AddAsync(project).ConfigureAwait(false);
    await this.context.SaveChangesAsync().ConfigureAwait(false);
  }

  /// <inheritdoc/>
  public async Task UpdateAsync(Project project)
  {
    this.context.Projects.Update(project);
    await this.context.SaveChangesAsync().ConfigureAwait(false);
  }

  /// <inheritdoc/>
  public async Task DeleteAsync(Project project)
  {
    project.SoftDelete();
    this.context.Projects.Update(project);
    await this.context.SaveChangesAsync().ConfigureAwait(false);
  }
}
