using TicketsPlease.Domain.Common;

namespace TicketsPlease.Domain.Entities;

public class TimeLog : BaseEntity
{
    public Guid TicketId { get; set; }
    public Ticket? Ticket { get; set; }
    public Guid UserId { get; set; }
    public User? User { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime? StoppedAt { get; set; }
    public decimal HoursLogged { get; set; }
    public string? Description { get; set; }
}
