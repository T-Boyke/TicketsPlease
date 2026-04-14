ui_ux_design_system: css_architecture: framework: "TailwindCSS 4.2.2" tokens: "CSS Custom Properties
in theme.css" components: "@apply in src/TicketsPlease.Web/Tailwind/components/" theming:
"data-theme on <html> element" design_language: aesthetics: "Premium, Bold, Asymmetric" colors:
"Dominant brand colors with high-contrast sharp accents" effects: ["Glassmorphism", "Gradient
Meshes", "Grain Overlays"] typography: "Modern, high-character fonts (Google Fonts)"
razor_philosophy: sfc: "ViewComponents bundle Template + UI Logic" dry: "Use Partials (e.g.,
\_Avatar.cshtml)" logic_free: "Zero C# business logic in CSHTML" accessibility_a11y: compliance:
"BFSG; keyboard-first, focus-traps, screen-reader ready" html5_semantics: ["<nav>", "<main>",
"<dialog>"] aria_usage: "Strictly use aria-label, aria-hidden, aria-expanded"
