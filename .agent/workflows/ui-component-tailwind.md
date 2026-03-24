ui_component_development_workflow:
  objective: "Standard procedure for creating high-quality, accessible UI components"
  fundamental_rules:
    no_bootstrap: "Strictly forbidden; use TailwindCSS 4.2 only"
    no_cdn: "All assets must be local via LibMan (libman.json)"
    no_inline_styles: "Use Tailwind classes or CSS variables only"
    no_hardcoded_colors: "Use --brand-* CSS Custom Properties in theme.css"
  steps:
    analysis_dry_check:
      action: "Check Views/Shared/Components/ or Partials before creating"
      dry_rule: "Create Partial, TagHelper, or ViewComponent if HTML repeats"
    component_creation:
      view_component: ["Default.cshtml", "[Name]ViewComponent.cs", "[Name].cshtml.css (Isolation)"]
      partial: "src/TicketsPlease.Web/Views/Shared/_[Name].cshtml"
    tailwind_application:
      css_variables: "Use @apply bg-[var(--brand-primary)] text-[var(--color-on-primary)]"
      class_order: "Layout -> Box Model -> Typography -> Effects -> Interactive"
      css_logic: "Use peer-* and group-* for CSS-only interactions (e.g., JS-free dropdowns)"
    abstraction:
      file_mapping:
        btn.css: "Button variations"
        cards.css: "Kanban and Ticket structures"
        form.css: "Inputs, validation states"
        theme.css: "Color tokens, Typography, Dark/Light modes"
    responsive_design:
      principle: "Mobile-First via Tailwind breakpoints (sm:, md:, lg:, xl:)"
      kanban: "Board must collapse to vertical lists on mobile"
    accessibility_a11y:
      checklist:
        - "aria-label for textless buttons/icons"
        - "aria-expanded for dropdowns/accordions"
        - "aria-describedby for form validation"
        - "Unique id for every interactive element"
        - "Logical keyboard tab-order; focus-traps in modals"
        - "Semantic HTML (<button>, <dialog>, <nav>)"
        - "i18n: Use @L['Key'] via IViewLocalizer"
        - "BFSG (EN 301 549): Strict accessibility compliance"
        - "Contrast: Minimum WCAG AA (4.5:1)"
    theming_branding:
      switching: "data-theme on <html>; no reload; CSS Variables only"
      skins: "ICorporateSkinProvider via DI for dynamic assets"
    icons:
      library: "FontAwesome 7.2 (Local)"
      rule: "aria-hidden='true' for decorative icons"
    animations_ux:
      hover: "transition-all duration-200 ease-in-out; brightness-110; shadow-lg"
      active: "brightness-90; scale-95"
      focus: "ring-2 ring-offset-2 ring-[var(--brand-primary)]"
      glass: "backdrop-blur-md (modals, dropdowns)"
