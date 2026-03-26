// <copyright file="FileAssetTests.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.UnitTests.Entities;

using System;
using FluentAssertions;
using TicketsPlease.Domain.Entities;
using Xunit;

public class FileAssetTests
{
  [Fact]
  public void Properties_ShouldBeSettable()
  {
    // Arrange
    var asset = new FileAsset
    {
      FileName = "logo.png",
      ContentType = "image/png",
      SizeBytes = 1024,
      BlobPath = "/blobs/123",
      TenantId = Guid.NewGuid()
    };

    // Assert
    asset.FileName.Should().Be("logo.png");
    asset.SizeBytes.Should().Be(1024);
  }
}
