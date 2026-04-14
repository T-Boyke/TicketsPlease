# 🎨 Frontend Tooling

Werkzeuge und Prozesse für das TicketsPlease Frontend.

## 📋 Table of Contents

- [LibMan (Library Manager)](#libman-library-manager)
- [Tailwind CSS 4.2](#tailwind-css-42)
- [Asset Management](#asset-management)

---

## LibMan (Library Manager)

Verwaltet clientseitige Bibliotheken in `wwwroot/lib`.

- **Konfiguration:** `libman.json` im Web-Projekt.
- **Wichtig:** Keine CDNs! Alle Bibliotheken müssen lokal vorhanden sein (DSGVO & Performance).
- **Update:** Rechtsklick auf `libman.json` -> "Restore Client-Side Libraries".

---

## Tailwind CSS 4.2

Modernes Styling ohne Node.js Abhängigkeit.

- **Integration:** `Tailwind.Hosting` und `tailwindcss.exe`.
- **Customization:** Styles liegen in `src/TicketsPlease.Web/styles/`.
- **Build:** Automatisch via `TailwindCSS.MSBuild`.

---

## Asset Management

- **Icons:** FontAwesome 7.2 (lokal eingebunden).
- **Bilder:** Optimierte SVGs oder WebP in `wwwroot/images/`.
- **Cleaning:** `bin` und `obj` Ordner bei CSS-Problemen bereinigen.

---

### FrontendTooling v1.0 | 2026-03-09
