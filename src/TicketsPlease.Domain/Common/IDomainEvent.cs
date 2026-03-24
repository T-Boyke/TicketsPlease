// <copyright file="IDomainEvent.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
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
