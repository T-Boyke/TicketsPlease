namespace TicketsPlease.UnitTests.Infrastructure;

using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using TicketsPlease.Infrastructure.Persistence;

public abstract class InfrastructureTestBase : IDisposable
{
    private readonly SqliteConnection _connection;
    protected readonly AppDbContext Context;

    protected InfrastructureTestBase()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();

        // Foreign Keys deaktivieren für Infrastruktur-Unit-Tests, um Setup-Komplexität zu reduzieren
        using (var command = _connection.CreateCommand())
        {
            command.CommandText = "PRAGMA foreign_keys = OFF;";
            command.ExecuteNonQuery();
        }

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(_connection)
            .Options;

        Context = new AppDbContext(options);
        Context.Database.EnsureCreated();
    }

    public void Dispose()
    {
        Context.Dispose();
        _connection.Close();
        _connection.Dispose();
    }
}
