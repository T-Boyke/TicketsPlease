// <copyright file="SubTicketTests.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.UnitTests.Entities;

using System;
using FluentAssertions;
using TicketsPlease.Domain.Entities;
using Xunit;

public class SubTicketTests
{
  [Fact]
  public void Properties_ShouldBeSettable()
  {
    // Arrange
    var sub = new SubTicket
    {
      TicketId = Guid.NewGuid(),
      Title = "Checklist item",
      IsCompleted = false,
      TenantId = Guid.NewGuid(),
      CreatorId = Guid.NewGuid()
    };

    // Act
    sub.IsCompleted = true;

    // Assert
    sub.Title.Should().Be("Checklist item");
    sub.IsCompleted.Should().BeTrue();
  }
}
