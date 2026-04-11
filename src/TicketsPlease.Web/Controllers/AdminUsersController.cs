// <copyright file="AdminUsersController.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Web.Controllers;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketsPlease.Domain.Entities;

/// <summary>
/// Controller für die Benutzerverwaltung im Administrationsbereich (F2).
/// </summary>
[Authorize(Roles = "Admin")]
internal class AdminUsersController : Controller
{
  private readonly UserManager<User> userManager;
  private readonly RoleManager<Role> roleManager;
  private readonly TicketsPlease.Application.Common.Interfaces.IUserRepository userRepository;

  /// <summary>
  /// Initializes a new instance of the <see cref="AdminUsersController"/> class.
  /// </summary>
  /// <param name="userManager">Die Benutzerverwaltung.</param>
  /// <param name="roleManager">Die Rollenverwaltung.</param>
  /// <param name="userRepository">Das Benutzer-Repository.</param>
    public AdminUsersController(UserManager<User> userManager, RoleManager<Role> roleManager, TicketsPlease.Application.Common.Interfaces.IUserRepository userRepository)
  {
    this.userManager = userManager;
    this.roleManager = roleManager;
    this.userRepository = userRepository;
  }

  /// <summary>
  /// Listet alle Benutzer im System auf.
  /// </summary>
  /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
  [HttpGet]
  public async Task<IActionResult> Index()
  {
    var users = await this.userManager.Users.ToListAsync().ConfigureAwait(false);
    var userViewModels = new List<UserListViewModel>();

    foreach (var user in users)
    {
      var roles = await this.userManager.GetRolesAsync(user).ConfigureAwait(false);
      userViewModels.Add(new UserListViewModel
      {
        Id = user.Id,
        UserName = user.UserName ?? "Unknown",
        Email = user.Email ?? "Unknown",
        Roles = roles.ToList(),
        IsActive = !user.LockoutEnabled || user.LockoutEnd == null || user.LockoutEnd < DateTimeOffset.UtcNow,
      });
    }

    return this.View(userViewModels);
  }

  /// <summary>
  /// Zeigt das Formular zum Bearbeiten eines Benutzers an.
  /// </summary>
  /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
  [HttpGet]
  public async Task<IActionResult> Edit(Guid id)
  {
    var user = await this.userManager.FindByIdAsync(id.ToString()).ConfigureAwait(false);
    if (user == null)
    {
      return this.NotFound();
    }

    var userRoles = await this.userManager.GetRolesAsync(user).ConfigureAwait(false);
    var allRoles = await this.roleManager.Roles.Select(r => r.Name!).ToListAsync().ConfigureAwait(false);

    var model = new EditUserViewModel
    {
      Id = user.Id,
      UserName = user.UserName ?? string.Empty,
      Email = user.Email ?? string.Empty,
      UserRoles = userRoles.ToList(),
      AllRoles = allRoles,
    };

    return this.View(model);
  }

  /// <summary>
  /// Speichert die Änderungen an einem Benutzer.
  /// </summary>
  /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> Edit(EditUserViewModel model)
  {
    if (!this.ModelState.IsValid)
    {
      return this.View(model);
    }

    var user = await this.userManager.FindByIdAsync(model.Id.ToString()).ConfigureAwait(false);
    if (user == null)
    {
      return this.NotFound();
    }

    user.Email = model.Email;
    user.UserName = model.UserName;

    var profile = await this.userRepository.GetOrCreateProfileAsync(user.Id).ConfigureAwait(false);
    profile.Position = model.Position;
    profile.TechStack = model.TechStack;
    profile.Street = model.Street;
    profile.HouseNumber = model.HouseNumber;
    profile.City = model.City;
    profile.Country = model.Country;
    await this.userRepository.UpdateProfileAsync(profile).ConfigureAwait(false);

    var result = await this.userManager.UpdateAsync(user).ConfigureAwait(false);
    if (!result.Succeeded)
    {
      foreach (var error in result.Errors)
      {
        this.ModelState.AddModelError(string.Empty, error.Description);
      }

      return this.View(model);
    }

    var userRoles = await this.userManager.GetRolesAsync(user).ConfigureAwait(false);
    var rolesToRemove = userRoles.Except(model.SelectedRoles).ToList();
    var rolesToAdd = model.SelectedRoles.Except(userRoles).ToList();

    if (rolesToRemove.Count > 0)
    {
      await this.userManager.RemoveFromRolesAsync(user, rolesToRemove).ConfigureAwait(false);
    }

    if (rolesToAdd.Count > 0)
    {
      await this.userManager.AddToRolesAsync(user, rolesToAdd).ConfigureAwait(false);
    }

    return this.RedirectToAction(nameof(this.Index));
  }
}
