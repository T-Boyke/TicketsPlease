using Microsoft.EntityFrameworkCore;
using TicketsPlease.Domain.Entities;

namespace TicketsPlease.Infrastructure.Persistence;

/// <summary>
/// Der zentrale Datenbankkontext der Anwendung.
/// Verwaltet die Persistenz der User- und Ticket-Entitäten.
/// </summary>
public class AppDbContext : DbContext
{
    /// <summary>
    /// Initialisiert eine neue Instanz von <see cref="AppDbContext"/> mit den angegebenen Optionen.
    /// </summary>
    /// <param name="options">Die Optionen für diesen Kontext.</param>
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    /// <summary>
    /// Ruft die Menge der Benutzer ab oder legt diese fest.
    /// </summary>
    public DbSet<User> Users => Set<User>();

    /// <summary>
    /// Ruft die Menge der Tickets ab oder legt diese fest.
    /// </summary>
    public DbSet<Ticket> Tickets => Set<Ticket>();

    /// <summary>
    /// Konfiguriert das Modell und die Datenbank-Mappings.
    /// Hier wird die explizite Konfiguration für Nebenläufigkeit und Tabellennamen vorgenommen.
    /// </summary>
    /// <param name="modelBuilder">Der Builder für die Modellkonfiguration.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Konfiguration für User
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.DisplayName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(255);

            if (Database.IsSqlServer())
            {
                entity.Property(e => e.RowVersion).IsRowVersion(); // Explizite RowVersion für Sicherheit
            }
        });

        // Konfiguration für Ticket
        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);

            if (Database.IsSqlServer())
            {
                entity.Property(e => e.RowVersion).IsRowVersion();
            }

            // Beziehung: Ein Ticket gehört zu einem User
            entity.HasOne(t => t.AssignedUser)
                  .WithMany()
                  .HasForeignKey(t => t.AssignedUserId)
                  .OnDelete(DeleteBehavior.Restrict);
        });
    }

    /// <summary>
    /// Konfiguriert zusätzliche Optionen wie die Resilience / Retry Strategie.
    /// </summary>
    /// <param name="optionsBuilder">Der Builder für die Kontext-Optionen.</param>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Hinweis: Die eigentliche SQL Server Konfiguration erfolgt meist in Program.cs/Startup.cs.
        // Falls hier konfiguriert wird, stellen wir sicher, dass RetryOnFailure aktiviert ist.
        if (!optionsBuilder.IsConfigured)
        {
            // Placeholder für lokale Entwicklung oder Fallback
            // optionsBuilder.UseSqlServer("fallback_connection_string",
            //    sqlOptions => sqlOptions.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null));
        }
    }
}
