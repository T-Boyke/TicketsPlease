// <copyright file="SampleUnitTests.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.UnitTests;

using FluentAssertions;
using Xunit;

/// <summary>
/// Eine Beispiel-Testklasse zur Validierung der Testumgebung.
/// </summary>
public class SampleUnitTests
{
  /// <summary>
  /// Ein einfacher Test, der sicherstellt, dass FluentAssertions funktioniert.
  /// </summary>
  [Fact]
  public void ShoudWork()
  {
    true.Should().BeTrue();
  }
}
