clean_architecture_scaffold_skill:
  name: "clean-architecture-scaffold"
  description: "Scaffolds a complete feature across all Clean Architecture layers (Domain, Application, Infrastructure, Web)"
  scenarios: ["New feature/use case from scratch", "New API endpoint", "Direct 'create me a feature' requests"]
  decision_tree:
    Read_Only: "Query (GetXxxQuery + Handler + Dto)"
    Data_Mutation: "Command (VerbXxxCommand + Validator + Handler)"
    Mixed: "Command + Query in separate files"
  layer_scaffold_sequence:
    Layer_1_Domain:
      path: "src/TicketsPlease.Domain/"
      components: ["Entity.cs", "ValueObject.cs", "Event.cs", "Enum.cs"]
      checklist:
        - "Inherit BaseEntity (Guid Id, byte[] RowVersion)"
        - "Private setters for all properties"
        - "Static Create(...) factory method"
        - "Private parameterless ctor for EF"
        - "IReadOnlyList<T> for external collections"
        - "XML-Docs for all public members"
    Layer_2_Application:
      path: "src/TicketsPlease.Application/"
      feature_folder: "Features/[FeatureName]/"
      components: ["Command.cs", "CommandValidator.cs", "CommandHandler.cs", "Query.cs", "QueryHandler.cs", "Dto.cs", "IEntityRepository.cs"]
      checklist:
        - "Commands use primitive types/IDs only"
        - "Validators enforce NotEmpty, MaxLength, IsInEnum"
        - "Inject Repository interfaces; never DbContext"
        - "Propagate CancellationToken to the end"
        - "Catch DbUpdateConcurrencyException in writes"
        - "Use Application Exceptions (NotFound, Validation)"
        - "XML-Docs for all public members"
    Layer_3_Infrastructure:
      path: "src/TicketsPlease.Infrastructure/"
      components: ["Repository.cs", "Configuration.cs (IEntityTypeConfiguration)"]
      checklist:
        - "Implement Application contracts"
        - "Use AsNoTracking() and .Select() for queries"
        - "Configure RowVersion as IsRowVersion()"
        - "Register in InfrastructureServiceRegistration"
    Layer_4_Web:
      path: "src/TicketsPlease.Web/"
      components: ["Controller.cs", "Views/Index.cshtml"]
      checklist:
        - "Zero business logic; MediatR.Send() only"
        - "[ValidateAntiForgeryToken] on POST"
        - "[Authorize] where necessary"
        - "a11y: Semantic HTML, aria-labels, keyboard-nav"
    Layer_5_Tests:
      path: "tests/"
      components: ["Application.Tests/CommandHandlerTests.cs", "Domain.Tests/EntityTests.cs"]
      checklist:
        - "TDD: Tests first"
        - "Arrange-Act-Assert blocks"
        - "Cover Happy-Path + Errors (Validation, NotFound, Concurrency)"
        - "FluentAssertions usage"
  version: "1.0"
