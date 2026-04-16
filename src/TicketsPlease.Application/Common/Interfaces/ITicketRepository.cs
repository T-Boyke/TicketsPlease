// <copyright file="ITicketRepository.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Application.Common.Interfaces;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TicketsPlease.Domain.Entities;

/// <summary>
/// Definiert die Datenzugriffsschicht für <see cref="Ticket"/> Entitäten.
/// </summary>
public interface ITicketRepository
{
  /// <summary>
  /// Ruft ein Ticket anhand seiner ID ab.
  /// </summary>
  /// <param name="id">Die ID des Tickets.</param>
  /// <param name="ct">Das Abbruchsignal (CancellationToken).</param>
  /// <returns>Die asynchrone Operation, die das gefundene Ticket oder null zurückgibt.</returns>
  public Task<Ticket?> GetByIdAsync(Guid id, CancellationToken ct = default);

  /// <summary>
  /// Ruft alle aktiven Tickets ab (Read-only Optimierung).
  /// </summary>
  /// <param name="ct">Das Abbruchsignal.</param>
  /// <returns>Die asynchrone Operation, die eine Liste von Tickets zurückgibt.</returns>
  public Task<List<Ticket>> GetAllActiveAsync(CancellationToken ct = default);

  /// <summary>
  /// Ruft gefilterte Tickets ab (F6 + Stage 3 Advanced Search).
  /// </summary>
  /// <param name="projectId">Die Projekt-ID.</param>
  /// <param name="assignedUserId">Die ID des zugewiesenen Benutzers.</param>
  /// <param name="creatorId">Die ID des Erstellers.</param>
  /// <param name="status">Der Ticket-Status.</param>
  /// <param name="priorityId">Die Prioritäts-ID.</param>
  /// <param name="fromDate">Startdatum.</param>
  /// <param name="toDate">Enddatum.</param>
  /// <param name="searchString">Der Suchbegriff.</param>
  /// <param name="tagId">Die Tag-ID.</param>
  /// <param name="ct">Das Abbruchsignal.</param>
  /// <returns>Eine Liste von Tickets.</returns>
  public Task<List<Ticket>> GetFilteredAsync(
      Guid? projectId = null,
      Guid? assignedUserId = null,
      Guid? creatorId = null,
      string? status = null,
      Guid? priorityId = null,
      DateTime? fromDate = null,
      DateTime? toDate = null,
      string? searchString = null,
      Guid? tagId = null,
      CancellationToken ct = default);

  /// <summary>
  /// Fügt ein neues Ticket hinzu.
  /// </summary>
  /// <param name="ticket">Das zu speichernde Ticket.</param>
  /// <param name="ct">Das Abbruchsignal.</param>
  /// <returns>Die asynchrone Operation.</returns>
  public Task AddAsync(Ticket ticket, CancellationToken ct = default);

  /// <summary>
  /// Speichert sämtliche Änderungen im Kontext persistent ab.
  /// </summary>
  /// <param name="ct">Das Abbruchsignal.</param>
  /// <returns>Die asynchrone Operation, die die Anzahl der betroffenen Datensätze zurückgibt.</returns>
  public Task<int> SaveChangesAsync(CancellationToken ct = default);

  /// <summary>
  /// Ruft den Standard-Workflow-Status ab.
  /// </summary>
  /// <param name="ct">Das Abbruchsignal.</param>
  /// <returns>Der Standard-Status.</returns>
  public Task<WorkflowState?> GetDefaultWorkflowStateAsync(CancellationToken ct = default);

  /// <summary>
  /// Ruft einen Workflow-Status anhand seines Namens ab.
  /// </summary>
  /// <param name="name">Der Name des Status.</param>
  /// <param name="ct">Das Abbruchsignal.</param>
  /// <returns>Der Status oder null.</returns>
  public Task<WorkflowState?> GetWorkflowStateByNameAsync(string name, CancellationToken ct = default);

