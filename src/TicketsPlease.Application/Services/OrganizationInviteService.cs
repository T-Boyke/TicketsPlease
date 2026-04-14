// <copyright file="OrganizationInviteService.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Application.Services;

using System;
using System.Linq;
using System.Threading.Tasks;
using TicketsPlease.Application.Common.Dtos;
using TicketsPlease.Application.Common.Interfaces;
using TicketsPlease.Domain.Entities;

/// <summary>
/// Implementierung des Dienstes zur Verwaltung von Organisationseinladungen.
/// </summary>
public class OrganizationInviteService : IOrganizationInviteService
{
  private readonly IOrganizationRepository repository;

  /// <summary>
  /// Initializes a new instance of the <see cref="OrganizationInviteService"/> class.
  /// </summary>
  /// <param name="repository">Das Organisations-Repository.</param>
  public OrganizationInviteService(IOrganizationRepository repository)
  {
    this.repository = repository;
  }

  /// <inheritdoc />
  public async Task<OrganizationInviteDto> CreateInviteAsync(Guid organizationId, string? targetedEmail = null, int expiryDays = 7)
  {
    var org = await this.repository.GetByIdAsync(organizationId).ConfigureAwait(false);
    if (org == null)
    {
      throw new ArgumentException("Organisation nicht gefunden.", nameof(organizationId));
    }

    var invite = new OrganizationInvite
    {
      Token = Guid.NewGuid(),
      OrganizationId = organizationId,
      TargetedEmail = targetedEmail,
      ExpiresAt = DateTime.UtcNow.AddDays(expiryDays),
      IsUsed = false
    };

    this.repository.AddInvite(invite);
    await this.repository.SaveChangesAsync().ConfigureAwait(false);

    return new OrganizationInviteDto(invite.Token, invite.OrganizationId, org.Name, invite.ExpiresAt, invite.TargetedEmail);
  }

  /// <inheritdoc />
  public async Task<OrganizationInviteDto?> ValidateTokenAsync(Guid token)
  {
    var invite = await this.repository.GetInviteByTokenAsync(token).ConfigureAwait(false);

    if (invite == null || invite.Organization == null)
    {
      return null;
    }

    return new OrganizationInviteDto(invite.Token, invite.OrganizationId, invite.Organization.Name, invite.ExpiresAt, invite.TargetedEmail);
  }

  /// <inheritdoc />
  public async Task MarkAsUsedAsync(Guid token, Guid userId)
  {
    var invite = await this.repository.GetInviteByTokenAsync(token).ConfigureAwait(false);

    if (invite != null)
    {
      invite.IsUsed = true;
      invite.UsedByUserId = userId;
      await this.repository.SaveChangesAsync().ConfigureAwait(false);
    }
  }
}
