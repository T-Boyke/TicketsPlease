using TicketsPlease.Domain.Common;

namespace TicketsPlease.Domain.Entities;

public class WorkflowState : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public int OrderIndex { get; set; }
    public string ColorHex { get; set; } = string.Empty;
    public bool IsTerminalState { get; set; }
}
