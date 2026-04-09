// <copyright file="DashboardService.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Application.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using TicketsPlease.Application.Common.Dtos;
using TicketsPlease.Application.Common.Interfaces;
using TicketsPlease.Domain.Entities;

/// <summary>
/// Implementierung des IDashboardService zur Aggregation von System-Statistiken.
/// Adheres to Clean Architecture by using repositories instead of DbContext.
/// </summary>
public class DashboardService : IDashboardService
{
  private readonly ITicketRepository ticketRepository;
  private readonly IProjectRepository projectRepository;
  private readonly UserManager<User> userManager;
  private readonly RoleManager<Role> roleManager;
  private readonly ITeamRepository teamRepository;
  private readonly ITimeLogRepository timeLogRepository;

  /// <summary>
  /// Initializes a new instance of the <see cref="DashboardService"/> class.
  /// </summary>
  /// <param name="ticketRepository">The ticket repository.</param>
  /// <param name="projectRepository">The project repository.</param>
  /// <param name="teamRepository">The team repository.</param>
  /// <param name="timeLogRepository">The time log repository.</param>
  /// <param name="userManager">The user manager.</param>
  /// <param name="roleManager">The role manager.</param>
  public DashboardService(
      ITicketRepository ticketRepository,
      IProjectRepository projectRepository,
      ITeamRepository teamRepository,
      ITimeLogRepository timeLogRepository,
      UserManager<User> userManager,
      RoleManager<Role> roleManager)
  {
    this.ticketRepository = ticketRepository;
    this.projectRepository = projectRepository;
    this.teamRepository = teamRepository;
    this.timeLogRepository = timeLogRepository;
    this.userManager = userManager;
    this.roleManager = roleManager;
  }

  /// <inheritdoc />
  public async Task<DashboardStatsDto> GetDashboardStatsAsync()
  {
    // Hinweis: Wir nutzen hier die Repositories für den Datenzugriff.
    // Falls organisationsspezifische Stats gewünscht sind, müssten die Repositories entsprechend gefiltert werden.
    var tickets = await this.ticketRepository.GetAllActiveAsync().ConfigureAwait(false);

    // Da IProjectRepository.GetAllAsync eine TenantId benötigt, weichen wir hier auf eine leere Guid aus
    // oder implementieren ein SystemWide-Repository-Pattern falls nötig.
    // Für das MVP nehmen wir an, dass wir alle Projekte zählen wollen.
    var projects = await this.projectRepository.GetAllAsync(Guid.Empty).ConfigureAwait(false);
    var users = this.userManager.Users.ToList();
    var roles = this.roleManager.Roles.ToList();

    var usersByRole = new Dictionary<string, int>();
    foreach (var roleName in roles.Select(r => r.Name!))
    {
      var usersInRole = await this.userManager.GetUsersInRoleAsync(roleName).ConfigureAwait(false);
      usersByRole.Add(roleName, usersInRole.Count);
    }

    // Highscore Berechnung
    var allTimeLogs = await this.timeLogRepository.GetAllAsync().ConfigureAwait(false);
    var allTeams = await this.teamRepository.GetAllTeamsAsync().ConfigureAwait(false);

    var individualHighscores = users.Select(u =>
    {
      var userTickets = tickets.Where(t => t.AssignedUserId == u.Id && (t.Status == "Closed" || t.Status == "Done")).ToList();
      var userLogs = allTimeLogs.Where(l => l.UserId == u.Id).ToList();
      return new UserHighscoreDto(
          u.Id,
          u.UserName ?? "Unknown",
          null, // Avatar placeholder
          userTickets.Count,
          userTickets.Sum(t => t.EstimatePoints ?? 0),
          userLogs.Sum(l => l.HoursLogged),
          userTickets.Count > 0 ? userTickets.Average(t => t.ChilliesDifficulty) : 0);
    })
    .Where(h => h.CompletedTickets > 0)
    .OrderByDescending(h => h.CompletedTickets)
    .Take(10)
    .ToList();

    var teamHighscores = allTeams.Select(t =>
    {
      var memberIds = t.Members.Select(m => m.UserId).ToList();
      var teamTickets = tickets.Where(tk => tk.AssignedUserId.HasValue && memberIds.Contains(tk.AssignedUserId.Value) && (tk.Status == "Closed" || tk.Status == "Done")).ToList();
      var teamLogs = allTimeLogs.Where(l => memberIds.Contains(l.UserId)).ToList();
      return new TeamHighscoreDto(
          t.Id,
          t.Name,
          t.ColorCode,
          teamTickets.Count,
          teamTickets.Sum(tk => tk.EstimatePoints ?? 0),
          teamLogs.Sum(l => l.HoursLogged),
          teamTickets.Count > 0 ? teamTickets.Average(tk => tk.ChilliesDifficulty) : 0,
          t.Members.Count);
    })
    .Where(h => h.CompletedTickets > 0)
    .OrderByDescending(h => h.CompletedTickets)
    .Take(10)
    .ToList();

    return new DashboardStatsDto(
        tickets.Count,
        tickets.Count(t => t.Status != "Closed" && t.Status != "Done"),
        tickets.Count(t => t.Status == "Closed" || t.Status == "Done"),
        projects.Count(),
        projects.Count(p => !p.EndDate.HasValue || p.EndDate > System.DateTime.UtcNow),
        users.Count,
        usersByRole,
        individualHighscores,
        teamHighscores);
  }

