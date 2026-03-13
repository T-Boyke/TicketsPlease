using TicketsPlease.Domain.Common;

namespace TicketsPlease.Domain.Entities;

public class SubTicket : BaseEntity
{
  public Guid ParentTicketId { get; set; }
  public Ticket? ParentTicket { get; set; }
  public string Title { get; set; } = string.Empty;
  public bool IsCompleted { get; set; }
  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
  public Guid CreatorId { get; set; }
  public User? Creator { get; set; }
}
