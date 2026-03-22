# 🏛️ TicketsPlease - Architectural Blueprint

<architectural_blueprint>
<layers>

- Direction: Web 🔵 -> Application 🟡 -> Domain 🟢 <- Infrastructure 🔴
- Domain: Rich Models. ZERO external dependencies.
- Application: Commands, Queries, Interfaces.
- Infrastructure: Persistence, External Services.
- Web: ViewModels, Views, Controllers.
  </layers>
  <cqrs_pipeline>
- Flow: LoggingBehavior -> ValidationBehavior -> TransactionBehavior -> Handler.
- Rule: FluentValidation is MANDATORY.
  </cqrs_pipeline>
  <ef_core_resilience>
- Reads: `AsNoTracking()` ONLY.
- Writes: `RowVersion` (Optimistic Concurrency) exclusively.
- Policy: SQL Retry-Policy active in `AppDbContext`.
  </ef_core_resilience>
  </architectural_blueprint>
