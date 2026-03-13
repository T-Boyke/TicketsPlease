using TicketsPlease.Domain.Common;

namespace TicketsPlease.Domain.Entities;

public class Organization : BaseEntity
{
  public string Name { get; set; } = string.Empty;
  public string SubscriptionLevel { get; set; } = "Trial";
  public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
  public bool IsActive { get; set; } = true;
}
