// <copyright file="TeamDto.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Application.Common.Dtos;

using System;
using System.Collections.Generic;

/// <summary>
/// Datenübertragungsobjekt für ein Team.
/// </summary>
/// <param name="Id">Die ID des Teams.</param>
/// <param name="Name">Der Name des Teams.</param>
/// <param name="Description">Die Beschreibung des Teams.</param>
/// <param name="ColorCode">Der Farbcode des Teams.</param>
/// <param name="CreatedAt">Erstellungszeitpunkt.</param>
/// <param name="MemberCount">Anzahl der Mitglieder.</param>
/// <param name="Members">Liste der Mitglieder-Details.</param>
public record TeamDto(
    Guid Id,
    string Name,
    string Description,
    string ColorCode,
    DateTime CreatedAt,
    int MemberCount,
    IEnumerable<TeamMemberDto> Members);

/// <summary>
/// Datenübertragungsobjekt für ein Teammitglied.
/// </summary>
/// <param name="UserId">Die ID des Benutzers.</param>
/// <param name="UserName">Der Name des Benutzers.</param>
/// <param name="JoinedAt">Beitrittsdatum.</param>
/// <param name="IsTeamLead">Gibt an, ob der Benutzer Teamleiter ist.</param>
public record TeamMemberDto(
    Guid UserId,
    string UserName,
    DateTime JoinedAt,
    bool IsTeamLead);
