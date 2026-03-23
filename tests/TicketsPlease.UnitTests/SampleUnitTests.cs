// <copyright file="SampleUnitTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
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
