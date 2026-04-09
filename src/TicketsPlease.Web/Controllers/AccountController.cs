// <copyright file="AccountController.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Web.Controllers;

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TicketsPlease.Application.Common.Interfaces;
using TicketsPlease.Domain.Entities;
using TicketsPlease.Web.Models.Account;

/// <summary>
/// Controller für die Benutzerverwaltung (Login, Registrierung, Profil).
/// </summary>
internal sealed class AccountController : Controller
{
  private readonly SignInManager<User> signInManager;
  private readonly UserManager<User> userManager;
  private readonly RoleManager<Role> roleManager;
  private readonly IUserRepository userRepository;
  private readonly IOrganizationService organizationService;
  private readonly IDashboardService dashboardService;

  /// <summary>
  /// Initializes a new instance of the <see cref="AccountController"/> class.
  /// </summary>
  /// <param name="signInManager">Der Identity SignInManager.</param>
  /// <param name="userManager">Der Identity UserManager.</param>
  /// <param name="roleManager">Die Rollenverwaltung.</param>
  /// <param name="userRepository">Das Benutzer-Repository.</param>
  /// <param name="organizationService">Der Organisations-Service.</param>
  /// <param name="dashboardService">Der Dashboard-Service.</param>
  public AccountController(
    SignInManager<User> signInManager,
    UserManager<User> userManager,
    RoleManager<Role> roleManager,
    IUserRepository userRepository,
    IOrganizationService organizationService,
    IDashboardService dashboardService)
  {
    this.signInManager = signInManager;
    this.userManager = userManager;
    this.roleManager = roleManager;
    this.userRepository = userRepository;
    this.organizationService = organizationService;
    this.dashboardService = dashboardService;
  }

  /// <summary>
  /// Zeigt die Login-Seite an.
  /// </summary>
  /// <returns>Die Login-View.</returns>
  [HttpGet]
  public IActionResult Login()
  {
    return this.View();
  }

  /// <summary>
  /// Verarbeitet den Login-Versuch.
  /// </summary>
  /// <param name="model">Das Login-ViewModel.</param>
  /// <returns>Ein Task mit dem Aktionsergebnis.</returns>
  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> Login(LoginViewModel model)
  {
    ArgumentNullException.ThrowIfNull(model);

    if (this.ModelState.IsValid)
    {
      var user = await this.userManager.FindByEmailAsync(model.Email).ConfigureAwait(false);
      if (user != null)
      {
        var result = await this.signInManager.PasswordSignInAsync(user.UserName!, model.Password, model.RememberMe ?? false, lockoutOnFailure: false).ConfigureAwait(false);
        if (result.Succeeded)
        {
          return this.RedirectToAction("Index", "Home");
        }
      }

      this.ModelState.AddModelError(string.Empty, "Ungültiger Login-Versuch.");
    }

    return this.View(model);
  }

  /// <summary>
  /// Zeigt die Registrierungsseite an.
  /// </summary>
  /// <returns>Die Registrierungs-View.</returns>
  [HttpGet]
  public IActionResult Register()
  {
    return this.View();
  }

  /// <summary>
  /// Verarbeitet die Benutzerregistrierung.
  /// </summary>
  /// <param name="model">Das Registrierungs-ViewModel.</param>
  /// <returns>Ein Task mit dem Aktionsergebnis.</returns>
  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> Register(RegisterViewModel model)
  {
    ArgumentNullException.ThrowIfNull(model);

    if (this.ModelState.IsValid)
    {
      // Standardmäßig die "User"-Rolle zuweisen
      var defaultRole = await this.roleManager.FindByNameAsync("User").ConfigureAwait(false);
      var defaultRoleId = defaultRole?.Id ?? Guid.Empty;

      var user = new User { UserName = model.Username, Email = model.Email, RoleId = defaultRoleId };
      var result = await this.userManager.CreateAsync(user, model.Password).ConfigureAwait(false);
      if (result.Succeeded)
      {
        await this.userManager.AddToRoleAsync(user, "User").ConfigureAwait(false);
        await this.signInManager.SignInAsync(user, isPersistent: false).ConfigureAwait(false);
        return this.RedirectToAction("Index", "Home");
      }

      foreach (var error in result.Errors)
      {
        this.ModelState.AddModelError(string.Empty, error.Description);
      }
    }

    return this.View(model);
  }

