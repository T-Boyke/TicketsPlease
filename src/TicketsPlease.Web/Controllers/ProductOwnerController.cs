// <copyright file="ProductOwnerController.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Web.Controllers;

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TicketsPlease.Application.Common.Dtos;
using TicketsPlease.Application.Common.Interfaces;
using TicketsPlease.Domain.Entities;

/// <summary>
/// Controller für ProductOwner-spezifische Aufgaben (Workspace Settings, Invites).
/// </summary>
[Authorize(Roles = "Admin,ProductOwner")]
internal sealed class ProductOwnerController : Controller
{
  private readonly IOrganizationInviteService inviteService;
  private readonly IOrganizationService organizationService;
  private readonly UserManager<User> userManager;

  /// <summary>
  /// Initializes a new instance of the <see cref="ProductOwnerController"/> class.
  /// </summary>
  /// <param name="inviteService">Der Einladungs-Service.</param>
  /// <param name="organizationService">Der Organisations-Service.</param>
  /// <param name="userManager">Der User-Manager.</param>
  public ProductOwnerController(
      IOrganizationInviteService inviteService,
      IOrganizationService organizationService,
      UserManager<User> userManager)
  {
    this.inviteService = inviteService;
    this.organizationService = organizationService;
    this.userManager = userManager;
  }

  [HttpGet]
  public async Task<IActionResult> Settings()
  {
    var user = await this.userManager.GetUserAsync(this.User).ConfigureAwait(false);
    if (user == null) return this.Challenge();

    var organization = await this.organizationService.GetOrganizationByIdAsync(user.TenantId).ConfigureAwait(false);
    if (organization == null) return this.NotFound();

    return this.View(organization);
  }

  /// <summary>
  /// Speichert die Governance-Einstellungen (SLA, Quiet Hours).
  /// </summary>
  /// <param name="dto">Die neuen Einstellungen.</param>
  /// <returns>Ein Redirect.</returns>
  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> SaveGovernanceSettings(OrganizationDto dto)
  {
      var user = await this.userManager.GetUserAsync(this.User).ConfigureAwait(false);
      if (user == null) return this.Challenge();

      var upsertDto = new UpsertOrganizationDto(
          dto.Name,
          dto.SubscriptionLevel,
          dto.IsActive,
          dto.SlaCheckIntervalMinutes,
          dto.QuietHoursStart,
          dto.QuietHoursEnd,
          dto.TimeZoneId,
          dto.NotifyOnLow,
          dto.NotifyOnMedium,
          dto.NotifyOnHigh,
          dto.NotifyOnBlocker);

      await this.organizationService.UpdateOrganizationAsync(user.TenantId, upsertDto).ConfigureAwait(false);
      this.TempData["StatusMessage"] = "Governance-Einstellungen erfolgreich gespeichert.";

      return this.RedirectToAction(nameof(this.Settings));
  }

  /// <summary>
  /// Zeigt das Governance-Protokoll (Audit Logs) an.
  /// </summary>
  /// <returns>Die GovernanceLog-View.</returns>
  [HttpGet]
  public async Task<IActionResult> GovernanceLog()
  {
      var user = await this.userManager.GetUserAsync(this.User).ConfigureAwait(false);
      if (user == null) return this.Challenge();

      var logs = await this.organizationService.GetAuditLogsAsync(user.TenantId).ConfigureAwait(false);
      return this.View(logs);
  }

  /// <summary>
  /// Erstellt einen neuen Einladungs-Token.
  /// </summary>
  /// <param name="targetedEmail">Optionale E-Mail.</param>
  /// <returns>Ein Redirect.</returns>
  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> CreateInvite(string? targetedEmail)
  {
    var user = await this.userManager.GetUserAsync(this.User).ConfigureAwait(false);
    if (user == null) return this.Challenge();

    await this.inviteService.CreateInviteAsync(user.TenantId, targetedEmail).ConfigureAwait(false);
    this.TempData["StatusMessage"] = "Einladungs-Link erfolgreich generiert.";
    
    return this.RedirectToAction(nameof(this.Settings));
  }

  /// <summary>
  /// Entfernt (hier: deaktiviert) den SLA-Schwellenwert für alle Profile (Legacy-Check, jetzt via Organization-Settings).
  /// </summary>
  /// <returns>Ein Redirect.</returns>
  [HttpPost]
  [ValidateAntiForgeryToken]
  public IActionResult UpdateSlaThreshold()
  {
      // Legacy support removal - we use Organization settings now
      return this.RedirectToAction(nameof(this.Settings));
  }
}
