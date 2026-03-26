// <copyright file="SubTicketDto.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Application.Common.Dtos;

using System;

/// <summary>
/// Datenübertragungsobjekt für ein Unterticket (Checklisten-Task).
/// </summary>
/// <param name="Id">Die ID des Untertickets.</param>
/// <param name="Title">Der Titel / die Aufgabe.</param>
/// <param name="IsCompleted">Gibt an, ob die Aufgabe erledigt ist.</param>
public record SubTicketDto(
    Guid Id,
    string Title,
    bool IsCompleted);
