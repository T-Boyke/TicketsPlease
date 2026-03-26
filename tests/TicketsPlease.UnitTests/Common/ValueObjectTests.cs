// <copyright file="ValueObjectTests.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.UnitTests.Common;

using System.Collections.Generic;
using FluentAssertions;
using TicketsPlease.Domain.Common;
using Xunit;

public class ValueObjectTests
{
  [Fact]
  public void Equals_WithSameValues_ShouldReturnTrue()
  {
    // Arrange
    var vo1 = new TestValueObject("Test", 1);
    var vo2 = new TestValueObject("Test", 1);

    // Act & Assert
    vo1.Equals(vo2).Should().BeTrue();
    (vo1 == vo2).Should().BeTrue();
  }

  [Fact]
  public void Equals_WithDifferentValues_ShouldReturnFalse()
  {
    // Arrange
    var vo1 = new TestValueObject("Test1", 1);
    var vo2 = new TestValueObject("Test2", 1);

    // Act & Assert
    vo1.Equals(vo2).Should().BeFalse();
    (vo1 != vo2).Should().BeTrue();
  }

  [Fact]
  public void Equals_WithNull_ShouldReturnFalse()
  {
    // Arrange
    var vo1 = new TestValueObject("Test", 1);

    // Act & Assert
    vo1.Equals(null).Should().BeFalse();
  }

  [Fact]
  public void Equals_WithDifferentType_ShouldReturnFalse()
  {
    // Arrange
    var vo1 = new TestValueObject("Test", 1);
    var other = new object();

    // Act & Assert
    vo1.Equals(other).Should().BeFalse();
  }

  [Fact]
  public void GetHashCode_WithSameValues_ShouldReturnSameHashCode()
  {
    // Arrange
    var vo1 = new TestValueObject("Test", 1);
    var vo2 = new TestValueObject("Test", 1);

    // Act & Assert
    vo1.GetHashCode().Should().Be(vo2.GetHashCode());
  }

  [Fact]
  public void GetHashCode_WithDifferentValues_ShouldReturnDifferentHashCode()
  {
    // Arrange
    var vo1 = new TestValueObject("Test1", 1);
    var vo2 = new TestValueObject("Test2", 1);

    // Act & Assert
    vo1.GetHashCode().Should().NotBe(vo2.GetHashCode());
  }

  [Fact]
  public void GetHashCode_WithNullComponent_ShouldHandleNull()
  {
    // Arrange
    var vo1 = new TestValueObject(null!, 1);
    var vo2 = new TestValueObject(null!, 1);

    // Act & Assert
    vo1.GetHashCode().Should().Be(vo2.GetHashCode());
  }

  private class TestValueObject : ValueObject
  {
    public TestValueObject(string name, int value)
    {
      this.Name = name;
      this.Value = value;
    }

    public string Name { get; }

    public int Value { get; }

    public static bool operator ==(TestValueObject? a, TestValueObject? b)
    {
      if (ReferenceEquals(a, b))
      {
        return true;
      }

      if (a is null || b is null)
      {
        return false;
      }

      return a.Equals(b);
    }

    public static bool operator !=(TestValueObject? a, TestValueObject? b) => !(a == b);

    protected override IEnumerable<object?> GetEqualityComponents()
    {
      yield return this.Name;
      yield return this.Value;
    }
  }
}
