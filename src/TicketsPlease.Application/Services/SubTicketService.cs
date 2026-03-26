// <copyright file="SubTicketService.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Application.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TicketsPlease.Application.Common.Dtos;
using TicketsPlease.Application.Common.Interfaces;
using TicketsPlease.Domain.Entities;
using TicketsPlease.Infrastructure.Persistence;

/// <summary>
/// Implementierung des Unterticket-Services für Checklisten.
/// </summary>
public class SubTicketService : ISubTicketService
{
    private readonly AppDbContext context;

    /// <summary>
    /// Initializes a new instance of the <see cref="SubTicketService"/> class.
    /// </summary>
    /// <param name="context">Der Datenbankkontext.</param>
    public SubTicketService(AppDbContext context)
    {
        this.context = context;
    }

    /// <inheritdoc/>
    public async Task<SubTicketDto> AddSubTicketAsync(Guid ticketId, string title, Guid creatorId)
    {
        var sub = new SubTicket
        {
            Id = Guid.NewGuid(),
            TicketId = ticketId,
            Title = title,
            CreatorId = creatorId,
            IsCompleted = false,
            CreatedAt = DateTime.UtcNow
        };

        this.context.SubTickets.Add(sub);
        await this.context.SaveChangesAsync().ConfigureAwait(false);

        return new SubTicketDto(sub.Id, sub.Title, sub.IsCompleted);
    }

    /// <inheritdoc/>
    public async Task ToggleSubTicketAsync(Guid subTicketId)
    {
        var sub = await this.context.SubTickets.FindAsync(subTicketId).ConfigureAwait(false);
        if (sub != null)
        {
            sub.IsCompleted = !sub.IsCompleted;
            await this.context.SaveChangesAsync().ConfigureAwait(false);
        }
    }

    /// <inheritdoc/>
    public async Task DeleteSubTicketAsync(Guid subTicketId)
    {
        var sub = await this.context.SubTickets.FindAsync(subTicketId).ConfigureAwait(false);
        if (sub != null)
        {
            this.context.SubTickets.Remove(sub);
            await this.context.SaveChangesAsync().ConfigureAwait(false);
        }
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<SubTicketDto>> GetSubTicketsAsync(Guid ticketId)
    {
        return await this.context.SubTickets
            .AsNoTracking()
            .Where(s => s.TicketId == ticketId)
            .OrderBy(s => s.CreatedAt)
            .Select(s => new SubTicketDto(s.Id, s.Title, s.IsCompleted))
            .ToListAsync()
            .ConfigureAwait(false);
    }
}
