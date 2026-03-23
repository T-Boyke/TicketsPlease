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
  public Guid Id { get; }

  /// <summary>
  /// Gets die Mandanten-ID, zu der diese Entität gehört.
  /// </summary>
  public Guid TenantId { get; }

  /// <summary>
  /// Gets a value indicating whether die Entität gelöscht wurde.
  /// </summary>
  public bool IsDeleted { get; }

  /// <summary>
  /// Gets den Zeitpunkt des Soft-Deletes.
  /// </summary>
  public DateTime? DeletedAt { get; }

  /// <summary>
  /// Gets die Version für die Nebenläufigkeitskontrolle.
  /// </summary>
#pragma warning disable CA1819 // Properties should not return arrays
  public byte[] RowVersion { get; }
#pragma warning restore CA1819 // Properties should not return arrays

  /// <summary>
  /// Gets die Liste der Domänenereignisse.
  /// </summary>
  public IReadOnlyCollection<IDomainEvent> DomainEvents { get; }

  /// <summary>
  /// Fügt ein Domänenereignis hinzu.
  /// </summary>
  /// <param name="domainEvent">Das Ereignis.</param>
  public void AddDomainEvent(IDomainEvent domainEvent);

  /// <summary>
  /// Entfernt ein Domänenereignis.
  /// </summary>
  /// <param name="domainEvent">Das Ereignis.</param>
  public void RemoveDomainEvent(IDomainEvent domainEvent);

  /// <summary>
  /// Leert die Domänenereignisse.
  /// </summary>
  public void ClearDomainEvents();
}
