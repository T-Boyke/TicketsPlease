// <copyright file="NotificationsViewModel.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Web.Models;

using System.Collections.Generic;
using TicketsPlease.Application.Common.Dtos;

/// <summary>
/// ViewModel für die Benachrichtigungsübersicht.
/// </summary>
public class NotificationsViewModel
{
    /// <summary>
    /// Gets or sets die Liste der Benachrichtigungen.
    /// </summary>
    public List<NotificationDto> Notifications { get; set; } = new();

    /// <summary>
    /// Gets or sets a value indicating whether es mehr Benachrichtigungen zum Laden gibt.
    /// </summary>
    public bool HasMore { get; set; }
}
