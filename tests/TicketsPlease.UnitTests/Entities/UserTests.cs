// <copyright file="UserTests.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.UnitTests.Entities;

using System;
using FluentAssertions;
using TicketsPlease.Domain.Entities;
using Xunit;

public class UserTests
{
  [Fact]
  public void Constructor_ShouldInitializeProperties()
  {
    // Act
    var user = new User();

    // Assert
    user.Id.Should().BeEmpty();
    user.IsActive.Should().BeTrue();
  }

  [Fact]
  public void Properties_ShouldBeSettable()
  {
    // Arrange
    var user = new User();
    var roleId = Guid.NewGuid();
    var tenantId = Guid.NewGuid();
    var profile = new UserProfile();

    // Act
    user.RoleId = roleId;
    user.TenantId = tenantId;
    user.Profile = profile;
    user.IsActive = false;

    // Assert
    user.RoleId.Should().Be(roleId);
    user.TenantId.Should().Be(tenantId);
    user.Profile.Should().Be(profile);
    user.IsActive.Should().BeFalse();
  }
}
