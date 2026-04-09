// <copyright file="UserProfileDto.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Web.Models.Account;

using System;
using System.Collections.Generic;
using TicketsPlease.Application.Common.Dtos;

/// <summary>
/// DTO für die detaillierte Profilanzeige.
/// </summary>
public record UserProfileDto(
    Guid UserId,
    string Username,
    string Email,
    string FirstName,
    string LastName,
    string? Bio,
    string? PhoneNumber,
    string? AvatarUrl,
    string? Position,
    string? TechStack,
    string? Street,
    string? HouseNumber,
    string? City,
    string? Country,
    string RoleName,
    DateTime CreatedAt,
    DateTime? LastLoginAt,
    bool IsOnline,
    string? OrganizationName,
    IEnumerable<string> TeamNames,
    PerformanceDetailDto Performance);
