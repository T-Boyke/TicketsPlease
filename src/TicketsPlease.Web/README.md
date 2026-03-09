# 🔵 TicketsPlease.Web – Die Präsentation

Dieser Layer ist für die Interaktion mit dem Benutzer zuständig. Er umfasst das
Web-Frontend, die API-Endpunkte und das gesamte UI/UX-Design.

## 🍴 Git Branch

- **Branch:** `layer/web`
- Alle Änderungen am Web-Layer müssen auf diesem Branch erfolgen.

## 📋 Arbeitsanweisungen für Web-Entwickler

### 1. Thin Controllers

- Controller dienen nur als Einstiegspunkt.
- Delegiere jegliche Logik sofort an den Application-Layer via **MediatR**.
- Rückgabewerte sind in der Regel `View()` oder `IActionResult`.

### 2. Frontend Excellence (Tailwind CSS 4.2)

- Nutze das **lokale Tailwind-CLI** (kein Bootstrap!).
- Abstrahiere wiederkehrende Styles mit `@apply` in den entsprechenden
  Komponenten-CSS-Dateien (`btn.css`, `cards.css`, etc.).
- Halte CSHTML-Dateien sauber von Business-Logik.

### 3. UI/UX & Barrierefreiheit (a11y)

- Entwickle nach dem **BFSG**-Standard.
- Nutze semantisches HTML5 und korrekte `aria-`-Attribute.
- Gewährleiste vollständige Keyboard-Navigation.

### 4. Theme-Switching

- Das System unterstützt Dark- und Light-Mode nativ über CSS-Variablen in `theme.css`.
- Nutze den `ICorporateSkinProvider` für dynamische Branding-Anpassungen.

## 📁 Struktur

- `Controllers/`: MVC & API Controller.
- `Views/`: Razor Views und Partials.
- `Views/Shared/Components/`: Wiederverwendbare ViewComponents.
- `wwwroot/`: Statische Dateien (JS-Libs, Bilder).
- `css/components/`: Komponenten-spezifische CSS-Styles.

---

## 🔗 Connectors

- **Application Layer:** Wird über MediatR Commands/Queries konsumiert.
- **Frontend Assets:** Lokale Verwaltung via `libman.json`.

> [!IMPORTANT]
> Nutze DOMPurify für alle Markdown-Outputs, um XSS-Angriffe zu verhindern!
