// <copyright file="ITeamRepository.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Application.Common.Interfaces;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TicketsPlease.Domain.Entities;

/// <summary>
/// Definiert die Datenzugriffsschicht für das Teammanagement.
/// </summary>
public interface ITeamRepository
{
  /// <summary>
  /// Ruft ein Team anhand der ID ab.
  /// </summary>
  /// <param name="id">Die ID des Teams.</param>
  /// <param name="cancellationToken">Das Abbruchtoken.</param>
  /// <returns>Das gefundene Team oder null.</returns>
  public Task<Team?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

  /// <summary>
  /// Ruft alle Teams ab, in denen ein Benutzer Mitglied ist.
  /// </summary>
  /// <param name="userId">Die ID des Benutzers.</param>
  /// <param name="cancellationToken">Das Abbruchtoken.</param>
  /// <returns>Eine Liste von Teams.</returns>
  public Task<IEnumerable<Team>> GetTeamsByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);

  /// <summary>
  /// Ruft alle im System verfügbaren Teams ab.
  /// </summary>
  /// <param name="cancellationToken">Das Abbruchtoken.</param>
  /// <returns>Eine Liste von Teams.</returns>
  public Task<IEnumerable<Team>> GetAllTeamsAsync(CancellationToken cancellationToken = default);

  /// <summary>
  /// Fügt ein neues Team hinzu.
  /// </summary>
  /// <param name="team">Das Teamobjekt.</param>
  /// <param name="cancellationToken">Das Abbruchtoken.</param>
  /// <returns>Das hinzugefügte Team.</returns>
  public Task AddAsync(Team team, CancellationToken cancellationToken = default);

  /// <summary>
  /// Aktualisiert ein bestehendes Team.
  /// </summary>
  /// <param name="team">Das Teamobjekt.</param>
  /// <param name="cancellationToken">Das Abbruchtoken.</param>
  /// <returns>Das aktualisierte Team.</returns>
  public Task UpdateAsync(Team team, CancellationToken cancellationToken = default);

  /// <summary>
  /// Entfernt ein Team aus dem System.
  /// </summary>
  /// <param name="team">Das Teamobjekt.</param>
  /// <returns>Die asynchrone Operation.</returns>
  public Task DeleteAsync(Team team);

  /// <summary>
  /// Speichert alle Änderungen asynchron.
  /// </summary>
  /// <param name="cancellationToken">Das Abbruchtoken.</param>
  /// <returns>Die Anzahl der betroffenen Zeilen.</returns>
  public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
