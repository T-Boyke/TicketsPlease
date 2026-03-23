# 🧪 TicketsPlease - Quality Assurance

<!-- markdownlint-disable MD033 -->
<quality_assurance>
  <tdd_cycle>

- Rule: Red (Fail) -> Green (Pass) -> Refactor (Keep Green). TDD is MANDATORY.
- Workflow: Write tests BEFORE business logic.

  </tdd_cycle>
  <test_structure>

- Architecture: NetArchTest (Layer dependencies).
- Unit: xUnit (Facts & Theories). Mock time predictably via `FakeTimeProvider`.
- Integration: Testcontainers SQL Server (Persistence completeness).
- System: Playwright E2E.

  </test_structure>
  <fortification>

- Mutation Testing: Stryker.NET ensures test validity. Surviving mutants are failures.
- Data Generation: Bogus generates production-like entities to fuzz inputs.
- Visual/Complex Assertions: Snapshot testing via `VerifyTests`.

  </fortification>
  <quality_gates>

- Build: Warnings = Errors.
- Tests: Must be 100% green. 100% Line Coverage (Domain/App). 100% Stryker score.
- Lighthouse: Score 100/100 (Perf, a11y).

  </quality_gates>
</quality_assurance>
