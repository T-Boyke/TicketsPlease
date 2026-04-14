ui_frontend_rules: styling: framework: "TailwindCSS 4.2.2 only (No Bootstrap)" asset_management:
"LibMan only (wwwroot/lib/); no CDN" inline_styles: "Strictly forbidden; use Tailwind or CSS
variables" colors: "No hardcoded colors; use CSS Custom Properties (--brand-primary,
--color-surface)" abstraction_pattern: "Use @apply in css/components/ (btn.css, cards.css, form.css,
theme.css)" accessibility_a11y: standards: "BFSG / EN 301 549 (WCAG 2.1 AA+)" navigation:
"Keyboard-only navigation must work; focus-traps in modals are mandatory" semantics: "Use HTML5
semantic tags: <main>, <nav>, <article>, <header>, <footer>" interaction: "Use <button> or
<a role='button'>; avoid <div> with click handlers" labeling: "aria-label for icons/empty buttons;
aria-describedby for validation errors" motion: "prefers-reduced-motion must be respected" theming:
implementation: "Extract variables to theme.css; switch via data-theme='dark' on <html> without
reloads" branding: "Multi-Tenancy via ICorporateSkinProvider" component_structure_sfc: structure:
"ViewComponents in Views/Shared/Components/; use Partials for repetition" logic_separation: "Zero C#
business logic in CSHTML files" icons: library: "FontAwesome 7.2 (Local)" usage: "aria-hidden='true'
on decorative icons" animations_ux: hover_defaults: "transition-all duration-200 ease-in-out"
aesthetic_effects: "backdrop-blur for modals/dropdowns (Glassmorphism)" state_styling: "Hover,
Focus, and Active states must be styled"
