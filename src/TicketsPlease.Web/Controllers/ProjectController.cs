// <copyright file="ProjectController.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Web.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketsPlease.Application.Common.Dtos;
using TicketsPlease.Application.Common.Interfaces;

/// <summary>
/// Controller für die Projektverwaltung (CRUD für Admins).
/// </summary>
[Authorize(Roles = "Admin")]
public class ProjectController : Controller
{
    private readonly IProjectService _projectService;

    /// <summary>
    /// Initialisiert eine neue Instanz der <see cref="ProjectController"/> Klasse.
    /// </summary>
    /// <param name="projectService">Der Dienst für Projektoperationen.</param>
    public ProjectController(IProjectService projectService)
    {
        _projectService = projectService;
    }

    /// <summary>
    /// Zeigt die Liste aller Projekte an.
    /// </summary>
    /// <returns>Die Index-View mit einer Liste von Projekten.</returns>
    public async Task<IActionResult> Index()
    {
        var projects = await _projectService.GetProjectsAsync();
        return View(projects);
    }

    /// <summary>
    /// Zeigt das Formular zum Erstellen eines neuen Projekts an.
    /// </summary>
    /// <returns>Die Create-View.</returns>
    public IActionResult Create()
    {
        return View();
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
        if (ModelState.IsValid)
        {
            await _projectService.CreateProjectAsync(dto);
            return RedirectToAction(nameof(Index));
        }
        return View(dto);
    }

    /// <summary>
    /// Zeigt das Formular zum Bearbeiten eines bestehenden Projekts an.
    /// </summary>
    /// <param name="id">Die ID des Projekts.</param>
    /// <returns>Die Edit-View mit den Projektdaten.</returns>
    public async Task<IActionResult> Edit(Guid id)
    {
        var project = await _projectService.GetProjectAsync(id);
        if (project == null) return NotFound();

        var dto = new UpdateProjectDto(project.Id, project.Title, project.Description, project.StartDate, project.EndDate, project.IsOpen);
        return View(dto);
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
        if (ModelState.IsValid)
        {
            await _projectService.UpdateProjectAsync(dto);
            return RedirectToAction(nameof(Index));
        }
        return View(dto);
    }

    /// <summary>
    /// Zeigt die Details eines Projekts an.
    /// </summary>
    /// <param name="id">Die ID des Projekts.</param>
    /// <returns>Die Details-View.</returns>
    public async Task<IActionResult> Details(Guid id)
    {
        var project = await _projectService.GetProjectAsync(id);
        if (project == null) return NotFound();
        return View(project);
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
        await _projectService.DeleteProjectAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
