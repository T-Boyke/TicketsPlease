// <copyright file="ITicketRepository.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Application.Common.Interfaces;

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
  /// Ruft gefilterte Tickets ab (F6).
  /// </summary>
  /// <param name="projectId">Optionale Projekt-ID.</param>
  /// <param name="assignedUserId">Optionale Zuweisungs-ID.</param>
  /// <param name="creatorId">Optionale Ersteller-ID.</param>
  /// <param name="ct">Das Abbruchsignal.</param>
  /// <returns>Eine Liste gefilterter Tickets.</returns>
  public Task<List<Ticket>> GetFilteredAsync(Guid? projectId = null, Guid? assignedUserId = null, Guid? creatorId = null, CancellationToken ct = default);

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
  /// Ruft die Standard-Workflow-Status-ID ab.
  /// </summary>
  /// <param name="ct">Das Abbruchsignal.</param>
  /// <returns>Die ID des Standard-Status.</returns>
  public Task<Guid> GetDefaultWorkflowStateIdAsync(CancellationToken ct = default);

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
}
