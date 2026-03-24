// <copyright file="CommentService.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Application.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using TicketsPlease.Application.Common.Dtos;
using TicketsPlease.Application.Common.Interfaces;
using TicketsPlease.Domain.Entities;

/// <summary>
/// Implementiert die Geschäftslogik für Kommentare (F5).
/// </summary>
public class CommentService : ICommentService
{
  private readonly ICommentRepository commentRepository;
  private readonly UserManager<User> userManager;
  private readonly IHttpContextAccessor httpContextAccessor;

  /// <summary>
  /// Initializes a new instance of the <see cref="CommentService"/> class.
  /// </summary>
  /// <param name="commentRepository">Das Repository für Kommentare.</param>
  /// <param name="userManager">Der Identity UserManager.</param>
  /// <param name="httpContextAccessor">Der Accessor für den aktuellen HttpContext.</param>
  public CommentService(
      ICommentRepository commentRepository,
      UserManager<User> userManager,
      IHttpContextAccessor httpContextAccessor)
  {
    this.commentRepository = commentRepository;
    this.userManager = userManager;
    this.httpContextAccessor = httpContextAccessor;
  }

  /// <inheritdoc/>
  public async Task<IEnumerable<CommentDto>> GetCommentsForTicketAsync(Guid ticketId)
  {
    var comments = await this.commentRepository.GetByTicketIdAsync(ticketId).ConfigureAwait(false);
    return comments.Select(c => new CommentDto(
        c.Id,
        c.Content,
        c.AuthorId,
        c.Author?.UserName ?? "Unbekannt",
        c.CreatedAt));
  }

  /// <inheritdoc/>
  public async Task CreateCommentAsync(CreateCommentDto dto)
  {
    ArgumentNullException.ThrowIfNull(dto);

    var user = await this.GetCurrentUserAsync().ConfigureAwait(false);
    if (user == null)
    {
      throw new UnauthorizedAccessException();
    }

    var comment = new Comment(dto.Content, dto.TicketId, user.Id);
    comment.SetTenantId(user.TenantId);

    await this.commentRepository.AddAsync(comment).ConfigureAwait(false);
    await this.commentRepository.SaveChangesAsync().ConfigureAwait(false);
  }

  private async Task<User?> GetCurrentUserAsync()
  {
    var userPrincipal = this.httpContextAccessor.HttpContext?.User;
    if (userPrincipal == null)
    {
      return null;
    }

    return await this.userManager.GetUserAsync(userPrincipal).ConfigureAwait(false);
  }
}
