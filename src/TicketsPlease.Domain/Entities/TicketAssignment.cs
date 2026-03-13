using TicketsPlease.Domain.Common;

namespace TicketsPlease.Domain.Entities;

public class TicketAssignment : BaseEntity
{
  public Guid TicketId { get; set; }
  public Ticket? Ticket { get; set; }
  public Guid? UserId { get; set; }
  public User? User { get; set; }
  public Guid? TeamId { get; set; }
  public Team? Team { get; set; }
  public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
}
