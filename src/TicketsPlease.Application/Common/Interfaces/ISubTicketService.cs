// <copyright file="ISubTicketService.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Application.Common.Interfaces;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TicketsPlease.Application.Common.Dtos;

/// <summary>
/// Definiert die Geschäftslogik für Untertickets / Checklisten (F2.1.7).
/// </summary>
public interface ISubTicketService
{
  /// <summary>
  /// Fügt ein neues Unterticket hinzu.
  /// </summary>
  /// <param name="ticketId">Hauptticket-ID.</param>
  /// <param name="title">Titel der Teilaufgabe.</param>
  /// <param name="creatorId">Ersteller-ID.</param>
  /// <returns>Das erstellte <see cref="SubTicketDto"/>.</returns>
  public Task<SubTicketDto> AddSubTicketAsync(Guid ticketId, string title, Guid creatorId);

  /// <summary>
  /// Toggelt den Status eines Untertickets.
  /// </summary>
  /// <param name="subTicketId">ID des Untertickets.</param>
  /// <returns>Ein Task.</returns>
  public Task ToggleSubTicketAsync(Guid subTicketId);

  /// <summary>
  /// Löscht ein Unterticket.
  /// </summary>
  /// <param name="subTicketId">ID des Untertickets.</param>
  /// <returns>Ein Task.</returns>
  public Task DeleteSubTicketAsync(Guid subTicketId);

  /// <summary>
  /// Holt alle Untertickets für ein Hauptticket.
  /// </summary>
  /// <param name="ticketId">Hauptticket-ID.</param>
  /// <returns>Liste von Untertickets.</returns>
  public Task<IEnumerable<SubTicketDto>> GetSubTicketsAsync(Guid ticketId);
}
