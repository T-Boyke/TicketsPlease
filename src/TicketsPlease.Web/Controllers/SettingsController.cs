// <copyright file="SettingsController.cs" company="BitLC-NE-2025-2026">
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

using Microsoft.Extensions.Localization;

/// <summary>
/// Controller für die persönlichen Benutzereinstellungen (Performance, Töne).
/// </summary>
[Authorize]
internal sealed class SettingsController : Controller
{
  private readonly UserManager<User> userManager;
  private readonly IUserRepository userRepository;
  private readonly IStringLocalizer<SettingsController> localizer;

  /// <summary>
  /// Initializes a new instance of the <see cref="SettingsController"/> class.
  /// </summary>
  /// <param name="userManager">Der User-Manager.</param>
  /// <param name="userRepository">Das User-Repository.</param>
  /// <param name="localizer">Der Lokalisierer.</param>
  public SettingsController(
    UserManager<User> userManager, 
    IUserRepository userRepository,
    IStringLocalizer<SettingsController> localizer)
  {
    this.userManager = userManager;
    this.userRepository = userRepository;
    this.localizer = localizer;
  }

  /// <summary>
  /// Zeigt die Einstellungsseite an.
  /// </summary>
  /// <returns>Die Index-View.</returns>
  [HttpGet]
  public async Task<IActionResult> Index()
  {
    var user = await this.userManager.GetUserAsync(this.User).ConfigureAwait(false);
    if (user == null)
    {
      return this.NotFound();
    }

    var profile = await this.userRepository.GetOrCreateProfileAsync(user.Id).ConfigureAwait(false);
    return this.View(profile);
  }

  /// <summary>
  /// Speichert die persönlichen Einstellungen.
  /// </summary>
  /// <param name="kanbanUpdateIntervalMs">Das Intervall.</param>
  /// <param name="reduceAnimations">Animationen reduzieren?</param>
  /// <param name="notificationSound">Soundname.</param>
  /// <param name="emailNotificationsEnabled">E-Mails an?</param>
  /// <returns>Ein Redirect.</returns>
  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> Save(int kanbanUpdateIntervalMs, bool reduceAnimations, string notificationSound, bool emailNotificationsEnabled)
  {
    var user = await this.userManager.GetUserAsync(this.User).ConfigureAwait(false);
    if (user == null)
    {
      return this.NotFound();
    }

    var profile = await this.userRepository.GetOrCreateProfileAsync(user.Id).ConfigureAwait(false);
    profile.KanbanUpdateIntervalMs = kanbanUpdateIntervalMs;
    profile.ReduceAnimations = reduceAnimations;
    profile.NotificationSound = notificationSound;
    profile.EmailNotificationsEnabled = emailNotificationsEnabled;

    await this.userRepository.UpdateProfileAsync(profile).ConfigureAwait(false);
    this.TempData["StatusMessage"] = this.localizer["SaveSuccess"].Value;
    
    return this.RedirectToAction(nameof(this.Index));
  }
}
