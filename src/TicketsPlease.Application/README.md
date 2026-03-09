# 🟡 TicketsPlease.Application – Die Use Cases

Dieser Layer orchestriert die Geschäftsprozesse. Hier wird definiert, **was** die Anwendung tut, ohne sich um das **Wie** (Datenbank, UI) zu kümmern.

## 🍴 Git Branch
- **Branch:** `layer/application`
- Alle Änderungen am Application-Layer müssen auf diesem Branch erfolgen.

## 📋 Arbeitsanweisungen für Application-Entwickler

### 1. CQRS (Command Query Responsibility Segregation)

- Alle Operationen werden als **Commands** (schreibend) oder **Queries** (lesend) abgebildet.
- **Commands:** `[Verb][Entity]Command` (z.B. `CreateTicketCommand`).
- **Queries:** `Get[Entity]Query` (z.B. `GetTicketListQuery`).
- Nutze **MediatR** für das Dispatching.

### 2. Validation (FluentValidation)

- Jeder Command **muss** einen zugehörigen Validator besitzen.
- Business-Regeln, die keine Datenbankabfrage erfordern, gehören in den Validator.

### 3. DTOs & Mapping

- Kommuniziere nach außen (Web) nur über **DTOs** (Data Transfer Objects).
- Nutze **Mapster** für ein performantes Mapping zwischen Entities und DTOs.

### 4. Interfaces (Contracts)

- Definiere hier alle Interfaces für externe Dienste (`ITicketRepository`, `IMailService`).
- Die Implementierung erfolgt erst im Infrastructure-Layer.

## 📁 Struktur

- `Features/`: Ordner pro Feature-Bereich (z.B. `Tickets/`), darin Commands, Queries und DTOs.
- `Contracts/`: Interfaces für Repositories und Services.
- `Behaviors/`: MediatR Pipeline-Logik (Logging, Validation, Transaction).
- `Exceptions/`: Spezifische Anwendungs-Exceptions (z.B. `NotFoundException`).

---

## 🔗 Connectors
- **Domain Layer:** Wird genutzt, um Geschäftslogik auszuführen.
- **Web Layer:** Triggert diesen Layer über MediatR.
- **Infrastructure Layer:** Implementiert die hier definierten Contracts.

> [!TIP]
> Reiche den `CancellationToken` immer bis zum untersten Call durch!
