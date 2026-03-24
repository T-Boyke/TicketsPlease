refactoring_patterns_skill:
  name: "refactoring-patterns"
  description: "Safe refactoring strategies for Clean Architecture: Extract Method, Value Object, Repository, Feature"
  scenarios: ["Tech debt reduction", "Clean Arch migration", "Value Object extraction", "Feature extraction"]
  golden_rules:
    - "Tests First: Keep existing tests green"
    - "Small Steps: One refactoring per commit"
    - "Dependency Check: Verify with NetArchTest"
    - "No Feature Creep: Focus solely on restructuring"
  pattern_catalog:
    extract_value_object:
      symptom: "Primitive Obsession (e.g., unvalidated string email)"
      steps:
        - "TDD for Value Object (validation/equality)"
        - "Create sealed record"
        - "Change Entity property type"
        - "Adapt EF Core mapping (Value Conversion)"
    extract_repository:
      symptom: "Handler accessing AppDbContext directly"
      steps:
        - "Define interface in Application/Contracts"
        - "Implement in Infrastructure"
        - "Register DI"
        - "Switch Handler to Repository-Mock in tests"
    extract_feature_cqrs:
      symptom: "Logic in Controller or Fat Handler"
      steps:
        - "Create Command/Query, Validator, and Handler"
        - "Switch Controller to IMediator.Send()"
    split_fat_handler:
      symptom: "Handler > 50 lines or multiple concerns"
      steps:
        - "Shift Logic: Domain -> Entity, Infra -> Service/Repo"
        - "Move Validation -> FluentValidation"
        - "Move Side-effects -> Domain Events"
  checklist:
    pre: ["Green tests", "Fixed scope", "Identified layers"]
    post: ["Green tests", "New coverage", "Build pass", "NetArchTest ok", "XML-Docs", "Atomic commit"]
  version: "1.0"
