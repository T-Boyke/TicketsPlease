// <copyright file="DbInitialiser.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Infrastructure.Persistence;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TicketsPlease.Domain.Entities;
using TicketsPlease.Domain.Enums;

/// <summary>
/// Bietet Methoden zur Initialisierung und zum Seeding der Datenbank mit synthetischen Testdaten.
/// </summary>
#pragma warning disable CA1848 // Use the LoggerMessage delegates
public static class DbInitialiser
{
  // Statische IDs (Übereinstimmend mit AppDbContext.SeedStaticData)
  private static readonly Guid AdminRoleId = new("32d733e1-4c7a-4c2d-9b51-1e9a7e6b7d21");
  private static readonly Guid TeamleadRoleId = new("b8f2e9d2-6c8a-4d3e-ac62-2f0b8f7c8e33");
  private static readonly Guid UserRoleId = new("c903f0e3-7d9b-4e4f-bd73-3f1c908d9f44");

  private static readonly Guid LowPriorityId = new("d01401f4-8e0c-4f50-ce84-402d019e0055");
  private static readonly Guid MediumPriorityId = new("e12512a5-9f1d-5061-df95-513e12af1166");
  private static readonly Guid HighPriorityId = new("f23623b6-af2e-5172-ef06-624f23b02277");
  private static readonly Guid BlockerPriorityId = new("034734c7-b03f-5283-f017-735034c13388");

  private static readonly Guid WorkflowId = new("145845d8-c140-5394-0128-846145d24499");
  private static readonly Guid TodoStateId = new("256956e9-d251-54a5-1239-957256e35500");
  private static readonly Guid InProgressStateId = new("367a67fa-e362-55b6-234a-a68367f46611");
  private static readonly Guid InReviewStateId = new("478b780b-f473-56c7-345b-b79478057722");
  private static readonly Guid DoneStateId = new("589c891c-0584-57d8-456c-c8a589168833");

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
      logger.LogInformation("Starte synthetisches Datenbank-Seeding...");

      // Hinweis: Roles, Priorities und WorkflowStates werden via AppDbContext.OnModelCreating (HasData) erzeugt.
      await context.Database.EnsureCreatedAsync().ConfigureAwait(false);

      if (await context.Users.AnyAsync().ConfigureAwait(false))
      {
        logger.LogInformation("Datenbank enthält bereits dynamische Testdaten. Seeding wird übersprungen.");
        return;
      }

      var faker = new Faker("de");
      var roleIds = new[] { AdminRoleId, TeamleadRoleId, UserRoleId };
      var priorityIds = new[] { LowPriorityId, MediumPriorityId, HighPriorityId, BlockerPriorityId };
      var stateIds = new[] { TodoStateId, InProgressStateId, InReviewStateId, DoneStateId };

      // 1. Organizations
      var orgs = Enumerable.Range(0, 3).Select(_ => new Organization
      {
        Name = faker.Company.CompanyName(),
        SubscriptionLevel = faker.PickRandom("Trial", "Basic", "Enterprise"),
      }).ToList();

      await context.Organizations.AddRangeAsync(orgs).ConfigureAwait(false);
      await context.SaveChangesAsync().ConfigureAwait(false);

      // 2. Projects (Verwendet den via HasData erstellten Workflow)
      var projects = new List<Project>();
      var project1 = new Project("Abschlussprüfung 2026", DateTime.UtcNow);
      project1.UpdateMetadata("Abschlussprüfung 2026", "Das Hauptprojekt für die IHK.");
      project1.AssignWorkflow(WorkflowId);
      project1.SetTenantId(faker.PickRandom(orgs).Id);
      projects.Add(project1);

      var project2 = new Project("Interne Toolentwicklung", DateTime.UtcNow.AddMonths(-1));
      project2.UpdateMetadata("Interne Toolentwicklung", "Entwicklung von internen Hilfsmitteln.");
      project2.AssignWorkflow(WorkflowId);
      project2.SetTenantId(faker.PickRandom(orgs).Id);
      projects.Add(project2);

      await context.Projects.AddRangeAsync(projects).ConfigureAwait(false);
      await context.SaveChangesAsync().ConfigureAwait(false);

      // 3. Users
      var users = new List<User>();
      for (int i = 0; i < 50; i++)
      {
        var user = new User
        {
          UserName = faker.Internet.UserName(),
          Email = faker.Internet.Email(),
          PasswordHash = Guid.NewGuid().ToString(),
          RoleId = faker.PickRandom(roleIds),
          TenantId = faker.PickRandom(orgs).Id,
        };
        users.Add(user);
      }

      await context.Users.AddRangeAsync(users).ConfigureAwait(false);
      await context.SaveChangesAsync().ConfigureAwait(false);

