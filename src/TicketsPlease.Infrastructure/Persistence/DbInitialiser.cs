// <copyright file="DbInitialiser.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Infrastructure.Persistence;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using Microsoft.AspNetCore.Identity;
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
  private static readonly Guid ProductOwnerRoleId = new("d01401f4-8e0c-4f50-ce84-402d019e0066");
  private static readonly Guid StakeholderRoleId = new("e12512a5-9f1d-5061-df95-513e12af1177");

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
      logger.LogInformation("Starte hochwertiges Datenbank-Seeding...");

      await context.Database.EnsureDeletedAsync().ConfigureAwait(false);
      await context.Database.EnsureCreatedAsync().ConfigureAwait(false);

      if (await context.Users.AnyAsync().ConfigureAwait(false))
      {
        logger.LogInformation("Datenbank enthält bereits dynamische Testdaten. Seeding wird übersprungen.");
        return;
      }

      var faker = new Faker("de");
      var priorityIds = new[] { LowPriorityId, MediumPriorityId, HighPriorityId, BlockerPriorityId };
      var stateIds = new[] { TodoStateId, InProgressStateId, InReviewStateId, DoneStateId };
      var stateNames = new Dictionary<Guid, string>
      {
          { TodoStateId, "Todo" },
          { InProgressStateId, "In Progress" },
          { InReviewStateId, "In Review" },
          { DoneStateId, "Done" }
      };

      // 1. Organizations (Workspaces) - Genau 2
      var orgs = new List<Organization>
      {
          new Organization { Name = "Global Solutions AG", SubscriptionLevel = "Enterprise", IsActive = true, CreatedAt = DateTime.UtcNow.AddMonths(-6) },
          new Organization { Name = "Digital Innovators GmbH", SubscriptionLevel = "Basic", IsActive = true, CreatedAt = DateTime.UtcNow.AddMonths(-3) }
      };

      await context.Organizations.AddRangeAsync(orgs).ConfigureAwait(false);
      await context.SaveChangesAsync().ConfigureAwait(false);

      // 2. Workflows (Transitions)
      var transitions = new List<WorkflowTransition>
      {
          new WorkflowTransition { FromStateId = TodoStateId, ToStateId = InProgressStateId, TenantId = orgs[0].Id },
          new WorkflowTransition { FromStateId = InProgressStateId, ToStateId = InReviewStateId, TenantId = orgs[0].Id },
          new WorkflowTransition { FromStateId = InReviewStateId, ToStateId = DoneStateId, TenantId = orgs[0].Id },
          new WorkflowTransition { FromStateId = InProgressStateId, ToStateId = TodoStateId, TenantId = orgs[0].Id },
          new WorkflowTransition { FromStateId = InReviewStateId, ToStateId = InProgressStateId, TenantId = orgs[0].Id },
          new WorkflowTransition { FromStateId = TodoStateId, ToStateId = DoneStateId, TenantId = orgs[0].Id },
          new WorkflowTransition { FromStateId = InProgressStateId, ToStateId = DoneStateId, TenantId = orgs[0].Id },

          new WorkflowTransition { FromStateId = TodoStateId, ToStateId = InProgressStateId, TenantId = orgs[1].Id },
          new WorkflowTransition { FromStateId = InProgressStateId, ToStateId = DoneStateId, TenantId = orgs[1].Id },
      };
      await context.WorkflowTransitions.AddRangeAsync(transitions).ConfigureAwait(false);

      // 3. Projects - 2 pro Organization
      var projects = new List<Project>();
      foreach (var org in orgs)
      {
        var p1 = new Project($"{org.Name} - Main Development", DateTime.UtcNow.AddMonths(-2));
        p1.UpdateMetadata(p1.Title, $"Zentrales Projekt für {org.Name}");
        p1.AssignWorkflow(WorkflowId);
        p1.SetTenantId(org.Id);
        projects.Add(p1);

        var p2 = new Project($"{org.Name} - Internal Tools", DateTime.UtcNow.AddMonths(-1));
        p2.UpdateMetadata(p2.Title, "Interne Werkzeuge und Skripte.");
        p2.AssignWorkflow(WorkflowId);
        p2.SetTenantId(org.Id);
        projects.Add(p2);
      }
      await context.Projects.AddRangeAsync(projects).ConfigureAwait(false);
      await context.SaveChangesAsync().ConfigureAwait(false);

      // 4. Users (Max 2 Admins, 5 Teamleads, 18 Users = 25 Total)
      var passwordHasher = new PasswordHasher<User>();
      var users = new List<User>();

      // Static Users (Bestehende Logins behalten)
      var adminUser = new User { UserName = "admin", Email = "admin@ticketsplease.com", RoleId = AdminRoleId, TenantId = orgs[0].Id, IsActive = true, CreatedAt = DateTime.UtcNow.AddMonths(-5) };
      var leadUser = new User { UserName = "teamlead", Email = "teamlead@ticketsplease.com", RoleId = TeamleadRoleId, TenantId = orgs[0].Id, IsActive = true, CreatedAt = DateTime.UtcNow.AddMonths(-4) };
      var stdUser = new User { UserName = "user", Email = "user@ticketsplease.com", RoleId = UserRoleId, TenantId = orgs[0].Id, IsActive = true, CreatedAt = DateTime.UtcNow.AddMonths(-3) };
      users.AddRange(new[] { adminUser, leadUser, stdUser });

      // Add 1 more Admin
      users.Add(new User { UserName = "admin2", Email = "admin2@ticketsplease.com", RoleId = AdminRoleId, TenantId = orgs[1].Id, IsActive = true });

      // Add Product Owners for each Org (Seeded Logins)
      var po1 = new User { UserName = "po", Email = "po@ticketsplease.com", RoleId = ProductOwnerRoleId, TenantId = orgs[0].Id, IsActive = true };
      var po2 = new User { UserName = "po2", Email = "po2@ticketsplease.com", RoleId = ProductOwnerRoleId, TenantId = orgs[1].Id, IsActive = true };
      users.AddRange(new[] { po1, po2 });

      // Add Stakeholders for each Org (Seeded Logins)
      var stakeholder1 = new User { UserName = "stakeholder", Email = "stakeholder@ticketsplease.com", RoleId = StakeholderRoleId, TenantId = orgs[0].Id, IsActive = true };
      var stakeholder2 = new User { UserName = "stakeholder2", Email = "stakeholder2@ticketsplease.com", RoleId = StakeholderRoleId, TenantId = orgs[1].Id, IsActive = true };
      users.AddRange(new[] { stakeholder1, stakeholder2 });

      // Add 4 more Teamleads
      for (int i = 2; i <= 5; i++)
      {
        var org = faker.PickRandom(orgs);
        users.Add(new User { UserName = $"teamlead{i}", Email = $"teamlead{i}@ticketsplease.com", RoleId = TeamleadRoleId, TenantId = org.Id, IsActive = true });
      }

      // Add 17 more Users
      for (int i = 2; i <= 18; i++)
      {
        var org = faker.PickRandom(orgs);
        users.Add(new User { UserName = $"user{i}", Email = $"user{i}@ticketsplease.com", RoleId = UserRoleId, TenantId = org.Id, IsActive = true });
      }

      foreach (var u in users)
      {
        u.PasswordHash = passwordHasher.HashPassword(u, "Pass123!");
        u.NormalizedEmail = u.Email?.ToUpperInvariant();
        u.NormalizedUserName = u.UserName?.ToUpperInvariant();
        u.SecurityStamp = Guid.NewGuid().ToString();
      }

      await context.Users.AddRangeAsync(users).ConfigureAwait(false);
      await context.SaveChangesAsync().ConfigureAwait(false);

      // Roles mapping
      foreach (var user in users)
      {
        context.UserRoles.Add(new IdentityUserRole<Guid> { UserId = user.Id, RoleId = user.RoleId });
      }
      await context.SaveChangesAsync().ConfigureAwait(false);

      // 5. Profiles & Addresses (Vollständig)
      var techStacks = new[] { ".NET, C#, SQL Server", "Frontend, React, Tailwind", "DevOps, Azure, Docker", "QA, Selenium, Playwright", "Management, Agile, Scrum" };
      var positions = new[] { "Senior Developer", "Junior Developer", "DevOps Engineer", "Project Manager", "Team Lead", "Quality Engineer" };

      foreach (var user in users)
      {
        var profile = new UserProfile
        {
          UserId = user.Id,
          FirstName = faker.Name.FirstName(),
          LastName = faker.Name.LastName(),
          Bio = faker.Lorem.Paragraph(),
          AvatarUrl = new Uri($"https://api.dicebear.com/7.x/avataaars/svg?seed={user.UserName}"),
          PhoneNumber = faker.Phone.PhoneNumber(),
          Position = faker.PickRandom(positions),
          TechStack = faker.PickRandom(techStacks),
          Street = faker.Address.StreetName(),
          HouseNumber = faker.Address.BuildingNumber(),
          City = faker.Address.City(),
          Country = "Deutschland",
          TenantId = user.TenantId,
          CreatedAt = DateTime.UtcNow.AddMonths(-2)
        };
        await context.UserProfiles.AddAsync(profile).ConfigureAwait(false);

        var address = new UserAddress
        {
          UserId = user.Id,
          Street = profile.Street,
          City = profile.City,
          ZipCode = faker.Address.ZipCode(),
          Country = profile.Country,
          TenantId = user.TenantId,
          CreatedAt = DateTime.UtcNow.AddMonths(-2)
        };
        await context.UserAddresses.AddAsync(address).ConfigureAwait(false);
      }
      await context.SaveChangesAsync().ConfigureAwait(false);

      // 6. Teams (Genau 5)
      var teamNames = new[] { "Core Development", "UI/UX Design", "Platform Engineering", "Strategic Sales", "Customer Success" };
      var teams = new List<Team>();
      for (int i = 0; i < 5; i++)
      {
        var org = i < 3 ? orgs[0] : orgs[1]; // 3 Teams in Org1, 2 Teams in Org2
        var orgUsers = users.Where(u => u.TenantId == org.Id).ToList();
        if (orgUsers.Count == 0) continue;

        var creator = orgUsers.FirstOrDefault(u => u.RoleId != UserRoleId) ?? orgUsers.First();
        var team = new Team
        {
          Name = teamNames[i],
          Description = $"Das Team für {teamNames[i]} bei {org.Name}.",
          ColorCode = faker.Internet.Color(),
          CreatedByUserId = creator.Id,
          TenantId = org.Id,
          CreatedAt = DateTime.UtcNow.AddMonths(-1)
        };
        teams.Add(team);
      }
      if (teams.Count > 0)
      {
          logger.LogInformation("Teams erstellt. Starte Team-Mitglieder-Zuweisung...");
          await context.Teams.AddRangeAsync(teams).ConfigureAwait(false);
          await context.SaveChangesAsync().ConfigureAwait(false);
      }
      else
      {
          logger.LogWarning("Keine Teams erstellt, da keine Benutzer für die Organisationen gefunden wurden.");
      }

      // Team Members
      foreach (var team in teams)
      {
        // Alle Teamleads der Org ins Team (als Leads)
        var leadUsers = users.Where(u => u.TenantId == team.TenantId && u.RoleId == TeamleadRoleId).Take(1).ToList();
        // Einige normale User dazu
        var memberUsers = users.Where(u => u.TenantId == team.TenantId && u.RoleId == UserRoleId).Take(3).ToList();

        if (leadUsers.Count > 0)
        {
            foreach (var lu in leadUsers)
            {
                context.TeamMembers.Add(new TeamMember { TeamId = team.Id, UserId = lu.Id, IsTeamLead = true, TenantId = team.TenantId, JoinedAt = DateTime.UtcNow.AddDays(-20) });
            }
        }

        if (memberUsers.Count > 0)
        {
            foreach (var mu in memberUsers)
            {
                context.TeamMembers.Add(new TeamMember { TeamId = team.Id, UserId = mu.Id, IsTeamLead = false, TenantId = team.TenantId, JoinedAt = DateTime.UtcNow.AddDays(-15) });
            }
        }
      }
      logger.LogInformation("Team-Mitglieder zugewiesen. Starte Ticket-Seeding...");
      await context.SaveChangesAsync().ConfigureAwait(false);

      // 7. Tickets (~50)
      var tickets = new List<Ticket>();
      for (int i = 0; i < 50; i++)
      {
        var project = faker.PickRandom(projects);
        var orgUsers = users.Where(u => u.TenantId == project.TenantId).ToList();
        if (orgUsers.Count == 0) continue;

        var creator = faker.PickRandom(orgUsers);
        var stateId = faker.PickRandom(stateIds);
        var ticket = new Ticket(
            faker.Commerce.ProductName(),
            faker.PickRandom<TicketType>(),
            project.Id,
            creator.Id,
            stateId,
            stateNames[stateId],
            faker.Internet.Ip())
        {
            TenantId = project.TenantId,
            CreatedAt = DateTime.UtcNow.AddDays(-faker.Random.Int(1, 30))
        };

        ticket.UpdateDescription(faker.Lorem.Sentence(), $"# {ticket.Title}\n\n{faker.Lorem.Paragraphs(2)}");
        ticket.SetPriority(faker.PickRandom(priorityIds));
        ticket.SetDifficulty(faker.Random.Int(1, 5));
        ticket.SetSize(faker.PickRandom<TicketSize>());

        // Zuweisung
        if (faker.Random.Bool(0.8f))
        {
           ticket.AssignUser(faker.PickRandom(orgUsers).Id);
        }

        tickets.Add(ticket);
      }
      logger.LogInformation("Tickets erstellt. Starte Ticket-Team-Zuweisung...");
      await context.Tickets.AddRangeAsync(tickets).ConfigureAwait(false);
      await context.SaveChangesAsync().ConfigureAwait(false);

      // Ticket Assignments (Teams)
      foreach (var ticket in tickets.Take(25)) // Halbe Tickets auch Teams zuweisen
      {
        var team = teams.FirstOrDefault(t => t.TenantId == ticket.TenantId);
        if (team != null)
        {
            context.TicketAssignments.Add(new TicketAssignment
            {
                TicketId = ticket.Id,
                TeamId = team.Id,
                TenantId = ticket.TenantId,
                AssignedAt = DateTime.UtcNow
            });
        }
      }
      logger.LogInformation("Ticket-Team-Zuweisungen abgeschlossen. Starte Benachrichtigungen...");
      await context.SaveChangesAsync().ConfigureAwait(false);

      // 8. Notifications
      var notifications = new List<Notification>();
      foreach (var user in users)
      {
        var userTickets = tickets.Where(t => t.TenantId == user.TenantId).ToList();
        for (int i = 0; i < 5; i++)
        {
          var targetTicket = faker.PickRandom(userTickets);
          notifications.Add(new Notification
          {
            UserId = user.Id,
            Title = i % 2 == 0 ? "Neues Ticket zugewiesen" : "Nachricht in Ticket",
            Content = faker.Lorem.Sentence(10),
            IsRead = i > 2,
            CreatedAt = DateTime.UtcNow.AddMinutes(-faker.Random.Int(10, 5000)),
            TenantId = user.TenantId,
            TargetUrl = targetTicket != null ? $"/Tickets/Details/{targetTicket.Id}" : "/Tickets"
          });
        }
      }
      logger.LogInformation("Benachrichtigungen erstellt. Starte Chat-Nachrichten...");
      await context.Notifications.AddRangeAsync(notifications).ConfigureAwait(false);

      // 8.1. Tags
      var tags = new List<Tag>();
      var tagDefinitions = new[]
      {
          ("Bug", "#ef4444", "fa-bug"),
          ("Feature", "#3b82f6", "fa-star"),
          ("Support", "#10b981", "fa-headset"),
          ("UI/UX", "#f59e0b", "fa-palette"),
          ("Backend", "#6366f1", "fa-server"),
          ("Database", "#8b5cf6", "fa-database"),
          ("Security", "#991b1b", "fa-shield-halved"),
          ("Refactor", "#ec4899", "fa-recycle"),
          ("Documentation", "#64748b", "fa-file-lines"),
          ("API", "#0ea5e9", "fa-cloud"),
          ("Mobile", "#f43f5e", "fa-mobile-screen-button"),
          ("Testing", "#22c55e", "fa-vial"),
          ("DevOps", "#06b6d4", "fa-infinity"),
          ("Performance", "#eab308", "fa-bolt"),
          ("Legal", "#4b5563", "fa-scale-balanced"),
          ("Marketing", "#d946ef", "fa-bullhorn"),
          ("Sales", "#f97316", "fa-cart-shopping"),
          ("Internal", "#6d28d9", "fa-briefcase"),
          ("Urgent", "#dc2626", "fa-circle-exclamation"),
          ("Legacy", "#78350f", "fa-clock-rotate-left")
      };

      foreach (var (name, color, icon) in tagDefinitions)
      {
          tags.Add(new Tag { Name = name, ColorHex = color, Icon = icon });
      }
      await context.Tags.AddRangeAsync(tags).ConfigureAwait(false);
      await context.SaveChangesAsync().ConfigureAwait(false);

      // 9. Messages (Chat)
      var messages = new List<Message>();
      foreach (var org in orgs)
      {
          var orgUsers = users.Where(u => u.TenantId == org.Id).ToList();
          if (orgUsers.Count < 2) continue; // Brauchen mindestens 2 User für Chat

          for (int i = 0; i < 40; i++)
          {
              var sender = faker.PickRandom(orgUsers);
              var receiver = faker.PickRandom(orgUsers.Where(u => u.Id != sender.Id).ToList());
              messages.Add(new Message
              {
                  SenderUserId = sender.Id,
                  ReceiverUserId = receiver.Id,
                  BodyMarkdown = faker.Lorem.Sentence(),
                  SentAt = DateTime.UtcNow.AddMinutes(-faker.Random.Int(1, 10000)),
                  TenantId = org.Id
              });
          }
      }
      logger.LogInformation("Chat-Nachrichten erstellt. Starte SubTickets und Kommentare...");
      await context.Messages.AddRangeAsync(messages).ConfigureAwait(false);

      // 10. SubTickets & Comments
      foreach (var ticket in tickets.Take(15))
      {
          var orgUsers = users.Where(u => u.TenantId == ticket.TenantId).ToList();
          if (orgUsers.Count == 0) continue;

          for (int i = 0; i < 3; i++)
          {
              context.SubTickets.Add(new SubTicket
              {
                  TicketId = ticket.Id,
                  Title = faker.Lorem.Sentence(3),
                  IsCompleted = faker.Random.Bool(),
                  CreatorId = ticket.CreatorId,
                  TenantId = ticket.TenantId,
                  CreatedAt = DateTime.UtcNow.AddDays(-1)
              });

              var commentContent = faker.Lorem.Sentence();
              var randomAuthor = faker.PickRandom(orgUsers);
              if (randomAuthor != null)
              {
                  var comment = new Comment(commentContent, ticket.Id, randomAuthor.Id);
                  comment.SetTenantId(ticket.TenantId);
                  comment.CreatedAt = DateTime.UtcNow.AddHours(-i);
                  context.Comments.Add(comment);
              }
          }
      }

      await context.SaveChangesAsync().ConfigureAwait(false);

      logger.LogInformation("Hochwertiges Seeding erfolgreich abgeschlossen.");
    }
    catch (Exception ex)
    {
      logger.LogError(ex, "Kritischer Fehler beim dynamischen Datenbank-Seeding.");
      throw new InvalidOperationException("Fehler beim synthetischen Seeding.", ex);
    }
  }
}
#pragma warning restore CA1848 // Use the LoggerMessage delegates
