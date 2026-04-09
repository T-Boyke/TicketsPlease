// <copyright file="PerformanceDetailDto.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Application.Common.Dtos;

using System.Collections.Generic;

/// <summary>
/// DTO für detaillierte Performance-Daten (für Charts und Profile).
/// </summary>
public record PerformanceDetailDto(
    string Name,
    IDictionary<string, int> StatusDistribution,
    IDictionary<string, int> PriorityDistribution,
    IDictionary<string, int> TypeDistribution,
    decimal TotalHours,
    int TotalTickets,
    int CompletedTickets,
    int TotalStoryPoints,
    double? AvgResolutionTimeHours);
