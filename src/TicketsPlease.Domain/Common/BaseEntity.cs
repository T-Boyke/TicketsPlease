using System.ComponentModel.DataAnnotations;

namespace TicketsPlease.Domain.Common;

/// <summary>
/// Stellt die Basisklasse für alle Domänen-Entitäten dar.
/// Enthält grundlegende Eigenschaften wie eine eindeutige ID und ein Feld für die Nebenläufigkeitskontrolle.
/// </summary>
public abstract class BaseEntity
{
    /// <summary>
    /// Ruft die eindeutige Identität der Entität ab oder legt diese fest.
    /// </summary>
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Ruft die Mandanten-ID ab, zu der diese Entität gehört, oder legt diese fest (Multi-Tenancy).
    /// </summary>
    public Guid TenantId { get; set; }

    /// <summary>
    /// Ruft einen Wert ab oder legt diesen fest, der angibt, ob die Entität gelöscht wurde (Soft-Delete).
    /// </summary>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// Ruft den Zeitpunkt des Soft-Deletes ab oder legt diesen fest.
    /// </summary>
    public DateTime? DeletedAt { get; set; }

    /// <summary>
    /// Dient der optimistischen Nebenläufigkeitskontrolle (Concurrency Control).
    /// Dieses Feld wird automatisch von SQL Server aktualisiert (Timestamp/RowVersion).
    /// </summary>
    [Timestamp]
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
}
