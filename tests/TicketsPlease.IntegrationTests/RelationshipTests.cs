// <copyright file="RelationshipTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TicketsPlease.IntegrationTests;

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TicketsPlease.Domain.Entities;
using TicketsPlease.Domain.Enums;
using TicketsPlease.Infrastructure.Persistence;
using Xunit;

/// <summary>
/// Verifiziert die Datenbank-Beziehungen und Constraints im integrierten System.
/// Nutzt die SQLite-In-Memory-Datenbank der Basisklasse.
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores", Justification = "Test naming convention")]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Reliability", "CA2007:Consider calling ConfigureAwait", Justification = "xUnit tests should not use ConfigureAwait(false)")]
public class RelationshipTests : IntegrationTestBase
{
  /// <summary>
  /// Prüft, ob ein Ticket einem Benutzer korrekt zugewiesen werden kann und die Beziehung persistiert wird.
  /// </summary>
  /// <returns>Ein <see cref="Task"/>, der die asynchrone Testausführung repräsentiert.</returns>
  [Fact]
  public async Task Ticket_Should_Be_Correctly_Assigned_To_User()
  {
    // Arrange
    using var scope = this.Factory.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    await SeedMinimalAsync(db);
    var roleId = (await db.Roles.FirstAsync()).Id;
    var priorityId = (await db.TicketPriorities.FirstAsync()).Id;
    var workflowStateId = (await db.WorkflowStates.FirstAsync()).Id;

    var project = await db.Projects.FirstAsync();

    var userId = Guid.NewGuid();
    var user = new User 
    { 
      Id = userId, 
      UserName = "verify@example.com", 
      NormalizedUserName = "VERIFY@EXAMPLE.COM",
      Email = "verify@example.com", 
      NormalizedEmail = "VERIFY@EXAMPLE.COM",
      RoleId = roleId, 
      TenantId = project.TenantId 
    };
    user.Profile = new UserProfile { UserId = userId, FirstName = "Verifikations", LastName = "User", TenantId = project.TenantId };
    
    db.Users.Add(user);
    await db.SaveChangesAsync();
    
    var ticket = new Ticket("Beziehungs-Test", TicketType.Task, project.Id, user.Id, workflowStateId, "127.0.0.1");
    ticket.UpdateDescription("Testet die FK-Integrität", "Testet die FK-Integrität");
    ticket.SetPriority(priorityId);
    ticket.AssignUser(user.Id);
    ticket.SetTenantId(project.TenantId);

    // Act
    db.Tickets.Add(ticket);
    await db.SaveChangesAsync();

    // Assert
    var savedTicket = await db.Tickets
        .Include(t => t.AssignedUser)
            .ThenInclude(u => u!.Profile)
        .FirstOrDefaultAsync(t => t.Id == ticket.Id);

    savedTicket.Should().NotBeNull();
    savedTicket!.AssignedUserId.Should().Be(user.Id);
    savedTicket.AssignedUser.Should().NotBeNull();
    savedTicket.AssignedUser!.Profile.Should().NotBeNull();
    savedTicket.AssignedUser!.Profile.FullName.Should().Be("Verifikations User");
  }

  /// <summary>
  /// Verifiziert, dass das Löschverhalten (DeleteBehavior.Restrict) eingehalten wird.
  /// Ein Benutzer darf nicht gelöscht werden, wenn ihm noch Tickets zugewiesen sind.
  /// </summary>
  /// <returns>Ein <see cref="Task"/>, der die asynchrone Testausführung repräsentiert.</returns>
  [Fact]
  public async Task Deleting_User_With_Tickets_Should_Fail_Due_To_Restrict_Behavior()
  {
    // Arrange
    using var scope = this.Factory.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    await SeedMinimalAsync(db);
    var roleId = (await db.Roles.FirstAsync()).Id;
    var priorityId = (await db.TicketPriorities.FirstAsync()).Id;
    var workflowStateId = (await db.WorkflowStates.FirstAsync()).Id;

    var project = await db.Projects.FirstAsync();

    var userId = Guid.NewGuid();
    var user = new User 
    { 
      Id = userId, 
      UserName = "restrict@example.com", 
      NormalizedUserName = "RESTRICT@EXAMPLE.COM",
      Email = "restrict@example.com", 
      NormalizedEmail = "RESTRICT@EXAMPLE.COM",
      RoleId = roleId, 
      TenantId = project.TenantId 
    };
    user.Profile = new UserProfile { UserId = userId, FirstName = "Restrict", LastName = "User", TenantId = project.TenantId };

    db.Users.Add(user);
    await db.SaveChangesAsync();

    var ticket = new Ticket("Restrict-Test", TicketType.Task, project.Id, user.Id, workflowStateId, "127.0.0.1");
    ticket.AssignUser(user.Id);
    ticket.SetPriority(priorityId);
    ticket.SetTenantId(project.TenantId);

    db.Tickets.Add(ticket);
    await db.SaveChangesAsync();

    // Act & Assert
    var act = async () =>
    {
      db.Users.Remove(user);
      await db.SaveChangesAsync();
    };

    await act.Should().ThrowAsync<Exception>(); // SQLite oder EF Core wirft hier eine DbUpdateException oder ähnliches
  }
}
