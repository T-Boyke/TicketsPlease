// <copyright file="WorkflowTests.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.UnitTests.Entities;

using System;
using FluentAssertions;
using TicketsPlease.Domain.Entities;
using Xunit;

public class WorkflowTests
{
  [Fact]
  public void Workflow_Properties_ShouldBeSettable()
  {
    // Arrange
    var workflow = new Workflow { Name = "Kanban", TenantId = Guid.NewGuid() };

    // Assert
    workflow.Name.Should().Be("Kanban");
    workflow.States.Should().BeEmpty();
  }

  [Fact]
  public void State_Properties_ShouldBeSettable()
  {
    // Arrange
    var state = new WorkflowState
    {
      Name = "Done",
      OrderIndex = 10,
      IsTerminalState = true,
      WorkflowId = Guid.NewGuid(),
      TenantId = Guid.NewGuid()
    };

    // Assert
    state.IsTerminalState.Should().BeTrue();
  }

  [Fact]
  public void Transition_Properties_ShouldBeSettable()
  {
    // Arrange
    var transition = new WorkflowTransition
    {
      FromStateId = Guid.NewGuid(),
      ToStateId = Guid.NewGuid(),
      TenantId = Guid.NewGuid()
    };

    // Assert
    transition.FromStateId.Should().NotBeEmpty();
  }
}
