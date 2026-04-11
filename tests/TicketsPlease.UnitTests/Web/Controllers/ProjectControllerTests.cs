using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TicketsPlease.Application.Common.Dtos;
using TicketsPlease.Application.Common.Interfaces;
using TicketsPlease.Web.Controllers;

namespace TicketsPlease.UnitTests.Web.Controllers;

public class ProjectControllerTests
{
    private readonly Mock<IProjectService> _projectServiceMock = new();
    private readonly Mock<ITicketService> _ticketServiceMock = new();
    private readonly ProjectController _controller;

    public ProjectControllerTests()
    {
        _controller = new ProjectController(_projectServiceMock.Object, _ticketServiceMock.Object);
    }

    [Fact]
    public async Task Index_ReturnsViewResult()
    {
        // Arrange
        _projectServiceMock.Setup(x => x.GetProjectsAsync())
            .ReturnsAsync(new List<ProjectDto>());

        // Act
        var result = await _controller.Index();

        // Assert
        result.Should().BeOfType<ViewResult>();
    }

    [Fact]
    public async Task Details_ProjectNotFound_ReturnsNotFound()
    {
        // Act
        var result = await _controller.Details(Guid.NewGuid());

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task Details_ProjectExists_ReturnsViewResult()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var projectDto = new ProjectDto(projectId, "Test", "Desc", DateTime.UtcNow, null, true, Guid.NewGuid());
        _projectServiceMock.Setup(x => x.GetProjectAsync(projectId)).ReturnsAsync(projectDto);
        _ticketServiceMock.Setup(x => x.GetFilteredTicketsAsync(null, null, null, null, projectId, null, null, null, null))
            .ReturnsAsync(new List<TicketDto>());

        // Act
        var result = await _controller.Details(projectId);

        // Assert
        result.Should().BeOfType<ViewResult>();
        var viewResult = result as ViewResult;
        ((ProjectDto)viewResult!.Model!).Id.Should().Be(projectId);
    }
}
