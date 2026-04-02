// <copyright file="CreateTicketDto.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Application.Common.Dtos;

using System;

/// <summary>
/// DTO zum Erstellen eines neuen Tickets.
/// </summary>
/// <param name="Title">Der Titel des Tickets.</param>
/// <param name="Description">Die Beschreibung des Tasks.</param>
/// <param name="ProjectId">Die ID des Projekts, dem es zugeordnet wird.</param>
/// <param name="PriorityId">Die ID der Priorität.</param>
/// <param name="AssignedUserId">Die ID des zuständigen Benutzers.</param>
/// <param name="EstimatePoints">Die Story Points.</param>
/// <param name="ChilliesDifficulty">Die Schwierigkeit (1-5 Chilis).</param>
/// <param name="TagIds">Die IDs der zuzuordnenden Tags.</param>
public record CreateTicketDto(
    string Title,
    string Description,
    Guid ProjectId,
    Guid PriorityId,
    Guid? AssignedUserId,
    int? EstimatePoints,
    int ChilliesDifficulty = 1,
    System.Collections.Generic.IList<Guid>? TagIds = null);
