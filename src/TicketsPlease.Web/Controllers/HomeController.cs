// <copyright file="HomeController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace TicketsPlease.Web.Controllers;

using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TicketsPlease.Web.Models;

internal class HomeController : Controller
{
  public IActionResult Index()
  {
    return this.View();
  }

  public IActionResult Privacy()
  {
    return this.View();
  }

  [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
  public IActionResult Error()
  {
    return this.View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
  }
}
