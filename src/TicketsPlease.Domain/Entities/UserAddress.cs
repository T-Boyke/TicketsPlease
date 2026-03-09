using TicketsPlease.Domain.Common;

namespace TicketsPlease.Domain.Entities;

public class UserAddress : BaseEntity
{
    public Guid UserId { get; set; }
    public User? User { get; set; }
    public string Street { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
}
