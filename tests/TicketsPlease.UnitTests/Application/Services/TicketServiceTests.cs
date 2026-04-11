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

public class TicketServiceTests
{
    private readonly Mock<ITicketRepository> _ticketRepoMock;
    private readonly Mock<UserManager<User>> _userManagerMock;
    private readonly Mock<RoleManager<Role>> _roleManagerMock;
    private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
    private readonly Mock<IFileAssetRepository> _fileAssetRepoMock;
    private readonly Mock<IFileStorageService> _fileStorageServiceMock;
    private readonly Mock<ITimeTrackingService> _timeTrackingServiceMock;
    private readonly Mock<ISubTicketService> _subTicketServiceMock;
    private readonly Mock<INotificationService> _notificationServiceMock;
    private readonly TicketService _service;

    public TicketServiceTests()
    {
        _ticketRepoMock = new Mock<ITicketRepository>();

        var userStore = new Mock<IUserStore<User>>();
        _userManagerMock = new Mock<UserManager<User>>(userStore.Object, null, null, null, null, null, null, null, null);

        var roleStore = new Mock<IRoleStore<Role>>();
        _roleManagerMock = new Mock<RoleManager<Role>>(roleStore.Object, null, null, null, null);

        _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        _fileAssetRepoMock = new Mock<IFileAssetRepository>();
        _fileStorageServiceMock = new Mock<IFileStorageService>();
        _timeTrackingServiceMock = new Mock<ITimeTrackingService>();
        _subTicketServiceMock = new Mock<ISubTicketService>();
        _notificationServiceMock = new Mock<INotificationService>();

        _service = new TicketService(
            _ticketRepoMock.Object,
            _userManagerMock.Object,
            _roleManagerMock.Object,
            _httpContextAccessorMock.Object,
            _fileAssetRepoMock.Object,
            _fileStorageServiceMock.Object,
            _timeTrackingServiceMock.Object,
            _subTicketServiceMock.Object,
            _notificationServiceMock.Object);

        // Default Mock Behavior
        _timeTrackingServiceMock.Setup(s => s.GetTimeLogsAsync(It.IsAny<Guid>())).ReturnsAsync(new List<TimeLogDto>());
        _subTicketServiceMock.Setup(s => s.GetSubTicketsAsync(It.IsAny<Guid>())).ReturnsAsync(new List<SubTicketDto>());

        var httpContext = new DefaultHttpContext();
        httpContext.User = new ClaimsPrincipal(new ClaimsIdentity());
        _httpContextAccessorMock.Setup(a => a.HttpContext).Returns(httpContext);
    }

    private void SetupCurrentUser(User user)
    {
        var httpContext = new DefaultHttpContext();
        httpContext.User = new ClaimsPrincipal(new ClaimsIdentity());
        _httpContextAccessorMock.Setup(a => a.HttpContext).Returns(httpContext);
        _userManagerMock.Setup(m => m.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);
    }

    [Fact]
    public async Task GetTicketAsync_WhenExists_ShouldReturnMappedDto()
    {
        // Arrange
        var id = Guid.NewGuid();
        var ticket = new Ticket("Title", Domain.Enums.TicketType.Task, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "Todo", "v1") { Id = id };
        _ticketRepoMock.Setup(r => r.GetByIdAsync(id, default)).ReturnsAsync(ticket);

        // Act
        var result = await _service.GetTicketAsync(id);

        // Assert
        result.Should().NotBeNull();
        result!.Title.Should().Be("Title");
    }

