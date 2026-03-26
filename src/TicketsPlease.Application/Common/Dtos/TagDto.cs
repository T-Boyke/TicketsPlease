// <copyright file="TagDto.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Application.Common.Dtos;

using System;

/// <summary>
/// Datenübertragungsobjekt für einen Tag (Schlagwort).
/// </summary>
/// <param name="Id">Die ID des Tags.</param>
/// <param name="Name">Der Name des Tags.</param>
/// <param name="ColorHex">Die Hex-Farbe für die Anzeige.</param>
public record TagDto(Guid Id, string Name, string ColorHex);
