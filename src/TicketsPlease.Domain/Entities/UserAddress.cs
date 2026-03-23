// <copyright file="UserAddress.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TicketsPlease.Domain.Entities;

using System;
using TicketsPlease.Domain.Common;

/// <summary>
/// Repräsentiert die postalische Adresse eines Benutzers.
/// </summary>
public class UserAddress : BaseAuditableEntity
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
  /// Gets or sets den Namen der Straße / Hausnummer.
  /// </summary>
  public string Street { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets den Ortsnamen / die Stadt.
  /// </summary>
  public string City { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets die Postleitzahl.
  /// </summary>
  public string ZipCode { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets das Land.
  /// </summary>
  public string Country { get; set; } = string.Empty;

}
