namespace TicketsPlease.UnitTests.Application.Services;

using FluentAssertions;
using Moq;
using TicketsPlease.Application.Common.Interfaces;
using TicketsPlease.Application.Services;
using TicketsPlease.Domain.Entities;
using Xunit;

public class TeamServiceTests
{
    private readonly Mock<ITeamRepository> _teamRepoMock;
    private readonly TeamService _teamService;

    public TeamServiceTests()
    {
        _teamRepoMock = new Mock<ITeamRepository>();
        _teamService = new TeamService(_teamRepoMock.Object);
    }

    [Fact]
    public async Task GetUserTeamsAsync_ShouldReturnMappedDtos()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var teams = new List<Team>
        {
            new Team { Id = Guid.NewGuid(), Name = "Team A", Description = "Desc A", ColorCode = "#111", CreatedAt = DateTime.UtcNow }
        };
        _teamRepoMock.Setup(r => r.GetTeamsByUserIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(teams);

        // Act
        var result = await _teamService.GetUserTeamsAsync(userId);

        // Assert
        result.Should().HaveCount(1);
        result.First().Name.Should().Be("Team A");
    }

    [Fact]
    public async Task GetTeamDetailsAsync_WhenExists_ShouldReturnDto()
    {
        // Arrange
        var teamId = Guid.NewGuid();
        var team = new Team { Id = teamId, Name = "Team A", CreatedAt = DateTime.UtcNow };
        _teamRepoMock.Setup(r => r.GetByIdAsync(teamId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(team);

        // Act
        var result = await _teamService.GetTeamDetailsAsync(teamId);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(teamId);
    }

    [Fact]
    public async Task GetAllTeamsAsync_ShouldReturnAllTeams()
    {
        // Arrange
        var teams = new List<Team>
        {
            new Team { Id = Guid.NewGuid(), Name = "Team A" },
            new Team { Id = Guid.NewGuid(), Name = "Team B" }
        };
        _teamRepoMock.Setup(r => r.GetAllTeamsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(teams);

        // Act
        var result = await _teamService.GetAllTeamsAsync();

        // Assert
        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task CreateTeamAsync_ShouldCallRepoAndReturnId()
    {
        // Arrange
        var name = "New Team";
        var desc = "Description";
        var color = "#fff";
        var creator = Guid.NewGuid();

        // Act
        var result = await _teamService.CreateTeamAsync(name, desc, color, creator);

        // Assert
        result.Should().NotBeEmpty();
        _teamRepoMock.Verify(r => r.AddAsync(It.Is<Team>(t => t.Name == name && t.CreatedByUserId == creator), It.IsAny<CancellationToken>()), Times.Once);
        _teamRepoMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task AddMemberAsync_WhenTeamExistsAndNotMember_ShouldAddMember()
    {
        // Arrange
        var teamId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var team = new Team { Id = teamId, Name = "Team A" };
        _teamRepoMock.Setup(r => r.GetByIdAsync(teamId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(team);

        // Act
        await _teamService.AddMemberAsync(teamId, userId);

        // Assert
        team.Members.Should().HaveCount(1);
        team.Members.First().UserId.Should().Be(userId);
        _teamRepoMock.Verify(r => r.UpdateAsync(team, It.IsAny<CancellationToken>()), Times.Once);
        _teamRepoMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task AddMemberAsync_WhenTeamNotFound_ShouldThrowException()
    {
        // Arrange
        var teamId = Guid.NewGuid();
        _teamRepoMock.Setup(r => r.GetByIdAsync(teamId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Team?)null);

        // Act
        var act = () => _teamService.AddMemberAsync(teamId, Guid.NewGuid());

        // Assert
        await act.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task RemoveMemberAsync_WhenMemberExists_ShouldRemoveMember()
    {
        // Arrange
        var teamId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var team = new Team { Id = teamId, Name = "Team A" };
        team.Members.Add(new TeamMember { UserId = userId });
        _teamRepoMock.Setup(r => r.GetByIdAsync(teamId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(team);

        // Act
        await _teamService.RemoveMemberAsync(teamId, userId);

        // Assert
        team.Members.Should().BeEmpty();
        _teamRepoMock.Verify(r => r.UpdateAsync(team, It.IsAny<CancellationToken>()), Times.Once);
        _teamRepoMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteTeamAsync_WhenTeamExists_ShouldDelete()
    {
        // Arrange
        var teamId = Guid.NewGuid();
        var team = new Team { Id = teamId, Name = "Team A" };
        _teamRepoMock.Setup(r => r.GetByIdAsync(teamId, default)).ReturnsAsync(team);

        // Act
        await _teamService.DeleteTeamAsync(teamId);

        // Assert
        _teamRepoMock.Verify(r => r.DeleteAsync(team), Times.Once);
        _teamRepoMock.Verify(r => r.SaveChangesAsync(default), Times.Once);
    }
}
