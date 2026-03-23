// <copyright file="AppDbContext.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TicketsPlease.Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;
using TicketsPlease.Domain.Common;
using TicketsPlease.Domain.Entities;

/// <summary>
/// Der zentrale Datenbankkontext der Anwendung.
/// Verwaltet die Persistenz der User- und Ticket-Entitäten.
/// </summary>
public class AppDbContext : DbContext
{
  /// <summary>
  /// Initializes a new instance of the <see cref="AppDbContext"/> class.
  /// Initialisiert eine neue Instanz von <see cref="AppDbContext"/> mit den angegebenen Optionen.
  /// </summary>
  /// <param name="options">Die Optionen für diesen Kontext.</param>
  public AppDbContext(DbContextOptions<AppDbContext> options)
    : base(options)
  {
  }

  /// <summary>
  /// Gets ruft die Menge der Benutzer ab oder legt diese fest.
  /// </summary>
  public DbSet<User> Users => this.Set<User>();

  /// <summary>
  /// Gets ruft die Menge der Tickets ab oder legt diese fest.
  /// </summary>
  public DbSet<Ticket> Tickets => this.Set<Ticket>();

  public DbSet<Organization> Organizations => this.Set<Organization>();

  public DbSet<Role> Roles => this.Set<Role>();

  public DbSet<UserProfile> UserProfiles => this.Set<UserProfile>();

  public DbSet<UserAddress> UserAddresses => this.Set<UserAddress>();

  public DbSet<FileAsset> FileAssets => this.Set<FileAsset>();

  public DbSet<Team> Teams => this.Set<Team>();

  public DbSet<TeamMember> TeamMembers => this.Set<TeamMember>();

  public DbSet<TicketPriority> TicketPriorities => this.Set<TicketPriority>();

  public DbSet<SubTicket> SubTickets => this.Set<SubTicket>();

  public DbSet<Tag> Tags => this.Set<Tag>();

  public DbSet<TicketTag> TicketTags => this.Set<TicketTag>();

  public DbSet<TicketAssignment> TicketAssignments => this.Set<TicketAssignment>();

  public DbSet<WorkflowState> WorkflowStates => this.Set<WorkflowState>();

  public DbSet<WorkflowTransition> WorkflowTransitions => this.Set<WorkflowTransition>();

  public DbSet<TimeLog> TimeLogs => this.Set<TimeLog>();

  public DbSet<TicketUpvote> TicketUpvotes => this.Set<TicketUpvote>();

  public DbSet<TicketHistory> TicketHistories => this.Set<TicketHistory>();

  public DbSet<TicketTemplate> TicketTemplates => this.Set<TicketTemplate>();

  public DbSet<SlaPolicy> SlaPolicies => this.Set<SlaPolicy>();

  public DbSet<CustomFieldDefinition> CustomFieldDefinitions => this.Set<CustomFieldDefinition>();

  public DbSet<TicketCustomValue> TicketCustomValues => this.Set<TicketCustomValue>();

  public DbSet<Message> Messages => this.Set<Message>();

  public DbSet<MessageReadReceipt> MessageReadReceipts => this.Set<MessageReadReceipt>();

  public DbSet<Notification> Notifications => this.Set<Notification>();

