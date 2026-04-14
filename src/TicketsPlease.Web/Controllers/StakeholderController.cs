// <copyright file="StakeholderController.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Web.Controllers;

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TicketsPlease.Application.Common.Interfaces;
using TicketsPlease.Domain.Entities;

/// <summary>
/// Controller für das Stakeholder-Reporting-Dashboard.
/// </summary>
[Authorize(Roles = "Admin,ProductOwner,Stakeholder")]
internal sealed class StakeholderController : Controller
{
  private readonly IReportingService reportingService;
  private readonly UserManager<User> userManager;

  /// <summary>
  /// Initializes a new instance of the <see cref="StakeholderController"/> class.
  /// </summary>
  /// <param name="reportingService">Der Reporting-Service.</param>
  /// <param name="userManager">Der User-Manager.</param>
  public StakeholderController(IReportingService reportingService, UserManager<User> userManager)
  {
    this.reportingService = reportingService;
    this.userManager = userManager;
  }

  /// <summary>
  /// Zeigt das Stakeholder-Dashboard an.
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

    var dashboardData = await this.reportingService.GetStakeholderDashboardAsync(user.TenantId).ConfigureAwait(false);
    return this.View(dashboardData);
  }
}
