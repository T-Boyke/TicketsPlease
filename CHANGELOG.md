# 📋 Changelog

Alle wichtigen Änderungen an diesem Projekt werden in dieser Datei dokumentiert.

Das Format basiert auf [Keep a Changelog](https://keepachangelog.com/de/1.1.0/).
Dieses Projekt nutzt [Conventional Commits](https://www.conventionalcommits.org/) und [Semantic Versioning](https://semver.org/lang/de/).

> [!IMPORTANT]
> Diese Datei **muss** vor jedem Commit aktualisiert werden, wenn die Änderung nutzerrelevant ist (Features, Bugfixes, Breaking Changes, Security).

---

## [Unreleased]

### Added
- **Agent Governance v3.1:** Vollständige Agent-Konfiguration mit 6 Rule-Dateien in `.agent/rules/`, 8 Workflows in `.agent/workflows/` und `instructions.md` (16 Sektionen)
- **CHANGELOG.md:** Automatisch aktualisiertes Änderungsprotokoll nach Keep-a-Changelog Standard

### Changed
- `.agent/instructions.md` auf Hub-Format umgestellt (verweist auf Rules, Workflows, Projekt-Docs)
- Alle 5 bestehenden Workflows massiv erweitert (CQRS, EF Core, Testing, UI, Commits)

---

## [0.1.0] - 2026-03-06

### Added
- 🏛️ Clean Architecture Solution Struktur (`TicketsPlease.Domain`, `.Application`, `.Infrastructure`, `.Web`)
- 🗄️ EF Core Database Resilience (`EnableRetryOnFailure`, `DefaultExecutionStrategy`) und Optimistic Concurrency (`RowVersion`)
- 🎨 TailwindCSS 4.2 Integration via `TailwindCSS.MSBuild` (Zero-Node) und `ICorporateSkinProvider` für Corporate Theming
- 📐 20 Architecture Decision Records (ADR-0001 bis ADR-0020)
- 📝 Umfassende Projektdokumentation:
  - [README.md](README.md) – Vision, Tech-Stack, Features, Roadmap
  - [database_schema.md](docs/database_schema.md) – 3NF ERD mit 20+ Entities
  - [domain_ticket.md](docs/domain_ticket.md) – DDD Ticket-Entity Spezifikation
  - [nuget_stack.md](docs/nuget_stack.md) – NuGet-Pakete pro Layer
  - [frontend_assets.md](docs/frontend_assets.md) – Asset-Management & No-CDN Policy
  - [architecture_diagrams.md](docs/architecture_diagrams.md) – Mermaid-Diagramme (Clean Architecture, Deployment, CQRS Flow)
  - [MVP_Roadmap.md](docs/MVP_Roadmap.md) – Phase-Abgrenzung (MVP vs. Enterprise)
  - [dev_setup_guide.md](docs/dev_setup_guide.md) – Einrichtungsanleitung
- 🔧 IDE-Konfiguration: `.editorconfig`, `.vsconfig`, JetBrains Rider Settings
- 🤖 GitHub Templates: Issue & PR Templates, CI/CD Workflow (`.github/`)

---

## Legende

| Kategorie | Beschreibung |
|---|---|
| **Added** | Neue Features oder Dateien |
| **Changed** | Änderungen an bestehender Funktionalität |
| **Deprecated** | Features die bald entfernt werden |
| **Removed** | Entfernte Features oder Dateien |
| **Fixed** | Bugfixes |
| **Security** | Sicherheitsrelevante Änderungen |