  /// <summary>
  /// Konfiguriert das Modell und die Datenbank-Mappings.
  /// Hier wird die explizite Konfiguration für Nebenläufigkeit und Tabellennamen vorgenommen.
  /// </summary>
  /// <param name="modelBuilder">Der Builder für die Modellkonfiguration.</param>
  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);

    // Global Configuration for RowVersion (Concurrency)
    foreach (var entityType in modelBuilder.Model.GetEntityTypes())
    {
      if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
      {
        modelBuilder.Entity(entityType.ClrType).Property("RowVersion").IsRowVersion();
        modelBuilder.Entity(entityType.ClrType).HasQueryFilter(ConvertFilterExpression(entityType.ClrType));
      }
    }

    // --- Identity & IAM ---
    modelBuilder.Entity<User>(entity =>
    {
      entity.HasKey(e => e.Id);
      entity.Property(e => e.DisplayName).IsRequired().HasMaxLength(100);
      entity.Property(e => e.Username).IsRequired().HasMaxLength(50);
      entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
      entity.HasIndex(e => e.Username).IsUnique();
      entity.HasIndex(e => e.Email).IsUnique();

      entity.HasOne(e => e.Role)
                .WithMany()
                .HasForeignKey(e => e.RoleId)
                .OnDelete(DeleteBehavior.Restrict);
    });

    modelBuilder.Entity<UserProfile>(entity =>
    {
      entity.HasKey(e => e.UserId);
      entity.HasOne(e => e.User).WithOne().HasForeignKey<UserProfile>(e => e.UserId);
      entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
    });

    modelBuilder.Entity<UserAddress>(entity =>
    {
      entity.HasKey(e => e.UserId);
      entity.HasOne(e => e.User).WithOne().HasForeignKey<UserAddress>(e => e.UserId);
    });

    // --- Teams ---
    modelBuilder.Entity<TeamMember>(entity =>
    {
      entity.HasKey(e => new { e.TeamId, e.UserId });
      entity.HasOne(e => e.Team).WithMany().HasForeignKey(e => e.TeamId);
      entity.HasOne(e => e.User).WithMany().HasForeignKey(e => e.UserId);
    });

    // --- Ticket Core ---
    modelBuilder.Entity<Ticket>(entity =>
    {
      entity.HasKey(e => e.Id);
      entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
      entity.Property(e => e.Sha1Hash).IsRequired().HasMaxLength(40);
      entity.HasIndex(e => e.Sha1Hash).IsUnique();

      entity.HasOne(t => t.Priority).WithMany().HasForeignKey(t => t.PriorityId).OnDelete(DeleteBehavior.Restrict);
      entity.HasOne(t => t.WorkflowState).WithMany().HasForeignKey(t => t.WorkflowStateId).OnDelete(DeleteBehavior.Restrict);
      entity.HasOne(t => t.Creator).WithMany().HasForeignKey(t => t.CreatorId).OnDelete(DeleteBehavior.Restrict);
      entity.HasOne(t => t.AssignedUser).WithMany().HasForeignKey(t => t.AssignedUserId).OnDelete(DeleteBehavior.Restrict);
    });

    modelBuilder.Entity<TicketTag>(entity =>
    {
      entity.HasKey(e => new { e.TicketId, e.TagId });
    });

    modelBuilder.Entity<TicketAssignment>(entity =>
    {
      entity.HasOne(e => e.Ticket).WithMany().HasForeignKey(e => e.TicketId);
    });

    modelBuilder.Entity<TicketUpvote>(entity =>
    {
      entity.HasKey(e => new { e.TicketId, e.UserId });
    });

    // --- Workflow ---
    modelBuilder.Entity<WorkflowTransition>(entity =>
    {
      entity.HasKey(e => new { e.FromStateId, e.ToStateId });
    });

    // --- Communication ---
    modelBuilder.Entity<Message>(entity =>
    {
      entity.HasOne(e => e.SenderUser).WithMany().HasForeignKey(e => e.SenderUserId).OnDelete(DeleteBehavior.Restrict);
      entity.HasOne(e => e.ReceiverUser).WithMany().HasForeignKey(e => e.ReceiverUserId).OnDelete(DeleteBehavior.Restrict);
    });

    modelBuilder.Entity<MessageReadReceipt>(entity =>
    {
      entity.HasKey(e => new { e.MessageId, e.UserId });
    });

    // --- Custom Fields ---
    modelBuilder.Entity<TicketCustomValue>(entity =>
    {
      entity.HasKey(e => new { e.TicketId, e.FieldDefinitionId });
    });
  }

  private static System.Linq.Expressions.LambdaExpression ConvertFilterExpression(Type type)
  {
    var parameter = System.Linq.Expressions.Expression.Parameter(type, "e");
    var property = System.Linq.Expressions.Expression.Property(parameter, "IsDeleted");
    var notDeleted = System.Linq.Expressions.Expression.Not(property);
    return System.Linq.Expressions.Expression.Lambda(notDeleted, parameter);
  }

  /// <summary>
  /// Konfiguriert zusätzliche Optionen wie die Resilience / Retry Strategie.
  /// </summary>
  /// <param name="optionsBuilder">Der Builder für die Kontext-Optionen.</param>
  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    // Hinweis: Die eigentliche SQL Server Konfiguration erfolgt meist in Program.cs/Startup.cs.
    // Falls hier konfiguriert wird, stellen wir sicher, dass RetryOnFailure aktiviert ist.
    if (!optionsBuilder.IsConfigured)
    {
      // Placeholder für lokale Entwicklung oder Fallback
      // optionsBuilder.UseSqlServer("fallback_connection_string",
      //    sqlOptions => sqlOptions.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null));
    }
  }
}
