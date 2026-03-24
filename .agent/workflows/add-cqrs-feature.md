add_cqrs_feature_workflow:
  objective: "Standard procedure for implementing a new Command or Query in Clean Architecture"
  pipeline_context:
    flow: ["Request", "LoggingBehavior", "ValidationBehavior", "TransactionBehavior", "Handler", "Response"]
  steps:
    domain_check:
      action: "Check Entity in src/TicketsPlease.Domain/Entities/"
      rules: ["BaseEntity inheritance", "RowVersion included", "Private setters", "Logic in methods"]
    dto_contract:
      path: "src/TicketsPlease.Application/Features/[FeatureName]/"
      naming: "[Entity][Purpose]Dto (e.g., TicketDetailDto)"
    request_creation:
      command: "Commands/[Verb][Entity]Command.cs (IRequest<T>)"
      query: "Queries/Get[Entity]Query.cs (IRequest<T>)"
      rule: "Primitive types/IDs only in Request properties"
    validator_creation:
      rule: "Mandatory for Commands via AbstractValidator<T> (FluentValidation)"
      path: "Same directory as Command"
      tasks: ["NotEmpty/MaxLength checks", "Business constraints", "Automated via Pipeline"]
    handler_implementation:
      rule: "Use IRequestHandler<TRequest, TResponse>"
      naming: "[CommandName]Handler"
      mandatory_rules:
        - "Inject Repository interfaces; never DbContext"
        - "Pass CancellationToken everywhere"
        - "Catch DbUpdateConcurrencyException in writes"
        - "Use Application Exceptions (NotFound, Validation)"
    repository_extension:
      interface: "src/TicketsPlease.Application/Contracts/Persistence/I[Entity]Repository.cs"
      impl: "src/TicketsPlease.Infrastructure/Persistence/Repositories/[Entity]Repository.cs"
      query_rules: ["Always AsNoTracking()", "Prefer .Select() over .Include()"]
    mapping:
      tool: "Mapster (explicit configurations preferred)"
    web_integration:
      rule: "IMediator.Send() only; zero logic in Controllers"
      security: "[ValidateAntiForgeryToken] for POST"
    documentation: "Mandatory XML-Docs for all public members"
    testing_tdd:
      naming: "[HandlerName]Tests -> Handle_[Scenario]_[ExpectedResult]"
      scenarios: ["Happy Path", "Validation", "NotFound", "Concurrency"]
