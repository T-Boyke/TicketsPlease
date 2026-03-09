# 🟡 TicketsPlease.Application – Die Use Cases

Dieser Layer orchestriert die Geschäftsprozesse. Hier wird definiert, **was** die Anwendung tut,
ohne sich um das **Wie** (Datenbank, UI) zu kümmern.

## 🍴 Git Branch

- **Branch:** `layer/application`
- Alle Änderungen am Application-Layer müssen auf diesem Branch erfolgen.

---

## 📋 Arbeitsanweisung: Wie füge ich einen Use Case hinzu?

Wir nutzen das **Vertical Slice Pattern** innerhalb der Features. Ein Use Case besteht meist aus
einer einzigen Datei, die Command/Query, Validator und Handler bündelt.

### 1. Feature-Ordner finden

Navigiere zu `Features/[Kategorie]/`. Erstelle bei Bedarf einen neuen Ordner.

### 2. Die Command/Query Datei erstellen

Erstelle eine Datei (z.B. `CreateTicket.cs`) mit folgendem Aufbau:

```csharp
public record CreateTicketCommand(string Title, string Description) : IRequest<Guid>;

public class CreateTicketValidator : AbstractValidator<CreateTicketCommand> {
    public CreateTicketValidator() {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
    }
}

public class CreateTicketHandler : IRequestHandler<CreateTicketCommand, Guid> {
    private readonly ITicketRepository _repo;
    public CreateTicketHandler(ITicketRepository repo) => _repo = repo;

    public async Task<Guid> Handle(CreateTicketCommand request, CancellationToken ct) {
        // Logik hier...
    }
}
```

### 3. Pipeline-Behaviors

MediatR führt automatisch Validierungen, Logging und Transaktions-Management durch. Du musst dich
im Handler nicht darum kümmern!

---

## 🛠️ Dependency Injection (DI) Connector

Alle Services und Handler dieses Layers werden automatisch registriert:

- **Ort**: `DependencyInjection.cs`
- **Methode**: `AddApplicationServices`
- **Wichtig**: Wenn du einen neuen Handler schreibst, wird er per Reflection gefunden. Es ist kein
  manuelles Registrieren nötig!

---

## 📁 Struktur

- `Behaviors/`: Pipeline-Logik (Cross-Cutting Concerns).
- `Contracts/`: Interfaces (z.B. `ITicketRepository`), die von der Infrastructure implementiert werden.
- `Features/`: Alle Geschäftsprozesse (Commands & Queries).
- `Mappings/`: Konfiguration für Mapster.

---

## 🔗 Connectors

- **Domain Layer:** Wird genutzt, um Geschäftslogik auszuführen.
- **Web Layer:** Triggert diesen Layer via `ISender.Send()`.
- **Infrastructure Layer:** Implementiert die hier definierten Contracts.

> [!TIP]
> Halte Handler "dumm". Die echte Logik gehört in die Domain-Entities!
