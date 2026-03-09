using TicketsPlease.Domain.Common;

namespace TicketsPlease.Domain.Entities;

public class SlaPolicy : BaseEntity
{
    public Guid PriorityId { get; set; }
    public TicketPriority? Priority { get; set; }
    public int ResponseTimeHours { get; set; }
    public int ResolutionTimeHours { get; set; }
}
