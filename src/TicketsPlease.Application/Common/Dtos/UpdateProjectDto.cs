// <copyright file="UpdateProjectDto.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Application.Common.Dtos;

using System;

/// <summary>
/// DTO zum Aktualisieren eines bestehenden Projekts.
/// </summary>
/// <param name="Id">Die ID des zu aktualisierenden Projekts.</param>
/// <param name="Title">Der neue Titel.</param>
/// <param name="Description">Die neue Beschreibung.</param>
/// <param name="StartDate">Das aktualisierte Startdatum.</param>
/// <param name="EndDate">Das optionale Enddatum.</param>
/// <param name="IsOpen">Gibt an, ob das Projekt offen bleibt.</param>
public record UpdateProjectDto(
    Guid Id,
    string Title,
    string Description,
    DateTime StartDate,
    DateTime? EndDate,
    bool IsOpen);
