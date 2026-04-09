// <copyright file="ITicketTemplateRepository.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Application.Common.Interfaces;

using TicketsPlease.Domain.Entities;

/// <summary>
/// Repository Interface für die Verwaltung von Ticket-Vorlagen.
/// </summary>
public interface ITicketTemplateRepository
{
    /// <summary>
    /// Ruft alle Vorlagen ab.
    /// </summary>
    /// <param name="ct">Abbruchsignal.</param>
    /// <returns>Liste der Vorlagen.</returns>
    Task<List<TicketTemplate>> GetAllAsync(CancellationToken ct = default);

    /// <summary>
    /// Ruft eine Vorlage anhand der ID ab.
    /// </summary>
    /// <param name="id">Die ID.</param>
    /// <param name="ct">Abbruchsignal.</param>
    /// <returns>Die Vorlage oder null.</returns>
    Task<TicketTemplate?> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>
    /// Fügt eine Vorlage hinzu.
    /// </summary>
    /// <param name="template">Die Vorlage.</param>
    /// <param name="ct">Abbruchsignal.</param>
    /// <returns>Task.</returns>
    Task AddAsync(TicketTemplate template, CancellationToken ct = default);

    /// <summary>
    /// Löscht eine Vorlage.
    /// </summary>
    /// <param name="template">Die Vorlage.</param>
    /// <param name="ct">Abbruchsignal.</param>
    /// <returns>Task.</returns>
    Task DeleteAsync(TicketTemplate template, CancellationToken ct = default);

    /// <summary>
    /// Speichert die Änderungen.
    /// </summary>
    /// <param name="ct">Abbruchsignal.</param>
    /// <returns>Task.</returns>
    Task SaveChangesAsync(CancellationToken ct = default);
}
