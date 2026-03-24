// <copyright file="Project.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TicketsPlease.Domain.Entities;

using System;
using System.Collections.Generic;
using TicketsPlease.Domain.Common;

/// <summary>
/// Repräsentiert ein Projekt, dem Tickets zugeordnet werden können.
/// (Pflicht F2.2).
/// </summary>
public class Project : BaseEntity
{
  /// <summary>
  /// Initializes a new instance of the <see cref="Project"/> class.
  /// </summary>
  /// <param name="title">Der Titel des Projekts.</param>
  /// <param name="startDate">Das Startdatum.</param>
  public Project(string title, DateTime startDate)
  {
    if (string.IsNullOrWhiteSpace(title))
    {
      throw new ArgumentException("Titel darf nicht leer sein.", nameof(title));
    }

    this.Title = title;
    this.StartDate = startDate;
    this.IsOpen = true;
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="Project"/> class.
  /// Required for EF Core.
  /// </summary>
  private Project()
  {
  }

  /// <summary>
  /// Gets den Titel des Projekts.
  /// </summary>
  public string Title { get; private set; } = string.Empty;

  /// <summary>
  /// Gets die Beschreibung des Projekts.
  /// </summary>
  public string Description { get; private set; } = string.Empty;

  /// <summary>
  /// Gets das Startdatum des Projekts (Pflicht).
  /// </summary>
  public DateTime StartDate { get; private set; }

  /// <summary>
  /// Gets das Enddatum des Projekts (Optional).
  /// </summary>
  public DateTime? EndDate { get; private set; }

  /// <summary>
  /// Gets a value indicating whether das Projekt offen ist.
  /// Nur offene Projekte können Tickets zugeordnet werden.
  /// </summary>
  public bool IsOpen { get; private set; } = true;

  /// <summary>
  /// Gets die ID des zugeordneten Workflows.
  /// </summary>
  public Guid? WorkflowId { get; private set; }

  /// <summary>
  /// Gets den zugeordneten Workflow.
  /// Each project can have exactly one workflow (F8.4).
  /// </summary>
  public Workflow? Workflow { get; }

  /// <summary>
  /// Gets die Liste der zugeordneten Tickets.
  /// </summary>
  public ICollection<Ticket> Tickets { get; private set; } = new List<Ticket>();

  /// <summary>
  /// Aktualisiert die Metadaten des Projekts.
  /// </summary>
  /// <param name="title">Der neue Titel.</param>
  /// <param name="description">Die neue Beschreibung.</param>
  public void UpdateMetadata(string title, string description)
  {
    if (string.IsNullOrWhiteSpace(title))
    {
      throw new ArgumentException("Titel darf nicht leer sein.", nameof(title));
    }

    this.Title = title;
    this.Description = description;
  }

  /// <summary>
  /// Setzt das Enddatum des Projekts.
  /// </summary>
  /// <param name="endDate">Das Enddatum.</param>
  public void SetEndDate(DateTime? endDate)
  {
    this.EndDate = endDate;
  }

  /// <summary>
  /// Schließt das Projekt.
  /// </summary>
  public void Close()
  {
    this.IsOpen = false;
    this.EndDate = DateTime.UtcNow;
  }

  /// <summary>
  /// Öffnet das Projekt wieder.
  /// </summary>
  public void Open()
  {
    this.IsOpen = true;
    this.EndDate = null;
  }

  /// <summary>
  /// Weist dem Projekt einen Workflow zu.
  /// </summary>
  /// <param name="workflowId">Die ID des Workflows.</param>
  public void AssignWorkflow(Guid workflowId)
  {
    this.WorkflowId = workflowId;
  }

  /// <summary>
  /// Setzt den Mandanten des Projekts (Multi-Tenancy).
  /// </summary>
  /// <param name="tenantId">Die ID des Mandanten.</param>
  public void SetTenantId(Guid tenantId)
  {
    this.TenantId = tenantId;
  }
}
