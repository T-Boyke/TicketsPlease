// <copyright file="UserRepository.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Infrastructure.Repositories;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TicketsPlease.Application.Common.Interfaces;
using TicketsPlease.Domain.Entities;
using TicketsPlease.Infrastructure.Persistence;

/// <summary>
/// Implementierung des IUserRepository für EF Core.
/// </summary>
public class UserRepository : IUserRepository
{
    private readonly AppDbContext context;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserRepository"/> class.
    /// </summary>
    /// <param name="context">Der Datenbankkontext.</param>
    public UserRepository(AppDbContext context)
    {
        this.context = context;
    }

    /// <inheritdoc />
    public async Task<User?> GetUserWithDetailsAsync(Guid userId)
    {
        return await this.context.Users
            .Include(u => u.Profile)
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Id == userId)
            .ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<List<Team>> GetUserTeamsAsync(Guid userId)
    {
        return await this.context.Teams
            .Where(t => t.Members.Any(mu => mu.UserId == userId))
            .ToListAsync()
            .ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<UserProfile> GetOrCreateProfileAsync(Guid userId)
    {
        var profile = await this.context.UserProfiles
            .FirstOrDefaultAsync(p => p.UserId == userId)
            .ConfigureAwait(false);

        if (profile == null)
        {
            profile = new UserProfile { UserId = userId };
            await this.context.UserProfiles.AddAsync(profile).ConfigureAwait(false);
            await this.context.SaveChangesAsync().ConfigureAwait(false);
        }

        return profile;
    }

    /// <inheritdoc />
    public async Task UpdateProfileAsync(UserProfile profile)
    {
        this.context.UserProfiles.Update(profile);
        await this.context.SaveChangesAsync().ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<int> GetActiveUserCountAsync(Guid tenantId)
    {
        return await this.context.Users
            .CountAsync(u => u.TenantId == tenantId && u.IsActive)
            .ConfigureAwait(false);
    }
}
