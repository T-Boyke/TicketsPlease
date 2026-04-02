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
internal sealed class TeamsController : Controller
{
  private readonly ITeamService teamService;
  private readonly UserManager<User> userManager;
  private readonly IStringLocalizer<TeamsController> localizer;

  /// <summary>
  /// Initializes a new instance of the <see cref="TeamsController"/> class.
  /// </summary>
  /// <param name="teamService">Der Teamservice.</param>
  /// <param name="userManager">Der Usermanager.</param>
  /// <param name="localizer">Der Localizer.</param>
  public TeamsController(
      ITeamService teamService,
      UserManager<User> userManager,
      IStringLocalizer<TeamsController> localizer)
  {
    this.teamService = teamService;
    this.userManager = userManager;
    this.localizer = localizer;
  }

  /// <summary>
  /// Zeigt die Liste der Teams an, in denen der aktuelle Benutzer Mitglied ist.
  /// </summary>
  /// <returns>Die Index-View.</returns>
  public async Task<IActionResult> Index()
  {
    var user = await this.userManager.GetUserAsync(this.User).ConfigureAwait(false);
    if (user == null)
    {
      return this.Challenge();
    }

    var teams = await this.teamService.GetUserTeamsAsync(user.Id).ConfigureAwait(false);
    return this.View(teams);
  }

  /// <summary>
  /// Zeigt Details eines Teams an.
  /// </summary>
  /// <param name="id">Die ID des Teams.</param>
  /// <returns>Die Details-View oder NotFound.</returns>
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
}
