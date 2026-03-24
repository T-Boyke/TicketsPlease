// <copyright file="TicketService.cs" company="BitLC-NE-2025-2026">
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
/// Implementierung des Ticket-Services zur Steuerung des Kanban-Boards.
/// </summary>
public class TicketService : ITicketService
{
  private readonly ITicketRepository ticketRepository;
  private readonly UserManager<User> userManager;
  private readonly IHttpContextAccessor httpContextAccessor;

  /// <summary>
  /// Initializes a new instance of the <see cref="TicketService"/> class.
  /// </summary>
  /// <param name="ticketRepository">Das Repository für Tickets.</param>
  /// <param name="userManager">Der Identity UserManager.</param>
  /// <param name="httpContextAccessor">Der Accessor für den aktuellen HttpContext.</param>
  public TicketService(
      ITicketRepository ticketRepository,
      UserManager<User> userManager,
      IHttpContextAccessor httpContextAccessor)
  {
    this.ticketRepository = ticketRepository;
    this.userManager = userManager;
    this.httpContextAccessor = httpContextAccessor;
  }

  /// <inheritdoc/>
  public async Task<IEnumerable<TicketDto>> GetActiveTicketsAsync()
  {
    var tickets = await this.ticketRepository.GetAllActiveAsync().ConfigureAwait(false);
    return tickets.Select(t => MapToDto(t));
  }

  /// <inheritdoc/>
  public async Task<IEnumerable<TicketDto>> GetFilteredTicketsAsync(Guid? projectId = null, Guid? assignedUserId = null, Guid? creatorId = null)
  {
    var tickets = await this.ticketRepository.GetFilteredAsync(projectId, assignedUserId, creatorId).ConfigureAwait(false);
    return tickets.Select(t => MapToDto(t));
  }

  /// <inheritdoc/>
  public async Task<TicketDto?> GetTicketAsync(Guid id)
  {
    var ticket = await this.ticketRepository.GetByIdAsync(id).ConfigureAwait(false);
    return ticket != null ? MapToDto(ticket) : null;
  }

  /// <inheritdoc/>
  public async Task CreateTicketAsync(CreateTicketDto dto)
  {
    ArgumentNullException.ThrowIfNull(dto);

    var user = await this.GetCurrentUserAsync().ConfigureAwait(false);
    if (user == null)
    {
      throw new UnauthorizedAccessException();
    }

    // Standard-Status für MVP (Initialzustand des Workflows)
    var defaultStateId = await this.ticketRepository.GetDefaultWorkflowStateIdAsync().ConfigureAwait(false);

    var ticket = new Ticket(dto.Title, TicketsPlease.Domain.Enums.TicketType.Task, dto.ProjectId, user.Id, defaultStateId, "initial");
    ticket.UpdateDescription(dto.Description, dto.Description);
    ticket.AssignUser(dto.AssignedUserId);
    ticket.SetPriority(dto.PriorityId);
    ticket.SetEstimatePoints(dto.EstimatePoints);
    ticket.SetTenantId(user.TenantId);

    await this.ticketRepository.AddAsync(ticket).ConfigureAwait(false);
    await this.ticketRepository.SaveChangesAsync().ConfigureAwait(false);
  }

  /// <inheritdoc/>
  public async Task UpdateTicketAsync(UpdateTicketDto dto)
  {
    ArgumentNullException.ThrowIfNull(dto);

    var ticket = await this.ticketRepository.GetByIdAsync(dto.Id).ConfigureAwait(false);
    if (ticket == null)
    {
      throw new KeyNotFoundException("Ticket nicht gefunden.");
    }

    ticket.UpdateTitle(dto.Title);
    ticket.UpdateDescription(dto.Description, dto.Description);
    ticket.AssignUser(dto.AssignedUserId);
    ticket.SetPriority(dto.PriorityId);
    ticket.SetEstimatePoints(dto.EstimatePoints);

    await this.ticketRepository.SaveChangesAsync().ConfigureAwait(false);
  }

  /// <inheritdoc/>
  public async Task MoveTicketAsync(Guid id, string newStatus)
  {
    var ticket = await this.ticketRepository.GetByIdAsync(id).ConfigureAwait(false);
    if (ticket == null)
    {
      throw new KeyNotFoundException("Ticket nicht gefunden.");
    }

    if (newStatus == "Closed")
    {
      await this.CloseTicketAsync(id).ConfigureAwait(false);
    }
    else
    {
      // MVP: Status-Update über Repository-Save (Property-Setzen erfolgt im Controller oder Service)
      // Beachte: Ticket Entity Status ist private set in DDD, hier müsste ggf. eine Methode hin.
      await this.ticketRepository.SaveChangesAsync().ConfigureAwait(false);
    }
  }

