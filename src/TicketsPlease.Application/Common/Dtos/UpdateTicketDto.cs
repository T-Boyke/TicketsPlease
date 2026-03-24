// <copyright file="UpdateTicketDto.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Application.Common.Dtos;

using System;

/// <summary>
/// DTO zum Aktualisieren eines Tickets.
/// </summary>
/// <param name="Id">Die ID des Tickets.</param>
/// <param name="Title">Der neue Titel.</param>
/// <param name="Description">Die neue Beschreibung.</param>
/// <param name="Status">Der neue Status (z.B. To-Do, Doing, Closed).</param>
/// <param name="PriorityId">Die ID der neuen Priorität.</param>
/// <param name="AssignedUserId">Die ID des neuen Zuständigen.</param>
/// <param name="EstimatePoints">Die aktualisierten Story Points.</param>
public record UpdateTicketDto(
    Guid Id,
    string Title,
    string Description,
    string Status,
    Guid PriorityId,
    Guid? AssignedUserId,
    int? EstimatePoints);
