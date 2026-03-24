// <copyright file="ICorporateSkinProvider.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Application.Common.Interfaces;

/// <summary>
/// Definiert die Schnittstelle für den Corporate Skin Provider.
/// Ermöglicht das Abrufen von branding-spezifischen Informationen wie Farben und Logos.
/// </summary>
public interface ICorporateSkinProvider
{
  /// <summary>
  /// Ruft die Primärfarbe für das Branding ab (hexadezimal oder CSS-Variable).
  /// </summary>
  /// <returns>Die Primärfarbe als String.</returns>
  public string GetPrimaryColor();

  /// <summary>
  /// Ruft die Sekundärfarbe für das Branding ab.
  /// </summary>
  /// <returns>Die Sekundärfarbe als String.</returns>
  public string GetSecondaryColor();

  /// <summary>
  /// Ruft den Namen oder Pfad des Firmenlogos ab.
  /// </summary>
  /// <returns>Den Dateinamen des Firmenlogos.</returns>
  public string GetLogoName();
}
