// <copyright file="TicketTemplateDto.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Application.Common.Dtos;

using System;

/// <summary>
/// Data Transfer Object für eine Ticket-Vorlage.
/// </summary>
/// <param name="Id">Eindeutige ID.</param>
/// <param name="Name">Anzeigename des Templates.</param>
/// <param name="DescriptionMarkdownTemplate">Markdown-Inhalt der Vorlage.</param>
/// <param name="DefaultPriorityId">Optimale Standard-Priorität.</param>
/// <param name="DefaultPriorityName">Name der Standard-Priorität.</param>
public record TicketTemplateDto(
    Guid Id,
    string Name,
    string DescriptionMarkdownTemplate,
    Guid? DefaultPriorityId,
    string? DefaultPriorityName);

/// <summary>
/// DTO zum Erstellen einer neuen Ticket-Vorlage.
/// </summary>
/// <param name="Name">Name der Vorlage.</param>
/// <param name="DescriptionMarkdownTemplate">Der Markdown-Body.</param>
/// <param name="DefaultPriorityId">Optionale Priorität.</param>
public record CreateTicketTemplateDto(
    string Name,
    string DescriptionMarkdownTemplate,
    Guid? DefaultPriorityId);
