// <copyright file="TicketTemplate.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TicketsPlease.Domain.Entities;

using System;
using TicketsPlease.Domain.Common;

/// <summary>
/// Repräsentiert eine Vorlage zur schnelleren Erstellung von standardisierten Tickets.
/// </summary>
public class TicketTemplate : BaseEntity
{
  /// <summary>
  /// Gets or sets den Namen der Ticket-Vorlage.
  /// </summary>
  public string Name { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets die Markdown-Vorlage für die Beschreibung des Tickets.
  /// </summary>
  public string DescriptionMarkdownTemplate { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets die (optionale) Standard-Prioritäts-ID für Tickets, die aus dieser Vorlage erstellt werden.
  /// </summary>
  public Guid? DefaultPriorityId { get; set; }

  /// <summary>
  /// Gets or sets das Navigation-Property für die Standard-Priorität.
  /// </summary>
  public TicketPriority? DefaultPriority { get; set; }

  /// <summary>
  /// Gets or sets die ID des Benutzers, der die Vorlage erstellt hat.
  /// </summary>
  public Guid CreatorId { get; set; }

  /// <summary>
  /// Gets or sets das Navigation-Property für den Ersteller der Vorlage.
  /// </summary>
  public User? Creator { get; set; }
}
