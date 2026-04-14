// <copyright file="IntegrationTestBase.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.IntegrationTests;

using System.Security.Claims;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TicketsPlease.Domain.Entities;
using TicketsPlease.Infrastructure.Persistence;

/// <summary>
/// Custom WebApplicationFactory with SQLite in-memory for integration tests.
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1515:Consider making public types internal", Justification = "Required for test infrastructure")]
public sealed class TestWebApplicationFactory : WebApplicationFactory<Program>
{
  private readonly SqliteConnection connection;

  /// <summary>
  /// Initializes a new instance of the <see cref="TestWebApplicationFactory"/> class.
  /// </summary>
  public TestWebApplicationFactory()
  {
    this.connection = new SqliteConnection("DataSource=:memory:");
    this.connection.Open();

    using var command = this.connection.CreateCommand();
    command.CommandText = "PRAGMA foreign_keys = ON;";
    command.ExecuteNonQuery();
  }

  /// <inheritdoc/>
  protected override void ConfigureWebHost(IWebHostBuilder builder)
  {
    ArgumentNullException.ThrowIfNull(builder);

    builder.UseEnvironment("Testing");
    builder.ConfigureServices(services =>
    {
      // 1. Remove background services to prevent crashes during startup
      var hostedServices = services.Where(d => d.ServiceType == typeof(IHostedService)).ToList();
      foreach (var d in hostedServices)
      {
        services.Remove(d);
      }

      // 2. Remove existing EF Core registrations
      var efServices = services.Where(d =>
          d.ServiceType.Namespace?.StartsWith("Microsoft.EntityFrameworkCore", StringComparison.Ordinal) == true ||
          d.ServiceType.FullName?.Contains("EntityFrameworkCore", StringComparison.Ordinal) == true).ToList();
      foreach (var d in efServices)
      {
        services.Remove(d);
      }

      var contextDescriptors = services.Where(d =>
          d.ServiceType == typeof(AppDbContext) ||
          d.ServiceType == typeof(DbContextOptions<AppDbContext>)).ToList();
      foreach (var d in contextDescriptors)
      {
        services.Remove(d);
      }

      // 3. Inject SQLite in-memory
      services.AddDbContext<AppDbContext>(options =>
      {
        options.UseSqlite(this.connection);
        options.EnableServiceProviderCaching(false); 
      });

      // 4. Fake Authentication & Antiforgery
      services.AddAuthentication(TestAuthHandler.AuthenticationScheme)
              .AddScheme<Microsoft.AspNetCore.Authentication.AuthenticationSchemeOptions, TestAuthHandler>(
                  TestAuthHandler.AuthenticationScheme, _ => { });

      services.AddSingleton<Microsoft.AspNetCore.Antiforgery.IAntiforgery, FakeAntiforgery>();
    });
  }

  /// <inheritdoc/>
  protected override void Dispose(bool disposing)
  {
    base.Dispose(disposing);
    if (disposing)
    {
      this.connection.Close();
      this.connection.Dispose();
    }
  }
}

/// <summary>
/// Fake Antiforgery implementation for tests.
/// </summary>
internal sealed class FakeAntiforgery : Microsoft.AspNetCore.Antiforgery.IAntiforgery
{
  public Microsoft.AspNetCore.Antiforgery.AntiforgeryTokenSet GetAndStoreTokens(HttpContext httpContext) => new("test", "test", "test", "test");

  public Microsoft.AspNetCore.Antiforgery.AntiforgeryTokenSet GetTokens(HttpContext httpContext) => new("test", "test", "test", "test");

  public Task<bool> IsRequestValidAsync(HttpContext httpContext) => Task.FromResult(true);

  public void SetCookieTokenAndHeader(HttpContext httpContext) { }

  public Task ValidateRequestAsync(HttpContext httpContext) => Task.CompletedTask;
}

/// <summary>
/// Base class for all integration tests.
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1515:Consider making public types internal", Justification = "Required as base class for IntegrationTests")]
public abstract class IntegrationTestBase : IDisposable
{
  /// <summary>
  /// The fixed Tenant ID used for all basic integration tests.
  /// </summary>
  public static readonly Guid TestTenantId = Guid.Parse("00000000-0000-0000-0000-000000001000");

  /// <summary>
  /// The fixed User ID used for basic integration tests.
  /// </summary>
  public static readonly Guid TestUserId = Guid.Parse("00000000-0000-0000-0000-000000002000");

  /// <summary>
  /// Standard Priority ID seeded in all tests.
  /// </summary>
  public static readonly Guid MediumPriorityId = Guid.Parse("00000000-0000-0000-0000-000000000002");

  /// <summary>
  /// Standard Todo State ID seeded in all tests.
  /// </summary>
  public static readonly Guid TodoStateId = Guid.Parse("00000000-0000-0000-0000-000000000003");

