// <copyright file="IBaseEntity.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TicketsPlease.Domain.Common;

using System;
using System.Collections.Generic;

/// <summary>
/// Definiert die Basiseigenschaften für alle Domänen-Entitäten.
/// Ermöglicht eine konsistente Behandlung von Entitäten, auch wenn diese
/// von externen Klassen (wie IdentityUser) erben müssen.
/// </summary>
public interface IBaseEntity
{
    /// <summary>
    /// Gets die eindeutige Identität der Entität.
    /// </summary>
    Guid Id { get; }

    /// <summary>
    /// Gets die Mandanten-ID, zu der diese Entität gehört.
    /// </summary>
    Guid TenantId { get; }

    /// <summary>
    /// Gets a value indicating whether die Entität gelöscht wurde.
    /// </summary>
    bool IsDeleted { get; }

    /// <summary>
    /// Gets den Zeitpunkt des Soft-Deletes.
    /// </summary>
    DateTime? DeletedAt { get; }

    /// <summary>
    /// Gets die Version für die Nebenläufigkeitskontrolle.
    /// </summary>
    byte[] RowVersion { get; }

    /// <summary>
    /// Gets die Liste der Domänenereignisse.
    /// </summary>
    IReadOnlyCollection<IDomainEvent> DomainEvents { get; }

    /// <summary>
    /// Fügt ein Domänenereignis hinzu.
    /// </summary>
    /// <param name="domainEvent">Das Ereignis.</param>
    void AddDomainEvent(IDomainEvent domainEvent);

    /// <summary>
    /// Entfernt ein Domänenereignis.
    /// </summary>
    /// <param name="domainEvent">Das Ereignis.</param>
    void RemoveDomainEvent(IDomainEvent domainEvent);

    /// <summary>
    /// Leert die Domänenereignisse.
    /// </summary>
    void ClearDomainEvents();
}
