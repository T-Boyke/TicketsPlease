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
    /// Ruft den eindeutigen Login-Namen ab oder legt diesen fest.
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Ruft die E-Mail-Adresse des Benutzers ab oder legt diese fest.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Ruft den Passwort-Hash ab oder legt diesen fest.
    /// </summary>
    public string PasswordHash { get; set; } = string.Empty;

    /// <summary>
    /// Ruft den Erstellungszeitpunkt ab oder legt diesen fest.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Ruft den Zeitpunkt des letzten Logins ab oder legt diesen fest.
    /// </summary>
    public DateTime? LastLoginAt { get; set; }

    /// <summary>
    /// Ruft einen Wert ab oder legt diesen fest, der angibt, ob der Benutzer aktiv ist.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Ruft einen Wert ab oder legt diesen fest, der den Online-Status angibt.
    /// </summary>
    public bool IsOnline { get; set; }

    /// <summary>
    /// Ruft die ID der zugewiesenen Rolle ab oder legt diese fest.
    /// </summary>
    public Guid RoleId { get; set; }

    /// <summary>
    /// Navigations-Eigenschaft zur Rolle.
    /// </summary>
    public Role? Role { get; set; }
}
