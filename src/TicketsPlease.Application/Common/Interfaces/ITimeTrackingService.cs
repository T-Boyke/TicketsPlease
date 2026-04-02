// <copyright file="ITimeTrackingService.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Application.Common.Interfaces;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TicketsPlease.Application.Common.Dtos;

/// <summary>
/// Definiert die Geschäftslogik für die Zeiterfassung an Tickets (F2.1.4).
/// </summary>
public interface ITimeTrackingService
{
  /// <summary>
  /// Startet eine neue Zeiterfassung für einen Benutzer an einem Ticket.
  /// </summary>
  /// <param name="ticketId">Die ID des Tickets.</param>
  /// <param name="userId">Die ID des Benutzers.</param>
  /// <returns>Ein Task.</returns>
  public Task StartTimeTrackingAsync(Guid ticketId, Guid userId);

  /// <summary>
  /// Stoppt die aktuelle Zeiterfassung eines Benutzers an einem Ticket.
  /// </summary>
  /// <param name="ticketId">Die ID des Tickets.</param>
  /// <param name="userId">Die ID des Benutzers.</param>
  /// <returns>Ein Task.</returns>
  public Task StopTimeTrackingAsync(Guid ticketId, Guid userId);

  /// <summary>
  /// Ruft alle Zeiterfassungseinträge für ein Ticket ab.
  /// </summary>
  /// <param name="ticketId">Die ID des Tickets.</param>
  /// <returns>Eine Liste von <see cref="TimeLogDto"/>.</returns>
  public Task<IEnumerable<TimeLogDto>> GetTimeLogsAsync(Guid ticketId);

  /// <summary>
  /// Prüft, ob für den Benutzer aktuell eine Zeiterfassung an diesem Ticket läuft.
  /// </summary>
  /// <param name="ticketId">Die ID des Tickets.</param>
  /// <param name="userId">Die ID des Benutzers.</param>
  /// <returns>True, wenn eine Erfassung läuft.</returns>
  public Task<bool> IsTimerRunningAsync(Guid ticketId, Guid userId);
}
