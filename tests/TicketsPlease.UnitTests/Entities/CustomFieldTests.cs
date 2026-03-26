// <copyright file="CustomFieldTests.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.UnitTests.Entities;

using System;
using FluentAssertions;
using TicketsPlease.Domain.Entities;
using Xunit;

public class CustomFieldTests
{
  [Fact]
  public void Definition_Properties_ShouldBeSettable()
  {
    // Arrange
    var def = new CustomFieldDefinition
    {
      Name = "Environment",
      FieldType = "String",
      TenantId = Guid.NewGuid()
    };

    // Assert
    def.Name.Should().Be("Environment");
  }

  [Fact]
  public void Value_Properties_ShouldBeSettable()
  {
    // Arrange
    var val = new TicketCustomValue
    {
      TicketId = Guid.NewGuid(),
      FieldDefinitionId = Guid.NewGuid(),
      Value = "Production",
      TenantId = Guid.NewGuid()
    };

    // Assert
    val.Value.Should().Be("Production");
  }
}
