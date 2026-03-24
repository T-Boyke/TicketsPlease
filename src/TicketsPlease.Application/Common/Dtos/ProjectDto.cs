// <copyright file="ProjectDto.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Application.Common.Dtos;

using System;

/// <summary>
/// Datenübertragungsobjekt für ein Projekt (F2.2).
/// </summary>
/// <param name="Id">Die eindeutige ID des Projekts.</param>
/// <param name="Title">Der Titel des Projekts.</param>
/// <param name="Description">Die Kurzbeschreibung des Projekts.</param>
/// <param name="StartDate">Das geplante Startdatum.</param>
/// <param name="EndDate">Das optionale Enddatum.</param>
/// <param name="IsOpen">Gibt an, ob das Projekt noch für Tickets offen ist.</param>
/// <param name="TenantId">Die Mandanten-ID zur Datenisolierung.</param>
public record ProjectDto(
    Guid Id,
    string Title,
    string Description,
    DateTime StartDate,
    DateTime? EndDate,
    bool IsOpen,
    Guid TenantId);
