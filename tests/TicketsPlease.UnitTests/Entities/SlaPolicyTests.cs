// <copyright file="SlaPolicyTests.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.UnitTests.Entities;

using System;
using FluentAssertions;
using TicketsPlease.Domain.Entities;
using Xunit;

public class SlaPolicyTests
{
  [Fact]
  public void Properties_ShouldBeSettable()
  {
    // Arrange
    var policy = new SlaPolicy
    {
      PriorityId = Guid.NewGuid(),
      ResponseTimeHours = 4,
      ResolutionTimeHours = 24,
      TenantId = Guid.NewGuid()
    };

    // Assert
    policy.PriorityId.Should().NotBeEmpty();
  }
}
