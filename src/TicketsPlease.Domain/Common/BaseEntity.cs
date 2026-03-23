// <copyright file="BaseEntity.cs" company="TicketsPlease">
// Copyright (c) TicketsPlease. All rights reserved.
// </copyright>

namespace TicketsPlease.Domain.Common;

using System;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// Stellt die Basisklasse für alle Domänen-Entitäten dar.
/// Enthält grundlegende Eigenschaften wie eine eindeutige ID und ein Feld für die Nebenläufigkeitskontrolle.
/// </summary>
public abstract class BaseEntity
{
    /// <summary>
    /// Gets or sets die eindeutige Identität der Entität.
    /// </summary>
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Gets or sets die Mandanten-ID, zu der diese Entität gehört (Multi-Tenancy).
    /// </summary>
    public Guid TenantId { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether die Entität gelöscht wurde (Soft-Delete).
    /// </summary>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// Gets or sets den Zeitpunkt des Soft-Deletes.
    /// </summary>
    public DateTime? DeletedAt { get; set; }

    /// <summary>
    /// Gets or sets das Feld zur optimistischen Nebenläufigkeitskontrolle (Concurrency Control).
    /// Dieses Feld wird automatisch von SQL Server aktualisiert (Timestamp/RowVersion).
    /// </summary>
    [Timestamp]
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
}
