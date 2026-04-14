// <copyright file="SLABackgroundService.cs" company="BitLC-NE-2025-2026">
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
using TicketsPlease.Application.Common.Interfaces;
using TicketsPlease.Domain.Entities;

/// <summary>
/// Background service for enterprise SLA automation.
/// Checks active organizations for SLA breaches and triggers notifications.
/// </summary>
public sealed partial class SLABackgroundService : BackgroundService
{
  private readonly IServiceProvider serviceProvider;
  private readonly ILogger<SLABackgroundService> logger;

  /// <summary>
  /// Initializes a new instance of the <see cref="SLABackgroundService"/> class.
  /// </summary>
  /// <param name="serviceProvider">The service provider.</param>
  /// <param name="logger">The logger.</param>
  public SLABackgroundService(IServiceProvider serviceProvider, ILogger<SLABackgroundService> logger)
  {
    this.serviceProvider = serviceProvider;
    this.logger = logger;
  }

  /// <inheritdoc/>
  protected override async Task ExecuteAsync(CancellationToken stoppingToken)
  {
    this.logger.LogInformation("SLABackgroundService is starting.");

    while (!stoppingToken.IsCancellationRequested)
    {
      try
      {
        await this.ProcessAllOrganizationsAsync(stoppingToken).ConfigureAwait(false);
      }
      catch (OperationCanceledException)
      {
        // Normal shutdown
      }
      catch (Exception ex)
      {
        this.logger.LogError(ex, "Error occurred in SLABackgroundService.");
      }

      // Default wait 5 minutes if no organizations, but usually handled by organization-specific logic
      await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken).ConfigureAwait(false);
    }

    this.logger.LogInformation("SLABackgroundService is stopping.");
  }

  private async Task ProcessAllOrganizationsAsync(CancellationToken stoppingToken)
  {
    using var scope = this.serviceProvider.CreateScope();
    var orgRepository = scope.ServiceProvider.GetRequiredService<IOrganizationRepository>();
    var ticketRepository = scope.ServiceProvider.GetRequiredService<ITicketRepository>();
    var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();

    var organizations = await orgRepository.GetAllAsync(stoppingToken).ConfigureAwait(false);

    foreach (var org in organizations.Where(o => o.IsActive))
    {
      if (stoppingToken.IsCancellationRequested)
      {
        break;
      }

      try
      {
        await this.ProcessOrganizationSlaAsync(org, ticketRepository, notificationService, stoppingToken).ConfigureAwait(false);
      }
      catch (Exception ex)
      {
        this.logger.LogError(ex, "Error processing SLA for organization {OrgName} ({OrgId})", org.Name, org.Id);
      }
    }
  }

  private async Task ProcessOrganizationSlaAsync(Organization org, ITicketRepository ticketRepository, INotificationService notificationService, CancellationToken ct)
  {
    // Check Quiet Hours
    if (this.IsInsideQuietHours(org))
    {
      this.logger.LogDebug("Organization {OrgName} is currently in quiet hours. Skipping SLA checks.", org.Name);
      return;
    }

    // Logic for interval can be implemented by storing "LastCheckedAt" or similar, 
    // but for this version we run every time the loop hits them.
    // In a more complex version, we would check if (DateTime.UtcNow - org.LastSlaCheck) > org.SlaCheckInterval.

    var tickets = await ticketRepository.GetAllActiveAsync(ct).ConfigureAwait(false);
    var orgTickets = tickets.Where(t => t.TenantId == org.Id && t.Status != "Done").ToList();

    foreach (var ticket in orgTickets)
    {
      await this.CheckTicketSlaAsync(org, ticket, notificationService).ConfigureAwait(false);
    }
  }

  private bool IsInsideQuietHours(Organization org)
  {
    if (org.QuietHoursStart == null || org.QuietHoursEnd == null)
    {
      return false;
    }

    try
    {
      var tz = TimeZoneInfo.FindSystemTimeZoneById(org.TimeZoneId ?? "UTC");
      var localTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, tz).TimeOfDay;

      if (org.QuietHoursStart < org.QuietHoursEnd)
      {
        // Typical case: e.g., 22:00 to 06:00 (this logic handles 08:00 to 18:00 if they wanted to invert it)
        // Wait, 22:00 to 06:00 is NOT org.QuietHoursStart < org.QuietHoursEnd.
        // If Start = 22 and End = 06, then Start > End.
        return localTime >= org.QuietHoursStart && localTime <= org.QuietHoursEnd;
      }
      else
      {
        // Overnight case: e.g., 22:00 to 06:00
        return localTime >= org.QuietHoursStart || localTime <= org.QuietHoursEnd;
      }
    }
    catch (TimeZoneNotFoundException)
    {
      this.logger.LogWarning("Timezone {TzId} not found for organization {OrgId}. Falling back to UTC.", org.TimeZoneId, org.Id);
      var utcTime = DateTime.UtcNow.TimeOfDay;
      if (org.QuietHoursStart < org.QuietHoursEnd)
      {
        return utcTime >= org.QuietHoursStart && utcTime <= org.QuietHoursEnd;
      }
      else
      {
        return utcTime >= org.QuietHoursStart || utcTime <= org.QuietHoursEnd;
      }
    }
  }

  private async Task CheckTicketSlaAsync(Organization org, Ticket ticket, INotificationService notificationService)
  {
    var now = DateTime.UtcNow;

    // Response SLA
    if (ticket.LastRespondedAt == null && ticket.ResponseDeadline.HasValue && now > ticket.ResponseDeadline.Value)
    {
        await this.NotifySlaBreachAsync(org, ticket, "Response", notificationService).ConfigureAwait(false);
    }

    // Resolution SLA
    if (ticket.ClosedAt == null && ticket.ResolutionDeadline.HasValue && now > ticket.ResolutionDeadline.Value)
    {
        await this.NotifySlaBreachAsync(org, ticket, "Resolution", notificationService).ConfigureAwait(false);
    }
  }

  private async Task NotifySlaBreachAsync(Organization org, Ticket ticket, string type, INotificationService notificationService)
  {
    // Check if notifications are enabled for this priority
    bool shouldNotify = ticket.Priority?.Name switch
    {
        "Low" => org.NotifyOnLow,
        "Medium" => org.NotifyOnMedium,
        "High" => org.NotifyOnHigh,
        "Blocker" => org.NotifyOnBlocker,
        _ => true // Default to true for unknown or unassigned priority
    };

    if (!shouldNotify)
    {
        return;
    }

    var message = $"SLA Breach ({type}): Ticket '{ticket.Title}' (ID: {ticket.Id}) has exceeded its deadline.";
    var title = "⚠️ SLA BREACH!";

    // Notify Organization (All) or specific PO/Admins? Requirement says "notifications for all"
    await notificationService.SendNotificationToAllAsync(title, message).ConfigureAwait(false);
  }
}
