namespace TicketsPlease.UnitTests.Infrastructure.Services;

using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using TicketsPlease.Domain.Entities;
using TicketsPlease.Domain.Enums;
using TicketsPlease.Infrastructure.Services;
using Xunit;

public class SubTicketServiceTests : InfrastructureTestBase
{
    private readonly SubTicketService _service;

    public SubTicketServiceTests()
    {
        _service = new SubTicketService(Context);
    }

    [Fact]
    public async Task AddSubTicketAsync_ShouldAddAndReturnDto()
    {
        // Arrange
        var user = new User { Id = Guid.NewGuid(), UserName = "Test" };
        var ticket = new Ticket("Ticket", TicketType.Task, Guid.NewGuid(), user.Id, Guid.NewGuid(), "Todo", "123");
        Context.Users.Add(user);
        Context.Tickets.Add(ticket);
        await Context.SaveChangesAsync();

        // Act
        var result = await _service.AddSubTicketAsync(ticket.Id, "Test Sub", user.Id);

        // Assert
        result.Title.Should().Be("Test Sub");
        result.IsCompleted.Should().BeFalse();

        var inDb = await Context.SubTickets.FindAsync(result.Id);
        inDb.Should().NotBeNull();
        inDb!.TicketId.Should().Be(ticket.Id);
    }

    [Fact]
    public async Task ToggleSubTicketAsync_ShouldFlipCompletionStatus()
    {
        // Arrange
        var user = new User { Id = Guid.NewGuid(), UserName = "Test" };
        var ticket = new Ticket("Ticket", TicketType.Task, Guid.NewGuid(), user.Id, Guid.NewGuid(), "Todo", "123");
        Context.Users.Add(user);
        Context.Tickets.Add(ticket);
        await Context.SaveChangesAsync();

        var sub = new SubTicket { Id = Guid.NewGuid(), Title = "Sub", IsCompleted = false, TicketId = ticket.Id, CreatorId = user.Id, CreatedAt = DateTime.UtcNow };
        Context.SubTickets.Add(sub);
        await Context.SaveChangesAsync();

        // Act
        await _service.ToggleSubTicketAsync(sub.Id);

        // Assert
        var updated = await Context.SubTickets.FindAsync(sub.Id);
        updated!.IsCompleted.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteSubTicketAsync_ShouldRemoveFromDb()
    {
        // Arrange
        var user = new User { Id = Guid.NewGuid(), UserName = "Test" };
        var ticket = new Ticket("Ticket", TicketType.Task, Guid.NewGuid(), user.Id, Guid.NewGuid(), "Todo", "123");
        Context.Users.Add(user);
        Context.Tickets.Add(ticket);
        await Context.SaveChangesAsync();

        var sub = new SubTicket { Id = Guid.NewGuid(), Title = "Sub", TicketId = ticket.Id, CreatorId = user.Id, CreatedAt = DateTime.UtcNow };
        Context.SubTickets.Add(sub);
        await Context.SaveChangesAsync();

        // Act
        await _service.DeleteSubTicketAsync(sub.Id);

        // Assert
        var inDb = await Context.SubTickets.FindAsync(sub.Id);
        inDb.Should().BeNull();
    }
}
