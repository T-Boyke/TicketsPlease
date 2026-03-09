# 🤖 TicketsPlease – Agent Configuration

Dieses Dokument ist der zentrale Einstiegspunkt für den KI-Agenten im
**TicketsPlease** Projekt.

> 📖 **Vollständige Governance:**
> Alle technischen Regeln in [instructions.md](file:///d:/DEV/Tickets/instructions.md)
> (16 Sektionen).

---

## 📏 Agent Rules

Die folgenden Regeln in `.agent/rules/` sind **verbindlich** für jede Interaktion:

| Regel-Datei                                                                      | Beschreibung                                       |
| -------------------------------------------------------------------------------- | -------------------------------------------------- |
| [agent-behavior.md](file:///d:/DEV/Tickets/.agent/rules/agent-behavior.md)       | 🤖 Grundhaltung, Plan-First, Kommunikation, No-Gos |
| [architecture.md](file:///d:/DEV/Tickets/.agent/rules/architecture.md)           | 🏛️ Clean Architecture, DDD, CQRS, EF Core, Naming  |
| [security.md](file:///d:/DEV/Tickets/.agent/rules/security.md)                   | 🛡️ Defense in Depth, Secrets, DSGVO, XSS           |
| [ui-frontend.md](file:///d:/DEV/Tickets/.agent/rules/ui-frontend.md)             | 🎨 Tailwind, a11y/BFSG, Theme-Switching, SFC       |
| [testing.md](file:///d:/DEV/Tickets/.agent/rules/testing.md)                     | 🧪 TDD, Testcontainers, NetArchTest, Lighthouse    |
| [git-documentation.md](file:///d:/DEV/Tickets/.agent/rules/git-documentation.md) | 🤝 Branching, Commits, PRs, XML-Docs, ADRs         |

---

## 🔧 Workflows

| Workflow                                                                                       | Beschreibung                                  |
| ---------------------------------------------------------------------------------------------- | --------------------------------------------- |
| [/add-cqrs-feature](file:///d:/DEV/Tickets/.agent/workflows/add-cqrs-feature.md)               | Neues CQRS Feature (Command/Query) hinzufügen |
| [/ef-core-migration](file:///d:/DEV/Tickets/.agent/workflows/ef-core-migration.md)             | EF Core Migrations erstellen & anwenden       |
| [/testing-standards](file:///d:/DEV/Tickets/.agent/workflows/testing-standards.md)             | Unit- & Integration-Tests schreiben           |
| [/ui-component-tailwind](file:///d:/DEV/Tickets/.agent/workflows/ui-component-tailwind.md)     | UI-Komponenten mit Tailwind erstellen         |
| [/atomic-commits](file:///d:/DEV/Tickets/.agent/workflows/atomic-commits.md)                   | Atomare, logische Git-Commits                 |
| [/security-review](file:///d:/DEV/Tickets/.agent/workflows/security-review.md)                 | Security-Checkliste (Defense in Depth)        |
| [/domain-entity](file:///d:/DEV/Tickets/.agent/workflows/domain-entity.md)                     | DDD Entity/Value Object erstellen             |
| [/documentation-standards](file:///d:/DEV/Tickets/.agent/workflows/documentation-standards.md) | Dokumentations-Standards einhalten            |

---

## 🧠 Skills

| Skill                                                                                                    | Beschreibung                                  |
| -------------------------------------------------------------------------------------------------------- | --------------------------------------------- |
| [clean-architecture-scaffold](file:///d:/DEV/Tickets/.agent/skills/clean-architecture-scaffold/SKILL.md) | 🏗️ Feature über alle Layer scaffolden         |
| [code-review](file:///d:/DEV/Tickets/.agent/skills/code-review/SKILL.md)                                 | 🔍 Code nach allen Projekt-Standards reviewen |
| [ef-core-debugging](file:///d:/DEV/Tickets/.agent/skills/ef-core-debugging/SKILL.md)                     | 🔬 EF Core Performance & Debugging            |
| [refactoring-patterns](file:///d:/DEV/Tickets/.agent/skills/refactoring-patterns/SKILL.md)               | 🔄 Sichere Refactorings in Clean Architecture |
| [adr-writer](file:///d:/DEV/Tickets/.agent/skills/adr-writer/SKILL.md)                                   | 📋 Architecture Decision Records erstellen    |
| [tailwind-component-patterns](file:///d:/DEV/Tickets/.agent/skills/tailwind-component-patterns/SKILL.md) | 🎨 UI-Patterns mit TailwindCSS 4.2 + a11y     |

---

## 📚 Projekt-Dokumentation

| Dokument                                                                                  | Beschreibung                              |
| ----------------------------------------------------------------------------------------- | ----------------------------------------- |
| [README.md](file:///d:/DEV/Tickets/README.md)                                             | Projekt-Vision, Features, Tech-Stack      |
| [MVP-Roadmap](file:///d:/DEV/Tickets/docs/MVP_Roadmap.md)                                 | Phase-Abgrenzung (MVP vs. Enterprise)     |
| [Context: Project](file:///d:/DEV/Tickets/.agent/context/project-intelligence.md)         | 🧠 Mission, Vision & Roadmap Status       |
| [Context: Tech-Stack](file:///d:/DEV/Tickets/.agent/context/tech-stack-referenz.md)       | 🛠️ NuGet, Layer & Versioning              |
| [Context: Architecture](file:///d:/DEV/Tickets/.agent/context/architectural-blueprint.md) | 🏛️ Layers, CQRS & EF Core Policy          |
| [Context: Domain](file:///d:/DEV/Tickets/.agent/context/domain-knowledge.md)              | 🧬 Entities, Rules & Metrics              |
| [Context: UI/UX](file:///d:/DEV/Tickets/.agent/context/ui-ux-design-system.md)            | 🎨 Tailwind, SFC & a11y                   |
| [Context: QA](file:///d:/DEV/Tickets/.agent/context/quality-assurance.md)                 | 🧪 TDD, Testing & Quality Gates           |
| [Tool: CLI](file:///d:/DEV/Tickets/.agent/tools/cli-essentials.md)                        | 💻 .NET, EF & Tailwind CLI                |
| [Tool: Frontend](file:///d:/DEV/Tickets/.agent/tools/frontend-tooling.md)                 | 🎨 LibMan, Assets & No-CDN                |
| [Tool: IDE](file:///d:/DEV/Tickets/.agent/tools/ide-intelligence.md)                      | 🧠 Rider, VS Code & EditorConfig          |
| [Tool: Lint/Format](file:///d:/DEV/Tickets/.agent/tools/linting-formatting.md)            | 🔍 Linter & Formatters                    |
| [ADR-Index](file:///d:/DEV/Tickets/docs/adr/README.md)                                    | Alle Architektur-Entscheidungen           |
| [database_schema.md](file:///d:/DEV/Tickets/docs/database_schema.md)                      | ERD & Entity-Beschreibungen               |
| [domain_ticket.md](file:///d:/DEV/Tickets/docs/domain_ticket.md)                          | Ticket-Entity DDD Spezifikation           |
| [nuget_stack.md](file:///d:/DEV/Tickets/docs/nuget_stack.md)                              | Erlaubte NuGet-Pakete pro Layer           |
| [frontend_assets.md](file:///d:/DEV/Tickets/docs/frontend_assets.md)                      | Asset-Management & No-CDN Policy          |
| [architecture_diagrams.md](file:///d:/DEV/Tickets/docs/architecture_diagrams.md)          | Clean Architecture & Deployment Diagramme |
| [dev_setup_guide.md](file:///d:/DEV/Tickets/docs/dev_setup_guide.md)                      | Einrichtungsanleitung                     |

---

### TicketsPlease Agent Config v3.1 | 2026-03-06
