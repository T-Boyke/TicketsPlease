---
description: Workflow for creating consistent UI components using Tailwind CSS and the Corporate Skin Provider.
---

# 🎨 UI Component Development Workflow

Dieser Workflow beschreibt den vollständigen Ablauf zur Erstellung hochwertiger, barrierefreier UI-Komponenten im TicketsPlease Frontend.

> **Referenz:** [ADR-0005 (SFC/Tailwind)](file:///d:/DEV/Tickets/docs/adr/0005-ui-sfc-tailwind.md) | [ADR-0017 (No Bootstrap)](file:///d:/DEV/Tickets/docs/adr/0017-no-bootstrap-policy.md) | [ADR-0020 (Corporate Skinning)](file:///d:/DEV/Tickets/docs/adr/0020-corporate-skinning-provider.md) | [frontend_assets.md](file:///d:/DEV/Tickets/docs/frontend_assets.md) | [instructions.md §7-§8](file:///d:/DEV/Tickets/instructions.md)

---

## Fundamentale Regeln

| Regel | Status |
|---|---|
| **No Bootstrap** | ❌ Verboten. Nur TailwindCSS 4.2. |
| **No CDN** | ❌ Verboten. Alle Assets lokal via LibMan (`libman.json`). |
| **No Inline-Styles** | ❌ Verboten. Alles über Tailwind oder CSS-Variablen. |
| **No hardcoded Farben** | ❌ Verboten. Nutze `--brand-*` CSS Custom Properties. |

---

## Schritte

### 1. Analyse (DRY-Check)
- Prüfe, ob die Komponente bereits in ähnlicher Form existiert:
  - `src/TicketsPlease.Web/Views/Shared/Components/`
  - `src/TicketsPlease.Web/Views/Shared/` (Partials)
- **DRY-Regel:** Sobald sich ein HTML-Konstrukt mehr als einmal wiederholt → Partial oder TagHelper!
  - `<partial name="_Avatar" />`
  - `<partial name="_TicketTag" />`
  - Custom TagHelper: `<icon name="check" />`

### 2. Komponente erstellen
- **ViewComponent:** `src/TicketsPlease.Web/Views/Shared/Components/[Name]/`
  - `Default.cshtml` (Template)
  - `[Name]ViewComponent.cs` (Logik)
  - `[Name].cshtml.css` (CSS Isolation, optional)
- **Partial View:** `src/TicketsPlease.Web/Views/Shared/_[Name].cshtml`

### 3. Tailwind-Klassen anwenden

#### CSS-Variablen für Farben (Pflicht!)
```css
/* ✅ RICHTIG: CSS Custom Properties */
.btn-primary {
    @apply bg-[var(--brand-primary)] text-[var(--color-on-primary)];
}

/* ❌ FALSCH: Hardcoded Tailwind-Farben */
.btn-primary {
    @apply bg-blue-600 text-white;
}
```

#### Klassen-Reihenfolge (Konvention)
```
Layout → Box Model → Typography → Effects → Interactive
```

Beispiel:
```html
<button class="flex items-center px-4 py-2 text-sm font-medium rounded-lg shadow-md hover:shadow-lg transition-all duration-200">
```

#### Komplexe Interaktionen ohne JS
- Nutze `peer-*` und `group-*` für CSS-only Interaktionen.
- Beispiel: Dropdown ohne JavaScript via `peer-checked`.

### 4. `@apply` Abstraktion (Wiederkehrende Patterns)
Abstrahiere wiederkehrende UI-Muster in dedizierte CSS-Dateien:

| Datei | Inhalt |
|---|---|
| `css/components/btn.css` | Alle Button-Variationen (`btn-primary`, `btn-outline`, `btn-danger`) |
| `css/components/cards.css` | Kanban-Card Struktur, Ticket-Card |
| `css/components/form.css` | Inputs, Selects, Textareas, Validation States |
| `css/components/theme.css` | Color-Tokens, Typografie, Dark/Light Mode Variables |

```css
/* css/components/btn.css */
.btn {
    @apply inline-flex items-center justify-center px-4 py-2
           font-medium rounded-lg
           transition-all duration-200 ease-in-out
           focus:outline-none focus:ring-2 focus:ring-offset-2;
}

.btn-primary {
    @apply btn bg-[var(--brand-primary)] text-[var(--color-on-primary)]
           hover:brightness-110 active:brightness-90
           focus:ring-[var(--brand-primary)];
}
```

### 5. Responsive Design (Mobile-First)
- Baue Komponenten **Mobile-First** und erweitere mit Tailwind-Breakpoints:
  - `sm:` → `md:` → `lg:` → `xl:`
- Das Kanban-Board muss auf Mobile als **vertikale Listen/Karten** funktionieren.
- Teste auf beiden Viewports (Desktop Breitbild + Mobile).

### 6. Accessibility (BFSG/a11y) – Checkliste

| Prüfpunkt | Beschreibung |
|---|---|
| **`aria-label`** | Jedes interaktive Element ohne sichtbaren Text braucht ein `aria-label`. |
| **`aria-expanded`** | Für Dropdowns und Accordions (zeigt offenen/geschlossenen State). |
| **`aria-describedby`** | Für Formulare: Verknüpft Fehlermeldungen mit dem Input. |
| **Unique `id`** | Jedes interaktive Element braucht eine eindeutige `id` für automatisierte Tests. |
| **Keyboard Navigation** | `Tab`-Reihenfolge muss logisch sein. Kein Element darf übersprungen werden. |
| **Focus-Trap** | In Modals: Fokus darf das Modal nicht per Tab verlassen. |
| **Semantisches HTML** | `<button>` statt `<div onclick>`. `<dialog>` statt custom Modal. `<nav>` für Navigation. |
| **Kontrast** | Mindestens WCAG AA Kontrastverhältnis (4.5:1 für Text). |

```html
<!-- ✅ RICHTIG: Barrierefrei -->
<button id="create-ticket-btn"
        aria-label="Neues Ticket erstellen"
        class="btn btn-primary">
    <i class="fa-solid fa-plus" aria-hidden="true"></i>
    Neues Ticket
</button>

<!-- ❌ FALSCH: Nicht barrierefrei -->
<div onclick="createTicket()" class="bg-blue-500 p-2">
    <i class="fa-solid fa-plus"></i>
</div>
```

### 7. Theme-Switching (Dark/Light Mode)
- Farben **ausschließlich** über CSS Custom Properties in `theme.css`:
  ```css
  :root, [data-theme="light"] {
      --color-surface: #ffffff;
      --color-on-surface: #1a1a2e;
      --brand-primary: #6366f1;
  }

  [data-theme="dark"] {
      --color-surface: #1a1a2e;
      --color-on-surface: #e2e8f0;
      --brand-primary: #818cf8;
  }
  ```
- Theme-Wechsel via `data-theme` Attribut auf `<html>`-Tag. **Kein Page-Reload!**

### 8. Corporate Branding (Multi-Tenancy)
- Falls die Komponente dynamische Brand-Assets benötigt (Logos, Farben):
  - Injiziere `ICorporateSkinProvider` via DI.
  - Nutze die vom Provider gelieferten Werte für CSS-Variablen.

### 9. Icons (FontAwesome 7.2)
- Nutze ausschließlich lokale FontAwesome-Klassen:
  - `fa-solid fa-check` (Solid Icons)
  - `fa-regular fa-bell` (Regular Icons)
- **`aria-hidden="true"`** auf dekorativen Icons (die keinen eigenen Textinhalt kommunizieren).

### 10. Micro-Animations & Premium Feeling

| Element | Tailwind-Klassen |
|---|---|
| **Hover-Transition** | `transition-all duration-200 ease-in-out` |
| **Hover-Effekt** | `hover:brightness-110 hover:shadow-lg` |
| **Active-State** | `active:brightness-90 active:scale-95` |
| **Focus-Ring** | `focus:ring-2 focus:ring-offset-2 focus:ring-[var(--brand-primary)]` |
| **Glassmorphism** | `backdrop-blur-md bg-white/10 border border-white/20` (Modals, Dropdowns) |

---

*Checkliste: DRY-Check ✓ → Komponente ✓ → Tailwind ✓ → @apply ✓ → Responsive ✓ → a11y ✓ → Theme ✓ → Branding ✓ → Icons ✓ → Animations ✓*
