// <copyright file="IFileAssetRepository.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Application.Common.Interfaces;

using System;
using System.Threading.Tasks;
using TicketsPlease.Domain.Entities;

/// <summary>
/// Repository für den Zugriff auf Datei-Metadaten.
/// </summary>
public interface IFileAssetRepository
{
  /// <summary>
  /// Ruft eine Datei nach ID ab.
  /// </summary>
  /// <param name="id">Die ID.</param>
  /// <returns>Die Datei.</returns>
  public Task<FileAsset?> GetByIdAsync(Guid id);

  /// <summary>
  /// Fügt eine neue Datei hinzu.
  /// </summary>
  /// <param name="fileAsset">Die Datei.</param>
  /// <returns>Ein Task.</returns>
  public Task AddAsync(FileAsset fileAsset);

  /// <summary>
  /// Löscht eine Datei aus der DB.
  /// </summary>
  /// <param name="fileAsset">Die zu löschende Datei.</param>
  /// <returns>Ein Task.</returns>
  public Task DeleteAsync(FileAsset fileAsset);

  /// <summary>
  /// Speichert Änderungen.
  /// </summary>
  /// <returns>Ein Task.</returns>
  public Task SaveChangesAsync();
}
