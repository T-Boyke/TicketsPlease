using FluentAssertions;
using TicketsPlease.Domain.Entities;
using TicketsPlease.Infrastructure.Repositories;
using Xunit;

namespace TicketsPlease.UnitTests.Infrastructure.Repositories;

public class NotificationRepositoryTests : InfrastructureTestBase
{
    private readonly NotificationRepository _repository;

    public NotificationRepositoryTests()
    {
        _repository = new NotificationRepository(Context);
    }

    [Fact]
    public async Task GetByUserIdAsync_ShouldReturnPagedResults()
    {
        // Arrange
        var userId = Guid.NewGuid();
        for (int i = 0; i < 5; i++)
        {
            Context.Notifications.Add(new Notification { Id = Guid.NewGuid(), UserId = userId, Title = $"N{i}", CreatedAt = DateTime.UtcNow.AddMinutes(i) });
        }
        await Context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByUserIdAsync(userId, limit: 2, offset: 0);

        // Assert
        result.Should().HaveCount(2);
        // Da OrderByDescending(CreatedAt)
        result[0].Title.Should().Be("N4");
        result[1].Title.Should().Be("N3");
    }

    [Fact]
    public async Task GetByIdAsync_WhenExists_ShouldReturnNotification()
    {
        // Arrange
        var noti = new Notification { Id = Guid.NewGuid(), Title = "Test", UserId = Guid.NewGuid() };
        Context.Notifications.Add(noti);
        await Context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByIdAsync(noti.Id);

        // Assert
        result.Should().NotBeNull();
        result!.Title.Should().Be("Test");
    }
}
