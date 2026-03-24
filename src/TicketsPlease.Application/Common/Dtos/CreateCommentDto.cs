// <copyright file="CreateCommentDto.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Application.Common.Dtos;

using System;

/// <summary>
/// DTO zum Erstellen eines neuen Kommentars (F5).
/// </summary>
/// <param name="TicketId">Die ID des zugehörigen Tickets.</param>
/// <param name="Content">Der Inhalt des Kommentars.</param>
public record CreateCommentDto(
    Guid TicketId,
    string Content);
