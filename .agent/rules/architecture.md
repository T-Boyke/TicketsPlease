architecture*rules: dependency_flow: direction: "Web 🔵 -> Application 🟡 -> Domain 🟢 <-
Infrastructure 🔴" domain: "Zero dependencies (except MediatR.Contracts)" application: "Defines
interfaces" infrastructure: "Implements interfaces" web: "Delegates to MediatR; zero business logic
in Controllers (enforced by NetArchTest)" ddd_principles: models: "Rich Domain Models; no anemic
entities" properties: "Private setters ({ get; private set; }); use static Create(...) factory
methods; no empty constructors" value_objects: "Encapsulate logic (e.g., EmailAddress, Sha1Hash)"
events: "Decouple side-effects via INotification" collections: "Expose IReadOnlyList<T>; internal
List<T>" concurrency: "RowVersion (byte[] with [Timestamp]) is mandatory on Domain Entities"
cqrs_mediatr: pipeline: ["Request", "LoggingBehavior", "ValidationBehavior", "TransactionBehavior",
"Handler"] validators: "AbstractValidator<T> (FluentValidation) per command" mapping: "Mapster only
(No AutoMapper)" async: "Pass CancellationToken everywhere" exceptions: ["ValidationException",
"NotFoundException", "BadRequestException"] ef_core: queries: "Always use AsNoTracking(); prioritize
.Select() projections over .Include()" writes: "Catch DbUpdateConcurrencyException" transactions:
"Use CreateExecutionStrategy() for manual transactions" migrations: "dotnet ef migrations add [Name]
--project src/TicketsPlease.Infrastructure --startup-project src/TicketsPlease.Web" schema: "Strict
3rd Normal Form (3NF); no denormalization without ADR" naming_conventions: interfaces: "I[Name]"
private_fields: "*[name]" cqrs: ["{Verb}{Entity}Command", "Get{Entity}Query", "{Request}Handler",
"{Request}Validator"] dto: "{Entity}{Purpose}Dto" tests: "{Class}Tests ->
{Method}_{Scenario}_{Expected}" file_bundling: cqrs: "Command/Query, Validator, and Handler for a
specific Use Case MUST be in ONE single file" others: "Strictly one Class per File for all other
elements"
