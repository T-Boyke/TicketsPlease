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
    private readonly IProjectRepository _projectRepository;
    private readonly UserManager<User> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;

    /// <summary>
    /// Initialisiert eine neue Instanz der <see cref="ProjectService"/> Klasse.
    /// </summary>
    /// <param name="projectRepository">Das Repository für Projekte.</param>
    /// <param name="userManager">Der Identity UserManager.</param>
    /// <param name="httpContextAccessor">Der Accessor für den aktuellen HttpContext.</param>
    public ProjectService(
        IProjectRepository projectRepository,
        UserManager<User> userManager,
        IHttpContextAccessor httpContextAccessor)
    {
        _projectRepository = projectRepository;
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
    }

    /// <summary>
    /// Ermittelt die Mandanten-ID des aktuell angemeldeten Benutzers.
    /// </summary>
    /// <returns>Die Mandanten-ID (Guid).</returns>
    private async Task<Guid> GetCurrentTenantIdAsync()
    {
        var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext!.User);
        return user?.TenantId ?? Guid.Empty;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<ProjectDto>> GetProjectsAsync()
    {
        var tenantId = await GetCurrentTenantIdAsync();
        var projects = await _projectRepository.GetAllAsync(tenantId);
        return projects.Select(p => new ProjectDto(
            p.Id, p.Title, p.Description, p.StartDate, p.EndDate, p.IsOpen, p.TenantId));
    }

    /// <inheritdoc/>
    public async Task<ProjectDto?> GetProjectAsync(Guid id)
    {
        var project = await _projectRepository.GetByIdAsync(id);
        if (project == null) return null;

        return new ProjectDto(
            project.Id, project.Title, project.Description, project.StartDate, project.EndDate, project.IsOpen, project.TenantId);
    }

    /// <inheritdoc/>
    public async Task CreateProjectAsync(CreateProjectDto dto)
    {
        var tenantId = await GetCurrentTenantIdAsync();
        var project = new Project(dto.Title, dto.StartDate);
        project.UpdateMetadata(dto.Title, dto.Description);
        project.SetEndDate(dto.EndDate);
        project.SetTenantId(tenantId);

        await _projectRepository.AddAsync(project);
    }

    /// <inheritdoc/>
    public async Task UpdateProjectAsync(UpdateProjectDto dto)
    {
        var project = await _projectRepository.GetByIdAsync(dto.Id);
        if (project == null) throw new KeyNotFoundException("Projekt nicht gefunden.");

        project.UpdateMetadata(dto.Title, dto.Description);
        project.SetEndDate(dto.EndDate);
        if (dto.IsOpen) project.Open(); else project.Close();

        await _projectRepository.UpdateAsync(project);
    }

    /// <inheritdoc/>
    public async Task DeleteProjectAsync(Guid id)
    {
        var project = await _projectRepository.GetByIdAsync(id);
        if (project != null)
        {
            await _projectRepository.DeleteAsync(project);
        }
    }
}
