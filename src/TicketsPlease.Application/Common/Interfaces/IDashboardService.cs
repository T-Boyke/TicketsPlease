// <copyright file="IDashboardService.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Application.Common.Interfaces;

using System.Threading.Tasks;
using TicketsPlease.Application.Common.Dtos;

/// <summary>
/// Interface für den Dashboard-Service zur Aggregation von Statistiken (F4).
/// </summary>
public interface IDashboardService
{
  /// <summary>
  /// Ruft alle relevanten Statistiken für das Dashboard ab.
  /// </summary>
  /// <returns>Ein <see cref="DashboardStatsDto"/>.</returns>
  public Task<DashboardStatsDto> GetDashboardStatsAsync();

  /// <summary>
  /// Ruft detaillierte Performance-Daten für einen Benutzer ab.
  /// </summary>
  /// <param name="userId">Die ID des Benutzers.</param>
  /// <returns>Ein <see cref="PerformanceDetailDto"/>.</returns>
  public Task<PerformanceDetailDto> GetUserPerformanceDetailAsync(Guid userId);

  /// <summary>
  /// Ruft detaillierte Performance-Daten für ein Team ab.
  /// </summary>
  /// <param name="teamId">Die ID des Teams.</param>
  /// <returns>Ein <see cref="PerformanceDetailDto"/>.</returns>
  public Task<PerformanceDetailDto> GetTeamPerformanceDetailAsync(Guid teamId);
}
