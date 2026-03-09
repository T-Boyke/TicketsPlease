using TicketsPlease.Domain.Common;

namespace TicketsPlease.Domain.Entities;

public class TicketPriority : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public int LevelWeight { get; set; }
    public string ColorHex { get; set; } = string.Empty;
}
