# 🎨 TicketsPlease - UI/UX Design System

<ui_ux>
  <css_architecture>
    - Framework: TailwindCSS 4.2.2.
    - Tokens: CSS Custom Properties in `theme.css`.
    - Components: `@apply` in `src/TicketsPlease.Web/Tailwind/components/`.
    - Theming: `data-theme` on `<html>`.
  </css_architecture>
  <design_language>
    - Aesthetics: Premium, Bold, Asymmetric.
    - Colors: Dominant brand colors with high-contrast sharp accents.
    - Effects: Glassmorphism, Gradient Meshes, Grain Overlays.
    - Typography: Modern, high-character fonts (Google Fonts).
  </design_language>
  <razor_philosophy>
    - SFC: ViewComponents bundle Template + UI Logic.
    - DRY: Use Partials (`_Avatar.cshtml`).
    - Logic-Free: ZERO C# business logic in CSHTML.
  </razor_philosophy>
  <a11y>
    - BFSG Compliance: Keyboard-first, Focus-traps, Screen-reader ready.
    - HTML5: Semantic (`<nav>`, `<main>`, `<dialog>`).
    - ARIA: `aria-label`, `aria-hidden`, `aria-expanded` strictly used.
  </a11y>
</ui_ux>
