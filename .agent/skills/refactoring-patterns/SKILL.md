---
name: refactoring-patterns
description: Guides safe refactoring in Clean Architecture projects. Covers
  Extract Method, Replace Primitive with Value Object, Introduce Repository,
  and Extract Feature patterns. Use when restructuring code, improving design,
  or reducing technical debt while maintaining test coverage.
---

# 🔄 Refactoring Patterns

Sichere Refactoring-Strategien für Clean Architecture Projekte.

> **Referenz:** [Architecture Rules](file:///d:/DEV/Tickets/.agent/rules/architecture.md) | [Testing Rules](file:///d:/DEV/Tickets/.agent/rules/testing.md)

---

## Wann dieses Skill verwenden

- Code-Qualität verbessern (Tech Debt reduzieren)
- Bestehende Logik in Clean Architecture Patterns überführen
- Primitive Obsession durch Value Objects ersetzen
- Feature aus monolithischerm Code extrahieren

---

## Goldene Regeln

1. **Tests zuerst** – Bestehende Tests MÜSSEN grün bleiben
2. **Kleine Schritte** – Ein Refactoring pro Commit
3. **NetArchTest** – Layer-Dependencies nach jedem Refactoring prüfen
4. **Kein Feature-Creep** – Refactoring ≠ neue Features

---

## Pattern-Katalog

### 1. Extract Value Object

**Symptom:** Primitive Obsession – `string email`, `int priority` ohne Validierung.

```text
VORHER                          NACHHER
────────────────────           ────────────────────
Entity:                        Entity:
  string Email { get; }          EmailAddress Email { get; }
  int Priority { get; }         PriorityLevel Priority { get; }

                               ValueObjects:
                                 EmailAddress (sealed record)
                                 PriorityLevel (sealed record)
```

**Schritte:**

1. Unit-Test für Value Object schreiben (Validierung, Equality)
2. Value Object als `sealed record` erstellen
3. Entity-Property Typ ändern
4. EF Core Configuration anpassen (Value Conversion)
5. Bestehende Tests anpassen
6. `dotnet build` + `dotnet test`

### 2. Extract Repository

**Symptom:** Handler greift direkt auf `AppDbContext` zu.

```text
VORHER                          NACHHER
────────────────────           ────────────────────
Handler:                       Handler:
  AppDbContext _ctx;             IEntityRepository _repo;
  _ctx.Entities.Where(...)       _repo.GetByIdAsync(id)

                               Application/Contracts:
                                 IEntityRepository.cs

                               Infrastructure/Repositories:
                                 EntityRepository.cs
```

**Schritte:**

1. Interface in Application/Contracts definieren
2. Repository in Infrastructure implementieren
3. DI-Registrierung hinzufügen
4. Handler auf Interface umstellen
5. Unit-Tests: Mock statt InMemory-DB
6. `dotnet build` + `dotnet test`

### 3. Extract Feature (Command/Query)

**Symptom:** Business-Logik im Controller oder zu großer Handler.

```text
VORHER                          NACHHER
────────────────────           ────────────────────
Controller:                    Controller:
  [HttpPost]                     [HttpPost]
  Create(model) {                Create(model) {
    // 20 Zeilen Logik             _mediator.Send(cmd);
    _ctx.SaveChanges();          }
  }
                               Application/Features:
                                 CreateEntityCommand.cs
                                 CreateEntityCommandValidator.cs
                                 CreateEntityCommandHandler.cs
```

**Schritte:**

1. Command/Query Klasse erstellen
2. Validator erstellen (bei Command)
3. Handler erstellen (Logik aus Controller)
4. Controller auf `IMediator.Send()` umstellen
5. Unit-Tests für Handler schreiben
6. `dotnet build` + `dotnet test`

### 4. Split Fat Handler

**Symptom:** Handler mit >50 Zeilen, mehrere Verantwortlichkeiten.

**Schritte:**

1. Verantwortlichkeiten identifizieren
2. Domain-Logik → Entity-Methoden verschieben
3. Infrastruktur-Logik → Services/Repositories
4. Validierung → FluentValidation Validator
5. Side Effects → Domain Events + Event Handler
6. `dotnet build` + `dotnet test`

---

## Refactoring-Checkliste

| # | Vor dem Refactoring | Status |
|---|---|---|
| 1 | Bestehende Tests sind grün | ☐ |
| 2 | Scope klar definiert (keine Feature-Änderung) | ☐ |
| 3 | Betroffene Layer identifiziert | ☐ |

| # | Nach dem Refactoring | Status |
|---|---|---|
| 4 | Alle bestehenden Tests grün | ☐ |
| 5 | Neue Tests für extrahierten Code | ☐ |
| 6 | `dotnet build` fehlerfrei | ☐ |
| 7 | NetArchTest: Layer-Dependencies korrekt | ☐ |
| 8 | XML-Docs vollständig | ☐ |
| 9 | Atomic Commit (Conventional Commits) | ☐ |

---

### Skill: refactoring-patterns v1.0
