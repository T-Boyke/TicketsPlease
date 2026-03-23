// <copyright file="WorkflowTransition.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TicketsPlease.Domain.Entities;

using System;
using TicketsPlease.Domain.Common;

/// <summary>
/// Repräsentiert einen erlaubten Zustandsübergang in einem Ticket-Workflow.
/// </summary>
public class WorkflowTransition : BaseEntity
{
  /// <summary>
  /// Gets or sets die ID des Ausgangszustands.
  /// </summary>
  public Guid FromStateId { get; set; }

  /// <summary>
  /// Gets or sets das Navigation-Property für den Ausgangszustand.
  /// </summary>
  public WorkflowState? FromState { get; set; }

  /// <summary>
  /// Gets or sets die ID des Zielzustands.
  /// </summary>
  public Guid ToStateId { get; set; }

  /// <summary>
  /// Gets or sets das Navigation-Property für den Zielzustand.
  /// </summary>
  public WorkflowState? ToState { get; set; }

  /// <summary>
  /// Gets or sets die (optionale) Rollen-ID, um den Übergang auf eine bestimmte Rolle zu beschränken.
  /// </summary>
  public Guid? AllowedRoleId { get; set; }

  /// <summary>
  /// Gets or sets das Navigation-Property für die erlaubte Rolle.
  /// </summary>
  public Role? AllowedRole { get; set; }
}
