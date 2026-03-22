# 🛠️ TicketsPlease - Tech-Stack Reference

<tech_stack>
  <core>
    - Runtime: .NET 10 / ASP.NET Core 10.3 / C# 14.
    - Architecture: Clean Architecture (Onion) + DDD + CQRS.
    - Database: MS SQL Server (EF Core Code-First).
    - Async: `CancellationToken` REQUIRED everywhere.
  </core>
  <test_suite>
    - Runner: `xUnit` (Facts/Theories).
    - Assertions: `FluentAssertions` + `VerifyTests` (Snapshots).
    - Mocks: `Moq` / `NSubstitute` / `FakeTimeProvider`.
    - Data: `Bogus` (Fakers).
    - Mutation: `Stryker.NET` (100% Mutation Score goal).
    - Containers: `Testcontainers.MsSql` (Integration).
    - Architecture: `NetArchTest.eXtend`.
    - E2E: `Playwright`.
  </test_suite>
  <nuget_layers>
    - Domain: `MediatR.Contracts` (Zero-Dependency Policy).
    - Application: `MediatR`, `FluentValidation`, `Mapster`.
    - Infrastructure: `Microsoft.EntityFrameworkCore.SqlServer`, `Serilog`, `MailKit`.
    - Web: `Tailwind.Hosting`, `Markdig`, `DOMPurify`, `Scalar`.
  </nuget_layers>
  <frontend_assets>
    - CSS: TailwindCSS 4.2 (Zero-Node integration via MSBuild `@theme`).
    - Icons: FontAwesome 7.2 Pro (Local).
    - LibMan: Client libraries in `wwwroot/lib` (ZERO CDN Policy).
    - Theming: `ICorporateSkinProvider`.
  </frontend_assets>
</tech_stack>