  /// <summary>
  /// Standard Done State ID seeded in all tests.
  /// </summary>
  public static readonly Guid DoneStateId = Guid.Parse("00000000-0000-0000-0000-000000000004");

  private bool disposedValue;

  /// <summary>
  /// Initializes a new instance of the <see cref="IntegrationTestBase"/> class.
  /// </summary>
  protected IntegrationTestBase()
  {
    this.Factory = new TestWebApplicationFactory();

    // Initialize database
    using var scope = this.Factory.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
  }

  /// <summary>
  /// Gets the WebApplicationFactory for the system under test.
  /// </summary>
  protected TestWebApplicationFactory Factory { get; }

  /// <inheritdoc/>
  public void Dispose()
  {
    this.Dispose(disposing: true);
    GC.SuppressFinalize(this);
  }

  /// <summary>
  /// Sets a mock HttpContext with the specified user and tenant information in the given service provider.
  /// This bypasses global query filters and provides identity for services.
  /// </summary>
  /// <param name="services">The service provider (usually from a scope).</param>
  /// <param name="userId">The user ID.</param>
  /// <param name="tenantId">The tenant ID.</param>
  /// <param name="role">The user role.</param>
  protected void SetContext(IServiceProvider services, Guid userId, Guid tenantId, string role = "Admin")
  {
    ArgumentNullException.ThrowIfNull(services);
    var httpContextAccessor = services.GetRequiredService<IHttpContextAccessor>();
    
    var claims = new[]
    {
      new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
      new Claim(ClaimTypes.Role, role),
      new Claim("TenantId", tenantId.ToString())
    };

    var identity = new ClaimsIdentity(claims, "Test");
    var principal = new ClaimsPrincipal(identity);

    httpContextAccessor.HttpContext = new DefaultHttpContext
    {
      User = principal
    };
  }

  /// <summary>
  /// Seeds minimal required data for tests.
  /// </summary>
  /// <param name="db">The database context.</param>
  /// <returns>A task.</returns>
  protected static async Task SeedMinimalAsync(AppDbContext db)
  {
    ArgumentNullException.ThrowIfNull(db);

    if (!await db.Projects.IgnoreQueryFilters().AnyAsync().ConfigureAwait(false))
    {
      await db.Organizations.AddAsync(new Organization { Id = TestTenantId, Name = "Test Org", TenantId = TestTenantId }).ConfigureAwait(false);

      var workflow = new Workflow { Id = Guid.NewGuid(), Name = "Standard Workflow", TenantId = TestTenantId };
      await db.Workflows.AddAsync(workflow).ConfigureAwait(false);

      var project = new Project("Test Projekt", DateTime.UtcNow);
      project.AssignWorkflow(workflow.Id);
      project.SetTenantId(TestTenantId);
      await db.Projects.AddAsync(project).ConfigureAwait(false);

      var role = new Role { Id = Guid.Parse("00000000-0000-0000-0000-000000000001"), Name = "Admin" };
      await db.Roles.AddAsync(role).ConfigureAwait(false);

      var doneStateId = DoneStateId;
      var todoStateId = TodoStateId;

      await db.TicketPriorities.AddAsync(new TicketPriority { Id = MediumPriorityId, Name = "Medium", TenantId = TestTenantId }).ConfigureAwait(false);
      await db.WorkflowStates.AddAsync(new WorkflowState { Id = todoStateId, Name = "Todo", WorkflowId = workflow.Id, TenantId = TestTenantId }).ConfigureAwait(false);
      await db.WorkflowStates.AddAsync(new WorkflowState { Id = doneStateId, Name = "Done", WorkflowId = workflow.Id, TenantId = TestTenantId, IsTerminalState = true }).ConfigureAwait(false);

      await db.WorkflowTransitions.AddAsync(new WorkflowTransition 
      { 
        Id = Guid.NewGuid(), 
        FromStateId = todoStateId, 
        ToStateId = doneStateId, 
        TenantId = TestTenantId 
      }).ConfigureAwait(false);

      await db.Users.AddAsync(new User 
      { 
        Id = TestUserId, 
        UserName = "testadmin", 
        Email = "admin@test.com", 
        TenantId = TestTenantId,
        RoleId = role.Id,
        NormalizedEmail = "ADMIN@TEST.COM",
        NormalizedUserName = "TESTADMIN",
        EmailConfirmed = true,
        SecurityStamp = Guid.NewGuid().ToString(),
        Profile = new UserProfile { UserId = TestUserId, FirstName = "Test", LastName = "Admin", TenantId = TestTenantId }
      }).ConfigureAwait(false);

      await db.SaveChangesAsync().ConfigureAwait(false);
    }
  }

  /// <summary>
  /// Disposes resources.
  /// </summary>
  /// <param name="disposing">True if disposing.</param>
  protected virtual void Dispose(bool disposing)
  {
    if (!this.disposedValue)
    {
      if (disposing)
      {
        this.Factory.Dispose();
      }

      this.disposedValue = true;
    }
  }
}
