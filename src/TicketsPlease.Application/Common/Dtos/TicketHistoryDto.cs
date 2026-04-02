// <copyright file="TicketHistoryDto.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Application.Common.Dtos;

using System;

/// <summary>
/// Datenübertragungsobjekt für einen Eintrag in der Ticket-Historie.
/// </summary>
public record TicketHistoryDto(
    Guid Id,
    string ChangeType,
    string OldValue,
    string NewValue,
    DateTime ChangedAt,
    string ChangedByUserName);
