---
name: tailwind-component-patterns
description:
  Provides TailwindCSS 4.2 component patterns with @apply abstractions, CSS
  Custom Properties, Dark/Light mode, Glassmorphism, and BFSG/a11y compliance.
  Use when UI components, styling pages, or implementing theme-aware designs in
  the TicketsPlease project.
---

# 🎨 TailwindCSS Component Patterns

UI-Patterns mit TailwindCSS 4.2, Theme-Support und BFSG-konformer Accessibility.

> **Referenz:** [UI Rules](file:///d:/DEV/Tickets/.agent/rules/ui-frontend.md) |
> [/ui-component-tailwind](file:///d:/DEV/Tickets/.agent/workflows/ui-component-tailwind.md)

---

## Wann dieses Skill verwenden

- Neue UI-Komponente erstellen
- Styling nach Corporate Branding
- Dark/Light Mode implementieren
- Accessibility-konforme interaktive Elemente

---

## 🚫 Design Anti-Patterns (VERBOTEN)

> [!CAUTION] Generische AI-Ästhetik zerstört die Premium-Wirkung. Diese Patterns
> sind **verboten**:

| ❌ Verboten                          | Warum                      | ✅ Stattdessen                               |
| ------------------------------------ | -------------------------- | -------------------------------------------- |
| Inter, Roboto, Arial, System-Fonts   | Generisch, überall gesehen | Curated Fonts mit Charakter                  |
| Lila Gradients auf weißem Background | Cliché AI-Look             | Dominante Brand-Farben mit scharfen Akzenten |
| Symmetrische, gleichmäßige Layouts   | Vorhersehbar, langweilig   | Asymmetrie, Overlap, Diagonaler Flow         |
| Bootstrap, CDN-Links                 | Verboten per Projekt-Rules | TailwindCSS 4.2 lokal                        |
| Solide einfarbige Hintergründe       | Flach, keine Atmosphäre    | Gradient Meshes, Noise, Texturen, Grain      |
| `style="color: red"`                 | Inline = Chaos             | `text-[var(--color-error)]`                  |
| `<div onclick>`                      | Semantik-Verbrechen        | `<button>` mit Semantik                      |
| Nur Hover-State                      | Unvollständig              | Hover + Focus + Active States                |

---

## 🎭 Design-Philosophie

### Farbe & Theme

- **CSS Custom Properties** für Konsistenz über das gesamte System
- **Dominante Farben mit scharfen Akzenten** – Keine zaghaften, gleichmäßig
  verteilten Paletten
- Jede Farbentscheidung muss **bewusst und mutig** sein

### Motion & Animation

- **Ein orchestrierter Page-Load** mit gestaffelten Reveals (`animation-delay`)
  erzeugt mehr Wirkung als verstreute Micro-Interactions
- **CSS-only Lösungen** bevorzugen (Transitions, Keyframes, `@starting-style`)
- **Scroll-triggered Animationen** und **überraschende Hover-States**
- High-Impact Momente priorisieren, nicht streuen

### Räumliche Komposition

- **Asymmetrie** > Symmetrie
- **Overlap** – Elemente dürfen sich überlagern
- **Diagonaler Flow** – Nicht alles in starre Grid-Reihen
- **Grid-brechende Elemente** – Bewusste Regelbrüche
- **Großzügiger Negative Space** ODER **kontrollierte Dichte** – kein Mittelmaß

### Hintergründe & Visuelle Details

- **Atmosphäre und Tiefe** statt einfarbiger Flächen
- Kontextuelle Effekte, die zur Gesamt-Ästhetik passen:
  - Gradient Meshes, Noise Textures, Geometrische Patterns
  - Layered Transparencies, Dramatische Schatten
  - Dekorative Borders, Custom Cursors, Grain Overlays

---

## Design-System Architektur

```text
wwwroot/css/
├── theme.css          → Color Tokens, Typography, Dark/Light, Grain
├── animations.css     → Keyframes, Staggered Reveals, Scroll-Triggers
├── components/
│   ├── btn.css        → Button-Variationen mit überraschenden Hover-States
│   ├── cards.css      → Card/Kanban mit Overlap und Tiefe
│   ├── form.css       → Inputs, Selects, Validation-Feedback-Animationen
│   ├── modal.css      → Dialog mit Glassmorphism und Backdrop-Blur
│   ├── nav.css        → Navigation, Sidebar mit asymmetrischem Accent
│   └── table.css      → Datentabellen mit subtilen Zeilen-Animationen
└── app.css            → @import für alle Komponenten
```

---

## CSS Custom Properties (Theme Tokens)

```css
/* theme.css – Dominante Farben, scharfe Akzente */
:root {
  /* ═══════ Brand (Dominant + Sharp Accent) ═══════ */
  --brand-primary: oklch(0.55 0.22 260);
  --brand-primary-hover: oklch(0.48 0.24 260);
  --brand-accent: oklch(0.75 0.18 75); /* Scharfer Kontrast-Akzent */
  --brand-accent-glow: oklch(0.75 0.18 75 / 0.3);

  /* ═══════ Surfaces (Tiefe über Layering) ═══════ */
  --color-surface: oklch(0.985 0.002 260);
  --color-surface-elevated: oklch(0.97 0.005 260);
  --color-surface-overlay: oklch(0.99 0.002 260 / 0.85);
  --color-surface-glass: oklch(0.99 0.002 260 / 0.6);

  /* ═══════ Text (Hoher Kontrast) ═══════ */
  --color-text: oklch(0.15 0.01 260);
  --color-text-muted: oklch(0.45 0.02 260);
  --color-text-on-brand: oklch(0.98 0.005 260);

  /* ═══════ Borders & Shadows ═══════ */
  --color-border: oklch(0.88 0.01 260);
  --color-border-subtle: oklch(0.93 0.005 260);
  --shadow-elevation-1: 0 1px 3px oklch(0.2 0.02 260 / 0.08);
  --shadow-elevation-2: 0 4px 16px oklch(0.2 0.02 260 / 0.12);
  --shadow-elevation-3: 0 12px 40px oklch(0.2 0.02 260 / 0.18);
  --shadow-dramatic: 0 20px 60px oklch(0.15 0.03 260 / 0.25);

  /* ═══════ Feedback ═══════ */
  --color-success: oklch(0.55 0.18 155);
  --color-warning: oklch(0.7 0.16 75);
  --color-error: oklch(0.55 0.2 25);
  --color-info: oklch(0.6 0.15 230);

  /* ═══════ Atmosphere (Grain, Noise) ═══════ */
  --grain-opacity: 0.03;
  --gradient-mesh:
    radial-gradient(
      ellipse at 20% 50%,
      oklch(0.55 0.22 260 / 0.06) 0%,
      transparent 50%
    ),
    radial-gradient(
      ellipse at 80% 20%,
      oklch(0.75 0.18 75 / 0.04) 0%,
      transparent 50%
    );

  /* ═══════ Motion ═══════ */
  --ease-spring: cubic-bezier(0.34, 1.56, 0.64, 1);
  --ease-smooth: cubic-bezier(0.25, 0.1, 0.25, 1);
  --duration-fast: 150ms;
  --duration-normal: 250ms;
  --duration-slow: 400ms;
  --duration-dramatic: 700ms;
}

[data-theme="dark"] {
  --brand-primary: oklch(0.7 0.2 260);
  --brand-primary-hover: oklch(0.75 0.22 260);
  --brand-accent: oklch(0.8 0.16 75);
  --brand-accent-glow: oklch(0.8 0.16 75 / 0.25);

  --color-surface: oklch(0.13 0.015 260);
  --color-surface-elevated: oklch(0.17 0.02 260);
  --color-surface-overlay: oklch(0.15 0.015 260 / 0.9);
  --color-surface-glass: oklch(0.15 0.015 260 / 0.5);

  --color-text: oklch(0.92 0.01 260);
  --color-text-muted: oklch(0.6 0.02 260);

  --color-border: oklch(0.25 0.02 260);
  --color-border-subtle: oklch(0.2 0.015 260);

  --shadow-elevation-1: 0 1px 3px oklch(0.05 0.02 260 / 0.3);
  --shadow-elevation-2: 0 4px 16px oklch(0.05 0.02 260 / 0.4);
  --shadow-elevation-3: 0 12px 40px oklch(0.05 0.02 260 / 0.5);
  --shadow-dramatic: 0 20px 60px oklch(0.05 0.03 260 / 0.6);

  --grain-opacity: 0.05;
  --gradient-mesh:
    radial-gradient(
      ellipse at 20% 50%,
      oklch(0.7 0.2 260 / 0.08) 0%,
      transparent 50%
    ),
    radial-gradient(
      ellipse at 80% 20%,
      oklch(0.8 0.16 75 / 0.05) 0%,
      transparent 50%
    );
}
```

---

## Atmosphärische Hintergründe

### Grain Overlay (CSS-only)

```css
/* ═══════ Filmisches Grain über dem gesamten UI ═══════ */
.grain-overlay::after {
  content: "";
  position: fixed;
  inset: 0;
  z-index: 9999;
  pointer-events: none;
  opacity: var(--grain-opacity);
  background-image: url("data:image/svg+xml,%3Csvg viewBox='0 0 256 256' xmlns='http://www.w3.org/2000/svg'%3E%3Cfilter id='noise'%3E%3CfeTurbulence type='fractalNoise' baseFrequency='0.9' numOctaves='4' stitchTiles='stitch'/%3E%3C/filter%3E%3Crect width='100%25' height='100%25' filter='url(%23noise)'/%3E%3C/svg%3E");
}
```

### Gradient Mesh Background

```css
/* ═══════ Tiefe durch überlagerte Gradients ═══════ */
.mesh-bg {
  background: var(--gradient-mesh), var(--color-surface);
}
```

---

## Animation Patterns (`animations.css`)

### Staggered Page-Load Reveal

```css
/* ═══════ Orchestrierter Page-Load (High-Impact) ═══════ */
@keyframes reveal-up {
  from {
    opacity: 0;
    transform: translateY(24px) scale(0.98);
    filter: blur(4px);
  }
  to {
    opacity: 1;
    transform: translateY(0) scale(1);
    filter: blur(0);
  }
}

.reveal {
  animation: reveal-up var(--duration-dramatic) var(--ease-spring) both;
}

/* Gestaffelte Delays für orchestrierten Load */
.reveal-delay-1 {
  animation-delay: 100ms;
}
.reveal-delay-2 {
  animation-delay: 200ms;
}
.reveal-delay-3 {
  animation-delay: 350ms;
}
.reveal-delay-4 {
  animation-delay: 500ms;
}
.reveal-delay-5 {
  animation-delay: 700ms;
}
```

### Überraschende Hover-States

```css
/* ═══════ Hover mit Charakter (nicht nur opacity/color) ═══════ */
.hover-lift {
  transition:
    transform var(--duration-normal) var(--ease-spring),
    box-shadow var(--duration-normal) var(--ease-smooth);
}

.hover-lift:hover {
  transform: translateY(-4px) rotate(-0.5deg);
  box-shadow: var(--shadow-dramatic);
}

.hover-glow {
  transition: box-shadow var(--duration-normal) var(--ease-smooth);
}

.hover-glow:hover {
  box-shadow:
    var(--shadow-elevation-2),
    0 0 0 1px var(--brand-accent),
    0 0 20px var(--brand-accent-glow);
}
```

### Scroll-Triggered Animations

```css
/* ═══════ Intersection Observer triggered via CSS ═══════ */
.scroll-reveal {
  opacity: 0;
  transform: translateY(32px);
  transition:
    opacity var(--duration-dramatic) var(--ease-smooth),
    transform var(--duration-dramatic) var(--ease-spring);
}

.scroll-reveal.is-visible {
  opacity: 1;
  transform: translateY(0);
}
```

```javascript
// Intersection Observer (minimal JS, CSS handles the animation)
const observer = new IntersectionObserver(
  (entries) =>
    entries.forEach((entry) => {
      entry.target.classList.toggle("is-visible", entry.isIntersecting);
    }),
  { threshold: 0.15, rootMargin: "0px 0px -60px 0px" },
);
document
  .querySelectorAll(".scroll-reveal")
  .forEach((el) => observer.observe(el));
```

---

## Component Patterns

### Button (`btn.css`)

```css
/* ═══════ Basis ═══════ */
.btn {
  @apply inline-flex items-center justify-center gap-2 rounded-xl px-5 py-2.5 font-semibold focus:outline-none focus-visible:ring-2 focus-visible:ring-offset-2 disabled:pointer-events-none disabled:cursor-not-allowed disabled:opacity-40;
  transition:
    transform var(--duration-fast) var(--ease-spring),
    box-shadow var(--duration-normal) var(--ease-smooth),
    background-color var(--duration-fast) var(--ease-smooth);
}

/* ═══════ Primary – Dominante Brand-Farbe ═══════ */
.btn-primary {
  @apply btn text-[var(--color-text-on-brand)];
  background: var(--brand-primary);
  box-shadow: var(--shadow-elevation-1);
}

.btn-primary:hover {
  background: var(--brand-primary-hover);
  transform: translateY(-2px);
  box-shadow: var(--shadow-elevation-2);
}

.btn-primary:active {
  transform: translateY(0) scale(0.97);
  box-shadow: var(--shadow-elevation-1);
}

/* ═══════ Accent – Scharfer Kontrast ═══════ */
.btn-accent {
  @apply btn text-[var(--color-text)];
  background: var(--brand-accent);
  box-shadow:
    var(--shadow-elevation-1),
    0 0 0 0 var(--brand-accent-glow);
}

.btn-accent:hover {
  transform: translateY(-2px);
  box-shadow:
    var(--shadow-elevation-2),
    0 0 16px var(--brand-accent-glow);
}

/* ═══════ Ghost – Subtle mit Border ═══════ */
.btn-ghost {
  @apply btn;
  background: transparent;
  color: var(--color-text);
  border: 1px solid var(--color-border);
}

.btn-ghost:hover {
  background: var(--color-surface-elevated);
  border-color: var(--brand-primary);
  color: var(--brand-primary);
}

/* ═══════ Danger ═══════ */
.btn-danger {
  @apply btn text-[var(--color-text-on-brand)];
  background: var(--color-error);
}

.btn-danger:hover {
  filter: brightness(1.1);
  transform: translateY(-2px);
  box-shadow: var(--shadow-elevation-2);
}

/* ═══════ Sizes ═══════ */
.btn-sm {
  @apply rounded-lg px-3.5 py-1.5 text-sm;
}
.btn-lg {
  @apply rounded-2xl px-7 py-3.5 text-lg;
}
```

### Card (`cards.css`)

```css
/* ═══════ Card mit Tiefe und Hover-Surprise ═══════ */
.card {
  @apply overflow-hidden rounded-2xl;
  background: var(--color-surface);
  border: 1px solid var(--color-border-subtle);
  box-shadow: var(--shadow-elevation-1);
  transition:
    transform var(--duration-normal) var(--ease-spring),
    box-shadow var(--duration-normal) var(--ease-smooth),
    border-color var(--duration-fast) var(--ease-smooth);
}

.card:hover {
  transform: translateY(-6px) rotate(-0.3deg);
  box-shadow: var(--shadow-elevation-3);
  border-color: var(--color-border);
}

.card-header {
  @apply px-6 py-5;
  border-bottom: 1px solid var(--color-border-subtle);
}

.card-body {
  @apply px-6 py-5;
}

/* ═══════ Kanban-Card (Draggable, Accent Border) ═══════ */
.kanban-card {
  @apply card cursor-grab active:cursor-grabbing;
  border-left: 3px solid var(--brand-primary);
}

.kanban-card:hover {
  border-left-color: var(--brand-accent);
  box-shadow:
    var(--shadow-elevation-2),
    -4px 0 12px var(--brand-accent-glow);
}

/* ═══════ Feature Card (Asymmetrischer Accent) ═══════ */
.card-feature {
  @apply card relative;
}

.card-feature::before {
  content: "";
  position: absolute;
  top: 0;
  right: 0;
  width: 40%;
  height: 3px;
  background: linear-gradient(90deg, transparent, var(--brand-accent));
  border-radius: 0 1rem 0 0;
}
```

### Form (`form.css`)

```css
/* ═══════ Input mit Fokus-Glow ═══════ */
.form-input {
  @apply w-full rounded-xl px-4 py-3;
  background: var(--color-surface);
  border: 1px solid var(--color-border);
  color: var(--color-text);
  transition:
    border-color var(--duration-fast) var(--ease-smooth),
    box-shadow var(--duration-normal) var(--ease-smooth),
    transform var(--duration-fast) var(--ease-spring);
}

.form-input::placeholder {
  color: var(--color-text-muted);
}

.form-input:focus {
  outline: none;
  border-color: var(--brand-primary);
  box-shadow:
    0 0 0 3px oklch(0.55 0.22 260 / 0.12),
    var(--shadow-elevation-1);
  transform: translateY(-1px);
}

.form-label {
  @apply mb-2 block text-sm font-semibold;
  color: var(--color-text);
  letter-spacing: 0.01em;
}

/* ═══════ Validation States mit Animation ═══════ */
@keyframes shake {
  0%,
  100% {
    transform: translateX(0);
  }
  25% {
    transform: translateX(-6px);
  }
  75% {
    transform: translateX(6px);
  }
}

.form-input-error {
  border-color: var(--color-error);
  animation: shake 300ms var(--ease-spring);
  box-shadow: 0 0 0 3px oklch(0.55 0.2 25 / 0.1);
}

.form-input-error:focus {
  border-color: var(--color-error);
  box-shadow: 0 0 0 3px oklch(0.55 0.2 25 / 0.15);
}

.form-error-message {
  @apply mt-1.5 flex items-center gap-1.5 text-sm;
  color: var(--color-error);
  animation: reveal-up var(--duration-normal) var(--ease-spring);
}

/* ═══════ Success State ═══════ */
.form-input-success {
  border-color: var(--color-success);
  box-shadow: 0 0 0 3px oklch(0.55 0.18 155 / 0.1);
}
```

### Modal / Dialog (`modal.css`)

```css
/* ═══════ Modal mit Glassmorphism und Backdrop-Blur ═══════ */
.modal-overlay {
  @apply fixed inset-0 z-50 flex items-center justify-center;
  background: oklch(0.1 0.02 260 / 0.6);
  backdrop-filter: blur(12px) saturate(1.2);
  animation: fade-in var(--duration-normal) var(--ease-smooth);
}

@keyframes fade-in {
  from {
    opacity: 0;
  }
  to {
    opacity: 1;
  }
}

@keyframes modal-enter {
  from {
    opacity: 0;
    transform: translateY(32px) scale(0.95);
    filter: blur(4px);
  }
  to {
    opacity: 1;
    transform: translateY(0) scale(1);
    filter: blur(0);
  }
}

.modal-content {
  @apply mx-4 w-full max-w-lg overflow-hidden rounded-2xl;
  background: var(--color-surface-glass);
  backdrop-filter: blur(24px) saturate(1.3);
  border: 1px solid var(--color-border-subtle);
  box-shadow: var(--shadow-dramatic);
  animation: modal-enter var(--duration-slow) var(--ease-spring);
}

.modal-header {
  @apply flex items-center justify-between px-7 py-5;
  border-bottom: 1px solid var(--color-border-subtle);
}

.modal-body {
  @apply px-7 py-6;
}

.modal-footer {
  @apply flex justify-end gap-3 px-7 py-4;
  background: var(--color-surface-elevated);
  border-top: 1px solid var(--color-border-subtle);
}
```

---

## Accessibility Checkliste (BFSG)

| #   | Regel                 | Implementierung                                           |
| --- | --------------------- | --------------------------------------------------------- |
| 1   | **Keyboard-First**    | `tabindex`, `:focus-visible` Styles                       |
| 2   | **Focus-Traps**       | In Modals: Focus bleibt im Dialog                         |
| 3   | **Semantisches HTML** | `<dialog>`, `<nav>`, `<button>` statt `<div>`             |
| 4   | **ARIA**              | `aria-label`, `aria-expanded`, `aria-describedby`         |
| 5   | **Kontrast**          | WCAG AA (4.5:1 Text, 3:1 UI-Elemente)                     |
| 6   | **Unique IDs**        | Jedes interaktive Element: eindeutige `id`                |
| 7   | **Icons**             | Dekorativ: `aria-hidden="true"`, Funktional: `aria-label` |
| 8   | **Hover + Focus**     | Beide States definiert, nicht nur Hover                   |
| 9   | **Reduced Motion**    | `@media (prefers-reduced-motion: reduce)` respektieren    |

### Reduced Motion (Pflicht!)

```css
@media (prefers-reduced-motion: reduce) {
  *,
  *::before,
  *::after {
    animation-duration: 0.01ms !important;
    animation-iteration-count: 1 !important;
    transition-duration: 0.01ms !important;
  }
}
```

---

## Theme-Switch (Kein Page-Reload)

```html
<!-- Theme-Toggle Button mit überraschendem Hover -->
<button
  id="theme-toggle"
  aria-label="Farbschema wechseln"
  class="btn-ghost btn-sm hover-lift"
>
  <i class="fa-solid fa-moon" aria-hidden="true"></i>
</button>
```

```javascript
// Theme-Switch mit Transition
const toggle = document.getElementById("theme-toggle");
toggle.addEventListener("click", () => {
  const html = document.documentElement;
  // View Transition API für smooth switch
  if (document.startViewTransition) {
    document.startViewTransition(() => {
      const next = html.dataset.theme === "dark" ? "light" : "dark";
      html.dataset.theme = next;
      localStorage.setItem("theme", next);
    });
  } else {
    const next = html.dataset.theme === "dark" ? "light" : "dark";
    html.dataset.theme = next;
    localStorage.setItem("theme", next);
  }
});

// Init: Gespeichertes Theme oder System-Preference
document.documentElement.dataset.theme =
  localStorage.getItem("theme") ||
  (matchMedia("(prefers-color-scheme: dark)").matches ? "dark" : "light");
```

---

## Spatial Composition Patterns

### Asymmetrischer Hero

```html
<!-- Asymmetrisches Layout mit Overlap -->
<section class="mesh-bg relative grid min-h-[60vh] grid-cols-12 gap-0">
  <!-- Content: Bewusst nicht zentriert, links-lastig -->
  <div
    class="reveal reveal-delay-1 col-span-7 col-start-2 flex flex-col justify-center py-20"
  >
    <h1 class="text-5xl font-bold tracking-tight text-[var(--color-text)]">
      TicketsPlease
    </h1>
    <p class="mt-4 max-w-lg text-xl text-[var(--color-text-muted)]">
      Beschreibung hier
    </p>
  </div>

  <!-- Dekoratives Element: Diagonal, überlagert -->
  <div
    class="absolute top-0 right-0 h-full w-1/3 origin-top-right -skew-x-6 bg-gradient-to-bl from-[var(--brand-accent)]/10 to-transparent"
    aria-hidden="true"
  ></div>
</section>
```

### Grid-Breaking Accent

```css
/* Element das bewusst aus dem Grid ausbricht */
.grid-break {
  @apply relative -mx-4 px-4;
  /* Visuell breiter als Container */
  width: calc(100% + 2rem);
  background: linear-gradient(
    135deg,
    var(--brand-primary) 0%,
    var(--brand-accent) 100%
  );
}
```

---

### Skill: tailwind-component-patterns v2.0 – Premium Design System
