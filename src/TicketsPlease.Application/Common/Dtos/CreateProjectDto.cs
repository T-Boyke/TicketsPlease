// <copyright file="CreateProjectDto.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Application.Common.Dtos;

using System;

/// <summary>
/// DTO zum Erstellen eines neuen Projekts.
/// </summary>
/// <param name="Title">Der Titel des Projekts (Pflicht).</param>
/// <param name="Description">Die Kurzbeschreibung.</param>
/// <param name="StartDate">Das Startdatum (Pflicht).</param>
/// <param name="EndDate">Das optionale Enddatum.</param>
public record CreateProjectDto(
    string Title,
    string Description,
    DateTime StartDate,
    DateTime? EndDate = null);
