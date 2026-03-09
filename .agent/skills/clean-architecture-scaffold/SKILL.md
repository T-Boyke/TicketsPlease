---
name: clean-architecture-scaffold
description: Scaffolds a complete feature across all Clean Architecture layers
  (Domain, Application, Infrastructure, Web). Use when creating a new feature,
  endpoint, or use case from scratch. Guides layer-by-layer file creation with
  correct naming, structure, and dependencies.
---

# 🏗️ Clean Architecture Scaffold

Dieses Skill scaffoldet ein komplettes Feature über alle Layer der Clean Architecture.

> **Referenz:** [Architecture Rules](file:///d:/DEV/Tickets/.agent/rules/architecture.md) |
> [/add-cqrs-feature](file:///d:/DEV/Tickets/.agent/workflows/add-cqrs-feature.md)

---

## Wann dieses Skill verwenden

- Neues Feature / Use Case wird von Grund auf erstellt
- Neuer API-Endpoint mit zugehöriger Business-Logik
- User fragt nach "erstelle mir ein Feature für X"

---

## Entscheidungsbaum

```text
Neues Feature?
├── Liest nur Daten → Query
│   └── GetXxxQuery + GetXxxQueryHandler + XxxDto
├── Ändert Daten → Command
│   └── VerbXxxCommand + VerbXxxCommandValidator + VerbXxxCommandHandler
└── Beides → Command + Query (getrennte Dateien!)
```

---

## Scaffold-Reihenfolge (Bottom-Up)

### Layer 1: 🟢 Domain (`src/TicketsPlease.Domain/`)

```csharp
Entities/[EntityName].cs          → BaseEntity, private set, Create()-Factory
ValueObjects/[VOName].cs          → sealed record, Validierung im Konstruktor
Events/[EventName].cs             → sealed record : INotification
Enums/[EnumName].cs               → Wenn Enum nötig
```

**Checkliste:**

- [ ] `BaseEntity` erbt, `Guid Id`, `byte[] RowVersion`
- [ ] Alle Properties: `{ get; private set; }`
- [ ] `static Create(...)` Fabrikmethode (kein öffentlicher Konstruktor)
- [ ] Private EF-Core Konstruktor: `private EntityName() { }`
- [ ] Collections: `IReadOnlyList<T>` extern, `List<T>` intern
- [ ] XML-Docs auf **allen** public Members

### Layer 2: 🟡 Application (`src/TicketsPlease.Application/`)

```csharp
Features/[FeatureName]/
├── Commands/
│   ├── VerbEntityCommand.cs           → IRequest<T>
│   ├── VerbEntityCommandValidator.cs  → AbstractValidator<T>
│   └── VerbEntityCommandHandler.cs    → IRequestHandler<T, R>
├── Queries/
│   ├── GetEntityQuery.cs              → IRequest<T>
│   └── GetEntityQueryHandler.cs       → IRequestHandler<T, R>
└── EntityDetailDto.cs / EntityListItemDto.cs
```

```csharp
Contracts/Persistence/
└── IEntityRepository.cs              → Interface für Repository
```

**Checkliste:**

- [ ] Command: Nur primitive Typen / IDs als Properties
- [ ] Validator: `NotEmpty()`, `MaximumLength()`, `IsInEnum()` wo nötig
- [ ] Handler: Repository via Interface injizieren (nie DbContext direkt)
- [ ] `CancellationToken` bis zum letzten Async-Call durchreichen
- [ ] Write-Handler: `DbUpdateConcurrencyException` fangen
- [ ] Application Exceptions: `NotFoundException`, `ValidationException`
- [ ] XML-Docs auf **allen** public Members

### Layer 3: 🔴 Infrastructure (`src/TicketsPlease.Infrastructure/`)

```csharp
Persistence/
├── Repositories/EntityRepository.cs   → Implementiert IEntityRepository
└── Configurations/EntityConfiguration.cs → IEntityTypeConfiguration<T>
```

**Checkliste:**

- [ ] Repository implementiert das Application-Interface
- [ ] Queries: `AsNoTracking()` + `.Select()` Projection
- [ ] Configuration: `RowVersion` als `IsRowVersion()` konfigurieren
- [ ] DI-Registrierung in `InfrastructureServiceRegistration`

### Layer 4: 🔵 Web (`src/TicketsPlease.Web/`)

```csharp
Controllers/EntityController.cs       → MediatR.Send() only
Views/Entity/Index.cshtml             → Razor View (bei MVC)
```

**Checkliste:**

- [ ] Controller: KEINE Business-Logik, nur `IMediator.Send()`
- [ ] `[ValidateAntiForgeryToken]` auf POST-Actions
- [ ] `[Authorize]` auf schützenswerten Endpunkten
- [ ] Semantisches HTML, `aria-label`, Keyboard-Nav

### Layer 5: 🧪 Tests (`tests/`)

```csharp
TicketsPlease.Application.Tests/
├── Features/[FeatureName]/
│   └── VerbEntityCommandHandlerTests.cs
TicketsPlease.Domain.Tests/
└── Entities/EntityNameTests.cs
```

**Checkliste:**

- [ ] TDD: Test ZUERST schreiben!
- [ ] AAA-Pattern: Arrange → Act → Assert
- [ ] Happy-Path + Fehlerfälle (Validation, NotFound, Concurrency)
- [ ] `FluentAssertions` für lesbare Asserts
- [ ] Naming: `Handle_ValidCommand_ReturnsId`

---

## Dateien pro Feature (Zusammenfassung)

| # | Datei | Layer |
| --- | --- | --- |
| 1 | `Entity.cs` | Domain |
| 2 | `IEntityRepository.cs` | Application/Contracts |
| 3 | `VerbEntityCommand.cs` | Application/Features |
| 4 | `VerbEntityCommandValidator.cs` | Application/Features |
| 5 | `VerbEntityCommandHandler.cs` | Application/Features |
| 6 | `EntityDetailDto.cs` | Application/Features |
| 7 | `EntityRepository.cs` | Infrastructure |
| 8 | `EntityConfiguration.cs` | Infrastructure |
| 9 | `EntityController.cs` | Web |
| 10 | `VerbEntityCommandHandlerTests.cs` | Tests |

---

### Skill: clean-architecture-scaffold v1.0
