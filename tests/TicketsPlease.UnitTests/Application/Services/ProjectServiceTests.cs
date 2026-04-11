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

public class ProjectServiceTests
{
    private readonly Mock<IProjectRepository> _projectRepoMock;
    private readonly Mock<UserManager<User>> _userManagerMock;
    private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
    private readonly ProjectService _service;

    public ProjectServiceTests()
    {
        _projectRepoMock = new Mock<IProjectRepository>();

        var store = new Mock<IUserStore<User>>();
        _userManagerMock = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);

        _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        var httpContext = new DefaultHttpContext();
        httpContext.User = new ClaimsPrincipal(new ClaimsIdentity());
        _httpContextAccessorMock.Setup(a => a.HttpContext).Returns(httpContext);

        _service = new ProjectService(_projectRepoMock.Object, _userManagerMock.Object, _httpContextAccessorMock.Object);
    }

    private void SetupCurrentUser(Guid tenantId)
    {
        var user = new User { Id = Guid.NewGuid(), TenantId = tenantId };
        _userManagerMock.Setup(m => m.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);
    }

    [Fact]
    public async Task GetProjectsAsync_ShouldReturnMappedDtosForTenant()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        SetupCurrentUser(tenantId);
        var projects = new List<Project> { new Project("P1", DateTime.UtcNow) { TenantId = tenantId } };
        _projectRepoMock.Setup(r => r.GetAllAsync(tenantId)).ReturnsAsync(projects);

        // Act
        var result = await _service.GetProjectsAsync();

        // Assert
        result.Should().HaveCount(1);
        result.First().Title.Should().Be("P1");
    }

    [Fact]
    public async Task GetProjectAsync_WhenExists_ShouldReturnDto()
    {
        // Arrange
        var id = Guid.NewGuid();
        var project = new Project("P1", DateTime.UtcNow) { Id = id };
        _projectRepoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(project);

        // Act
        var result = await _service.GetProjectAsync(id);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(id);
    }

    [Fact]
    public async Task CreateProjectAsync_ShouldSetTenantAndAdd()
    {
        // Arrange
        var tenantId = Guid.NewGuid();
        SetupCurrentUser(tenantId);
        var dto = new CreateProjectDto("New Project", "Desc", DateTime.UtcNow, null);

        // Act
        await _service.CreateProjectAsync(dto);

        // Assert
        _projectRepoMock.Verify(r => r.AddAsync(It.Is<Project>(p => p.Title == "New Project" && p.TenantId == tenantId)), Times.Once);
    }

    [Fact]
    public async Task UpdateProjectAsync_WhenExists_ShouldUpdateAndCallUpdateRepo()
    {
        // Arrange
        var id = Guid.NewGuid();
        var project = new Project("Old Title", DateTime.UtcNow) { Id = id };
        _projectRepoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(project);
        var dto = new UpdateProjectDto(id, "New Title", "New Desc", DateTime.UtcNow, DateTime.UtcNow.AddDays(10), true);

        // Act
        await _service.UpdateProjectAsync(dto);

        // Assert
        project.Title.Should().Be("New Title");
        project.IsOpen.Should().BeTrue();
        _projectRepoMock.Verify(r => r.UpdateAsync(project), Times.Once);
    }

    [Fact]
    public async Task UpdateProjectAsync_WhenNotFound_ShouldThrowException()
    {
        // Arrange
        var id = Guid.NewGuid();
        _projectRepoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((Project?)null);
        var dto = new UpdateProjectDto(id, "T", "D", DateTime.UtcNow, null, true);

        // Act
        var act = () => _service.UpdateProjectAsync(dto);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>();
    }

    [Fact]
    public async Task DeleteProjectAsync_WhenExists_ShouldDelete()
    {
        // Arrange
        var id = Guid.NewGuid();
        var project = new Project("P", DateTime.UtcNow) { Id = id };
        _projectRepoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(project);

        // Act
        await _service.DeleteProjectAsync(id);

        // Assert
        _projectRepoMock.Verify(r => r.DeleteAsync(project), Times.Once);
    }
}
