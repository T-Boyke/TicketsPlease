---
trigger: model_decision
description: each time we commit, we check if we completed an issue and commit on it
---

# 🎫 TicketsPlease - GitHub Issues & Projects Rules

<!-- markdownlint-disable MD033 -->

<github_issues_rules>

## Repository & Project Board

- **Repository:** `BitLC-NE-2025-2026/TicketsPlease`
- **Project Board:** [TicketsPlease Kanban](https://github.com/orgs/BitLC-NE-2025-2026/projects/1/views/1)
- **Assignee:** [T-Boyke](https://github.com/T-Boyke)
- **Technische Dokumentation:**
  - [Projekt-Fahrplan (Gantt)](file:///d:/DEV/Tickets/docs/gantt_roadmap.md)
  - [MVP Feature-Katalog (F1-F9)](file:///d:/DEV/Tickets/docs/MVP_Roadmap.md)
  - [Architektur-Diagramme (C4/Layer)](file:///d:/DEV/Tickets/docs/architecture_diagrams.md)
  - [Datenbank-Schema](file:///d:/DEV/Tickets/docs/database_schema.md)
  - [Domain-Modell: Ticket (Deep Dive)](file:///d:/DEV/Tickets/docs/domain_ticket.md)
  - [Technologie-Stack (NuGet/Libs)](file:///d:/DEV/Tickets/docs/nuget_stack.md)
  - [Frontend Assets & UI](file:///d:/DEV/Tickets/docs/frontend_assets.md)
  - [Entwickler-Setup Guide](file:///d:/DEV/Tickets/docs/dev_setup_guide.md)
  - [Antigravity Agent Guide](file:///d:/DEV/Tickets/docs/antigravity-guide.md)
- **Relationships:** Alle Issues müssen **nativ** verknüpft sein (Sidebar "Relationships"). (Siehe #31 für Beispiel).
  - **Parent:** Epics/Features (z.B. F1) als Parent für Task-Issues.
  - **Blocked by:** Layer-Abhängigkeiten (Domain -> App -> Infra -> Web).

## Issue Types

GitHub Issues use **Issue Type** (`Bug`, `Feature`, `Task`) as the primary classification.

| Type      | Verwendung                                           |
| :-------- | :--------------------------------------------------- |
| `Feature` | Neue Funktionalität (IHK F1–F9, Enterprise Add-Ons)  |
| `Bug`     | Fehler in bestehender Funktionalität                 |
| `Task`    | Technische Aufgabe (Refactoring, Docs, Infra, CI/CD) |

## Labels (Pflicht)

Jedes Issue **MUSS** mindestens ein `area:`-Label und ein `size/`-Label erhalten.

### Area Labels (Pflicht, mind. 1)

| Label                 | Beschreibung                                          |
| :-------------------- | :---------------------------------------------------- |
| `area:domain`         | Domain Layer (Entities, Value Objects, Domain Events) |
| `area:infrastructure` | Infrastructure Layer (EF Core, Repos, Services)       |
| `area:web`            | Web Layer (Controllers, Views, Razor, Tailwind)       |
| `area:tests`          | Unit Tests, Integration Tests, E2E Tests              |
| `area:docs`           | Dokumentation, ADRs, README, CHANGELOG                |
| `area:github`         | CI/CD, GitHub Actions, Project Board, Issues          |

### Size Labels (Pflicht, genau 1)

| Label     | Story Points | Beschreibung                                 |
| :-------- | :----------- | :------------------------------------------- |
| `size/XS` | 1            | < 30 Min (Config, Typo, kleine Fixes)        |
| `size/S`  | 2–3          | 30 Min – 2h (Einfaches Feature, kleiner Fix) |
| `size/XL` | 8–13         | 1–3 Tage (Komplexes Feature, Multi-Layer)    |

### Ergänzende Labels (Optional)

`enhancement`, `bug`, `documentation`, `dependencies`, `diagrams`,
`duplicate`, `good first issue`, `help wanted`, `invalid`, `question`,
`wontfix`

## Issue-Struktur

### Epic/Parent Issue

Für jedes IHK-Feature (F1–F9) gibt es ein **Parent Issue** (Epic),
das alle Subtasks referenziert:

```markdown
## F3: Ticket-Management

IHK Feature F3 – Alle User Stories zum Ticket-Bereich.

### Sub-Issues

- [ ] #42 [Domain] Ticket Entity mit Close-Regeln
- [ ] #43 [Application] CreateTicketCommand + Validator
- [ ] #44 [Infrastructure] TicketRepository
- [ ] #45 [Web] TicketController + Views
- [ ] #46 [Tests] Ticket Unit + Integration Tests
```

### Sub-Issue Template

```markdown
## Beschreibung

[Was wird implementiert und warum?]

## Akzeptanzkriterien (IHK)

- [ ] [Exaktes Kriterium aus der Aufgabe]
- [ ] [Weiteres Kriterium]

## Technische Umsetzung

- **Layer:** Domain / Application / Infrastructure / Web
- **Dateien:** `TicketEntity.cs`, `CreateTicketCommand.cs`
- **Abhängigkeiten:** Blocked by #XX

## Definition of Done

- [ ] Code kompiliert (`dotnet build`)
- [ ] Tests grün (`dotnet test`)
- [ ] XML-Docs vorhanden
- [ ] CHANGELOG aktualisiert (bei feat/fix)
- [ ] Atomic Commit mit `Closes #XX`
```

## Kanban Columns

| Column          | Bedeutung                                         |
| :-------------- | :------------------------------------------------ |
| **Backlog**     | Geplant, aber noch nicht begonnen                 |
| **Ready**       | Alle Abhängigkeiten erfüllt, kann begonnen werden |
| **In Progress** | Aktiv in Bearbeitung (max. 3 WIP)                 |
| **Done**        | Implementiert, getestet, committed, gepushed      |

## Workflow-Regeln

<workflow>

1. **Issue erstellen:** Type + Labels + Assignee + Beschreibung.
2. **Backlog → Ready:** Wenn alle `blocked by` Issues geschlossen sind.
3. **Ready → In Progress:** Entwickler nimmt Issue aktiv an.
4. **In Progress → Done:** Commit mit `Closes #XX` im Message-Body.
5. **Kommentar bei Done:** Abschluss-Kommentar mit Commit-SHA.

</workflow>

## Commit-Integration

Commits referenzieren Issues über Keywords im Body:

```text
feat(domain): add Ticket entity with close rules

Implement rich domain model for Ticket with private setters,
Close method with actor validation, and SHA1 hash generation.

Closes #42
```

## Blocker & Dependencies

- Verwende `blocked by #XX` in der Issue-Beschreibung.
- Abhängigkeiten folgen dem Clean Architecture Layer-Flow:
  `Domain → Application → Infrastructure → Web → Tests`

## Layer-basierte Reihenfolge

Features werden **strikt per Layer** implementiert:

1. **Domain:** Entity, Value Objects, Enums, Domain Events (Ref: `docs/database_schema.md`, `docs/domain_ticket.md`)
2. **Application:** Commands, Queries, Validators, Handlers
3. **Infrastructure:** Repositories, EF Configurations, Services (Ref: `docs/nuget_stack.md`)
4. **Web:** Controllers, Views, ViewComponents, Partials (Ref: `docs/frontend_assets.md`)
5. **Tests:** Unit Tests (Domain + Application), Integration Tests (Ref: `tests/README.md`)

Jeder Layer-Schritt ist ein **eigenes Sub-Issue** mit eigenem Commit.

</github_issues_rules>
