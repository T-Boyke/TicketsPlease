using System.Security.Claims;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using TicketsPlease.Application.Common.Dtos;
using TicketsPlease.Application.Common.Interfaces;
using TicketsPlease.Domain.Entities;
using TicketsPlease.Infrastructure.Persistence;
using TicketsPlease.Web.Controllers;

namespace TicketsPlease.UnitTests.Web.Controllers;

public class MessagesControllerTests : IDisposable
{
    private readonly AppDbContext _context;
    private readonly Mock<IMessageService> _messageServiceMock = new();
    private readonly Mock<UserManager<User>> _userManagerMock;
    private readonly MessagesController _controller;
    private readonly User _currentUser;

    public MessagesControllerTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _context = new AppDbContext(options);

        var store = new Mock<IUserStore<User>>();
        _userManagerMock = new Mock<UserManager<User>>(store.Object, null!, null!, null!, null!, null!, null!, null!, null!);

        _currentUser = new User { Id = Guid.NewGuid(), UserName = "testuser", TenantId = Guid.NewGuid() };
        _userManagerMock.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(_currentUser);
        _userManagerMock.Setup(x => x.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns(_currentUser.Id.ToString());

        _controller = new MessagesController(_messageServiceMock.Object, _userManagerMock.Object, _context);

        var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.NameIdentifier, _currentUser.Id.ToString())
        }, "TestAuth"));

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };
    }

    [Fact]
    public async Task Index_ReturnsViewResultWithMessages()
    {
        // Arrange
        _messageServiceMock.Setup(x => x.GetUserMessagesAsync(_currentUser.Id))
            .ReturnsAsync(new List<MessageDto>());

        // Act
        var result = await _controller.Index();

        // Assert
        result.Should().BeOfType<ViewResult>();
    }

    [Fact]
    public async Task Conversation_WithInvalidUser_ReturnsNotFound()
    {
        // Act
        var result = await _controller.Conversation(Guid.NewGuid());

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
        GC.SuppressFinalize(this);
    }
}
