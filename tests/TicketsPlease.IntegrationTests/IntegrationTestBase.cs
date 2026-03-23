// <copyright file="IntegrationTestBase.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TicketsPlease.IntegrationTests;

using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using TicketsPlease.Infrastructure.Persistence;

/// <summary>
/// Basisklasse für alle Integrations-Tests.
/// Konfiguriert eine Test-Infrastruktur mit SQLite In-Memory Datenbank.
/// </summary>
internal abstract class IntegrationTestBase : IDisposable
{
  private readonly SqliteConnection connection;
  protected readonly WebApplicationFactory<Program> Factory;

  /// <summary>
  /// Initializes a new instance of the <see cref="IntegrationTestBase"/> class.
  /// Initialisiert eine neue Instanz von <see cref="IntegrationTestBase"/>.
  /// Erstellt die offene SQLite-Verbindung und die WebApplicationFactory.
  /// </summary>
  protected IntegrationTestBase()
  {
    // SQLite in-memory benötigt eine offene Verbindung über die gesamte Testdauer
    this.connection = new SqliteConnection("DataSource=:memory:");
    this.connection.Open();

    this.Factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
    {
      builder.ConfigureServices(services =>
          {
            // Vorhandenen DbContext entfernen
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
            if (descriptor != null)
            {
              services.Remove(descriptor);
            }

            // SQLite für Tests hinzufügen
            services.AddDbContext<AppDbContext>(options =>
                {
                  options.UseSqlite(this.connection);
                });
          });
    });

    // Datenbank-Schema initialisieren
    using var scope = this.Factory.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
  }

  /// <summary>
  /// Gibt die Ressourcen (Verbindung und Factory) frei.
  /// </summary>
  public void Dispose()
  {
    this.connection.Close();
    this.connection.Dispose();
    this.Factory.Dispose();
  }
}
