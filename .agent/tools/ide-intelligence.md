# 🧠 IDE Intelligence

Konfigurationen für eine konsistente Developer Experience in Rider und VS Code.

## 📋 Table of Contents

- [JetBrains Rider](#jetbrains-rider)
- [Visual Studio Code](#visual-studio-code)
- [Editorconfig](#editorconfig)

---

## JetBrains Rider

Die bevorzugte IDE für dieses Projekt.

- **Live Templates:** Eigene Vorlagen für MediatR Commands/Queries und SFCs.
- **Dictionaries:** Projekt-spezifische Wörterbücher in `.idea/dictionaries/` zur Reduzierung
  von Spell-Check Noise.
- **Plugins:** Roslyn Analyzer Unterstützung aktiv.

---

## Visual Studio Code

Support für leichtgewichtiges Editing und Dokumentation.

- **Workspace:** `.vscode/settings.json` enthält optimierte Formatierungseinstellungen.
- **Extensions:** Prettier, C# Dev Kit und Markdownlint empfohlen.
- **MCP Tooling:** Konfigurationen in `.vscode/mcp.json`.

---

## Editorconfig

Das "Gesetz" für Code-Style und Naming:

- Erschwingt `I`-Prefix für Interfaces.
- Definiert Tab-Breite (4 spaces für C#, 2 für Web/MD).
- Wird in der CI-Pipeline via `dotnet format --verify-no-changes` validiert.

---

_IDEIntel v1.0 | 2026-03-09_
