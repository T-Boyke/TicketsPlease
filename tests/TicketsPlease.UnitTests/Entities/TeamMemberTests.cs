// <copyright file="TeamMemberTests.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.UnitTests.Entities;

using System;
using FluentAssertions;
using TicketsPlease.Domain.Entities;
using Xunit;

public class TeamMemberTests
{
  [Fact]
  public void Properties_ShouldBeSettable()
  {
    // Arrange
    var member = new TeamMember
    {
      TeamId = Guid.NewGuid(),
      UserId = Guid.NewGuid(),
      TenantId = Guid.NewGuid(),
      JoinedAt = DateTime.UtcNow
    };

    // Assert
    member.TeamId.Should().NotBeEmpty();
    member.JoinedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
  }
}
