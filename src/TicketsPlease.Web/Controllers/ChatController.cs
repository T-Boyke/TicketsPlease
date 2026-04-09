// <copyright file="ChatController.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Web.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TicketsPlease.Application.Common.Dtos;
using TicketsPlease.Application.Common.Interfaces;
using TicketsPlease.Domain.Entities;

/// <summary>
/// Controller für die Chat-Funktionalität (HQ &amp; Team).
/// </summary>
[Authorize]
public class ChatController : Controller
{
  private readonly IMessageService messageService;
  private readonly UserManager<User> userManager;

  /// <summary>
  /// Initializes a new instance of the <see cref="ChatController"/> class.
  /// </summary>
  /// <param name="messageService">Der Dienst für Nachrichten.</param>
  /// <param name="userManager">Die Benutzerverwaltung.</param>
  public ChatController(IMessageService messageService, UserManager<User> userManager)
  {
    this.messageService = messageService;
    this.userManager = userManager;
  }

  /// <summary>
  /// Ruft die globalen (HQ) Nachrichten ab.
  /// </summary>
  /// <returns>JSON-Liste der Nachrichten.</returns>
  [HttpGet]
  public async Task<IActionResult> GetGlobalHistory()
  {
    var messages = await this.messageService.GetGlobalMessagesAsync().ConfigureAwait(false);
    return this.Ok(messages);
  }

  /// <summary>
  /// Ruft die Team-Nachrichten ab.
  /// </summary>
  /// <param name="teamId">Die Team-ID.</param>
  /// <returns>JSON-Liste der Nachrichten.</returns>
  [HttpGet]
  public async Task<IActionResult> GetTeamHistory(Guid teamId)
  {
    var messages = await this.messageService.GetTeamMessagesAsync(teamId).ConfigureAwait(false);
    return this.Ok(messages);
  }

  /// <summary>
  /// Sendet eine Nachricht.
  /// </summary>
  /// <param name="dto">Das Nachrichten-DTO.</param>
  /// <returns>Das erstellte Nachrichten-DTO.</returns>
  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> SendMessage([FromForm] CreateMessageDto dto)
  {
    var user = await this.userManager.GetUserAsync(this.User).ConfigureAwait(false);
    if (user == null)
    {
      return this.Unauthorized();
    }

    var result = await this.messageService.SendMessageAsync(user.Id, dto).ConfigureAwait(false);
    return this.Ok(result);
  }
}
