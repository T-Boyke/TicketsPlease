testing*qa_standards: objective: "Define complete testing standards; tests drive design (TDD)"
tdd_cycle: steps: RED: "Write a failing test" GREEN: "Write minimal code to pass the test" REFACTOR:
"Improve code while keeping tests green" rule: "Write tests BEFORE implementation" coverage_targets:
Domain: { target: "100%", desc: "Business rules; zero compromise" } Application_Handlers: { target:
"90%+", desc: "All commands/queries including error cases" } Infrastructure: { target:
"Integration", desc: "Testcontainers for SQL" } Web: { target: "E2E", desc: "Playwright + Vitest" }
naming_conventions: test_class: "[ClassName]Tests (e.g., CreateTicketCommandHandlerTests)"
test_method_success: "[Method]*[Scenario]_[ExpectedResult] (e.g.,
Handle_ValidCommand_ReturnsNewTicketId)" test_method_error:
"[Method]_[InvalidScenario]\_Throws[Exception] (e.g.,
Handle_TicketNotFound_ThrowsNotFoundException)" aaa_pattern: blocks: ["Arrange", "Act", "Assert"]
rule: "Mandatory visual separation via comments" unit_testing_rules: mocking: "Use Moq/NSubstitute
for repositories and interfaces" isolation: "Independent tests; no state sharing" scenarios: ["Happy
Path", "ValidationException", "NotFoundException"] async_check: "Verify CancellationToken
propagation" domain_logic: "Test rich model methods separately" integration_testing_rules:
testcontainers: "Use Testcontainers.MsSql; strictly no InMemoryDatabase" isolation: "Fresh database
per test run (automated container spin-up)" concurrency_check: "Test RowVersion with simultaneous
updates" tracking_check: "Verify AsNoTracking in queries" seed_data: "Use test-specific seeds; no
production seeds" architectural_tests: tool: "NetArchTest.Rules" purpose: "Detect layer dependency
violations automatically (e.g., Domain must not depend on Infrastructure)" assertions_framework:
rule: "FluentAssertions is mandatory" comparison: equal: "result.Should().Be(expected)" null_check:
"result.Should().NotBeNull()" exception: "act.Should().ThrowAsync<T>()" collections:
"list.Should().HaveCount(3).And.Contain(x => x.Title == 'Test')" verification_workflows: pre_commit:
"dotnet test --verbosity minimal" ci_cd: "PR fails on any test failure or Lighthouse score below
100/100"
