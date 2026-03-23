// <copyright file="Ticket.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TicketsPlease.Domain.Entities;

using System;
using TicketsPlease.Domain.Common;

/// <summary>
/// Repräsentiert ein Ticket (Aufgabe) im Kanban-System.
/// Erbt von <see cref="BaseEntity"/> für die ID und Nebenläufigkeitskontrolle.
/// </summary>
public class Ticket : BaseEntity
{
  /// <summary>
  /// Gets or sets den Titel des Tickets.
  /// </summary>
  public string Title { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets den SHA1-Hash zur eindeutigen Identifizierung.
  /// </summary>
  public string Sha1Hash { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets die detaillierte Beschreibung des Tickets.
  /// </summary>
  public string Description { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets die Beschreibung im Markdown-Format.
  /// </summary>
  public string DescriptionMarkdown { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets den Status des Tickets (z.B. To-Do, Doing, Done).
  /// </summary>
  public string Status { get; set; } = "Todo";

  /// <summary>
  /// Gets or sets die ID der Priorität.
  /// </summary>
  public Guid PriorityId { get; set; }

  /// <summary>
  /// Gets or sets das Navigation-Property zur Priorität.
  /// </summary>
  public TicketPriority? Priority { get; set; }

  /// <summary>
  /// Gets or sets die Schwierigkeit (1-5 Chilis).
  /// </summary>
  public int ChilliesDifficulty { get; set; }

  /// <summary>
  /// Gets or sets den GeoIP-Zeitstempel für Audits.
  /// </summary>
  public string GeoIpTimestamp { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets den geplanten Startzeitpunkt.
  /// </summary>
  public DateTime? StartDate { get; set; }

  /// <summary>
  /// Gets or sets die Deadline.
  /// </summary>
  public DateTime? Deadline { get; set; }

  /// <summary>
  /// Gets or sets die ID des Projekts, dem das Ticket zugeordnet ist.
  /// </summary>
  public Guid ProjectId { get; set; }

  /// <summary>
  /// Gets or sets das Navigation-Property zum Projekt.
  /// </summary>
  public Project? Project { get; set; }

  /// <summary>
  /// Gets or sets die ID des zugeordneten Workflows.
  /// </summary>
  public Guid? WorkflowId { get; set; }

  /// <summary>
  /// Gets or sets das Navigation-Property zum Workflow.
  /// </summary>
  public Workflow? Workflow { get; set; }

  /// <summary>
  /// Gets or sets die ID des Workflow-Status.
  /// </summary>
  public Guid WorkflowStateId { get; set; }

  /// <summary>
  /// Gets or sets das Navigation-Property zum Workflow-Status.
  /// </summary>
  public WorkflowState? WorkflowState { get; set; }

  /// <summary>
  /// Gets or sets die ID des Erstellers.
  /// </summary>
  public Guid CreatorId { get; set; }

  /// <summary>
  /// Gets or sets das Navigation-Property zum Ersteller.
  /// </summary>
  public User? Creator { get; set; }

  /// <summary>
  /// Gets or sets den Erstellungszeitpunkt.
  /// </summary>
  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

  /// <summary>
  /// Gets or sets den Zeitpunkt der letzten Änderung.
  /// </summary>
  public DateTime? UpdatedAt { get; set; }

  /// <summary>
  /// Gets or sets die ID des Benutzers, dem das Ticket zugewiesen ist (Nullable).
  /// </summary>
  public Guid? AssignedUserId { get; set; }

  /// <summary>
  /// Gets or sets das Navigation-Property zum zugewiesenen Benutzer.
  /// </summary>
  public User? AssignedUser { get; set; }
}
