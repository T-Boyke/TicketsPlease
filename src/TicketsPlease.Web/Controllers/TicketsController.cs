// <copyright file="TicketsController.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Web.Controllers;

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TicketsPlease.Application.Common.Dtos;
using TicketsPlease.Application.Common.Interfaces;
using TicketsPlease.Infrastructure.Persistence;

/// <summary>
/// Controller für das Ticket-Handling und das Kanban-Board.
/// </summary>
[Authorize]
public class TicketsController : Controller
{
  private readonly ITicketService ticketService;
  private readonly IProjectService projectService;
  private readonly AppDbContext context;

  /// <summary>
  /// Initializes a new instance of the <see cref="TicketsController"/> class.
  /// </summary>
  /// <param name="ticketService">Der Dienst für Ticketoperationen.</param>
  /// <param name="projectService">Der Dienst für Projektinformationen.</param>
  /// <param name="context">Der Datenbankkontext für Metadata-Lookups.</param>
  public TicketsController(ITicketService ticketService, IProjectService projectService, AppDbContext context)
  {
    this.ticketService = ticketService;
    this.projectService = projectService;
    this.context = context;
  }

  /// <summary>
  /// Zeigt das Kanban-Board an (F3).
  /// </summary>
  /// <param name="projectId">Der Filter für das Projekt (F6).</param>
  /// <param name="assignedUserId">Der Filter für den zugewiesenen Benutzer (F6).</param>
  /// <param name="creatorId">Der Filter für den Ersteller (F6).</param>
  /// <returns>Die Index-View mit allen aktiven Tickets.</returns>
  [HttpGet]
  public async Task<IActionResult> Index(Guid? projectId, Guid? assignedUserId, Guid? creatorId)
  {
    var tickets = await this.ticketService.GetFilteredTicketsAsync(projectId, assignedUserId, creatorId).ConfigureAwait(false);

    await this.PrepareViewBags().ConfigureAwait(false);

    this.ViewData["CurrentProject"] = projectId;
    this.ViewData["CurrentAssignee"] = assignedUserId;
    this.ViewData["CurrentCreator"] = creatorId;

    return this.View(tickets);
  }

  /// <summary>
  /// Zeigt detaillierte Informationen zu einem Ticket an.
  /// </summary>
  /// <param name="id">Die ID des Tickets.</param>
  /// <returns>Die Details-View.</returns>
  [HttpGet]
  public async Task<IActionResult> Details(Guid id)
  {
    var ticket = await this.ticketService.GetTicketAsync(id).ConfigureAwait(false);
    if (ticket == null)
    {
      return this.NotFound();
    }

    return this.View(ticket);
  }

  /// <summary>
  /// Zeigt das Formular zum Erstellen eines neuen Tickets an.
  /// </summary>
  /// <returns>Die Create-View.</returns>
  [HttpGet]
  public async Task<IActionResult> Create()
  {
    await this.PrepareViewBags().ConfigureAwait(false);
    return this.View();
  }

  /// <summary>
  /// Verarbeitet die Erstellung eines neuen Tickets.
  /// </summary>
  /// <param name="dto">Die Ticketdaten.</param>
  /// <returns>Ein Redirect auf das Board bei Erfolg.</returns>
  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> Create(CreateTicketDto dto)
  {
    if (this.ModelState.IsValid)
    {
      await this.ticketService.CreateTicketAsync(dto).ConfigureAwait(false);
      return this.RedirectToAction(nameof(this.Index));
    }

    await this.PrepareViewBags().ConfigureAwait(false);
    return this.View(dto);
  }

