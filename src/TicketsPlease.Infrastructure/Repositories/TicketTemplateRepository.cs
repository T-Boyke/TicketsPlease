// <copyright file="TicketTemplateRepository.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using TicketsPlease.Application.Common.Interfaces;
using TicketsPlease.Domain.Entities;
using TicketsPlease.Infrastructure.Persistence;

/// <summary>
/// Implementiert den Datenzugriff für Ticket-Vorlagen.
/// </summary>
public class TicketTemplateRepository : ITicketTemplateRepository
{
    private readonly AppDbContext context;

    /// <summary>
    /// Initializes a new instance of the <see cref="TicketTemplateRepository"/> class.
    /// </summary>
    /// <param name="context">Der Datenbankkontext.</param>
    public TicketTemplateRepository(AppDbContext context)
    {
        this.context = context;
    }

    /// <inheritdoc/>
    public async Task<List<TicketTemplate>> GetAllAsync(CancellationToken ct = default)
    {
        return await this.context.TicketTemplates
            .Include(t => t.DefaultPriority)
            .OrderBy(t => t.Name)
            .ToListAsync(ct).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<TicketTemplate?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await this.context.TicketTemplates
            .Include(t => t.DefaultPriority)
            .FirstOrDefaultAsync(t => t.Id == id, ct).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task AddAsync(TicketTemplate template, CancellationToken ct = default)
    {
        await this.context.TicketTemplates.AddAsync(template, ct).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task DeleteAsync(TicketTemplate template, CancellationToken ct = default)
    {
        this.context.TicketTemplates.Remove(template);
        await Task.CompletedTask.ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task SaveChangesAsync(CancellationToken ct = default)
    {
        await this.context.SaveChangesAsync(ct).ConfigureAwait(false);
    }
}
