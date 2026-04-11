// <copyright file="INotificationRepository.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Application.Common.Interfaces;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TicketsPlease.Domain.Entities;

/// <summary>
/// Repository-Interface für den Zugriff auf <see cref="Notification"/>-Entitäten.
/// </summary>
public interface INotificationRepository
{
    /// <summary>
    /// Ruft Benachrichtigungen für einen Benutzer ab.
    /// </summary>
    /// <param name="userId">Die Benutzer-ID.</param>
    /// <param name="limit">Maximalanzahl.</param>
    /// <param name="offset">Offset für Paging.</param>
    /// <param name="ct">CancellationToken.</param>
    /// <returns>Eine Liste von Benachrichtigungen.</returns>
    Task<List<Notification>> GetByUserIdAsync(Guid userId, int limit = 20, int offset = 0, CancellationToken ct = default);

    /// <summary>
    /// Ruft eine Benachrichtigung nach ID ab.
    /// </summary>
    /// <param name="id">Die ID.</param>
    /// <param name="ct">CancellationToken.</param>
    /// <returns>Die Benachrichtigung oder null.</returns>
    Task<Notification?> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>
    /// Speichert Änderungen im Kontext.
    /// </summary>
    /// <param name="ct">CancellationToken.</param>
    /// <returns>Anzahl der betroffenen Zeilen.</returns>
    Task<int> SaveChangesAsync(CancellationToken ct = default);

    /// <summary>
    /// Fügt eine neue Benachrichtigung hinzu.
    /// </summary>
    /// <param name="notification">Die Benachrichtigung.</param>
    /// <param name="ct">CancellationToken.</param>
    /// <returns>Task.</returns>
    Task AddAsync(Notification notification, CancellationToken ct = default);
}
