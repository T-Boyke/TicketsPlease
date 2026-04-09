// <copyright file="IOrganizationRepository.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Application.Common.Interfaces;

using TicketsPlease.Domain.Entities;

/// <summary>
/// Repository Interface für die Verwaltung von Organisationen/Workspaces.
/// </summary>
public interface IOrganizationRepository
{
    /// <summary>
    /// Ruft alle Organisationen ab.
    /// </summary>
    /// <param name="ct">Abbruchsignal.</param>
    /// <returns>Liste der Organisationen.</returns>
    Task<List<Organization>> GetAllAsync(CancellationToken ct = default);

    /// <summary>
    /// Ruft eine Organisation nach ID ab.
    /// </summary>
    /// <param name="id">Die ID.</param>
    /// <param name="ct">Abbruchsignal.</param>
    /// <returns>Die Organisation oder null.</returns>
    Task<Organization?> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>
    /// Fügt eine Organisation hinzu.
    /// </summary>
    /// <param name="organization">Die Organisation.</param>
    /// <param name="ct">Abbruchsignal.</param>
    /// <returns>Task.</returns>
    Task AddAsync(Organization organization, CancellationToken ct = default);

    /// <summary>
    /// Speichert Änderungen.
    /// </summary>
    /// <param name="ct">Abbruchsignal.</param>
    /// <returns>Task.</returns>
    Task SaveChangesAsync(CancellationToken ct = default);
}
