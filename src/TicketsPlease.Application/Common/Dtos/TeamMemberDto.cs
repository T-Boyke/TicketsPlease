// <copyright file="TeamMemberDto.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Application.Common.Dtos;

using System;

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
