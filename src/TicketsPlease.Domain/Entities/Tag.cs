using TicketsPlease.Domain.Common;

namespace TicketsPlease.Domain.Entities;

public class Tag : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string ColorHex { get; set; } = string.Empty;
}
