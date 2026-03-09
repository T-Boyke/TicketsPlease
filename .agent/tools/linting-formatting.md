# 🔍 Linting & Formatting

Regeln und Tools zur Sicherstellung der Code-Qualität.

## 📋 Table of Contents

- [🔷 C# Formatting](#-c-formatting)
- [📝 Markdown Linting](#-markdown-linting)
- [✨ Prettier](#-prettier)

---

## 🔷 C# Formatting

- **Tool:** `dotnet format`.
- **Regeln:** Definiert in `.editorconfig`.
- **CI-Gate:** Pushed Code wird auf Formatierung geprüft; Abweichungen führen zum Build-Failure.

---

## 📝 Markdown Linting

- **Tool:** `markdownlint`.
- **Config:** `.markdownlint.json`.
- **Fokus:** Einhaltung von Header-Hierarchien, korrekten Listen-Styles und Zeilenlängen für Dokumentationen.

---

## ✨ Prettier

- **Tool:** Prettier CLI.
- **Config:** `.prettierrc`.
- **Einsatz:** Formatiert JSON, CSS, JS und Markdown Dateien konsistent über alle IDEs hinweg.

---

_LintingConfig v1.0 | 2026-03-09_
