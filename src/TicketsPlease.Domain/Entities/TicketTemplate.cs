using TicketsPlease.Domain.Common;

namespace TicketsPlease.Domain.Entities;

public class TicketTemplate : BaseEntity
{
  public string Name { get; set; } = string.Empty;
  public string DescriptionMarkdownTemplate { get; set; } = string.Empty;
  public Guid? DefaultPriorityId { get; set; }
  public TicketPriority? DefaultPriority { get; set; }
  public Guid CreatorId { get; set; }
  public User? Creator { get; set; }
}