  /// <summary>
  /// Meldet den Benutzer ab.
  /// </summary>
  /// <returns>Ein Task mit dem Aktionsergebnis.</returns>
  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> Logout()
  {
    await this.signInManager.SignOutAsync().ConfigureAwait(false);
    return this.RedirectToAction("Index", "Home");
  }

  /// <summary>
  /// Zeigt das detaillierte Benutzerprofil an.
  /// </summary>
  /// <returns>Die Profil-View.</returns>
  [Authorize]
  [HttpGet]
  public async Task<IActionResult> Profile()
  {
    var userId = Guid.Parse(this.userManager.GetUserId(this.User)!);
    var user = await this.userRepository.GetUserWithDetailsAsync(userId).ConfigureAwait(false);
    if (user == null)
    {
      return this.NotFound();
    }

    var organization = await this.organizationService.GetOrganizationByIdAsync(user.TenantId).ConfigureAwait(false);
    var teams = await this.userRepository.GetUserTeamsAsync(userId).ConfigureAwait(false);
    var performance = await this.dashboardService.GetUserPerformanceDetailAsync(userId).ConfigureAwait(false);

    var model = new UserProfileDto(
        user.Id,
        user.UserName ?? string.Empty,
        user.Email ?? string.Empty,
        user.Profile?.FirstName ?? string.Empty,
        user.Profile?.LastName ?? string.Empty,
        user.Profile?.Bio,
        user.Profile?.PhoneNumber,
        user.Role?.Name ?? "User",
        user.CreatedAt,
        user.LastLoginAt,
        user.IsOnline,
        organization?.Name,
        teams.Select(t => t.Name),
        performance);

    return this.View(model);
  }

  /// <summary>
  /// Zeigt die Seite zum Bearbeiten des Profils an.
  /// </summary>
  /// <returns>Die Edit-View.</returns>
  [Authorize]
  [HttpGet]
  public async Task<IActionResult> Edit()
  {
    var userId = Guid.Parse(this.userManager.GetUserId(this.User)!);
    var user = await this.userRepository.GetUserWithDetailsAsync(userId).ConfigureAwait(false);
    if (user == null)
    {
      return this.NotFound();
    }

    var model = new EditProfileViewModel
    {
      Username = user.UserName ?? string.Empty,
      Email = user.Email ?? string.Empty,
      FirstName = user.Profile?.FirstName ?? string.Empty,
      LastName = user.Profile?.LastName ?? string.Empty,
      Bio = user.Profile?.Bio,
      PhoneNumber = user.Profile?.PhoneNumber,
    };

    return this.View(model);
  }

  /// <summary>
  /// Verarbeitet die Profilbearbeitung.
  /// </summary>
  /// <param name="model">Das EditProfile-ViewModel.</param>
  /// <returns>Ein Task mit dem Aktionsergebnis.</returns>
  [Authorize]
  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> Edit(EditProfileViewModel model)
  {
    ArgumentNullException.ThrowIfNull(model);

    if (!this.ModelState.IsValid)
    {
      return this.View(model);
    }

    var user = await this.userManager.GetUserAsync(this.User).ConfigureAwait(false);
    if (user == null)
    {
      return this.NotFound();
    }

    user.UserName = model.Username;
    user.Email = model.Email;
    await this.userManager.UpdateAsync(user).ConfigureAwait(false);

    var profile = await this.userRepository.GetOrCreateProfileAsync(user.Id).ConfigureAwait(false);
    profile.FirstName = model.FirstName;
    profile.LastName = model.LastName;
    profile.Bio = model.Bio;
    profile.PhoneNumber = model.PhoneNumber;

    await this.userRepository.UpdateProfileAsync(profile).ConfigureAwait(false);

    if (!string.IsNullOrEmpty(model.NewPassword))
    {
      var token = await this.userManager.GeneratePasswordResetTokenAsync(user).ConfigureAwait(false);
      await this.userManager.ResetPasswordAsync(user, token, model.NewPassword).ConfigureAwait(false);
    }

    await this.signInManager.RefreshSignInAsync(user).ConfigureAwait(false);
    return this.RedirectToAction(nameof(this.Profile));
  }

  /// <summary>
  /// Zeigt die Seite für verweigerten Zugriff an.
  /// </summary>
  /// <returns>Die AccessDenied-View.</returns>
  [HttpGet]
  public IActionResult AccessDenied()
  {
    return this.View();
  }
}