      // 4. Profiles & Addresses
      foreach (var user in users)
      {
        var profile = new UserProfile
        {
          UserId = user.Id,
          FirstName = faker.Person.FirstName,
          LastName = faker.Person.LastName,
          Bio = faker.Lorem.Sentence(),
          AvatarUrl = new Uri(faker.Internet.Avatar()),
          TenantId = user.TenantId,
        };
        await context.UserProfiles.AddAsync(profile).ConfigureAwait(false);

        var address = new UserAddress
        {
          UserId = user.Id,
          Street = faker.Address.StreetAddress(),
          City = faker.Address.City(),
          ZipCode = faker.Address.ZipCode(),
          Country = "Deutschland",
          TenantId = user.TenantId,
        };
        await context.UserAddresses.AddAsync(address).ConfigureAwait(false);
      }
      await context.SaveChangesAsync().ConfigureAwait(false);

      // 5. Teams & TeamMembers
      var teams = new List<Team>();
      for (int i = 0; i < 10; i++)
      {
        var team = new Team
        {
          Name = faker.Commerce.Department(),
          Description = faker.Lorem.Sentence(),
          ColorCode = faker.Internet.Color(),
          CreatedByUserId = faker.PickRandom(users).Id,
          TenantId = faker.PickRandom(orgs).Id,
        };
        teams.Add(team);
      }

      await context.Teams.AddRangeAsync(teams).ConfigureAwait(false);
      await context.SaveChangesAsync().ConfigureAwait(false);

      foreach (var team in teams)
      {
        var membersCount = faker.Random.Int(2, 5);
        var teamUsers = faker.PickRandom(users, membersCount).ToList();
        var members = teamUsers.Select(user => new TeamMember
        {
          TeamId = team.Id,
          UserId = user.Id,
          IsTeamLead = user.Id == team.CreatedByUserId,
          TenantId = team.TenantId,
        }).ToList();
        await context.TeamMembers.AddRangeAsync(members).ConfigureAwait(false);
      }

      await context.SaveChangesAsync().ConfigureAwait(false);

      // 6. Tickets & Related
      var tickets = new List<Ticket>();
      for (int i = 0; i < 200; i++)
      {
        var projectId = faker.PickRandom(projects).Id;
        var creatorId = faker.PickRandom(users).Id;
        var workflowStateId = faker.PickRandom(stateIds);
        var geoIp = faker.Internet.Ip();

        var ticket = new Ticket(faker.Commerce.ProductName(), faker.PickRandom<TicketType>(), projectId, creatorId, workflowStateId, geoIp);
        ticket.UpdateDescription(faker.Lorem.Paragraphs(1), $"# {faker.Commerce.ProductName()}\n\n{faker.Lorem.Paragraphs(2)}");
        ticket.AssignUser(faker.Random.Bool() ? faker.PickRandom(users).Id : null);
        ticket.SetPriority(faker.PickRandom(priorityIds));
        ticket.SetDifficulty(faker.Random.Int(1, 5));
        ticket.SetTenantId(faker.PickRandom(orgs).Id);

        tickets.Add(ticket);
      }

      await context.Tickets.AddRangeAsync(tickets).ConfigureAwait(false);
      await context.SaveChangesAsync().ConfigureAwait(false);

      // 7. Messages
      var messages = new List<Message>();
      for (int i = 0; i < 150; i++)
      {
        var message = new Message
        {
          SenderUserId = faker.PickRandom(users).Id,
          BodyMarkdown = faker.Lorem.Sentence(),
          TicketId = faker.PickRandom(tickets).Id,
          SentAt = faker.Date.Recent(),
          TenantId = faker.PickRandom(orgs).Id,
        };
        messages.Add(message);
      }

      await context.Messages.AddRangeAsync(messages).ConfigureAwait(false);
      await context.SaveChangesAsync().ConfigureAwait(false);

      // 8. SubTickets
      var subTickets = new List<SubTicket>();
      for (int i = 0; i < 50; i++)
      {
        var subTicket = new SubTicket
        {
          Title = faker.Lorem.Sentence(3),
          IsCompleted = faker.Random.Bool(),
          CreatedAt = faker.Date.Past(),
          CreatorId = faker.PickRandom(users).Id,
          TicketId = faker.PickRandom(tickets).Id,
          TenantId = faker.PickRandom(orgs).Id,
        };
        subTickets.Add(subTicket);
      }

      await context.SubTickets.AddRangeAsync(subTickets).ConfigureAwait(false);
      await context.SaveChangesAsync().ConfigureAwait(false);

      logger.LogInformation("Synthetisches Seeding erfolgreich abgeschlossen.");
    }
    catch (Exception ex)
    {
      logger.LogError(ex, "Kritischer Fehler beim dynamischen Datenbank-Seeding.");
      throw new InvalidOperationException("Fehler beim synthetischen Seeding.", ex);
    }
  }
}
#pragma warning restore CA1848 // Use the LoggerMessage delegates
