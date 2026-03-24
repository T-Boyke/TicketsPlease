// <copyright file="CommentDto.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Application.Common.Dtos;

using System;

/// <summary>
/// Datenübertragungsobjekt für einen Kommentar (F5).
/// </summary>
/// <param name="Id">Die ID des Kommentars.</param>
/// <param name="Content">Der Inhalt.</param>
/// <param name="AuthorId">Die ID des Erstellers.</param>
/// <param name="AuthorName">Der Name des Erstellers.</param>
/// <param name="CreatedAt">Der Erstellungszeitpunkt.</param>
public record CommentDto(
    Guid Id,
    string Content,
    Guid AuthorId,
    string AuthorName,
    DateTime CreatedAt);
