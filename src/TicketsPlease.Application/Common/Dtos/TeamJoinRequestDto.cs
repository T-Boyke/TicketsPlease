// <copyright file="TeamJoinRequestDto.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Application.Common.Dtos;

using System;
using TicketsPlease.Domain.Enums;

/// <summary>
/// Datenübertragungsobjekt für eine Beitrittsanfrage.
/// </summary>
/// <param name="Id">Die ID der Anfrage.</param>
/// <param name="TeamId">Die ID des Teams.</param>
/// <param name="TeamName">Der Name des Teams.</param>
/// <param name="UserId">Die ID des Benutzers.</param>
/// <param name="UserName">Der Name des Benutzers.</param>
/// <param name="Status">Der Status der Anfrage.</param>
/// <param name="RequestedAt">Zeitpunkt der Anfrage.</param>
public record TeamJoinRequestDto(
    Guid Id,
    Guid TeamId,
    string TeamName,
    Guid UserId,
    string UserName,
    JoinRequestStatus Status,
    DateTime RequestedAt);
