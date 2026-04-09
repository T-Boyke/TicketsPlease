// <copyright file="OrganizationDto.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Application.Common.Dtos;

using System;

/// <summary>
/// DTO für eine Organisation/Workspace.
/// </summary>
/// <param name="Id">Die ID.</param>
/// <param name="Name">Der Name.</param>
/// <param name="SubscriptionLevel">Das Abo-Level.</param>
/// <param name="IsActive">Status.</param>
public record OrganizationDto(
    Guid Id,
    string Name,
    string SubscriptionLevel,
    bool IsActive);

/// <summary>
/// DTO zum Erstellen/Bearbeiten einer Organisation.
/// </summary>
/// <param name="Name">Name.</param>
/// <param name="SubscriptionLevel">Level.</param>
/// <param name="IsActive">Status.</param>
public record UpsertOrganizationDto(
    string Name,
    string SubscriptionLevel,
    bool IsActive);
