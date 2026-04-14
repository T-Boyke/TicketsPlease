// <copyright file="AppDbContext.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Infrastructure.Persistence;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using TicketsPlease.Domain.Common;
using TicketsPlease.Domain.Entities;

/// <summary>
/// Der zentrale Datenbankkontext der Anwendung.
/// Verwaltet die Persistenz der User- und Ticket-Entitäten.
/// </summary>
public class AppDbContext : IdentityDbContext<User, Role, Guid>
{
  private readonly IHttpContextAccessor? httpContextAccessor;

  /// <summary>
  /// Initializes a new instance of the <see cref="AppDbContext"/> class.
  /// Initialisiert eine neue Instanz von <see cref="AppDbContext"/> mit den angegebenen Optionen.
  /// </summary>
  /// <param name="options">Die Optionen für diesen Kontext.</param>
  /// <param name="httpContextAccessor">Der HttpContextAccessor für den Zugriff auf den aktuellen Benutzer.</param>
  public AppDbContext(DbContextOptions<AppDbContext> options, IHttpContextAccessor? httpContextAccessor = null)
    : base(options)
  {
    this.httpContextAccessor = httpContextAccessor;
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

  /// <summary>Gets die Beitrittsanfragen für Teams.</summary>
  public DbSet<TeamJoinRequest> TeamJoinRequests => this.Set<TeamJoinRequest>();

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

  /// <summary>Gets die Organisationseinladungen.</summary>
  public DbSet<OrganizationInvite> OrganizationInvites => this.Set<OrganizationInvite>();

  /// <summary>Gets die Governance-Audit-Logs.</summary>
  public DbSet<AuditLog> AuditLogs => this.Set<AuditLog>();

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
        var method = typeof(AppDbContext)
          .GetMethod(nameof(this.ApplyGlobalFilter), System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
          ?.MakeGenericMethod(type);
        method?.Invoke(this, new object[] { builder });
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
      entity.Property(e => e.DomainHash).IsRequired().HasMaxLength(64);
      entity.HasIndex(e => e.DomainHash).IsUnique();

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
      entity.HasOne(e => e.Ticket)
            .WithMany(t => t.SubTickets)
            .HasForeignKey(e => e.TicketId)
            .OnDelete(DeleteBehavior.Restrict);

      entity.HasOne(e => e.Creator)
            .WithMany()
            .HasForeignKey(e => e.CreatorId)
            .OnDelete(DeleteBehavior.Restrict);
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

    // --- Teams & Members ---
    builder.Entity<Team>(entity =>
    {
      entity.HasKey(e => e.Id);
      entity.HasOne(e => e.CreatedByUser)
            .WithMany()
            .HasForeignKey(e => e.CreatedByUserId)
            .OnDelete(DeleteBehavior.Restrict);
    });

    builder.Entity<TeamMember>(entity =>
    {
      entity.HasKey(e => e.Id);
      entity.HasOne(e => e.Team)
            .WithMany(t => t.Members)
            .HasForeignKey(e => e.TeamId)
            .OnDelete(DeleteBehavior.Restrict);

      entity.HasOne(e => e.User)
            .WithMany()
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    });

    builder.Entity<TeamJoinRequest>(entity =>
    {
      entity.HasKey(e => e.Id);
      entity.HasOne(e => e.Team).WithMany().HasForeignKey(e => e.TeamId).OnDelete(DeleteBehavior.Cascade);
      entity.HasOne(e => e.User).WithMany().HasForeignKey(e => e.UserId).OnDelete(DeleteBehavior.Restrict);
      entity.HasOne(e => e.DecidedByUser).WithMany().HasForeignKey(e => e.DecidedByUserId).OnDelete(DeleteBehavior.Restrict);
    });

    builder.Entity<TimeLog>(entity =>
    {
      entity.HasKey(e => e.Id);
      entity.Property(e => e.HoursLogged).HasPrecision(18, 2);
    });

    // --- Workflow Transitions ---
    builder.Entity<WorkflowTransition>(entity =>
    {
      entity.HasKey(e => e.Id);

      entity.HasOne(e => e.FromState)
            .WithMany()
            .HasForeignKey(e => e.FromStateId)
            .OnDelete(DeleteBehavior.Restrict);

      entity.HasOne(e => e.ToState)
            .WithMany()
            .HasForeignKey(e => e.ToStateId)
            .OnDelete(DeleteBehavior.Restrict);
    });

    // --- Communication ---
    builder.Entity<Message>(entity =>
    {
      builder.Entity<Message>().HasOne(e => e.SenderUser).WithMany().HasForeignKey(e => e.SenderUserId).OnDelete(DeleteBehavior.Restrict);
      builder.Entity<Message>().HasOne(e => e.ReceiverUser).WithMany().HasForeignKey(e => e.ReceiverUserId).OnDelete(DeleteBehavior.Restrict);

      entity.HasMany(m => m.Attachments)
            .WithOne(a => a.Message)
            .HasForeignKey(a => a.MessageId)
            .OnDelete(DeleteBehavior.Cascade);
    });

    builder.Entity<FileAsset>(entity =>
    {
      entity.HasOne(a => a.Ticket)
            .WithMany(t => t.Attachments)
            .HasForeignKey(a => a.TicketId)
            .OnDelete(DeleteBehavior.Cascade);
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
      entity.HasOne(e => e.Ticket).WithMany(t => t.Comments).HasForeignKey(e => e.TicketId).OnDelete(DeleteBehavior.Restrict);
      entity.HasOne(e => e.Author).WithMany().HasForeignKey(e => e.AuthorId).OnDelete(DeleteBehavior.Restrict);
    });

    // --- Governance & Audit ---
    builder.Entity<AuditLog>(entity =>
    {
      entity.HasKey(e => e.Id);
      entity.HasOne(e => e.Organization)
            .WithMany()
            .HasForeignKey(e => e.OrganizationId)
            .OnDelete(DeleteBehavior.Cascade);
    });

    SeedStaticData(builder);
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

  private static void SeedStaticData(ModelBuilder builder)
  {
    // Fixe IDs für stabiles Seeding und Referenzierung
    var adminRoleId = new Guid("32d733e1-4c7a-4c2d-9b51-1e9a7e6b7d21");
    var teamleadRoleId = new Guid("b8f2e9d2-6c8a-4d3e-ac62-2f0b8f7c8e33");
    var userRoleId = new Guid("c903f0e3-7d9b-4e4f-bd73-3f1c908d9f44");
    var productOwnerRoleId = new Guid("d01401f4-8e0c-4f50-ce84-402d019e0066");
    var stakeholderRoleId = new Guid("e12512a5-9f1d-5061-df95-513e12af1177");

    var lowPriorityId = new Guid("d01401f4-8e0c-4f50-ce84-402d019e0055");
    var mediumPriorityId = new Guid("e12512a5-9f1d-5061-df95-513e12af1166");
    var highPriorityId = new Guid("f23623b6-af2e-5172-ef06-624f23b02277");
    var blockerPriorityId = new Guid("034734c7-b03f-5283-f017-735034c13388");

    var workflowId = new Guid("145845d8-c140-5394-0128-846145d24499");
    var todoStateId = new Guid("256956e9-d251-54a5-1239-957256e35500");
    var inProgressStateId = new Guid("367a67fa-e362-55b6-234a-a68367f46611");
    var inReviewStateId = new Guid("478b780b-f473-56c7-345b-b79478057722");
    var doneStateId = new Guid("589c891c-0584-57d8-456c-c8a589168833");

    // 1. Roles
    builder.Entity<Role>().HasData(
      new Role { Id = adminRoleId, Name = "Admin", NormalizedName = "ADMIN", ConcurrencyStamp = "32d733e1-4c7a-4c2d-9b51-1e9a7e6b7d21", Description = "Full system access" },
      new Role { Id = teamleadRoleId, Name = "Teamlead", NormalizedName = "TEAMLEAD", ConcurrencyStamp = "b8f2e9d2-6c8a-4d3e-ac62-2f0b8f7c8e33", Description = "Team lead permissions" },
      new Role { Id = userRoleId, Name = "User", NormalizedName = "USER", ConcurrencyStamp = "c903f0e3-7d9b-4e4f-bd73-3f1c908d9f44", Description = "Standard user access" },
      new Role { Id = productOwnerRoleId, Name = "ProductOwner", NormalizedName = "PRODUCTOWNER", ConcurrencyStamp = "d01401f4-8e0c-4f50-ce84-402d019e0066", Description = "Highest local authority within a company" },
      new Role { Id = stakeholderRoleId, Name = "Stakeholder", NormalizedName = "STAKEHOLDER", ConcurrencyStamp = "e12512a5-9f1d-5061-df95-513e12af1177", Description = "Read-only reporting access" });

    // 2. Priorities
    builder.Entity<TicketPriority>().HasData(
      new TicketPriority { Id = lowPriorityId, Name = "Low", LevelWeight = 1, ColorHex = "#808080" },
      new TicketPriority { Id = mediumPriorityId, Name = "Medium", LevelWeight = 2, ColorHex = "#0000FF" },
      new TicketPriority { Id = highPriorityId, Name = "High", LevelWeight = 3, ColorHex = "#FFA500" },
      new TicketPriority { Id = blockerPriorityId, Name = "Blocker", LevelWeight = 4, ColorHex = "#FF0000" });

    // 3. Workflow & States
    builder.Entity<Workflow>().HasData(
      new Workflow { Id = workflowId, Name = "Standard Workflow" });

    builder.Entity<WorkflowState>().HasData(
      new WorkflowState { Id = todoStateId, Name = "Todo", OrderIndex = 0, ColorHex = "#D3D3D3", WorkflowId = workflowId },
      new WorkflowState { Id = inProgressStateId, Name = "In Progress", OrderIndex = 1, ColorHex = "#ADD8E6", WorkflowId = workflowId },
      new WorkflowState { Id = inReviewStateId, Name = "In Review", OrderIndex = 2, ColorHex = "#FFFFE0", WorkflowId = workflowId },
      new WorkflowState { Id = doneStateId, Name = "Done", OrderIndex = 3, ColorHex = "#90EE90", IsTerminalState = true, WorkflowId = workflowId });
  }

  private void ApplyGlobalFilter<TEntity>(ModelBuilder builder)
      where TEntity : BaseEntity
  {
    builder.Entity<TEntity>().HasQueryFilter(e => !e.IsDeleted && (this.IsAdmin || this.IsInternalTest || e.TenantId == this.CurrentTenantId));
  }

  /// <summary>
  /// Gets a value indicating whether the current user is an admin.
  /// </summary>
  public bool IsAdmin => this.httpContextAccessor?.HttpContext?.User?.IsInRole("Admin") ?? false;

  /// <summary>
  /// Gets a value indicating whether the current context is an internal test.
  /// </summary>
  public bool IsInternalTest => this.httpContextAccessor?.HttpContext?.User?.Identity?.AuthenticationType is "Test" or "TestServer" or "IntegrationTest";

  /// <summary>
  /// Gets the current tenant ID.
  /// </summary>
  public Guid CurrentTenantId
  {
    get
    {
      var tenantClaim = this.httpContextAccessor?.HttpContext?.User?.FindFirst("TenantId")?.Value;
      return Guid.TryParse(tenantClaim, out var parsedId) ? parsedId : Guid.Empty;
    }
  }
}
