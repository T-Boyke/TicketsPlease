// <copyright file="TicketService.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Application.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using TicketsPlease.Application.Common.Dtos;
using TicketsPlease.Application.Common.Interfaces;
using TicketsPlease.Domain.Entities;

/// <summary>
/// Implementierung des Ticket-Services zur Steuerung des Kanban-Boards.
/// </summary>
public class TicketService : ITicketService
{
  private const string TicketNotFoundMessage = "Ticket nicht gefunden.";

  private readonly ITicketRepository ticketRepository;
  private readonly UserManager<User> userManager;
  private readonly IHttpContextAccessor httpContextAccessor;
  private readonly IFileStorageService fileStorageService;
  private readonly IFileAssetRepository fileAssetRepository;
  private readonly RoleManager<Role> roleManager;
  private readonly ITimeTrackingService timeTrackingService;
  private readonly ISubTicketService subTicketService;

  /// <summary>
  /// Initializes a new instance of the <see cref="TicketService"/> class.
  /// </summary>
  /// <param name="projectService">Der Dienst für Projekte.</param>
  /// <param name="fileAssetRepository">Das Repository für Datei-Metadaten.</param>
  /// <param name="fileStorageService">Der Dienst zur Dateispeicherung.</param>
  /// <param name="timeTrackingService">Der Dienst für Zeiterfassung.</param>
  /// <param name="subTicketService">Der Dienst für Untertickets.</param>
  public TicketService(
      ITicketRepository ticketRepository,
      UserManager<User> userManager,
      RoleManager<Role> roleManager,
      IHttpContextAccessor httpContextAccessor,
      IProjectService projectService,
      IFileAssetRepository fileAssetRepository,
      IFileStorageService fileStorageService,
      ITimeTrackingService timeTrackingService,
      ISubTicketService subTicketService)
  {
    this.ticketRepository = ticketRepository;
    this.userManager = userManager;
    this.roleManager = roleManager;
    this.httpContextAccessor = httpContextAccessor;
    this.fileStorageService = fileStorageService;
    this.fileAssetRepository = fileAssetRepository;
    this.timeTrackingService = timeTrackingService;
    this.subTicketService = subTicketService;
  }

  /// <inheritdoc/>
  public async Task<IEnumerable<TicketDto>> GetActiveTicketsAsync()
  {
    var tickets = await this.ticketRepository.GetAllActiveAsync().ConfigureAwait(false);
    return await Task.WhenAll(tickets.Select(t => this.MapToDtoAsync(t))).ConfigureAwait(false);
  }

  /// <inheritdoc/>
  public async Task<IEnumerable<TicketDto>> GetFilteredTicketsAsync(Guid? projectId = null, Guid? assignedUserId = null, Guid? creatorId = null)
  {
    var tickets = await this.ticketRepository.GetFilteredAsync(projectId, assignedUserId, creatorId).ConfigureAwait(false);
    return await Task.WhenAll(tickets.Select(t => this.MapToDtoAsync(t))).ConfigureAwait(false);
  }

  /// <inheritdoc/>
  public async Task<TicketDto?> GetTicketAsync(Guid id)
  {
    var ticket = await this.ticketRepository.GetByIdAsync(id).ConfigureAwait(false);
    return ticket != null ? await this.MapToDtoAsync(ticket).ConfigureAwait(false) : null;
  }

  /// <inheritdoc/>
  public async Task CreateTicketAsync(CreateTicketDto dto)
  {
    ArgumentNullException.ThrowIfNull(dto);

    var user = await this.GetCurrentUserAsync().ConfigureAwait(false);
    if (user == null)
    {
      throw new UnauthorizedAccessException();
    }

    var defaultStateId = await this.ticketRepository.GetDefaultWorkflowStateIdAsync().ConfigureAwait(false);

    var ticket = new Ticket(dto.Title, TicketsPlease.Domain.Enums.TicketType.Task, dto.ProjectId, user.Id, defaultStateId, "initial");
    ticket.UpdateDescription(dto.Description, dto.Description);
    ticket.AssignUser(dto.AssignedUserId);
    ticket.SetPriority(dto.PriorityId);
    ticket.SetEstimatePoints(dto.EstimatePoints);
    ticket.SetDifficulty(dto.ChilliesDifficulty);
    ticket.SetTenantId(user.TenantId);

    if (dto.TagIds != null && dto.TagIds.Count > 0)
    {
      ticket.SyncTags(dto.TagIds);
    }

    await this.ticketRepository.AddAsync(ticket).ConfigureAwait(false);
    await this.ticketRepository.SaveChangesAsync().ConfigureAwait(false);
  }

