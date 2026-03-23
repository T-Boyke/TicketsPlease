// <copyright file="TicketType.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TicketsPlease.Domain.Enums;

/// <summary>
/// Definiert die verschiedenen Typen von Tickets im System.
/// </summary>
public enum TicketType
{
  /// <summary>
  /// Eine Standard-Aufgabe.
  /// </summary>
  Task = 0,

  /// <summary>
  /// Ein Software-Fehler.
  /// </summary>
  Bug = 1,

  /// <summary>
  /// Eine neue Funktionalität.
  /// </summary>
  Feature = 2,

  /// <summary>
  /// Ein übergeordnetes Thema (Epic), das mehrere Untertickets enthalten kann.
  /// </summary>
  Epic = 3,
}
