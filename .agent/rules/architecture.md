# 🏛️ TicketsPlease – Architecture Rules

Diese Regeln erzwingen die Clean Architecture, DDD und CQRS Standards bei jeder
Code-Änderung.

---

## Clean Architecture – Dependency Rule

```text
Abhängigkeiten zeigen IMMER NUR NACH INNEN (→ Domain):

  🔵 Web → 🟡 Application → 🟢 Domain ← 🔴 Infrastructure
```

- **Domain** hat **KEINE** externen Abhängigkeiten (Ausnahme:
  `MediatR.Contracts`).
- **Application** definiert Interfaces. **Infrastructure** implementiert sie.
- **Web/Controller** delegieren alles an MediatR. Keine Business-Logik im
  Controller.
- Verletze **niemals** die Dependency Direction. `NetArchTest` prüft
  automatisch.

---

## Projekt-Struktur

```text
src/
├── TicketsPlease.Domain/          # 🟢 Entities, Value Objects, Events, Enums
├── TicketsPlease.Application/     # 🟡 Features/{Name}/Commands|Queries,
│                                  # Contracts, Behaviors, Exceptions
├── TicketsPlease.Infrastructure/  # 🔴 Persistence (DbContext, Repos, Migrations),
│                                  # Services, Identity
└── TicketsPlease.Web/             # 🔵 Controllers, Views, wwwroot, css/components/
```

---

## Domain-Driven Design (DDD)

- **Rich Models** – Entities sind keine Datencontainer. Sie enthalten
  Business-Logik.
- **Private Setter** – Alle Properties: `{ get; private set; }`.
- **Fabrikmethoden** – Kein leerer Konstruktor. Pflichtfelder über
  `static Create(...)` erzwingen.
- **Value Objects** – Komplexe Typen kapseln: `EmailAddress`, `Sha1Hash`,
  `PriorityLevel`.
- **Domain Events** – Seiteneffekte über `INotification` entkoppeln.
- **Immutable Collections** – Extern `IReadOnlyList<T>`, intern `List<T>`.
- **RowVersion** – Pflicht auf allen Domain-Entities (`byte[] RowVersion`,
  `[Timestamp]`).

---

## CQRS & MediatR

- Pipeline:
  `Request → LoggingBehavior → ValidationBehavior → TransactionBehavior → Handler`
- Jeder Command hat einen `AbstractValidator<T>` (FluentValidation).
- Mapping über **Mapster** (kein AutoMapper).
- `CancellationToken` bis zum letzten Async-Call durchreichen.
- Application Exceptions: `ValidationException`, `NotFoundException`,
  `BadRequestException`.

---

## EF Core Strict Policy

- **Queries:** Immer `AsNoTracking()`.
- **Projections:** `.Select(t => new Dto { ... })` statt `.Include()`.
- **Concurrency:** `DbUpdateConcurrencyException` in jedem Write-Handler fangen.
- **Transaktionen:** `CreateExecutionStrategy()` für manuelle Transaktionen.
- **Migrations:**
  `dotnet ef migrations add [Name] --project src/TicketsPlease.Infrastructure --startup-project src/TicketsPlease.Web`
- **Schema:** 3. Normalform (3NF). Keine Denormalisierung ohne ADR.

---

## Naming Conventions

| Element       | Pattern                          | Beispiel                          |
| ------------- | -------------------------------- | --------------------------------- |
| Interface     | `I[Name]`                        | `ITicketRepository`               |
| Private Field | `_[name]`                        | `_ticketRepository`               |
| Command       | `[Verb][Entity]Command`          | `CreateTicketCommand`             |
| Query         | `Get[Entity]Query`               | `GetTicketDetailQuery`            |
| Handler       | `[Request]Handler`               | `CreateTicketCommandHandler`      |
| DTO           | `[Entity][Purpose]Dto`           | `TicketDetailDto`                 |
| Validator     | `[Request]Validator`             | `CreateTicketCommandValidator`    |
| Test-Klasse   | `[Class]Tests`                   | `CreateTicketCommandHandlerTests` |
| Test-Methode  | `[Method]_[Scenario]_[Expected]` | `Handle_ValidCommand_ReturnsId`   |

---

## Datei-Disziplin & CQRS-Bündelung

- **CQRS-Bündelung:** Um die Token-Effizienz zu steigern und Fragmentierung zu
  vermeiden, werden `Command`/`Query`, `Validator` und `Handler` eines
  spezifischen Use Cases **zwingend** in einer einzigen Datei zusammengefasst.
- **Sonstige Elemente:** Für alle anderen Konstrukte (Entities, Value Objects,
  generische Interfaces, Enums) gilt weiterhin strikt: **1 Klasse pro Datei**.

---

## TicketsPlease Architecture Rules v1.0 | 2026-03-06
