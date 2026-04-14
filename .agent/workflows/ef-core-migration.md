ef_core_migration_workflow: objective: "Standard procedure for database changes using EF Core
Code-First Migrations" steps: entity_modification: layer: "Domain" mandatory_properties: - "Inherit
from BaseEntity (Guid Id)" - "byte[] RowVersion with [Timestamp] (Concurrency)" - "Properties with
'private set' (Rich Model)" - "Value Objects for complex types" - "XML-Documentation for all
properties" dbcontext_mapping: layer: "Infrastructure (AppDbContext.cs)" method: "Fluent API in
OnModelCreating()" checklist: - "Data types (HasMaxLength, HasPrecision)" - "IsRowVersion() for
concurrency" - "Indices on search columns" - "Default values (HasDefaultValueSql)" - "Explicitly
defined FK-Relationships and Cascading" create_migration: command: | dotnet ef migrations add
[MigrationName] \
 --project src/TicketsPlease.Infrastructure \
 --startup-project src/TicketsPlease.Web naming_convention: "PascalCase (e.g.,
AddRowVersionToTicket)" verification_checklist: - "Up() correctness" - "Down() completeness
(Rollback-ready)" - "Mapping-Data types" - "Indices and Default-values" - "RowVersion configured as
rowversion" database_update: command: | dotnet ef database update \
 --project src/TicketsPlease.Infrastructure \
 --startup-project src/TicketsPlease.Web seeding_strategy: master_data: "Use HasData() in
OnModelCreating" test_data: "Separate Seed-Classes or Scripts" rule: "Use fixed Guid-IDs only"
concurrency_handling: rule: "Catch DbUpdateConcurrencyException in all write-handlers"
transaction_management: strategy: "Use CreateExecutionStrategy() for manual transactions"
documentation_cleanup: - "Update database_schema.md" - "Update Mermaid ERD-Diagram" - "Create ADR if
design shift occurred" testing: - "Update Integration-Tests (Testcontainers)" - "Verify RowVersion +
AsNoTracking behavior"
