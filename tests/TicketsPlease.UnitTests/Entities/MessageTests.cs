// <copyright file="MessageTests.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.UnitTests.Entities;

using System;
using FluentAssertions;
using TicketsPlease.Domain.Entities;
using Xunit;

public class MessageTests
{
  [Fact]
  public void Properties_ShouldBeSettable()
  {
    // Arrange
    var msg = new Message
    {
      SenderUserId = Guid.NewGuid(),
      ReceiverUserId = Guid.NewGuid(),
      BodyMarkdown = "Hello!",
      SentAt = DateTime.UtcNow,
      TenantId = Guid.NewGuid()
    };

    // Assert
    msg.BodyMarkdown.Should().Be("Hello!");
    msg.SentAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
  }
}
