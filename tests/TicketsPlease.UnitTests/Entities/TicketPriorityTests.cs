// <copyright file="TicketPriorityTests.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.UnitTests.Entities;

using System;
using FluentAssertions;
using TicketsPlease.Domain.Entities;
using Xunit;

public class TicketPriorityTests
{
  [Fact]
  public void Properties_ShouldBeSettable()
  {
    // Arrange
    var priority = new TicketPriority
    {
      Name = "Critical",
      LevelWeight = 10,
      ColorHex = "#FF0000",
      TenantId = Guid.NewGuid()
    };

    // Assert
    priority.Name.Should().Be("Critical");
    priority.LevelWeight.Should().Be(10);
  }
}
