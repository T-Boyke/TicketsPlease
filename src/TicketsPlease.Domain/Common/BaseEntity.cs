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
    /// Dient der optimistischen Nebenläufigkeitskontrolle (Concurrency Control).
    /// Dieses Feld wird automatisch von SQL Server aktualisiert (Timestamp/RowVersion).
    /// </summary>
    [Timestamp]
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
}
