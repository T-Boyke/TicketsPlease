// <copyright file="RegisterViewModel.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Web.Models.Account;

using System.ComponentModel.DataAnnotations;

/// <summary>
/// Das ViewModel für die Registrierung.
/// </summary>
public class RegisterViewModel
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
  
  /// <summary>
  /// Gets or sets die Organisation.
  /// </summary>
  [Required(ErrorMessage = "The Organization field is required.")]
  [Display(Name = "Organization")]
  public Guid OrganizationId { get; set; }

  /// <summary>
  /// Gets or sets die Position.
  /// </summary>
  [Required(ErrorMessage = "The Position field is required.")]
  [Display(Name = "Position")]
  public string Position { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets den Tech-Stack.
  /// </summary>
  [Display(Name = "Tech Stack")]
  public string? TechStack { get; set; }

  /// <summary>
  /// Gets or sets die Straße.
  /// </summary>
  [Required(ErrorMessage = "The Street field is required.")]
  [Display(Name = "Street")]
  public string Street { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets die Hausnummer.
  /// </summary>
  [Required(ErrorMessage = "The House Number field is required.")]
  [Display(Name = "House Number")]
  public string HouseNumber { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets die Stadt.
  /// </summary>
  [Required(ErrorMessage = "The City field is required.")]
  [Display(Name = "City")]
  public string City { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets das Land.
  /// </summary>
  [Required(ErrorMessage = "The Country field is required.")]
  [Display(Name = "Country")]
  public string Country { get; set; } = string.Empty;

  /// <summary>
  /// Gets or sets den optionalen Einladungs-Token.
  /// </summary>
  public Guid? InviteToken { get; set; }
}
