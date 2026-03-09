# 📋 Changelog

Alle wichtigen Änderungen an diesem Projekt werden in dieser Datei dokumentiert.

Das Format basiert auf [Keep a Changelog](https://keepachangelog.com/de/1.1.0/).
Dieses Projekt nutzt [Conventional Commits](https://www.conventionalcommits.org/) und [Semantic Versioning](https://semver.org/lang/de/).

> [!IMPORTANT]
> Diese Datei **muss** vor jedem Commit aktualisiert werden, wenn die Änderung nutzerrelevant ist
> (Features, Bugfixes, Breaking Changes, Security).

---

## [Unreleased]

### Added

- **Layer-Specific READMEs:** 5 neue README-Dateien im `src`-Verzeichnis mit
  spezifischen Arbeitsanweisungen für Domain, Application, Infrastructure etc.
- **Git Layer-Branching:** Einführung einer Schicht-basierten Branching-Strategie
  (`layer/domain`, etc.) inkl. Dokumentation in allen READMEs.
- **Antigravity AI Skills:** 6 automatisierte Skills für Scaffolding,
  Code-Review, EF-Debugging und ADR-Erstellung (#64)
- **Client-side Libraries:** Integration von Markdig, SortableJS,
  FontAwesome 7.2 und DOMPurify (#160)
- **Tailwind CSS v4:** Vollständige Migration auf die node-freie
  `tailwindcss-dotnet` Integration (#051)
- **ADR Renumbering & Restructuring:** Re-indexed all Architecture Decision
  Records to a 10-step MVP-centric sequence (0000, 0010, 0020, etc.) and
  added/updated 22+ foundational records for project alignment. (#65)
- **Database Verification Infrastructure:** Added arquitectura and integration
  tests for automated database verification.
- ADR 0062: Database Verification Strategy.
- Datenbank-Seeding mit `Bogus` (de Locale) für realistische Testdaten.
- ADR 0120: Synthetic Data Generation and Privacy Compliance.
- **Agent Governance v3.1:** Erweiterte Konfiguration für konsistente
  Clean Architecture Entwicklung (#aa3)

### Changed

- **README Hub:** Umgestaltung der README zur zentralen Dokumentations-Drehscheibe mit Fokus auf
  MVP vs. Enterprise Fortschritt.
- **Gitignore:** Optimierung für Tailwind v4 build artifacts und libman assets.
- **Agent Hub Configuration:** `.agent/instructions.md` auf Hub-Format umgestellt (verweist auf Rules,
  Workflows, Projekt-Docs).
- **Workflow Expansion:** Alle 5 bestehenden Workflows massiv erweitert (CQRS, EF Core, Testing,
  UI, Commits).
- **AppDbContext:** Made provider-aware to support SQLite for integration tests while maintaining
  SQL Server specific features (RowVersion).

### Security

- **Vulnerability Patching:** Updated `MimeKit` to 4.11.0 and `Newtonsoft.Json` to 13.0.3 to address
  high/moderate severity vulnerabilities.

---

## [0.1.0] - 2026-03-06

### Added

- 🏛️ Clean Architecture Solution Struktur (`TicketsPlease.Domain`, `.Application`,
  `.Infrastructure`, `.Web`)
- 🗄️ EF Core Database Resilience (`EnableRetryOnFailure`, `DefaultExecutionStrategy`) und
  Optimistic Concurrency (`RowVersion`)
- 🎨 TailwindCSS 4.2 Integration via `TailwindCSS.MSBuild` (Zero-Node) und
  `ICorporateSkinProvider` für Corporate Theming
- 📐 20 Architecture Decision Records (ADR-0001 bis ADR-0020)
- 📝 Umfassende Projektdokumentation:
  - [README.md](README.md) – Vision, Tech-Stack, Features, Roadmap
  - [database_schema.md](docs/database_schema.md) – 3NF ERD mit 20+ Entities
  - [domain_ticket.md](docs/domain_ticket.md) – DDD Ticket-Entity Spezifikation
  - [nuget_stack.md](docs/nuget_stack.md) – NuGet-Pakete pro Layer
  - [frontend_assets.md](docs/frontend_assets.md) – Asset-Management & No-CDN Policy
  - [architecture_diagrams.md](docs/architecture_diagrams.md) – Mermaid-Diagramme (Clean Architecture,
    Deployment, CQRS Flow)
  - [MVP_Roadmap.md](docs/MVP_Roadmap.md) – Phase-Abgrenzung (MVP vs. Enterprise)
  - [dev_setup_guide.md](docs/dev_setup_guide.md) – Einrichtungsanleitung
- 🔧 IDE-Konfiguration: `.editorconfig`, `.vsconfig`, JetBrains Rider Settings
- 🤖 GitHub Templates: Issue & PR Templates, CI/CD Workflow (`.github/`)

---

## Legende

| Kategorie      | Beschreibung                             |
| :------------- | :--------------------------------------- |
| **Added**      | Neue Features oder Dateien               |
| **Changed**    | Änderungen an bestehender Funktionalität |
| **Deprecated** | Features die bald entfernt werden        |
| **Removed**    | Entfernte Features oder Dateien          |
| **Fixed**      | Bugfixes                                 |
| **Security**   | Sicherheitsrelevante Änderungen          |
