// <copyright file="ValueObject.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TicketsPlease.Domain.Common;

using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Die Basisklasse für alle ValueObjects in der Domäne.
/// Implementiert die wertbasierte Gleichheit (Structural Equality).
/// </summary>
public abstract class ValueObject
{
    /// <summary>
    /// Überprüft die Gleichheit zweier ValueObjects.
    /// </summary>
    /// <param name="left">Das linke Objekt.</param>
    /// <param name="right">Das rechte Objekt.</param>
    /// <returns>True, wenn beide gleich sind.</returns>
    public static bool operator ==(ValueObject? left, ValueObject? right)
    {
        if (left is null ^ right is null)
        {
            return false;
        }

        return left?.Equals(right!) ?? true;
    }

    /// <summary>
    /// Überprüft die Ungleichheit zweier ValueObjects.
    /// </summary>
    /// <param name="left">Das linke Objekt.</param>
    /// <param name="right">Das rechte Objekt.</param>
    /// <returns>True, wenn beide ungleich sind.</returns>
    public static bool operator !=(ValueObject? left, ValueObject? right)
    {
        return !(left == right);
    }

    /// <summary>
    /// Gibt alle Komponenten zurück, die zur Identifizierung der Gleichheit beitragen.
    /// </summary>
    /// <returns>Eine Enumeration von Objekten.</returns>
    protected abstract IEnumerable<object?> GetEqualityComponents();

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        if (obj == null || obj.GetType() != this.GetType())
        {
            return false;
        }

        var other = (ValueObject)obj;

        return this.GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return this.GetEqualityComponents()
            .Select(x => x?.GetHashCode() ?? 0)
            .Aggregate((x, y) => x ^ y);
    }
}
