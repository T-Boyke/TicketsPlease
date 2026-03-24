// <copyright file="FileAsset.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Domain.Entities;

using System;
using TicketsPlease.Domain.Common;

/// <summary>
/// Repräsentiert eine hochgeladene Datei oder einen Anhang im System.
/// </summary>
public class FileAsset : BaseEntity
{
  /// <summary>
  /// Gets or sets den ursprünglichen Dateinamen.
  /// </summary>
  public string FileName { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets den MIME/Content-Type der Datei.
  /// </summary>
  public string ContentType { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets den Pfad oder URI, unter dem die Datei im Blob-Storage gespeichert ist.
  /// </summary>
  public string BlobPath { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets die Größe der Datei in Bytes.
  /// </summary>
  public long SizeBytes { get; set; }

  /// <summary>
  /// Gets or sets den Zeitpunkt (UTC), zu dem die Datei hochgeladen wurde.
  /// </summary>
  public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

  /// <summary>
  /// Gets or sets die ID des Benutzers, der die Datei hochgeladen hat.
  /// </summary>
  public Guid UploadedByUserId { get; set; }

  /// <summary>
  /// Gets or sets das Navigation-Property für den Benutzer, der die Datei hochgeladen hat.
  /// </summary>
  public User? UploadedByUser { get; set; }
}
