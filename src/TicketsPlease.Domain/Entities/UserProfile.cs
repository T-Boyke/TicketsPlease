// <copyright file="UserProfile.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Domain.Entities;

using System;
using TicketsPlease.Domain.Common;

/// <summary>
/// Repräsentiert das detaillierte Profil eines Benutzers mit persönlichen Daten.
/// </summary>
public class UserProfile : BaseAuditableEntity
{
  /// <summary>
  /// Gets or sets die ID des zugehörigen Benutzers.
  /// </summary>
  public Guid UserId { get; set; }

  /// <summary>
  /// Gets or sets das Navigation-Property für den zugehörigen Benutzer.
  /// </summary>
  public virtual User? User { get; set; }

  /// <summary>
  /// Gets or sets den Vornamen des Benutzers.
  /// </summary>
  public string FirstName { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets den Nachnamen des Benutzers.
  /// </summary>
  public string LastName { get; set; } = string.Empty;

  /// <summary>
  /// Gets den vollständigen Namen des Benutzers.
  /// </summary>
  public string FullName => $"{this.FirstName} {this.LastName}".Trim();

  /// <summary>
  /// Gets or sets die Biographie des Benutzers.
  /// </summary>
  public string? Bio { get; set; }

  /// <summary>
  /// Gets or sets die URL zum Avatar-Bild.
  /// </summary>
  public Uri? AvatarUrl { get; set; }

  /// <summary>
  /// Gets or sets die (optionale) Telefonnummer des Benutzers.
  /// </summary>
  public string? PhoneNumber { get; set; }

  /// <summary>
  /// Gets or sets die (optionale) ID des hochgeladenen Profilbilds (Avatar).
  /// </summary>
  public Guid? AvatarImageId { get; set; }

  /// <summary>
  /// Gets or sets das Navigation-Property für das Profilbild als FileAsset.
  /// </summary>
  public FileAsset? AvatarImage { get; set; }

  /// <summary>
  /// Gets or sets die berufliche Position.
  /// </summary>
  public string? Position { get; set; }

  /// <summary>
  /// Gets or sets den Tech-Stack (z.B. kommagetrennt).
  /// </summary>
  public string? TechStack { get; set; }

  /// <summary>
  /// Gets or sets die Straße der Anschrift.
  /// </summary>
  public string? Street { get; set; }

  /// <summary>
  /// Gets or sets die Hausnummer der Anschrift.
  /// </summary>
  public string? HouseNumber { get; set; }

  /// <summary>
  /// Gets or sets die Stadt der Anschrift.
  /// </summary>
  public string? City { get; set; }

  /// <summary>
  /// Gets or sets das Land der Anschrift.
  /// </summary>
  public string? Country { get; set; }

  /// <summary>
  /// Gets or sets das Aktualisierungsintervall für das Kanban-Board in Millisekunden.
  /// </summary>
  public int KanbanUpdateIntervalMs { get; set; } = 30000;

  /// <summary>
  /// Gets or sets a value indicating whether Animationen reduziert werden sollen (Performance).
  /// </summary>
  public bool ReduceAnimations { get; set; }

  /// <summary>
  /// Gets or sets den Namen des ausgewählten Benachrichtigungstons.
  /// </summary>
  public string NotificationSound { get; set; } = "Default";

  /// <summary>
  /// Gets or sets a value indicating whether E-Mail-Benachrichtigungen aktiviert sind.
  /// </summary>
  public bool EmailNotificationsEnabled { get; set; } = true;

  /// <summary>
  /// Gets or sets den SLA-Schwellenwert in Stunden für dieses Profil/Organisation-Kontext.
  /// </summary>
  public int SlaThresholdHours { get; set; } = 4;
}
