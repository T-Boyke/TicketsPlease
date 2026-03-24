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
    private readonly ITicketService _ticketService;
    private readonly IProjectService _projectService;
    private readonly AppDbContext _context;

    /// <summary>
    /// Initialisiert eine neue Instanz der <see cref="TicketsController"/> Klasse.
    /// </summary>
    /// <param name="ticketService">Der Dienst für Ticketoperationen.</param>
    /// <param name="projectService">Der Dienst für Projektinformationen.</param>
    /// <param name="context">Der Datenbankkontext für Metadata-Lookups.</param>
    public TicketsController(ITicketService ticketService, IProjectService projectService, AppDbContext context)
    {
        _ticketService = ticketService;
        _projectService = projectService;
        _context = context;
    }

    /// <summary>
    /// Zeigt das Kanban-Board an.
    /// </summary>
    /// <returns>Die Index-View mit allen aktiven Tickets.</returns>
    public async Task<IActionResult> Index()
    {
        var tickets = await _ticketService.GetActiveTicketsAsync();
        return View(tickets);
    }

    /// <summary>
    /// Zeigt detaillierte Informationen zu einem Ticket an.
    /// </summary>
    /// <param name="id">Die ID des Tickets.</param>
    /// <returns>Die Details-View.</returns>
    public async Task<IActionResult> Details(Guid id)
    {
        var ticket = await _ticketService.GetTicketAsync(id);
        if (ticket == null) return NotFound();
        return View(ticket);
    }

    /// <summary>
    /// Zeigt das Formular zum Erstellen eines neuen Tickets an.
    /// </summary>
    /// <returns>Die Create-View.</returns>
    public async Task<IActionResult> Create()
    {
        await PrepareViewBags();
        return View();
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
        if (ModelState.IsValid)
        {
            await _ticketService.CreateTicketAsync(dto);
            return RedirectToAction(nameof(Index));
        }
        await PrepareViewBags();
        return View(dto);
    }

    /// <summary>
    /// Zeigt das Formular zum Bearbeiten eines Tickets an.
    /// </summary>
    /// <param name="id">Die ID des Tickets.</param>
    /// <returns>Die Edit-View mit den Ticketdaten.</returns>
    public async Task<IActionResult> Edit(Guid id)
    {
        var ticket = await _ticketService.GetTicketAsync(id);
        if (ticket == null) return NotFound();

        var dto = new UpdateTicketDto(
            ticket.Id,
            ticket.Title,
            ticket.Description,
            ticket.Status,
            ticket.Priority.Id,
            ticket.AssignedUserId,
            ticket.EstimatePoints);

        await PrepareViewBags();
        return View(dto);
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
        if (ModelState.IsValid)
        {
            await _ticketService.UpdateTicketAsync(dto);
            return RedirectToAction(nameof(Index));
        }
        await PrepareViewBags();
        return View(dto);
    }

    /// <summary>
    /// Verschiebt ein Ticket in einen neuen Status (via AJAX).
    /// </summary>
    /// <param name="id">Die ID des Tickets.</param>
    /// <param name="status">Der Zielstatus.</param>
    /// <returns>Ein OK-Ergebnis.</returns>
    [HttpPost]
    public async Task<IActionResult> Move(Guid id, string status)
    {
        await _ticketService.MoveTicketAsync(id, status);
        return Ok();
    }

    /// <summary>
    /// Bereitet die Dropdown-Listen für die Views vor.
    /// </summary>
    /// <returns>Ein Task für die asynchrone Operation.</returns>
    private async Task PrepareViewBags()
    {
        var projects = await _projectService.GetProjectsAsync();
        ViewBag.Projects = new SelectList(projects, "Id", "Title");

        var users = await _context.Users.OrderBy(u => u.UserName).ToListAsync();
        ViewBag.Users = new SelectList(users, "Id", "UserName");

        var priorities = await _context.TicketPriorities.OrderByDescending(p => p.LevelWeight).ToListAsync();
        ViewBag.Priorities = new SelectList(priorities, "Id", "Name");
    }
}
