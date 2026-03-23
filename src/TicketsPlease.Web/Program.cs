// <copyright file="Program.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Microsoft.EntityFrameworkCore;
using TicketsPlease.Application.Common.Interfaces;
using TicketsPlease.Infrastructure.Persistence;
using TicketsPlease.Infrastructure.Repositories;
using TicketsPlease.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddOpenApi();

// Enterprise Skinning / Theming
builder.Services.AddScoped<ICorporateSkinProvider, DefaultCorporateSkinProvider>();

// Data Access Layer
builder.Services.AddScoped<ITicketRepository, TicketRepository>();

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
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapOpenApi();
app.MapScalarApiReference(options =>
{
  options
    .WithTitle("TicketsPlease 🔵 Professional API")
    .WithTheme(ScalarTheme.BluePlanet)
    .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient)
    .WithSidebar(true);
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();

/// <summary>
/// Ermöglicht den Zugriff auf die Program-Klasse für Integrations-Tests.
/// </summary>
internal partial class Program
{
}
