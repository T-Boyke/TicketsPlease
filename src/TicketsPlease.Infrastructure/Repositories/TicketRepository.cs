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
        .Include(t => t.Comments)
            .ThenInclude(c => c.Author)
        .Include(t => t.BlockedBy)
            .ThenInclude(l => l.SourceTicket)
        .Include(t => t.Blocking)
            .ThenInclude(l => l.TargetTicket)
        .Include(t => t.Attachments)
            .ThenInclude(a => a.UploadedByUser)
        .Include(t => t.Tags)
            .ThenInclude(tt => tt.Tag)
        .Include(t => t.History)
            .ThenInclude(h => h.ActorUser)
        .Include(t => t.Upvotes)
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
        .Include(t => t.Upvotes)
        .OrderByDescending(t => t.Priority != null ? t.Priority.LevelWeight : 0)
        .ToListAsync(ct).ConfigureAwait(false);
  }

  /// <summary>
  /// Ruft gefilterte Tickets ab.
  /// </summary>
  /// <param name="projectId">Die Projekt-ID.</param>
  /// <param name="assignedUserId">Die ID des zugewiesenen Benutzers.</param>
  /// <param name="creatorId">Die ID des Erstellers.</param>
  /// <param name="status">Der Ticket-Status.</param>
  /// <param name="priorityId">Die Prioritäts-ID.</param>
  /// <param name="fromDate">Startdatum.</param>
  /// <param name="toDate">Enddatum.</param>
  /// <param name="searchString">Der Suchbegriff.</param>
  /// <param name="tagId">Die Tag-ID.</param>
  /// <param name="ct">Das Abbruchsignal.</param>
  /// <returns>Eine Liste von Tickets.</returns>
  public async Task<List<Ticket>> GetFilteredAsync(
      Guid? projectId = null,
      Guid? assignedUserId = null,
      Guid? creatorId = null,
      string? status = null,
      Guid? priorityId = null,
      DateTime? fromDate = null,
      DateTime? toDate = null,
      string? searchString = null,
      Guid? tagId = null,
      CancellationToken ct = default)
  {
    var query = this.context.Tickets
        .AsNoTracking()
        .Include(t => t.AssignedUser)
        .Include(t => t.Project)
        .Include(t => t.Priority)
        .Include(t => t.Upvotes)
        .Include(t => t.Tags)
        .AsQueryable();

    if (!string.IsNullOrWhiteSpace(searchString))
    {
      query = query.Where(t => t.Title.Contains(searchString) || t.Description.Contains(searchString));
    }

    if (projectId.HasValue)
    {
      query = query.Where(t => t.ProjectId == projectId.Value);
    }

    if (assignedUserId.HasValue)
    {
      query = query.Where(t => t.AssignedUserId == assignedUserId.Value);
    }

    if (creatorId.HasValue)
    {
      query = query.Where(t => t.CreatorId == creatorId.Value);
    }

    if (!string.IsNullOrWhiteSpace(status))
    {
      query = query.Where(t => t.Status == status);
    }

    if (priorityId.HasValue)
    {
      query = query.Where(t => t.PriorityId == priorityId.Value);
    }

    if (fromDate.HasValue)
    {
      query = query.Where(t => t.CreatedAt >= fromDate.Value);
    }

    if (toDate.HasValue)
    {
      query = query.Where(t => t.CreatedAt <= toDate.Value);
    }

    if (tagId.HasValue)
    {
      query = query.Where(t => t.Tags.Any(tt => tt.TagId == tagId.Value));
    }

    return await query
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
  public async Task<WorkflowState?> GetDefaultWorkflowStateAsync(CancellationToken ct = default)
  {
    return await this.context.WorkflowStates.AsNoTracking().FirstOrDefaultAsync(ct).ConfigureAwait(false);
  }

  /// <inheritdoc />
  public async Task<WorkflowState?> GetWorkflowStateByNameAsync(string name, CancellationToken ct = default)
  {
    return await this.context.WorkflowStates.AsNoTracking().FirstOrDefaultAsync(s => s.Name == name, ct).ConfigureAwait(false);
  }

  /// <inheritdoc />
  public async Task<WorkflowTransition?> GetTransitionAsync(Guid fromStateId, Guid toStateId, CancellationToken ct = default)
  {
    return await this.context.WorkflowTransitions
        .AsNoTracking()
        .FirstOrDefaultAsync(tr => tr.FromStateId == fromStateId && tr.ToStateId == toStateId, ct)
        .ConfigureAwait(false);
  }

  /// <inheritdoc />
  public async Task RemoveLinkAsync(Guid linkId, CancellationToken ct = default)
  {
    var link = await this.context.TicketLinks.FindAsync(new object?[] { linkId }, ct).ConfigureAwait(false);
    if (link != null)
    {
      this.context.TicketLinks.Remove(link);
    }
  }

  /// <inheritdoc />
  public async Task<List<Tag>> GetAllTagsAsync(CancellationToken ct = default)
  {
    return await this.context.Tags.AsNoTracking().OrderBy(t => t.Name).ToListAsync(ct).ConfigureAwait(false);
  }

  /// <inheritdoc />
  public async Task AddHistoryAsync(TicketHistory history)
  {
    await this.context.TicketHistories.AddAsync(history).ConfigureAwait(false);
  }

  /// <inheritdoc />
  public async Task AddUpvoteAsync(TicketUpvote upvote)
  {
    await this.context.TicketUpvotes.AddAsync(upvote).ConfigureAwait(false);
  }

  /// <inheritdoc />
  public async Task RemoveUpvoteAsync(Guid ticketId, Guid userId)
  {
    var vote = await this.context.TicketUpvotes.FindAsync(ticketId, userId).ConfigureAwait(false);
    if (vote != null)
    {
      this.context.TicketUpvotes.Remove(vote);
    }
  }

  /// <inheritdoc />
  public async Task<bool> UserHasUpvotedAsync(Guid ticketId, Guid userId)
  {
    return await this.context.TicketUpvotes.AsNoTracking().AnyAsync(u => u.TicketId == ticketId && u.UserId == userId).ConfigureAwait(false);
  }

  /// <inheritdoc />
  public async Task<int> GetUpvoteCountAsync(Guid ticketId)
  {
    return await this.context.TicketUpvotes.AsNoTracking().CountAsync(u => u.TicketId == ticketId).ConfigureAwait(false);
  }

  /// <inheritdoc />
  public async Task<List<TicketPriority>> GetPrioritiesAsync(CancellationToken ct = default)
  {
    return await this.context.TicketPriorities
        .OrderByDescending(p => p.LevelWeight)
        .ToListAsync(ct)
        .ConfigureAwait(false);
  }

  /// <inheritdoc />
  public void SetOriginalRowVersion(Ticket ticket, byte[] rowVersion)
  {
    this.context.Entry(ticket).Property("RowVersion").OriginalValue = rowVersion;
  }

  /// <inheritdoc />
  public async Task<List<Ticket>> GetByTenantAsync(Guid tenantId)
  {
    return await this.context.Tickets
        .AsNoTracking()
        .Where(t => t.TenantId == tenantId)
        .Include(t => t.Priority)
        .ToListAsync()
        .ConfigureAwait(false);
  }
}
