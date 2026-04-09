// <copyright file="EditProfileViewModel.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Web.Models.Account;

using System.ComponentModel.DataAnnotations;

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
}
