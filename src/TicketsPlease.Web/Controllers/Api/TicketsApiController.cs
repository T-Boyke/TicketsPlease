// <copyright file="TicketsApiController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TicketsPlease.Web.Controllers.Api;

using Microsoft.AspNetCore.Mvc;

/// <summary>
/// API für die Verwaltung von Tickets.
/// Diese API wird über Scalar dokumentiert.
/// </summary>
[ApiController]
[Route("api/v1/tickets")]
[Produces("application/json")]
internal class TicketsApiController : ControllerBase
{
  /// <summary>
  /// Ruft alle Tickets ab.
  /// </summary>
  /// <returns>Eine Liste aller Tickets.</returns>
  /// <response code="200">Die Liste wurde erfolgreich abgerufen.</response>
  [HttpGet]
  [ProducesResponseType(StatusCodes.Status200OK)]
  public IActionResult GetTickets()
  {
    // Demo-Daten
    return this.Ok(new[]
    {
      new { Id = 1, Title = "Login funktioniert nicht", Priority = "Hoch", Status = "Offen" },
      new { Id = 2, Title = "Design-Anpassungen im Footer", Priority = "Niedrig", Status = "In Arbeit" },
    });
  }

  /// <summary>
  /// Ruft ein spezifisches Ticket ab.
  /// </summary>
  /// <param name="id">Die ID des Tickets.</param>
  /// <returns>Das angefragte Ticket.</returns>
  /// <response code="200">Das Ticket wurde gefunden.</response>
  /// <response code="404">Das Ticket wurde nicht gefunden.</response>
  [HttpGet("{id:int}")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public IActionResult GetTicket(int id)
  {
    if (id != 1)
    {
      return this.NotFound();
    }

    return this.Ok(new { Id = 1, Title = "Login funktioniert nicht", Priority = "Hoch", Status = "Offen" });
  }
}
