using TicketsPlease.Domain.Common;

namespace TicketsPlease.Domain.Entities;

/// <summary>
/// Repräsentiert ein Ticket (Aufgabe) im Kanban-System.
/// Erbt von <see cref="BaseEntity"/> für die ID und Nebenläufigkeitskontrolle.
/// </summary>
public class Ticket : BaseEntity
{
    /// <summary>
    /// Ruft den Titel des Tickets ab oder legt diesen fest.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Ruft den SHA1-Hash zur eindeutigen Identifizierung ab oder legt diesen fest.
    /// </summary>
    public string Sha1Hash { get; set; } = string.Empty;

    /// <summary>
    /// Ruft die detaillierte Beschreibung des Tickets ab oder legt diese fest.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Ruft die Beschreibung im Markdown-Format ab oder legt diese fest.
    /// </summary>
    public string DescriptionMarkdown { get; set; } = string.Empty;

    /// <summary>
    /// Ruft den Status des Tickets ab oder legt diesen fest (z.B. Todo, Doing, Done).
    /// </summary>
    public string Status { get; set; } = "Todo";

    /// <summary>
    /// Ruft die ID der Priorität ab oder legt diese fest.
    /// </summary>
    public Guid PriorityId { get; set; }

    /// <summary>
    /// Navigations-Eigenschaft zur Priorität.
    /// </summary>
    public TicketPriority? Priority { get; set; }

    /// <summary>
    /// Ruft die Schwierigkeit ab (1-5 Chilis).
    /// </summary>
    public int ChilliesDifficulty { get; set; }

    /// <summary>
    /// Ruft den GeoIP-Zeitstempel für Audits ab oder legt diesen fest.
    /// </summary>
    public string GeoIpTimestamp { get; set; } = string.Empty;

    /// <summary>
    /// Ruft den geplanten Startzeitpunkt ab oder legt diesen fest.
    /// </summary>
    public DateTime? StartDate { get; set; }

    /// <summary>
    /// Ruft die Deadline ab oder legt diese fest.
    /// </summary>
    public DateTime? Deadline { get; set; }

    /// <summary>
    /// Ruft die ID des Workflow-Status ab oder legt diese fest.
    /// </summary>
    public Guid WorkflowStateId { get; set; }

    /// <summary>
    /// Navigations-Eigenschaft zum Workflow-Status.
    /// </summary>
    public WorkflowState? WorkflowState { get; set; }

    /// <summary>
    /// Ruft die ID des Erstellers ab oder legt diese fest.
    /// </summary>
    public Guid CreatorId { get; set; }

    /// <summary>
    /// Navigations-Eigenschaft zum Ersteller.
    /// </summary>
    public User? Creator { get; set; }

    /// <summary>
    /// Ruft den Erstellungszeitpunkt ab oder legt diesen fest.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Ruft den Zeitpunkt der letzten Änderung ab oder legt diesen fest.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Ruft die ID des Benutzers ab, dem das Ticket zugewiesen ist, oder legt diese fest (Nullable).
    /// </summary>
    public Guid? AssignedUserId { get; set; }

    /// <summary>
    /// Navigations-Eigenschaft zum zugewiesenen Benutzer.
    /// </summary>
    public User? AssignedUser { get; set; }
}
