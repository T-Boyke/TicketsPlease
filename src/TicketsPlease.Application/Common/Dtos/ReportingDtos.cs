// <copyright file="ReportingDtos.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Application.Common.Dtos;

using System;
using System.Collections.Generic;

/// <summary>
/// Datentransferobjekt für den SLA-Status.
/// </summary>
public record SlaComplianceDto(string ProjectName, int TotalTickets, int BreachedTickets, double ComplianceRate);

/// <summary>
/// Datentransferobjekt für den Durchsatz eines Teams.
/// </summary>
public record TeamThroughputDto(string TeamName, int TicketsCompleted, double AveragePointsPerWeek);

/// <summary>
/// Datentransferobjekt für den Projekt-Gesundheitsstatus.
/// </summary>
public record ProjectHealthDto(string ProjectName, int OpenTickets, int UrgentTickets, string HealthStatus);

/// <summary>
/// Zusammenfassendes DTO für das Stakeholder Dashboard.
/// </summary>
public record StakeholderDashboardDto(
    List<SlaComplianceDto> SlaCompliance,
    List<TeamThroughputDto> TeamThroughput,
    List<ProjectHealthDto> ProjectHealth,
    int TotalActiveUsers);
