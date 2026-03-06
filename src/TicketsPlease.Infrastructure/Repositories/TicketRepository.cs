using Microsoft.EntityFrameworkCore;
using TicketsPlease.Application.Common.Interfaces;
using TicketsPlease.Domain.Entities;
using TicketsPlease.Infrastructure.Persistence;

namespace TicketsPlease.Infrastructure.Repositories;

/// <summary>
/// Implementiert den Datenzugriff für Tickets unter Verwendung von Entity Framework Core.
/// Berücksichtigt Performance-Optimierungen wie AsNoTracking für Leseabfragen.
/// </summary>
public class TicketRepository : ITicketRepository
{
    private readonly AppDbContext _context;

    /// <summary>
    /// Initialisiert eine neue Instanz von <see cref="TicketRepository"/>.
    /// </summary>
    /// <param name="context">Der injizierte Datenbankkontext.</param>
    public TicketRepository(AppDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public async Task<Ticket?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        // Wir nutzen hier kein AsNoTracking, da das Objekt evtl. bearbeitet werden soll (Tracking benötig).
        return await _context.Tickets
            .Include(t => t.AssignedUser)
            .FirstOrDefaultAsync(t => t.Id == id, ct);
    }

    /// <inheritdoc />
    public async Task<List<Ticket>> GetAllActiveAsync(CancellationToken ct = default)
    {
        // Enterprise Pattern: Reine Leseabfragen strikt mit AsNoTracking ausführen
        return await _context.Tickets
            .AsNoTracking()
            .Include(t => t.AssignedUser)
            .OrderByDescending(t => t.Priority)
            .ToListAsync(ct);
    }

    /// <inheritdoc />
    public async Task AddAsync(Ticket ticket, CancellationToken ct = default)
    {
        await _context.Tickets.AddAsync(ticket, ct);
    }

    /// <inheritdoc />
    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        // EF Core führt hier automatisch die Nebenläufigkeitsprüfung (RowVersion) durch
        return await _context.SaveChangesAsync(ct);
    }
}
