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
            logger.LogInformation("Starte vollständiges Datenbank-Seeding...");

            await context.Database.EnsureCreatedAsync();

            if (await context.Users.AnyAsync())
            {
                logger.LogInformation("Datenbank enthält bereits Daten. Seeding wird übersprungen.");
                return;
            }

            var faker = new Faker("de");

            // 1. Organizations
            var orgs = new Faker<Organization>("de")
                .RuleFor(o => o.Name, f => f.Company.CompanyName())
                .RuleFor(o => o.SubscriptionLevel, f => f.PickRandom("Trial", "Basic", "Enterprise"))
                .Generate(3);
            await context.Organizations.AddRangeAsync(orgs);
            await context.SaveChangesAsync();

            // 2. Roles
            var roles = new List<Role>
            {
                new() { Name = "Admin", Description = "Full system access" },
                new() { Name = "Owner", Description = "Organization owner" },
                new() { Name = "Teamlead", Description = "Team lead permissions" },
                new() { Name = "User", Description = "Standard user access" }
            };
            await context.Roles.AddRangeAsync(roles);
            await context.SaveChangesAsync();

            // 3. Users & Profiles & Addresses
            var userFaker = new Faker<User>("de")
                .RuleFor(u => u.DisplayName, f => f.Person.FullName)
                .RuleFor(u => u.Username, f => f.Internet.UserName())
                .RuleFor(u => u.Email, f => f.Internet.Email())
                .RuleFor(u => u.PasswordHash, f => Guid.NewGuid().ToString())
                .RuleFor(u => u.RoleId, f => f.PickRandom(roles).Id)
                .RuleFor(u => u.TenantId, f => f.PickRandom(orgs).Id);

            var users = userFaker.Generate(200);
            await context.Users.AddRangeAsync(users);
            await context.SaveChangesAsync();

            foreach (var user in users)
            {
                var profile = new UserProfile
                {
                    UserId = user.Id,
                    FirstName = user.DisplayName.Split(' ')[0],
                    LastName = user.DisplayName.Split(' ').Length > 1 ? user.DisplayName.Split(' ')[1] : "",
                    PhoneNumber = faker.Phone.PhoneNumber(),
                    TenantId = user.TenantId
                };
                await context.UserProfiles.AddAsync(profile);

                var address = new UserAddress
                {
                    UserId = user.Id,
                    Street = faker.Address.StreetAddress(),
                    City = faker.Address.City(),
                    ZipCode = faker.Address.ZipCode(),
                    Country = "Deutschland",
                    TenantId = user.TenantId
                };
                await context.UserAddresses.AddAsync(address);
            }
            await context.SaveChangesAsync();

            // 4. Priorities & Workflow States
            var priorities = new List<TicketPriority>
            {
                new() { Name = "Low", LevelWeight = 1, ColorHex = "#808080" },
                new() { Name = "Medium", LevelWeight = 2, ColorHex = "#0000FF" },
                new() { Name = "High", LevelWeight = 3, ColorHex = "#FFA500" },
                new() { Name = "Blocker", LevelWeight = 4, ColorHex = "#FF0000" }
            };
            await context.TicketPriorities.AddRangeAsync(priorities);

            var workflowStates = new List<WorkflowState>
            {
                new() { Name = "Todo", OrderIndex = 0, ColorHex = "#D3D3D3" },
                new() { Name = "In Progress", OrderIndex = 1, ColorHex = "#ADD8E6" },
                new() { Name = "In Review", OrderIndex = 2, ColorHex = "#FFFFE0" },
                new() { Name = "Done", OrderIndex = 3, ColorHex = "#90EE90", IsTerminalState = true }
            };
            await context.WorkflowStates.AddRangeAsync(workflowStates);
            await context.SaveChangesAsync();

            // 5. Teams & TeamMembers
            var teamFaker = new Faker<Team>("de")
                .RuleFor(t => t.Name, f => f.Commerce.Department())
                .RuleFor(t => t.Description, f => f.Lorem.Sentence())
                .RuleFor(t => t.ColorCode, f => f.Internet.Color())
                .RuleFor(t => t.CreatedByUserId, f => f.PickRandom(users).Id)
                .RuleFor(t => t.TenantId, f => f.PickRandom(orgs).Id);

            var teams = teamFaker.Generate(15);
            await context.Teams.AddRangeAsync(teams);
            await context.SaveChangesAsync();

            foreach (var team in teams)
            {
                var membersCount = faker.Random.Int(2, 5);
                var teamUsers = faker.PickRandom(users, membersCount).ToList();
                foreach (var user in teamUsers)
                {
                    await context.TeamMembers.AddAsync(new TeamMember
                    {
                        TeamId = team.Id,
                        UserId = user.Id,
                        IsTeamLead = user.Id == team.CreatedByUserId,
                        TenantId = team.TenantId
                    });
                }
            }
            await context.SaveChangesAsync();

            // 6. Tickets & Related
            var ticketFaker = new Faker<Ticket>("de")
                .RuleFor(t => t.Title, f => f.Commerce.ProductName())
                .RuleFor(t => t.Sha1Hash, f => f.Random.Hash())
                .RuleFor(t => t.Description, f => f.Lorem.Paragraphs(1))
                .RuleFor(t => t.DescriptionMarkdown, f => $"# {f.Commerce.ProductName()}\n\n{f.Lorem.Paragraphs(2)}")
                .RuleFor(t => t.PriorityId, f => f.PickRandom(priorities).Id)
                .RuleFor(t => t.WorkflowStateId, f => f.PickRandom(workflowStates).Id)
                .RuleFor(t => t.CreatorId, f => f.PickRandom(users).Id)
                .RuleFor(t => t.AssignedUserId, f => f.Random.Bool() ? f.PickRandom(users).Id : null)
                .RuleFor(t => t.ChilliesDifficulty, f => f.Random.Int(1, 5))
                .RuleFor(t => t.CreatedAt, f => f.Date.Past())
                .RuleFor(t => t.TenantId, (f, t) => context.Users.Find(t.CreatorId)?.TenantId ?? f.PickRandom(orgs).Id);

            var tickets = ticketFaker.Generate(500);
            await context.Tickets.AddRangeAsync(tickets);
            await context.SaveChangesAsync();

            // 7. Messages & Logs
            var messageFaker = new Faker<Message>("de")
                .RuleFor(m => m.SenderUserId, f => f.PickRandom(users).Id)
                .RuleFor(m => m.BodyMarkdown, f => f.Lorem.Sentence())
                .RuleFor(m => m.TicketId, f => f.PickRandom(tickets).Id)
                .RuleFor(m => m.SentAt, f => f.Date.Recent())
                .RuleFor(m => m.TenantId, (f, m) => context.Tickets.Find(m.TicketId)?.TenantId ?? f.PickRandom(orgs).Id);

            var messages = messageFaker.Generate(300);
            await context.Messages.AddRangeAsync(messages);
            await context.SaveChangesAsync();

            // 8. SubTickets
            var subTicketFaker = new Faker<SubTicket>("de")
                .RuleFor(st => st.Title, f => f.Lorem.Sentence(3))
                .RuleFor(st => st.IsCompleted, f => f.Random.Bool())
                .RuleFor(st => st.CreatedAt, f => f.Date.Past())
                .RuleFor(st => st.CreatorId, f => f.PickRandom(users).Id)
                .RuleFor(st => st.ParentTicketId, f => f.PickRandom(tickets).Id)
                .RuleFor(st => st.TenantId, (f, st) => context.Tickets.Find(st.ParentTicketId)?.TenantId ?? f.PickRandom(orgs).Id);

            var subTickets = subTicketFaker.Generate(100);
            await context.SubTickets.AddRangeAsync(subTickets);
            await context.SaveChangesAsync();

            logger.LogInformation("Datenbank-Seeding erfolgreich abgeschlossen.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Fehler beim Datenbank-Seeding.");
            throw;
        }
    }
}
