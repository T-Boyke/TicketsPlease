// <copyright file="FileAssetRepository.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Infrastructure.Repositories;

using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TicketsPlease.Application.Common.Interfaces;
using TicketsPlease.Domain.Entities;
using TicketsPlease.Infrastructure.Persistence;

/// <summary>
/// EF Implementation of IFileAssetRepository.
/// </summary>
public class FileAssetRepository : IFileAssetRepository
{
    private readonly AppDbContext context;

    /// <summary>
    /// Initializes a new instance of the <see cref="FileAssetRepository"/> class.
    /// </summary>
    /// <param name="context">Der DB Kontext.</param>
    public FileAssetRepository(AppDbContext context)
    {
        this.context = context;
    }

    /// <inheritdoc/>
    public async Task<FileAsset?> GetByIdAsync(Guid id)
    {
        return await this.context.FileAssets
            .Include(f => f.UploadedByUser)
            .FirstOrDefaultAsync(f => f.Id == id)
            .ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task AddAsync(FileAsset fileAsset)
    {
        await this.context.FileAssets.AddAsync(fileAsset).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public Task DeleteAsync(FileAsset fileAsset)
    {
        this.context.FileAssets.Remove(fileAsset);
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public async Task SaveChangesAsync()
    {
        await this.context.SaveChangesAsync().ConfigureAwait(false);
    }
}
