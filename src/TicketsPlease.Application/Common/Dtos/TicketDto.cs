// <copyright file="TicketDto.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Application.Common.Dtos;

using System;
using TicketsPlease.Domain.Enums;

/// <summary>
/// Datenübertragungsobjekt für ein Ticket (F3).
/// </summary>
/// <param name="Id">Die eindeutige ID des Tickets.</param>
/// <param name="Title">Der Titel des Tasks.</param>
/// <param name="Description">Die detaillierte Beschreibung.</param>
/// <param name="Status">Der aktuelle Status (z.B. Todo, Doing, Closed).</param>
/// <param name="ProjectId">Die ID des zugehörigen Projekts.</param>
/// <param name="ProjectTitle">Der Name des Projekts (für die UI).</param>
/// <param name="AssignedUserId">Die ID des zugewiesenen Benutzers.</param>
/// <param name="AssignedUserName">Der Name des zugewiesenen Benutzers.</param>
/// <param name="Type">Der Ticket-Typ (Task, Bug, etc.).</param>
/// <param name="Priority">Die Prioritätsdetails.</param>
/// <param name="CreatedAt">Der Erstellungszeitpunkt.</param>
/// <param name="EstimatePoints">Die geschätzten Story Points.</param>
public record TicketDto(
    Guid Id,
    string Title,
    string Description,
    string Status,
    Guid ProjectId,
    string ProjectTitle,
    Guid? AssignedUserId,
    string AssignedUserName,
    TicketType Type,
    TicketPriorityDto Priority,
    DateTime CreatedAt,
    int? EstimatePoints);

/// <summary>
/// Datenübertragungsobjekt für Ticket-Prioritäten.
/// </summary>
/// <param name="Id">Die ID der Priorität.</param>
/// <param name="Name">Der Anzeigename.</param>
/// <param name="ColorHex">Der Hex-Farbcode für die UI.</param>
public record TicketPriorityDto(
    Guid Id,
    string Name,
    string ColorHex);
