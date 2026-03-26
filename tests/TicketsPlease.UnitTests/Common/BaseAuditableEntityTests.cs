// <copyright file="BaseAuditableEntityTests.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.UnitTests.Common;

using System;
using FluentAssertions;
using TicketsPlease.Domain.Common;
using Xunit;

public class BaseAuditableEntityTests
{
  [Fact]
  public void Constructor_ShouldInitializeAuditProperties()
  {
    // Act
    var entity = new TestAuditableEntity();

    // Assert
    entity.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    entity.CreatedBy.Should().BeNull();
    entity.UpdatedAt.Should().BeNull();
    entity.UpdatedBy.Should().BeNull();
  }

  [Fact]
  public void Properties_ShouldBeSettable()
  {
    // Arrange
    var entity = new TestAuditableEntity();
    var now = DateTime.UtcNow;
    var user = "TestUser";

    // Act
    entity.CreatedAt = now;
    entity.CreatedBy = user;
    entity.UpdatedAt = now;
    entity.UpdatedBy = user;

    // Assert
    entity.CreatedAt.Should().Be(now);
    entity.CreatedBy.Should().Be(user);
    entity.UpdatedAt.Should().Be(now);
    entity.UpdatedBy.Should().Be(user);
  }

  private class TestAuditableEntity : BaseAuditableEntity
  {
  }
}
