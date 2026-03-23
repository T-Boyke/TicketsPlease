---
name: tailwind-component-patterns
description: Premium TailwindCSS 4.2 patterns with OKLCH, Glassmorphism, and BFSG/a11y.
---

# 🎨 TailwindCSS - Component Patterns

<tailwind_patterns>
  <philosophy>
    - Aesthetics: "Wow-Effect" via premium OKLCH colors, asymmetric layouts, and overlapping elements.
    - Anti-Patterns: NO generic fonts, NO purple AI gradients, NO Bootstrap, NO CDNs.
    - Atmosphere: Use Grain Overlays, Motion (Reveal-up), and Glassmorphism (Backdrop-blur).
  </philosophy>

  <design_tokens>
    - Space: Asymmetry > Symmetry. Use -mx margins for grid-breaking sections.
    - Colors: Use OKLCH for brilliance. Sharp Contrast Accents (e.g., Ocean Blue + Gold).
    - Surfaces: Depth via `shadow-dramatic` and layered transparencies.
  </design_tokens>

  <components>
    <buttons>
      - Class: `.btn` (Base), `.btn-primary` (Brand), `.btn-accent` (Sharp).
      - Interaction: `hover-lift` (translateY + rotate) + `hover-glow`.
    </buttons>
    <cards>
      - Pattern: `.card` with `rounded-2xl` and `shadow-elevation-1`.
      - Effect: `.card-feature` with asymmetric linear-gradient borders.
      - Kanban: `.kanban-card` with dynamic accent borders and active-dragging cursor.
    </cards>
    <forms>
      - Input: `.form-input` with focus-glow and spring-based shake animation on error.
      - A11y: Contrast-check every color; `aria-label` for icons mandatory.
    </forms>
    <modals>
      - Style: `.modal-content` with `backdrop-filter: blur(24px)` (Glassmorphism).
      - Entry: Staggered `reveal-up` animation for a "premium" feel.
    </modals>
  </components>

  <animations>
    - Reveal: `reveal-up` using `cubic-bezier(0.34, 1.56, 0.64, 1)` (Spring).
    - Staggered: Classes `.reveal-delay-1` through `.reveal-delay-5`.
    - Motion: Always respect `prefers-reduced-motion: reduce`.
  </animations>

  <accessibility>
    - Checkbox: Keyboard-first (Focus-visible ring-2).
    - Semantics: Use `<dialog>`, `<nav>`, `<main>`, `<section>`.
    - ARIA: `aria-expanded`, `aria-describedby`, `aria-hidden` roles mandatory.
  </accessibility>
</tailwind_patterns>
