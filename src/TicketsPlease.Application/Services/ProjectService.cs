// <copyright file="ProjectService.cs" company="BitLC-NE-2025-2026">
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
/// Implementierung des Projektdienstes für die Verwaltung von Projekten.
/// Berücksichtigt Mandantentrennung (Multi-Tenancy).
/// </summary>
public class ProjectService : IProjectService
{
    private readonly IProjectRepository projectRepository;
    private readonly UserManager<User> userManager;
    private readonly IHttpContextAccessor httpContextAccessor;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProjectService"/> class.
    /// </summary>
    /// <param name="projectRepository">Das Repository für Projekte.</param>
    /// <param name="userManager">Der Identity UserManager.</param>
    /// <param name="httpContextAccessor">Der Accessor für den aktuellen HttpContext.</param>
    public ProjectService(
        IProjectRepository projectRepository,
        UserManager<User> userManager,
        IHttpContextAccessor httpContextAccessor)
    {
        this.projectRepository = projectRepository;
        this.userManager = userManager;
        this.httpContextAccessor = httpContextAccessor;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<ProjectDto>> GetProjectsAsync()
    {
        var tenantId = await this.GetCurrentTenantIdAsync().ConfigureAwait(false);
        var projects = await this.projectRepository.GetAllAsync(tenantId).ConfigureAwait(false);
        return projects.Select(p => new ProjectDto(
            p.Id, p.Title, p.Description, p.StartDate, p.EndDate, p.IsOpen, p.TenantId));
    }

    /// <inheritdoc/>
    public async Task<ProjectDto?> GetProjectAsync(Guid id)
    {
        var project = await this.projectRepository.GetByIdAsync(id).ConfigureAwait(false);
        if (project == null)
        {
            return null;
        }

        return new ProjectDto(
            project.Id, project.Title, project.Description, project.StartDate, project.EndDate, project.IsOpen, project.TenantId);
    }

    /// <inheritdoc/>
    public async Task CreateProjectAsync(CreateProjectDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        var tenantId = await this.GetCurrentTenantIdAsync().ConfigureAwait(false);
        var project = new Project(dto.Title, dto.StartDate);
        project.UpdateMetadata(dto.Title, dto.Description);
        project.SetEndDate(dto.EndDate);
        project.SetTenantId(tenantId);

        await this.projectRepository.AddAsync(project).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task UpdateProjectAsync(UpdateProjectDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        var project = await this.projectRepository.GetByIdAsync(dto.Id).ConfigureAwait(false);
        if (project == null)
        {
            throw new KeyNotFoundException("Projekt nicht gefunden.");
        }

        project.UpdateMetadata(dto.Title, dto.Description);
        project.SetEndDate(dto.EndDate);

        if (dto.IsOpen)
        {
            project.Open();
        }
        else
        {
            project.Close();
        }

        await this.projectRepository.UpdateAsync(project).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task DeleteProjectAsync(Guid id)
    {
        var project = await this.projectRepository.GetByIdAsync(id).ConfigureAwait(false);
        if (project != null)
        {
            await this.projectRepository.DeleteAsync(project).ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Ermittelt die Mandanten-ID des aktuell angemeldeten Benutzers.
    /// </summary>
    /// <returns>Die Mandanten-ID (Guid).</returns>
    private async Task<Guid> GetCurrentTenantIdAsync()
    {
        var user = await this.userManager.GetUserAsync(this.httpContextAccessor.HttpContext!.User).ConfigureAwait(false);
        return user?.TenantId ?? Guid.Empty;
    }
}
