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

        var user = new User { DisplayName = "Verifikations-User", Email = "verify@example.com" };
        var ticket = new Ticket { Title = "Beziehungs-Test", Description = "Testet die FK-Integrität" };

        ticket.AssignedUser = user;

        // Act
        db.Users.Add(user);
        db.Tickets.Add(ticket);
        await db.SaveChangesAsync();

        // Assert
        var savedTicket = await db.Tickets
            .Include(t => t.AssignedUser)
            .FirstOrDefaultAsync(t => t.Id == ticket.Id);

        savedTicket.Should().NotBeNull();
        savedTicket!.AssignedUserId.Should().Be(user.Id);
        savedTicket.AssignedUser.Should().NotBeNull();
        savedTicket.AssignedUser!.DisplayName.Should().Be("Verifikations-User");
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

        var user = new User { DisplayName = "Restrict-User", Email = "restrict@example.com" };
        var ticket = new Ticket { Title = "Restrict-Test", AssignedUser = user };

        db.Users.Add(user);
        db.Tickets.Add(ticket);
        await db.SaveChangesAsync();

        // Act
        db.Users.Remove(user);

        // Assert: Bei SQLite wird der Constraint-Verstoß oft erst beim SaveChanges ausgelöst
        var act = async () => await db.SaveChangesAsync();
        await act.Should().ThrowAsync<Exception>(); // SQLite oder EF Core wirft hier eine DbUpdateException oder ähnliches
    }
}
