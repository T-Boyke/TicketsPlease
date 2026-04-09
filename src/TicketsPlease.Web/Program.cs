// <copyright file="Program.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using TicketsPlease.Application.Common.Interfaces;
using TicketsPlease.Application.Services;
using TicketsPlease.Domain.Entities;
using TicketsPlease.Infrastructure.Persistence;
using TicketsPlease.Infrastructure.Repositories;
using TicketsPlease.Infrastructure.Services;
using TicketsPlease.Web.BackgroundServices;
using TicketsPlease.Web.Hubs;
using TicketsPlease.Web.Services;

[assembly: InternalsVisibleTo("TicketsPlease.IntegrationTests")]

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSignalR();
builder.Services.AddControllersWithViews()
    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
    .AddDataAnnotationsLocalization()
    .ConfigureApplicationPartManager(manager =>
    {
      var defaultProvider = manager.FeatureProviders.OfType<Microsoft.AspNetCore.Mvc.Controllers.ControllerFeatureProvider>().FirstOrDefault();
      if (defaultProvider != null)
      {
        manager.FeatureProviders.Remove(defaultProvider);
      }

      manager.FeatureProviders.Add(new TicketsPlease.Web.InternalControllerFeatureProvider());
    });

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services.AddOpenApi();

// Add Data Protection configuration to prevent cryptographic mismatches (Enterprise Standard)
builder.Services.AddDataProtection()
    .SetApplicationName("TicketsPlease");

// Identity Configuration
builder.Services.AddIdentity<User, Role>(options =>
{
  options.Password.RequireDigit = true;
  options.Password.RequireLowercase = true;
  options.Password.RequireNonAlphanumeric = true;
  options.Password.RequireUppercase = true;
  options.Password.RequiredLength = 8;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
  options.LoginPath = "/Account/Login";
  options.LogoutPath = "/Account/Logout";
  options.AccessDeniedPath = "/Account/AccessDenied";
});

// Enterprise Skinning / Theming
builder.Services.AddScoped<ICorporateSkinProvider, DefaultCorporateSkinProvider>();

// Data Access Layer
builder.Services.AddScoped<ITicketRepository, TicketRepository>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();
builder.Services.AddScoped<ITicketTemplateRepository, TicketTemplateRepository>();
builder.Services.AddScoped<IOrganizationRepository, OrganizationRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IFileAssetRepository, FileAssetRepository>();
builder.Services.AddScoped<ITeamRepository, TeamRepository>();
builder.Services.AddScoped<ITimeLogRepository, TimeLogRepository>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<ITicketService, TicketService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddScoped<ITicketTemplateService, TicketTemplateService>();
builder.Services.AddScoped<IOrganizationService, OrganizationService>();
builder.Services.AddScoped<IFileStorageService, LocalStorageService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<ITimeTrackingService, TimeTrackingService>();
builder.Services.AddScoped<ISubTicketService, SubTicketService>();
builder.Services.AddScoped<ITeamService, TeamService>();
builder.Services.AddScoped<INotificationService, SignalRNotificationService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHostedService<TicketCleanupWorker>();

// Database Configuration with Resilience

// Database Configuration with Resilience
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString, sqlOptions =>
    {
      // Enterprise Resilience: Enable automatic retries for transient failures
      sqlOptions.EnableRetryOnFailure(
          maxRetryCount: 5,
          maxRetryDelay: TimeSpan.FromSeconds(30),
          errorNumbersToAdd: null);
    }));

var app = builder.Build();

// Database Seeding in Development
if (app.Environment.IsDevelopment())
{
  await DbInitialiser.SeedAsync(app.Services).ConfigureAwait(false);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
  app.UseExceptionHandler("/Home/Error");

  // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
  app.UseHsts();
}

app.UseHttpsRedirection();

// Standard Security Headers (Enterprise Compliance)
app.Use((context, next) =>
{
  var csp = "default-src 'self'; script-src 'self'; style-src 'self' https://fonts.googleapis.com; font-src 'self' data: https://fonts.gstatic.com; img-src 'self' data: https:; frame-ancestors 'none';";

  if (app.Environment.IsDevelopment())
  {
    // Relax CSP in Development for Styleguide swatches, Browser Link, and Browser Refresh
    csp = "default-src 'self' http://localhost:* ws://localhost:*; connect-src 'self' http://localhost:* ws: wss:; script-src 'self' 'unsafe-inline'; style-src 'self' 'unsafe-inline' https://fonts.googleapis.com; font-src 'self' data: https://fonts.gstatic.com; img-src 'self' data: https:; frame-ancestors 'none';";
  }

  context.Response.Headers.Append("Content-Security-Policy", csp);
  context.Response.Headers.Append("X-Frame-Options", "DENY");
  context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
  context.Response.Headers.Append("Referrer-Policy", "strict-origin-when-cross-origin");
  context.Response.Headers.Append("Cross-Origin-Opener-Policy", "same-origin");
  return next();
});

// Localization Middleware
var supportedCultures = new[] { "de", "en" };
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);

app.UseRequestLocalization(localizationOptions);

app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();
app.MapOpenApi();
app.MapScalarApiReference(options =>
{
  options
    .WithTitle("TicketsPlease 🔵 Professional API")
    .WithTheme(ScalarTheme.BluePlanet)
    .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapHub<NotificationHub>("/notificationHub");

await app.RunAsync().ConfigureAwait(false);

/// <summary>
/// Ermöglicht den Zugriff auf die Program-Klasse für Integrations-Tests.
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1515:Consider making public types internal", Justification = "Required for WebApplicationFactory in IntegrationTests")]
public partial class Program
{
  /// <summary>
  /// Initializes a new instance of the <see cref="Program"/> class.
  /// </summary>
  protected Program()
  {
  }
}
