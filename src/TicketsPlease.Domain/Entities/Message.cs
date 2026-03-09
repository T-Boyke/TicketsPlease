using TicketsPlease.Domain.Common;

namespace TicketsPlease.Domain.Entities;

public class Message : BaseEntity
{
    public Guid SenderUserId { get; set; }
    public User? SenderUser { get; set; }
    public Guid? TicketId { get; set; }
    public Ticket? Ticket { get; set; }
    public Guid? TeamId { get; set; }
    public Team? Team { get; set; }
    public Guid? ReceiverUserId { get; set; }
    public User? ReceiverUser { get; set; }
    public string BodyMarkdown { get; set; } = string.Empty;
    public DateTime SentAt { get; set; } = DateTime.UtcNow;
    public bool IsEdited { get; set; }
}
