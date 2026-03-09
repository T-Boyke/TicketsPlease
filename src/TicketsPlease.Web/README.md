# 🔵 TicketsPlease.Web – Die Präsentation

Dieser Layer ist für die Interaktion mit dem Benutzer zuständig. Er umfasst das Web-Frontend,
die API-Endpunkte und das UI/UX-Design.

## 🍴 Git Branch

- **Branch:** `layer/web`
- Alle Änderungen am Web-Layer müssen auf diesem Branch erfolgen.

---

## 📋 Arbeitsanweisungen: Wie erstelle ich ein Feature im Web?

### 1. Thin Controllers (Standard)

Controller dürfen **keine** Logik enthalten. Sie nehmen Daten an und senden sie an MediatR.

```csharp
public class TicketsController : Controller {
    private readonly ISender _sender;
    public TicketsController(ISender sender) => _sender = sender;

    [HttpPost]
    public async Task<IActionResult> Create(CreateTicketCommand command, CancellationToken ct) {
        var id = await _sender.Send(command, ct);
        return RedirectToAction(nameof(Details), new { id });
    }
}
```

### 2. Frontend Workflow (Tailwind CSS 4.2)

Wir nutzen ein node-freies Build-System. Alles wird über die `TailwindCSS.MSBuild` gesteuert.

- **CSS Abstraktion**: Schreib keine langen Utility-Ketten in HTML. Nutze `@apply` in `css/components/`.
- **Naming**: Klassen-Ketten in Razor-Dateien sollten lesbar bleiben.

### 3. Sicherheit (Pflicht)

- **XSS**: Alle User-generierten Inhalte (z.B. Markdown) **müssen** mit `DOMPurify` im
  JavaScript gesäubert werden.
- **CSRF**: Nutze `[ValidateAntiForgeryToken]` für alle POST/PUT/DELETE Aktionen.

---

## 🛠️ Dependency Injection (DI) Connector

Die Registrierung der Web-Dienste erfolgt in:

- **Ort**: `Program.cs` / `DependencyInjection.cs`
- **Wichtig**: Hier werden Controller, ViewComponents und der `ICorporateSkinProvider` konfiguriert.

---

## 📁 Struktur

- `Controllers/`: Dünne Brücken zur Application Layer.
- `Views/`: Razor-Templates (SFC - Single File Component Style angestrebt).
- `css/components/`: Abstrahierte UI-Styles (Cards, Buttons, Layout).
- `wwwroot/`: Alle statischen Assets (lokal via LibMan verwaltet).

---

## 🔗 Connectors

- **Application Layer:** Konsumiert Use Cases via MediatR.
- **Infrastructure Layer:** Konsumiert Konfigurationen für Auth & Identity.

> [!IMPORTANT]
> Keine Logik in Views! Wenn eine View ein `if` oder eine Schleife braucht, die über UI-Zustand
> hinausgeht, gehört das in die Application Layer.
