// <copyright file="CommentTests.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.UnitTests.Entities;

using System;
using FluentAssertions;
using TicketsPlease.Domain.Entities;
using Xunit;

public class CommentTests
{
  [Fact]
  public void Constructor_WithValidData_ShouldInitializeProperties()
  {
    // Arrange
    var ticketId = Guid.NewGuid();
    var authorId = Guid.NewGuid();
    var content = "Test comment";

    // Act
    var comment = new Comment(content, ticketId, authorId);

    // Assert
    comment.TicketId.Should().Be(ticketId);
    comment.AuthorId.Should().Be(authorId);
    comment.Content.Should().Be(content);
    comment.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
  }

  [Theory]
  [InlineData(null)]
  [InlineData("")]
  [InlineData(" ")]
  public void Constructor_WithInvalidContent_ShouldThrowException(string? content)
  {
    // Act
    Action act = () => _ = new Comment(content!, Guid.NewGuid(), Guid.NewGuid());

    // Assert
    act.Should().Throw<ArgumentException>().WithMessage("*Inhalt darf nicht leer sein.*");
  }

  [Fact]
  public void UpdateContent_WithValidContent_ShouldUpdateProperties()
  {
    // Arrange
    var comment = new Comment("Initial", Guid.NewGuid(), Guid.NewGuid());
    var newContent = "Updated content";

    // Act
    comment.UpdateContent(newContent);

    // Assert
    comment.Content.Should().Be(newContent);
    comment.UpdatedAt.Should().NotBeNull();
  }
}
