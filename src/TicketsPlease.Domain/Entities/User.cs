using TicketsPlease.Domain.Common;

namespace TicketsPlease.Domain.Entities;

/// <summary>
/// Repräsentiert einen Benutzer im Ticketsystem.
/// Erbt von <see cref="BaseEntity"/> für die ID und Nebenläufigkeitskontrolle.
/// </summary>
public class User : BaseEntity
{
    /// <summary>
    /// Ruft den Anzeigenamen des Benutzers ab oder legt diesen fest.
    /// </summary>
    public string DisplayName { get; set; } = string.Empty;

    /// <summary>
    /// Ruft die E-Mail-Adresse des Benutzers ab oder legt diese fest.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Ruft einen Wert ab oder legt diesen fest, der angibt, ob der Benutzer aktiv ist.
    /// </summary>
    public bool IsActive { get; set; } = true;
}
