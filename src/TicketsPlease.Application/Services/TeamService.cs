// <copyright file="TeamService.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Application.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TicketsPlease.Application.Common.Dtos;
using TicketsPlease.Application.Common.Interfaces;
using TicketsPlease.Domain.Entities;

/// <summary>
/// Service-Implementierung für das Management von Teams.
/// </summary>
public class TeamService : ITeamService
{
  private readonly ITeamRepository teamRepository;

  /// <summary>
  /// Initializes a new instance of the <see cref="TeamService"/> class.
  /// </summary>
  /// <param name="teamRepository">Das Repository für Teams.</param>
  public TeamService(ITeamRepository teamRepository)
  {
    this.teamRepository = teamRepository;
  }

  /// <inheritdoc/>
  public async Task<IEnumerable<TeamDto>> GetUserTeamsAsync(Guid userId, CancellationToken cancellationToken = default)
  {
    var teams = await this.teamRepository.GetTeamsByUserIdAsync(userId, cancellationToken).ConfigureAwait(false);
    return teams.Select(this.MapToDto);
  }

  /// <inheritdoc/>
  public async Task<TeamDto?> GetTeamDetailsAsync(Guid teamId, CancellationToken cancellationToken = default)
  {
    var team = await this.teamRepository.GetByIdAsync(teamId, cancellationToken).ConfigureAwait(false);
    return team != null ? this.MapToDto(team) : null;
  }

  /// <inheritdoc/>
  public async Task<IEnumerable<TeamDto>> GetAllTeamsAsync(CancellationToken cancellationToken = default)
  {
    var teams = await this.teamRepository.GetAllTeamsAsync(cancellationToken).ConfigureAwait(false);
    return teams.Select(this.MapToDto);
  }

  /// <inheritdoc/>
  public async Task<Guid> CreateTeamAsync(string name, string description, string colorCode, Guid creatorUserId, CancellationToken cancellationToken = default)
  {
    var team = new Team
    {
      Id = Guid.NewGuid(),
      Name = name,
      Description = description,
      ColorCode = colorCode,
      CreatedAt = DateTime.UtcNow,
      CreatedByUserId = creatorUserId,
    };

    await this.teamRepository.AddAsync(team, cancellationToken).ConfigureAwait(false);
    await this.teamRepository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

    return team.Id;
  }

  /// <inheritdoc/>
  public async Task AddMemberAsync(Guid teamId, Guid userId, bool isTeamLead = false, CancellationToken cancellationToken = default)
  {
    var team = await this.teamRepository.GetByIdAsync(teamId, cancellationToken).ConfigureAwait(false);
    if (team == null)
    {
      throw new ArgumentException("Team not found", nameof(teamId));
    }

    if (team.Members.Any(m => m.UserId == userId))
    {
      return;
    }

    team.Members.Add(new TeamMember
    {
      Id = Guid.NewGuid(),
      TeamId = teamId,
      UserId = userId,
      JoinedAt = DateTime.UtcNow,
      IsTeamLead = isTeamLead,
    });

    await this.teamRepository.UpdateAsync(team, cancellationToken).ConfigureAwait(false);
    await this.teamRepository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
  }

  /// <inheritdoc/>
  public async Task RemoveMemberAsync(Guid teamId, Guid userId, CancellationToken cancellationToken = default)
  {
    var team = await this.teamRepository.GetByIdAsync(teamId, cancellationToken).ConfigureAwait(false);
    if (team == null)
    {
      throw new ArgumentException("Team not found", nameof(teamId));
    }

    var member = team.Members.FirstOrDefault(m => m.UserId == userId);
    if (member != null)
    {
      team.Members.Remove(member);
      await this.teamRepository.UpdateAsync(team, cancellationToken).ConfigureAwait(false);
      await this.teamRepository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }
  }

  /// <inheritdoc/>
  public async Task DeleteTeamAsync(Guid teamId)
  {
    var team = await this.teamRepository.GetByIdAsync(teamId).ConfigureAwait(false);
    if (team != null)
    {
      await this.teamRepository.DeleteAsync(team).ConfigureAwait(false);
      await this.teamRepository.SaveChangesAsync().ConfigureAwait(false);
    }
  }

  private TeamDto MapToDto(Team team)
  {
    return new TeamDto(
        team.Id,
        team.Name,
        team.Description,
        team.ColorCode,
        team.CreatedAt,
        team.Members.Count,
        team.Members.Select(m => new TeamMemberDto(
            m.UserId,
            m.User?.UserName ?? "Unknown",
            m.JoinedAt,
            m.IsTeamLead)));
  }
}
