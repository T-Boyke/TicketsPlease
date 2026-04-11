using System.Security.Claims;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TicketsPlease.Application.Common.Dtos;
using TicketsPlease.Application.Common.Interfaces;
using TicketsPlease.Web.Controllers;
using TicketsPlease.Web.Models;

namespace TicketsPlease.UnitTests.Web.Controllers;

public class NotificationsControllerTests
{
    private readonly Mock<INotificationService> _notificationServiceMock = new();
    private readonly NotificationsController _controller;
    private readonly Guid _userId = Guid.NewGuid();

    public NotificationsControllerTests()
    {
        _controller = new NotificationsController(_notificationServiceMock.Object);

        var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.NameIdentifier, _userId.ToString())
        }, "TestAuth"));

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };
    }

    [Fact]
    public async Task Index_ReturnsViewResultWithModel()
    {
        // Arrange
        _notificationServiceMock.Setup(x => x.GetNotificationsForUserAsync(_userId, It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(new List<NotificationDto>());

        // Act
        var result = await _controller.Index();

        // Assert
        var viewResult = result.Should().BeOfType<ViewResult>().Subject;
        var model = viewResult.Model.Should().BeOfType<NotificationsViewModel>().Subject;
        model.Notifications.Should().BeEmpty();
    }

    [Fact]
    public async Task MarkAsRead_ReturnsOk()
    {
        // Act
        var result = await _controller.MarkAsRead(Guid.NewGuid());

        // Assert
        result.Should().BeOfType<OkResult>();
    }
}
