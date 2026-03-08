using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TicketsPlease.Infrastructure.Persistence;

namespace TicketsPlease.IntegrationTests;

/// <summary>
/// Basisklasse für alle Integrations-Tests.
/// Konfiguriert eine Test-Infrastruktur mit SQLite In-Memory Datenbank.
/// </summary>
public abstract class IntegrationTestBase : IDisposable
{
    private readonly SqliteConnection _connection;
    protected readonly WebApplicationFactory<Program> Factory;

    /// <summary>
    /// Initialisiert eine neue Instanz von <see cref="IntegrationTestBase"/>.
    /// Erstellt die offene SQLite-Verbindung und die WebApplicationFactory.
    /// </summary>
    protected IntegrationTestBase()
    {
        // SQLite in-memory benötigt eine offene Verbindung über die gesamte Testdauer
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();

        Factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                // Vorhandenen DbContext entfernen
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
                if (descriptor != null) services.Remove(descriptor);

                // SQLite für Tests hinzufügen
                services.AddDbContext<AppDbContext>(options =>
                {
                    options.UseSqlite(_connection);
                });
            });
        });

        // Datenbank-Schema initialisieren
        using var scope = Factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        db.Database.EnsureCreated();
    }

    /// <summary>
    /// Gibt die Ressourcen (Verbindung und Factory) frei.
    /// </summary>
    public void Dispose()
    {
        _connection.Close();
        _connection.Dispose();
        Factory.Dispose();
    }
}
