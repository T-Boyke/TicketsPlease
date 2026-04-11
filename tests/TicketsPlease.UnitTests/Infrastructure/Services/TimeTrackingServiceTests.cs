namespace TicketsPlease.UnitTests.Infrastructure.Services;

using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using TicketsPlease.Domain.Entities;
using TicketsPlease.Domain.Enums;
using TicketsPlease.Infrastructure.Services;
using Xunit;

public class TimeTrackingServiceTests : InfrastructureTestBase
{
    private readonly TimeTrackingService _service;

    public TimeTrackingServiceTests()
    {
        _service = new TimeTrackingService(Context);
    }

    [Fact]
    public async Task StartTimeTrackingAsync_ShouldCreateNewLog()
    {
        // Arrange
        var user = new User { Id = Guid.NewGuid(), UserName = "Test" };
        var ticket = new Ticket("Ticket", TicketType.Task, Guid.NewGuid(), user.Id, Guid.NewGuid(), "Todo", "123");
        Context.Users.Add(user);
        Context.Tickets.Add(ticket);
        await Context.SaveChangesAsync();

        // Act
        await _service.StartTimeTrackingAsync(ticket.Id, user.Id);

        // Assert
        var log = await Context.TimeLogs.FirstOrDefaultAsync(l => l.TicketId == ticket.Id && l.UserId == user.Id);
        log.Should().NotBeNull();
        log!.StartedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(10));
        log.StoppedAt.Should().BeNull();
    }

    [Fact]
    public async Task StopTimeTrackingAsync_ShouldUpdateExistingLog()
    {
        // Arrange
        var user = new User { Id = Guid.NewGuid(), UserName = "Test" };
        var ticket = new Ticket("Ticket", TicketType.Task, Guid.NewGuid(), user.Id, Guid.NewGuid(), "Todo", "123");
        Context.Users.Add(user);
        Context.Tickets.Add(ticket);
        await Context.SaveChangesAsync();

        var log = new TimeLog
        {
            Id = Guid.NewGuid(),
            TicketId = ticket.Id,
            UserId = user.Id,
            StartedAt = DateTime.UtcNow.AddHours(-2),
            HoursLogged = 0
        };
        Context.TimeLogs.Add(log);
        await Context.SaveChangesAsync();

        // Act
        await _service.StopTimeTrackingAsync(ticket.Id, user.Id);

        // Assert
        var updatedLog = await Context.TimeLogs.FindAsync(log.Id);
        updatedLog.Should().NotBeNull();
        updatedLog!.StoppedAt.Should().NotBeNull();
        updatedLog.HoursLogged.Should().BeApproximately(2.0m, 0.1m);
    }

    [Fact]
    public async Task GetTimeLogsAsync_ShouldReturnLogsForTicket()
    {
        // Arrange
        var user = new User { Id = Guid.NewGuid(), UserName = "TestUser", Email = "test@test.com" };
        var ticket = new Ticket("Ticket", TicketType.Task, Guid.NewGuid(), user.Id, Guid.NewGuid(), "Todo", "123");
        Context.Users.Add(user);
        Context.Tickets.Add(ticket);

        var log = new TimeLog
        {
            Id = Guid.NewGuid(),
            TicketId = ticket.Id,
            UserId = user.Id,
            StartedAt = DateTime.UtcNow,
            HoursLogged = 1.5m
        };
        Context.TimeLogs.Add(log);
        await Context.SaveChangesAsync();

        // Act
        var result = await _service.GetTimeLogsAsync(ticket.Id);

        // Assert
        result.Should().HaveCount(1);
        result.First().UserName.Should().Be("TestUser");
    }
}
