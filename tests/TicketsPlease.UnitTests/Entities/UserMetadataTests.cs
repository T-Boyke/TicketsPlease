// <copyright file="UserMetadataTests.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.UnitTests.Entities;

using System;
using FluentAssertions;
using TicketsPlease.Domain.Entities;
using Xunit;

public class UserMetadataTests
{
  [Fact]
  public void UserAddress_Properties_ShouldBeSettable()
  {
    // Arrange
    var addr = new UserAddress
    {
      Street = "Teststr. 1",
      City = "Bielefeld",
      ZipCode = "33602",
      Country = "Germany",
      UserId = Guid.NewGuid()
    };

    // Assert
    addr.Street.Should().Be("Teststr. 1");
    addr.City.Should().Be("Bielefeld");
  }

  [Fact]
  public void UserProfile_Properties_ShouldBeSettable()
  {
    // Arrange
    var profile = new UserProfile
    {
      FirstName = "Max",
      LastName = "Mustermann",
      Bio = "Test bio",
      UserId = Guid.NewGuid()
    };

    // Assert
    profile.FirstName.Should().Be("Max");
    profile.FullName.Should().Be("Max Mustermann");
  }
}
