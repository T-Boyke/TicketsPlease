// <copyright file="TimeTrackingService.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Infrastructure.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TicketsPlease.Application.Common.Dtos;
using TicketsPlease.Application.Common.Interfaces;
using TicketsPlease.Domain.Entities;
using TicketsPlease.Infrastructure.Persistence;

/// <summary>
/// Implementierung des Zeit-Management-Services.
/// </summary>
public class TimeTrackingService : ITimeTrackingService
{
  private readonly AppDbContext context;

  /// <summary>
  /// Initializes a new instance of the <see cref="TimeTrackingService"/> class.
  /// </summary>
  /// <param name="context">Der Datenbankkontext.</param>
  public TimeTrackingService(AppDbContext context)
  {
    this.context = context;
  }

  /// <inheritdoc/>
  public async Task StartTimeTrackingAsync(Guid ticketId, Guid userId)
  {
    // Prüfen, ob bereits ein Timer läuft
    var existing = await this.context.TimeLogs
        .FirstOrDefaultAsync(l => l.TicketId == ticketId && l.UserId == userId && l.StoppedAt == null)
        .ConfigureAwait(false);

    if (existing != null)
    {
      return; // Bereits aktiv
    }

    var log = new TimeLog
    {
      Id = Guid.NewGuid(),
      TicketId = ticketId,
      UserId = userId,
      StartedAt = DateTime.UtcNow,
      HoursLogged = 0,
    };

    this.context.TimeLogs.Add(log);
    await this.context.SaveChangesAsync().ConfigureAwait(false);
  }

  /// <inheritdoc/>
  public async Task StopTimeTrackingAsync(Guid ticketId, Guid userId)
  {
    var log = await this.context.TimeLogs
        .FirstOrDefaultAsync(l => l.TicketId == ticketId && l.UserId == userId && l.StoppedAt == null)
        .ConfigureAwait(false);

    if (log == null)
    {
      return;
    }

    log.StoppedAt = DateTime.UtcNow;
    var duration = log.StoppedAt.Value - log.StartedAt;
    log.HoursLogged = (decimal)duration.TotalHours;

    await this.context.SaveChangesAsync().ConfigureAwait(false);
  }

  /// <inheritdoc/>
  public async Task<IEnumerable<TimeLogDto>> GetTimeLogsAsync(Guid ticketId)
  {
    return await this.context.TimeLogs
        .AsNoTracking()
        .Include(l => l.User)
        .Where(l => l.TicketId == ticketId)
        .OrderByDescending(l => l.StartedAt)
        .Select(l => new TimeLogDto(
            l.Id,
            l.UserId,
            l.User != null ? l.User.UserName! : "Unbekannt",
            l.StartedAt,
            l.StoppedAt,
            l.HoursLogged,
            l.Description))
        .ToListAsync()
        .ConfigureAwait(false);
  }

  /// <inheritdoc/>
  public async Task<bool> IsTimerRunningAsync(Guid ticketId, Guid userId)
  {
    return await this.context.TimeLogs
        .AnyAsync(l => l.TicketId == ticketId && l.UserId == userId && l.StoppedAt == null)
        .ConfigureAwait(false);
  }
}
