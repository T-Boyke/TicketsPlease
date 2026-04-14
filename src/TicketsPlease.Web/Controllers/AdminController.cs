// <copyright file="AdminController.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Web.Controllers;

using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketsPlease.Infrastructure.Services;

/// <summary>
/// Haupt-Controller für den Administrationsbereich.
/// Erfordert die Admin-Rolle.
/// </summary>
[Authorize(Roles = "Admin")]
internal sealed class AdminController : Controller
{
  private readonly SystemMaintenanceService maintenanceService;

  /// <summary>
  /// Initializes a new instance of the <see cref="AdminController"/> class.
  /// </summary>
  /// <param name="maintenanceService">Der Wartungsdienst.</param>
  public AdminController(SystemMaintenanceService maintenanceService)
  {
    this.maintenanceService = maintenanceService;
  }

  /// <summary>
  /// Leitet zur Benutzerverwaltung weiter.
  /// </summary>
  /// <returns>Ein Redirect auf die Benutzerliste.</returns>
  [HttpGet]
  public IActionResult Index()
  {
    return this.RedirectToAction("Index", "AdminUsers");
  }

  /// <summary>
  /// Leitet zur tatsächlichen Benutzerverwaltung weiter.
  /// </summary>
  /// <returns>Ein Redirect auf die Benutzerliste.</returns>
  [HttpGet]
  public IActionResult Users()
  {
    return this.RedirectToAction("Index", "AdminUsers");
  }

  /// <summary>
  /// Zeigt die Systemeinstellungen an.
  /// </summary>
  /// <returns>Die Settings-View.</returns>
  [HttpGet]
  public IActionResult Settings()
  {
    return this.View();
  }

  /// <summary>
  /// Führt einen kompletten Datenbank-Wipe aus.
  /// </summary>
  /// <param name="confirmPhrase">Die Sicherheitsphrase.</param>
  /// <returns>Ein Task.</returns>
  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> WipeDatabase(string confirmPhrase)
  {
    if (confirmPhrase != "DELETE CONFIRM")
    {
      this.ModelState.AddModelError(string.Empty, "Ungültige Sicherheitsphrase.");
      return this.View("Settings");
    }

    await this.maintenanceService.WipeDatabaseAsync().ConfigureAwait(false);
    this.TempData["StatusMessage"] = "Datenbank wurde erfolgreich zurückgesetzt. Bitte führen Sie ein Re-Seeding durch.";
    return this.RedirectToAction(nameof(this.Settings));
  }

  /// <summary>
  /// Leert eine spezifische Tabelle.
  /// </summary>
  /// <param name="tableName">Tabellenname.</param>
  /// <param name="confirmPhrase">Die Sicherheitsphrase.</param>
  /// <returns>Ein Task.</returns>
  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> TruncateTable(string tableName, string confirmPhrase)
  {
    if (confirmPhrase != "DELETE CONFIRM")
    {
      this.ModelState.AddModelError(string.Empty, "Ungültige Sicherheitsphrase.");
      return this.View("Settings");
    }

    try
    {
      await this.maintenanceService.TruncateTableAsync(tableName).ConfigureAwait(false);
      this.TempData["StatusMessage"] = $"Tabelle {tableName} wurde erfolgreich geleert.";
    }
    catch (Exception ex)
    {
      this.ModelState.AddModelError(string.Empty, ex.Message);
    }

    return this.RedirectToAction(nameof(this.Settings));
  }
}
