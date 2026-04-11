// <copyright file="InfrastructureTests.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.IntegrationTests;

using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TicketsPlease.Domain.Entities;
using TicketsPlease.Domain.Enums;
using TicketsPlease.Infrastructure.Persistence;
using TicketsPlease.Infrastructure.Repositories;
using Xunit;

/// <summary>
/// Integrations-Tests für die Infrastructure-Repositories.
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores", Justification = "Test naming convention")]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Reliability", "CA2007:Consider calling ConfigureAwait", Justification = "xUnit tests should not use ConfigureAwait(false)")]
public class InfrastructureTests : IntegrationTestBase
{
  /// <summary>
  /// Tests GetByIdAsync.
  /// </summary>
  /// <returns>A task.</returns>
  [Fact]
  public async Task TicketRepository_GetByIdAsync_Should_Load_All_Includes()
  {
    // Arrange
    using var scope = this.Factory.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var repo = new TicketRepository(db);

    await SeedMinimalAsync(db);
    var project = await db.Projects.ToListAsync();
    var projects = project[0];
    var priorityId = (await db.TicketPriorities.ToListAsync())[0].Id;
    var workflowStateId = (await db.WorkflowStates.ToListAsync())[0].Id;

    var creator = await CreateTestUserAsync(db, "creator", projects.TenantId);

    var ticket = new Ticket("Include-Test", TicketType.Task, projects.Id, creator.Id, workflowStateId, "Todo", "::1");
    ticket.SetPriority(priorityId);
    ticket.SetTenantId(projects.TenantId);
    db.Tickets.Add(ticket);
    await db.SaveChangesAsync();

    var comment = new Comment("Test-Comment", ticket.Id, creator.Id);
    comment.SetTenantId(projects.TenantId);
    db.Comments.Add(comment);
    await db.SaveChangesAsync();

    // Act
    var result = await repo.GetByIdAsync(ticket.Id);

    // Assert
    result.Should().NotBeNull();
    result!.Comments.Should().HaveCount(1);
    result.Comments.ElementAt(0).Author.Should().NotBeNull();
    result.Project.Should().NotBeNull();
  }

  /// <summary>
  /// Tests GetAllActiveAsync.
  /// </summary>
  /// <returns>A task.</returns>
  [Fact]
  public async Task TicketRepository_GetAllActiveAsync_Should_Return_Non_Deleted_Sorted_By_Priority()
  {
    // Arrange
    using var scope = this.Factory.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var repo = new TicketRepository(db);

    await SeedMinimalAsync(db);
    var projects = await db.Projects.ToListAsync();
    var project = projects[0];
    var stateId = (await db.WorkflowStates.ToListAsync())[0].Id;
    var creator = await CreateTestUserAsync(db, "sorter", project.TenantId);

    var p1 = new TicketPriority { Id = Guid.NewGuid(), Name = "High", LevelWeight = 100, TenantId = project.TenantId };
    var p2 = new TicketPriority { Id = Guid.NewGuid(), Name = "Low", LevelWeight = 10, TenantId = project.TenantId };
    db.TicketPriorities.AddRange(p1, p2);

    var t1 = new Ticket("Low Priority", TicketType.Task, project.Id, creator.Id, stateId, "Todo", "::1");
    t1.SetPriority(p2.Id);
    t1.SetTenantId(project.TenantId);

    var t2 = new Ticket("High Priority", TicketType.Task, project.Id, creator.Id, stateId, "Todo", "::1");
    t2.SetPriority(p1.Id);
    t2.SetTenantId(project.TenantId);

    db.Tickets.AddRange(t1, t2);
    await db.SaveChangesAsync();

    // Act
    var results = await repo.GetAllActiveAsync();

    // Assert
    results.Should().NotBeEmpty();
    results[0].Title.Should().Be("High Priority");
  }

  /// <summary>
  /// Tests GetFilteredAsync.
  /// </summary>
  /// <param name="filterProject">Filter project.</param>
  /// <param name="filterAssignee">Filter assignee.</param>
  /// <param name="filterCreator">Filter creator.</param>
  /// <returns>A task.</returns>
  [Theory]
  [InlineData(true, false, false)]
  [InlineData(false, true, false)]
  [InlineData(false, false, true)]
  [InlineData(true, true, true)]
  public async Task TicketRepository_GetFilteredAsync_Should_Apply_Filters(bool filterProject, bool filterAssignee, bool filterCreator)
  {
    // Arrange
    using var scope = this.Factory.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var repo = new TicketRepository(db);

    await SeedMinimalAsync(db);
    var projects = await db.Projects.ToListAsync();
    var project = projects[0];
    var otherProject = new Project("Other", DateTime.UtcNow);
    otherProject.SetTenantId(project.TenantId);
    db.Projects.Add(otherProject);

    var user1 = await CreateTestUserAsync(db, "u1", project.TenantId);
    var user2 = await CreateTestUserAsync(db, "u2", project.TenantId);

    var priorities = await db.TicketPriorities.ToListAsync();
    var priorityId = priorities[0].Id;
    var states = await db.WorkflowStates.ToListAsync();
    var stateId = states[0].Id;
    var ticket = new Ticket("Target", TicketType.Task, project.Id, user1.Id, stateId, "Todo", "::1");
    ticket.AssignUser(user2.Id);
    ticket.SetPriority(priorityId);
    ticket.SetTenantId(project.TenantId);
    db.Tickets.Add(ticket);
    await db.SaveChangesAsync();

    // Act
    var results = await repo.GetFilteredAsync(
        filterProject ? project.Id : null,
        filterAssignee ? user2.Id : null,
        filterCreator ? user1.Id : null);

    // Assert
    results.Should().Contain(t => t.Id == ticket.Id);
  }

