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
/// <param name="SlaCheckIntervalMinutes">Check interval in minutes.</param>
/// <param name="QuietHoursStart">Quiet hours start time.</param>
/// <param name="QuietHoursEnd">Quiet hours end time.</param>
/// <param name="TimeZoneId">Organization timezone ID.</param>
/// <param name="NotifyOnLow">Notify on low priority SLA breaches.</param>
/// <param name="NotifyOnMedium">Notify on medium priority SLA breaches.</param>
/// <param name="NotifyOnHigh">Notify on high priority SLA breaches.</param>
/// <param name="NotifyOnBlocker">Notify on blocker priority SLA breaches.</param>
public record OrganizationDto(
    Guid Id,
    string Name,
    string SubscriptionLevel,
    bool IsActive,
    int SlaCheckIntervalMinutes,
    TimeSpan? QuietHoursStart,
    TimeSpan? QuietHoursEnd,
    string TimeZoneId,
    bool NotifyOnLow,
    bool NotifyOnMedium,
    bool NotifyOnHigh,
    bool NotifyOnBlocker);

/// <summary>
/// DTO zum Erstellen/Bearbeiten einer Organisation.
/// </summary>
/// <param name="Name">Name.</param>
/// <param name="SubscriptionLevel">Level.</param>
/// <param name="IsActive">Status.</param>
/// <param name="SlaCheckIntervalMinutes">Check interval in minutes.</param>
/// <param name="QuietHoursStart">Quiet hours start time.</param>
/// <param name="QuietHoursEnd">Quiet hours end time.</param>
/// <param name="TimeZoneId">Organization timezone ID.</param>
/// <param name="NotifyOnLow">Notify on low priority SLA breaches.</param>
/// <param name="NotifyOnMedium">Notify on medium priority SLA breaches.</param>
/// <param name="NotifyOnHigh">Notify on high priority SLA breaches.</param>
/// <param name="NotifyOnBlocker">Notify on blocker priority SLA breaches.</param>
public record UpsertOrganizationDto(
    string Name,
    string SubscriptionLevel,
    bool IsActive,
    int SlaCheckIntervalMinutes,
    TimeSpan? QuietHoursStart,
    TimeSpan? QuietHoursEnd,
    string TimeZoneId,
    bool NotifyOnLow,
    bool NotifyOnMedium,
    bool NotifyOnHigh,
    bool NotifyOnBlocker);
