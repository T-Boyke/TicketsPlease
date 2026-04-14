// <copyright file="AuditLogService.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Application.Services;

using System;
using System.Threading.Tasks;
using TicketsPlease.Application.Common.Interfaces;
using TicketsPlease.Domain.Entities;

/// <summary>
/// Implementierung des Audit-Log-Dienstes zur Erfassung von Governance-Aktionen.
/// </summary>
public class AuditLogService : IAuditLogService
{
    private readonly IOrganizationRepository organizationRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuditLogService"/> class.
    /// </summary>
    /// <param name="organizationRepository">Das Organisations-Repository.</param>
    public AuditLogService(IOrganizationRepository organizationRepository)
    {
        this.organizationRepository = organizationRepository;
    }

    /// <inheritdoc/>
    public async Task LogActionAsync(Guid organizationId, Guid actorUserId, string actionType, string description)
    {
        var log = new AuditLog(organizationId, actorUserId, actionType, description);
        await this.organizationRepository.AddAuditLogAsync(log).ConfigureAwait(false);
        await this.organizationRepository.SaveChangesAsync().ConfigureAwait(false);
    }
}
