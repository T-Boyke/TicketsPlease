// <copyright file="LocalStorageService.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Infrastructure.Services;

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using TicketsPlease.Application.Common.Interfaces;

/// <summary>
/// Implementiert die Dateispeicherung im lokalen Dateisystem.
/// </summary>
public class LocalStorageService : IFileStorageService
{
    private readonly string storageRoot;

    /// <summary>
    /// Initializes a new instance of the <see cref="LocalStorageService"/> class.
    /// </summary>
    /// <param name="env">Die WebHostEnvironment für Pfade.</param>
    public LocalStorageService(IWebHostEnvironment env)
    {
        // Wir speichern außerhalb von wwwroot für bessere Zugriffskontrolle
        this.storageRoot = Path.Combine(env.ContentRootPath, "App_Data", "Uploads");

        if (!Directory.Exists(this.storageRoot))
        {
            Directory.CreateDirectory(this.storageRoot);
        }
    }

    /// <inheritdoc />
    public async Task<string> SaveFileAsync(Stream content, string fileName, CancellationToken ct = default)
    {
        var extension = Path.GetExtension(fileName);
        var blobName = $"{Guid.NewGuid()}{extension}";
        var fullPath = Path.Combine(this.storageRoot, blobName);

        using (var fileStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write, FileShare.None, 4096, true))
        {
            await content.CopyToAsync(fileStream, ct).ConfigureAwait(false);
        }

        return blobName;
    }

    /// <inheritdoc />
    public Task<Stream> GetFileAsync(string blobPath, CancellationToken ct = default)
    {
        var fullPath = Path.Combine(this.storageRoot, blobPath);
        if (!File.Exists(fullPath))
        {
            throw new FileNotFoundException("Datei nicht gefunden.", blobPath);
        }

        return Task.FromResult<Stream>(new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.Read));
    }

    /// <inheritdoc />
    public Task DeleteFileAsync(string blobPath, CancellationToken ct = default)
    {
        var fullPath = Path.Combine(this.storageRoot, blobPath);
        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }

        return Task.CompletedTask;
    }
}