  /// <inheritdoc />
  public async Task<PerformanceDetailDto> GetUserPerformanceDetailAsync(Guid userId)
  {
    var user = await this.userManager.FindByIdAsync(userId.ToString()).ConfigureAwait(false);
    if (user == null)
    {
      throw new KeyNotFoundException("User not found");
    }

    var tickets = await this.ticketRepository.GetFilteredAsync(assignedUserId: userId).ConfigureAwait(false);
    var logs = await this.timeLogRepository.GetByUserIdAsync(userId).ConfigureAwait(false);

    var closedTickets = tickets.Where(t => t.ClosedAt.HasValue).ToList();
    var avgResolutionTime = closedTickets.Any()
        ? closedTickets.Average(t => (t.ClosedAt!.Value - t.CreatedAt).TotalHours)
        : (double?)null;

    return new PerformanceDetailDto(
        user.UserName ?? "Unknown",
        tickets.GroupBy(t => t.Status).ToDictionary(g => g.Key, g => g.Count()),
        tickets.Where(t => t.Priority != null).GroupBy(t => t.Priority!.Name).ToDictionary(g => g.Key, g => g.Count()),
        tickets.GroupBy(t => t.Type.ToString()).ToDictionary(g => g.Key, g => g.Count()),
        logs.Sum(l => l.HoursLogged),
        tickets.Count,
        tickets.Count(t => t.Status == "Closed" || t.Status == "Done"),
        tickets.Sum(t => t.EstimatePoints ?? 0),
        avgResolutionTime);
  }

  /// <inheritdoc />
  public async Task<PerformanceDetailDto> GetTeamPerformanceDetailAsync(Guid teamId)
  {
    var team = await this.teamRepository.GetByIdAsync(teamId).ConfigureAwait(false);
    if (team == null)
    {
      throw new KeyNotFoundException("Team not found");
    }

    var memberIds = team.Members.Select(m => m.UserId).ToList();
    var allTickets = await this.ticketRepository.GetAllActiveAsync().ConfigureAwait(false);
    var teamTickets = allTickets.Where(t => t.AssignedUserId.HasValue && memberIds.Contains(t.AssignedUserId.Value)).ToList();

    var allLogs = await this.timeLogRepository.GetAllAsync().ConfigureAwait(false);
    var teamLogs = allLogs.Where(l => memberIds.Contains(l.UserId)).ToList();

    var teamClosedTickets = teamTickets.Where(t => t.ClosedAt.HasValue).ToList();
    var teamAvgResolutionTime = teamClosedTickets.Any()
        ? teamClosedTickets.Average(t => (t.ClosedAt!.Value - t.CreatedAt).TotalHours)
        : (double?)null;

    return new PerformanceDetailDto(
        team.Name,
        teamTickets.GroupBy(t => t.Status).ToDictionary(g => g.Key, g => g.Count()),
        teamTickets.Where(t => t.Priority != null).GroupBy(t => t.Priority!.Name).ToDictionary(g => g.Key, g => g.Count()),
        teamTickets.GroupBy(t => t.Type.ToString()).ToDictionary(g => g.Key, g => g.Count()),
        teamLogs.Sum(l => l.HoursLogged),
        teamTickets.Count,
        teamTickets.Count(t => t.Status == "Closed" || t.Status == "Done"),
        teamTickets.Sum(t => t.EstimatePoints ?? 0),
        teamAvgResolutionTime);
  }
}
