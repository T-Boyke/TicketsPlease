// <copyright file="Project.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TicketsPlease.Domain.Entities;

using System;
using System.Collections.Generic;
using TicketsPlease.Domain.Common;

/// <summary>
/// Repräsentiert ein Projekt, dem Tickets zugeordnet werden können.
/// (IHK Pflicht F2.2).
/// </summary>
public class Project : BaseEntity
{
    /// <summary>
    /// Gets or sets den Titel des Projekts.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets die Beschreibung des Projekts.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets das Startdatum des Projekts (Pflicht).
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// Gets or sets das Enddatum des Projekts (Optional).
    /// </summary>
    public DateTime? EndDate { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether das Projekt offen ist.
    /// Nur offene Projekte können Tickets zugeordnet werden.
    /// </summary>
    public bool IsOpen { get; set; } = true;

    /// <summary>
    /// Gets or sets die ID des zugeordneten Workflows.
    /// </summary>
    public Guid? WorkflowId { get; set; }

    /// <summary>
    /// Gets or sets den zugeordneten Workflow.
    /// Each project can have exactly one workflow (IHK F8.4).
    /// </summary>
    public Workflow? Workflow { get; set; }

    /// <summary>
    /// Gets or sets die Liste der zugeordneten Tickets.
    /// </summary>
    public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
