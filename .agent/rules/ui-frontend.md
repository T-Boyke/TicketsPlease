# 🎨 TicketsPlease – UI & Frontend Rules

Regeln für Frontend-Entwicklung, Barrierefreiheit und Styling.

---

## TailwindCSS & Styling

- **No Bootstrap** – Komplett verboten. Nur TailwindCSS 4.2.
- **No CDN** – Alle Assets lokal via LibMan (`libman.json` → `wwwroot/lib/`).
- **No Inline-Styles** – Alles über Tailwind oder CSS-Variablen.
- **No Hardcoded Farben** – Nutze CSS Custom Properties (`--brand-primary`, `--color-surface`).
- **`@apply`** – Wiederkehrende Patterns in `css/components/` abstrahieren:
  - `btn.css` – Button-Variationen
  - `cards.css` – Kanban-Card Struktur
  - `form.css` – Inputs, Selects, Validation States
  - `theme.css` – Color-Tokens, Typografie, Dark/Light Mode

## Accessibility (BFSG / a11y)

- **Keyboard-First** – Alles per `Tab` bedienbar.
- **Focus-Traps** – In Modals Pflicht.
- **Semantisches HTML5** – `<dialog>`, `<nav>`, `<main>`, `<button>` (nicht `<div onclick>`).
- **ARIA-Attribute** – `aria-label`, `aria-expanded`, `aria-describedby` wo nötig.
- **Unique `id`** – Jedes interaktive Element hat eine eindeutige `id`.
- **Kontrast** – WCAG AA Minimum (4.5:1).

## Theme-Switching

- CSS Custom Properties in `theme.css`.
- Theme-Wechsel via `data-theme="dark"` auf `<html>`. Kein Page-Reload.
- `ICorporateSkinProvider` für Multi-Tenancy Branding.

## SFC & DRY

- ViewComponents in `Views/Shared/Components/`.
- Partials für wiederkehrende Konstrukte: `<partial name="_Avatar" />`.
- Kein C# Business-Logic Code im CSHTML.

## Icons

- FontAwesome 7.2 (lokal): `fa-solid`, `fa-regular`.
- `aria-hidden="true"` auf dekorativen Icons.

## Micro-Animations

- `transition-all duration-200 ease-in-out` für Hover.
- `backdrop-blur` für Glassmorphism (Modals, Dropdowns).
- Hover-, Focus- und Active-States sind Pflicht.

---

## TicketsPlease UI Rules v1.0 | 2026-03-06
