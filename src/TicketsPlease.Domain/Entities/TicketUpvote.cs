using TicketsPlease.Domain.Common;

namespace TicketsPlease.Domain.Entities;

public class TicketUpvote : BaseEntity
{
    public Guid TicketId { get; set; }
    public Ticket? Ticket { get; set; }
    public Guid UserId { get; set; }
    public User? User { get; set; }
    public DateTime VotedAt { get; set; } = DateTime.UtcNow;
}
