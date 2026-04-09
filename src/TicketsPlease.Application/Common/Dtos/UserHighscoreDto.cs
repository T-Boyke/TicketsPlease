// <copyright file="UserHighscoreDto.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Application.Common.Dtos;

using System;

/// <summary>
/// DTO für die User-Performance im Highscore-Board.
/// </summary>
public record UserHighscoreDto(
    Guid UserId,
    string UserName,
    string? UserAvatar,
    int CompletedTickets,
    int TotalStoryPoints,
    decimal TotalHoursLogged,
    double AverageDifficulty);
