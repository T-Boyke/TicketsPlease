// <copyright file="ITicketService.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Application.Common.Interfaces;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TicketsPlease.Application.Common.Dtos;

/// <summary>
/// Definiert die Geschäftslogik für das Ticket-Management (Kanban).
/// </summary>
public interface ITicketService
{
    /// <summary>
    /// Ruft alle aktiven Tickets für den aktuellen Mandanten ab.
    /// </summary>
    /// <returns>Eine Liste von <see cref="TicketDto"/>.</returns>
    Task<IEnumerable<TicketDto>> GetActiveTicketsAsync();

    /// <summary>
    /// Ruft ein spezifisches Ticket ab.
    /// </summary>
    /// <param name="id">Die ID des Tickets.</param>
    /// <returns>Ein <see cref="TicketDto"/> oder null.</returns>
    Task<TicketDto?> GetTicketAsync(Guid id);

    /// <summary>
    /// Erstellt ein neues Ticket.
    /// </summary>
    /// <param name="dto">Die Ticketdaten.</param>
    /// <returns>Ein Task für die asynchrone Operation.</returns>
    Task CreateTicketAsync(CreateTicketDto dto);

    /// <summary>
    /// Aktualisiert ein bestehendes Ticket.
    /// </summary>
    /// <param name="dto">Die aktualisierten Daten.</param>
    /// <returns>Ein Task für die asynchrone Operation.</returns>
    Task UpdateTicketAsync(UpdateTicketDto dto);

    /// <summary>
    /// Verschiebt ein Ticket in einen neuen Status.
    /// </summary>
    /// <param name="id">Die ID des Tickets.</param>
    /// <param name="newStatus">Der Zielstatus.</param>
    /// <returns>Ein Task für die asynchrone Operation.</returns>
    Task MoveTicketAsync(Guid id, string newStatus);

    /// <summary>
    /// Schließt ein Ticket endgültig.
    /// </summary>
    /// <param name="id">Die ID des zu schließenden Tickets.</param>
    /// <returns>Ein Task für die asynchrone Operation.</returns>
    Task CloseTicketAsync(Guid id);
}
