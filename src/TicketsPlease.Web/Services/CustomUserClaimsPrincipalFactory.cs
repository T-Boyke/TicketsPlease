// <copyright file="CustomUserClaimsPrincipalFactory.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.Web.Services;

using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using TicketsPlease.Domain.Entities;

/// <summary>
/// Erweiterung der Standard-Claims-Generierung, um den TenantId-Claim hinzuzufügen.
/// </summary>
public class CustomUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<User, Role>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CustomUserClaimsPrincipalFactory"/> class.
    /// </summary>
    /// <param name="userManager">Der UserManager.</param>
    /// <param name="roleManager">Der RoleManager.</param>
    /// <param name="optionsAccessor">Die Identity Options.</param>
    public CustomUserClaimsPrincipalFactory(
        UserManager<User> userManager,
        RoleManager<Role> roleManager,
        IOptions<IdentityOptions> optionsAccessor)
        : base(userManager, roleManager, optionsAccessor)
    {
    }

    /// <inheritdoc />
    protected override async Task<ClaimsIdentity> GenerateClaimsAsync(User user)
    {
        var identity = await base.GenerateClaimsAsync(user).ConfigureAwait(false);
        
        if (user.TenantId != Guid.Empty)
        {
            identity.AddClaim(new Claim("TenantId", user.TenantId.ToString()));
        }
        
        return identity;
    }
}
