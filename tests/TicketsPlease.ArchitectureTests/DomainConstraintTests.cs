// <copyright file="DomainConstraintTests.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.ArchitectureTests;

using FluentAssertions;
using NetArchTest.Rules;
using TicketsPlease.Domain.Common;
using TicketsPlease.Domain.Entities;

/// <summary>
/// Enthält Architektur-Tests zur Sicherstellung der Datenintegrität und Einhaltung von Domain-Vorgaben.
/// Nutzt NetArchTest zur statischen Analyse der Assembly-Struktur.
/// </summary>
#pragma warning disable CA1707 // Identifiers should not contain underscores
public class DomainConstraintTests
{
  /// <summary>
  /// Prüft, ob alle Entitäten im Domain-Layer von der Klasse <see cref="BaseEntity"/> erben.
  /// Dies stellt sicher, dass grundlegende Felder wie Id und RowVersion überall vorhanden sind.
  /// </summary>
  [Fact]
  public void Entities_Should_Inherit_From_BaseEntity()
  {
    var result = Types.InAssembly(typeof(User).Assembly)
        .That()
        .ResideInNamespace("TicketsPlease.Domain.Entities")
        .And()
        .AreClasses()
        .And()
        .DoNotHaveName("User")
        .And()
        .DoNotHaveName("Role")
        .Should()
        .Inherit(typeof(BaseEntity))
        .GetResult();

    var failureMessage = "alle Entitäten müssen von BaseEntity erben. Fehlend: " +
                         (result.FailingTypeNames != null ? string.Join(", ", result.FailingTypeNames) : "keine");
    result.IsSuccessful.Should().BeTrue(failureMessage);
  }

  /// <summary>
  /// Prüft, ob alle Entitäten im Domain-Namespace liegen.
  /// </summary>
  [Fact]
  public void Entities_Should_Reside_In_Entities_Namespace()
  {
    var result = Types.InAssembly(typeof(User).Assembly)
        .That()
        .Inherit(typeof(BaseEntity))
        .And()
        .AreNotAbstract()
        .Should()
        .ResideInNamespace("TicketsPlease.Domain.Entities")
        .GetResult();

    result.IsSuccessful.Should().BeTrue("Entitäten sollten in einem spezifischen Namespace gruppiert sein.");
  }
}
#pragma warning restore CA1707 // Identifiers should not contain underscores
