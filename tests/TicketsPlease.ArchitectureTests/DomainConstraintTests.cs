using FluentAssertions;
using NetArchTest.Rules;
using TicketsPlease.Domain.Common;
using TicketsPlease.Domain.Entities;
using Xunit;

namespace TicketsPlease.ArchitectureTests;

/// <summary>
/// Enthält Architektur-Tests zur Sicherstellung der Datenintegrität und Einhaltung von Domain-Vorgaben.
/// Nutzt NetArchTest zur statischen Analyse der Assembly-Struktur.
/// </summary>
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
            .Should()
            .Inherit(typeof(BaseEntity))
            .GetResult();

        result.IsSuccessful.Should().BeTrue("alle Entitäten müssen von BaseEntity erben, um Konsistenz zu gewährleisten.");
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
            .Should()
            .ResideInNamespace("TicketsPlease.Domain.Entities")
            .GetResult();

        result.IsSuccessful.Should().BeTrue("Entitäten sollten in einem spezifischen Namespace gruppiert sein.");
    }
}
