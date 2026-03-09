---
name: code-review
description: Reviews code changes against all TicketsPlease project standards
  including Clean Architecture, DDD, Security, Testing, UI/a11y, and
  documentation. Use when reviewing PRs, checking code quality, or validating
  changes before commit.
---

# 🔍 Code Review Skill

Konsolidierte Code-Review Checkliste basierend auf allen Projekt-Standards.

> **Referenz:** [Architecture Rules](file:///d:/DEV/Tickets/.agent/rules/architecture.md)
> | [Security Rules](file:///d:/DEV/Tickets/.agent/rules/security.md)
> | [Testing Rules](file:///d:/DEV/Tickets/.agent/rules/testing.md)
> | [UI Rules](file:///d:/DEV/Tickets/.agent/rules/ui-frontend.md)

---

## Wann dieses Skill verwenden

- Code-Review eines PRs oder Branches
- Qualitäts-Check vor einem Commit
- User fragt "prüfe meinen Code" oder "review das mal"

---

## Review-Ablauf

```text
1. 🏛️ Architecture Check (Dependency Direction)
2. 🧬 DDD Check (Rich Model, Encapsulation)
3. 🛡️ Security Check (Input Validation, XSS, DSGVO)
4. 🧪 Test Check (Coverage, TDD, AAA)
5. 🎨 UI/a11y Check (Tailwind, BFSG, Semantik)
6. 📖 Documentation Check (XML-Docs, ADRs)
7. 🔄 Git Check (Conventional Commits, Atomic)
```

---

## Checkliste (nach Severity)

### 🔴 Blocker (Merge-Verhindernd)

| # | Check | Beschreibung |
| --- | --- | --- |
| B1 | **Dependency Direction** | Domain darf NIE von Infrastructure/Web abhängen |
| B2 | **Secrets** | Keine Secrets in appsettings.json oder Code |
| B3 | **SQL Injection** | Keine String-Concatenation für Queries |
| B4 | **Tests fehlen** | Jede neue Logik braucht Tests |
| B5 | **Breaking Change** | Ohne Ankündigung/ADR nicht erlaubt |
| B6 | **Build broken** | `dotnet build` muss grün sein |

### 🟡 Major (Muss vor Merge gefixt werden)

| # | Check | Beschreibung |
| --- | --- | --- |
| M1 | **Private Setter** | Entity-Properties: `{ get; private set; }` |
| M2 | **CancellationToken** | Bis zum letzten Async-Call durchreichen |
| M3 | **AsNoTracking** | Alle Lese-Queries |
| M4 | **Concurrency** | `DbUpdateConcurrencyException` in Write-Ops fangen |
| M5 | **Validator fehlt** | Jeder Command braucht `AbstractValidator<T>` |
| M6 | **XML-Docs fehlen** | Alle neuen public Members |
| M7 | **Anti-Forgery** | `[ValidateAntiForgeryToken]` auf POST |
| M8 | **DOMPurify** | Markdown-Output sanitizen |
| M9 | **1 Class per File** | Keine Multi-Class Dateien |
| M10 | **Fabrikmethode** | `static Create(...)` statt öffentlicher Konstruktor |

### 🟢 Minor (Sollte gefixt werden)

| # | Check | Beschreibung |
| --- | --- | --- |
| N1 | **Naming Convention** | Projekt-Naming beachten (siehe Architecture Rules) |
| N2 | **Hardcoded Farben** | CSS Custom Properties verwenden |
| N3 | **ARIA-Attribute** | `aria-label`, `aria-expanded` wo nötig |
| N4 | **Semantisches HTML** | `<dialog>`, `<nav>`, `<button>` statt `<div>` |
| N5 | **DateTimeOffset** | Statt `DateTime` verwenden |
| N6 | **Projection** | `.Select()` statt `.Include()` bevorzugen |

---

## Feedback-Format

Strukturiere Feedback so:

```markdown
## 🔍 Code Review: [Feature/PR Name]

- **B1 – Dependency Direction:** `Domain.Entity` referenziert
  `Infrastructure.DbContext` direkt.
  → Entity darf keine Infrastructure-Abhängigkeit haben.

- **M2 – CancellationToken:** In `GetTicketQueryHandler.Handle()` fehlt `ct`
  bei `ToListAsync()`.
  → `await _repo.GetAllAsync(cancellationToken)` verwenden.

- **N5 – DateTimeOffset:** `DateTime.Now` in `Ticket.Create()` →
  `DateTimeOffset.UtcNow` verwenden.

### ✅ Positiv
- Saubere Trennung von Command und Query ✓
- FluentValidation korrekt implementiert ✓
```

---

### Skill: code-review v1.0
