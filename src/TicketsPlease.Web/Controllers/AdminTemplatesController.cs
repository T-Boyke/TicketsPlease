// <copyright file="AdminTemplatesController.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Web.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TicketsPlease.Application.Common.Dtos;
using TicketsPlease.Application.Common.Interfaces;
using TicketsPlease.Domain.Entities;

/// <summary>
/// Controller für die Verwaltung von Ticket-Vorlagen im Admin-Bereich.
/// </summary>
[Authorize(Roles = "Admin")]
internal sealed class AdminTemplatesController : Controller
{
  private readonly ITicketTemplateService templateService;
  private readonly ITicketRepository ticketRepository;
  private readonly UserManager<User> userManager;

  /// <summary>
  /// Initializes a new instance of the <see cref="AdminTemplatesController"/> class.
  /// </summary>
  /// <param name="templateService">Der Vorlagen-Dienst.</param>
  /// <param name="ticketRepository">Das Ticket-Repository (für Prioritäten).</param>
  /// <param name="userManager">Die Benutzerverwaltung.</param>
  public AdminTemplatesController(
      ITicketTemplateService templateService,
      ITicketRepository ticketRepository,
      UserManager<User> userManager)
  {
    this.templateService = templateService;
    this.ticketRepository = ticketRepository;
    this.userManager = userManager;
  }

  /// <summary>
  /// Listet alle Vorlagen auf.
  /// </summary>
  /// <returns>Die Übersichtsview.</returns>
  [HttpGet]
  public async Task<IActionResult> Index()
  {
    var templates = await this.templateService.GetAllTemplatesAsync().ConfigureAwait(false);
    return this.View(templates);
  }

  /// <summary>
  /// Zeigt das Formular zum Erstellen einer Vorlage.
  /// </summary>
  /// <returns>Die Create-View.</returns>
  [HttpGet]
  public async Task<IActionResult> Create()
  {
    await this.PreparePrioritiesViewBag().ConfigureAwait(false);
    return this.View();
  }

  /// <summary>
  /// Verarbeitet die Erstellung einer Vorlage.
  /// </summary>
  /// <param name="dto">Die Formulardaten.</param>
  /// <returns>Redirect oder View mit Fehlern.</returns>
  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> Create(CreateTicketTemplateDto dto)
  {
    if (!this.ModelState.IsValid)
    {
      await this.PreparePrioritiesViewBag().ConfigureAwait(false);
      return this.View(dto);
    }

    var user = await this.userManager.GetUserAsync(this.User).ConfigureAwait(false);
    await this.templateService.CreateTemplateAsync(user!.Id, dto).ConfigureAwait(false);

    return this.RedirectToAction(nameof(this.Index));
  }

  /// <summary>
  /// Löscht eine Vorlage.
  /// </summary>
  /// <param name="id">Die ID.</param>
  /// <returns>Redirect.</returns>
  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> Delete(Guid id)
  {
    await this.templateService.DeleteTemplateAsync(id).ConfigureAwait(false);
    return this.RedirectToAction(nameof(this.Index));
  }

  private async Task PreparePrioritiesViewBag()
  {
    var priorities = await this.ticketRepository.GetPrioritiesAsync().ConfigureAwait(false);
    this.ViewBag.Priorities = new SelectList(priorities, "Id", "Name");
  }
}
