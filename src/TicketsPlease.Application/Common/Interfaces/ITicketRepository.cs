using TicketsPlease.Domain.Entities;

namespace TicketsPlease.Application.Common.Interfaces;

/// <summary>
/// Definiert die Datenzugriffsschicht für <see cref="Ticket"/> Entitäten.
/// </summary>
public interface ITicketRepository
{
    /// <summary>
    /// Ruft ein Ticket anhand seiner ID ab.
    /// </summary>
    /// <param name="id">Die ID des Tickets.</param>
    /// <param name="ct">Das Abbruchsignal (CancellationToken).</param>
    /// <returns>Das Ticket oder null, falls nicht gefunden.</returns>
    Task<Ticket?> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>
    /// Ruft alle aktiven Tickets ab (Read-only Optimierung).
    /// </summary>
    /// <param name="ct">Das Abbruchsignal.</param>
    /// <returns>Eine Liste von Tickets.</returns>
    Task<List<Ticket>> GetAllActiveAsync(CancellationToken ct = default);

    /// <summary>
    /// Fügt ein neues Ticket hinzu.
    /// </summary>
    /// <param name="ticket">Das zu speichernde Ticket.</param>
    /// <param name="ct">Das Abbruchsignal.</param>
    Task AddAsync(Ticket ticket, CancellationToken ct = default);

    /// <summary>
    /// Speichert sämtliche Änderungen im Kontext persistent ab.
    /// </summary>
    /// <param name="ct">Das Abbruchsignal.</param>
    /// <returns>Anzahl der betroffenen Datensätze.</returns>
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
