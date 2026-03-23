// <copyright file="Workflow.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TicketsPlease.Domain.Entities;

using System.Collections.Generic;
using TicketsPlease.Domain.Common;

/// <summary>
/// Repräsentiert einen Workflow (Status-Reihe), den ein Ticket annehmen kann.
/// (IHK Pflicht F8).
/// </summary>
public class Workflow : BaseEntity
{
    /// <summary>
    /// Gets or sets die Bezeichnung des Workflows.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets die Liste der Status, die zu diesem Workflow gehören.
    /// </summary>
    public ICollection<WorkflowState> States { get; set; } = new List<WorkflowState>();

    /// <summary>
    /// Gets or sets die Liste der Projekte, die diesen Workflow nutzen.
    /// </summary>
    public ICollection<Project> Projects { get; set; } = new List<Project>();
}
