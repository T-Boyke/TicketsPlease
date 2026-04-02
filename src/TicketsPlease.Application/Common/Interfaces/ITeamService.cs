// <copyright file="ITeamService.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Application.Common.Interfaces;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TicketsPlease.Application.Common.Dtos;

/// <summary>
/// Definiert die Geschäftslogik für das Teammanagement.
/// </summary>
public interface ITeamService
{
  /// <summary>
  /// Ruft alle Teams ab, in denen der Benutzer Mitglied ist.
  /// </summary>
  /// <param name="userId">Die ID des Benutzers.</param>
  /// <param name="cancellationToken">Das Abbruchtoken.</param>
  /// <returns>Die Teamliste als DTOs.</returns>
  public Task<IEnumerable<TeamDto>> GetUserTeamsAsync(Guid userId, CancellationToken cancellationToken = default);

  /// <summary>
  /// Ruft Detailinformationen zu einem spezifischen Team ab.
  /// </summary>
  /// <param name="teamId">Die ID des Teams.</param>
  /// <param name="cancellationToken">Das Abbruchtoken.</param>
  /// <returns>Das Team-DTO oder null.</returns>
  public Task<TeamDto?> GetTeamDetailsAsync(Guid teamId, CancellationToken cancellationToken = default);

  /// <summary>
  /// Ruft alle Teams im System ab (für Administrative Zwecke).
  /// </summary>
  /// <param name="cancellationToken">Das Abbruchtoken.</param>
  /// <returns>Die vollständige Liste der Team-DTOs.</returns>
  public Task<IEnumerable<TeamDto>> GetAllTeamsAsync(CancellationToken cancellationToken = default);

  /// <summary>
  /// Erstellt ein neues Team im System.
  /// </summary>
  /// <param name="name">Der Name des Teams.</param>
  /// <param name="description">Die Beschreibung des Teams.</param>
  /// <param name="colorCode">Der Farbcode der UI-Darstellung.</param>
  /// <param name="creatorUserId">Die ID des Ersteller-Benutzers.</param>
  /// <param name="cancellationToken">Das Abbruchtoken.</param>
  /// <returns>Die ID des neu erstellten Teams.</returns>
  public Task<Guid> CreateTeamAsync(string name, string description, string colorCode, Guid creatorUserId, CancellationToken cancellationToken = default);

  /// <summary>
  /// Fügt ein Mitglied zu einem Team hinzu.
  /// </summary>
  /// <param name="teamId">Die ID des Teams.</param>
  /// <param name="userId">Die ID des Benutzers.</param>
  /// <param name="isTeamLead">Gibt an, ob das Mitglied als Leiter fungiert.</param>
  /// <param name="cancellationToken">Das Abbruchtoken.</param>
  /// <returns>Die asynchrone Operation.</returns>
  public Task AddMemberAsync(Guid teamId, Guid userId, bool isTeamLead = false, CancellationToken cancellationToken = default);

  /// <summary>
  /// Entfernt ein Mitglied aus einem Team.
  /// </summary>
  /// <param name="teamId">Die ID des Teams.</param>
  /// <param name="userId">Die ID des Benutzers.</param>
  /// <param name="cancellationToken">Das Abbruchtoken.</param>
  /// <returns>Die asynchrone Operation.</returns>
  public Task RemoveMemberAsync(Guid teamId, Guid userId, CancellationToken cancellationToken = default);

  /// <summary>
  /// Markiert ein Team als gelöscht.
  /// </summary>
  /// <param name="teamId">Die ID des Teams.</param>
  /// <returns>Die asynchrone Operation.</returns>
  public Task DeleteTeamAsync(Guid teamId);
}
