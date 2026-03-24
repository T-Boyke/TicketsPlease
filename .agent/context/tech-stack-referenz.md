tech_stack_reference:
  core_runtime:
    version: ".NET 10 / ASP.NET Core 10.3 / C# 14"
    architecture: "Clean Architecture (Onion) + DDD + CQRS"
    database_abstraction: "MS SQL Server (EF Core Code-First)"
    async_policy: "CancellationToken REQUIRED everywhere"
  test_suite:
    runner: "xUnit (Facts, Theories)"
    assertions: "FluentAssertions + VerifyTests (Snapshots)"
    mocking: ["Moq", "NSubstitute", "FakeTimeProvider"]
    data_generation: "Bogus (Fakers)"
    mutation_testing: "Stryker.NET (100% Mutation Score goal)"
    containers: "Testcontainers.MsSql (Integration)"
    architecture_tests: "NetArchTest.eXtend"
    e2e: "Playwright"
  nuget_layers:
    domain: "MediatR.Contracts (Zero-Dependency Policy)"
    application: ["MediatR", "FluentValidation", "Mapster"]
    infrastructure: ["Microsoft.EntityFrameworkCore.SqlServer", "Serilog", "MailKit"]
    web: ["Tailwind.Hosting", "Markdig", "DOMPurify", "Scalar"]
  frontend_assets:
    css_framework: "TailwindCSS 4.2.2 (Zero-Node integration via MSBuild @theme)"
    iconography: "FontAwesome 7.2.0 (Local Free/Pro)"
    typography: ["Roboto Flex & Noto Sans (Sans)", "Google Sans Flex (Display)"]
    asset_management: "LibMan in wwwroot/lib (Zero CDN Policy)"
    branding: "ICorporateSkinProvider + OKLCH Color Space"
