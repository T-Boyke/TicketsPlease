# 🧪 TicketsPlease - Testing Rules

<!-- markdownlint-disable MD033 -->
<testing_rules>
<fundamentals>

- TDD: RED-GREEN-REFACTOR cycle is STRICTLY MANDATORY.
- AAA Pattern: Arrange, Act, Assert blocks must be visually separated by comments:
  `// Arrange`, `// Act`, `// Assert`.
- Naming: `[ClassName]Tests` -> `[Method]_[Scenario]_[Result]`.

</fundamentals>

<coverage_mutation>

- Line Coverage: 100% minimum on Domain and Application logic. Zero compromise.
- Mutation Testing (Stryker.NET): Tests must fail when code is mutated.
  Objective is 100% Mutation Score for core logic.

</coverage_mutation>

<xunit_features>

- Single scenarios: Use `[Fact]`.
- Edge cases/Parameterization: Use `[Theory]` and `[InlineData]` or `[ClassData]`.
  Do NOT duplicate code for edge cases.

</xunit_features>

<mocking_data>

- Data Builders: Use `Bogus` to generate realistic Fakes (Names, UUIDs). NO hardcoded `TestUser`.
- Determinism: Use .NET 8 `TimeProvider` interface and `FakeTimeProvider`. NEVER use `DateTime.UtcNow`.
- Mocks: `Moq` or `NSubstitute`. Fakes must act strictly to avoid hidden interactions.

</mocking_data>

<test_layers>

- Architecture: NetArchTest (Ensures Clean Architecture dependency rules are unbroken).
- Unit: Domain logic, MediatR Handlers, Services.
- Integration: Testcontainers for SQL Server. NEVER use InMemoryDB.
  Verify `AsNoTracking` and concurrency changes.
- System/E2E: Playwright for crucial Web Journeys.

</test_layers>

<assertions>

- Framework: `FluentAssertions` is MANDATORY (`.Should().NotBeNull()`).
- Snapshots: Use `VerifyTests` for asserting complex JSON, HTML, or large DTO results
  instead of manual field mapping.

</assertions>

<ci_cd>

- Rule: CI pipeline fails immediately on any warnings or failing tests.

</ci_cd>
</testing_rules>
