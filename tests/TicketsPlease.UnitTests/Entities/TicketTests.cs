// <copyright file="TicketTests.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.UnitTests.Entities;

using System;
using FluentAssertions;
using TicketsPlease.Domain.Entities;
using TicketsPlease.Domain.Enums;
using Xunit;

public class TicketTests
{
  [Fact]
  public void Constructor_WithValidData_ShouldInitializeProperties()
  {
    // Arrange
    var title = "Test Ticket";
    var type = TicketType.Task;
    var projectId = Guid.NewGuid();
    var creatorId = Guid.NewGuid();
    var workflowStateId = Guid.NewGuid();
    var geoIp = "127.0.0.1";

    // Act
    var ticket = new Ticket(title, type, projectId, creatorId, workflowStateId, geoIp);

    // Assert
    ticket.Title.Should().Be(title);
    ticket.Type.Should().Be(type);
    ticket.ProjectId.Should().Be(projectId);
    ticket.CreatorId.Should().Be(creatorId);
    ticket.WorkflowStateId.Should().Be(workflowStateId);
    ticket.GeoIpTimestamp.Should().Be(geoIp);
    ticket.Status.Should().Be("Todo");
    ticket.DomainHash.Should().NotBeEmpty();
    ticket.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
  }

  [Theory]
  [InlineData(null)]
  [InlineData("")]
  [InlineData(" ")]
  public void Constructor_WithInvalidTitle_ShouldThrowException(string? title)
  {
    // Arrange
    var type = TicketType.Task;
    var projectId = Guid.NewGuid();
    var creatorId = Guid.NewGuid();
    var workflowStateId = Guid.NewGuid();
    var geoIp = "127.0.0.1";

    // Act
    Action act = () => _ = new Ticket(title!, type, projectId, creatorId, workflowStateId, geoIp);

    // Assert
    act.Should().Throw<ArgumentException>().WithMessage("*Titel darf nicht leer sein.*");
  }

  [Fact]
  public void UpdateTitle_WithValidTitle_ShouldUpdateTitleAndTimestamp()
  {
    // Arrange
    var ticket = CreateTicket();
    var newTitle = "Updated Title";

    // Act
    ticket.UpdateTitle(newTitle);

    // Assert
    ticket.Title.Should().Be(newTitle);
    ticket.UpdatedAt.Should().NotBeNull();
  }

  [Fact]
  public void UpdateDescription_ShouldUpdateDetails()
  {
    // Arrange
    var ticket = CreateTicket();
    var desc = "Short desc";
    var md = "# Long desc";

    // Act
    ticket.UpdateDescription(desc, md);

    // Assert
    ticket.Description.Should().Be(desc);
    ticket.DescriptionMarkdown.Should().Be(md);
    ticket.UpdatedAt.Should().NotBeNull();
  }

  [Fact]
  public void MoveToState_ShouldUpdateWorkflowStateId()
  {
    // Arrange
    var ticket = CreateTicket();
    var newStateId = Guid.NewGuid();

    // Act
    ticket.MoveToState(newStateId);

    // Assert
    ticket.WorkflowStateId.Should().Be(newStateId);
    ticket.UpdatedAt.Should().NotBeNull();
  }

  [Fact]
  public void AssignUser_ShouldUpdateAssignedUserId()
  {
    // Arrange
    var ticket = CreateTicket();
    var userId = Guid.NewGuid();

    // Act
    ticket.AssignUser(userId);

    // Assert
    ticket.AssignedUserId.Should().Be(userId);
    ticket.UpdatedAt.Should().NotBeNull();
  }

  [Fact]
  public void Close_ByAuthorizedUser_ShouldSetCloseStatus()
  {
    // Arrange
    var creatorId = Guid.NewGuid();
    var ticket = CreateTicket(creatorId: creatorId);

    // Act
    ticket.Close(creatorId, false);

    // Assert
    ticket.Status.Should().Be("Closed");
    ticket.ClosedAt.Should().NotBeNull();
    ticket.ClosedByUserId.Should().Be(creatorId);
  }

  [Fact]
  public void Close_ByUnauthorizedUser_ShouldThrowException()
  {
    // Arrange
    var ticket = CreateTicket();
    var unauthorizedUserId = Guid.NewGuid();

    // Act
    Action act = () => ticket.Close(unauthorizedUserId, false);

    // Assert
    act.Should().Throw<InvalidOperationException>();
  }

  [Fact]
  public void Close_ByAdmin_ShouldSucceedRegardlessOfOwnership()
  {
    // Arrange
    var ticket = CreateTicket();
    var adminId = Guid.NewGuid();

    // Act
    ticket.Close(adminId, true);

    // Assert
    ticket.Status.Should().Be("Closed");
  }

  [Theory]
  [InlineData(0)]
  [InlineData(6)]
  public void SetDifficulty_WithInvalidValue_ShouldThrowException(int difficulty)
  {
    // Arrange
    var ticket = CreateTicket();

    // Act
    Action act = () => ticket.SetDifficulty(difficulty);

    // Assert
    act.Should().Throw<ArgumentOutOfRangeException>();
  }

  [Fact]
  public void SetDifficulty_WithValidValue_ShouldUpdateValue()
  {
    // Arrange
    var ticket = CreateTicket();
    var difficulty = 3;

    // Act
    ticket.SetDifficulty(difficulty);

    // Assert
    ticket.ChilliesDifficulty.Should().Be(difficulty);
  }

  [Fact]
  public void SetParent_WithTicketId_ShouldThrowException()
  {
    // Arrange
    var ticket = CreateTicket();

    // Act
    Action act = () => ticket.SetParent(ticket.Id);

    // Assert
    act.Should().Throw<InvalidOperationException>();
  }

  [Fact]
  public void GetProgressPercentage_WithNoSubTickets_ShouldReturn0or100()
  {
    // Arrange
    var ticket = CreateTicket();

    // Act & Assert
    ticket.GetProgressPercentage().Should().Be(0);

    var adminId = Guid.NewGuid();
    ticket.Close(adminId, true);
    ticket.GetProgressPercentage().Should().Be(100);
  }

  private static Ticket CreateTicket(Guid? creatorId = null)
  {
    return new Ticket(
        "Test",
        TicketType.Task,
        Guid.NewGuid(),
        creatorId ?? Guid.NewGuid(),
        Guid.NewGuid(),
        "127.0.0.1");
  }
}
