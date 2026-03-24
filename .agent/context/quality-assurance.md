quality_assurance:
  tdd_cycle:
    rule: "Red (Fail) -> Green (Pass) -> Refactor (Keep Green); TDD is mandatory"
    workflow: "Write tests before business logic"
  test_structure:
    architecture: "NetArchTest (layer dependencies)"
    unit: "xUnit (Facts & Theories); predictable time mocking via FakeTimeProvider"
    integration: "Testcontainers SQL Server (persistence completeness)"
    system: "Playwright E2E"
  fortification:
    mutation_testing: "Stryker.NET ensures test validity; surviving mutants are failures"
    data_generation: "Bogus generates production-like entities to fuzz inputs"
    visual_complex_assertions: "Snapshot testing via VerifyTests"
  quality_gates:
    build: "Treat warnings as errors"
    tests: "100% green; 100% line coverage (Domain/App); 100% Stryker score"
    lighthouse: "Score 100/100 (Performance, Accessibility)"
