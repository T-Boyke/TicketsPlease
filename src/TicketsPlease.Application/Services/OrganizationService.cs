// <copyright file="OrganizationService.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Application.Services;

using TicketsPlease.Application.Common.Dtos;
using TicketsPlease.Application.Common.Interfaces;
using TicketsPlease.Domain.Entities;

/// <summary>
/// Implementiert die Logik für das Organisations-Management.
/// </summary>
public class OrganizationService : IOrganizationService
{
    private readonly IOrganizationRepository repository;
    private readonly IOrganizationInviteService inviteService;
    private readonly IAuditLogService auditLogService;

    /// <summary>
    /// Initializes a new instance of the <see cref="OrganizationService"/> class.
    /// </summary>
    /// <param name="repository">Das injizierte Repository.</param>
    /// <param name="inviteService">Der Einladungs-Service.</param>
    /// <param name="auditLogService">Der Audit-Log-Service.</param>
    public OrganizationService(
        IOrganizationRepository repository,
        IOrganizationInviteService inviteService,
        IAuditLogService auditLogService)
    {
        this.repository = repository;
        this.inviteService = inviteService;
        this.auditLogService = auditLogService;
    }

    /// <inheritdoc/>
    public async Task<List<OrganizationDto>> GetOrganizationsAsync(CancellationToken ct = default)
    {
        var orgs = await this.repository.GetAllAsync(ct).ConfigureAwait(false);
        return orgs.Select(o => new OrganizationDto(
            o.Id,
            o.Name,
            o.SubscriptionLevel,
            o.IsActive,
            o.SlaCheckIntervalMinutes,
            o.QuietHoursStart,
            o.QuietHoursEnd,
            o.TimeZoneId,
            o.NotifyOnLow,
            o.NotifyOnMedium,
            o.NotifyOnHigh,
            o.NotifyOnBlocker)).ToList();
    }

    /// <inheritdoc/>
    public async Task<OrganizationDto?> GetOrganizationByIdAsync(Guid id, CancellationToken ct = default)
    {
        var o = await this.repository.GetByIdAsync(id, ct).ConfigureAwait(false);
        return o == null ? null : new OrganizationDto(
            o.Id,
            o.Name,
            o.SubscriptionLevel,
            o.IsActive,
            o.SlaCheckIntervalMinutes,
            o.QuietHoursStart,
            o.QuietHoursEnd,
            o.TimeZoneId,
            o.NotifyOnLow,
            o.NotifyOnMedium,
            o.NotifyOnHigh,
            o.NotifyOnBlocker);
    }

    /// <inheritdoc/>
    public async Task<OrganizationDto> CreateOrganizationAsync(UpsertOrganizationDto dto, CancellationToken ct = default)
    {
        var org = new Organization
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            SubscriptionLevel = dto.SubscriptionLevel,
            IsActive = dto.IsActive,
            SlaCheckIntervalMinutes = dto.SlaCheckIntervalMinutes,
            QuietHoursStart = dto.QuietHoursStart,
            QuietHoursEnd = dto.QuietHoursEnd,
            TimeZoneId = dto.TimeZoneId,
            NotifyOnLow = dto.NotifyOnLow,
            NotifyOnMedium = dto.NotifyOnMedium,
            NotifyOnHigh = dto.NotifyOnHigh,
            NotifyOnBlocker = dto.NotifyOnBlocker
        };

        await this.repository.AddAsync(org, ct).ConfigureAwait(false);
        await this.repository.SaveChangesAsync(ct).ConfigureAwait(false);

        return new OrganizationDto(
            org.Id,
            org.Name,
            org.SubscriptionLevel,
            org.IsActive,
            org.SlaCheckIntervalMinutes,
            org.QuietHoursStart,
            org.QuietHoursEnd,
            org.TimeZoneId,
            org.NotifyOnLow,
            org.NotifyOnMedium,
            org.NotifyOnHigh,
            org.NotifyOnBlocker);
    }

    /// <inheritdoc/>
    public async Task UpdateOrganizationAsync(Guid id, UpsertOrganizationDto dto, CancellationToken ct = default)
    {
        var org = await this.repository.GetByIdAsync(id, ct).ConfigureAwait(false);
        if (org != null)
        {
            var changes = $"Name: {org.Name}->{dto.Name}, Active: {org.IsActive}->{dto.IsActive}, SLA Interval: {org.SlaCheckIntervalMinutes}->{dto.SlaCheckIntervalMinutes}";

            org.Name = dto.Name;
            org.SubscriptionLevel = dto.SubscriptionLevel;
            org.IsActive = dto.IsActive;
            org.SlaCheckIntervalMinutes = dto.SlaCheckIntervalMinutes;
            org.QuietHoursStart = dto.QuietHoursStart;
            org.QuietHoursEnd = dto.QuietHoursEnd;
            org.TimeZoneId = dto.TimeZoneId;
            org.NotifyOnLow = dto.NotifyOnLow;
            org.NotifyOnMedium = dto.NotifyOnMedium;
            org.NotifyOnHigh = dto.NotifyOnHigh;
            org.NotifyOnBlocker = dto.NotifyOnBlocker;

            await this.repository.SaveChangesAsync(ct).ConfigureAwait(false);

            // Log governance action
            await this.auditLogService.LogActionAsync(id, Guid.Empty, "UpdateSettings", changes).ConfigureAwait(false);
        }
    }

    /// <inheritdoc/>
    public async Task<OrganizationInviteDto?> ValidateInviteTokenAsync(Guid token)
    {
        return await this.inviteService.ValidateTokenAsync(token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task MarkInviteAsUsedAsync(Guid token, Guid userId)
    {
        await this.inviteService.MarkAsUsedAsync(token, userId).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<List<AuditLogDto>> GetAuditLogsAsync(Guid organizationId, CancellationToken ct = default)
    {
        var logs = await this.repository.GetAuditLogsAsync(organizationId, ct).ConfigureAwait(false);
        return logs.Select(l => new AuditLogDto(
            l.Timestamp,
            l.ActorName ?? "System",
            l.ActionType,
            l.Description)).ToList();
    }
}
