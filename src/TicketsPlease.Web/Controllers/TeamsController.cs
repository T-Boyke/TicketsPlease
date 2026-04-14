// <copyright file="TeamsController.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Web.Controllers;

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using TicketsPlease.Application.Common.Interfaces;
using TicketsPlease.Domain.Entities;

/// <summary>
/// Controller für das Teammanagement.
/// </summary>
[Authorize]
public sealed class TeamsController : Controller
{
  private readonly ITeamService teamService;
  private readonly UserManager<User> userManager;
  private readonly INotificationService notificationService;
  private readonly IStringLocalizer<TeamsController> localizer;

  /// <summary>
  /// Initializes a new instance of the <see cref="TeamsController"/> class.
  /// </summary>
  /// <param name="teamService">Der Teamservice.</param>
  /// <param name="userManager">Der Usermanager.</param>
  /// <param name="notificationService">Der Benachrichtigungs-Service.</param>
  /// <param name="localizer">Der Localizer.</param>
  public TeamsController(
      ITeamService teamService,
      UserManager<User> userManager,
      INotificationService notificationService,
      IStringLocalizer<TeamsController> localizer)
  {
    this.teamService = teamService;
    this.userManager = userManager;
    this.notificationService = notificationService;
    this.localizer = localizer;
  }

  /// <summary>
  /// Zeigt die Liste der Teams an, in denen der aktuelle Benutzer Mitglied ist.
  /// </summary>
  /// <returns>Die Index-View.</returns>
  [HttpGet]
  public async Task<IActionResult> Index()
  {
    var user = await this.userManager.GetUserAsync(this.User).ConfigureAwait(false);
    if (user == null)
    {
      return this.Challenge();
    }

    var teams = await this.teamService.GetTenantTeamsAsync(user.TenantId, user.Id).ConfigureAwait(false);
    return this.View(teams);
  }

  /// <summary>
  /// Zeigt Details eines Teams an.
  /// </summary>
  /// <param name="id">Die ID des Teams.</param>
  /// <returns>Die Details-View oder NotFound.</returns>
  [HttpGet]
  public async Task<IActionResult> Details(Guid id)
  {
    var team = await this.teamService.GetTeamDetailsAsync(id).ConfigureAwait(false);
    if (team == null)
    {
      return this.NotFound();
    }

    return this.View(team);
  }

  /// <summary>
  /// Zeigt die Management-Oberfläche für Teams an.
  /// </summary>
  /// <returns>Die Management-View.</returns>
  [HttpGet]
  [Authorize(Roles = "Admin,Teamlead")]
  public async Task<IActionResult> Management()
  {
    var allTeams = await this.teamService.GetAllTeamsAsync().ConfigureAwait(false);
    return this.View(allTeams);
  }

  /// <summary>
  /// Erstellt ein neues Team.
  /// </summary>
  /// <param name="name">Name des Teams.</param>
  /// <param name="description">Beschreibung.</param>
  /// <param name="colorCode">Farbcode.</param>
  /// <returns>Redirect zur Index-View.</returns>
  [HttpPost]
  [Authorize(Roles = "Admin")]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> Create(string name, string description, string colorCode)
  {
    if (string.IsNullOrWhiteSpace(name))
    {
      this.ModelState.AddModelError("Name", this.localizer["NameRequired"]);
      return this.RedirectToAction(nameof(this.Management));
    }

    var user = await this.userManager.GetUserAsync(this.User).ConfigureAwait(false);
    if (user != null)
    {
      await this.teamService.CreateTeamAsync(name, description, colorCode, user.Id).ConfigureAwait(false);
    }

    return this.RedirectToAction(nameof(this.Management));
  }

  /// <summary>
  /// Sendet eine Beitrittsanfrage für ein Team.
  /// </summary>
  /// <param name="teamId">ID des Teams.</param>
  /// <returns>Redirect zur Index-View.</returns>
  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> RequestJoin(Guid teamId)
  {
    var user = await this.userManager.GetUserAsync(this.User).ConfigureAwait(false);
    if (user == null)
    {
      return this.Challenge();
    }

    var team = await this.teamService.GetTeamDetailsAsync(teamId).ConfigureAwait(false);
    if (team == null)
    {
      return this.NotFound();
    }

    await this.teamService.RequestJoinAsync(teamId, user.Id).ConfigureAwait(false);

    // Benachrichtigung an Teamleads des Teams senden
    var teamLeads = team.Members.Where(m => m.IsTeamLead).Select(m => m.UserId).ToList();

    // Falls keine Teamleads, an alle Admins der Organisation
    if (teamLeads.Count == 0)
    {
      var admins = await this.userManager.GetUsersInRoleAsync("Admin").ConfigureAwait(false);
      teamLeads.AddRange(admins.Where(a => a.TenantId == user.TenantId).Select(a => a.Id));
    }

    foreach (var leadId in teamLeads.Distinct())
    {
      await this.notificationService.SendNotificationToUserAsync(
          leadId,
          this.localizer["JoinRequestTitle"],
          string.Format(this.localizer["JoinRequestMessage"], user.UserName, team.Name),
          $"/Teams/Details/{team.Id}").ConfigureAwait(false);
    }

    this.TempData["StatusMessage"] = this.localizer["JoinRequestSent"].Value;
    return this.RedirectToAction(nameof(this.Index));
  }

  /// <summary>
  /// Entscheidet über eine Beitrittsanfrage.
  /// </summary>
  /// <param name="requestId">ID der Anfrage.</param>
  /// <param name="approve">Ob die Anfrage angenommen werden soll.</param>
  /// <returns>Redirect zur Details-View des Teams.</returns>
  [HttpPost]
  [Authorize(Roles = "Admin,Teamlead")]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> DecideJoinRequest(Guid requestId, bool approve)
  {
    var currentUser = await this.userManager.GetUserAsync(this.User).ConfigureAwait(false);
    if (currentUser == null)
    {
      return this.Challenge();
    }

    await this.teamService.DecideJoinRequestAsync(requestId, currentUser.Id, approve).ConfigureAwait(false);

    this.TempData["StatusMessage"] = approve ? this.localizer["JoinRequestApproved"].Value : this.localizer["JoinRequestRejected"].Value;
    return this.RedirectToAction(nameof(this.Management));
  }
}
