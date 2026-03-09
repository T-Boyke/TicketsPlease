using TicketsPlease.Domain.Common;

namespace TicketsPlease.Domain.Entities;

public class WorkflowTransition : BaseEntity
{
    public Guid FromStateId { get; set; }
    public WorkflowState? FromState { get; set; }
    public Guid ToStateId { get; set; }
    public WorkflowState? ToState { get; set; }
    public Guid? AllowedRoleId { get; set; }
    public Role? AllowedRole { get; set; }
}
