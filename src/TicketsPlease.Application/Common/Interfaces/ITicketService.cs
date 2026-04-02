// <copyright file="ITicketService.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Application.Common.Interfaces;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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
  public Task<IEnumerable<TicketDto>> GetActiveTicketsAsync();

  /// <summary>
  /// Ruft gefilterte Tickets ab (F6).
  /// </summary>
  /// <param name="projectId">Optionale Projekt-ID.</param>
  /// <param name="assignedUserId">Optionale Zuweisungs-ID.</param>
  /// <param name="creatorId">Optionale Ersteller-ID.</param>
  /// <returns>Eine Liste von <see cref="TicketDto"/>.</returns>
  public Task<IEnumerable<TicketDto>> GetFilteredTicketsAsync(Guid? projectId = null, Guid? assignedUserId = null, Guid? creatorId = null);

  /// <summary>
  /// Ruft ein spezifisches Ticket ab.
  /// </summary>
  /// <param name="id">Die ID des Tickets.</param>
  /// <returns>Ein <see cref="TicketDto"/> oder null.</returns>
  public Task<TicketDto?> GetTicketAsync(Guid id);

  /// <summary>
  /// Erstellt ein neues Ticket.
  /// </summary>
  /// <param name="dto">Die Ticketdaten.</param>
  /// <returns>Ein Task für die asynchrone Operation.</returns>
  public Task CreateTicketAsync(CreateTicketDto dto);

  /// <summary>
  /// Aktualisiert ein bestehendes Ticket.
  /// </summary>
  /// <param name="dto">Die aktualisierten Daten.</param>
  /// <returns>Ein Task für die asynchrone Operation.</returns>
  public Task UpdateTicketAsync(UpdateTicketDto dto);

  /// <summary>
  /// Verschiebt ein Ticket in einen neuen Status.
  /// </summary>
  /// <param name="id">Die ID des Tickets.</param>
  /// <param name="newStatus">Der Zielstatus.</param>
  /// <returns>Ein Task für die asynchrone Operation.</returns>
  public Task MoveTicketAsync(Guid id, string newStatus);

  /// <summary>
  /// Schließt ein Ticket endgültig (F3.4).
  /// Berücksichtigt Abhängigkeiten (Blocker) (F7).
  /// </summary>
  /// <param name="id">Die ID des zu schließenden Tickets.</param>
  /// <returns>Ein Task für die asynchrone Operation.</returns>
  public Task CloseTicketAsync(Guid id);

  /// <summary>
  /// Fügt eine Abhängigkeit hinzu (F7).
  /// </summary>
  /// <param name="ticketId">Das blockierte Ticket (Nachfolger).</param>
  /// <param name="blockerId">Das blockierende Ticket (Vorgänger).</param>
  /// <returns>Ein Task für die asynchrone Operation.</returns>
  public Task AddDependencyAsync(Guid ticketId, Guid blockerId);

  /// <summary>
  /// Entfernt eine Abhängigkeit zwischen zwei Tickets.
  /// </summary>
  /// <param name="sourceId">Das Master-Ticket.</param>
  /// <param name="targetId">Das abhängige Ticket.</param>
  /// <returns>Ein Task.</returns>
  public Task RemoveDependencyAsync(Guid sourceId, Guid targetId);

  /// <summary>
  /// Lädt eine Datei als Anhang für ein Ticket hoch.
  /// </summary>
  /// <param name="ticketId">Die Ticket-ID.</param>
  /// <param name="file">Die Datei.</param>
  /// <returns>Ein Task.</returns>
  public Task UploadAttachmentAsync(Guid ticketId, IFormFile file);

  /// <summary>
  /// Ruft alle verfügbaren Tags ab.
  /// </summary>
  /// <returns>Eine Liste von Tag-DTOs.</returns>
  public Task<IEnumerable<TagDto>> GetAllTagsAsync();

  /// <summary>
  /// Gibt einen Upvote für ein Ticket ab (Phase 2 Enterprise).
  /// </summary>
  /// <param name="id">Die ID des Tickets.</param>
  /// <returns>Ein Task.</returns>
  public Task UpvoteAsync(Guid id);

  /// <summary>
  /// Entfernt einen Upvote von einem Ticket.
  /// </summary>
  /// <param name="id">Die ID des Tickets.</param>
  /// <returns>Ein Task.</returns>
  public Task DownvoteAsync(Guid id);
}
