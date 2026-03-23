// <copyright file="CustomFieldDefinition.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TicketsPlease.Domain.Entities;

using TicketsPlease.Domain.Common;

/// <summary>
/// Repräsentiert die Definition eines benutzerdefinierten Feldes für Tickets.
/// </summary>
public class CustomFieldDefinition : BaseEntity
{
  /// <summary>
  /// Gets or sets den Namen des benutzerdefinierten Feldes.
  /// </summary>
  public string Name { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets den Typ des Feldes (z.B. Text, Number, Date, List).
  /// </summary>
  public string FieldType { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets die (optionale) JSON-Konfiguration für das Feld, z.B. Auswahlmöglichkeiten für Listen.
  /// </summary>
  public string? ConfigurationJson { get; set; }
}