  /// <inheritdoc/>
  public async Task UpdateTicketAsync(UpdateTicketDto dto)
  {
    ArgumentNullException.ThrowIfNull(dto);

    var ticket = await this.ticketRepository.GetByIdAsync(dto.Id).ConfigureAwait(false);
    if (ticket == null)
    {
      throw new KeyNotFoundException(TicketNotFoundMessage);
    }

    ticket.UpdateTitle(dto.Title);
    ticket.UpdateDescription(dto.Description, dto.Description);
    ticket.AssignUser(dto.AssignedUserId);
    ticket.SetPriority(dto.PriorityId);
    ticket.SetEstimatePoints(dto.EstimatePoints);
    ticket.SetDifficulty(dto.ChilliesDifficulty);

    if (dto.TagIds != null)
    {
      ticket.SyncTags(dto.TagIds);
    }

    await this.ticketRepository.SaveChangesAsync().ConfigureAwait(false);
  }

  /// <inheritdoc/>
  public async Task MoveTicketAsync(Guid id, string newStatus)
  {
    var ticket = await this.ticketRepository.GetByIdAsync(id).ConfigureAwait(false);
    var user = await this.GetCurrentUserAsync().ConfigureAwait(false);
    if (ticket == null || user == null)
    {
      throw new KeyNotFoundException(TicketNotFoundMessage);
    }

    var targetState = await this.ticketRepository.GetWorkflowStateByNameAsync(newStatus).ConfigureAwait(false);
    if (targetState == null)
    {
      throw new ArgumentException($"Ungültiger Status: {newStatus}");
    }

    // Übergangsregel prüfen (F8)
    var transition = await this.ticketRepository.GetTransitionAsync(ticket.WorkflowStateId, targetState.Id).ConfigureAwait(false);
    if (transition == null)
    {
      throw new InvalidOperationException($"Der Übergang von '{ticket.Status}' nach '{newStatus}' ist nicht erlaubt.");
    }

    // Rollenprüfung falls eingeschränkt
    if (transition.AllowedRoleId.HasValue)
    {
        var roles = await this.userManager.GetRolesAsync(user).ConfigureAwait(false);
        // Wir gehen davon aus, dass wir die Rollen-Namen prüfen oder die ID vergleichen müssen.
        // Da wir in der Transition die RoleId haben, prüfen wir ob der User diese Rolle hat.
        var allowedRole = await this.userManager.GetUsersInRoleAsync(transition.AllowedRoleId.Value.ToString()).ConfigureAwait(false);
        // Besser: roleManager nutzen oder rollen-Strings vergleichen.
        // Einfachere Lösung für MVVM: User-Rollen gegen Namen prüfen wenn Role-ID bekannt ist.
        // Da wir statische IDs haben, können wir es hardcoden oder sauber auflösen.
        
        // Suche Rolle Name für ID
        var role = await this.roleManager.FindByIdAsync(transition.AllowedRoleId.Value.ToString()).ConfigureAwait(false);
        if (role != null && !roles.Contains(role.Name!))
        {
            throw new UnauthorizedAccessException($"Dieser Übergang ist nur für die Rolle '{role.Name}' erlaubt.");
        }
    }

    if (newStatus == "Closed" || targetState.IsTerminalState)
    {
      await this.CloseTicketAsync(id).ConfigureAwait(false);
    }
    else
    {
      ticket.MoveToState(targetState.Id);
      await this.ticketRepository.SaveChangesAsync().ConfigureAwait(false);
    }
  }

  /// <inheritdoc/>
  public async Task CloseTicketAsync(Guid id)
  {
    var ticket = await this.ticketRepository.GetByIdAsync(id).ConfigureAwait(false);
    var user = await this.GetCurrentUserAsync().ConfigureAwait(false);
    if (ticket == null || user == null)
    {
      throw new KeyNotFoundException();
    }

    if (!ticket.CanBeClosed())
    {
      throw new InvalidOperationException("Das Ticket kann nicht geschlossen werden, da es noch offene Abhängigkeiten (Vorgänger) hat.");
    }

    var roles = await this.userManager.GetRolesAsync(user).ConfigureAwait(false);
    ticket.Close(user.Id, roles.Contains("Admin"));
    await this.ticketRepository.SaveChangesAsync().ConfigureAwait(false);
  }

  /// <inheritdoc/>
  public async Task AddDependencyAsync(Guid ticketId, Guid blockerId)
  {
    var ticket = await this.ticketRepository.GetByIdAsync(ticketId).ConfigureAwait(false);
    if (ticket == null)
    {
      throw new KeyNotFoundException(TicketNotFoundMessage);
    }

    ticket.AddLink(blockerId, TicketsPlease.Domain.Enums.TicketLinkType.Blocks);
    await this.ticketRepository.SaveChangesAsync().ConfigureAwait(false);
  }

  /// <inheritdoc/>
  public async Task RemoveDependencyAsync(Guid sourceId, Guid targetId)
  {
    var ticket = await this.ticketRepository.GetByIdAsync(sourceId).ConfigureAwait(false);
    if (ticket == null)
    {
      throw new KeyNotFoundException(TicketNotFoundMessage);
    }

    var link = ticket.BlockedBy.Union(ticket.Blocking).FirstOrDefault(l => l.Id == targetId);
    if (link != null)
    {
      await this.ticketRepository.RemoveLinkAsync(link.Id).ConfigureAwait(false);
      await this.ticketRepository.SaveChangesAsync().ConfigureAwait(false);
    }
  }