  /// <inheritdoc/>
  public async Task CloseTicketAsync(Guid id)
  {
    var ticket = await this.ticketRepository.GetByIdAsync(id).ConfigureAwait(false);
    var user = await this.GetCurrentUserAsync().ConfigureAwait(false);
    if (ticket == null || user == null)
    {
      throw new KeyNotFoundException();
    }

    if (!ticket.CanBeClosed())
    {
      throw new InvalidOperationException("Das Ticket kann nicht geschlossen werden, da es noch offene Abhängigkeiten (Vorgänger) hat.");
    }

    var roles = await this.userManager.GetRolesAsync(user).ConfigureAwait(false);
    ticket.Close(user.Id, roles.Contains("Admin"));
    await this.ticketRepository.SaveChangesAsync().ConfigureAwait(false);
  }

  /// <inheritdoc/>
  public async Task AddDependencyAsync(Guid ticketId, Guid blockerId)
  {
    var ticket = await this.ticketRepository.GetByIdAsync(ticketId).ConfigureAwait(false);
    if (ticket == null)
    {
      throw new KeyNotFoundException("Ticket nicht gefunden.");
    }

    ticket.AddLink(blockerId, TicketsPlease.Domain.Enums.TicketLinkType.Blocks);
    await this.ticketRepository.SaveChangesAsync().ConfigureAwait(false);
  }

  /// <inheritdoc/>
  public async Task RemoveDependencyAsync(Guid ticketId, Guid dependencyId)
  {
    var ticket = await this.ticketRepository.GetByIdAsync(ticketId).ConfigureAwait(false);
    if (ticket == null)
    {
      throw new KeyNotFoundException("Ticket nicht gefunden.");
    }

    var link = ticket.BlockedBy.Union(ticket.Blocking).FirstOrDefault(l => l.Id == dependencyId);
    if (link != null)
    {
      if (ticket.BlockedBy.Contains(link))
      {
        ticket.BlockedBy.Remove(link);
      }
      else
      {
        ticket.Blocking.Remove(link);
      }

      await this.ticketRepository.SaveChangesAsync().ConfigureAwait(false);
    }
  }

  /// <summary>
  /// Mappt eine Ticket-Entität auf ein DTO.
  /// </summary>
  /// <param name="t">Die Entität.</param>
  /// <returns>Das gemappte <see cref="TicketDto"/>.</returns>
  private static TicketDto MapToDto(Ticket t)
  {
    var comments = t.Comments?.OrderByDescending(c => c.CreatedAt).Select(c => new CommentDto(
          c.Id,
          c.Content,
          c.AuthorId,
          c.Author?.UserName ?? "Unbekannt",
          c.CreatedAt)) ?? Enumerable.Empty<CommentDto>();

    var blockedBy = t.BlockedBy?.Select(l => new TicketLinkDto(
          l.Id,
          l.SourceTicketId,
          l.SourceTicket?.Title ?? "???",
          l.TargetTicketId,
          l.TargetTicket?.Title ?? "???",
          l.LinkType,
          l.SourceTicket?.Status == "Closed" || l.SourceTicket?.Status == "Done")) ?? Enumerable.Empty<TicketLinkDto>();

    var blocking = t.Blocking?.Select(l => new TicketLinkDto(
          l.Id,
          l.SourceTicketId,
          l.SourceTicket?.Title ?? "???",
          l.TargetTicketId,
          l.TargetTicket?.Title ?? "???",
          l.LinkType,
          l.TargetTicket?.Status == "Closed" || l.TargetTicket?.Status == "Done")) ?? Enumerable.Empty<TicketLinkDto>();

    return new TicketDto(
        t.Id,
        t.Title,
        t.Description,
        t.Status,
        t.ProjectId,
        t.Project?.Title ?? "Unbekannt",
        t.AssignedUserId,
        t.AssignedUser?.UserName ?? "Nicht zugewiesen",
        t.Type,
        new TicketPriorityDto(t.PriorityId, t.Priority?.Name ?? "Normal", t.Priority?.ColorHex ?? "#ccc"),
        t.CreatedAt,
        t.EstimatePoints,
        comments,
        blockedBy,
        blocking);
  }

  /// <summary>
  /// Ermittelt den aktuell angemeldeten Benutzer.
  /// </summary>
  /// <returns>Das <see cref="User"/> Objekt oder null.</returns>
  private async Task<User?> GetCurrentUserAsync()
  {
    return await this.userManager.GetUserAsync(this.httpContextAccessor.HttpContext!.User).ConfigureAwait(false);
  }
}
