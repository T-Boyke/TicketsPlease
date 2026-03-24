// <copyright file="IProjectRepository.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Application.Common.Interfaces;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TicketsPlease.Domain.Entities;

/// <summary>
/// Definiert die Datenzugriffsschicht für Projekte unter Berücksichtigung von Multi-Tenancy.
/// </summary>
public interface IProjectRepository
{
  /// <summary>
  /// Ruft ein Projekt anhand seiner ID ab.
  /// </summary>
  /// <param name="id">Die ID des Projekts.</param>
  /// <returns>Das gefundene Projekt oder null.</returns>
  public Task<Project?> GetByIdAsync(Guid id);

  /// <summary>
  /// Ruft alle Projekte einer Organisation (Mandant) ab.
  /// </summary>
  /// <param name="tenantId">Die ID der Organisation.</param>
  /// <returns>Eine Liste von Projekten.</returns>
  public Task<IEnumerable<Project>> GetAllAsync(Guid tenantId);

  /// <summary>
  /// Fügt ein neues Projekt hinzu.
  /// </summary>
  /// <param name="project">Das zu speichernde Projekt.</param>
  /// <returns>Die asynchrone Operation.</returns>
  public Task AddAsync(Project project);

  /// <summary>
  /// Aktualisiert ein bestehendes Projekt.
  /// </summary>
  /// <param name="project">Das zu aktualisierende Projekt.</param>
  /// <returns>Die asynchrone Operation.</returns>
  public Task UpdateAsync(Project project);

  /// <summary>
  /// Markiert ein Projekt als gelöscht oder entfernt es persistent.
  /// </summary>
  /// <param name="project">Das zu löschende Projekt.</param>
  /// <returns>Die asynchrone Operation.</returns>
  public Task DeleteAsync(Project project);
}
