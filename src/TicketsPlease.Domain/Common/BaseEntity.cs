// <copyright file="BaseEntity.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
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
#pragma warning disable CA1819 // Properties should not return arrays
  [Timestamp]
  public byte[] RowVersion { get; set; } = Array.Empty<byte>();
#pragma warning restore CA1819 // Properties should not return arrays

  /// <summary>
  /// Eine Liste von Domain-Events, die von dieser Entität ausgelöst wurden.
  /// </summary>
  private readonly List<object> domainEvents = new();

  /// <summary>
  /// Gets die Liste der Domain-Events (Read-Only).
  /// </summary>
  public IReadOnlyCollection<object> DomainEvents => this.domainEvents.AsReadOnly();

  /// <summary>
  /// Fügt ein Domain-Event hinzu.
  /// </summary>
  /// <param name="domainEvent">Das hinzuzufügende Event.</param>
  public void AddDomainEvent(object domainEvent) => this.domainEvents.Add(domainEvent);

  /// <summary>
  /// Entfernt ein Domain-Event.
  /// </summary>
  /// <param name="domainEvent">Das zu entfernende Event.</param>
  public void RemoveDomainEvent(object domainEvent) => this.domainEvents.Remove(domainEvent);

  /// <summary>
  /// Leert die Liste der Domain-Events.
  /// </summary>
  public void ClearDomainEvents() => this.domainEvents.Clear();
}
