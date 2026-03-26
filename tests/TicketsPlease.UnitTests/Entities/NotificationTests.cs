// <copyright file="NotificationTests.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.UnitTests.Entities;

using System;
using FluentAssertions;
using TicketsPlease.Domain.Entities;
using Xunit;

public class NotificationTests
{
  [Fact]
  public void Properties_ShouldBeSettable()
  {
    // Arrange
    var notification = new Notification
    {
      UserId = Guid.NewGuid(),
      Title = "New Ticket",
      Content = "A new ticket has been assigned to you.",
      IsRead = false,
      TenantId = Guid.NewGuid()
    };

    // Act
    notification.IsRead = true;

    // Assert
    notification.Title.Should().Be("New Ticket");
    notification.IsRead.Should().BeTrue();
  }
}
