// <copyright file="DashboardApiController.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Web.Controllers.Api;

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketsPlease.Application.Common.Interfaces;

/// <summary>
/// API-Controller für Dashboard-Daten und Performance-Details.
/// </summary>
[Authorize]
[ApiController]
[Route("api/dashboard")]
internal sealed class DashboardApiController : ControllerBase
{
    private readonly IDashboardService dashboardService;

    /// <summary>
    /// Initializes a new instance of the <see cref="DashboardApiController"/> class.
    /// </summary>
    /// <param name="dashboardService">Der Dashboard-Service.</param>
    public DashboardApiController(IDashboardService dashboardService)
    {
        this.dashboardService = dashboardService;
    }

    /// <summary>
    /// Ruft Performance-Details für einen Benutzer ab.
    /// </summary>
    /// <param name="id">Die Benutzer-ID.</param>
    /// <returns>Detaillierte Statistiken.</returns>
    [HttpGet("user/{id}")]
    public async Task<IActionResult> GetUserDetail(Guid id)
    {
        try
        {
            var detail = await this.dashboardService.GetUserPerformanceDetailAsync(id).ConfigureAwait(false);
            return this.Ok(detail);
        }
        catch (Exception ex)
        {
            return this.NotFound(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Ruft Performance-Details für ein Team ab.
    /// </summary>
    /// <param name="id">Die Team-ID.</param>
    /// <returns>Detaillierte Statistiken.</returns>
    [HttpGet("team/{id}")]
    public async Task<IActionResult> GetTeamDetail(Guid id)
    {
        try
        {
            var detail = await this.dashboardService.GetTeamPerformanceDetailAsync(id).ConfigureAwait(false);
            return this.Ok(detail);
        }
        catch (Exception ex)
        {
            return this.NotFound(new { message = ex.Message });
        }
    }
}
