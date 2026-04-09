// <copyright file="ITicketTemplateService.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Application.Common.Interfaces;

using TicketsPlease.Application.Common.Dtos;

/// <summary>
/// Service Interface für die Geschäftslogik von Ticket-Vorlagen.
/// </summary>
public interface ITicketTemplateService
{
    /// <summary>
    /// Ruft alle verfügbaren Vorlagen ab.
    /// </summary>
    /// <param name="ct">Abbruchsignal.</param>
    /// <returns>Liste von DTOs.</returns>
    Task<List<TicketTemplateDto>> GetAllTemplatesAsync(CancellationToken ct = default);

    /// <summary>
    /// Erstellt eine neue Vorlage.
    /// </summary>
    /// <param name="creatorId">ID des Erstellers.</param>
    /// <param name="dto">Die Daten.</param>
    /// <param name="ct">Abbruchsignal.</param>
    /// <returns>Das erstellte DTO.</returns>
    Task<TicketTemplateDto> CreateTemplateAsync(Guid creatorId, CreateTicketTemplateDto dto, CancellationToken ct = default);

    /// <summary>
    /// Löscht eine Vorlage.
    /// </summary>
    /// <param name="id">Die ID.</param>
    /// <param name="ct">Abbruchsignal.</param>
    /// <returns>Task.</returns>
    Task DeleteTemplateAsync(Guid id, CancellationToken ct = default);
}
