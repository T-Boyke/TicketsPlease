// <copyright file="Ticket.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Domain.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using TicketsPlease.Domain.Common;
using TicketsPlease.Domain.Enums;

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
  /// <param name="type">Der Typ des Tickets (z.B. Task, Bug, Epic).</param>
  /// <param name="projectId">Die ID des Projekts.</param>
  /// <param name="creatorId">Die ID des Erstellers.</param>
  /// <param name="workflowStateId">Die ID des initialen Status.</param>
  /// <param name="geoIpTimestamp">Der GeoIP-Zeitstempel für Audits.</param>
  public Ticket(string title, TicketType type, Guid projectId, Guid creatorId, Guid workflowStateId, string geoIpTimestamp)
  {
    if (string.IsNullOrWhiteSpace(title))
    {
      throw new ArgumentException("Titel darf nicht leer sein.", nameof(title));
    }

    this.Title = title;
    this.Type = type;
    this.ProjectId = projectId;
    this.CreatorId = creatorId;
    this.WorkflowStateId = workflowStateId;
    this.GeoIpTimestamp = geoIpTimestamp ?? string.Empty;
    this.CreatedAt = DateTime.UtcNow;
    this.Status = "Todo";
    this.DomainHash = this.GenerateDomainHash();
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
  /// Gets den Hash zur eindeutigen Identifizierung (Domain-Level).
  /// </summary>
  public string DomainHash { get; private set; } = string.Empty;

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
  public TicketPriority? Priority { get; }

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
  public DateTime? StartDate { get; }

  /// <summary>
  /// Gets die Deadline.
  /// </summary>
  public DateTime? Deadline { get; }

  /// <summary>
  /// Gets die ID des Projekts, dem das Ticket zugeordnet ist.
  /// </summary>
  public Guid ProjectId { get; private set; }

  /// <summary>
  /// Gets das Navigation-Property zum Projekt.
  /// </summary>
  public Project? Project { get; }

  /// <summary>
  /// Gets die ID des zugeordneten Workflows.
  /// </summary>
  public Guid? WorkflowId { get; }

  /// <summary>
  /// Gets das Navigation-Property zum Workflow.
  /// </summary>
  public Workflow? Workflow { get; }

  /// <summary>
  /// Gets die ID des Workflow-Status.
  /// </summary>
  public Guid WorkflowStateId { get; private set; }

  /// <summary>
  /// Gets das Navigation-Property zum Workflow-Status.
  /// </summary>
  public WorkflowState? WorkflowState { get; }

  /// <summary>
  /// Gets die ID des Erstellers.
  /// </summary>
  public Guid CreatorId { get; private set; }

  /// <summary>
  /// Gets das Navigation-Property zum Ersteller.
  /// </summary>
  public User? Creator { get; }

  /// <summary>
  /// Gets den Typ des Tickets.
  /// </summary>
  public TicketType Type { get; private set; }

  /// <summary>
  /// Gets die geschätzte Größe des Tickets.
  /// </summary>
  public TicketSize Size { get; private set; }

  /// <summary>
  /// Gets die geschätzten Story Points.
  /// </summary>
  public int? EstimatePoints { get; private set; }

  /// <summary>
  /// Gets die ID des übergeordneten Tickets (für Epics/Hierarchien).
  /// </summary>
  public Guid? ParentTicketId { get; private set; }

  /// <summary>
  /// Gets das übergeordnete Ticket.
  /// </summary>
  public Ticket? ParentTicket { get; }

  /// <summary>
  /// Gets die Liste der zugeordneten Tags.
  /// </summary>
  public ICollection<TicketTag> Tags { get; private set; } = new List<TicketTag>();

  /// <summary>
  /// Gets die Liste der hierarchisch untergeordneten Tickets (Children).
  /// </summary>
  public ICollection<Ticket> Children { get; private set; } = new List<Ticket>();

  /// <summary>
  /// Gets die Liste der Untertickets (Checklisteneinträge).
  /// </summary>
  public ICollection<SubTicket> SubTickets { get; private set; } = new List<SubTicket>();

  /// <summary>
  /// Gets die Liste der Kommentare zu diesem Ticket (F5).
  /// </summary>
  public ICollection<Comment> Comments { get; private set; } = new List<Comment>();

  /// <summary>
  /// Gets die Liste der Tickets, die dieses Ticket blockieren.
  /// </summary>
  public ICollection<TicketLink> BlockedBy { get; private set; } = new List<TicketLink>();

  /// <summary>
  /// Gets die Liste der Tickets, die von diesem Ticket blockiert werden.
  /// </summary>
  public ICollection<TicketLink> Blocking { get; private set; } = new List<TicketLink>();

  /// <summary>
  /// Gets die Liste der Dateianhänge des Tickets.
  /// </summary>
  public ICollection<FileAsset> Attachments { get; private set; } = new List<FileAsset>();

  /// <summary>
  /// Gets die ID des Benutzers, dem das Ticket zugewiesen ist (Nullable).
  /// </summary>
  public Guid? AssignedUserId { get; private set; }

  /// <summary>
  /// Gets das Navigation-Property zum zugewiesenen Benutzer.
  /// </summary>
  public User? AssignedUser { get; }

  /// <summary>
  /// Gets den Zeitpunkt (UTC), zu dem das Ticket geschlossen wurde.
  /// </summary>
  public DateTime? ClosedAt { get; private set; }

  /// <summary>
  /// Gets die ID des Benutzers, der das Ticket geschlossen hat.
  /// </summary>
  public Guid? ClosedByUserId { get; private set; }

  /// <summary>
  /// Prüft, ob das Ticket geschlossen werden kann (F7).
  /// Ein Ticket kann nicht geschlossen werden, wenn es noch offene Abhängigkeiten (Vorgänger) hat.
  /// </summary>
  /// <returns>True, wenn keine offenen Blocker existieren.</returns>
  public bool CanBeClosed()
  {
    // Ein Ticket ist blockiert, wenn einer seiner Vorgänger (BlockedBy -> SourceTicket) nicht geschlossen ist.
    return !this.BlockedBy.Any(d => d.LinkType == TicketsPlease.Domain.Enums.TicketLinkType.Blocks && d.SourceTicket != null && d.SourceTicket.Status != "Closed" && d.SourceTicket.Status != "Done");
  }

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

  /// <summary>
  /// Setzt den Typ des Tickets.
  /// </summary>
  /// <param name="type">Der neue Typ.</param>
  public void SetType(TicketType type)
  {
    this.Type = type;
    this.UpdatedAt = DateTime.UtcNow;
  }

  /// <summary>
  /// Setzt die Größe des Tickets.
  /// </summary>
  /// <param name="size">Die geschätzte Größe.</param>
  public void SetSize(TicketSize size)
  {
    this.Size = size;
    this.UpdatedAt = DateTime.UtcNow;
  }

  /// <summary>
  /// Setzt die geschätzten Story Points.
  /// </summary>
  /// <param name="points">Die Punkte.</param>
  public void SetEstimatePoints(int? points)
  {
    this.EstimatePoints = points;
    this.UpdatedAt = DateTime.UtcNow;
  }

  /// <summary>
  /// Setzt das übergeordnete Ticket.
  /// </summary>
  /// <param name="parentId">Die ID des Eltern-Tickets.</param>
  public void SetParent(Guid? parentId)
  {
    if (parentId == this.Id)
    {
      throw new InvalidOperationException("Ein Ticket kann nicht sein eigenes Eltern-Ticket sein.");
    }

    this.ParentTicketId = parentId;
    this.UpdatedAt = DateTime.UtcNow;
  }

  /// <summary>
  /// Fügt eine Verknüpfung zu einem anderen Ticket hinzu.
  /// </summary>
  /// <param name="targetId">Das Ziel-Ticket.</param>
  /// <param name="linkType">Der Typ der Verknüpfung.</param>
  public void AddLink(Guid targetId, TicketLinkType linkType)
  {
    if (targetId == this.Id)
    {
      throw new InvalidOperationException("Ein Ticket kann nicht mit sich selbst verknüpft werden.");
    }

    this.Blocking.Add(new TicketLink(this.Id, targetId, linkType));
    this.UpdatedAt = DateTime.UtcNow;
  }

  /// <summary>
  /// Synchronisiert die Tags des Tickets.
  /// </summary>
  /// <param name="tagIds">Die Liste der neuen Tag-IDs.</param>
  public void SyncTags(IEnumerable<Guid> tagIds)
  {
    tagIds ??= Enumerable.Empty<Guid>();

    // Entferne Tags, die nicht mehr in der Liste sind
    var toRemove = this.Tags.Where(tt => !tagIds.Contains(tt.TagId)).ToList();
    foreach (var tt in toRemove)
    {
      this.Tags.Remove(tt);
    }

    // Füge neue Tags hinzu
    foreach (var tagId in tagIds)
    {
      if (!this.Tags.Any(tt => tt.TagId == tagId))
      {
        this.Tags.Add(new TicketTag { TicketId = this.Id, TagId = tagId });
      }
    }

    this.UpdatedAt = DateTime.UtcNow;
  }

  /// <summary>
  /// Berechnet den Fortschritt in Prozent basierend auf den Untertickets (Checklisteneinträgen).
  /// </summary>
  /// <returns>Ein Wert zwischen 0 und 100.</returns>
  public int GetProgressPercentage()
  {
    if (this.SubTickets.Count == 0)
    {
      return this.Status == "Closed" ? 100 : 0;
    }

    var closedCount = this.SubTickets.Count(t => t.IsCompleted);
    return (int)Math.Round((double)closedCount / this.SubTickets.Count * 100);
  }

  private string GenerateDomainHash()
  {
    // SHA256 Hash für den Domain Hash (64 Zeichen Hex)
    var raw = $"{this.ProjectId}-{this.CreatorId}-{this.CreatedAt.Ticks}-{Guid.NewGuid()}";
    var hashBytes = System.Security.Cryptography.SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(raw));
    return Convert.ToHexString(hashBytes).ToUpperInvariant();
  }
}
