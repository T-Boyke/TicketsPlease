using FluentAssertions;
using TicketsPlease.Domain.Entities;
using TicketsPlease.Infrastructure.Repositories;
using Xunit;

namespace TicketsPlease.UnitTests.Infrastructure.Repositories;

public class MessageRepositoryTests : InfrastructureTestBase
{
    private readonly MessageRepository _repository;

    public MessageRepositoryTests()
    {
        _repository = new MessageRepository(Context);
    }

    [Fact]
    public async Task GetByIdAsync_WhenExists_ShouldReturnMessage()
    {
        // Arrange
        var sender = new User { Id = Guid.NewGuid(), UserName = "Sender" };
        var msg = new Message { Id = Guid.NewGuid(), SenderUserId = sender.Id, BodyMarkdown = "Hello", SentAt = DateTime.UtcNow };

        Context.Users.Add(sender);
        Context.Messages.Add(msg);
        await Context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByIdAsync(msg.Id);

        // Assert
        result.Should().NotBeNull();
        result!.BodyMarkdown.Should().Be("Hello");
    }

    [Fact]
    public async Task GetConversationAsync_ShouldReturnMessagesInOrder()
    {
        // Arrange
        var u1 = new User { Id = Guid.NewGuid(), UserName = "U1" };
        var u2 = new User { Id = Guid.NewGuid(), UserName = "U2" };

        var m1 = new Message { Id = Guid.NewGuid(), SenderUserId = u1.Id, ReceiverUserId = u2.Id, BodyMarkdown = "1", SentAt = DateTime.UtcNow.AddMinutes(-5) };
        var m2 = new Message { Id = Guid.NewGuid(), SenderUserId = u2.Id, ReceiverUserId = u1.Id, BodyMarkdown = "2", SentAt = DateTime.UtcNow.AddMinutes(-2) };

        Context.Users.AddRange(u1, u2);
        Context.Messages.AddRange(m1, m2);
        await Context.SaveChangesAsync();

        // Act
        var result = await _repository.GetConversationAsync(u1.Id, u2.Id);

        // Assert
        result.Should().HaveCount(2);
        result[0].BodyMarkdown.Should().Be("1");
        result[1].BodyMarkdown.Should().Be("2");
    }
}
