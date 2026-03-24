// <copyright file="AccountController.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Web.Controllers;

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
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;

    /// <summary>
    /// Initialisiert eine neue Instanz der <see cref="AccountController"/> Klasse.
    /// </summary>
    /// <param name="signInManager">Der Identity SignInManager.</param>
    /// <param name="userManager">Der Identity UserManager.</param>
    public AccountController(SignInManager<User> signInManager, UserManager<User> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    /// <summary>
    /// Zeigt die Login-Seite an.
    /// </summary>
    /// <returns>Die Login-View.</returns>
    [HttpGet]
    public IActionResult Login()
    {
        return View();
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
        if (ModelState.IsValid)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError(string.Empty, "Ungültiger Login-Versuch.");
        }
        return View(model);
    }

    /// <summary>
    /// Zeigt die Registrierungsseite an.
    /// </summary>
    /// <returns>Die Registrierungs-View.</returns>
    [HttpGet]
    public IActionResult Register()
    {
        return View();
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
        if (ModelState.IsValid)
        {
            var user = new User { UserName = model.Username, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
        return View(model);
    }

    /// <summary>
    /// Meldet den Benutzer ab.
    /// </summary>
    /// <returns>Ein Task mit dem Aktionsergebnis.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

    /// <summary>
    /// Zeigt das Benutzerprofil an.
    /// </summary>
    /// <returns>Die Profil-View.</returns>
    [Authorize]
    public async Task<IActionResult> Profile()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return NotFound();
        return View(user);
    }

    /// <summary>
    /// Zeigt die Seite für verweigerten Zugriff an.
    /// </summary>
    /// <returns>Die AccessDenied-View.</returns>
    public IActionResult AccessDenied()
    {
        return View();
    }
}
