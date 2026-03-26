// <copyright file="ProjectTests.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.UnitTests.Entities;

using System;
using FluentAssertions;
using TicketsPlease.Domain.Entities;
using Xunit;

public class ProjectTests
{
  [Fact]
  public void Constructor_WithValidData_ShouldInitializeProperties()
  {
    // Arrange
    var title = "Test Project";
    var start = DateTime.UtcNow;

    // Act
    var project = new Project(title, start);

    // Assert
    project.Title.Should().Be(title);
    project.StartDate.Should().Be(start);
    project.IsOpen.Should().BeTrue();
    project.Tickets.Should().BeEmpty();
  }

  [Theory]
  [InlineData(null)]
  [InlineData("")]
  [InlineData(" ")]
  public void Constructor_WithInvalidTitle_ShouldThrowException(string? title)
  {
    // Act
    Action act = () => _ = new Project(title!, DateTime.UtcNow);

    // Assert
    act.Should().Throw<ArgumentException>();
  }

  [Fact]
  public void UpdateMetadata_WithValidData_ShouldUpdateProperties()
  {
    // Arrange
    var project = new Project("Initial", DateTime.UtcNow);
    var newTitle = "Updated";
    var newDesc = "Desc";

    // Act
    project.UpdateMetadata(newTitle, newDesc);

    // Assert
    project.Title.Should().Be(newTitle);
    project.Description.Should().Be(newDesc);
  }

  [Fact]
  public void Close_ShouldSetIsOpenToFalse()
  {
    // Arrange
    var project = new Project("Test", DateTime.UtcNow);

    // Act
    project.Close();

    // Assert
    project.IsOpen.Should().BeFalse();
    project.EndDate.Should().NotBeNull();
  }

  [Fact]
  public void Open_ShouldSetIsOpenToTrue()
  {
    // Arrange
    var project = new Project("Test", DateTime.UtcNow);
    project.Close();

    // Act
    project.Open();

    // Assert
    project.IsOpen.Should().BeTrue();
    project.EndDate.Should().BeNull();
  }

  [Fact]
  public void AssignWorkflow_ShouldUpdateWorkflowId()
  {
    // Arrange
    var project = new Project("Test", DateTime.UtcNow);
    var workflowId = Guid.NewGuid();

    // Act
    project.AssignWorkflow(workflowId);

    // Assert
    project.WorkflowId.Should().Be(workflowId);
  }

  [Fact]
  public void SetTenantId_ShouldUpdateTenantId()
  {
    // Arrange
    var project = new Project("Test", DateTime.UtcNow);
    var tenantId = Guid.NewGuid();

    // Act
    project.SetTenantId(tenantId);

    // Assert
    project.TenantId.Should().Be(tenantId);
  }

  [Fact]
  public void SetEndDate_ShouldUpdateEndDate()
  {
    // Arrange
    var project = new Project("Test", DateTime.UtcNow);
    var endDate = DateTime.UtcNow.AddDays(10);

    // Act
    project.SetEndDate(endDate);

    // Assert
    project.EndDate.Should().Be(endDate);
  }
}
