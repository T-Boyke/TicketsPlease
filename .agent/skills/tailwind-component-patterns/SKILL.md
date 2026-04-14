tailwind_component_patterns_skill: name: "tailwind-component-patterns" description: "Premium
TailwindCSS 4.2 patterns using OKLCH, Glassmorphism, and BFSG/a11y" design_philosophy: aesthetics:
"'Wow-Effect' via premium OKLCH colors, asymmetric layouts, and overlapping elements" anti_patterns:
["Generic fonts", "Purple AI gradients", "Bootstrap", "CDNs"] atmosphere: ["Grain Overlays", "Motion
(Reveal-up)", "Glassmorphism (Backdrop-blur)"] design_tokens: spacing: "Asymmetry > Symmetry;
grid-breaking sections (-mx margins)" colors: "OKLCH brilliance; sharp contrast accents (e.g., Ocean
Blue + Gold)" surfaces: "Depth via shadow-dramatic and layered transparencies" components: buttons:
classes: [".btn", ".btn-primary", ".btn-accent"] interaction: "hover-lift (translateY + rotate) and
hover-glow" cards: base: ".card (rounded-2xl, shadow-elevation-1)" feature: ".card-feature
(asymmetric linear-gradient borders)" kanban: ".kanban-card (dynamic accent borders)" forms: inputs:
".form-input (focus-glow, spring-shake on error)" a11y: "Contrast checks; aria-label for icon-only
buttons" modals: styling: ".modal-content (backdrop-blur-md, Glassmorphism)" entry: "Staggered
reveal-up animation" animations: reveal: "reveal-up (spring cubic-bezier)" staggering:
".reveal-delay-1 to .reveal-delay-5" sensitivity: "Respect prefers-reduced-motion: reduce"
accessibility_a11y: controls: "Keyboard-first (Focus-visible ring-2)" semantics: ["<dialog>",
"<nav>", "<main>", "<section>"] aria_roles: ["aria-expanded", "aria-describedby", "aria-hidden"]
version: "1.0"