  /// <summary>
  /// Zeigt das Formular zum Bearbeiten eines Tickets an.
  /// </summary>
  /// <param name="id">Die ID des Tickets.</param>
  /// <returns>Die Edit-View mit den Ticketdaten.</returns>
  [HttpGet]
  public async Task<IActionResult> Edit(Guid id)
  {
    var ticket = await this.ticketService.GetTicketAsync(id).ConfigureAwait(false);
    if (ticket == null)
    {
      return this.NotFound();
    }

    var dto = new UpdateTicketDto(
        ticket.Id,
        ticket.Title,
        ticket.Description,
        ticket.Status,
        ticket.Priority.Id,
        ticket.AssignedUserId,
        ticket.EstimatePoints);

    await this.PrepareViewBags().ConfigureAwait(false);
    return this.View(dto);
  }

  /// <summary>
  /// Verarbeitet die Aktualisierung eines Tickets.
  /// </summary>
  /// <param name="dto">Die aktualisierten Ticketdaten.</param>
  /// <returns>Ein Redirect auf das Board bei Erfolg.</returns>
  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> Edit(UpdateTicketDto dto)
  {
    if (this.ModelState.IsValid)
    {
      await this.ticketService.UpdateTicketAsync(dto).ConfigureAwait(false);
      return this.RedirectToAction(nameof(this.Index));
    }

    await this.PrepareViewBags().ConfigureAwait(false);
    return this.View(dto);
  }

  /// <summary>
  /// Verschiebt ein Ticket in einen neuen Status (via AJAX).
  /// </summary>
  /// <param name="id">Die ID des Tickets.</param>
  /// <param name="status">Der Zielstatus.</param>
  /// <returns>Ein OK-Ergebnis.</returns>
  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> Move(Guid id, string status)
  {
    await this.ticketService.MoveTicketAsync(id, status).ConfigureAwait(false);
    return this.Ok();
  }

  /// <summary>
  /// Fügt eine Abhängigkeit hinzu (F7).
  /// </summary>
  /// <param name="id">Die ID des blockierten Tickets.</param>
  /// <param name="blockerId">Die ID des blockierenden Tickets.</param>
  /// <returns>Ein Umleitungsergebnis.</returns>
  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> AddDependency(Guid id, Guid blockerId)
  {
    try
    {
      await this.ticketService.AddDependencyAsync(id, blockerId).ConfigureAwait(false);
      return this.RedirectToAction(nameof(this.Details), new { id });
    }
    catch (InvalidOperationException ex)
    {
      this.TempData["ErrorMessage"] = ex.Message;
      return this.RedirectToAction(nameof(this.Details), new { id });
    }
  }

  /// <summary>
  /// Entfernt eine Abhängigkeit (F7).
  /// </summary>
  /// <param name="id">Die ID des Tickets.</param>
  /// <param name="dependencyId">Die ID der Verknüpfung.</param>
  /// <returns>Ein Umleitungsergebnis.</returns>
  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> RemoveDependency(Guid id, Guid dependencyId)
  {
    await this.ticketService.RemoveDependencyAsync(id, dependencyId).ConfigureAwait(false);
    return this.RedirectToAction(nameof(this.Details), new { id });
  }

  /// <summary>
  /// Bereitet die Dropdown-Listen für die Views vor.
  /// </summary>
  /// <returns>Ein Task für die asynchrone Operation.</returns>
  private async Task PrepareViewBags()
  {
    var projects = await this.projectService.GetProjectsAsync().ConfigureAwait(false);
    this.ViewBag.Projects = new SelectList(projects, "Id", "Title");

    var users = await this.context.Users.OrderBy(u => u.UserName).ToListAsync().ConfigureAwait(false);
    this.ViewBag.Users = new SelectList(users, "Id", "UserName");

    var priorities = await this.context.TicketPriorities.OrderByDescending(p => p.LevelWeight).ToListAsync().ConfigureAwait(false);
    this.ViewBag.Priorities = new SelectList(priorities, "Id", "Name");

    var allTickets = await this.context.Tickets.Where(t => !t.IsDeleted).OrderBy(t => t.Title).ToListAsync().ConfigureAwait(false);
    this.ViewBag.AllTickets = new SelectList(allTickets, "Id", "Title");
  }
}
