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

public class DashboardServiceTests
{
    private readonly Mock<ITicketRepository> _ticketRepoMock;
    private readonly Mock<IProjectRepository> _projectRepoMock;
    private readonly Mock<ITeamRepository> _teamRepoMock;
    private readonly Mock<ITimeLogRepository> _timeLogRepoMock;
    private readonly Mock<UserManager<User>> _userManagerMock;
    private readonly Mock<RoleManager<Role>> _roleManagerMock;
    private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
    private readonly DashboardService _service;

    public DashboardServiceTests()
    {
        _ticketRepoMock = new Mock<ITicketRepository>();
        _projectRepoMock = new Mock<IProjectRepository>();
        _teamRepoMock = new Mock<ITeamRepository>();
        _timeLogRepoMock = new Mock<ITimeLogRepository>();

        var userStore = new Mock<IUserStore<User>>();
        _userManagerMock = new Mock<UserManager<User>>(userStore.Object, null, null, null, null, null, null, null, null);

        var roleStore = new Mock<IRoleStore<Role>>();
        _roleManagerMock = new Mock<RoleManager<Role>>(roleStore.Object, null, null, null, null);

        _httpContextAccessorMock = new Mock<IHttpContextAccessor>();

        _service = new DashboardService(
            _ticketRepoMock.Object,
            _projectRepoMock.Object,
            _teamRepoMock.Object,
            _timeLogRepoMock.Object,
            _userManagerMock.Object,
            _roleManagerMock.Object,
            _httpContextAccessorMock.Object);

        var httpContext = new DefaultHttpContext();
        httpContext.User = new ClaimsPrincipal(new ClaimsIdentity());
        _httpContextAccessorMock.Setup(a => a.HttpContext).Returns(httpContext);
    }

    private void SetupCurrentUser(User user)
    {
        _userManagerMock.Setup(m => m.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);
    }

    [Fact]
    public async Task GetDashboardStatsAsync_ShouldReturnValidStatsDto()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        var user = new User { Id = Guid.NewGuid(), TenantId = tenantId, UserName = "Admin" };
        SetupCurrentUser(user);

        var tickets = new List<Ticket>
        {
            new Ticket("T1", Domain.Enums.TicketType.Task, Guid.NewGuid(), user.Id, Guid.NewGuid(), "Done", "v1") { TenantId = tenantId },
            new Ticket("T2", Domain.Enums.TicketType.Bug, Guid.NewGuid(), user.Id, Guid.NewGuid(), "Todo", "v1") { TenantId = tenantId }
        };
        _ticketRepoMock.Setup(r => r.GetAllActiveAsync(default)).ReturnsAsync(tickets);
        _projectRepoMock.Setup(r => r.GetAllAsync(tenantId)).ReturnsAsync(new List<Project>());
        _teamRepoMock.Setup(r => r.GetAllTeamsAsync(default)).ReturnsAsync(new List<Team>());
        _timeLogRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<TimeLog>());

        _userManagerMock.Setup(m => m.Users).Returns(new List<User> { user }.AsQueryable());
        _roleManagerMock.Setup(m => m.Roles).Returns(new List<Role>().AsQueryable());

        // Act
        var result = await _service.GetDashboardStatsAsync();

        // Assert
        result.Should().NotBeNull();
        result.TotalTickets.Should().Be(2);
        result.ClosedTickets.Should().Be(1);
        result.OpenTickets.Should().Be(1);
    }

    [Fact]
    public async Task GetUserPerformanceDetailAsync_WhenExists_ShouldReturnPerformanceDto()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User { Id = userId, UserName = "User1" };
        _userManagerMock.Setup(m => m.FindByIdAsync(userId.ToString())).ReturnsAsync(user);

        var ticket = new Ticket("T", Domain.Enums.TicketType.Task, Guid.NewGuid(), userId, Guid.NewGuid(), "Done", "v1") { Id = Guid.NewGuid() };
        ticket.Close(userId, false, null);
        var tickets = new List<Ticket> { ticket };

        _ticketRepoMock.Setup(r => r.GetFilteredAsync(null, userId, null, null, null, null, null, null, null, default))
            .ReturnsAsync(tickets);
        _timeLogRepoMock.Setup(r => r.GetByUserIdAsync(userId, default)).ReturnsAsync(new List<TimeLog>());

        // Act
        var result = await _service.GetUserPerformanceDetailAsync(userId);

        // Assert
        result.Should().NotBeNull();
        result.TotalTickets.Should().Be(1);
        result.CompletedTickets.Should().Be(1);
    }
}
