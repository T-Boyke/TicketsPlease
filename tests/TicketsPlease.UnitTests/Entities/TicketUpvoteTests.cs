// <copyright file="TicketUpvoteTests.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.UnitTests.Entities;

using System;
using FluentAssertions;
using TicketsPlease.Domain.Entities;
using Xunit;

public class TicketUpvoteTests
{
  [Fact]
  public void Properties_ShouldBeSettable()
  {
    // Arrange
    var upvote = new TicketUpvote
    {
      TicketId = Guid.NewGuid(),
      UserId = Guid.NewGuid(),
      VotedAt = DateTime.UtcNow,
      TenantId = Guid.NewGuid()
    };

    // Assert
    upvote.TicketId.Should().NotBeEmpty();
    upvote.VotedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
  }
}
