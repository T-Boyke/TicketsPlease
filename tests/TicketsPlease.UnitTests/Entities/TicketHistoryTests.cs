// <copyright file="TicketHistoryTests.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.UnitTests.Entities;

using System;
using FluentAssertions;
using TicketsPlease.Domain.Entities;
using Xunit;

public class TicketHistoryTests
{
  [Fact]
  public void Properties_ShouldBeSettable()
  {
    // Arrange
    var history = new TicketHistory
    {
      TicketId = Guid.NewGuid(),
      ActorUserId = Guid.NewGuid(),
      FieldName = "Status",
      OldValue = "Todo",
      NewValue = "Done",
      ChangedAt = DateTime.UtcNow,
      TenantId = Guid.NewGuid()
    };

    // Assert
    history.FieldName.Should().Be("Status");
    history.NewValue.Should().Be("Done");
  }
}
