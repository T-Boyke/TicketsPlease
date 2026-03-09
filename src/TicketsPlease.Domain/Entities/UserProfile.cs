using TicketsPlease.Domain.Common;

namespace TicketsPlease.Domain.Entities;

public class UserProfile : BaseEntity
{
    public Guid UserId { get; set; }
    public User? User { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public Guid? AvatarImageId { get; set; }
    public FileAsset? AvatarImage { get; set; }
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
