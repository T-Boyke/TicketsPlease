ef_core_debugging_skill:
  name: "ef-core-debugging"
  description: "Debugs and optimizes EF Core queries, detects N+1 issues, analyzes plans, and troubleshoots migrations"
  scenarios: ["Slow queries", "N+1 problems", "Concurrency errors", "Migration failures", "Unexpected behavior"]
  diagnosis_tree:
    Slow_Queries: ["N+1 -> Use .Select()", "Missing indices -> Add in migration", "Too many columns -> Projections"]
    Concurrency: ["Missing RowVersion", "Uncaught Exception", "Retry logic via ExecutionStrategy"]
    Migrations: ["Pending updates", "Snapshot desync", "Manual schema adjustments"]
    Tracking: ["Read-query tracking -> Add AsNoTracking()", "Detached entities -> Attach manually"]
  diagnosis_steps:
    logging: "Enable DB command info in appsettings.Development.json"
    n1_detection: "Single JOIN projection with .Select() over multiple .Include() calls"
    as_no_tracking: "Strictly mandatory for all reads; check via grep for leaks"
    concurrency: "Always catch DbUpdateConcurrencyException in write-handlers"
    indices: "Analyze Filter/Sort/Composite indices in EntityConfiguration.cs"
  performance_checklist:
    - "AsNoTracking() on all reads"
    - ".Select() projection over .Include()"
    - "N+1 query monitoring"
    - "Filter/Sort column indices"
    - "CancellationToken propagation"
    - "List pagination (Skip/Take)"
    - "RowVersion [Timestamp] configured"
    - "ExecutionStrategy for manual transactions"
  cli_tools:
    add: "dotnet ef migrations add [Name] --project src/TicketsPlease.Infrastructure --startup-project src/TicketsPlease.Web"
    update: "dotnet ef database update --project src/TicketsPlease.Infrastructure --startup-project src/TicketsPlease.Web"
    script: "dotnet ef migrations script --project src/TicketsPlease.Infrastructure --startup-project src/TicketsPlease.Web"
    remove: "dotnet ef migrations remove --project src/TicketsPlease.Infrastructure --startup-project src/TicketsPlease.Web"
  version: "1.0"
