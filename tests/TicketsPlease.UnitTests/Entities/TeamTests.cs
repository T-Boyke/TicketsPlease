// <copyright file="TeamTests.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.UnitTests.Entities;

using System;
using FluentAssertions;
using TicketsPlease.Domain.Entities;
using Xunit;

public class TeamTests
{
  [Fact]
  public void Properties_ShouldBeSettable()
  {
    // Arrange
    var team = new Team
    {
      Name = "Dev Team",
      Description = "Main developers",
      TenantId = Guid.NewGuid(),
      CreatedByUserId = Guid.NewGuid()
    };

    // Assert
    team.Name.Should().Be("Dev Team");
    team.Members.Should().BeEmpty();
  }
}
