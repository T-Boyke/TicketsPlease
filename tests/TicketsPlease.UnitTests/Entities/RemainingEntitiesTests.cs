// <copyright file="RemainingEntitiesTests.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.UnitTests.Entities;

using System;
using FluentAssertions;
using TicketsPlease.Domain.Entities;
using Xunit;

public class RemainingEntitiesTests
{
  [Fact]
  public void Role_Properties_ShouldBeSettable()
  {
    // Arrange
    var role = new Role { Name = "Admin", NormalizedName = "ADMIN" };

    // Assert
    role.Name.Should().Be("Admin");
  }

  [Fact]
  public void MessageReadReceipt_Properties_ShouldBeSettable()
  {
    // Arrange
    var receipt = new MessageReadReceipt
    {
      MessageId = Guid.NewGuid(),
      UserId = Guid.NewGuid(),
      ReadAt = DateTime.UtcNow,
      TenantId = Guid.NewGuid()
    };

    // Assert
    receipt.ReadAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
  }

  [Fact]
  public void TicketTag_Properties_ShouldBeSettable()
  {
    // Arrange
    var tt = new TicketTag
    {
      TicketId = Guid.NewGuid(),
      TagId = Guid.NewGuid(),
      TenantId = Guid.NewGuid()
    };

    // Assert
    tt.TicketId.Should().NotBeEmpty();
  }

  [Fact]
  public void TicketAssignment_Properties_ShouldBeSettable()
  {
    // Arrange
    var ta = new TicketAssignment
    {
      TicketId = Guid.NewGuid(),
      UserId = Guid.NewGuid(),
      AssignedAt = DateTime.UtcNow,
      TenantId = Guid.NewGuid()
    };

    // Assert
    ta.AssignedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
  }

  [Fact]
  public void TicketTemplate_Properties_ShouldBeSettable()
  {
    // Arrange
    var template = new TicketTemplate
    {
      Name = "Bug Template",
      DescriptionMarkdownTemplate = "Bug content",
      TenantId = Guid.NewGuid()
    };

    // Assert
    template.DescriptionMarkdownTemplate.Should().Be("Bug content");
  }
}
