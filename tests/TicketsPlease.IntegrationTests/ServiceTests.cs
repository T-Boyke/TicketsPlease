// <copyright file="ServiceTests.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.IntegrationTests;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using TicketsPlease.Application.Common.Dtos;
using TicketsPlease.Application.Services;
using TicketsPlease.Domain.Entities;
using TicketsPlease.Domain.Enums;
using TicketsPlease.Infrastructure.Persistence;
using TicketsPlease.Infrastructure.Repositories;
using Xunit;

/// <summary>
/// Integrations-Tests für die Application-Services.
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores", Justification = "Test naming convention")]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Reliability", "CA2007:Consider calling ConfigureAwait", Justification = "xUnit tests should not use ConfigureAwait(false)")]
public class ServiceTests : IntegrationTestBase
{
  /// <summary>
  /// Tests CreateTicketAsync.
  /// </summary>
  /// <returns>A task.</returns>
  [Fact]
  public async Task TicketService_CreateTicketAsync_Should_Work_For_Authenticated_User()
  {
    // Arrange
    using var scope = this.Factory.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await SeedMinimalAsync(db);

    var projectsList = await db.Projects.ToListAsync();
    var project = projectsList[0];
    var prioritiesList = await db.TicketPriorities.ToListAsync();
    var priorityId = prioritiesList[0].Id;
    var user = await CreateTestUserWithClaimsAsync(db, "serviceuser", project.TenantId);

    var mockAccessor = new Mock<IHttpContextAccessor>();
    mockAccessor.Setup(x => x.HttpContext!.User).Returns(new ClaimsPrincipal(new ClaimsIdentity(
        new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName!),
        },
        "TestAuth")));

    var ticketRepo = new TicketRepository(db);
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
    var service = new TicketService(ticketRepo, userManager, mockAccessor.Object);

    var dto = new CreateTicketDto(
        "Service Created",
        "Description",
        project.Id,
        priorityId,
        user.Id,
        5);

    // Act
    await service.CreateTicketAsync(dto);

    // Assert
    var created = await db.Tickets.FirstOrDefaultAsync(t => t.Title == "Service Created");
    created.Should().NotBeNull();
    created!.CreatorId.Should().Be(user.Id);
    created.EstimatePoints.Should().Be(5);
  }

  /// <summary>
  /// Tests UpdateTicketAsync failure.
  /// </summary>
  /// <returns>A task.</returns>
  [Fact]
  public async Task TicketService_UpdateTicketAsync_Should_Throw_If_Not_Found()
  {
    // Arrange
    using var scope = this.Factory.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var service = new TicketService(new TicketRepository(db), scope.ServiceProvider.GetRequiredService<UserManager<User>>(), new Mock<IHttpContextAccessor>().Object);

    var dto = new UpdateTicketDto(Guid.NewGuid(), "Title", "Desc", "Todo", Guid.NewGuid(), null, null);

    // Act
    var act = () => service.UpdateTicketAsync(dto);

    // Assert
    await act.Should().ThrowAsync<KeyNotFoundException>();
  }

  /// <summary>
  /// Tests CloseTicketAsync dependency check.
  /// </summary>
  /// <returns>A task.</returns>
  [Fact]
  public async Task TicketService_CloseTicketAsync_Should_Throw_If_Has_Dependencies()
  {
    // Arrange
    using var scope = this.Factory.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await SeedMinimalAsync(db);
    var projectsList = await db.Projects.ToListAsync();
    var project = projectsList[0];
    var user = await CreateTestUserWithClaimsAsync(db, "closer", project.TenantId);
    var statesList = await db.WorkflowStates.ToListAsync();
    var stateId = statesList[0].Id;
    var priorityId = (await db.TicketPriorities.ToListAsync())[0].Id;

    var t1 = new Ticket("Blocker", TicketType.Task, project.Id, user.Id, stateId, "::1");
    t1.SetTenantId(project.TenantId);
    t1.SetPriority(priorityId);
    var t2 = new Ticket("Blocked", TicketType.Task, project.Id, user.Id, stateId, "::1");
    t2.SetTenantId(project.TenantId);
    t2.SetPriority(priorityId);
    db.Tickets.AddRange(t1, t2);
    await db.SaveChangesAsync();

    var link = new TicketLink(t1.Id, t2.Id, TicketLinkType.Blocks);
    link.TenantId = project.TenantId;
    db.TicketLinks.Add(link);
    await db.SaveChangesAsync();

    var mockAccessor = new Mock<IHttpContextAccessor>();
    mockAccessor.Setup(x => x.HttpContext!.User).Returns(new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()) })));

    var service = new TicketService(new TicketRepository(db), scope.ServiceProvider.GetRequiredService<UserManager<User>>(), mockAccessor.Object);

    // Act
    var act = () => service.CloseTicketAsync(t2.Id);

    // Assert
    await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("*Abhängigkeiten*");
  }

  /// <summary>
  /// Tests MoveTicketAsync.
  /// </summary>
  /// <returns>A task.</returns>
  [Fact]
  public async Task TicketService_MoveTicketAsync_Should_Update_State()
  {
    // Arrange
    using var scope = this.Factory.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await SeedMinimalAsync(db);
    var projectsList = await db.Projects.ToListAsync();
    var project = projectsList[0];
    var user = await CreateTestUserWithClaimsAsync(db, "mover", project.TenantId);
    var states = await db.WorkflowStates.ToListAsync();
    var s1 = states[0];
    var s2 = states[1];
    var priorityId = (await db.TicketPriorities.ToListAsync())[0].Id;

    var ticket = new Ticket("To Move", TicketType.Task, project.Id, user.Id, s1.Id, "::1");
    ticket.SetTenantId(project.TenantId);
    ticket.SetPriority(priorityId);
    db.Tickets.Add(ticket);
    await db.SaveChangesAsync();

    var mockAccessor = new Mock<IHttpContextAccessor>();
    mockAccessor.Setup(x => x.HttpContext!.User).Returns(new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()) })));

    var service = new TicketService(new TicketRepository(db), scope.ServiceProvider.GetRequiredService<UserManager<User>>(), mockAccessor.Object);

    // Act
    await service.MoveTicketAsync(ticket.Id, s2.Name);

    // Assert
    var updated = await db.Tickets.FindAsync(ticket.Id);
    updated!.WorkflowStateId.Should().Be(s2.Id);
  }

  /// <summary>
  /// Tests dependency removal.
  /// </summary>
  /// <returns>A task.</returns>
  [Fact]
  public async Task TicketService_RemoveDependency_Should_Work()
  {
    // Arrange
    using var scope = this.Factory.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await SeedMinimalAsync(db);
    var projectsList = await db.Projects.ToListAsync();
    var project = projectsList[0];
    var user = await CreateTestUserWithClaimsAsync(db, "remover", project.TenantId);
    var statesList = await db.WorkflowStates.ToListAsync();
    var stateId = statesList[0].Id;
    var priorityId = (await db.TicketPriorities.ToListAsync())[0].Id;

    var t1 = new Ticket("T1", TicketType.Task, project.Id, user.Id, stateId, "::1");
    t1.SetTenantId(project.TenantId);
    t1.SetPriority(priorityId);
    var t2 = new Ticket("T2", TicketType.Task, project.Id, user.Id, stateId, "::1");
    t2.SetTenantId(project.TenantId);
    t2.SetPriority(priorityId);
    db.Tickets.AddRange(t1, t2);
    await db.SaveChangesAsync();

    var link = new TicketLink(t1.Id, t2.Id, TicketLinkType.Blocks);
    link.TenantId = project.TenantId;
    db.TicketLinks.Add(link);
    await db.SaveChangesAsync();

    var service = new TicketService(new TicketRepository(db), scope.ServiceProvider.GetRequiredService<UserManager<User>>(), new Mock<IHttpContextAccessor>().Object);

    // Act & Assert (Remove as BlockedBy)
    await service.RemoveDependencyAsync(t2.Id, link.Id);
    (await db.TicketLinks.AnyAsync(l => l.SourceTicketId == t1.Id && l.TargetTicketId == t2.Id)).Should().BeFalse();

    // Re-add and remove as Blocking
    var newLink = new TicketLink(t1.Id, t2.Id, TicketLinkType.Blocks);
    newLink.TenantId = project.TenantId;
    db.TicketLinks.Add(newLink);
    await db.SaveChangesAsync();
    await service.RemoveDependencyAsync(t1.Id, newLink.Id);
    (await db.TicketLinks.AnyAsync(l => l.SourceTicketId == t1.Id && l.TargetTicketId == t2.Id)).Should().BeFalse();
  }

  /// <summary>
  /// Tests ProjectService multi-tenancy.
  /// </summary>
  /// <returns>A task.</returns>
  [Fact]
  public async Task ProjectService_MultiTenancy_Should_Isolate_Projects()
  {
    // Arrange
    using var scope = this.Factory.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    var t1 = Guid.NewGuid();
    var t2 = Guid.NewGuid();
    db.Organizations.AddRange(new Organization { Id = t1, Name = "O1", TenantId = t1 }, new Organization { Id = t2, Name = "O2", TenantId = t2 });
    var p1 = new Project("P1", DateTime.UtcNow);
    p1.SetTenantId(t1);
    var p2 = new Project("P2", DateTime.UtcNow);
    p2.SetTenantId(t2);
    db.Projects.AddRange(p1, p2);

    var u1 = await CreateTestUserWithClaimsAsync(db, "u1", t1);
    await db.SaveChangesAsync();

    var mockAccessor = new Mock<IHttpContextAccessor>();
    mockAccessor.Setup(x => x.HttpContext!.User).Returns(new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, u1.Id.ToString()) })));

    var service = new ProjectService(new ProjectRepository(db), scope.ServiceProvider.GetRequiredService<UserManager<User>>(), mockAccessor.Object);

    // Act
    var projects = await service.GetProjectsAsync();

    // Assert
    projects.Should().ContainSingle(p => p.Title == "P1");
    projects.OrderBy(p => p.Title).Select(p => p.Title).Should().NotContain("P2");
  }

  /// <summary>
  /// Tests CommentService creation.
  /// </summary>
  /// <returns>A task.</returns>
  [Fact]
  public async Task CommentService_CreateCommentAsync_Should_Link_Author()
  {
    // Arrange
    using var scope = this.Factory.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await SeedMinimalAsync(db);
    var projectsList = await db.Projects.ToListAsync();
    var project = projectsList[0];
    var user = await CreateTestUserWithClaimsAsync(db, "commenter", project.TenantId);
    var statesList = await db.WorkflowStates.ToListAsync();
    var stateId = statesList[0].Id;
    var priorityId = (await db.TicketPriorities.ToListAsync())[0].Id;

    var ticket = new Ticket("T", TicketType.Task, project.Id, user.Id, stateId, "::1");
    ticket.SetTenantId(project.TenantId);
    ticket.SetPriority(priorityId);
    db.Tickets.Add(ticket);
    await db.SaveChangesAsync();

    var mockAccessor = new Mock<IHttpContextAccessor>();
    mockAccessor.Setup(x => x.HttpContext!.User).Returns(new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()) })));

    var service = new CommentService(new CommentRepository(db), scope.ServiceProvider.GetRequiredService<UserManager<User>>(), mockAccessor.Object);

    // Act
    await service.CreateCommentAsync(new CreateCommentDto(ticket.Id, "Comment Content"));

    // Assert
    var comment = await db.Comments.Include(c => c.Author).FirstOrDefaultAsync(c => c.TicketId == ticket.Id);
    comment.Should().NotBeNull();
    comment!.Author.Should().NotBeNull();
    comment.Author!.UserName.Should().Be(user.UserName);
  }

  private static async Task<User> CreateTestUserWithClaimsAsync(AppDbContext db, string name, Guid tenantId)
  {
    var rolesList = await db.Roles.ToListAsync();
    var roleId = rolesList[0].Id;
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
