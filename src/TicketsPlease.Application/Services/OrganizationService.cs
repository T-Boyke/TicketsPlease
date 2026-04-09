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

    /// <summary>
    /// Initializes a new instance of the <see cref="OrganizationService"/> class.
    /// </summary>
    /// <param name="repository">Das injizierte Repository.</param>
    public OrganizationService(IOrganizationRepository repository)
    {
        this.repository = repository;
    }

    /// <inheritdoc/>
    public async Task<List<OrganizationDto>> GetOrganizationsAsync(CancellationToken ct = default)
    {
        var orgs = await this.repository.GetAllAsync(ct).ConfigureAwait(false);
        return orgs.Select(o => new OrganizationDto(o.Id, o.Name, o.SubscriptionLevel, o.IsActive)).ToList();
    }

    /// <inheritdoc/>
    public async Task<OrganizationDto?> GetOrganizationByIdAsync(Guid id, CancellationToken ct = default)
    {
        var o = await this.repository.GetByIdAsync(id, ct).ConfigureAwait(false);
        return o == null ? null : new OrganizationDto(o.Id, o.Name, o.SubscriptionLevel, o.IsActive);
    }

    /// <inheritdoc/>
    public async Task<OrganizationDto> CreateOrganizationAsync(UpsertOrganizationDto dto, CancellationToken ct = default)
    {
        var org = new Organization
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            SubscriptionLevel = dto.SubscriptionLevel,
            IsActive = dto.IsActive
        };

        await this.repository.AddAsync(org, ct).ConfigureAwait(false);
        await this.repository.SaveChangesAsync(ct).ConfigureAwait(false);

        return new OrganizationDto(org.Id, org.Name, org.SubscriptionLevel, org.IsActive);
    }

    /// <inheritdoc/>
    public async Task UpdateOrganizationAsync(Guid id, UpsertOrganizationDto dto, CancellationToken ct = default)
    {
        var org = await this.repository.GetByIdAsync(id, ct).ConfigureAwait(false);
        if (org != null)
        {
            org.Name = dto.Name;
            org.SubscriptionLevel = dto.SubscriptionLevel;
            org.IsActive = dto.IsActive;

            await this.repository.SaveChangesAsync(ct).ConfigureAwait(false);
        }
    }
}
