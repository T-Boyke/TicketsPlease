// <copyright file="TicketLinkDto.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Application.Common.Dtos;

using System;
using TicketsPlease.Domain.Enums;

/// <summary>
/// Datenübertragungsobjekt für eine Ticket-Verknüpfung (F7).
/// </summary>
/// <param name="Id">Die ID der Verknüpfung.</param>
/// <param name="SourceTicketId">Die ID des Quell-Tickets.</param>
/// <param name="SourceTicketTitle">Der Titel des Quell-Tickets.</param>
/// <param name="TargetTicketId">Die ID des Ziel-Tickets.</param>
/// <param name="TargetTicketTitle">Der Titel des Ziel-Tickets.</param>
/// <param name="LinkType">Der Typ der Verknüpfung.</param>
/// <param name="IsClosed">Gibt an, ob das verknüpfte Ticket geschlossen ist.</param>
public record TicketLinkDto(
    Guid Id,
    Guid SourceTicketId,
    string SourceTicketTitle,
    Guid TargetTicketId,
    string TargetTicketTitle,
    TicketLinkType LinkType,
    bool IsClosed);
