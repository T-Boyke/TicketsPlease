// <copyright file="Notification.cs" company="TicketsPlease">
// Copyright (c) TicketsPlease. All rights reserved.
// </copyright>

namespace TicketsPlease.Domain.Entities;

using System;
using TicketsPlease.Domain.Common;

/// <summary>
/// Repräsentiert eine Systembenachrichtigung für einen Benutzer.
/// </summary>
public class Notification : BaseEntity
{
    /// <summary>
    /// Gets or sets die ID des zu benachrichtigenden Benutzers.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Gets or sets das Navigation-Property für den zu benachrichtigenden Benutzer.
    /// </summary>
    public User? User { get; set; }

    /// <summary>
    /// Gets or sets den Titel der Benachrichtigung.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets den detaillierten Inhalt der Benachrichtigung.
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets die (optionale) Ziel-URL, zu der die Benachrichtigung führen soll.
    /// </summary>
    public string? TargetUrl { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether die Benachrichtigung vom Benutzer als gelesen markiert wurde.
    /// </summary>
    public bool IsRead { get; set; }

    /// <summary>
    /// Gets or sets den Zeitpunkt (UTC) der Erstellung der Benachrichtigung.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
