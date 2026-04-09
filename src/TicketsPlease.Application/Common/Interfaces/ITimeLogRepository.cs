// <copyright file="ITimeLogRepository.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Application.Common.Interfaces;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TicketsPlease.Domain.Entities;

/// <summary>
/// Definiert die Datenzugriffsschicht für <see cref="TimeLog"/> Entitäten.
/// </summary>
public interface ITimeLogRepository
{
    /// <summary>
    /// Ruft alle Zeiterfassungseinträge ab.
    /// </summary>
    /// <param name="ct">Das Abbruchsignal.</param>
    /// <returns>Eine Liste von TimeLogs.</returns>
    Task<List<TimeLog>> GetAllAsync(CancellationToken ct = default);

    /// <summary>
    /// Ruft alle Zeiterfassungseinträge für einen bestimmten Benutzer ab.
    /// </summary>
    /// <param name="userId">Die ID des Benutzers.</param>
    /// <param name="ct">Das Abbruchsignal.</param>
    /// <returns>Eine Liste von TimeLogs.</returns>
    Task<List<TimeLog>> GetByUserIdAsync(Guid userId, CancellationToken ct = default);
}
