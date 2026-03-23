// <copyright file="BaseAuditableEntity.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TicketsPlease.Domain.Common;

using System;

/// <summary>
/// Erweitert <see cref="BaseEntity"/> um Felder für die automatische Überprüfung (Auditing).
/// </summary>
public abstract class BaseAuditableEntity : BaseEntity
{
    /// <summary>
    /// Gets or sets den Zeitpunkt der Erstellung.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets die ID oder den Namen des Benutzers, der die Entität erstellt hat.
    /// </summary>
    public string? CreatedBy { get; set; }

    /// <summary>
    /// Gets or sets den Zeitpunkt der letzten Änderung.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Gets or sets die ID oder den Namen des Benutzers, der die letzte Änderung vorgenommen hat.
    /// </summary>
    public string? UpdatedBy { get; set; }
}
