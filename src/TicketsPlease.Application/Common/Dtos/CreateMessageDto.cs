// <copyright file="CreateMessageDto.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Application.Common.Dtos;

using System;

/// <summary>
/// Datenübertragungsobjekt zum Erstellen einer Nachricht.
/// </summary>
public record CreateMessageDto(
    string BodyMarkdown,
    Guid? ReceiverUserId = null,
    Guid? TeamId = null,
    Guid? TicketId = null,
    Microsoft.AspNetCore.Http.IFormFile? Attachment = null);
