using Microsoft.AspNetCore.Mvc;

namespace TicketsPlease.Web.Controllers;

/// <summary>
/// Controller für den UI-Styleguide.
/// Dient als Referenz für Entwickler, um einheitliche UI-Komponenten zu verwenden.
/// </summary>
public class StyleguideController : Controller
{
    /// <summary>
    /// Zeigt die Übersicht aller UI-Komponenten und Design-Tokens an.
    /// </summary>
    /// <returns>Die Styleguide-View.</returns>
    public IActionResult Index()
    {
        return View();
    }
}
