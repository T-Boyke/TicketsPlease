// <copyright file="User.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TicketsPlease.Domain.Entities;

using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using TicketsPlease.Domain.Common;

/// <summary>
/// Repräsentiert einen Benutzer im Ticketsystem.
/// Erbt von <see cref="IdentityUser{TKey}"/> für ASP.NET Core Identity Integration.
/// </summary>
public class User : IdentityUser<Guid>, IBaseEntity
{
  /// <summary>
  /// Gets or sets den Login-Namen (Alias for UserName).
  /// </summary>
  [NotMapped]
  public string Username { get => this.UserName ?? string.Empty; set => this.UserName = value; }

  /// <summary>
  /// Gets or sets den Erstellungszeitpunkt.
  /// </summary>
  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

  /// <summary>
  /// Gets or sets den Zeitpunkt des letzten Logins.
  /// </summary>
  public DateTime? LastLoginAt { get; set; }

  /// <summary>
  /// Gets or sets a value indicating whether der Benutzer aktiv ist.
  /// </summary>
  public bool IsActive { get; set; } = true;

  /// <summary>
  /// Gets or sets a value indicating whether der das den Online-Status angibt.
  /// </summary>
  public bool IsOnline { get; set; }

  /// <summary>
  /// Gets or sets den Fremdschlüssel zur Organisation (Mandantenfähigkeit).
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
  /// Gets or sets die Version für die Nebenläufigkeitskontrolle.
  /// </summary>
  public byte[] RowVersion { get; set; } = Array.Empty<byte>();

  /// <summary>
  /// Gets or sets die ID der zugewiesenen Rolle.
  /// </summary>
  public Guid RoleId { get; set; }

  /// <summary>
  /// Gets or sets das Navigation-Property zur Rolle.
  /// </summary>
  public Role? Role { get; set; }

  /// <summary>
  /// Gets or sets das zugehörige Benutzerprofil (1:1).
  /// </summary>
  public virtual UserProfile? Profile { get; set; }

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