  /// <inheritdoc/>
  public async Task UploadAttachmentAsync(Guid ticketId, IFormFile file)
  {
    ArgumentNullException.ThrowIfNull(file);

    var user = await this.GetCurrentUserAsync().ConfigureAwait(false);
    if (user == null)
    {
      throw new UnauthorizedAccessException();
    }

    var ticket = await this.ticketRepository.GetByIdAsync(ticketId).ConfigureAwait(false);
    if (ticket == null)
    {
      throw new KeyNotFoundException(TicketNotFoundMessage);
    }

    using var stream = file.OpenReadStream();
    var blobPath = await this.fileStorageService.SaveFileAsync(stream, file.FileName).ConfigureAwait(false);

    var asset = new FileAsset
    {
      Id = Guid.NewGuid(),
      FileName = file.FileName,
      ContentType = file.ContentType,
      SizeBytes = file.Length,
      BlobPath = blobPath,
      UploadedByUserId = user.Id,
      TicketId = ticketId,
      UploadedAt = DateTime.UtcNow,
    };

    await this.fileAssetRepository.AddAsync(asset).ConfigureAwait(false);
    await this.fileAssetRepository.SaveChangesAsync().ConfigureAwait(false);
  }

  /// <inheritdoc/>
  public async Task<IEnumerable<TagDto>> GetAllTagsAsync()
  {
    var tags = await this.ticketRepository.GetAllTagsAsync().ConfigureAwait(false);
    return tags.Select(t => new TagDto(t.Id, t.Name, t.ColorHex));
  }

  /// <summary>
  /// Mappt eine Ticket-Entität auf ein DTO.
  /// </summary>
  /// <param name="t">Die Entität.</param>
  /// <returns>Das gemappte <see cref="TicketDto"/>.</returns>
  private async Task<TicketDto> MapToDtoAsync(Ticket t)
  {
    var comments = t.Comments?.OrderByDescending(c => c.CreatedAt).Select(c => new CommentDto(
          c.Id,
          c.Content,
          c.AuthorId,
          c.Author?.UserName ?? "Unbekannt",
          c.CreatedAt)) ?? Enumerable.Empty<CommentDto>();

    var blockedBy = t.BlockedBy?.Select(l => new TicketLinkDto(
          l.Id,
          l.SourceTicketId,
          l.SourceTicket?.Title ?? "???",
          l.TargetTicketId,
          l.TargetTicket?.Title ?? "???",
          l.LinkType,
          l.SourceTicket?.Status == "Closed" || l.SourceTicket?.Status == "Done")) ?? Enumerable.Empty<TicketLinkDto>();

    var blocking = t.Blocking?.Select(l => new TicketLinkDto(
          l.Id,
          l.SourceTicketId,
          l.SourceTicket?.Title ?? "???",
          l.TargetTicketId,
          l.TargetTicket?.Title ?? "???",
          l.LinkType,
          l.TargetTicket?.Status == "Closed" || l.TargetTicket?.Status == "Done")) ?? Enumerable.Empty<TicketLinkDto>();

    var attachments = t.Attachments?.Select(a => new FileAssetDto(
          a.Id,
          a.FileName,
          a.ContentType ?? "application/octet-stream",
          a.SizeBytes,
          a.UploadedAt,
          a.UploadedByUser?.UserName ?? "Unbekannt")) ?? Enumerable.Empty<FileAssetDto>();

    var tags = t.Tags?.Select(tt => new TagDto(tt.TagId, tt.Tag?.Name ?? "Unbekannt", tt.Tag?.ColorHex ?? "#ccc")) ?? Enumerable.Empty<TagDto>();

    var timeLogs = await this.timeTrackingService.GetTimeLogsAsync(t.Id).ConfigureAwait(false);
    var subTickets = await this.subTicketService.GetSubTicketsAsync(t.Id).ConfigureAwait(false);
    var user = await this.GetCurrentUserAsync().ConfigureAwait(false);
    bool isTimerRunning = user != null && await this.timeTrackingService.IsTimerRunningAsync(t.Id, user.Id).ConfigureAwait(false);

    return new TicketDto(
        t.Id,
        t.Title,
        t.Description,
        t.Status,
        t.ProjectId,
        t.Project?.Title ?? "Unbekannt",
        t.AssignedUserId,
        t.AssignedUser?.UserName ?? "Nicht zugewiesen",
        t.Type,
        new TicketPriorityDto(t.PriorityId, t.Priority?.Name ?? "Normal", t.Priority?.ColorHex ?? "#ccc"),
        t.CreatedAt,
        t.EstimatePoints,
        t.ChilliesDifficulty,
        tags,
        timeLogs,
        subTickets,
        isTimerRunning,
        comments,
        blockedBy,
        blocking,
        attachments);
  }

  private async Task<User?> GetCurrentUserAsync()
  {
    return await this.userManager.GetUserAsync(this.httpContextAccessor.HttpContext!.User).ConfigureAwait(false);
  }
}
