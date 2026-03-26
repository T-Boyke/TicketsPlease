// <copyright file="IntegrationTestBase.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.IntegrationTests;

using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using TicketsPlease.Domain.Entities;
using TicketsPlease.Infrastructure.Persistence;

/// <summary>
/// Basisklasse für alle Integrations-Tests.
/// Konfiguriert eine Test-Infrastruktur mit SQLite In-Memory Datenbank.
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1515:Consider making public types internal", Justification = "Required as base class for IntegrationTests")]
public abstract class IntegrationTestBase : IDisposable
{
  private readonly SqliteConnection connection;

  /// <summary>
  /// Initializes a new instance of the <see cref="IntegrationTestBase"/> class.
  /// Initialisiert eine neue Instanz von <see cref="IntegrationTestBase"/>.
  /// Erstellt die offene SQLite-Verbindung und die WebApplicationFactory.
  /// </summary>
  [System.Diagnostics.CodeAnalysis.SuppressMessage("Reliability", "CA2000:Objekte verwerfen, bevor der Gültigkeitsbereich verloren geht", Justification = "Factory is disposed in the Dispose method of this class.")]
  protected IntegrationTestBase()
  {
    // SQLite in-memory benötigt eine offene Verbindung über die gesamte Testdauer
    this.connection = new SqliteConnection("DataSource=:memory:");
    this.connection.Open();

    // SQLite Foreign Keys explizit aktivieren
    using (var command = this.connection.CreateCommand())
    {
      command.CommandText = "PRAGMA foreign_keys = ON;";
      command.ExecuteNonQuery();
    }

    this.Factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
    {
      builder.UseEnvironment("Testing");
      builder.ConfigureServices(services =>
          {
            // Absolut alle Entity Framework bezogenen Services entfernen, um Provider-Konflikte zu vermeiden
            var efDescriptors = services.Where(d => d.ServiceType.FullName?.Contains("EntityFrameworkCore", StringComparison.Ordinal) == true).ToList();
            foreach (var d in efDescriptors)
            {
              services.Remove(d);
            }

            // SQLite für Tests hinzufügen
            services.AddDbContext<AppDbContext>(options =>
                {
                  options.UseSqlite(this.connection);
                });

            // Authentifizierung für Tests überschreiben
            services.AddAuthentication(TestAuthHandler.AuthenticationScheme)
                    .AddScheme<Microsoft.AspNetCore.Authentication.AuthenticationSchemeOptions, TestAuthHandler>(
                        TestAuthHandler.AuthenticationScheme, options => { });

            // Antiforgery deaktivieren für Tests
            services.AddSingleton<Microsoft.AspNetCore.Antiforgery.IAntiforgery, FakeAntiforgery>();
          });
    });

    // Datenbank-Schema initialisieren
    using var scope = this.Factory.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
  }

  /// <summary>
  /// Gets die WebApplicationFactory für das SUT (System Under Test).
  /// </summary>
  protected WebApplicationFactory<Program> Factory { get; }

  /// <summary>
  /// Gibt die Ressourcen (Verbindung und Factory) frei.
  /// </summary>
  public void Dispose()
  {
    this.Dispose(true);
    GC.SuppressFinalize(this);
  }

  /// <summary>
  /// Seeds minimal required data for tests (Roles, Priorities, WorkflowStates).
  /// </summary>
  /// <param name="db">The database context.</param>
  /// <returns>A task representing the asynchronous operation.</returns>
  protected static async Task SeedMinimalAsync(AppDbContext db)
  {
    ArgumentNullException.ThrowIfNull(db);

    if (!await db.Projects.AnyAsync().ConfigureAwait(false))
    {
      var tenantId = Guid.NewGuid();
      await db.Organizations.AddAsync(new Organization { Id = tenantId, Name = "Test Org", TenantId = tenantId }).ConfigureAwait(false);

      var workflow = new Workflow { Id = Guid.NewGuid(), Name = "Standard Workflow", TenantId = tenantId };
      await db.Workflows.AddAsync(workflow).ConfigureAwait(false);

      var project = new Project("Test Projekt", DateTime.UtcNow);
      project.AssignWorkflow(workflow.Id);
      project.SetTenantId(tenantId);
      await db.Projects.AddAsync(project).ConfigureAwait(false);

      var role = new Role { Id = Guid.Parse("00000000-0000-0000-0000-000000000001"), Name = "Admin" };
      await db.Roles.AddAsync(role).ConfigureAwait(false);

      await db.TicketPriorities.AddAsync(new TicketPriority { Id = Guid.Parse("00000000-0000-0000-0000-000000000002"), Name = "Medium", TenantId = tenantId }).ConfigureAwait(false);
      await db.WorkflowStates.AddAsync(new WorkflowState { Id = Guid.Parse("00000000-0000-0000-0000-000000000003"), Name = "Todo", WorkflowId = workflow.Id, TenantId = tenantId }).ConfigureAwait(false);

      await db.SaveChangesAsync().ConfigureAwait(false);
    }
  }

  /// <summary>
  /// Gibt verwaltete und unverwaltete Ressourcen frei.
  /// </summary>
  /// <param name="disposing">Gibt an, ob verwaltete Ressourcen freigegeben werden sollen.</param>
  protected virtual void Dispose(bool disposing)
  {
    if (disposing)
    {
      this.connection.Close();
      this.connection.Dispose();
      this.Factory?.Dispose();
    }
  }

  /// <summary>
  /// Fake Antiforgery implementation for tests.
  /// </summary>
  [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1812:Avoid uninstantiated internal classes", Justification = "Instantiated by DI in IntegrationTestBase")]
  private sealed class FakeAntiforgery : Microsoft.AspNetCore.Antiforgery.IAntiforgery
  {
    public Microsoft.AspNetCore.Antiforgery.AntiforgeryTokenSet GetAndStoreTokens(Microsoft.AspNetCore.Http.HttpContext httpContext) => new("test", "test", "test", "test");

    public Microsoft.AspNetCore.Antiforgery.AntiforgeryTokenSet GetTokens(Microsoft.AspNetCore.Http.HttpContext httpContext) => new("test", "test", "test", "test");

    public Task<bool> IsRequestValidAsync(Microsoft.AspNetCore.Http.HttpContext httpContext) => Task.FromResult(true);

    public void SetCookieTokenAndHeader(Microsoft.AspNetCore.Http.HttpContext httpContext)
    {
    }

    public Task ValidateRequestAsync(Microsoft.AspNetCore.Http.HttpContext httpContext) => Task.CompletedTask;
  }
}
