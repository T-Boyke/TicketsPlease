# ADR 0052: Domain Event Dispatching via MediatR
* Status: accepted
* Deciders: Tobias
* Date: 2026-03-08

## Context and Problem Statement
When business logic in the Domain Layer triggers side effects (e.g., sending an email after a ticket is created), creating a direct dependency on notification services violates the Clean Architecture "Dependency Rule." We need a way to trigger these side effects while keeping the Domain Layer pure and decoupled.

## Decision Drivers
* Separation of Concerns.
* Domain Layer purity (No external dependencies).
* Extensibility (Adding new side effects without changing core logic).
* Testability of individual handlers.

## Considered Options
* Direct calls to services from the Domain Layer (N-Tier style).
* Manual Observer pattern implementation.
* MediatR `INotification` and `INotificationHandler`.

## Decision Outcome
Chosen option: "MediatR INotification", because it provides a proven, standardized way to implement the "Publish-Subscribe" pattern in .NET applications, fitting perfectly into our CQRS stack.

### Positive Consequences
* Domain entities only publish what happened (`TicketCreatedEvent`), not who should react.
* New handlers (Logging, Mail, Audit) can be added in the Infrastructure or Application layers without touching the Domain.
* Handlers can be executed asynchronously, improving system responsiveness.

### Negative Consequences
* Logic flow becomes slightly harder to follow visually (Decoupling).
* Slight overhead due to reflection-based discovery of handlers (mitigated by MediatR optimizations).

## Implementation Details
1. Entities inherit from a `BaseEntity` that tracks a `List<IDomainEvent>`.
2. The `AppDbContext` (Infrastructure) intercepts `SaveChangesAsync`.
3. It collects all events from the tracked entities and publishes them via `IMediator`.
4. Handlers implement `INotificationHandler<TEvent>` and reside in the Application or Infrastructure layers.
