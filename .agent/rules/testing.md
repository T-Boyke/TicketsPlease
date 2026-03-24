testing_rules:
  fundamentals:
    tdd_cycle: "RED-GREEN-REFACTOR is strictly mandatory"
    aaa_pattern: "Arrange, Act, Assert blocks must be visually separated by comments"
    naming_convention: "[ClassName]Tests -> [Method]_[Scenario]_[Result]"
  coverage_mutation:
    line_coverage: "100% minimum on Domain and Application logic (no exceptions)"
    mutation_testing: "Stryker.NET score of 100% for core logic"
  xunit_features:
    single_scenarios: "Use [Fact]"
    parameterization: "Use [Theory] with [InlineData] or [ClassData]; do not duplicate code for edge cases"
  mocking_data_generation:
    data_builders: "Use Bogus for realistic fakes (e.g., Names, UUIDs); no hardcoded 'TestUser'"
    determinism: "Use TimeProvider interface and FakeTimeProvider; never use DateTime.UtcNow"
    mocks_fakes: "Use Moq or NSubstitute; fakes must act strictly to avoid hidden interactions"
  testing_layers:
    architecture: "NetArchTest for Clean Architecture dependency verification"
    unit_tests: "Domain logic, MediatR Handlers, Services"
    integration_tests: "Use Testcontainers for SQL Server; strictly no InMemoryDB; verify AsNoTracking and concurrency"
    e2e_system_tests: "Playwright for critical web journeys"
  assertions:
    framework: "FluentAssertions is mandatory (.Should().NotBeNull())"
    snapshot_testing: "Use VerifyTests for asserting complex JSON, HTML, or large DTO results; avoid manual field mapping"
  ci_cd_policy:
    failure_rule: "CI pipeline must fail immediately on any warnings or failing tests"
