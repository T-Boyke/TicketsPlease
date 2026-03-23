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
public class Ticket : BaseAuditableEntity
{
  /// <summary>
  /// Initializes a new instance of the <see cref="Ticket"/> class.
  /// </summary>
  /// <param name="title">Der Titel des Tickets.</param>
  /// <param name="projectId">Die ID des Projekts.</param>
  /// <param name="creatorId">Die ID des Erstellers.</param>
  /// <param name="workflowStateId">Die ID des initialen Status.</param>
  /// <param name="geoIpTimestamp">Der GeoIP-Zeitstempel für Audits.</param>
  public Ticket(string title, Guid projectId, Guid creatorId, Guid workflowStateId, string geoIpTimestamp)
  {
    if (string.IsNullOrWhiteSpace(title))
    {
      throw new ArgumentException("Titel darf nicht leer sein.", nameof(title));
    }

    this.Title = title;
    this.ProjectId = projectId;
    this.CreatorId = creatorId;
    this.WorkflowStateId = workflowStateId;
    this.GeoIpTimestamp = geoIpTimestamp ?? string.Empty;
    this.CreatedAt = DateTime.UtcNow;
    this.Status = "Todo";
    this.Sha1Hash = this.GenerateSha1Hash();
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="Ticket"/> class.
  /// Required for EF Core.
  /// </summary>
  private Ticket()
  {
  }

  /// <summary>
  /// Gets den Titel des Tickets.
  /// </summary>
  public string Title { get; private set; } = string.Empty;

  /// <summary>
  /// Gets den SHA1-Hash zur eindeutigen Identifizierung.
  /// </summary>
  public string Sha1Hash { get; private set; } = string.Empty;

  /// <summary>
  /// Gets die detaillierte Beschreibung des Tickets.
  /// </summary>
  public string Description { get; private set; } = string.Empty;

  /// <summary>
  /// Gets die Beschreibung im Markdown-Format.
  /// </summary>
  public string DescriptionMarkdown { get; private set; } = string.Empty;

  /// <summary>
  /// Gets den Status des Tickets (z.B. To-Do, Doing, Done).
  /// </summary>
  public string Status { get; private set; } = "Todo";

  /// <summary>
  /// Gets die ID der Priorität.
  /// </summary>
  public Guid PriorityId { get; private set; }

  /// <summary>
  /// Gets das Navigation-Property zur Priorität.
  /// </summary>
  public TicketPriority? Priority { get; private set; }

  /// <summary>
  /// Gets die Schwierigkeit (1-5 Chilis).
  /// </summary>
  public int ChilliesDifficulty { get; private set; }

  /// <summary>
  /// Gets den GeoIP-Zeitstempel für Audits.
  /// </summary>
  public string GeoIpTimestamp { get; private set; } = string.Empty;

  /// <summary>
  /// Gets den geplanten Startzeitpunkt.
  /// </summary>
  public DateTime? StartDate { get; private set; }

  /// <summary>
  /// Gets die Deadline.
  /// </summary>
  public DateTime? Deadline { get; private set; }

  /// <summary>
  /// Gets die ID des Projekts, dem das Ticket zugeordnet ist.
  /// </summary>
  public Guid ProjectId { get; private set; }

  /// <summary>
  /// Gets das Navigation-Property zum Projekt.
  /// </summary>
  public Project? Project { get; private set; }

  /// <summary>
  /// Gets die ID des zugeordneten Workflows.
  /// </summary>
  public Guid? WorkflowId { get; private set; }

  /// <summary>
  /// Gets das Navigation-Property zum Workflow.
  /// </summary>
  public Workflow? Workflow { get; private set; }

  /// <summary>
  /// Gets die ID des Workflow-Status.
  /// </summary>
  public Guid WorkflowStateId { get; private set; }

  /// <summary>
  /// Gets das Navigation-Property zum Workflow-Status.
  /// </summary>
  public WorkflowState? WorkflowState { get; private set; }

  /// <summary>
  /// Gets die ID des Erstellers.
  /// </summary>
  public Guid CreatorId { get; private set; }

  /// <summary>
  /// Gets das Navigation-Property zum Ersteller.
  /// </summary>
  public User? Creator { get; private set; }


  /// <summary>
  /// Gets die ID des Benutzers, dem das Ticket zugewiesen ist (Nullable).
  /// </summary>
  public Guid? AssignedUserId { get; private set; }

  /// <summary>
  /// Gets das Navigation-Property zum zugewiesenen Benutzer.
  /// </summary>
  public User? AssignedUser { get; private set; }

  /// <summary>
  /// Gets den Zeitpunkt (UTC), zu dem das Ticket geschlossen wurde.
  /// </summary>
  public DateTime? ClosedAt { get; private set; }

  /// <summary>
  /// Gets die ID des Benutzers, der das Ticket geschlossen hat.
  /// </summary>
  public Guid? ClosedByUserId { get; private set; }

  /// <summary>
  /// Aktualisiert den Titel des Tickets.
  /// </summary>
  /// <param name="title">Der neue Titel.</param>
  public void UpdateTitle(string title)
  {
    if (string.IsNullOrWhiteSpace(title))
    {
      throw new ArgumentException("Titel darf nicht leer sein.", nameof(title));
    }

    this.Title = title;
    this.UpdatedAt = DateTime.UtcNow;
  }

  /// <summary>
  /// Aktualisiert die Beschreibung des Tickets.
  /// </summary>
  /// <param name="description">Die einfache Beschreibung.</param>
  /// <param name="markdown">Die Markdown-Beschreibung.</param>
  public void UpdateDescription(string description, string markdown)
  {
    this.Description = description;
    this.DescriptionMarkdown = markdown;
    this.UpdatedAt = DateTime.UtcNow;
  }

  /// <summary>
  /// Verschiebt das Ticket in einen neuen Workflow-Status.
  /// </summary>
  /// <param name="workspaceStateId">Die ID des neuen Status.</param>
  public void MoveToState(Guid workspaceStateId)
  {
    this.WorkflowStateId = workspaceStateId;
    this.UpdatedAt = DateTime.UtcNow;
  }

  /// <summary>
  /// Weist das Ticket einem Benutzer zu.
  /// </summary>
  /// <param name="userId">Die ID des Benutzers oder null.</param>
  public void AssignUser(Guid? userId)
  {
    this.AssignedUserId = userId;
    this.UpdatedAt = DateTime.UtcNow;
  }

  /// <summary>
  /// Setzt die Priorität des Tickets.
  /// </summary>
  /// <param name="priorityId">Die ID der Priorität.</param>
  public void SetPriority(Guid priorityId)
  {
    this.PriorityId = priorityId;
    this.UpdatedAt = DateTime.UtcNow;
  }

  /// <summary>
  /// Schließt das Ticket.
  /// (Regel F3.4: Nur Admin, Ersteller oder Zugewiesener dürfen schließen).
  /// </summary>
  /// <param name="actorId">Die ID des handelnden Benutzers.</param>
  /// <param name="isAdmin">Gibt an, ob der Benutzer Admin-Rechte hat.</param>
  public void Close(Guid actorId, bool isAdmin)
  {
    if (!isAdmin && actorId != this.CreatorId && actorId != this.AssignedUserId)
    {
      throw new InvalidOperationException("Nur der Admin, der Ersteller oder der zugewiesene Benutzer dürfen das Ticket schließen.");
    }

    this.ClosedAt = DateTime.UtcNow;
    this.ClosedByUserId = actorId;
    this.Status = "Closed";
    this.UpdatedAt = DateTime.UtcNow;
  }

  /// <summary>
  /// Setzt die Schwierigkeit des Tickets.
  /// </summary>
  /// <param name="difficulty">Die Schwierigkeit (1-5).</param>
  public void SetDifficulty(int difficulty)
  {
    if (difficulty < 1 || difficulty > 5)
    {
      throw new ArgumentOutOfRangeException(nameof(difficulty), "Schwierigkeit muss zwischen 1 und 5 liegen.");
    }

    this.ChilliesDifficulty = difficulty;
    this.UpdatedAt = DateTime.UtcNow;
  }

  /// <summary>
  /// Setzt den Mandanten des Tickets (Multi-Tenancy).
  /// </summary>
  /// <param name="tenantId">Die ID des Mandanten.</param>
  public void SetTenantId(Guid tenantId)
  {
    this.TenantId = tenantId;
    this.UpdatedAt = DateTime.UtcNow;
  }

  private string GenerateSha1Hash()
  {
    // Echter SHA1 Hash für den Domain Hash (40 Zeichen Hex)
    var raw = $"{this.ProjectId}-{this.CreatorId}-{this.CreatedAt.Ticks}-{Guid.NewGuid()}";
    var hashBytes = System.Security.Cryptography.SHA1.HashData(System.Text.Encoding.UTF8.GetBytes(raw));
    return Convert.ToHexString(hashBytes).ToLowerInvariant();
  }
}
