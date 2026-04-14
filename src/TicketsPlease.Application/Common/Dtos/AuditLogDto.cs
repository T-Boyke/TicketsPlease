// <copyright file="AuditLogDto.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Application.Common.Dtos;

using System;

/// <summary>
/// DTO für einen Audit-Log-Eintrag.
/// </summary>
/// <param name="Timestamp">Zeitpunkt der Aktion.</param>
/// <param name="ActorUserName">Name des ausführenden Benutzers.</param>
/// <param name="ActionType">Typ der Aktion.</param>
/// <param name="Description">Beschreibung der Änderungen.</param>
public record AuditLogDto(
    DateTime Timestamp,
    string ActorUserName,
    string ActionType,
    string Description);
