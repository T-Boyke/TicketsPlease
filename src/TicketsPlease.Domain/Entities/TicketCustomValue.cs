using TicketsPlease.Domain.Common;

namespace TicketsPlease.Domain.Entities;

public class TicketCustomValue : BaseEntity
{
  public Guid TicketId { get; set; }
  public Ticket? Ticket { get; set; }
  public Guid FieldDefinitionId { get; set; }
  public CustomFieldDefinition? FieldDefinition { get; set; }
  public string Value { get; set; } = string.Empty;
}
