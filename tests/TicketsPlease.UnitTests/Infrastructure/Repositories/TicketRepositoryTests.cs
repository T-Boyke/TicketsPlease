using FluentAssertions;
using TicketsPlease.Domain.Entities;
using TicketsPlease.Domain.Enums;
using TicketsPlease.Infrastructure.Repositories;
using Xunit;

namespace TicketsPlease.UnitTests.Infrastructure.Repositories;

public class TicketRepositoryTests : InfrastructureTestBase
{
    private readonly TicketRepository _repository;

    public TicketRepositoryTests()
    {
        _repository = new TicketRepository(Context);
    }

    [Fact]
    public async Task GetByIdAsync_WhenExists_ShouldReturnTicketWithIncludes()
    {
        // Arrange
        var user = new User { Id = Guid.NewGuid(), UserName = "Test" };
        var priority = new TicketPriority { Id = Guid.NewGuid(), Name = "High", ColorHex = "#FF0000" };
        var project = new Project("Project", DateTime.UtcNow);
        var state = new WorkflowState { Id = Guid.NewGuid(), Name = "Todo" };

        var ticket = new Ticket("Ticket", TicketType.Task, project.Id, user.Id, state.Id, "Todo", "123");
        ticket.SetPriority(priority.Id);
        ticket.UpdateDescription("Some description", "Some description");

        Context.Users.Add(user);
        Context.TicketPriorities.Add(priority);
        Context.Projects.Add(project);
        Context.WorkflowStates.Add(state);
        Context.Tickets.Add(ticket);
        await Context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByIdAsync(ticket.Id);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(ticket.Id);
    }

    [Fact]
    public async Task GetFilteredAsync_BySearchString_ShouldReturnMatches()
    {
        // Arrange
        var user = new User { Id = Guid.NewGuid(), UserName = "Test" };
        var project = new Project("P", DateTime.UtcNow);
        var priority = new TicketPriority { Id = Guid.NewGuid(), Name = "Low", LevelWeight = 1, ColorHex = "#000" };
        var state = new WorkflowState { Id = Guid.NewGuid(), Name = "S" };

        var t1 = new Ticket("SearchableTitle", TicketType.Task, project.Id, user.Id, state.Id, "Todo", "1");
        t1.SetPriority(priority.Id);
        t1.UpdateDescription("Desc1", "Desc1");

        Context.Users.Add(user);
        Context.Projects.Add(project);
        Context.TicketPriorities.Add(priority);
        Context.WorkflowStates.Add(state);
        Context.Tickets.Add(t1);
        await Context.SaveChangesAsync();

        // Act
        var result = await _repository.GetFilteredAsync(searchString: "Searchable");

        // Assert
        result.Should().HaveCount(1);
        result.First().Title.Should().Be("SearchableTitle");
    }

    [Fact]
    public async Task AddUpvoteAsync_ShouldIncreaseCount()
    {
        // Arrange
        var ticketId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var upvote = new TicketUpvote { TicketId = ticketId, UserId = userId, VotedAt = DateTime.UtcNow };

        // Act
        await _repository.AddUpvoteAsync(upvote);
        await _repository.SaveChangesAsync();

        // Assert
        var count = await _repository.GetUpvoteCountAsync(ticketId);
        count.Should().Be(1);

        var hasUpvoted = await _repository.UserHasUpvotedAsync(ticketId, userId);
        hasUpvoted.Should().BeTrue();
    }
}
