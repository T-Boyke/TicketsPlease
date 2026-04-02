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
/// <param name="Status">Der aktuelle Status (z.B. To-Do, Doing, Closed).</param>
/// <param name="ProjectId">Die ID des zugehörigen Projekts.</param>
/// <param name="ProjectTitle">Der Name des Projekts (für die UI).</param>
/// <param name="AssignedUserId">Die ID des zugewiesenen Benutzers.</param>
/// <param name="AssignedUserName">Der Name des zugewiesenen Benutzers.</param>
/// <param name="Type">Der Ticket-Typ (Task, Bug, etc.).</param>
/// <param name="Priority">Die Prioritätsdetails.</param>
/// <param name="CreatedAt">Der Erstellungszeitpunkt.</param>
/// <param name="EstimatePoints">Die geschätzten Story Points.</param>
/// <param name="Comments">Die Liste der Kommentare zum Ticket (F5).</param>
/// <param name="BlockedBy">Tickets, die dieses Ticket blockieren (F7).</param>
/// <param name="Blocking">Tickets, die von diesem Ticket blockiert werden (F7).</param>
#pragma warning disable CA1819 // Properties should not return arrays (RowVersion for EF Core)
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
    int? EstimatePoints,
    int ChilliesDifficulty,
    IEnumerable<TagDto> Tags,
    IEnumerable<TimeLogDto> TimeLogs,
    IEnumerable<SubTicketDto> SubTickets,
    bool IsTimerRunning,
    IEnumerable<CommentDto> Comments,
    IEnumerable<TicketLinkDto> BlockedBy,
    IEnumerable<TicketLinkDto> Blocking,
    IEnumerable<FileAssetDto> Attachments,
    IEnumerable<TicketHistoryDto> History,
    int UpvoteCount,
    bool UserHasUpvoted,
    byte[] RowVersion);
#pragma warning restore CA1819 // Properties should not return arrays
