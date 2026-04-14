architectural_blueprint: layers: direction: "Web 🔵 -> Application 🟡 -> Domain 🟢 <- Infrastructure
🔴" domain: "Rich models; zero external dependencies" application: "Commands, Queries, Interfaces"
infrastructure: "Persistence, External Services" web: "ViewModels, Views, Controllers"
testing_monitoring: "Cross-cutting via NetArchTest" cqrs_pipeline: flow: ["LoggingBehavior",
"ValidationBehavior", "TransactionBehavior", "Handler"] mandatory: "FluentValidation usage" outcome:
"Every request/command returns a predictable Result<T> type" ef_core_resilience: reads:
"AsNoTracking() only" writes: "RowVersion (Optimistic Concurrency) exclusively" policy: "SQL
Retry-Policy active in AppDbContext" architecture_enforcement: tool: "NetArchTest.eXtend" rules: -
"Domain must not have dependencies outside of System" - "Application must not depend on
Infrastructure or Web" - "Infrastructure must only be accessed through Application interfaces"
