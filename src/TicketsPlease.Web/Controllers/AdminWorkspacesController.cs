// <copyright file="AdminWorkspacesController.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Web.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketsPlease.Application.Common.Dtos;
using TicketsPlease.Application.Common.Interfaces;

/// <summary>
/// Controller für die Verwaltung von Workspaces (Organisationen) im Admin-Bereich.
/// </summary>
[Authorize(Roles = "Admin")]
internal sealed class AdminWorkspacesController : Controller
{
    private readonly IOrganizationService organizationService;

    /// <summary>
    /// Initializes a new instance of the <see cref="AdminWorkspacesController"/> class.
    /// </summary>
    /// <param name="organizationService">Der Organisations-Dienst.</param>
    public AdminWorkspacesController(IOrganizationService organizationService)
    {
        this.organizationService = organizationService;
    }

    /// <summary>
    /// Listet alle Workspaces auf.
    /// </summary>
    /// <returns>Die Übersichtsview.</returns>
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var organizations = await this.organizationService.GetOrganizationsAsync().ConfigureAwait(false);
        return this.View(organizations);
    }

    /// <summary>
    /// Zeigt das Formular zum Erstellen eines Workspaces.
    /// </summary>
    /// <returns>Die Create-View.</returns>
    [HttpGet]
    public IActionResult Create()
    {
        return this.View();
    }

    /// <summary>
    /// Verarbeitet die Erstellung eines Workspaces.
    /// </summary>
    /// <param name="dto">Die Daten.</param>
    /// <returns>Redirect oder View mit Fehlern.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(UpsertOrganizationDto dto)
    {
        if (!this.ModelState.IsValid)
        {
            return this.View(dto);
        }

        await this.organizationService.CreateOrganizationAsync(dto).ConfigureAwait(false);
        return this.RedirectToAction(nameof(this.Index));
    }

    /// <summary>
    /// Zeigt das Formular zum Bearbeiten eines Workspaces.
    /// </summary>
    /// <param name="id">Die ID.</param>
    /// <returns>Die Edit-View.</returns>
    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        var org = await this.organizationService.GetOrganizationByIdAsync(id).ConfigureAwait(false);
        if (org == null)
        {
            return this.NotFound();
        }

        var dto = new UpsertOrganizationDto(org.Name, org.SubscriptionLevel, org.IsActive);
        this.ViewData["OrgId"] = id;
        return this.View(dto);
    }

    /// <summary>
    /// Verarbeitet das Update eines Workspaces.
    /// </summary>
    /// <param name="id">Die ID.</param>
    /// <param name="dto">Die Daten.</param>
    /// <returns>Redirect.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, UpsertOrganizationDto dto)
    {
        if (!this.ModelState.IsValid)
        {
            this.ViewData["OrgId"] = id;
            return this.View(dto);
        }

        await this.organizationService.UpdateOrganizationAsync(id, dto).ConfigureAwait(false);
        return this.RedirectToAction(nameof(this.Index));
    }
}
