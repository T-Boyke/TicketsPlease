// <copyright file="RegisterViewModel.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Web.Models.Account;

using System.ComponentModel.DataAnnotations;

/// <summary>
/// Das ViewModel für die Registrierung.
/// </summary>
internal class RegisterViewModel
{
  /// <summary>
  /// Gets or sets den Benutzernamen.
  /// </summary>
  [Required(ErrorMessage = "The Username field is required.")]
  [Display(Name = "Username")]
  public string Username { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets die E-Mail-Adresse.
  /// </summary>
  [Required(ErrorMessage = "The Email field is required.")]
  [EmailAddress(ErrorMessage = "The Email field is not a valid e-mail address.")]
  [Display(Name = "Email")]
  public string Email { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets das Passwort.
  /// </summary>
  [Required(ErrorMessage = "The Password field is required.")]
  [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 8)]
  [DataType(DataType.Password)]
  [Display(Name = "Password")]
  public string Password { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets die Passwort-Bestätigung.
  /// </summary>
  [DataType(DataType.Password)]
  [Display(Name = "Confirm password")]
  [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
  public string ConfirmPassword { get; set; } = string.Empty;
}
