// <copyright file="IOrganizationInviteService.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Application.Common.Interfaces;

using System;
using System.Threading.Tasks;
using TicketsPlease.Application.Common.Dtos;

/// <summary>
/// Schnittstelle für den Dienst zur Verwaltung von Organisationseinladungen.
/// </summary>
public interface IOrganizationInviteService
{
  /// <summary>
  /// Erstellt eine neue Einladung für eine Organisation.
  /// </summary>
  /// <param name="organizationId">Die ID der Organisation.</param>
  /// <param name="targetedEmail">Die optionale Ziel-E-Mail.</param>
  /// <param name="expiryDays">Gültigkeitsdauer in Tagen.</param>
  /// <returns>Das DTO der erstellten Einladung.</returns>
  Task<OrganizationInviteDto> CreateInviteAsync(Guid organizationId, string? targetedEmail = null, int expiryDays = 7);

  /// <summary>
  /// Validiert einen Einladungs-Token.
  /// </summary>
  /// <param name="token">Der Token GUID.</param>
  /// <returns>Das DTO, falls gültig, sonst null.</returns>
  Task<OrganizationInviteDto?> ValidateTokenAsync(Guid token);

  /// <summary>
  /// Markiert einen Token als verwendet.
  /// </summary>
  /// <param name="token">Der Token GUID.</param>
  /// <param name="userId">Der Benutzer, der den Token verwendet hat.</param>
  /// <returns>Ein Task.</returns>
  Task MarkAsUsedAsync(Guid token, Guid userId);
}
