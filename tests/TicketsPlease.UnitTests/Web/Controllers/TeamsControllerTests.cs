using System.Security.Claims;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Moq;
using TicketsPlease.Application.Common.Dtos;
using TicketsPlease.Application.Common.Interfaces;
using TicketsPlease.Domain.Entities;
using TicketsPlease.Web.Controllers;

namespace TicketsPlease.UnitTests.Web.Controllers;

public class TeamsControllerTests
{
    private readonly Mock<ITeamService> _teamServiceMock = new();
    private readonly Mock<UserManager<User>> _userManagerMock;
    private readonly Mock<INotificationService> _notificationServiceMock = new();
    private readonly Mock<IStringLocalizer<TeamsController>> _localizerMock = new();
    private readonly TeamsController _controller;
    private readonly User _currentUser;

    public TeamsControllerTests()
    {
        var store = new Mock<IUserStore<User>>();
        _userManagerMock = new Mock<UserManager<User>>(store.Object, null!, null!, null!, null!, null!, null!, null!, null!);

        _currentUser = new User { Id = Guid.NewGuid(), UserName = "testuser", TenantId = Guid.NewGuid() };
        _userManagerMock.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(_currentUser);

        _controller = new TeamsController(
            _teamServiceMock.Object,
            _userManagerMock.Object,
            _notificationServiceMock.Object,
            _localizerMock.Object);

        var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.NameIdentifier, _currentUser.Id.ToString()),
            new Claim("TenantId", _currentUser.TenantId.ToString())
        }, "TestAuth"));

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };
    }

    [Fact]
    public async Task Index_ReturnsViewResult()
    {
        // Arrange
        _teamServiceMock.Setup(x => x.GetAllTeamsAsync(_currentUser.Id))
            .ReturnsAsync(new List<TeamDto>());

        // Act
        var result = await _controller.Index();

        // Assert
        result.Should().BeOfType<ViewResult>();
    }

    [Fact]
    public async Task Details_TeamNotFound_ReturnsNotFound()
    {
        // Act
        var result = await _controller.Details(Guid.NewGuid());

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task Details_TeamExists_ReturnsViewResult()
    {
        // Arrange
        var teamId = Guid.NewGuid();
        var teamDto = new TeamDto(teamId, "Team", "Desc", "#000", DateTime.UtcNow, 0, new List<TeamMemberDto>());
        _teamServiceMock.Setup(x => x.GetTeamDetailsAsync(teamId)).ReturnsAsync(teamDto);

        // Act
        var result = await _controller.Details(teamId);

        // Assert
        result.Should().BeOfType<ViewResult>();
        var viewResult = result as ViewResult;
        ((TeamDto)viewResult!.Model!).Id.Should().Be(teamId);
    }
}
