// <copyright file="TicketPriorityDto.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Application.Common.Dtos;

using System;

/// <summary>
/// Datenübertragungsobjekt für Ticket-Prioritäten.
/// </summary>
/// <param name="Id">Die ID der Priorität.</param>
/// <param name="Name">Der Anzeigename.</param>
/// <param name="ColorHex">Der Hex-Farbcode für die UI.</param>
public record TicketPriorityDto(
    Guid Id,
    string Name,
    string ColorHex);
