# 🎨 TicketsPlease – UI/UX Design System

Vorgaben für das visuelle Frontend und die CSS-Architektur.

## 📋 Table of Contents

- [🖌️ CSS-Architektur](#-css-architektur)
- [🧩 Razor Philosophy](#-razor-philosophy)
- [♿ Accessibility (BFSG)](#-accessibility-bfsg)

---

## 🖌️ CSS-Architektur

Wir nutzen TailwindCSS 4.2 mit einer strukturierten Abstraktion:

- **Tokens:** CSS Custom Properties in `theme.css` (`--brand-primary`).
- **Komponenten:** `@apply` in dedizierten Dateien (`btn.css`, `cards.css`).
- **Theming:** Dark/Light Switch via `data-theme` Attribut auf `<html>`.

---

## 🧩 Razor Philosophy

- **SFC / ViewComponents:** Jede Komponente bündelt Template & Logik.
- **DRY:** Wiederkehrende Muster immer in `Partials` (`_Avatar.cshtml`).
- **Logic-Free:** CSHTML enthält keine Business-Logik.

---

## ♿ Accessibility (BFSG)

- **Keyboard-First:** Alle Komponenten sind per Tab bedienbar.
- **Semantic HTML:** Korrekte Nutzung von `<main>`, `<nav>`, `<dialog>`.
- **ARIA:** Strikte Nutzung von `aria-label` für Icons und Buttons.

---

_UIDesign v1.0 | 2026-03-09_
