// <copyright file="ReportingService.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Application.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketsPlease.Application.Common.Dtos;
using TicketsPlease.Application.Common.Interfaces;

/// <summary>
/// Implementierung des ReportingService für Stakeholder-Auswertungen.
/// </summary>
public class ReportingService : IReportingService
{
  private readonly IProjectRepository projectRepository;
  private readonly ITicketRepository ticketRepository;
  private readonly ITeamRepository teamRepository;
  private readonly IUserRepository userRepository;

  /// <summary>
  /// Initializes a new instance of the <see cref="ReportingService"/> class.
  /// </summary>
  public ReportingService(
      IProjectRepository projectRepository,
      ITicketRepository ticketRepository,
      ITeamRepository teamRepository,
      IUserRepository userRepository)
  {
    this.projectRepository = projectRepository;
    this.ticketRepository = ticketRepository;
    this.teamRepository = teamRepository;
    this.userRepository = userRepository;
  }

  /// <inheritdoc />
  public async Task<StakeholderDashboardDto> GetStakeholderDashboardAsync(Guid tenantId)
  {
    // 1. SLA Compliance pro Projekt
    var projects = (await this.projectRepository.GetAllAsync(tenantId).ConfigureAwait(false)).ToList();
    var allTickets = await this.ticketRepository.GetByTenantAsync(tenantId).ConfigureAwait(false);

    var slaCompliance = projects.Select(p =>
    {
      var projectTickets = allTickets.Where(t => t.ProjectId == p.Id).ToList();
      var total = projectTickets.Count;
      var breached = projectTickets.Count(t => t.ClosedAt.HasValue && (t.ClosedAt.Value - t.CreatedAt).TotalHours > 24); // Beispiel-SLA: 24h
      var rate = total > 0 ? (double)(total - breached) / total * 100 : 100;
      return new SlaComplianceDto(p.Title, total, breached, Math.Round(rate, 2));
    }).ToList();

    // 2. Team Durchsatz
    var teams = await this.teamRepository.GetTeamsByTenantAsync(tenantId).ConfigureAwait(false);
    var doneTickets = allTickets.Where(t => t.Status == "Done").ToList();

    var teamThroughput = teams.Select(team =>
    {
      var memberIds = team.Members.Select(m => m.UserId).ToList();
      var completedCount = doneTickets.Count(t => t.AssignedUserId.HasValue && memberIds.Contains(t.AssignedUserId.Value));
      return new TeamThroughputDto(team.Name, completedCount, Math.Round((double)completedCount / 4, 2)); // Dummy "per Week" (Last 4 weeks)
    }).ToList();

    // 3. Projekt-Gesundheit
    var projectHealth = projects.Select(p =>
    {
      var projectTickets = allTickets.Where(t => t.ProjectId == p.Id).ToList();
      var open = projectTickets.Count(t => t.Status != "Done");
      var urgent = projectTickets.Count(t => t.Priority != null && t.Priority.Name == "Blocker");
      var status = urgent > 2 ? "At Risk" : (open > 10 ? "Warning" : "Healthy");
      return new ProjectHealthDto(p.Title, open, urgent, status);
    }).ToList();

    // 4. Aktive User
    var userCount = await this.userRepository.GetActiveUserCountAsync(tenantId).ConfigureAwait(false);

    return new StakeholderDashboardDto(slaCompliance, teamThroughput, projectHealth, userCount);
  }
}
