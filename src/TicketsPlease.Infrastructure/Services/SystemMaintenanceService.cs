// <copyright file="SystemMaintenanceService.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Infrastructure.Services;

using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TicketsPlease.Infrastructure.Persistence;

/// <summary>
/// Dienst für systemkritische Wartungsarbeiten wie das Löschen der Datenbank oder einzelner Tabellen.
/// </summary>
public class SystemMaintenanceService
{
  private readonly AppDbContext context;
  private readonly ILogger<SystemMaintenanceService> logger;

  /// <summary>
  /// Initializes a new instance of the <see cref="SystemMaintenanceService"/> class.
  /// </summary>
  /// <param name="context">Der Datenbankkontext.</param>
  /// <param name="logger">Der Logger.</param>
  public SystemMaintenanceService(AppDbContext context, ILogger<SystemMaintenanceService> logger)
  {
    this.context = context;
    this.logger = logger;
  }

  /// <summary>
  /// Löscht alle Daten aus einer spezifischen Tabelle.
  /// </summary>
  /// <param name="tableName">Der Name der Tabelle.</param>
  /// <returns>Ein Task.</returns>
  public async Task TruncateTableAsync(string tableName)
  {
    this.logger.LogWarning("Manuelle Tabellenlöschung angefordert: {TableName}", tableName);
    
    // Einfache SQL-Safe-Prüfung (nur bekannte Tabellennamen erlauben)
    var allowedTables = new[] { "Tickets", "Tasks", "Comments", "TimeLogs", "Notifications", "Messages", "Teams", "TeamMembers", "SubTickets" };
    if (!System.Linq.Enumerable.Contains(allowedTables, tableName))
    {
        throw new InvalidOperationException($"Tabelle '{tableName}' ist nicht für automatisierte Löschung freigegeben.");
    }

    var sql = $"DELETE FROM [{tableName}]";
    await this.context.Database.ExecuteSqlRawAsync(sql).ConfigureAwait(false);
    this.logger.LogInformation("Tabelle {TableName} wurde erfolgreich geleert.", tableName);
  }

  /// <summary>
  /// Löscht die gesamte Datenbank und erstellt das Schema neu (Wipe).
  /// </summary>
  /// <returns>Ein Task.</returns>
  public async Task WipeDatabaseAsync()
  {
    this.logger.LogCritical("KOMPLETTER DATENBANK-WIPE ANGEFORDERT!");
    
    await this.context.Database.EnsureDeletedAsync().ConfigureAwait(false);
    await this.context.Database.EnsureCreatedAsync().ConfigureAwait(false);
    
    this.logger.LogInformation("Datenbank wurde erfolgreich gewiped und neu erstellt.");
  }
}
