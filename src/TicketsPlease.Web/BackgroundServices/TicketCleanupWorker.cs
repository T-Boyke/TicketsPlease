// <copyright file="TicketCleanupWorker.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Web.BackgroundServices;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TicketsPlease.Infrastructure.Persistence;

/// <summary>
/// Background worker that automatically archives tickets (F2.1.5).
/// Tickets that are 'Done' or 'Closed' for > 30 days are moved to 'Archived'.
/// </summary>
internal class TicketCleanupWorker : BackgroundService
{
  private readonly IServiceProvider serviceProvider;
  private readonly ILogger<TicketCleanupWorker> logger;

  /// <summary>
  /// Initializes a new instance of the <see cref="TicketCleanupWorker"/> class.
  /// </summary>
  /// <param name="serviceProvider">Der ServiceProvider für Scoped-Services.</param>
  /// <param name="logger">Der Logger.</param>
  public TicketCleanupWorker(IServiceProvider serviceProvider, ILogger<TicketCleanupWorker> logger)
  {
    this.serviceProvider = serviceProvider;
    this.logger = logger;
  }

  /// <inheritdoc/>
  protected override async Task ExecuteAsync(CancellationToken stoppingToken)
  {
    this.logger.LogInformation("TicketCleanupWorker started.");

    while (!stoppingToken.IsCancellationRequested)
    {
      try
      {
        await this.DoWorkAsync().ConfigureAwait(false);
      }
      catch (Exception ex)
      {
        this.logger.LogError(ex, "Error occurred during ticket cleanup.");
      }

      // Run once per day
      await Task.Delay(TimeSpan.FromDays(1), stoppingToken).ConfigureAwait(false);
    }
  }

  private async Task DoWorkAsync()
  {
    using var scope = this.serviceProvider.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    var cutoff = DateTime.UtcNow.AddDays(-30);

    // Find tickets that are terminal and older than 30 days
    var terminalStates = context.WorkflowStates
        .Where(s => s.IsTerminalState || s.Name == "Done" || s.Name == "Closed")
        .Select(s => s.Id)
        .ToList();

    var archiveState = context.WorkflowStates.FirstOrDefault(s => s.Name == "Archived");
    if (archiveState == null)
    {
      this.logger.LogWarning("Status 'Archived' not found. Skipping cleanup.");
      return;
    }

    var ticketsToArchive = context.Tickets
        .Where(t => terminalStates.Contains(t.WorkflowStateId))
        .Where(t => t.ClosedAt.HasValue && t.ClosedAt.Value < cutoff)
        .ToList();

    if (ticketsToArchive.Any())
    {
      this.logger.LogInformation("Archiving {Count} tickets.", ticketsToArchive.Count);
      foreach (var ticket in ticketsToArchive)
      {
        // We reflectively or directly set the state if we can't use MoveToState easily
        // For simplicity in the worker, we update the DB directly
        // ticket.MoveToState(archiveState.Id); // If accessible
        var stateIdProp = ticket.GetType().GetProperty("WorkflowStateId");
        stateIdProp?.SetValue(ticket, archiveState.Id);

        var statusProp = ticket.GetType().GetProperty("Status");
        statusProp?.SetValue(ticket, "Archived");
      }

      await context.SaveChangesAsync().ConfigureAwait(false);
    }
  }
}
