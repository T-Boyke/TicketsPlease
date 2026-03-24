domain_entity_workflow:
  objective: "Standard procedure for creating DDD Domain Entities, Value Objects, and Domain Events"
  fundamental_rules:
    zero_dependencies: "Domain layer has no NuGet references (except MediatR.Contracts)"
    rich_model: "Entities contain logic, not just data"
    encapsulation: "All properties use { get; private set; }"
    factory_pattern: "Enforce required fields via static Create(...) methods; private parameterless ctor for EF"
    immutability: "Expose collections as IReadOnlyList<T>; keep internal List<T> private"
  steps:
    entity_creation:
      path: "src/TicketsPlease.Domain/Entities/[EntityName].cs"
      mandatory_fields: ["Guid Id (via BaseEntity)", "byte[] RowVersion ([Timestamp])", "DateTimeOffset CreatedAt"]
      structure: ["Private parameterless ctor (EF)", "Public static Create() (Factory)", "Behavioral methods for state changes"]
    value_objects:
      path: "src/TicketsPlease.Domain/ValueObjects/[ValueObjectName].cs"
      purpose: "Encapsulate complex types; enforce validity on creation (e.g., EmailAddress, PriorityLevel)"
    domain_events:
      path: "src/TicketsPlease.Domain/Events/[EventName].cs"
      purpose: "Decouple side-effects via MediatR INotification; triggered within entity methods"
    bounded_context_mapping:
      Identity: ["User", "UserProfile", "UserAddress", "Role"]
      Ticket: ["Ticket", "SubTicket", "Tag", "Priority", "TimeLog", "Upvote"]
      Workflow: ["WorkflowState", "SlaPolicy"]
      Communication: ["Message", "MessageReadReceipt", "Notification"]
      Asset: ["FileAsset"]
    business_logic:
      principle: "Methods over property setters (e.g., Close(User actor) with permission check)"
    concurrency_handling:
      rule: "RowVersion with [Timestamp] is mandatory"
    documentation: "Mandatory XML-Docs on all public members"
    testing_tdd: "Write tests before logic; target 100% Domain coverage"
