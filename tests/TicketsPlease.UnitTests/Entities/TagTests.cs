// <copyright file="TagTests.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.UnitTests.Entities;

using System;
using FluentAssertions;
using TicketsPlease.Domain.Entities;
using Xunit;

public class TagTests
{
  [Fact]
  public void Properties_ShouldBeSettable()
  {
    // Arrange
    var tag = new Tag { Name = "Bug", ColorHex = "#FF0000", TenantId = Guid.NewGuid() };

    // Assert
    tag.Name.Should().Be("Bug");
    tag.ColorHex.Should().Be("#FF0000");
    tag.TenantId.Should().NotBeEmpty();
  }
}
