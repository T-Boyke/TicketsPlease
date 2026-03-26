// <copyright file="OrganizationTests.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.UnitTests.Entities;

using System;
using FluentAssertions;
using TicketsPlease.Domain.Entities;
using Xunit;

public class OrganizationTests
{
  [Fact]
  public void Properties_ShouldBeSettable()
  {
    // Arrange
    var org = new Organization { Name = "BitLC", TenantId = Guid.NewGuid() };

    // Assert
    org.Name.Should().Be("BitLC");
    org.TenantId.Should().NotBeEmpty();
  }
}
