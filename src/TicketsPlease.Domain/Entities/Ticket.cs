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
    /// Ruft die detaillierte Beschreibung des Tickets ab oder legt diese fest (unterstützt Markdown).
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Ruft den Status des Tickets ab oder legt diesen fest (z.B. Todo, Doing, Done).
    /// </summary>
    public string Status { get; set; } = "Todo";

    /// <summary>
    /// Ruft die Priorität des Tickets ab oder legt diese fest.
    /// </summary>
    public int Priority { get; set; } = 0;

    /// <summary>
    /// Ruft die ID des Benutzers ab, dem das Ticket zugewiesen ist, oder legt diese fest.
    /// </summary>
    public Guid? AssignedUserId { get; set; }

    /// <summary>
    /// Navigations-Eigenschaft zum zugewiesenen Benutzer.
    /// </summary>
    public User? AssignedUser { get; set; }
}
