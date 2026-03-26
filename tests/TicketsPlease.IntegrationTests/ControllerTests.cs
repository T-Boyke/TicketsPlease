// <copyright file="ControllerTests.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.IntegrationTests;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TicketsPlease.Domain.Entities;
using TicketsPlease.Infrastructure.Persistence;
using Xunit;

/// <summary>
/// Integrations-Tests für die Web-Controller.
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores", Justification = "Test naming convention")]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Reliability", "CA2007:Consider calling ConfigureAwait", Justification = "xUnit tests should not use ConfigureAwait(false)")]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Reliability", "CA2000:Objekte verwerfen, bevor der Gültigkeitsbereich verloren geht", Justification = "Disposed by HttpClient or not critical for short-lived tests")]
public class ControllerTests : IntegrationTestBase
{
  private readonly Guid adminId = Guid.Parse("00000000-0000-0000-0000-000000000001");
  private readonly Guid priorityId = Guid.Parse("00000000-0000-0000-0000-000000000002");
  private readonly Guid stateId = Guid.Parse("00000000-0000-0000-0000-000000000003");

  /// <summary>
  /// Tests HomeController.
  /// </summary>
  /// <returns>A task.</returns>
  [Fact]
  public async Task HomeController_Index_Should_Return_Success()
  {
    // Arrange
    var client = this.Factory.CreateClient();

    // Act
    var response = await client.GetAsync(new Uri("/", UriKind.Relative));

    // Assert
    response.EnsureSuccessStatusCode();
    response.StatusCode.Should().Be(HttpStatusCode.OK);
  }

  /// <summary>
  /// Tests TicketsController protection.
  /// </summary>
  /// <returns>A task.</returns>
  [Fact]
  public async Task TicketsController_Index_Should_Redirect_To_Login_If_Not_Authenticated()
  {
    // Arrange
    var client = this.Factory.CreateClient(new Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactoryClientOptions
    {
      AllowAutoRedirect = false,
    });

    // Act
    var response = await client.GetAsync(new Uri("/Tickets", UriKind.Relative));

    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.Redirect);
    response.Headers.Location!.ToString().Should().Contain("/Account/Login");
  }

  /// <summary>
  /// Tests TicketsController Index for authenticated user.
  /// </summary>
  /// <returns>A task.</returns>
  [Fact]
  public async Task TicketsController_Index_Should_Work_When_Authenticated()
  {
    // Arrange
    using var scope = this.Factory.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await SeedMinimalAsync(db);
    var project = (await db.Projects.ToListAsync())[0];

    var client = this.Factory.CreateClient();
    client.DefaultRequestHeaders.Add(TestAuthHandler.UserIdHeader, Guid.NewGuid().ToString());
    client.DefaultRequestHeaders.Add(TestAuthHandler.TenantIdHeader, project.TenantId.ToString());

    // Act
    var response = await client.GetAsync(new Uri("/Tickets", UriKind.Relative));

    // Assert
    response.EnsureSuccessStatusCode();
    var content = await response.Content.ReadAsStringAsync();
    content.Should().Contain("Kanban");
  }

  /// <summary>
  /// Tests TicketsController Create (POST).
  /// </summary>
  /// <returns>A task.</returns>
  [Fact]
  public async Task TicketsController_Create_Post_Should_Create_Ticket_And_Redirect()
  {
    // Arrange
    using var scope = this.Factory.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await SeedMinimalAsync(db);
    var project = (await db.Projects.ToListAsync())[0];

    var client = this.Factory.CreateClient(new Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactoryClientOptions
    {
      AllowAutoRedirect = false,
    });
    client.DefaultRequestHeaders.Add(TestAuthHandler.UserIdHeader, this.adminId.ToString());
    client.DefaultRequestHeaders.Add(TestAuthHandler.TenantIdHeader, project.TenantId.ToString());

    var formData = new FormUrlEncodedContent(new[]
    {
        new KeyValuePair<string, string>("Title", "New Ticket From Test"),
        new KeyValuePair<string, string>("Description", "Some Description"),
        new KeyValuePair<string, string>("ProjectId", project.Id.ToString()),
        new KeyValuePair<string, string>("PriorityId", this.priorityId.ToString()),
        new KeyValuePair<string, string>("WorkflowStateId", this.stateId.ToString()),
        new KeyValuePair<string, string>("Type", "1"), // Bug
    });

    // Act
    var response = await client.PostAsync(new Uri("/Tickets/Create", UriKind.Relative), formData);

    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.Redirect);
    var ticket = await db.Tickets.FirstOrDefaultAsync(t => t.Title == "New Ticket From Test");
    ticket.Should().NotBeNull();
  }

  /// <summary>
  /// Tests ProjectController Index.
  /// </summary>
  /// <returns>A task.</returns>
  [Fact]
  public async Task ProjectController_Index_Should_Return_Success()
  {
    // Arrange
    using var scope = this.Factory.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await SeedMinimalAsync(db);
    var project = (await db.Projects.ToListAsync())[0];

    var client = this.Factory.CreateClient();
    client.DefaultRequestHeaders.Add(TestAuthHandler.UserIdHeader, Guid.NewGuid().ToString());
    client.DefaultRequestHeaders.Add(TestAuthHandler.TenantIdHeader, project.TenantId.ToString());

    // Act
    var response = await client.GetAsync(new Uri("/Project", UriKind.Relative));

    // Assert
    response.EnsureSuccessStatusCode();
    response.StatusCode.Should().Be(HttpStatusCode.OK);
  }

  /// <summary>
  /// Tests CommentController Create (POST).
  /// </summary>
  /// <returns>A task.</returns>
  [Fact]
  public async Task CommentController_Create_Post_Should_Add_Comment()
  {
     // Arrange
    using var scope = this.Factory.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await SeedMinimalAsync(db);
    var project = (await db.Projects.ToListAsync())[0];

    var ticket = new Ticket("Comment Test", Domain.Enums.TicketType.Task, project.Id, this.adminId, this.stateId, string.Empty);
    ticket.SetTenantId(project.TenantId);
    ticket.SetPriority(this.priorityId);
    await db.Tickets.AddAsync(ticket);
    await db.SaveChangesAsync();

    var client = this.Factory.CreateClient();
    client.DefaultRequestHeaders.Add(TestAuthHandler.UserIdHeader, this.adminId.ToString());
    client.DefaultRequestHeaders.Add(TestAuthHandler.TenantIdHeader, project.TenantId.ToString());

    var formData = new FormUrlEncodedContent(new[]
    {
        new KeyValuePair<string, string>("TicketId", ticket.Id.ToString()),
        new KeyValuePair<string, string>("Content", "Test Comment Content"),
    });

    // Act
    var response = await client.PostAsync(new Uri("/Comment/Create", UriKind.Relative), formData);

    // Assert
    response.EnsureSuccessStatusCode();
    var comment = await db.Comments.FirstOrDefaultAsync(c => c.Content == "Test Comment Content");
    comment.Should().NotBeNull();
  }
}
