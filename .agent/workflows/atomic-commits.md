---
description: Workflow for ensuring small, logical, and atomic commits in the
  TicketsPlease project.
---

# ⚛️ Atomic Commit & Git Workflow

Um eine saubere, nachvollziehbare Git-Historie zu gewährleisten, müssen Commits atomar und logisch getrennt sein. Dieser Workflow umfasst die gesamte Git-Etikette des Projekts.

> **Referenz:** [README §7 – GitHub Etikette](file:///d:/DEV/Tickets/README.md) | [instructions.md §12](file:///d:/DEV/Tickets/instructions.md)

---

## Branching Strategy

### Branch-Naming Conventions

| Prefix | Verwendung | Beispiel |
| --- | --- | --- |
| `feature/` | Neues Feature | `feature/ticket-drag-and-drop` |
| `bugfix/` | Bugfix | `bugfix/fix-login-redirect` |
| `hotfix/` | Urgent Production Fix | `hotfix/patch-sql-injection` |
| `docs/` | Dokumentations-Änderungen | `docs/update-database-schema` |
| `refactor/` | Code-Refactoring | `refactor/extract-ticket-validator` |
| `test/` | Test-Erweiterungen | `test/add-handler-integration-tests` |

### Workflow

```bash
1. git checkout main
2. git pull origin main
3. git checkout -b feature/mein-neues-feature
4. ... Implementierung ...
5. Atomare Commits (siehe unten)
6. git push origin feature/mein-neues-feature
7. Pull Request erstellen → CI/CD → Code Review → Merge
```

---

## Atomare Commits

### 1. Vorbereitung

Prüfe vor dem Commit den Status:

```bash
// turbo
git status
```

### 2. Atomarität prüfen
- Gehören **alle** Änderungen zu **einem** logischen Task?
  - ✅ `feat: add RowVersion to Ticket entity` (eine logische Einheit)
  - ❌ `feat: add RowVersion AND fix login layout` (zwei Tasks = zwei Commits!)
- Falls nein: Nutze `git add <file>` oder `git add -p` zum selektiven Stagen.

### 3. Conventional Commits (Pflicht)

Format: `<type>(<scope>): <subject>`

```
feat(ticket): add SHA1 hash generation on creation
fix(auth): resolve null reference during login
docs(adr): add ADR-0021 for Redis caching strategy
style(web): apply editorconfig formatting to controllers
refactor(application): extract validation pipeline behavior
test(domain): add ticket close-rule unit tests
chore(deps): update MediatR to v13
```

| Feld | Regel |
| --- | --- |
| **Type** | `feat`, `fix`, `docs`, `style`, `refactor`, `test`, `chore` |
| **Scope** | Optional. Betroffener Layer/Bereich (z.B. `ticket`, `auth`, `domain`, `web`) |
| **Subject** | Kurz, prägnant, **Imperativ** ("add", nicht "added" oder "adding") |
| **Body** | Optional. Erkläre das **"Warum"**, nicht das "Wie" (bei komplexen Änderungen) |
| **Footer** | Optional. `Closes #42` oder `BREAKING CHANGE: ...` |

### 4. Keine Misch-Commits!

| ❌ Verboten | ✅ Stattdessen |
|---|---|
| Refactoring + neue Features | Erst Refactoring committen, dann Feature |
| Bugfix + Dokumentation | Erst Bugfix committen, dann Docs |
| Formatting + Code-Änderung | Erst `style:` Formatting, dann `feat:`/`fix:` |

### 5. Commit-Frequenz
- Committe lieber **zu oft** als zu selten.
- Ein "atomarer" Schritt = eine kleine Gruppe von Dateien, die zusammen eine funktionale Einheit bilden.
- **Beispiel-Sequenz für ein CQRS Feature:**
  1. `feat(domain): add Ticket entity with SHA1 hash`
  2. `feat(application): add CreateTicketCommand and validator`
  3. `feat(application): add CreateTicketCommandHandler`
  4. `feat(infrastructure): add TicketRepository implementation`
  5. `feat(web): add TicketController with create endpoint`
  6. `test(application): add CreateTicketHandler unit tests`
  7. `docs(adr): add ADR for ticket identification strategy`

### 6. Verifizierung vor Commit

| Prüfpunkt | Befehl | Pflicht? |
| --- | --- | --- |
| **CHANGELOG.md** | `[Unreleased]` Sektion aktualisieren | ✅ Bei `feat`, `fix`, `security` |
| **Kompiliert** | `dotnet build` | ✅ Ja |
| **Tests grün** | `dotnet test` | ✅ Ja |
| **Formatting** | `dotnet format --verify-no-changes` | ✅ Ja |
| **Keine Secrets** | Manuell prüfen | ✅ Ja |

> **Regel:** Die Solution **muss** nach jedem Commit kompilierbar sein. "Broken Commits" sind **untersagt**.

---

## Pull Request (PR) Regeln

| Regel | Beschreibung |
|---|---|
| **Pflicht** | Features werden **ausschließlich** über PRs in `main` gemerged. |
| **CI/CD grün** | Der PR benötigt zwingend das grüne Licht der GitHub Actions Pipeline. |
| **Code Review** | Mindestens **ein Approve** durch einen anderen Entwickler. |
| **Issue-Referenz** | Jeder PR verweist auf das zugehörige GitHub Issue (`Closes #42`). |
| **Beschreibung** | Der PR enthält eine aussagekräftige Beschreibung der Änderungen. |

---

## Der `main` Branch ist HEILIG

> [!CAUTION]
> - `main` muss **jederzeit** lauffähig sein (Compilable & Green Tests).
> - Direct Pushes auf `main` sind per **Branch-Protection** gesperrt.
> - Kein Code-Change ohne zugehöriges **GitHub Issue**.

---

*Regel: Ein Commit = Eine logische Änderung. Kein Code ohne Issue. Kein Merge ohne PR.*