  /// <summary>
  /// Ruft einen Workflow-Übergang ab.
  /// </summary>
  /// <param name="fromStateId">Ausgangszustand.</param>
  /// <param name="toStateId">Zielzustand.</param>
  /// <param name="ct">Das Abbruchsignal.</param>
  /// <returns>Der Übergang oder null.</returns>
  public Task<WorkflowTransition?> GetTransitionAsync(Guid fromStateId, Guid toStateId, CancellationToken ct = default);

  /// <summary>
  /// Entfernt eine Ticket-Verknüpfung.
  /// </summary>
  /// <param name="linkId">Die ID der Verknüpfung.</param>
  /// <param name="ct">Das Abbruchsignal.</param>
  /// <returns>Ein Task.</returns>
  public Task RemoveLinkAsync(Guid linkId, CancellationToken ct = default);

  /// <summary>
  /// Ruft alle verfügbaren Tags ab.
  /// </summary>
  /// <param name="ct">Das Abbruchsignal.</param>
  /// <returns>Eine Liste von Tags.</returns>
  public Task<List<Tag>> GetAllTagsAsync(CancellationToken ct = default);

  /// <summary>
  /// Fügt einen Eintrag in die Historie hinzu.
  /// </summary>
  /// <param name="history">Der Historien-Eintrag.</param>
  /// <returns>Ein Task.</returns>
  public Task AddHistoryAsync(TicketHistory history);

  /// <summary>
  /// Fügt einen Upvote hinzu.
  /// </summary>
  /// <param name="upvote">Der Upvote.</param>
  /// <returns>Ein Task.</returns>
  public Task AddUpvoteAsync(TicketUpvote upvote);

  /// <summary>
  /// Entfernt einen Upvote.
  /// </summary>
  /// <param name="ticketId">Die Ticket-ID.</param>
  /// <param name="userId">Die User-ID.</param>
  /// <returns>Ein Task.</returns>
  public Task RemoveUpvoteAsync(Guid ticketId, Guid userId);

  /// <summary>
  /// Prüft ob ein User bereits gevotet hat.
  /// </summary>
  /// <param name="ticketId">Die Ticket-ID.</param>
  /// <param name="userId">Die User-ID.</param>
  /// <returns>Ein Task.</returns>
  public Task<bool> UserHasUpvotedAsync(Guid ticketId, Guid userId);

  /// <summary>
  /// Zählt die Upvotes.
  /// </summary>
  /// <param name="ticketId">Die Ticket-ID.</param>
  /// <returns>Ein Task.</returns>
  public Task<int> GetUpvoteCountAsync(Guid ticketId);

  /// <summary>
  /// Ruft alle verfügbaren Prioritäten ab.
  /// </summary>
  /// <param name="ct">Das Abbruchsignal.</param>
  /// <returns>Eine Liste von Prioritäten.</returns>
  public Task<List<TicketPriority>> GetPrioritiesAsync(CancellationToken ct = default);

  /// <summary>
  /// Setzt die ursprüngliche RowVersion für die Nebenläufigkeitsprüfung.
  /// </summary>
  /// <param name="ticket">Das Ticket.</param>
  /// <param name="rowVersion">Die zu vergleichende Zeilenversion.</param>
  public void SetOriginalRowVersion(Ticket ticket, byte[] rowVersion);

  /// <summary>
  /// Fügt einen Workflow-Übergang hinzu.
  /// </summary>
  /// <param name="transition">Der Übergang.</param>
  public Task AddWorkflowTransitionAsync(WorkflowTransition transition);

  /// <summary>
  /// Ruft alle Tickets eines Tenants ab (für Reporting).
  /// </summary>
  /// <param name="tenantId">Die Mandanten-ID.</param>
  /// <returns>Liste der Tickets.</returns>
  public Task<List<Ticket>> GetByTenantAsync(Guid tenantId);
}
