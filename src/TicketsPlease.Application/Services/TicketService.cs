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
    private readonly ITicketRepository _ticketRepository;
    private readonly UserManager<User> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;

    /// <summary>
    /// Initialisiert eine neue Instanz der <see cref="TicketService"/> Klasse.
    /// </summary>
    /// <param name="ticketRepository">Das Repository für Tickets.</param>
    /// <param name="userManager">Der Identity UserManager.</param>
    /// <param name="httpContextAccessor">Der Accessor für den aktuellen HttpContext.</param>
    public TicketService(
        ITicketRepository ticketRepository,
        UserManager<User> userManager,
        IHttpContextAccessor httpContextAccessor)
    {
        _ticketRepository = ticketRepository;
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
    }

    /// <summary>
    /// Ermittelt den aktuell angemeldeten Benutzer.
    /// </summary>
    /// <returns>Das <see cref="User"/> Objekt oder null.</returns>
    private async Task<User?> GetCurrentUserAsync()
    {
        return await _userManager.GetUserAsync(_httpContextAccessor.HttpContext!.User);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<TicketDto>> GetActiveTicketsAsync()
    {
        var tickets = await _ticketRepository.GetAllActiveAsync();
        return tickets.Select(MapToDto);
    }

    /// <inheritdoc/>
    public async Task<TicketDto?> GetTicketAsync(Guid id)
    {
        var ticket = await _ticketRepository.GetByIdAsync(id);
        return ticket != null ? MapToDto(ticket) : null;
    }

    /// <inheritdoc/>
    public async Task CreateTicketAsync(CreateTicketDto dto)
    {
        var user = await GetCurrentUserAsync();
        if (user == null) throw new UnauthorizedAccessException();

        // Standard-Status für MVP (Initialzustand des Workflows)
        var defaultStateId = await _ticketRepository.GetDefaultWorkflowStateIdAsync();

        var ticket = new Ticket(dto.Title, TicketsPlease.Domain.Enums.TicketType.Task, dto.ProjectId, user.Id, defaultStateId, "initial");
        ticket.UpdateDescription(dto.Description, dto.Description);
        ticket.AssignUser(dto.AssignedUserId);
        ticket.SetPriority(dto.PriorityId);
        ticket.SetEstimatePoints(dto.EstimatePoints);
        ticket.SetTenantId(user.TenantId);

        await _ticketRepository.AddAsync(ticket);
        await _ticketRepository.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public async Task UpdateTicketAsync(UpdateTicketDto dto)
    {
        var ticket = await _ticketRepository.GetByIdAsync(dto.Id);
        if (ticket == null) throw new KeyNotFoundException("Ticket nicht gefunden.");

        ticket.UpdateTitle(dto.Title);
        ticket.UpdateDescription(dto.Description, dto.Description);
        ticket.AssignUser(dto.AssignedUserId);
        ticket.SetPriority(dto.PriorityId);
        ticket.SetEstimatePoints(dto.EstimatePoints);

        await _ticketRepository.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public async Task MoveTicketAsync(Guid id, string newStatus)
    {
        var ticket = await _ticketRepository.GetByIdAsync(id);
        if (ticket == null) throw new KeyNotFoundException("Ticket nicht gefunden.");

        if (newStatus == "Closed")
        {
            await CloseTicketAsync(id);
        }
        else
        {
            // MVP: Status-Update über Repository-Save (Property-Setzen erfolgt im Controller oder Service)
            // Beachte: Ticket Entity Status ist private set in DDD, hier müsste ggf. eine Methode hin.
            await _ticketRepository.SaveChangesAsync();
        }
    }

    /// <inheritdoc/>
    public async Task CloseTicketAsync(Guid id)
    {
        var ticket = await _ticketRepository.GetByIdAsync(id);
        var user = await GetCurrentUserAsync();
        if (ticket == null || user == null) throw new KeyNotFoundException();

        var roles = await _userManager.GetRolesAsync(user);
        ticket.Close(user.Id, roles.Contains("Admin"));
        await _ticketRepository.SaveChangesAsync();
    }

    /// <summary>
    /// Mappt eine Ticket-Entität auf ein DTO.
    /// </summary>
    /// <param name="t">Die Entität.</param>
    /// <returns>Das gemappte <see cref="TicketDto"/>.</returns>
    private TicketDto MapToDto(Ticket t) => new TicketDto(
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
        t.EstimatePoints
    );
}
