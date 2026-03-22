---
trigger: model_decision
---

# 🎨 TicketsPlease - UI & Frontend Rules

<ui_rules>
<styling>

- Framework: TailwindCSS 4.2 ONLY. NO Bootstrap.
- Assets: LibMan ONLY (`wwwroot/lib/`). NO CDN.
- Inline Styles: STRICTLY FORBIDDEN. Use Tailwind or CSS variables.
- Colors: NO hardcoded colors. Use CSS Custom Properties (`--brand-primary`, `--color-surface`).
- Abstraction: Use `@apply` in `css/components/` (btn.css, cards.css, form.css, theme.css).
  </styling>
  <a11y>
    - Standard: BFSG / EN 301 549 (WCAG 2.1 AA+).
    - Navigation: Keyboard-only MUST work. Focus-traps in modals mandatory.
    - Semantics: Use `<main>`, `<nav>`, `<article>`, `<header>`, `<footer>`. 
    - Buttons: Use `<button>` or `<a role="button">`. NO `div` with click handlers.
    - Labels: `aria-label` for icons/empty buttons. `aria-describedby` for validation errors.
    - Motion: `prefers-reduced-motion` MUST be respected.
  </a11y>
  <theming>
- Variables: Extract to `theme.css`. Switch via `data-theme="dark"` on `<html>` (NO reload).
- Branding: Multi-Tenancy via `ICorporateSkinProvider`.
  </theming>
  <sfc_dry>
- ViewComponents: `Views/Shared/Components/`. Partials for repetition (`<partial name="_Avatar" />`).
- Logic: ZERO C# business logic in CSHTML.
  </sfc_dry>
  <icons>
- FontAwesome 7.2 (Local). `aria-hidden="true"` on decorative icons.
  </icons>
  <animations>
- Hover: `transition-all duration-200 ease-in-out`.
- Glass: `backdrop-blur` for modals/dropdowns.
- States: Hover, Focus, Active MUST be styled.
  </animations>
  </ui_rules>
