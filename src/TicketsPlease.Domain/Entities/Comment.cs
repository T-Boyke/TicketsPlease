// <copyright file="Comment.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Domain.Entities;

using System;
using TicketsPlease.Domain.Common;

/// <summary>
/// Repräsentiert einen Kommentar zu einem Ticket (F5).
/// </summary>
public class Comment : BaseAuditableEntity
{
  /// <summary>
  /// Initializes a new instance of the <see cref="Comment"/> class.
  /// </summary>
  /// <param name="content">Der Inhalt des Kommentars.</param>
  /// <param name="ticketId">Die ID des zugehörigen Tickets.</param>
  /// <param name="authorId">Die ID des Erstellers.</param>
  public Comment(string content, Guid ticketId, Guid authorId)
  {
    if (string.IsNullOrWhiteSpace(content))
    {
      throw new ArgumentException("Inhalt darf nicht leer sein.", nameof(content));
    }

    this.Content = content;
    this.TicketId = ticketId;
    this.AuthorId = authorId;
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="Comment"/> class.
  /// Required for EF Core.
  /// </summary>
  private Comment()
  {
  }

  /// <summary>
  /// Gets den Inhalt des Kommentars.
  /// </summary>
  public string Content { get; private set; } = string.Empty;

  /// <summary>
  /// Gets die ID des zugehörigen Tickets.
  /// </summary>
  public Guid TicketId { get; private set; }

  /// <summary>
  /// Gets das zugehörige Ticket.
  /// </summary>
  public Ticket? Ticket { get; }

  /// <summary>
  /// Gets die ID des Erstellers.
  /// </summary>
  public Guid AuthorId { get; private set; }

  /// <summary>
  /// Gets den Ersteller des Kommentars.
  /// </summary>
  public User? Author { get; }

  /// <summary>
  /// Aktualisiert den Inhalt des Kommentars.
  /// </summary>
  /// <param name="content">Der neue Inhalt.</param>
  public void UpdateContent(string content)
  {
    if (string.IsNullOrWhiteSpace(content))
    {
      throw new ArgumentException("Inhalt darf nicht leer sein.", nameof(content));
    }

    this.Content = content;
    this.UpdatedAt = DateTime.UtcNow;
  }

  /// <summary>
  /// Setzt den Mandanten des Kommentars (Multi-Tenancy).
  /// </summary>
  /// <param name="tenantId">Die ID des Mandanten.</param>
  public void SetTenantId(Guid tenantId)
  {
    this.TenantId = tenantId;
  }
}
