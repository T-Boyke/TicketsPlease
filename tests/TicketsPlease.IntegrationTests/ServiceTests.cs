// <copyright file="ServiceTests.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.IntegrationTests;

using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TicketsPlease.Application.Common.Dtos;
using TicketsPlease.Application.Common.Interfaces;
using TicketsPlease.Domain.Entities;
using TicketsPlease.Domain.Enums;
using TicketsPlease.Infrastructure.Persistence;
using Xunit;

/// <summary>
/// Integrations-Tests für die Service-Layer (TicketService, etc.).
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores", Justification = "Test naming convention")]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Reliability", "CA2007:Consider calling ConfigureAwait", Justification = "xUnit tests should not use ConfigureAwait(false)")]
public class ServiceTests : IntegrationTestBase
{
  private readonly Guid adminId = IntegrationTestBase.TestUserId;
  private readonly Guid priorityId = IntegrationTestBase.MediumPriorityId;
  private readonly Guid stateId = IntegrationTestBase.TodoStateId;

  /// <summary>
  /// Initializes a new instance of the <see cref="ServiceTests"/> class.
  /// </summary>
  public ServiceTests()
  {
  }

  /// <summary>
  /// Tests TicketService.CreateTicketAsync.
  /// </summary>
  /// <returns>A task.</returns>
  [Fact]
  public async Task TicketService_CreateTicket_Should_Persist_Ticket()
  {
    // Arrange
    using var scope = this.Factory.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await SeedMinimalAsync(db);
    this.SetContext(scope.ServiceProvider, IntegrationTestBase.TestUserId, IntegrationTestBase.TestTenantId);
    var project = (await db.Projects.IgnoreQueryFilters().ToListAsync())[0];

    var service = scope.ServiceProvider.GetRequiredService<ITicketService>();
    var dto = new CreateTicketDto(
        Title: "Service Test Ticket",
        Description: "Desc",
        ProjectId: project.Id,
        PriorityId: this.priorityId,
        AssignedUserId: this.adminId,
        EstimatePoints: 3);

    // Act
    await service.CreateTicketAsync(dto);

    // Assert
    var ticket = await db.Tickets.IgnoreQueryFilters().FirstOrDefaultAsync(t => t.Title == "Service Test Ticket");
    ticket.Should().NotBeNull();
    ticket!.ProjectId.Should().Be(project.Id);
  }

  /// <summary>
  /// Tests TicketService.UpdateTicketAsync.
  /// </summary>
  /// <returns>A task.</returns>
  [Fact]
  public async Task TicketService_UpdateTicket_Should_Modify_Ticket()
  {
    // Arrange
    using var scope = this.Factory.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await SeedMinimalAsync(db);
    this.SetContext(scope.ServiceProvider, IntegrationTestBase.TestUserId, IntegrationTestBase.TestTenantId);
    var project = (await db.Projects.IgnoreQueryFilters().ToListAsync())[0];
    var todoStateId = Guid.Parse("00000000-0000-0000-0000-000000000003");
    var doneStateId = Guid.Parse("00000000-0000-0000-0000-000000000004");
    var priorityId = Guid.Parse("00000000-0000-0000-0000-000000000002");

    var ticket = new Ticket("Transition Test", TicketType.Task, project.Id, IntegrationTestBase.TestUserId, todoStateId, "Todo", string.Empty);
    ticket.SetPriority(priorityId);
    ticket.SetTenantId(IntegrationTestBase.TestTenantId);

    await db.Tickets.AddAsync(ticket);
    await db.SaveChangesAsync();

    var service = scope.ServiceProvider.GetRequiredService<ITicketService>();
    var dto = new UpdateTicketDto(
        Id: ticket.Id,
        Title: "After",
        Description: "Updated Desc",
        Status: "Testing",
        PriorityId: this.priorityId,
        AssignedUserId: this.adminId,
        EstimatePoints: 5);

    // Act
    await service.UpdateTicketAsync(dto);

    // Assert
    var updated = await db.Tickets.FindAsync(ticket.Id);
    updated!.Title.Should().Be("After");
    updated.Description.Should().Be("Updated Desc");
  }

