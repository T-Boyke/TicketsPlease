// <copyright file="ErrorViewModel.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Web.Models;

/// <summary>
/// Modell für die Anzeige von Fehlermeldungen.
/// </summary>
public sealed class ErrorViewModel
{
  /// <summary>
  /// Gets or sets die ID der Anforderung.
  /// </summary>
  public string? RequestId { get; set; }

  /// <summary>
  /// Gets a value indicating whether die Anforderungs-ID angezeigt werden soll.
  /// </summary>
  public bool ShowRequestId => !string.IsNullOrEmpty(this.RequestId);
}
