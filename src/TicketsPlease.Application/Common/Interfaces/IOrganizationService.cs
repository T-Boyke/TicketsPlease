// <copyright file="IOrganizationService.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Application.Common.Interfaces;

using TicketsPlease.Application.Common.Dtos;

/// <summary>
/// Service Interface für Organisations-Management (Workspaces).
/// </summary>
public interface IOrganizationService
{
    /// <summary>
    /// Ruft alle Organisationen ab.
    /// </summary>
    /// <param name="ct">Abbruchsignal.</param>
    /// <returns>Liste von DTOs.</returns>
    Task<List<OrganizationDto>> GetOrganizationsAsync(CancellationToken ct = default);

    /// <summary>
    /// Ruft eine Organisation nach ID ab.
    /// </summary>
    /// <param name="id">Die ID.</param>
    /// <param name="ct">Abbruchsignal.</param>
    /// <returns>Das DTO oder null.</returns>
    Task<OrganizationDto?> GetOrganizationByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>
    /// Erstellt eine neue Organisation.
    /// </summary>
    /// <param name="dto">Die Daten.</param>
    /// <param name="ct">Abbruchsignal.</param>
    /// <returns>Das erstellte DTO.</returns>
    Task<OrganizationDto> CreateOrganizationAsync(UpsertOrganizationDto dto, CancellationToken ct = default);

    /// <summary>
    /// Aktualisiert eine Organisation.
    /// </summary>
    /// <param name="id">Die ID.</param>
    /// <param name="dto">Die neuen Daten.</param>
    /// <param name="ct">Abbruchsignal.</param>
    /// <returns>Task.</returns>
    Task UpdateOrganizationAsync(Guid id, UpsertOrganizationDto dto, CancellationToken ct = default);
}
