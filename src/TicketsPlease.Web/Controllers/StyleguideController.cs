// <copyright file="StyleguideController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TicketsPlease.Web.Controllers;

using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Controller für den UI-Styleguide.
/// Dient als Referenz für Entwickler, um einheitliche UI-Komponenten zu verwenden.
/// </summary>
internal sealed class StyleguideController : Controller
{
  /// <summary>
  /// Zeigt die Übersicht aller UI-Komponenten und Design-Tokens an.
  /// </summary>
  /// <returns>Die Styleguide-View.</returns>
  public IActionResult Index()
  {
    return this.View();
  }
}
