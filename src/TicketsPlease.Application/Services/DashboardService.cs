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

  /// <summary>
  /// Initializes a new instance of the <see cref="DashboardService"/> class.
  /// </summary>
  /// <param name="ticketRepository">The ticket repository.</param>
  /// <param name="projectRepository">The project repository.</param>
  /// <param name="userManager">The user manager.</param>
  /// <param name="roleManager">The role manager.</param>
  public DashboardService(
      ITicketRepository ticketRepository,
      IProjectRepository projectRepository,
      UserManager<User> userManager,
      RoleManager<Role> roleManager)
  {
    this.ticketRepository = ticketRepository;
    this.projectRepository = projectRepository;
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

    return new DashboardStatsDto(
        tickets.Count,
        tickets.Count(t => t.Status != "Closed" && t.Status != "Done"),
        tickets.Count(t => t.Status == "Closed" || t.Status == "Done"),
        projects.Count(),
        projects.Count(p => !p.EndDate.HasValue || p.EndDate > System.DateTime.UtcNow),
        users.Count,
        usersByRole);
  }
}
