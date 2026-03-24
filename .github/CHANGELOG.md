# 📋 Changelog

Alle wichtigen Änderungen an diesem Projekt werden in dieser Datei dokumentiert.

Das Format basiert auf [Keep a Changelog](https://keepachangelog.com/de/1.1.0/).
Dieses Projekt nutzt
[Conventional Commits](https://www.conventionalcommits.org/) und
[Semantic Versioning](https://semver.org/lang/de/).

> [!IMPORTANT] Diese Datei **muss** vor jedem Commit aktualisiert werden, wenn
> die Änderung nutzerrelevant ist (Features, Bugfixes, Breaking Changes,
> Security).

---

## [Unreleased]

### Added

- **User Stories:** Zentrale `aufgabe.md` mit allen 9 Features und
  Akzeptanzkriterien für das MVP erstellt, spezifischen Arbeitsanweisungen
  für Domain, Application, Infrastructure etc.
- **Git Layer-Branching:** Einführung einer Schicht-basierten
  Branching-Strategie (`layer/domain`, etc.) inkl. Dokumentation in allen
  READMEs.
- **Antigravity AI Skills:** 6 automatisierte Skills für Scaffolding,
  Code-Review, EF-Debugging und ADR-Erstellung (#64)
- **Client-side Libraries:** Integration von Markdig, SortableJS, FontAwesome
  7.2 und DOMPurify (#160)
- **Tailwind CSS v4:** Vollständige Migration auf die node-freie
  `tailwindcss-dotnet` Integration (#051)
- **ADR Renumbering & Restructuring:** Re-indexed all Architecture Decision
  Records to a 10-step MVP-centric sequence (0000, 0010, 0020, etc.) and
  added/updated 22+ foundational records for project alignment. (#65)
- **Database Verification Infrastructure:** Added architecture and integration
  tests for automated database verification.
- ADR 0062: Database Verification Strategy.
- Datenbank-Seeding mit `Bogus` (de Locale) für realistische Testdaten.
- ADR 0120: Synthetic Data Generation and Privacy Compliance.
- **Agent Governance v3.1:** Erweiterte Konfiguration für konsistente Clean
  Architecture Entwicklung (#aa3)
- **Kanban Domain Expansion:** Neue Enums (`TicketType`, `TicketSize`,
  `TicketLinkType`) und `TicketLink`-Entity für Ticket-Abhängigkeiten
  (Blocks, RelatesTo, Duplicates). (#37, #40)
- **Project Entity:** `Project`-Entity mit `StartDate`, `EndDate` und
  `WorkflowId` für F2.2 Projektmanagement hinzugefügt. (#34)
- **DDD Base Classes:** `BaseAuditableEntity`, `IAggregateRoot`,
  `IDomainEvent` und `IBaseEntity`-Interface für Rich Domain Model
  Infrastruktur eingeführt.

### Changed

- **Framework Update:** Migrated all Microsoft packages to v10.0.5 and locked
  SDK to v10.0.200 for team consistency.
- **Ticket Entity Expansion:** `Ticket` um Kanban-Properties erweitert:
  `ProjectId`, `AssignedUserId`, `ClosedAt`, `ClosedByUserId`,
  `ChilliesDifficulty`, `StartDate`, `Deadline` und Parent/Child-Hierarchie.
- **Domain Layer Compliance:** Member-Ordering (SA1201/SA1202), XML-Docs und
  Code-Analyse-Suppressions (`CA1819`, `CA5350`) in allen 18 Domain-Dateien
  korrigiert.
- **Database Schema Docs:** `docs/database_schema.md` vollständig mit dem
  aktuellen Domain Model (29 Entities) synchronisiert, inkl. Mermaid ERD,
  tabellarischer Übersicht und MVP-Status.
- **Architecture Diagrams:** `docs/architecture_diagrams.md` mit aktualisierten
  Layer- und Entitätsdiagrammen aktualisiert.

### Fixed

- **CI Pipeline:** Playwright E2E-Tests stabilisiert durch automatische
  Browser-Installation in `dotnet.yml`.
- **EF Core SQLite Compatibility:**
  - Provider-Konflikt (`Multiple DB Providers`) in `IntegrationTestBase.cs`
    durch aggressives Service-Removal gelöst.
  - `RowVersion` NOT NULL Constraints in SQLite durch provider-spezifische
    Konfiguration (`ValueGeneratedNever`) in `AppDbContext.cs` behoben.
  - Foreign Key Fehler in Integrationstests durch Einführung von
    `SeedMinimalAsync` (Roles, Priorities, WorkflowStates) beseitigt.
- **Code Style & Linting:**
  - Sämtliche `dotnet format` Verstöße (SA1202, SA1201, SA1413, CA2007) in
    den Test-Projekten behoben.
  - Markdown Linting Fehler (MD033, MD041, MD045) in `README.md` korrigiert.
- **Build Error CS0029:** `string` zu `Uri` Konvertierung in
  `DbInitialiser.cs` für `AvatarUrl` behoben.
- **Mermaid ERD Syntax:** Nullable `?`-Typen und nicht-quotierte
  Relationship-Labels (Klammern, Schrägstriche) in `database_schema.md`
  korrigiert.
- **Architecture Tests:** `DomainConstraintTests` aktualisiert, um
  Identity-basierte Entities (`User`, `Role`) korrekt auszuschließen.
- **Seeding Logic:** `DbInitialiser` TeamMember-Loop durch LINQ und
  `AddRangeAsync` vereinfacht (S3267).
- **README Hub:** Umgestaltung der README zur zentralen
  Dokumentations-Drehscheibe mit Fokus auf MVP vs. Enterprise Fortschritt.
- **Gitignore:** Optimierung für Tailwind v4 build artifacts und libman assets.
- **Agent Hub Configuration:** `.agent/instructions.md` auf Hub-Format
  umgestellt (verweist auf Rules, Workflows, Projekt-Docs).
- **Workflow Expansion:** Alle 5 bestehenden Workflows massiv erweitert (CQRS,
  EF Core, Testing, UI, Commits).
- **AppDbContext:** Made provider-aware to support SQLite for integration tests
  while maintaining SQL Server specific features (RowVersion).

### Security

- **Vulnerability Patching:** Updated `MimeKit` to 4.11.0 and `Newtonsoft.Json`
  to 13.0.3 to address high/moderate severity vulnerabilities.

---

## [0.1.0] - 2026-03-06

### Added

- 🏛️ Clean Architecture Solution Struktur (`TicketsPlease.Domain`,
  `.Application`, `.Infrastructure`, `.Web`)
- 🗄️ EF Core Database Resilience (`EnableRetryOnFailure`,
  `DefaultExecutionStrategy`) und Optimistic Concurrency (`RowVersion`)
- 🎨 TailwindCSS 4.2 Integration via `TailwindCSS.MSBuild` (Zero-Node) und
  `ICorporateSkinProvider` für Corporate Theming
- 📐 20 Architecture Decision Records (ADR-0001 bis ADR-0020)
- 📝 Umfassende Projektdokumentation:
  - [README.md](README.md) – Vision, Tech-Stack, Features, Roadmap
  - [database_schema.md](docs/database_schema.md) – 3NF ERD mit 20+ Entities
  - [domain_ticket.md](docs/domain_ticket.md) – DDD Ticket-Entity Spezifikation
  - [nuget_stack.md](docs/nuget_stack.md) – NuGet-Pakete pro Layer
  - [frontend_assets.md](docs/frontend_assets.md) – Asset-Management & No-CDN
    Policy
  - [architecture_diagrams.md](docs/architecture_diagrams.md) –
    Mermaid-Diagramme (Clean Architecture, Deployment, CQRS Flow)
  - [MVP_Roadmap.md](docs/MVP_Roadmap.md) – Phase-Abgrenzung (MVP vs.
    Enterprise)
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
