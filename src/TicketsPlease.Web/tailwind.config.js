/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    "./Pages/**/*.cshtml",
    "./Views/**/*.cshtml",
    "./wwwroot/**/*.html"
  ],
  theme: {
    extend: {
      colors: {
        // Diese Farben werden später über CSS-Variablen (ICorporateSkinProvider) gesteuert
        brand: {
          primary: 'var(--brand-primary, #3b82f6)',
          secondary: 'var(--brand-secondary, #1e40af)',
        }
      }
    },
  },
  plugins: [],
}