    [Fact]
    public async Task CreateTicketAsync_ShouldSetDefaultWorkflowStateAndSave()
    {
        // Arrange
        var user = new User { Id = Guid.NewGuid() };
        SetupCurrentUser(user);
        var dto = new CreateTicketDto("New Ticket", "Desc", Guid.NewGuid(), Guid.NewGuid(), null, 1, 1, new List<Guid>());

        var workflowState = new WorkflowState { Id = Guid.NewGuid(), Name = "Todo" };
        _ticketRepoMock.Setup(r => r.GetDefaultWorkflowStateAsync(default)).ReturnsAsync(workflowState);

        // Act
        await _service.CreateTicketAsync(dto);

        // Assert
        _ticketRepoMock.Verify(r => r.AddAsync(It.Is<Ticket>(t => t.Title == "New Ticket" && t.Status == "Todo"), default), Times.Once);
        _ticketRepoMock.Verify(r => r.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task MoveTicketAsync_ShouldUpdateStatusAndSave()
    {
        // Arrange
        var id = Guid.NewGuid();
        var user = new User { Id = Guid.NewGuid() };
        SetupCurrentUser(user);
        var fromStateId = Guid.NewGuid();
        var ticket = new Ticket("T", Domain.Enums.TicketType.Task, Guid.NewGuid(), user.Id, fromStateId, "Todo", "v1") { Id = id };
        _ticketRepoMock.Setup(r => r.GetByIdAsync(id, default)).ReturnsAsync(ticket);

        var toStateId = Guid.NewGuid();
        var targetState = new WorkflowState { Id = toStateId, Name = "Doing" };
        _ticketRepoMock.Setup(r => r.GetWorkflowStateByNameAsync("Doing", default)).ReturnsAsync(targetState);
        _ticketRepoMock.Setup(r => r.GetTransitionAsync(fromStateId, toStateId, default))
            .ReturnsAsync(new WorkflowTransition { FromStateId = fromStateId, ToStateId = toStateId });

        // Act
        await _service.MoveTicketAsync(id, "Doing");

        // Assert
        ticket.WorkflowStateId.Should().Be(toStateId);
        ticket.Status.Should().Be("Doing");
        _ticketRepoMock.Verify(r => r.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task UpdateTicketAsync_WhenTitleChanged_ShouldAddHistoryAndSave()
    {
        // Arrange
        var id = Guid.NewGuid();
        var user = new User { Id = Guid.NewGuid() };
        SetupCurrentUser(user);
        var ticket = new Ticket("Old Title", Domain.Enums.TicketType.Task, Guid.NewGuid(), user.Id, Guid.NewGuid(), "Todo", "v1") { Id = id };
        _ticketRepoMock.Setup(r => r.GetByIdAsync(id, default)).ReturnsAsync(ticket);

        var dto = new UpdateTicketDto(id, "New Title", "Desc", "Todo", Guid.NewGuid(), user.Id, 1, 1, null, null);

        // Act
        await _service.UpdateTicketAsync(dto);

        // Assert
        ticket.Title.Should().Be("New Title");
        _ticketRepoMock.Verify(r => r.AddHistoryAsync(It.Is<TicketHistory>(h => h.FieldName == "Title")), Times.Once);
        _ticketRepoMock.Verify(r => r.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task AddDependencyAsync_ShouldAddLinkToBlockerAndSave()
    {
        // Arrange
        var id = Guid.NewGuid();
        var blockerId = Guid.NewGuid();
        var user = new User { Id = Guid.NewGuid() };
        SetupCurrentUser(user);
        var ticket = new Ticket("Target", Domain.Enums.TicketType.Task, Guid.NewGuid(), user.Id, Guid.NewGuid(), "Todo", "v1") { Id = id };
        var blocker = new Ticket("Source", Domain.Enums.TicketType.Task, Guid.NewGuid(), user.Id, Guid.NewGuid(), "Todo", "v1") { Id = blockerId };

        _ticketRepoMock.Setup(r => r.GetByIdAsync(id, default)).ReturnsAsync(ticket);
        _ticketRepoMock.Setup(r => r.GetByIdAsync(blockerId, default)).ReturnsAsync(blocker);

        // Act
        await _service.AddDependencyAsync(id, blockerId);

        // Assert
        blocker.Blocking.Should().HaveCount(1);
        blocker.Blocking.First().TargetTicketId.Should().Be(id);
        _ticketRepoMock.Verify(r => r.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task UpvoteAsync_WhenNotVoted_ShouldAddUpvoteAndSave()
    {
        // Arrange
        var id = Guid.NewGuid();
        var user = new User { Id = Guid.NewGuid() };
        SetupCurrentUser(user);
        _ticketRepoMock.Setup(r => r.UserHasUpvotedAsync(id, user.Id)).ReturnsAsync(false);

        // Act
        await _service.UpvoteAsync(id);

        // Assert
        _ticketRepoMock.Verify(r => r.AddUpvoteAsync(It.Is<TicketUpvote>(u => u.TicketId == id && u.UserId == user.Id)), Times.Once);
        _ticketRepoMock.Verify(r => r.SaveChangesAsync(default), Times.Once);
    }
}
