// <copyright file="OrganizationRepository.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using TicketsPlease.Application.Common.Interfaces;
using TicketsPlease.Domain.Entities;
using TicketsPlease.Infrastructure.Persistence;

/// <summary>
/// Implementiert den Datenzugriff für Organisationen.
/// </summary>
public class OrganizationRepository : IOrganizationRepository
{
    private readonly AppDbContext context;

    /// <summary>
    /// Initializes a new instance of the <see cref="OrganizationRepository"/> class.
    /// </summary>
    /// <param name="context">Der Datenbankkontext.</param>
    public OrganizationRepository(AppDbContext context)
    {
        this.context = context;
    }

    /// <inheritdoc/>
    public async Task<List<Organization>> GetAllAsync(CancellationToken ct = default)
    {
        return await this.context.Organizations
            .OrderBy(o => o.Name)
            .ToListAsync(ct).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<Organization?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await this.context.Organizations
            .FirstOrDefaultAsync(o => o.Id == id, ct).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task AddAsync(Organization organization, CancellationToken ct = default)
    {
        await this.context.Organizations.AddAsync(organization, ct).ConfigureAwait(false);
    }

    /// <summary>
    /// Speichert Änderungen.
    /// </summary>
    /// <param name="ct">Abbruchsignal.</param>
    /// <returns>Task.</returns>
    public async Task SaveChangesAsync(CancellationToken ct = default)
    {
        await this.context.SaveChangesAsync(ct).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task AddAuditLogAsync(AuditLog log, CancellationToken ct = default)
    {
        await this.context.AuditLogs.AddAsync(log, ct).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<List<AuditLog>> GetAuditLogsAsync(Guid organizationId, CancellationToken ct = default)
    {
        return await this.context.AuditLogs
            .Where(l => l.OrganizationId == organizationId)
            .OrderByDescending(l => l.Timestamp)
            .ToListAsync(ct).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task AddInvite(OrganizationInvite invite)
    {
        await this.context.OrganizationInvites.AddAsync(invite).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<OrganizationInvite?> GetInviteByTokenAsync(Guid token)
    {
        return await this.context.OrganizationInvites
            .Include(i => i.Organization)
            .FirstOrDefaultAsync(i => i.Token == token).ConfigureAwait(false);
    }
}
