// <copyright file="ITicketRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
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
}
