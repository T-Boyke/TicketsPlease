namespace TicketsPlease.UnitTests.Application.Services;

using FluentAssertions;
using Moq;
using TicketsPlease.Application.Common.Dtos;
using TicketsPlease.Application.Common.Interfaces;
using TicketsPlease.Application.Services;
using TicketsPlease.Domain.Entities;
using Xunit;

public class OrganizationServiceTests
{
    private readonly Mock<IOrganizationRepository> _repositoryMock;
    private readonly OrganizationService _service;

    public OrganizationServiceTests()
    {
        _repositoryMock = new Mock<IOrganizationRepository>();
        _service = new OrganizationService(_repositoryMock.Object);
    }

    [Fact]
    public async Task GetOrganizationsAsync_ShouldReturnMappedDtos()
    {
        // Arrange
        var orgs = new List<Organization>
        {
            new Organization { Id = Guid.NewGuid(), Name = "Org 1", SubscriptionLevel = "Basic", IsActive = true },
            new Organization { Id = Guid.NewGuid(), Name = "Org 2", SubscriptionLevel = "Pro", IsActive = false }
        };
        _repositoryMock.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(orgs);

        // Act
        var result = await _service.GetOrganizationsAsync();

        // Assert
        result.Should().HaveCount(2);
        result[0].Name.Should().Be("Org 1");
        result[1].IsActive.Should().BeFalse();
    }

    [Fact]
    public async Task GetOrganizationByIdAsync_WhenExists_ShouldReturnDto()
    {
        // Arrange
        var id = Guid.NewGuid();
        var org = new Organization { Id = id, Name = "Org 1" };
        _repositoryMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(org);

        // Act
        var result = await _service.GetOrganizationByIdAsync(id);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(id);
    }

    [Fact]
    public async Task GetOrganizationByIdAsync_WhenNotExists_ShouldReturnNull()
    {
        // Arrange
        var id = Guid.NewGuid();
        _repositoryMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync((Organization?)null);

        // Act
        var result = await _service.GetOrganizationByIdAsync(id);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task CreateOrganizationAsync_ShouldAddAndSave()
    {
        // Arrange
        var dto = new UpsertOrganizationDto("New Org", "Basic", true);

        // Act
        var result = await _service.CreateOrganizationAsync(dto);

        // Assert
        result.Name.Should().Be("New Org");
        _repositoryMock.Verify(r => r.AddAsync(It.Is<Organization>(o => o.Name == "New Org"), It.IsAny<CancellationToken>()), Times.Once);
        _repositoryMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateOrganizationAsync_WhenExists_ShouldUpdateAndSave()
    {
        // Arrange
        var id = Guid.NewGuid();
        var org = new Organization { Id = id, Name = "Old Name" };
        var dto = new UpsertOrganizationDto("New Name", "Pro", false);
        _repositoryMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>())).ReturnsAsync(org);

        // Act
        await _service.UpdateOrganizationAsync(id, dto);

        // Assert
        org.Name.Should().Be("New Name");
        org.SubscriptionLevel.Should().Be("Pro");
        org.IsActive.Should().BeFalse();
        _repositoryMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
