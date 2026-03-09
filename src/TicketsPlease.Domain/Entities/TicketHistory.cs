using TicketsPlease.Domain.Common;

namespace TicketsPlease.Domain.Entities;

public class TicketHistory : BaseEntity
{
    public Guid TicketId { get; set; }
    public Ticket? Ticket { get; set; }
    public Guid ActorUserId { get; set; }
    public User? ActorUser { get; set; }
    public string FieldName { get; set; } = string.Empty;
    public string? OldValue { get; set; }
    public string? NewValue { get; set; }
    public DateTime ChangedAt { get; set; } = DateTime.UtcNow;
}
