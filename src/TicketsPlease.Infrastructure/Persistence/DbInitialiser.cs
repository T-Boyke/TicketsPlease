// <copyright file="DbInitialiser.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
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

      await context.Database.EnsureCreatedAsync().ConfigureAwait(false);

      if (await context.Users.AnyAsync().ConfigureAwait(false))
      {
        logger.LogInformation("Datenbank enthält bereits Daten. Seeding wird übersprungen.");
        return;
      }

      var faker = new Faker("de");

      // 1. Organizations
      var orgs = Enumerable.Range(0, 3).Select(_ => new Organization
      {
        Name = faker.Company.CompanyName(),
        SubscriptionLevel = faker.PickRandom("Trial", "Basic", "Enterprise"),
      }).ToList();

      await context.Organizations.AddRangeAsync(orgs).ConfigureAwait(false);
      await context.SaveChangesAsync().ConfigureAwait(false);

      // 2. Roles
      var roles = new List<Role>
      {
        new() { Name = "Admin", Description = "Full system access" },
        new() { Name = "Owner", Description = "Organization owner" },
        new() { Name = "Teamlead", Description = "Team lead permissions" },
        new() { Name = "User", Description = "Standard user access" },
      };
      await context.Roles.AddRangeAsync(roles).ConfigureAwait(false);
      await context.SaveChangesAsync().ConfigureAwait(false);

      // 3. Projects & Workflows
      var workflow = new Workflow { Name = "Standard IHK Workflow" };
      await context.Workflows.AddAsync(workflow).ConfigureAwait(false);

      var workflowStates = new List<WorkflowState>
      {
        new() { Name = "Todo", OrderIndex = 0, ColorHex = "#D3D3D3", Workflow = workflow },
        new() { Name = "In Progress", OrderIndex = 1, ColorHex = "#ADD8E6", Workflow = workflow },
        new() { Name = "In Review", OrderIndex = 2, ColorHex = "#FFFFE0", Workflow = workflow },
        new() { Name = "Done", OrderIndex = 3, ColorHex = "#90EE90", IsTerminalState = true, Workflow = workflow },
      };
      await context.WorkflowStates.AddRangeAsync(workflowStates).ConfigureAwait(false);
      await context.SaveChangesAsync().ConfigureAwait(false);

      var projects = new List<Project>();

      var project1 = new Project("IHK Abschlussprüfung 2026", DateTime.UtcNow);
      project1.UpdateMetadata("IHK Abschlussprüfung 2026", "Das Hauptprojekt für die IHK.");
      project1.AssignWorkflow(workflow.Id);
      project1.SetTenantId(faker.PickRandom(orgs).Id);
      projects.Add(project1);

      var project2 = new Project("Interne Toolentwicklung", DateTime.UtcNow.AddMonths(-1));
      project2.UpdateMetadata("Interne Toolentwicklung", "Entwicklung von internen Hilfsmitteln.");
      project2.AssignWorkflow(workflow.Id);
      project2.SetTenantId(faker.PickRandom(orgs).Id);
      projects.Add(project2);
      await context.Projects.AddRangeAsync(projects).ConfigureAwait(false);
      await context.SaveChangesAsync().ConfigureAwait(false);

      // 4. Users
      var users = new List<User>();
      for (int i = 0; i < 50; i++)
      {
        var user = new User
        {
          UserName = faker.Internet.UserName(),
          Email = faker.Internet.Email(),
          PasswordHash = Guid.NewGuid().ToString(),
          RoleId = faker.PickRandom(roles).Id,
          TenantId = faker.PickRandom(orgs).Id,
        };
        users.Add(user);
      }

      await context.Users.AddRangeAsync(users).ConfigureAwait(false);
      await context.SaveChangesAsync().ConfigureAwait(false);

      // 5. Profiles & Addresses
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

      // 6. Priorities
      var priorities = new List<TicketPriority>
      {
        new() { Name = "Low", LevelWeight = 1, ColorHex = "#808080" },
        new() { Name = "Medium", LevelWeight = 2, ColorHex = "#0000FF" },
        new() { Name = "High", LevelWeight = 3, ColorHex = "#FFA500" },
        new() { Name = "Blocker", LevelWeight = 4, ColorHex = "#FF0000" },
      };
      await context.TicketPriorities.AddRangeAsync(priorities).ConfigureAwait(false);
      await context.SaveChangesAsync().ConfigureAwait(false);

      // 7. Teams & TeamMembers
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

      // 8. Tickets & Related
      var tickets = new List<Ticket>();
      for (int i = 0; i < 200; i++)
      {
        var projectId = faker.PickRandom(projects).Id;
        var creatorId = faker.PickRandom(users).Id;
        var workflowStateId = faker.PickRandom(workflowStates).Id;
        var geoIp = faker.Internet.Ip();

        var ticket = new Ticket(faker.Commerce.ProductName(), faker.PickRandom<TicketType>(), projectId, creatorId, workflowStateId, geoIp);
        ticket.UpdateDescription(faker.Lorem.Paragraphs(1), $"# {faker.Commerce.ProductName()}\n\n{faker.Lorem.Paragraphs(2)}");
        ticket.AssignUser(faker.Random.Bool() ? faker.PickRandom(users).Id : null);
        ticket.SetPriority(faker.PickRandom(priorities).Id);
        ticket.SetDifficulty(faker.Random.Int(1, 5));
        ticket.SetTenantId(faker.PickRandom(orgs).Id);

        tickets.Add(ticket);
      }

      await context.Tickets.AddRangeAsync(tickets).ConfigureAwait(false);
      await context.SaveChangesAsync().ConfigureAwait(false);

      // 9. Messages
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

      // 10. SubTickets
      var subTickets = new List<SubTicket>();
      for (int i = 0; i < 50; i++)
      {
        var subTicket = new SubTicket
        {
          Title = faker.Lorem.Sentence(3),
          IsCompleted = faker.Random.Bool(),
          CreatedAt = faker.Date.Past(),
          CreatorId = faker.PickRandom(users).Id,
          ParentTicketId = faker.PickRandom(tickets).Id,
          TenantId = faker.PickRandom(orgs).Id,
        };
        subTickets.Add(subTicket);
      }

      await context.SubTickets.AddRangeAsync(subTickets).ConfigureAwait(false);
      await context.SaveChangesAsync().ConfigureAwait(false);

      logger.LogInformation("Datenbank-Seeding erfolgreich abgeschlossen.");
    }
    catch (Exception ex)
    {
      throw new InvalidOperationException("Fehler beim Datenbank-Seeding.", ex);
    }
  }
}
#pragma warning restore CA1848 // Use the LoggerMessage delegates
