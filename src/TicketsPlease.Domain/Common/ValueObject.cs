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
  /// <inheritdoc />
  public override bool Equals(object? obj)
  {
    if (obj == null || obj.GetType() != this.GetType())
    {
      return false;
    }

    var other = (ValueObject)obj;
    return this.GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
  }

  /// <inheritdoc />
  public override int GetHashCode()
  {
    return this.GetEqualityComponents()
        .Select(x => x?.GetHashCode() ?? 0)
        .Aggregate((x, y) => x ^ y);
  }

  /// <summary>
  /// Gets die Komponenten, die für den Gleichheitsvergleich herangezogen werden.
  /// </summary>
  /// <returns>Eine Aufzählung der Komponenten.</returns>
  protected abstract IEnumerable<object?> GetEqualityComponents();
}
