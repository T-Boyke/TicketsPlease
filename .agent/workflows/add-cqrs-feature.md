---
description: Workflow for adding a new CQRS feature (Command/Query) to the
  TicketsPlease project.
---

# ⚡ Add CQRS Feature Workflow

Dieser Workflow beschreibt den vollständigen Ablauf, um ein neues Command oder Query in der TicketsPlease Clean Architecture zu implementieren.

> **Referenz:** [ADR-0009 (CQRS)](file:///d:/DEV/Tickets/docs/adr/0009-cqrs-mediatr.md) | [ADR-0010 (FluentValidation)](file:///d:/DEV/Tickets/docs/adr/0010-validation-fluentvalidation.md) | [instructions.md §4](file:///d:/DEV/Tickets/instructions.md)

---

## Pipeline-Kontext

Jeder Request durchläuft automatisch die MediatR Pipeline in dieser Reihenfolge:

```text
📨 Request → 📋 LoggingBehavior → ✅ ValidationBehavior → 🔄 TransactionBehavior → ⚙️ Handler → 📤 Response
```

---

## Schritte

### 1. Domain Entity prüfen

- Prüfe, ob die benötigte Entity in `src/TicketsPlease.Domain/Entities/` existiert.
- Die Entity **muss** von `BaseEntity` erben und `byte[] RowVersion` besitzen.
- Properties haben **`private set`** (Rich Model). Zustandsänderungen nur über Verhaltensmethoden.
- Falls eine neue Entity nötig ist → nutze den `/domain-entity` Workflow.

### 2. DTO / Contract definieren

- Erstelle bei Bedarf ein DTO in `src/TicketsPlease.Application/Features/[FeatureName]/`.
  - **Queries:** DTOs in einem `Queries/` Unterordner.
  - **Commands:** DTOs in einem `Commands/` Unterordner.
- Naming: `[Entity][Purpose]Dto` (z.B. `TicketDetailDto`, `TicketListItemDto`).

### 3. Request erstellen (Command oder Query)

- Erstelle eine `IRequest<T>` Klasse:
  - **Command:** `src/TicketsPlease.Application/Features/[FeatureName]/Commands/[Verb][Entity]Command.cs`
  - **Query:** `src/TicketsPlease.Application/Features/[FeatureName]/Queries/Get[Entity]Query.cs`
- Der Request sollte **alle** nötigen Daten als Properties enthalten (keine komplexen Objekte, nur primitive Typen oder IDs).

### 4. Validator erstellen (Pflicht für Commands!)

- Erstelle einen `AbstractValidator<T>` im selben Ordner wie den Command.
- Naming: `[CommandName]Validator` (z.B. `CreateTicketCommandValidator`).
- **Regeln:**
  - Validiere alle Pflichtfelder (`.NotEmpty()`, `.MaximumLength()`, `.Must()`).
  - Validiere Business-Constraints (z.B. "Priority muss gültiger Enum-Wert sein").
  - Der Validator wird **automatisch** von der `ValidationBehavior` Pipeline aufgerufen.

```csharp
/// <summary>
/// Validiert den <see cref="CreateTicketCommand"/> vor der Handler-Ausführung.
/// </summary>
public class CreateTicketCommandValidator : AbstractValidator<CreateTicketCommand>
{
    /// <summary>
    /// Initialisiert die Validierungsregeln für das Anlegen eines neuen Tickets.
    /// </summary>
    public CreateTicketCommandValidator()
    {
        RuleFor(c => c.Title)
            .NotEmpty().WithMessage("Titel ist ein Pflichtfeld.")
            .MaximumLength(150).WithMessage("Titel darf maximal 150 Zeichen lang sein.");

        RuleFor(c => c.Priority)
            .IsInEnum().WithMessage("Ungültige Priorität.");
    }
}
```

### 5. Handler implementieren

- Erstelle einen `IRequestHandler<TRequest, TResponse>` im selben Feature-Ordner.
- Naming: `[CommandName]Handler` (z.B. `CreateTicketCommandHandler`).
- **Pflicht-Regeln:**
  - Injiziere Repository-Interfaces (niemals `AppDbContext` direkt!).
  - **`CancellationToken` durchreichen** bis `ToListAsync(ct)` / `SaveChangesAsync(ct)`.
  - **Write-Ops:** `DbUpdateConcurrencyException` explizit fangen und User-Feedback geben.
  - Nutze Application-Exceptions (`NotFoundException`, `ValidationException`) statt generischer Exceptions.

```csharp
/// <summary>
/// Verarbeitet den <see cref="CreateTicketCommand"/> und persistiert ein neues Ticket.
/// </summary>
public class CreateTicketCommandHandler : IRequestHandler<CreateTicketCommand, Guid>
{
    private readonly ITicketRepository _ticketRepository;

    public CreateTicketCommandHandler(ITicketRepository ticketRepository)
    {
        _ticketRepository = ticketRepository;
    }

    /// <inheritdoc />
    public async Task<Guid> Handle(CreateTicketCommand request, CancellationToken cancellationToken)
    {
        var ticket = Ticket.Create(request.Title, request.Description, request.GeoIpTimestamp);
        await _ticketRepository.AddAsync(ticket, cancellationToken);
        return ticket.Id;
    }
}
```

### 6. Repository erweitern (falls nötig)

- **Interface:** In `src/TicketsPlease.Application/Contracts/Persistence/I[Entity]Repository.cs`.
- **Implementierung:** In `src/TicketsPlease.Infrastructure/Persistence/Repositories/[Entity]Repository.cs`.
- **Queries:** Immer `AsNoTracking()` nutzen!
- **Projections:** `.Select(t => new Dto { ... })` bevorzugen statt `.Include()`.

### 7. Mapping registrieren (Mapster)

- Falls DTOs verwendet werden: Registriere Mappings in der Mapster-Konfiguration.
- Bevorzuge **explizite** Mapping-Konfigurationen über implizites Auto-Mapping.

### 8. Controller / API-Endpoint erstellen

- Erstelle oder erweitere einen Controller in `src/TicketsPlease.Web/Controllers/`.
- Der Controller sendet den Request **ausschließlich** via `IMediator.Send()`.
- **Keine** Business-Logik im Controller!
- Anti-Forgery Token für POST-Requests: `[ValidateAntiForgeryToken]`.

### 9. XML-Dokumentation

- **Alle** neuen `public` Members müssen vollständige XML-Kommentare haben:
  - `<summary>`, `<param>`, `<returns>`, `<exception>` wo zutreffend.

### 10. Unit-Test schreiben (TDD!)

- Erstelle einen Unit-Test im `tests/` Projekt.
- Naming: `[HandlerName]Tests` → `Handle_[Scenario]_[ExpectedResult]`.
- Mocke Repositories via `Moq` oder `NSubstitute`.
- Nutze `FluentAssertions` für lesbare Asserts.
- Teste **sowohl** den Happy-Path als auch Fehlerfälle (Validation, NotFound, Concurrency).

---

### Zusammenfassung

Checkliste: Entity ✓ → DTO ✓ → Request ✓ → Validator ✓ → Handler ✓ →
Repository ✓ → Mapping ✓ → Controller ✓ → XML-Docs ✓ → Test ✓
