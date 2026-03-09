using TicketsPlease.Domain.Common;

namespace TicketsPlease.Domain.Entities;

public class TeamMember : BaseEntity
{
    public Guid TeamId { get; set; }
    public Team? Team { get; set; }
    public Guid UserId { get; set; }
    public User? User { get; set; }
    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
    public bool IsTeamLead { get; set; }
}
