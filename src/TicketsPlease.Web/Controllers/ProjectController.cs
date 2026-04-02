// <copyright file="ProjectController.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Web.Controllers;

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketsPlease.Application.Common.Dtos;
using TicketsPlease.Application.Common.Interfaces;

/// <summary>
/// Controller für die Projektverwaltung (CRUD für Admins).
/// </summary>
[Authorize(Roles = "Admin")]
internal sealed class ProjectController : Controller
{
  private readonly IProjectService projectService;
  private readonly ITicketService ticketService;

  /// <summary>
  /// Initializes a new instance of the <see cref="ProjectController"/> class.
  /// </summary>
  /// <param name="projectService">Der Dienst für Projektoperationen.</param>
  /// <param name="ticketService">Der Dienst für Ticketoperationen.</param>
  public ProjectController(IProjectService projectService, ITicketService ticketService)
  {
    this.projectService = projectService;
    this.ticketService = ticketService;
  }

  /// <summary>
  /// Zeigt die Liste aller Projekte an.
  /// </summary>
  /// <returns>Die Index-View mit einer Liste von Projekten.</returns>
  [HttpGet]
  public async Task<IActionResult> Index()
  {
    var projects = await this.projectService.GetProjectsAsync().ConfigureAwait(false);
    return this.View(projects);
  }

  /// <summary>
  /// Zeigt das Formular zum Erstellen eines neuen Projekts an.
  /// </summary>
  /// <returns>Die Create-View.</returns>
  [HttpGet]
  public IActionResult Create()
  {
    return this.View();
  }

  /// <summary>
  /// Verarbeitet die Erstellung eines neuen Projekts.
  /// </summary>
  /// <param name="dto">Die Projektdaten.</param>
  /// <returns>Ein Redirect auf die Index-Seite bei Erfolg.</returns>
  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> Create(CreateProjectDto dto)
  {
    if (this.ModelState.IsValid)
    {
      await this.projectService.CreateProjectAsync(dto).ConfigureAwait(false);
      return this.RedirectToAction(nameof(this.Index));
    }

    return this.View(dto);
  }

  /// <summary>
  /// Zeigt das Formular zum Bearbeiten eines bestehenden Projekts an.
  /// </summary>
  /// <param name="id">Die ID des Projekts.</param>
  /// <returns>Die Edit-View mit den Projektdaten.</returns>
  [HttpGet]
  public async Task<IActionResult> Edit(Guid id)
  {
    var project = await this.projectService.GetProjectAsync(id).ConfigureAwait(false);
    if (project == null)
    {
      return this.NotFound();
    }

    var dto = new UpdateProjectDto(project.Id, project.Title, project.Description, project.StartDate, project.EndDate, project.IsOpen);
    return this.View(dto);
  }

  /// <summary>
  /// Verarbeitet die Aktualisierung eines Projekts.
  /// </summary>
  /// <param name="dto">Die aktualisierten Projektdaten.</param>
  /// <returns>Ein Redirect auf die Index-Seite bei Erfolg.</returns>
  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> Edit(UpdateProjectDto dto)
  {
    if (this.ModelState.IsValid)
    {
      await this.projectService.UpdateProjectAsync(dto).ConfigureAwait(false);
      return this.RedirectToAction(nameof(this.Index));
    }

    return this.View(dto);
  }

  /// <summary>
  /// Zeigt die Details eines Projekts an.
  /// </summary>
  /// <param name="id">Die ID des Projekts.</param>
  /// <returns>Die Details-View.</returns>
  [HttpGet]
  public async Task<IActionResult> Details(Guid id)
  {
    var project = await this.projectService.GetProjectAsync(id).ConfigureAwait(false);
    if (project == null)
    {
      return this.NotFound();
    }

    var tickets = await this.ticketService.GetFilteredTicketsAsync(projectId: id).ConfigureAwait(false);
    this.ViewBag.Tickets = tickets;

    return this.View(project);
  }

  /// <summary>
  /// Löscht ein Projekt.
  /// </summary>
  /// <param name="id">Die ID des zu löschenden Projekts.</param>
  /// <returns>Ein Redirect auf die Index-Seite.</returns>
  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> Delete(Guid id)
  {
    await this.projectService.DeleteProjectAsync(id).ConfigureAwait(false);
    return this.RedirectToAction(nameof(this.Index));
  }
}
