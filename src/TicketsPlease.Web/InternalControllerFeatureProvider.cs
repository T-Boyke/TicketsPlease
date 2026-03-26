// <copyright file="InternalControllerFeatureProvider.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Web;

using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;

/// <summary>
/// Ein benutzerdefinierter <see cref="ControllerFeatureProvider"/>, der auch interne Klassen als Controller zulässt.
/// Dies ist notwendig, wenn Controller als 'internal sealed' markiert sind (z. B. zur Einhaltung von CA1515).
/// </summary>
internal sealed class InternalControllerFeatureProvider : ControllerFeatureProvider
{
  /// <inheritdoc />
  protected override bool IsController(TypeInfo typeInfo)
  {
    if (!typeInfo.IsClass || typeInfo.IsAbstract || typeInfo.ContainsGenericParameters)
    {
      return false;
    }

    if (typeInfo.IsDefined(typeof(NonControllerAttribute)))
    {
      return false;
    }

    if (!typeInfo.Name.EndsWith("Controller", StringComparison.OrdinalIgnoreCase) &&
        !typeInfo.IsDefined(typeof(ControllerAttribute)))
    {
      return false;
    }

    // Wir erlauben hier auch interne Klassen (im Gegensatz zum Standard-Provider, der nur public Typen prüft).
    return true;
  }
}
