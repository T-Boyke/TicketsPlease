// <copyright file="DashboardStatsDto.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Application.Common.Dtos;

using System.Collections.Generic;

/// <summary>
/// DTO für die Dashboard-Statistiken (F4).
/// </summary>
public record DashboardStatsDto(
    int TotalTickets,
    int OpenTickets,
    int ClosedTickets,
    int TotalProjects,
    int ActiveProjects,
    int TotalUsers,
    IDictionary<string, int> UsersByRole,
    IEnumerable<UserHighscoreDto> IndividualHighscores,
    IEnumerable<TeamHighscoreDto> TeamHighscores);
