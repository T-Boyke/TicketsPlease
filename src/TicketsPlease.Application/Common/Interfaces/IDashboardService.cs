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
    Task<DashboardStatsDto> GetDashboardStatsAsync();
}
