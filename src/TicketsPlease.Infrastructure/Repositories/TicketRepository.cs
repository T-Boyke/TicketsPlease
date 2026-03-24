// <copyright file="TicketRepository.cs" company="BitLC-NE-2025-2026">
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
/// Implementiert den Datenzugriff für Tickets unter Verwendung von Entity Framework Core.
/// Berücksichtigt Performance-Optimierungen wie AsNoTracking für Leseabfragen.
/// </summary>
public class TicketRepository : ITicketRepository
{
  private readonly AppDbContext context;

  /// <summary>
  /// Initializes a new instance of the <see cref="TicketRepository"/> class.
  /// </summary>
  /// <param name="context">Der injizierte Datenbankkontext.</param>
  public TicketRepository(AppDbContext context)
  {
    this.context = context;
  }

  /// <inheritdoc />
  public async Task<Ticket?> GetByIdAsync(Guid id, CancellationToken ct = default)
  {
    // Wir nutzen hier kein AsNoTracking, da das Objekt evtl. bearbeitet werden soll (Tracking benötig).
    return await this.context.Tickets
        .Include(t => t.AssignedUser)
        .Include(t => t.Project)
        .Include(t => t.Priority)
        .FirstOrDefaultAsync(t => t.Id == id, ct).ConfigureAwait(false);
  }

  /// <inheritdoc />
  public async Task<List<Ticket>> GetAllActiveAsync(CancellationToken ct = default)
  {
    // Enterprise Pattern: Reine Leseabfragen strikt mit AsNoTracking ausführen
    return await this.context.Tickets
        .AsNoTracking()
        .Include(t => t.AssignedUser)
        .Include(t => t.Project)
        .Include(t => t.Priority)
        .OrderByDescending(t => t.Priority != null ? t.Priority.LevelWeight : 0)
        .ToListAsync(ct).ConfigureAwait(false);
  }

  /// <inheritdoc />
  public async Task AddAsync(Ticket ticket, CancellationToken ct = default)
  {
    await this.context.Tickets.AddAsync(ticket, ct).ConfigureAwait(false);
  }

  /// <inheritdoc />
  public async Task<int> SaveChangesAsync(CancellationToken ct = default)
  {
    // EF Core führt hier automatisch die Nebenläufigkeitsprüfung (RowVersion) durch
    return await this.context.SaveChangesAsync(ct).ConfigureAwait(false);
  }

  /// <inheritdoc />
  public async Task<Guid> GetDefaultWorkflowStateIdAsync(CancellationToken ct = default)
  {
    var state = await this.context.WorkflowStates.AsNoTracking().FirstOrDefaultAsync(ct).ConfigureAwait(false);
    return state?.Id ?? Guid.Empty;
  }
}
