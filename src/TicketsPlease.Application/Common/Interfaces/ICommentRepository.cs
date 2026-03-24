// <copyright file="ICommentRepository.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Application.Common.Interfaces;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TicketsPlease.Domain.Entities;

/// <summary>
/// Definiert die Repository-Methoden für Kommentare (F5).
/// </summary>
public interface ICommentRepository
{
  /// <summary>
  /// Ruft alle Kommentare für ein Ticket ab.
  /// </summary>
  /// <param name="ticketId">Die ID des Tickets.</param>
  /// <param name="ct">Das CancellationToken.</param>
  /// <returns>Eine Liste von <see cref="Comment"/>.</returns>
  Task<List<Comment>> GetByTicketIdAsync(Guid ticketId, CancellationToken ct = default);

  /// <summary>
  /// Fügt einen neuen Kommentar hinzu.
  /// </summary>
  /// <param name="comment">Der Kommentar.</param>
  /// <param name="ct">Das CancellationToken.</param>
  /// <returns>Ein Task für die asynchrone Operation.</returns>
  Task AddAsync(Comment comment, CancellationToken ct = default);

  /// <summary>
  /// Speichert die Änderungen in der Datenbank.
  /// </summary>
  /// <param name="ct">Das CancellationToken.</param>
  /// <returns>Die Anzahl der betroffenen Datensätze.</returns>
  Task<int> SaveChangesAsync(CancellationToken ct = default);
}
