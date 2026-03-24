// <copyright file="CommentController.cs" company="BitLC-NE-2025-2026">
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
/// Controller für die Kommentarverwaltung (F5).
/// </summary>
[Authorize]
public class CommentController : Controller
{
  private readonly ICommentService commentService;

  /// <summary>
  /// Initializes a new instance of the <see cref="CommentController"/> class.
  /// </summary>
  /// <param name="commentService">Der Dienst für Kommentare.</param>
  public CommentController(ICommentService commentService)
  {
    this.commentService = commentService;
  }

  /// <summary>
  /// Erstellt einen neuen Kommentar.
  /// </summary>
  /// <param name="dto">Die Kommentardaten.</param>
  /// <returns>Ein Redirect auf die Ticketdetails.</returns>
  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> Create(CreateCommentDto dto)
  {
    ArgumentNullException.ThrowIfNull(dto);

    if (this.ModelState.IsValid)
    {
      try
      {
        await this.commentService.CreateCommentAsync(dto).ConfigureAwait(false);
        return this.RedirectToAction("Details", "Tickets", new { id = dto.TicketId });
      }
      catch (InvalidOperationException ex)
      {
        this.ModelState.AddModelError(string.Empty, ex.Message);
      }
    }

    return this.RedirectToAction("Details", "Tickets", new { id = dto.TicketId });
  }
}
