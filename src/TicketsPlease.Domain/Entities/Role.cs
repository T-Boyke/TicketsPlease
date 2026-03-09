using TicketsPlease.Domain.Common;

namespace TicketsPlease.Domain.Entities;

public class Role : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
