// <copyright file="Workflow.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Domain.Entities;

using System.Collections.Generic;
using TicketsPlease.Domain.Common;

/// <summary>
/// Repräsentiert einen Workflow (Status-Reihe), den ein Ticket annehmen kann.
/// (Pflicht F8).
/// </summary>
public class Workflow : BaseEntity
{
  /// <summary>
  /// Gets or sets die Bezeichnung des Workflows.
  /// </summary>
  public string Name { get; set; } = string.Empty;

  /// <summary>
  /// Gets die Liste der Status, die zu diesem Workflow gehören.
  /// </summary>
  public virtual ICollection<WorkflowState> States { get; } = new List<WorkflowState>();

  /// <summary>
  /// Gets die Liste der Projekte, die diesen Workflow nutzen.
  /// </summary>
  public virtual ICollection<Project> Projects { get; } = new List<Project>();
}
