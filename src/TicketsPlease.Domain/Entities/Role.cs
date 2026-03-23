// <copyright file="Role.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TicketsPlease.Domain.Entities;

using System;
using Microsoft.AspNetCore.Identity;
using TicketsPlease.Domain.Common;

/// <summary>
/// Repräsentiert eine Benutzerrolle im System zur Berechtigungssteuerung.
/// Erbt von <see cref="IdentityRole{TKey}"/> für ASP.NET Core Identity Integration.
/// </summary>
public class Role : IdentityRole<Guid>, IBaseEntity
{
  /// <summary>
  /// Gets or sets die detaillierte Beschreibung der Rolle und ihrer Berechtigungen.
  /// </summary>
  public string Description { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets die Mandanten-ID (Multi-Tenancy).
  /// </summary>
  public Guid TenantId { get; set; }

  /// <summary>
  /// Gets or sets a value indicating whether die Entität gelöscht wurde (Soft Delete).
  /// </summary>
  public bool IsDeleted { get; set; }

  /// <summary>
  /// Gets or sets den Zeitpunkt des Soft-Deletes.
  /// </summary>
  public DateTime? DeletedAt { get; set; }

  /// <summary>
  /// Gets or sets die Version für die Nebenläufigkeitskontrolle.
  /// </summary>
  public byte[] RowVersion { get; set; } = Array.Empty<byte>();

  /// <summary>
  /// Eine Liste von Domain-Events, die von dieser Entität ausgelöst wurden.
  /// </summary>
  private readonly List<IDomainEvent> domainEvents = new();

  /// <summary>
  /// Gets die Liste der Domain-Events (Read-Only).
  /// </summary>
  public IReadOnlyCollection<IDomainEvent> DomainEvents => this.domainEvents.AsReadOnly();

  /// <summary>
  /// Fügt ein Domain-Event hinzu.
  /// </summary>
  /// <param name="domainEvent">Das hinzuzufügende Event.</param>
  public void AddDomainEvent(IDomainEvent domainEvent) => this.domainEvents.Add(domainEvent);

  /// <summary>
  /// Entfernt ein Domain-Event.
  /// </summary>
  /// <param name="domainEvent">Das zu entfernende Event.</param>
  public void RemoveDomainEvent(IDomainEvent domainEvent) => this.domainEvents.Remove(domainEvent);

  /// <summary>
  /// Leert die Liste der Domain-Events.
  /// </summary>
  public void ClearDomainEvents() => this.domainEvents.Clear();
}
