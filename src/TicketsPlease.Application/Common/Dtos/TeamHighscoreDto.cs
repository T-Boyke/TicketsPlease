// <copyright file="TeamHighscoreDto.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Application.Common.Dtos;

using System;

/// <summary>
/// DTO für die Team-Performance im Highscore-Board.
/// </summary>
public record TeamHighscoreDto(
    Guid TeamId,
    string TeamName,
    string ColorCode,
    int CompletedTickets,
    int TotalStoryPoints,
    decimal TotalHoursLogged,
    double AverageDifficulty,
    int MemberCount);
