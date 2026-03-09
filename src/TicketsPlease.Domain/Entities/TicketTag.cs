using TicketsPlease.Domain.Common;

namespace TicketsPlease.Domain.Entities;

public class TicketTag : BaseEntity
{
    public Guid TicketId { get; set; }
    public Ticket? Ticket { get; set; }
    public Guid TagId { get; set; }
    public Tag? Tag { get; set; }
}
