# 🏛️ TicketsPlease - Architectural Blueprint

<architectural_blueprint>
  <layers>
    - Direction: Web 🔵 -> Application 🟡 -> Domain 🟢 <- Infrastructure 🔴
    - Domain: Rich Models. ZERO external dependencies.
    - Application: Commands, Queries, Interfaces.
    - Infrastructure: Persistence, External Services.
    - Web: ViewModels, Views, Controllers.
    - Testing: Cross-cutting. Monitors all layers via `NetArchTest`.
  </layers>
  <cqrs_pipeline>
    - Flow: LoggingBehavior -> ValidationBehavior -> TransactionBehavior -> Handler.
    - Rule: FluentValidation is MANDATORY.
    - Result: Every request/command returns a predictable `Result<T>` type.
  </cqrs_pipeline>
  <ef_core_resilience>
    - Reads: `AsNoTracking()` ONLY.
    - Writes: `RowVersion` (Optimistic Concurrency) exclusively.
    - Policy: SQL Retry-Policy active in `AppDbContext`.
  </ef_core_resilience>
  <architecture_enforcement>
    - Tool: `NetArchTest.eXtend`.
    - Rules:
      - Domain must not have dependencies outside of System.
      - Application must not depend on Infrastructure or Web.
      - Infrastructure must only be accessed through Application interfaces.
  </architecture_enforcement>
</architectural_blueprint>
