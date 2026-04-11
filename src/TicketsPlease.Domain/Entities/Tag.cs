// <copyright file="Tag.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
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

  /// <summary>
  /// Gets or sets das Font-Awesome Icon (z.B. "fa-bug").
  /// </summary>
  public string Icon { get; set; } = "fa-tag";
}
