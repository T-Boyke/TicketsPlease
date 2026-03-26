// <copyright file="TicketLinkTests.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.UnitTests.Entities;

using System;
using FluentAssertions;
using TicketsPlease.Domain.Entities;
using TicketsPlease.Domain.Enums;
using Xunit;

public class TicketLinkTests
{
  [Fact]
  public void Constructor_WithValidData_ShouldInitializeProperties()
  {
    // Arrange
    var sourceId = Guid.NewGuid();
    var targetId = Guid.NewGuid();
    var type = TicketLinkType.Blocks;

    // Act
    var link = new TicketLink(sourceId, targetId, type);

    // Assert
    link.SourceTicketId.Should().Be(sourceId);
    link.TargetTicketId.Should().Be(targetId);
    link.LinkType.Should().Be(type);
  }
}