  /// <summary>
  /// Tests GetDefaultWorkflowStateAsync fallback.
  /// </summary>
  /// <returns>A task.</returns>
  [Fact]
  public async Task TicketRepository_GetDefaultWorkflowStateAsync_Should_Return_Null_If_None()
  {
    // Arrange
    using var scope = this.Factory.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var repo = new TicketRepository(db);

    // Delete static seeded states to test fallback
    db.WorkflowStates.RemoveRange(db.WorkflowStates);
    await db.SaveChangesAsync();

    // Act
    var result = await repo.GetDefaultWorkflowStateAsync();

    // Assert
    result.Should().BeNull();
  }

  /// <summary>
  /// Tests CRUD on ProjectRepository.
  /// </summary>
  /// <returns>A task.</returns>
  [Fact]
  public async Task ProjectRepository_CRUD_Should_Work()
  {
    // Arrange
    using var scope = this.Factory.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var repo = new ProjectRepository(db);
    var tenantId = Guid.NewGuid();
    db.Organizations.Add(new Organization { Id = tenantId, Name = "Org", TenantId = tenantId });
    await db.SaveChangesAsync();

    var project = new Project("Initial", DateTime.UtcNow);
    project.SetTenantId(tenantId);

    // Act & Assert (Create)
    await repo.AddAsync(project);
    var saved = await repo.GetByIdAsync(project.Id);
    saved.Should().NotBeNull();

    // Update
    saved!.UpdateMetadata("Updated", "Desc");
    await repo.UpdateAsync(saved);
    var updated = await repo.GetByIdAsync(project.Id);
    updated!.Title.Should().Be("Updated");

    // GetAll
    var all = await repo.GetAllAsync(tenantId);
    all.Should().HaveCount(1);

    // Delete
    await repo.DeleteAsync(updated);
    (await repo.GetByIdAsync(project.Id)).Should().BeNull();
  }

  /// <summary>
  /// Tests CommentRepository retrieval.
  /// </summary>
  /// <returns>A task.</returns>
  [Fact]
  public async Task CommentRepository_Should_Return_Comments_For_Ticket()
  {
    // Arrange
    using var scope = this.Factory.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var repo = new CommentRepository(db);

    await SeedMinimalAsync(db);
    var projects = await db.Projects.ToListAsync();
    var project = projects[0];
    var user = await CreateTestUserAsync(db, "commenter", project.TenantId);
    var priorities = await db.TicketPriorities.ToListAsync();
    var priorityId = priorities[0].Id;
    var states = await db.WorkflowStates.ToListAsync();
    var stateId = states[0].Id;

    var ticket = new Ticket("Comment-Test", TicketType.Task, project.Id, user.Id, stateId, "Todo", "::1");
    ticket.SetPriority(priorityId);
    ticket.SetTenantId(project.TenantId);
    db.Tickets.Add(ticket);
    await db.SaveChangesAsync();

    var comment = new Comment("Content", ticket.Id, user.Id);
    comment.SetTenantId(project.TenantId);
    await repo.AddAsync(comment);
    await repo.SaveChangesAsync();

    // Act
    var comments = await repo.GetByTicketIdAsync(ticket.Id);

    // Assert
    comments.Should().HaveCount(1);
    comments[0].Content.Should().Be("Content");
  }

  private static async Task<User> CreateTestUserAsync(AppDbContext db, string name, Guid tenantId)
  {
    var roles = await db.Roles.ToListAsync();
    var roleId = roles[0].Id;
    var userId = Guid.NewGuid();
    var user = new User
    {
      Id = userId,
      UserName = $"{name}@example.com",
      NormalizedUserName = $"{name}@example.com".ToUpperInvariant(),
      Email = $"{name}@example.com",
      NormalizedEmail = $"{name}@example.com".ToUpperInvariant(),
      RoleId = roleId,
      TenantId = tenantId,
      SecurityStamp = Guid.NewGuid().ToString(),
    };
    user.Profile = new UserProfile { UserId = userId, FirstName = "Test", LastName = name, TenantId = tenantId };

    db.Users.Add(user);
    await db.SaveChangesAsync();
    return user;
  }
}
