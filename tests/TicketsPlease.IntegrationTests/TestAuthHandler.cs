// <copyright file="TestAuthHandler.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.IntegrationTests;

using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

/// <summary>
/// Ein Authentifizierungs-Handler für Tests, der eine Identität basierend auf Header-Werten simuliert.
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1812:Avoid uninstantiated internal classes", Justification = "Instantiated by DI in IntegrationTestBase")]
internal sealed class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
  /// <summary>
  /// Der Name des Authentifizierungsschemas für Tests.
  /// </summary>
  public const string AuthenticationScheme = "TestAuth";

  /// <summary>
  /// Header-Name für die Benutzer-ID.
  /// </summary>
  public const string UserIdHeader = "X-Test-UserId";

  /// <summary>
  /// Header-Name für den Rollennamen.
  /// </summary>
  public const string RoleHeader = "X-Test-Role";

  /// <summary>
  /// Header-Name für die Tenant-ID.
  /// </summary>
  public const string TenantIdHeader = "X-Test-TenantId";

  /// <summary>
  /// Initializes a new instance of the <see cref="TestAuthHandler"/> class.
  /// </summary>
  /// <param name="options">Die Optionen.</param>
  /// <param name="logger">Der Logger.</param>
  /// <param name="encoder">Der Url Encoder.</param>
  public TestAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder)
      : base(options, logger, encoder)
  {
  }

  /// <inheritdoc />
  protected override Task<AuthenticateResult> HandleAuthenticateAsync()
  {
    if (!this.Context.Request.Headers.TryGetValue(UserIdHeader, out var userIdValues))
    {
      return Task.FromResult(AuthenticateResult.NoResult());
    }

    var userId = userIdValues.ToString();
    var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, userId),
        new Claim(ClaimTypes.Name, $"TestUser_{userId}"),
    };

    if (this.Context.Request.Headers.TryGetValue(RoleHeader, out var roleValues))
    {
      foreach (var role in roleValues)
      {
        if (!string.IsNullOrEmpty(role))
        {
          claims.Add(new Claim(ClaimTypes.Role, role));
        }
      }
    }
    else
    {
      claims.Add(new Claim(ClaimTypes.Role, "Admin"));
    }

    if (this.Context.Request.Headers.TryGetValue(TenantIdHeader, out var tenantValues))
    {
      claims.Add(new Claim("TenantId", tenantValues.ToString()));
    }
    else
    {
      claims.Add(new Claim("TenantId", IntegrationTestBase.TestTenantId.ToString()));
    }

    var identity = new ClaimsIdentity(claims, AuthenticationScheme);
    var principal = new ClaimsPrincipal(identity);
    var ticket = new AuthenticationTicket(principal, AuthenticationScheme);

    return Task.FromResult(AuthenticateResult.Success(ticket));
  }
}
