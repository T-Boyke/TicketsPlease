// <copyright file="CommentRepository.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Infrastructure.Repositories;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TicketsPlease.Application.Common.Interfaces;
using TicketsPlease.Domain.Entities;
using TicketsPlease.Infrastructure.Persistence;

/// <summary>
/// Implementiert den Datenzugriff für Kommentare unter Verwendung von Entity Framework Core.
/// </summary>
public class CommentRepository : ICommentRepository
{
  private readonly AppDbContext context;

  /// <summary>
  /// Initializes a new instance of the <see cref="CommentRepository"/> class.
  /// </summary>
  /// <param name="context">Der injizierte Datenbankkontext.</param>
  public CommentRepository(AppDbContext context)
  {
    this.context = context;
  }

  /// <inheritdoc />
  public async Task<List<Comment>> GetByTicketIdAsync(Guid ticketId, CancellationToken ct = default)
  {
    return await this.context.Comments
        .IgnoreQueryFilters()
        .AsNoTracking()
        .Include(c => c.Author)
        .Where(c => c.TicketId == ticketId)
        .OrderByDescending(c => c.CreatedAt)
        .ToListAsync(ct).ConfigureAwait(false);
  }

  /// <inheritdoc />
  public async Task AddAsync(Comment comment, CancellationToken ct = default)
  {
    await this.context.Comments.AddAsync(comment, ct).ConfigureAwait(false);
  }

  /// <inheritdoc />
  public async Task<int> SaveChangesAsync(CancellationToken ct = default)
  {
    return await this.context.SaveChangesAsync(ct).ConfigureAwait(false);
  }
}
