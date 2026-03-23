// <copyright file="IDomainEvent.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TicketsPlease.Domain.Common;

using System;

/// <summary>
/// Definiert die Schnittstelle für alle Domänenereignisse.
/// Domänenereignisse stellen wichtige Änderungen im Geschäftszustand dar.
/// </summary>
public interface IDomainEvent
{
  /// <summary>
  /// Gets ruft den Zeitpunkt ab, zu dem das Ereignis eingetreten ist (UTC).
  /// </summary>
  public DateTime OccurredOn { get; }
}
