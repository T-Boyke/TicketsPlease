using TicketsPlease.Domain.Common;

namespace TicketsPlease.Domain.Entities;

public class CustomFieldDefinition : BaseEntity
{
  public string Name { get; set; } = string.Empty;
  public string FieldType { get; set; } = string.Empty; // Text, Number, Date, List
  public string? ConfigurationJson { get; set; }
}
