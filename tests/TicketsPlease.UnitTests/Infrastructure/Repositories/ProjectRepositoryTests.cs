using FluentAssertions;
using TicketsPlease.Domain.Entities;
using TicketsPlease.Infrastructure.Repositories;
using Xunit;

namespace TicketsPlease.UnitTests.Infrastructure.Repositories;

public class ProjectRepositoryTests : InfrastructureTestBase
{
    private readonly ProjectRepository _repository;

    public ProjectRepositoryTests()
    {
        _repository = new ProjectRepository(Context);
    }

    [Fact]
    public async Task GetByIdAsync_WhenExists_ShouldReturnProjectWithTickets()
    {
        // Arrange
        var project = new Project("Test Project", DateTime.UtcNow);
        Context.Projects.Add(project);
        await Context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByIdAsync(project.Id);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(project.Id);
        result.Tickets.Should().BeEmpty();
    }

    [Fact]
    public async Task GetAllAsync_ShouldFilterByTenant()
    {
        // Arrange
        var tenant1 = Guid.Empty; // Needs to match default filter
        var tenant2 = Guid.NewGuid();

        var p1 = new Project("P1", DateTime.UtcNow);
        p1.SetTenantId(tenant1);

        var p2 = new Project("P2", DateTime.UtcNow);
        p2.SetTenantId(tenant2);

        Context.Projects.AddRange(p1, p2);
        await Context.SaveChangesAsync();

        // Act
        var result = await _repository.GetAllAsync(tenant1);

        // Assert
        result.Should().HaveCount(1);
        result.First().Title.Should().Be("P1");
    }

    [Fact]
    public async Task CRUD_Operations_ShouldWork()
    {
        // Add
        var project = new Project("New", DateTime.UtcNow);
        await _repository.AddAsync(project);

        var inDb = await Context.Projects.FindAsync(project.Id);
        inDb.Should().NotBeNull();

        // Update
        project.UpdateMetadata("Updated", "Desc");
        await _repository.UpdateAsync(project);

        var updated = await Context.Projects.FindAsync(project.Id);
        updated!.Title.Should().Be("Updated");

        // Delete
        await _repository.DeleteAsync(project);
        var deleted = await Context.Projects.FindAsync(project.Id);
        deleted.Should().BeNull();
    }
}
