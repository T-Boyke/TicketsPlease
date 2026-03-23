// <copyright file="TimeLog.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TicketsPlease.Domain.Entities;

using System;
using TicketsPlease.Domain.Common;

/// <summary>
/// Repräsentiert einen Zeiterfassungseintrag für die Arbeit an einem Ticket.
/// </summary>
public class TimeLog : BaseEntity
{
  /// <summary>
  /// Gets or sets die ID des zugehörigen Tickets.
  /// </summary>
  public Guid TicketId { get; set; }

  /// <summary>
  /// Gets or sets das Navigation-Property für das zugehörige Ticket.
  /// </summary>
  public Ticket? Ticket { get; set; }

  /// <summary>
  /// Gets or sets die ID des Benutzers, der die Zeit erfasst hat.
  /// </summary>
  public Guid UserId { get; set; }

  /// <summary>
  /// Gets or sets das Navigation-Property für den Benutzer, der die Zeit erfasst hat.
  /// </summary>
  public User? User { get; set; }

  /// <summary>
  /// Gets or sets den Startzeitpunkt (UTC) der Zeiterfassung.
  /// </summary>
  public DateTime StartedAt { get; set; }

  /// <summary>
  /// Gets or sets den (optionalen) Endzeitpunkt (UTC) der Zeiterfassung.
  /// </summary>
  public DateTime? StoppedAt { get; set; }

  /// <summary>
  /// Gets or sets die manuell oder automatisch berechnete gebuchte Zeit in Stunden.
  /// </summary>
  public decimal HoursLogged { get; set; }

  /// <summary>
  /// Gets or sets eine (optionale) Beschreibung oder Bemerkung zur gebuchten Zeit.
  /// </summary>
  public string? Description { get; set; }
}
