// <copyright file="NotificationDto.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Application.Common.Dtos;

using System;

/// <summary>
/// Datenübertragungsobjekt für eine Benachrichtigung.
/// </summary>
public class NotificationDto
{
    /// <summary>
    /// Gets or sets die ID der Benachrichtigung.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets den Titel.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets den Inhalt.
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets die Ziel-URL.
    /// </summary>
    public string? TargetUrl { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether die Benachrichtigung gelesen wurde.
    /// </summary>
    public bool IsRead { get; set; }

    /// <summary>
    /// Gets or sets den Erstellungszeitpunkt.
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
