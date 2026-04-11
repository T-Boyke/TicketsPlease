namespace TicketsPlease.UnitTests.Application.Services;

using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Moq;
using System.Security.Claims;
using TicketsPlease.Application.Common.Dtos;
using TicketsPlease.Application.Common.Interfaces;
using TicketsPlease.Application.Services;
using TicketsPlease.Domain.Entities;
using Xunit;

public class CommentServiceTests
{
    private readonly Mock<ICommentRepository> _commentRepoMock;
    private readonly Mock<ITicketRepository> _ticketRepoMock;
    private readonly Mock<UserManager<User>> _userManagerMock;
    private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
    private readonly Mock<INotificationService> _notificationServiceMock;
    private readonly CommentService _service;

    public CommentServiceTests()
    {
        _commentRepoMock = new Mock<ICommentRepository>();
        _ticketRepoMock = new Mock<ITicketRepository>();

        var userStore = new Mock<IUserStore<User>>();
        _userManagerMock = new Mock<UserManager<User>>(userStore.Object, null, null, null, null, null, null, null, null);

        _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        _notificationServiceMock = new Mock<INotificationService>();

        _service = new CommentService(
            _commentRepoMock.Object,
            _ticketRepoMock.Object,
            _userManagerMock.Object,
            _httpContextAccessorMock.Object,
            _notificationServiceMock.Object);
    }

    private void SetupCurrentUser(User user)
    {
        var httpContext = new DefaultHttpContext();
        httpContext.User = new ClaimsPrincipal(new ClaimsIdentity());
        _httpContextAccessorMock.Setup(a => a.HttpContext).Returns(httpContext);
        _userManagerMock.Setup(m => m.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);
    }

    [Fact]
    public async Task GetCommentsForTicketAsync_ShouldReturnMappedDtos()
    {
        // Arrange
        var ticketId = Guid.NewGuid();
        var comments = new List<Comment> { new Comment("Content", ticketId, Guid.NewGuid()) };
        _commentRepoMock.Setup(r => r.GetByTicketIdAsync(ticketId)).ReturnsAsync(comments);

        // Act
        var result = await _service.GetCommentsForTicketAsync(ticketId);

        // Assert
        result.Should().HaveCount(1);
        result.First().Content.Should().Be("Content");
    }

    [Fact]
    public async Task CreateCommentAsync_ShouldAddCommentAndNotify()
    {
        // Arrange
        var user = new User { Id = Guid.NewGuid(), UserName = "TestUser", TenantId = Guid.NewGuid() };
        SetupCurrentUser(user);
        var ticketId = Guid.NewGuid();
        var dto = new CreateCommentDto(ticketId, "New Comment");

        var ticket = new Ticket("T", Domain.Enums.TicketType.Task, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "Todo", "v1") { Id = ticketId };
        _ticketRepoMock.Setup(r => r.GetByIdAsync(ticketId, default)).ReturnsAsync(ticket);

        // Act
        await _service.CreateCommentAsync(dto);

        // Assert
        _commentRepoMock.Verify(r => r.AddAsync(It.Is<Comment>(c => c.Content == "New Comment" && c.TicketId == ticketId)), Times.Once);
        _commentRepoMock.Verify(r => r.SaveChangesAsync(default), Times.Once);
        _notificationServiceMock.Verify(n => n.NotifyNewCommentAsync(ticketId, It.IsAny<CommentDto>()), Times.Once);
    }
}