  /// <summary>
  /// Tests TicketService.MoveTicketAsync.
  /// </summary>
  /// <returns>A task.</returns>
  [Fact]
  public async Task TicketService_MoveTicket_Should_Update_State()
  {
    // Arrange
    using var scope = this.Factory.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await SeedMinimalAsync(db);
    this.SetContext(scope.ServiceProvider, IntegrationTestBase.TestUserId, IntegrationTestBase.TestTenantId);
    var project = (await db.Projects.IgnoreQueryFilters().ToListAsync())[0];

    // Use seeded state IDs
    var todoStateId = IntegrationTestBase.TodoStateId;
    var doneStateId = IntegrationTestBase.DoneStateId;

    var ticket = new Ticket("Move Test", TicketType.Task, project.Id, this.adminId, todoStateId, "Todo", string.Empty);
    ticket.SetTenantId(project.TenantId);
    ticket.SetPriority(this.priorityId);
    await db.Tickets.AddAsync(ticket);
    await db.SaveChangesAsync();

    var service = scope.ServiceProvider.GetRequiredService<ITicketService>();

    // Act
    await service.MoveTicketAsync(ticket.Id, "Done");

    // Assert
    using (var verifyScope = this.Factory.Services.CreateScope())
    {
      var verifyDb = verifyScope.ServiceProvider.GetRequiredService<AppDbContext>();
      var updatedTicket = await verifyDb.Tickets.IgnoreQueryFilters().FirstOrDefaultAsync(t => t.Id == ticket.Id);
      updatedTicket.Should().NotBeNull();
      updatedTicket!.WorkflowStateId.Should().Be(IntegrationTestBase.DoneStateId);
    }
  }

  /// <summary>
  /// Tests TicketService.RemoveDependencyAsync.
  /// </summary>
  /// <returns>A task.</returns>
  [Fact]
  public async Task TicketService_RemoveDependency_Should_Work()
  {
    // Arrange
    using var scope = this.Factory.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await SeedMinimalAsync(db);
    this.SetContext(scope.ServiceProvider, IntegrationTestBase.TestUserId, IntegrationTestBase.TestTenantId);
    var project = (await db.Projects.IgnoreQueryFilters().ToListAsync())[0];

    var t1 = new Ticket("T1", TicketType.Task, project.Id, this.adminId, this.stateId, "Todo", string.Empty);
    t1.SetTenantId(project.TenantId);
    t1.SetPriority(this.priorityId);
    var t2 = new Ticket("T2", TicketType.Task, project.Id, this.adminId, this.stateId, "Todo", string.Empty);
    t2.SetTenantId(project.TenantId);
    t2.SetPriority(this.priorityId);
    await db.Tickets.AddRangeAsync(t1, t2);
    await db.SaveChangesAsync();

    var service = scope.ServiceProvider.GetRequiredService<ITicketService>();
    await service.AddDependencyAsync(t1.Id, t2.Id);

    // Verify added
    (await db.TicketLinks.CountAsync()).Should().Be(1);
    var link = await db.TicketLinks.FirstAsync();

    // Act
    await service.RemoveDependencyAsync(t1.Id, link.Id);

    // Assert
    using (var verifyScope = this.Factory.Services.CreateScope())
    {
      var verifyDb = verifyScope.ServiceProvider.GetRequiredService<AppDbContext>();
      (await verifyDb.TicketLinks.CountAsync()).Should().Be(0);
    }
  }

  /// <summary>
  /// Tests TicketService.GetFilteredTicketsAsync (F6).
  /// </summary>
  /// <returns>A task.</returns>
  [Fact]
  public async Task TicketService_GetFilteredTickets_Should_Return_Correct_Results()
  {
    // Arrange
    using var scope = this.Factory.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await SeedMinimalAsync(db);
    this.SetContext(scope.ServiceProvider, IntegrationTestBase.TestUserId, IntegrationTestBase.TestTenantId);
    var project = (await db.Projects.IgnoreQueryFilters().ToListAsync())[0];

    var t1 = new Ticket("T1", TicketType.Task, project.Id, this.adminId, this.stateId, "Todo", string.Empty);
    t1.SetTenantId(project.TenantId);
    t1.SetPriority(this.priorityId);
    await db.Tickets.AddAsync(t1);
    await db.SaveChangesAsync();

    var service = scope.ServiceProvider.GetRequiredService<ITicketService>();

    // Act
    var results = await service.GetFilteredTicketsAsync(projectId: project.Id);

    // Assert
    results.Should().NotBeEmpty();
    results.All(r => r.ProjectId == project.Id).Should().BeTrue();
  }
}
