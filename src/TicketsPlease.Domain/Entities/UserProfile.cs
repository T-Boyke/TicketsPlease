// <copyright file="UserProfile.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
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
  public User? User { get; set; }

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
  public string? AvatarUrl { get; set; }

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


}
