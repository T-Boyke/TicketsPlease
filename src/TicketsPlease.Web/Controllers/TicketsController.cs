// <copyright file="TicketsController.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Web.Controllers;

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using TicketsPlease.Application.Common.Dtos;
using TicketsPlease.Application.Common.Interfaces;
using TicketsPlease.Domain.Entities;
using TicketsPlease.Infrastructure.Persistence;

/// <summary>
/// Controller für das Ticket-Handling und das Kanban-Board.
/// </summary>
[Authorize]
public sealed class TicketsController : Controller
{
  private readonly ITicketService ticketService;
  private readonly IProjectService projectService;
  private readonly IFileAssetRepository fileAssetRepository;
  private readonly IFileStorageService fileStorageService;
  private readonly ITimeTrackingService timeTrackingService;
  private readonly ISubTicketService subTicketService;
  private readonly ITicketTemplateService templateService;
  private readonly UserManager<User> userManager;
  private readonly AppDbContext context;
  private readonly IStringLocalizer<TicketsController> l;

  /// <summary>
  /// Initializes a new instance of the <see cref="TicketsController"/> class.
  /// </summary>
  /// <param name="ticketService">Der Dienst für Ticketoperationen.</param>
  /// <param name="projectService">Der Dienst für Projektinformationen.</param>
  /// <param name="fileAssetRepository">Das Repository für Dateien.</param>
  /// <param name="fileStorageService">Der Dienst zur Dateispeicherung.</param>
  /// <param name="timeTrackingService">Der Dienst für Zeiterfassung.</param>
  /// <param name="subTicketService">Der Dienst für Untertickets.</param>
  /// <param name="templateService">Der Dienst für Ticket-Vorlagen.</param>
  /// <param name="userManager">Die Benutzerverwaltung.</param>
  /// <param name="context">Der Datenbankkontext für Metadata-Lookups.</param>
  /// <param name="localizer">Der Localizer für UI-Strings.</param>
  [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S107:Methods should not have too many parameters", Justification = "Standard set of dependencies for a primary feature controller.")]
  public TicketsController(
      ITicketService ticketService,
      IProjectService projectService,
      IFileAssetRepository fileAssetRepository,
      IFileStorageService fileStorageService,
      ITimeTrackingService timeTrackingService,
      ISubTicketService subTicketService,
      ITicketTemplateService templateService,
      UserManager<User> userManager,
      AppDbContext context,
      IStringLocalizer<TicketsController> localizer)
  {
    this.ticketService = ticketService;
    this.projectService = projectService;
    this.fileAssetRepository = fileAssetRepository;
    this.fileStorageService = fileStorageService;
    this.timeTrackingService = timeTrackingService;
    this.subTicketService = subTicketService;
    this.templateService = templateService;
    this.userManager = userManager;
    this.context = context;
    this.l = localizer;
  }

  /// <summary>
  /// Zeigt das Kanban-Board an (F3).
  /// </summary>
  /// <param name="projectId">Der Filter für das Projekt (F6).</param>
  /// <param name="assignedUserId">Der Filter für den zugewiesenen Benutzer (F6).</param>
  /// <param name="creatorId">Der Filter für den Ersteller (F6).</param>
  /// <param name="status">Der Status-Filter.</param>
  /// <param name="priorityId">Der Prioritäts-Filter.</param>
  /// <param name="fromDate">Startdatum.</param>
  /// <param name="toDate">Enddatum.</param>
  /// <param name="search">Der Filter für den Suchbegriff.</param>
  /// <param name="tagId">Der Tag-Filter.</param>
  /// <returns>Die Index-View mit allen aktiven Tickets.</returns>
  [HttpGet]
  public async Task<IActionResult> Index(
      Guid? projectId,
      Guid? assignedUserId,
      Guid? creatorId,
      string? status,
      Guid? priorityId,
      DateTime? fromDate,
      DateTime? toDate,
      string? search,
      Guid? tagId)
  {
    var tickets = await this.ticketService.GetFilteredTicketsAsync(
        projectId,
        assignedUserId,
        creatorId,
        status,
        priorityId,
        fromDate,
        toDate,
        search,
        tagId).ConfigureAwait(false);

    await this.PrepareViewBags().ConfigureAwait(false);

    this.ViewData["CurrentProject"] = projectId;
    this.ViewData["CurrentAssignee"] = assignedUserId;
    this.ViewData["CurrentCreator"] = creatorId;
    this.ViewData["CurrentStatus"] = status;
    this.ViewData["CurrentPriority"] = priorityId;
    this.ViewData["CurrentFromDate"] = fromDate;
    this.ViewData["CurrentToDate"] = toDate;
    this.ViewData["CurrentSearch"] = search;
    this.ViewData["CurrentTag"] = tagId;

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
        ticket.EstimatePoints,
        ticket.ChilliesDifficulty,
        ticket.Tags.Select(tt => tt.Id).ToList(),
        ticket.RowVersion);

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
      try
      {
        await this.ticketService.UpdateTicketAsync(dto).ConfigureAwait(false);
        return this.RedirectToAction(nameof(this.Index));
      }
      catch (DbUpdateConcurrencyException)
      {
        this.ModelState.AddModelError(string.Empty, this.l["The data has been modified by another user. Please reload the page."].Value);
      }
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
  /// Lädt einen Anhang für das Ticket hoch.
  /// </summary>
  /// <param name="id">Die ID des Tickets.</param>
  /// <param name="file">Die hochgeladene Datei.</param>
  /// <returns>Ein Umleitungsergebnis.</returns>
  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> UploadAttachment(Guid id, IFormFile file)
  {
    if (file != null && file.Length > 0)
    {
      await this.ticketService.UploadAttachmentAsync(id, file).ConfigureAwait(false);
    }

    return this.RedirectToAction(nameof(this.Details), new { id });
  }

  /// <summary>
  /// Lädt eine Datei herunter.
  /// </summary>
  /// <param name="id">Die ID der Datei.</param>
  /// <returns>Das Dateiergebnis.</returns>
  [HttpGet]
  public async Task<IActionResult> DownloadAttachment(Guid id)
  {
    var asset = await this.fileAssetRepository.GetByIdAsync(id).ConfigureAwait(false);
    if (asset == null)
    {
      return this.NotFound();
    }

    var stream = await this.fileStorageService.GetFileAsync(asset.BlobPath).ConfigureAwait(false);
    return this.File(stream, asset.ContentType, asset.FileName);
  }

  /// <summary>
  /// Startet den Timer für ein Ticket.
  /// </summary>
  /// <param name="id">Die ID des Tickets.</param>
  /// <returns>Ein Umleitungsergebnis.</returns>
  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> StartTimer(Guid id)
  {
    var user = await this.userManager.GetUserAsync(this.User).ConfigureAwait(false);
    if (user != null)
    {
      await this.timeTrackingService.StartTimeTrackingAsync(id, user.Id).ConfigureAwait(false);
    }

    return this.RedirectToAction(nameof(this.Details), new { id });
  }

  /// <summary>
  /// Stoppt den Timer für ein Ticket.
  /// </summary>
  /// <param name="id">Die ID des Tickets.</param>
  /// <returns>Ein Umleitungsergebnis.</returns>
  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> StopTimer(Guid id)
  {
    var user = await this.userManager.GetUserAsync(this.User).ConfigureAwait(false);
    if (user != null)
    {
      await this.timeTrackingService.StopTimeTrackingAsync(id, user.Id).ConfigureAwait(false);
    }

    return this.RedirectToAction(nameof(this.Details), new { id });
  }

  /// <summary>
  /// Fügt ein Unterticket hinzu.
  /// </summary>
  /// <param name="id">Hauptticket-ID.</param>
  /// <param name="title">Titel der Teilaufgabe.</param>
  /// <returns>Ein Umleitungsergebnis.</returns>
  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> AddSubTicket(Guid id, string title)
  {
    if (!string.IsNullOrWhiteSpace(title))
    {
      var user = await this.userManager.GetUserAsync(this.User).ConfigureAwait(false);
      if (user != null)
      {
        await this.subTicketService.AddSubTicketAsync(id, title, user.Id).ConfigureAwait(false);
      }
    }

    return this.RedirectToAction(nameof(this.Details), new { id });
  }

  /// <summary>
  /// Toggelt den Status eines Untertickets.
  /// </summary>
  /// <param name="id">Hauptticket-ID (für Redirect).</param>
  /// <param name="subTicketId">ID des Untertickets.</param>
  /// <returns>Ein Umleitungsergebnis.</returns>
  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> ToggleSubTicket(Guid id, Guid subTicketId)
  {
    await this.subTicketService.ToggleSubTicketAsync(subTicketId).ConfigureAwait(false);
    return this.RedirectToAction(nameof(this.Details), new { id });
  }

  /// <summary>
  /// Löscht ein Unterticket.
  /// </summary>
  /// <param name="id">Hauptticket-ID (für Redirect).</param>
  /// <param name="subTicketId">ID des Untertickets.</param>
  /// <returns>Ein Umleitungsergebnis.</returns>
  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> DeleteSubTicket(Guid id, Guid subTicketId)
  {
    await this.subTicketService.DeleteSubTicketAsync(subTicketId).ConfigureAwait(false);
    return this.RedirectToAction(nameof(this.Details), new { id });
  }

  /// <summary>
  /// Toggelt den Upvote eines Benutzers für ein Ticket.
  /// </summary>
  /// <param name="id">Die ID des Tickets.</param>
  /// <param name="returnUrl">Optionale URL zum Zurückkehren.</param>
  /// <returns>Ein Umleitungsergebnis.</returns>
  [HttpPost]
  [ValidateAntiForgeryToken]
  [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1054:URI-Parameter dürfen keine Zeichenfolgen sein", Justification = "ReturnURL follows internal redirect pattern correctly as string.")]
  public async Task<IActionResult> ToggleUpvote(Guid id, string? returnUrl = null)
  {
    var ticket = await this.ticketService.GetTicketAsync(id).ConfigureAwait(false);
    if (ticket == null)
    {
      return this.NotFound();
    }

    if (ticket.UserHasUpvoted)
    {
      await this.ticketService.DownvoteAsync(id).ConfigureAwait(false);
    }
    else
    {
      await this.ticketService.UpvoteAsync(id).ConfigureAwait(false);
    }

    if (!string.IsNullOrEmpty(returnUrl))
    {
      return this.LocalRedirect(returnUrl);
    }

    return this.RedirectToAction(nameof(this.Details), new { id });
  }

  /// <summary>
  /// Erstellt ein neues Tag.
  /// </summary>
  /// <param name="request">Tag-Anfrage.</param>
  /// <returns>Die JSON-Antwort.</returns>
  [HttpPost]
  [Authorize]
  public async Task<IActionResult> CreateTag([FromBody] CreateTagRequest request)
  {
      if (request == null || string.IsNullOrWhiteSpace(request.Name))
      {
          return this.BadRequest("Name is required");
      }

      var tag = new Tag
      {
          Id = Guid.NewGuid(),
          Name = request.Name,
          ColorHex = request.Color ?? "#64748b",
          Icon = request.Icon ?? "fa-tag"
      };

      this.context.Tags.Add(tag);
      await this.context.SaveChangesAsync().ConfigureAwait(false);

      return this.Json(new { id = tag.Id, name = tag.Name, color = tag.ColorHex, icon = tag.Icon });
  }

  /// <summary>
  /// Request DTO für Tag-Erstellung.
  /// </summary>
  public class CreateTagRequest
  {
      /// <summary>Gets or sets Name.</summary>
      public string Name { get; set; } = string.Empty;

      /// <summary>Gets or sets Color.</summary>
      public string? Color { get; set; }

      /// <summary>Gets or sets Icon.</summary>
      public string? Icon { get; set; }
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

    var tags = await this.ticketService.GetAllTagsAsync().ConfigureAwait(false);
    this.ViewBag.Tags = new MultiSelectList(tags, "Id", "Name");

    var templates = await this.templateService.GetAllTemplatesAsync().ConfigureAwait(false);
    this.ViewBag.Templates = templates;
  }
}
