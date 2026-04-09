// <copyright file="EditProfileViewModel.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Web.Models.Account;

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

/// <summary>
/// ViewModel zum Bearbeiten des eigenen Benutzerprofils.
/// </summary>
public class EditProfileViewModel
{
    /// <summary>
    /// Gets or sets den Benutzernamen.
    /// </summary>
    [Required(ErrorMessage = "Benutzername ist erforderlich")]
    [Display(Name = "Benutzername")]
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets die E-Mail-Adresse.
    /// </summary>
    [Required(ErrorMessage = "E-Mail ist erforderlich")]
    [EmailAddress(ErrorMessage = "Ungültige E-Mail-Adresse")]
    [Display(Name = "E-Mail")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets den Vornamen.
    /// </summary>
    [Display(Name = "Vorname")]
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets den Nachnamen.
    /// </summary>
    [Display(Name = "Nachname")]
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets die Bio.
    /// </summary>
    [Display(Name = "Biographie")]
    public string? Bio { get; set; }

    /// <summary>
    /// Gets or sets die Telefonnummer.
    /// </summary>
    [Display(Name = "Telefonnummer")]
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// Gets or sets das neue Passwort (optional).
    /// </summary>
    [DataType(DataType.Password)]
    [Display(Name = "Neues Passwort (optional)")]
    public string? NewPassword { get; set; }

    /// <summary>
    /// Gets or sets die Passwortbestätigung.
    /// </summary>
    [DataType(DataType.Password)]
    [Display(Name = "Passwort bestätigen")]
    [Compare("NewPassword", ErrorMessage = "Die Passwörter stimmen nicht überein.")]
    public string? ConfirmPassword { get; set; }

    /// <summary>
    /// Gets or sets die berufliche Position.
    /// </summary>
    [Display(Name = "Berufliche Position")]
    public string? Position { get; set; }

    /// <summary>
    /// Gets or sets den Tech-Stack.
    /// </summary>
    [Display(Name = "Tech-Stack (kommagetrennt)")]
    public string? TechStack { get; set; }

    /// <summary>
    /// Gets or sets die Straße.
    /// </summary>
    [Display(Name = "Straße")]
    public string? Street { get; set; }

    /// <summary>
    /// Gets or sets die Hausnummer.
    /// </summary>
    [Display(Name = "Hausnummer")]
    public string? HouseNumber { get; set; }

    /// <summary>
    /// Gets or sets die Stadt.
    /// </summary>
    [Display(Name = "Stadt")]
    public string? City { get; set; }

    /// <summary>
    /// Gets or sets das Land.
    /// </summary>
    [Display(Name = "Land")]
    public string? Country { get; set; }

    /// <summary>
    /// Gets or sets das hochzuladende Profilbild.
    /// </summary>
    [Display(Name = "Profilbild")]
    public IFormFile? AvatarFile { get; set; }
}
