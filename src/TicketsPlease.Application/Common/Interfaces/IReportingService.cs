// <copyright file="IReportingService.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Application.Common.Interfaces;

using System;
using System.Threading.Tasks;
using TicketsPlease.Application.Common.Dtos;

/// <summary>
/// Schnittstelle für den Berichterstattungsdienst (Stakeholder Dashboard).
/// </summary>
public interface IReportingService
{
  /// <summary>
  /// Ruft die zusammengefassten Statistiken für das Stakeholder-Dashboard ab.
  /// </summary>
  /// <param name="tenantId">Die ID der Organisation.</param>
  /// <returns>Das Dashboard DTO.</returns>
  Task<StakeholderDashboardDto> GetStakeholderDashboardAsync(Guid tenantId);
}
