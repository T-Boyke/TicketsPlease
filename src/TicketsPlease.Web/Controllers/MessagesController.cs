// <copyright file="MessagesController.cs" company="BitLC-NE-2025-2026">
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
using TicketsPlease.Application.Common.Dtos;
using TicketsPlease.Application.Common.Interfaces;
using TicketsPlease.Domain.Entities;
using TicketsPlease.Infrastructure.Persistence;

/// <summary>
/// Controller für das Messaging-System (F9).
/// </summary>
[Authorize]
internal sealed class MessagesController : Controller
{
  private readonly IMessageService messageService;
  private readonly UserManager<User> userManager;
  private readonly AppDbContext context;

  /// <summary>
  /// Initializes a new instance of the <see cref="MessagesController"/> class.
  /// </summary>
  /// <param name="messageService">Der Dienst für Nachrichtenoperationen.</param>
  /// <param name="userManager">Die Benutzerverwaltung.</param>
  /// <param name="context">Der Datenbankkontext für Benutzer-Lookups.</param>
  public MessagesController(IMessageService messageService, UserManager<User> userManager, AppDbContext context)
  {
    this.messageService = messageService;
    this.userManager = userManager;
    this.context = context;
  }

  /// <summary>
  /// Zeigt die Nachrichtenübersicht des Benutzers an (F9).
  /// </summary>
  /// <returns>Die Index-View.</returns>
  [HttpGet]
  public async Task<IActionResult> Index()
  {
    var user = await this.userManager.GetUserAsync(this.User).ConfigureAwait(false);
    if (user == null)
    {
      return this.Challenge();
    }

    var messages = await this.messageService.GetUserMessagesAsync(user.Id).ConfigureAwait(false);
    return this.View(messages);
  }

  /// <summary>
  /// Zeigt das Formular zum Erstellen einer Nachricht an.
  /// </summary>
  /// <returns>Die Create-View.</returns>
  [HttpGet]
  public async Task<IActionResult> Create()
  {
    await this.PrepareUserList().ConfigureAwait(false);
    return this.View();
  }

  /// <summary>
  /// Sendet eine neue Nachricht (F9).
  /// </summary>
  /// <param name="dto">Die Nachrichtendaten.</param>
  /// <returns>Ein Umleitungsergebnis.</returns>
  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> Create(CreateMessageDto dto)
  {
    if (!this.ModelState.IsValid)
    {
      await this.PrepareUserList().ConfigureAwait(false);
      return this.View(dto);
    }

    var user = await this.userManager.GetUserAsync(this.User).ConfigureAwait(false);
    if (user == null)
    {
      return this.Challenge();
    }

    await this.messageService.SendMessageAsync(user.Id, dto).ConfigureAwait(false);

    if (this.Request.Headers.XRequestedWith == "XMLHttpRequest" || this.Request.Headers.Accept.ToString().Contains("application/json", StringComparison.Ordinal))
    {
      return this.Ok();
    }

    return this.RedirectToAction(nameof(this.Index));
  }

  /// <summary>
  /// Zeigt die Konversation mit einem bestimmten Benutzer an (F9).
  /// </summary>
  /// <param name="userId">Die ID des Gesprächspartners.</param>
  /// <returns>Die Conversation-View.</returns>
  [HttpGet]
  public async Task<IActionResult> Conversation(Guid userId)
  {
    var currentUser = await this.userManager.GetUserAsync(this.User).ConfigureAwait(false);
    if (currentUser == null)
    {
      return this.Challenge();
    }

    var messages = await this.messageService.GetConversationAsync(currentUser.Id, userId).ConfigureAwait(false);
    var otherUser = await this.userManager.FindByIdAsync(userId.ToString()).ConfigureAwait(false);

    if (otherUser == null)
    {
      return this.NotFound();
    }

    this.ViewBag.OtherUserName = otherUser.UserName;
    this.ViewBag.OtherUserId = otherUser.Id;

    return this.View(messages);
  }

  private async Task PrepareUserList()
  {
    var currentUserId = Guid.Parse(this.userManager.GetUserId(this.User)!);
    var users = await this.context.Users
        .Where(u => u.Id != currentUserId)
        .OrderBy(u => u.UserName)
        .ToListAsync().ConfigureAwait(false);

    this.ViewBag.Users = new SelectList(users, "Id", "UserName");
  }
}
