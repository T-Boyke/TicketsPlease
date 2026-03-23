// <copyright file="Tag.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TicketsPlease.Domain.Entities;

using TicketsPlease.Domain.Common;

/// <summary>
/// Repräsentiert ein Schlagwort (Tag) zur Kategorisierung von Tickets.
/// </summary>
public class Tag : BaseEntity
{
  /// <summary>
  /// Gets or sets den Namen des Tags.
  /// </summary>
  public string Name { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets den Hexadezimal-Farbcode des Tags für die UI-Darstellung.
  /// </summary>
  public string ColorHex { get; set; } = string.Empty;
}
