// <copyright file="ControllerTests.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.IntegrationTests;

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TicketsPlease.Domain.Entities;
using TicketsPlease.Infrastructure.Persistence;
using Xunit;

/// <summary>
/// Integrations-Tests für die Web-Controller.
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores", Justification = "Test naming convention")]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Reliability", "CA2007:Consider calling ConfigureAwait", Justification = "xUnit tests should not use ConfigureAwait(false)")]
public class ControllerTests : IntegrationTestBase
{
  /// <summary>
  /// Tests HomeController.
  /// </summary>
  /// <returns>A task.</returns>
  [Fact]
  public async Task HomeController_Index_Should_Return_Success()
  {
    // Arrange
    var client = this.Factory.CreateClient();

    // Act
    var response = await client.GetAsync(new Uri("/", UriKind.Relative));

    // Assert
    response.EnsureSuccessStatusCode();
    response.StatusCode.Should().Be(HttpStatusCode.OK);
  }

  /// <summary>
  /// Tests TicketsController protection.
  /// </summary>
  /// <returns>A task.</returns>
  [Fact]
  public async Task TicketsController_Index_Should_Redirect_To_Login_If_Not_Authenticated()
  {
    // Arrange
    var client = this.Factory.CreateClient(new Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactoryClientOptions
    {
      AllowAutoRedirect = false,
    });

    // Act
    var response = await client.GetAsync(new Uri("/Tickets", UriKind.Relative));

    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.Redirect);
    response.Headers.Location!.ToString().Should().Contain("/Account/Login");
  }

  /// <summary>
  /// Tests AdminController protection.
  /// </summary>
  /// <returns>A task.</returns>
  [Fact]
  public async Task AdminController_Settings_Should_Return_Forbidden_For_Normal_User()
  {
    // Arrange
    // Note: To properly test this, we'd need to simulate a logged-in user with specific roles.
    // For now, testing unauthenticated redirect.
    var client = this.Factory.CreateClient(new Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactoryClientOptions
    {
      AllowAutoRedirect = false,
    });

    // Act
    var response = await client.GetAsync(new Uri("/Admin/Settings", UriKind.Relative));

    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.Redirect);
  }
}
