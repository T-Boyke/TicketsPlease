# 🏛️ TicketsPlease - Architecture Rules

<architecture_rules>
<dependency_rule>

- Direction: Web 🔵 -> Application 🟡 -> Domain 🟢 <- Infrastructure 🔴
- Domain: NO dependencies (except MediatR.Contracts).
- Application: Defines interfaces. Infrastructure: Implements them.
- Web: Delegates to MediatR. ZERO business logic in Controllers. Enforced by NetArchTest.
  </dependency_rule>

  <ddd>
  - Models: Rich Models with logic. NO anemic entities.
  - Properties: Private setters `{ get; private set; }`. Use `static Create(...)`. No empty constructors.
  - ValueObjects: Encapsulate logic (e.g. EmailAddress, Sha1Hash).
  - Events: Decouple side-effects via INotification.
  - Collections: IReadOnlyList<T> exposed, List<T> internal.
  - Concurrency: RowVersion (`byte[]`, `[Timestamp]`) is MANDATORY on Domain Entities.
  </ddd>

<cqrs_mediatr>

- Pipeline: Request -> LoggingBehavior -> ValidationBehavior -> TransactionBehavior -> Handler.
- Validators: AbstractValidator<T> (FluentValidation) per command.
- Mapping: Mapster ONLY (No AutoMapper).
- Async: Pass CancellationToken everywhere.
- Exceptions: ValidationException, NotFoundException, BadRequestException.
  </cqrs_mediatr>

<ef_core>

- Queries: ALWAYS `AsNoTracking()`. Use `.Select()` projections, NOT `.Include()`.
- Writes: Catch `DbUpdateConcurrencyException`.
- Transactions: Use `CreateExecutionStrategy()` for manual tx.
- Migrations: `dotnet ef migrations add [Name] --project src/TicketsPlease.Infrastructure --startup-project src/TicketsPlease.Web`.
- DB Schema: Strict 3rd Normal Form (3NF). No denormalization without ADR.
  </ef_core>

<naming_conventions>

- Interfaces: `I[Name]`. Private Fields: `_[name]`.
- CQRS: `[Verb][Entity]Command`, `Get[Entity]Query`, `[Request]Handler`, `[Request]Validator`.
- DTO: `[Entity][Purpose]Dto`.
- Tests: `[Class]Tests` -> `[Method]_[Scenario]_[Expected]`.
  </naming_conventions>

<cqrs_file_bundling>

- Enforce bundling: Command/Query, Validator, and Handler for a specific Use Case MUST reside in ONE single file to maximize token efficiency.
- Other elements: 1 Class per File strictly.
  </cqrs_file_bundling>
  </architecture_rules>
