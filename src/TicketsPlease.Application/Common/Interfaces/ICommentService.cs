// <copyright file="ICommentService.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Application.Common.Interfaces;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TicketsPlease.Application.Common.Dtos;

/// <summary>
/// Definiert die Geschäftslogik für die Kommentarverwaltung (F5).
/// </summary>
public interface ICommentService
{
  /// <summary>
  /// Ruft alle Kommentare für ein spezifisches Ticket ab.
  /// </summary>
  /// <param name="ticketId">Die ID des Tickets.</param>
  /// <returns>Eine Liste von <see cref="CommentDto"/>.</returns>
  public Task<IEnumerable<CommentDto>> GetCommentsForTicketAsync(Guid ticketId);

  /// <summary>
  /// Erstellt einen neuen Kommentar.
  /// </summary>
  /// <param name="dto">Die Kommentardaten.</param>
  /// <returns>Ein Task für die asynchrone Operation.</returns>
  public Task CreateCommentAsync(CreateCommentDto dto);
}
