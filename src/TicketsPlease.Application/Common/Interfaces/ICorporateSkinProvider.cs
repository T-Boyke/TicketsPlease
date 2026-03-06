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
    string GetPrimaryColor();

    /// <summary>
    /// Ruft die Sekundärfarbe für das Branding ab.
    /// </summary>
    string GetSecondaryColor();

    /// <summary>
    /// Ruft den Namen oder Pfad des Firmenlogos ab.
    /// </summary>
    string GetLogoName();
}
