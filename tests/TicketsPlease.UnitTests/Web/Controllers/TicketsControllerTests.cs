using System.Security.Claims;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Moq;
using TicketsPlease.Application.Common.Dtos;
using TicketsPlease.Application.Common.Interfaces;
using TicketsPlease.Domain.Entities;
using TicketsPlease.Domain.Enums;
using TicketsPlease.Infrastructure.Persistence;
using TicketsPlease.Web.Controllers;

namespace TicketsPlease.UnitTests.Web.Controllers;

public class TicketsControllerTests : IDisposable
{
    private readonly AppDbContext _context;
    private readonly Mock<ITicketService> _ticketServiceMock = new();
    private readonly Mock<IProjectService> _projectServiceMock = new();
    private readonly Mock<IFileAssetRepository> _fileAssetRepositoryMock = new();
    private readonly Mock<IFileStorageService> _fileStorageServiceMock = new();
    private readonly Mock<ITimeTrackingService> _timeTrackingServiceMock = new();
    private readonly Mock<ISubTicketService> _subTicketServiceMock = new();
    private readonly Mock<ITicketTemplateService> _templateServiceMock = new();
    private readonly Mock<IStringLocalizer<TicketsController>> _localizerMock = new();
    private readonly Mock<UserManager<User>> _userManagerMock;
    private readonly TicketsController _controller;
    private readonly User _currentUser;

    public TicketsControllerTests()
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

        _controller = new TicketsController(
            _ticketServiceMock.Object,
            _projectServiceMock.Object,
            _fileAssetRepositoryMock.Object,
            _fileStorageServiceMock.Object,
            _timeTrackingServiceMock.Object,
            _subTicketServiceMock.Object,
            _templateServiceMock.Object,
            _userManagerMock.Object,
            _context,
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
        _ticketServiceMock.Setup(x => x.GetFilteredTicketsAsync(
            It.IsAny<Guid?>(), It.IsAny<Guid?>(), It.IsAny<Guid?>(),
            It.IsAny<string?>(), It.IsAny<Guid?>(), It.IsAny<DateTime?>(),
            It.IsAny<DateTime?>(), It.IsAny<string?>(), It.IsAny<Guid?>()))
            .ReturnsAsync(new List<TicketDto>());

        // Act
        var result = await _controller.Index(null, null, null, null, null, null, null, null, null);

        // Assert
        result.Should().BeOfType<ViewResult>();
    }

    [Fact]
    public async Task Details_TicketNotFound_ReturnsNotFound()
    {
        // Act
        var result = await _controller.Details(Guid.NewGuid());

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task Details_TicketExists_ReturnsViewResult()
    {
        // Arrange
        var ticketId = Guid.NewGuid();
        var ticketDto = new TicketDto(
            ticketId, "Test Ticket", "Desc", "Todo", Guid.NewGuid(), "Proj", null, "User",
            TicketType.Task, new TicketPriorityDto(Guid.NewGuid(), "High", "#FF0000"),
            DateTime.UtcNow, 5, 3, new List<TagDto>(), new List<TimeLogDto>(), new List<SubTicketDto>(),
            false, new List<CommentDto>(), new List<TicketLinkDto>(), new List<TicketLinkDto>(),
            new List<FileAssetDto>(), new List<TicketHistoryDto>(), 0, false, new byte[8]);

        _ticketServiceMock.Setup(x => x.GetTicketAsync(ticketId)).ReturnsAsync(ticketDto);

        // Act
        var result = await _controller.Details(ticketId);

        // Assert
        result.Should().BeOfType<ViewResult>();
        var viewResult = result as ViewResult;
        ((TicketDto)viewResult!.Model!).Id.Should().Be(ticketId);
    }

    [Fact]
    public async Task ToggleUpvote_TicketNotFound_ReturnsNotFound()
    {
        // Act
        var result = await _controller.ToggleUpvote(Guid.NewGuid());

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
