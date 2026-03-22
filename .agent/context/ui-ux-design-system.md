# 🎨 TicketsPlease - UI/UX Design System

<ui_ux>
<css_architecture>

- Framework: TailwindCSS 4.2.
- Tokens: CSS Custom Properties in `theme.css`.
- Components: `@apply` in `btn.css`, `cards.css`.
- Theming: `data-theme` on `<html>`.
  </css_architecture>
  <razor_philosophy>
- SFC: ViewComponents bundle Template + UI Logic.
- DRY: Use Partials (`_Avatar.cshtml`).
- Logic-Free: ZERO C# business logic in CSHTML.
  </razor_philosophy>
  <a11y>
- Usability: Keyboard-first (Tab).
- HTML5: Semantic (`<nav>`, `<main>`, `<dialog>`).
- ARIA: `aria-label` for icons/buttons.
  </a11y>
  </ui_ux>
