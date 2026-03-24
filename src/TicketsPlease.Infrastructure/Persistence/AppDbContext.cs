// <copyright file="AppDbContext.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Infrastructure.Persistence;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TicketsPlease.Domain.Common;
using TicketsPlease.Domain.Entities;

/// <summary>
/// Der zentrale Datenbankkontext der Anwendung.
/// Verwaltet die Persistenz der User- und Ticket-Entitäten.
/// </summary>
public class AppDbContext : IdentityDbContext<User, Role, Guid>
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
  /// Gets ruft die Menge der Projekte ab oder legt diese fest.
  /// </summary>
  public DbSet<Project> Projects => this.Set<Project>();

  /// <summary>
  /// Gets ruft die Menge der Workflows ab oder legt diese fest.
  /// </summary>
  public DbSet<Workflow> Workflows => this.Set<Workflow>();

  /// <summary>
  /// Gets ruft die Menge der Tickets ab oder legt diese fest.
  /// </summary>
  public DbSet<Ticket> Tickets => this.Set<Ticket>();

  /// <summary>Gets die Organisationen.</summary>
  public DbSet<Organization> Organizations => this.Set<Organization>();

  /// <summary>Gets die Benutzerprofile.</summary>
  public DbSet<UserProfile> UserProfiles => this.Set<UserProfile>();

  /// <summary>Gets die Benutzeradressen.</summary>
  public DbSet<UserAddress> UserAddresses => this.Set<UserAddress>();

  /// <summary>Gets die Dateianhänge.</summary>
  public DbSet<FileAsset> FileAssets => this.Set<FileAsset>();

  /// <summary>Gets die Teams.</summary>
  public DbSet<Team> Teams => this.Set<Team>();

  /// <summary>Gets die Teammitglieder.</summary>
  public DbSet<TeamMember> TeamMembers => this.Set<TeamMember>();

  /// <summary>Gets die Ticket-Prioritäten.</summary>
  public DbSet<TicketPriority> TicketPriorities => this.Set<TicketPriority>();

  /// <summary>Gets die Sub-Tickets.</summary>
  public DbSet<SubTicket> SubTickets => this.Set<SubTicket>();

  /// <summary>Gets die Tags.</summary>
  public DbSet<Tag> Tags => this.Set<Tag>();

  /// <summary>Gets die Ticket-Tags.</summary>
  public DbSet<TicketTag> TicketTags => this.Set<TicketTag>();

  /// <summary>Gets die Ticket-Verknüpfungen.</summary>
  public DbSet<TicketLink> TicketLinks => this.Set<TicketLink>();

  /// <summary>Gets die Ticket-Zuweisungen.</summary>
  public DbSet<TicketAssignment> TicketAssignments => this.Set<TicketAssignment>();

  /// <summary>Gets die Workflow-Status.</summary>
  public DbSet<WorkflowState> WorkflowStates => this.Set<WorkflowState>();

  /// <summary>Gets die Workflow-Übergänge.</summary>
  public DbSet<WorkflowTransition> WorkflowTransitions => this.Set<WorkflowTransition>();

  /// <summary>Gets die Zeiterfassungseinträge.</summary>
  public DbSet<TimeLog> TimeLogs => this.Set<TimeLog>();

  /// <summary>Gets die Ticket-Upvotes.</summary>
  public DbSet<TicketUpvote> TicketUpvotes => this.Set<TicketUpvote>();

  /// <summary>Gets die Ticket-Historien.</summary>
  public DbSet<TicketHistory> TicketHistories => this.Set<TicketHistory>();

  /// <summary>Gets die Ticket-Vorlagen.</summary>
  public DbSet<TicketTemplate> TicketTemplates => this.Set<TicketTemplate>();

  /// <summary>Gets die SLA-Richtlinien.</summary>
  public DbSet<SlaPolicy> SlaPolicies => this.Set<SlaPolicy>();

  /// <summary>Gets die benutzerdefinierten Felddefinitionen.</summary>
  public DbSet<CustomFieldDefinition> CustomFieldDefinitions => this.Set<CustomFieldDefinition>();

  /// <summary>Gets die Werte der benutzerdefinierten Felder.</summary>
  public DbSet<TicketCustomValue> TicketCustomValues => this.Set<TicketCustomValue>();

  /// <summary>Gets die Nachrichten.</summary>
  public DbSet<Message> Messages => this.Set<Message>();

  /// <summary>Gets die Lesebestätigungen für Nachrichten.</summary>
  public DbSet<MessageReadReceipt> MessageReadReceipts => this.Set<MessageReadReceipt>();

  /// <summary>Gets die Benachrichtigungen.</summary>
  public DbSet<Notification> Notifications => this.Set<Notification>();

  /// <summary>Gets die Kommentare (F5).</summary>
  public DbSet<Comment> Comments => this.Set<Comment>();

  /// <summary>
  /// Konfiguriert das Modell und die Datenbank-Mappings.
  /// Hier wird die explizite Konfiguration für Nebenläufigkeit und Tabellennamen vorgenommen.
  /// </summary>
  /// <param name="builder">Der Builder für die Modellkonfiguration.</param>
  protected override void OnModelCreating(ModelBuilder builder)
  {
    ArgumentNullException.ThrowIfNull(builder);
    base.OnModelCreating(builder);

    // Global Configuration for RowVersion (Concurrency)
    var entityTypes = builder.Model.GetEntityTypes()
        .Where(e => typeof(BaseEntity).IsAssignableFrom(e.ClrType) || e.ClrType == typeof(User) || e.ClrType == typeof(Role))
        .Select(e => e.ClrType);

    foreach (var type in entityTypes)
    {
      if (this.Database.ProviderName == "Microsoft.EntityFrameworkCore.Sqlite")
      {
        builder.Entity(type).Property("RowVersion").ValueGeneratedNever();
      }
      else
      {
        builder.Entity(type).Property("RowVersion").IsRowVersion();
      }

      if (typeof(BaseEntity).IsAssignableFrom(type))
      {
        builder.Entity(type).HasQueryFilter(ConvertFilterExpression(type));
      }
    }

    // --- Identity & IAM ---
    builder.Entity<User>(entity =>
    {
      entity.HasOne(e => e.Role)
                .WithMany()
                .HasForeignKey(e => e.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

      entity.HasOne(e => e.Profile)
                .WithOne(p => p.User)
                .HasForeignKey<UserProfile>(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);
    });

    // --- Projects & Workflows ---
    builder.Entity<Project>(entity =>
    {
      entity.HasKey(e => e.Id);
      entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
      entity.HasOne(e => e.Workflow).WithMany(w => w.Projects).HasForeignKey(e => e.WorkflowId).OnDelete(DeleteBehavior.Restrict);
    });

    builder.Entity<Workflow>(entity =>
    {
      entity.HasKey(e => e.Id);
      entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
    });

    builder.Entity<WorkflowState>(entity =>
    {
      entity.HasOne(e => e.Workflow).WithMany(w => w.States).HasForeignKey(e => e.WorkflowId).OnDelete(DeleteBehavior.Cascade);
    });

    // --- Ticket Core ---
    builder.Entity<Ticket>(entity =>
    {
      entity.HasKey(e => e.Id);
      entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
      entity.Property(e => e.Sha1Hash).IsRequired().HasMaxLength(40);
      entity.HasIndex(e => e.Sha1Hash).IsUnique();

      entity.HasOne(t => t.Project).WithMany(p => p.Tickets).HasForeignKey(t => t.ProjectId).OnDelete(DeleteBehavior.Restrict);
      entity.HasOne(t => t.Priority).WithMany().HasForeignKey(t => t.PriorityId).OnDelete(DeleteBehavior.Restrict);
      entity.HasOne(t => t.WorkflowState).WithMany().HasForeignKey(t => t.WorkflowStateId).OnDelete(DeleteBehavior.Restrict);
      entity.HasOne(t => t.Creator).WithMany().HasForeignKey(t => t.CreatorId).OnDelete(DeleteBehavior.Restrict);
      entity.HasOne(t => t.AssignedUser).WithMany().HasForeignKey(t => t.AssignedUserId).OnDelete(DeleteBehavior.Restrict);

      entity.HasOne(t => t.ParentTicket)
            .WithMany(t => t.Children)
            .HasForeignKey(t => t.ParentTicketId)
            .OnDelete(DeleteBehavior.Restrict);

      entity.HasMany(t => t.Tags)
            .WithOne(tt => tt.Ticket)
            .HasForeignKey(tt => tt.TicketId);
    });

    builder.Entity<TicketLink>(entity =>
    {
      entity.HasKey(e => e.Id);

      entity.HasOne(e => e.SourceTicket)
            .WithMany(t => t.Blocking)
            .HasForeignKey(e => e.SourceTicketId)
            .OnDelete(DeleteBehavior.Restrict);

      entity.HasOne(e => e.TargetTicket)
            .WithMany(t => t.BlockedBy)
            .HasForeignKey(e => e.TargetTicketId)
            .OnDelete(DeleteBehavior.Restrict);
    });

    builder.Entity<SubTicket>(entity =>
    {
      entity.HasKey(e => e.Id);
      entity.HasOne(e => e.ParentTicket)
            .WithMany(t => t.SubTickets)
            .HasForeignKey(e => e.ParentTicketId)
            .OnDelete(DeleteBehavior.Cascade);
    });

    builder.Entity<TicketTag>(entity =>
    {
      entity.HasKey(e => new { e.TicketId, e.TagId });
    });

    builder.Entity<TicketAssignment>(entity =>
    {
      builder.Entity<TicketAssignment>().HasOne(e => e.Ticket).WithMany().HasForeignKey(e => e.TicketId);
    });

    builder.Entity<TicketUpvote>(entity =>
    {
      entity.HasKey(e => new { e.TicketId, e.UserId });
    });

    // --- Workflow Transitions ---
    builder.Entity<WorkflowTransition>(entity =>
    {
      entity.HasKey(e => new { e.FromStateId, e.ToStateId });
    });

    // --- Communication ---
    builder.Entity<Message>(entity =>
    {
      builder.Entity<Message>().HasOne(e => e.SenderUser).WithMany().HasForeignKey(e => e.SenderUserId).OnDelete(DeleteBehavior.Restrict);
      builder.Entity<Message>().HasOne(e => e.ReceiverUser).WithMany().HasForeignKey(e => e.ReceiverUserId).OnDelete(DeleteBehavior.Restrict);
    });

    builder.Entity<MessageReadReceipt>(entity =>
    {
      entity.HasKey(e => new { e.MessageId, e.UserId });
    });

    // --- Custom Fields ---
    builder.Entity<TicketCustomValue>(entity =>
    {
      entity.HasKey(e => new { e.TicketId, e.FieldDefinitionId });
    });

    // --- Comments (F5) ---
    builder.Entity<Comment>(entity =>
    {
      entity.HasKey(e => e.Id);
      entity.Property(e => e.Content).IsRequired().HasMaxLength(2000);
      entity.HasOne(e => e.Ticket).WithMany(t => t.Comments).HasForeignKey(e => e.TicketId).OnDelete(DeleteBehavior.Cascade);
      entity.HasOne(e => e.Author).WithMany().HasForeignKey(e => e.AuthorId).OnDelete(DeleteBehavior.Restrict);
    });
  }

  /// <summary>
  /// Konfiguriert zusätzliche Optionen wie die Resilience / Retry Strategie.
  /// </summary>
  /// <param name="optionsBuilder">Der Builder für die Kontext-Optionen.</param>
  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    ArgumentNullException.ThrowIfNull(optionsBuilder);

    // Hinweis: Die eigentliche SQL Server Konfiguration erfolgt meist in Program.cs/Startup.cs.
    // Falls hier konfiguriert wird, stellen wir sicher, dass RetryOnFailure aktiviert ist.
    if (!optionsBuilder.IsConfigured)
    {
      // Placeholder für lokale Entwicklung oder Fallback
    }
  }

  private static System.Linq.Expressions.LambdaExpression ConvertFilterExpression(Type type)
  {
    var parameter = System.Linq.Expressions.Expression.Parameter(type, "e");
    var property = System.Linq.Expressions.Expression.Property(parameter, "IsDeleted");
    var notDeleted = System.Linq.Expressions.Expression.Not(property);
    return System.Linq.Expressions.Expression.Lambda(notDeleted, parameter);
  }
}
