// <copyright file="MessageDto.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Application.Common.Dtos;

using System;

/// <summary>
/// Datenübertragungsobjekt für Nachrichten.
/// </summary>
public record MessageDto(
    Guid Id,
    Guid SenderUserId,
    string SenderUserName,
    Guid? ReceiverUserId,
    string? ReceiverUserName,
    string BodyMarkdown,
    DateTime SentAt);
