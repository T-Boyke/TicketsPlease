using TicketsPlease.Application.Common.Interfaces;

namespace TicketsPlease.Infrastructure.Services;

/// <summary>
/// Standard-Implementierung des <see cref="ICorporateSkinProvider"/>.
/// Liefert initiale Standard-Branding-Werte. In einer Multi-Tenant-Umgebung würde dies
/// kontextabhängig (z.B. über die Subdomain oder User-Settings) agieren.
/// </summary>
public class DefaultCorporateSkinProvider : ICorporateSkinProvider
{
    private const string DefaultPrimary = "#3b82f6";   // Tailwind Blue 500
    private const string DefaultSecondary = "#1e40af"; // Tailwind Blue 900

    /// <inheritdoc />
    public string GetPrimaryColor() => DefaultPrimary;

    /// <inheritdoc />
    public string GetSecondaryColor() => DefaultSecondary;

    /// <inheritdoc />
    public string GetLogoName() => "TicketsPlease";
}
