using Bogus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TicketsPlease.Domain.Entities;

namespace TicketsPlease.Infrastructure.Persistence;

/// <summary>
/// Bietet Methoden zur Initialisierung und zum Seeding der Datenbank mit synthetischen Testdaten.
/// </summary>
public static class DbInitialiser
{
    /// <summary>
    /// Führt das Seeding der Datenbank asynchron aus, falls diese leer ist.
    /// </summary>
    /// <param name="serviceProvider">Der Service-Provider zum Abrufen des Datenbankkontexts.</param>
    /// <returns>Ein Task, der den asynchronen Vorgang repräsentiert.</returns>
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<AppDbContext>>();

        try
        {
            logger.LogInformation("Starte Datenbank-Seeding...");

            // Sicherstellen, dass die Datenbank existiert
            await context.Database.EnsureCreatedAsync();

            if (await context.Tickets.AnyAsync())
            {
                logger.LogInformation("Datenbank enthält bereits Daten. Seeding wird übersprungen.");
                return;
            }

            // Locale auf 'de' (Deutsch) setzen für realistische Testdaten im deutschen Markt
            var faker = new Faker("de");

            // 1. Kategorien erstellen (Beispielhaft)
            var categories = new[] { "Hardware", "Software", "Netzwerk", "Sonstiges" };

            // 2. Tickets generieren
            var ticketFaker = new Faker<Ticket>("de")
                .RuleFor(t => t.Title, f => f.Commerce.ProductName())
                .RuleFor(t => t.Description, f => f.Lorem.Paragraphs(2))
                .RuleFor(t => t.Status, f => f.PickRandom(new[] { "Todo", "Doing", "Done" }))
                .RuleFor(t => t.Priority, f => f.Random.Int(0, 5));

            var tickets = ticketFaker.Generate(100);

            await context.Tickets.AddRangeAsync(tickets);
            await context.SaveChangesAsync();

            logger.LogInformation("{Count} Tickets erfolgreich generiert und gespeichert.", tickets.Count);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Fehler beim Datenbank-Seeding.");
            throw;
        }
    }
}
