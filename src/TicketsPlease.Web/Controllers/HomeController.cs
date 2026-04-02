// <copyright file="HomeController.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Web.Controllers;

using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TicketsPlease.Application.Common.Interfaces;
using TicketsPlease.Web.Models;

/// <summary>
/// Der Standard-Controller für die Startseite und allgemeine Seiten.
/// </summary>
internal sealed class HomeController : Controller
{
  private readonly IDashboardService dashboardService;

  public HomeController(IDashboardService dashboardService)
  {
    this.dashboardService = dashboardService;
  }

  /// <summary>
  /// Zeigt die Index-Seite an.
  /// </summary>
  /// <returns>Die Index-View.</returns>
  [HttpGet]
  public async Task<IActionResult> Index()
  {
    if (this.User.Identity?.IsAuthenticated == true)
    {
      var stats = await this.dashboardService.GetDashboardStatsAsync().ConfigureAwait(false);
      return this.View(stats);
    }

    return this.View();
  }

  /// <summary>
  /// Zeigt die Datenschutz-Seite an.
  /// </summary>
  /// <returns>Die Privacy-View.</returns>
  [HttpGet]
  public IActionResult Privacy()
  {
    return this.View();
  }

  /// <summary>
  /// Zeigt das Impressum an.
  /// </summary>
  /// <returns>Die Impressum-View.</returns>
  [HttpGet]
  public IActionResult Impressum()
  {
    return this.View();
  }

  /// <summary>
  /// Zeigt die Fehlerseite für HTTP-Fehler an.
  /// </summary>
  /// <returns>Die Error-View mit Anforderungs-ID.</returns>
  [HttpGet]
  [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
  public IActionResult Error()
  {
    return this.View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
  }
}
