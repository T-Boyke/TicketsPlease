// <copyright file="TimeLogDto.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Application.Common.Dtos;

using System;

/// <summary>
/// Datenübertragungsobjekt für einen Zeiterfassungseintrag.
/// </summary>
/// <param name="Id">Die ID des Eintrags.</param>
/// <param name="UserId">Die ID des Benutzers.</param>
/// <param name="UserName">Der Name des Benutzers.</param>
/// <param name="StartedAt">Startzeitpunkt.</param>
/// <param name="StoppedAt">Endzeitpunkt.</param>
/// <param name="HoursLogged">Gebuchte Stunden.</param>
/// <param name="Description">Optionale Beschreibung.</param>
public record TimeLogDto(
    Guid Id,
    Guid UserId,
    string UserName,
    DateTime StartedAt,
    DateTime? StoppedAt,
    decimal HoursLogged,
    string? Description);
