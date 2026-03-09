# ADR 0062: Database Verification Strategy

- Status: Proposed

## Context

In a Code-First approach, ensuring that relationship constraints (Foreign Keys, Delete Behavior) are
correctly implemented and enforced is critical. We need a way to verify this without manual
database inspection.

## Decision

We implement a two-tier automated verification strategy:

1. **Architecture Tests (Statisch)**:
   - Use `NetArchTest.eXtend` to verify that all Domain Entities follow mapping conventions.
   - Rule: Every navigation property (e.g., `User AssignedUser`) must have a corresponding ID
     property (e.g., `Guid? AssignedUserId`).
   - Rule: Every Entity must inherit from `BaseEntity`.
2. **Integration Tests (Dynamisch)**:
   - Use `Microsoft.AspNetCore.Mvc.Testing` and `WebApplicationFactory`.
   - Use a real database provider (SQL Server via `Testcontainers` or SQLite In-Memory as fallback).
   - Tests verify that `SaveChangesAsync()` throws exceptions on FK violations and that `Include()`
     correctly loads related data.

## Consequences

- **Positive**: Automated detection of relationship "drift".
- **Positive**: Documentation of intent (e.g., `Restrict` vs `Cascade`) through executable tests.
- **Negative**: Requires a configured CI/CD environment with Docker support for full MSSQL
  verification.
- **Neutral**: `AppDbContext` must be provider-agnostic for some configurations (e.g.,
  `RowVersion`).

## Mapping Verification Example

| Entity    | Relationship  | Delete Behavior   |
| :-------- | :------------ | :---------------- |
| Ticket    | 1:n to User   | Restrict          |
| SubTicket | n:1 to Ticket | Cascade (Planned) |
