// <copyright file="AccountController.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Web.Controllers;

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TicketsPlease.Domain.Entities;
using TicketsPlease.Web.Models.Account;

/// <summary>
/// Controller für die Benutzerverwaltung (Login, Registrierung, Profil).
/// </summary>
public class AccountController : Controller
{
  private readonly SignInManager<User> signInManager;
  private readonly UserManager<User> userManager;

  /// <summary>
  /// Initializes a new instance of the <see cref="AccountController"/> class.
  /// </summary>
  /// <param name="signInManager">Der Identity SignInManager.</param>
  /// <param name="userManager">Der Identity UserManager.</param>
  public AccountController(SignInManager<User> signInManager, UserManager<User> userManager)
  {
    this.signInManager = signInManager;
    this.userManager = userManager;
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
      var result = await this.signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false).ConfigureAwait(false);
      if (result.Succeeded)
      {
        return this.RedirectToAction("Index", "Home");
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
      var user = new User { UserName = model.Username, Email = model.Email };
      var result = await this.userManager.CreateAsync(user, model.Password).ConfigureAwait(false);
      if (result.Succeeded)
      {
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
  /// Zeigt das Benutzerprofil an.
  /// </summary>
  /// <returns>Die Profil-View.</returns>
  [Authorize]
  [HttpGet]
  public async Task<IActionResult> Profile()
  {
    var user = await this.userManager.GetUserAsync(this.User).ConfigureAwait(false);
    if (user == null)
    {
      return this.NotFound();
    }

    return this.View(user);
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
