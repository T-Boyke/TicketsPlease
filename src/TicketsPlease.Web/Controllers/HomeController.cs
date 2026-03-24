// <copyright file="HomeController.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Web.Controllers;

using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TicketsPlease.Web.Models;

/// <summary>
/// Der Standard-Controller für die Startseite und allgemeine Seiten.
/// </summary>
internal sealed class HomeController : Controller
{
  /// <summary>
  /// Zeigt die Index-Seite an.
  /// </summary>
  /// <returns>Die Index-View.</returns>
  public IActionResult Index()
  {
    return this.View();
  }

  /// <summary>
  /// Zeigt die Datenschutz-Seite an.
  /// </summary>
  /// <returns>Die Privacy-View.</returns>
  public IActionResult Privacy()
  {
    return this.View();
  }

  /// <summary>
  /// Zeigt das Impressum an.
  /// </summary>
  /// <returns>Die Impressum-View.</returns>
  public IActionResult Impressum()
  {
    return this.View();
  }

  /// <summary>
  /// Zeigt die Fehlerseite für HTTP-Fehler an.
  /// </summary>
  /// <returns>Die Error-View mit Anforderungs-ID.</returns>
  [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
  public IActionResult Error()
  {
    return this.View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
  }
}
