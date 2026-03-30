// <copyright file="LoginViewModel.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Web.Models.Account;

using System.ComponentModel.DataAnnotations;

/// <summary>
/// Das ViewModel für den Login.
/// </summary>
public class LoginViewModel
{
  /// <summary>
  /// Gets or sets die E-Mail-Adresse.
  /// </summary>
  [Required(ErrorMessage = "The Email field is required.")]
  [EmailAddress(ErrorMessage = "The Email field is not a valid e-mail address.")]
  public string Email { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets das Passwort.
  /// </summary>
  [Required(ErrorMessage = "The Password field is required.")]
  [DataType(DataType.Password)]
  public string Password { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets a value indicating whether die Sitzung gespeichert werden soll.
  /// </summary>
  [Display(Name = "Remember me?")]
  public bool? RememberMe { get; set; }
}
