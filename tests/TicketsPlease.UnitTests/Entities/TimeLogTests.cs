// <copyright file="TimeLogTests.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.UnitTests.Entities;

using System;
using FluentAssertions;
using TicketsPlease.Domain.Entities;
using Xunit;

public class TimeLogTests
{
  [Fact]
  public void Properties_ShouldBeSettable()
  {
    // Arrange
    var log = new TimeLog
    {
      TicketId = Guid.NewGuid(),
      UserId = Guid.NewGuid(),
      HoursLogged = 2.5m,
      StartedAt = DateTime.UtcNow,
      TenantId = Guid.NewGuid()
    };

    // Assert
    log.HoursLogged.Should().Be(2.5m);
    log.StartedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
  }
}
