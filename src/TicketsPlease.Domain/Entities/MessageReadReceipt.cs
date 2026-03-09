using TicketsPlease.Domain.Common;

namespace TicketsPlease.Domain.Entities;

public class MessageReadReceipt : BaseEntity
{
    public Guid MessageId { get; set; }
    public Message? Message { get; set; }
    public Guid UserId { get; set; }
    public User? User { get; set; }
    public DateTime ReadAt { get; set; } = DateTime.UtcNow;
}
