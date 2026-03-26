// <copyright file="BaseEntityTests.cs" company="BitLC-NE-2025-2026">
// Copyright (c) BitLC-NE-2025-2026. All rights reserved.
// </copyright>

namespace TicketsPlease.UnitTests.Common;

using FluentAssertions;
using TicketsPlease.Domain.Common;
using Xunit;

public class BaseEntityTests
{
  [Fact]
  public void Constructor_ShouldInitializeWithNewGuid()
  {
    // Act
    var entity = new TestEntity();

    // Assert
    entity.Id.Should().NotBeEmpty();
  }

  [Fact]
  public void Id_ShouldBeSettable()
  {
    // Arrange
    var entity = new TestEntity();
    var newId = Guid.NewGuid();

    // Act
    entity.Id = newId;

    // Assert
    entity.Id.Should().Be(newId);
  }

  [Fact]
  public void DomainEvents_ShouldBeEmptyByDefault()
  {
    // Act
    var entity = new TestEntity();

    // Assert
    entity.DomainEvents.Should().BeEmpty();
  }

  [Fact]
  public void AddDomainEvent_ShouldAddEventToList()
  {
    // Arrange
    var entity = new TestEntity();
    var domainEvent = new TestDomainEvent();

    // Act
    entity.AddDomainEvent(domainEvent);

    // Assert
    entity.DomainEvents.Should().Contain(domainEvent);
  }

  [Fact]
  public void RemoveDomainEvent_ShouldRemoveEventFromList()
  {
    // Arrange
    var entity = new TestEntity();
    var domainEvent = new TestDomainEvent();
    entity.AddDomainEvent(domainEvent);

    // Act
    entity.RemoveDomainEvent(domainEvent);

    // Assert
    entity.DomainEvents.Should().NotContain(domainEvent);
  }

  [Fact]
  public void ClearDomainEvents_ShouldEmptyList()
  {
    // Arrange
    var entity = new TestEntity();
    entity.AddDomainEvent(new TestDomainEvent());

    // Act
    entity.ClearDomainEvents();

    // Assert
    entity.DomainEvents.Should().BeEmpty();
  }

  [Fact]
  public void Properties_ShouldBeSettable()
  {
    // Arrange
    var entity = new TestEntity();
    var tenantId = Guid.NewGuid();
    var rowVersion = new byte[] { 1, 2, 3 };

    // Act
    entity.TenantId = tenantId;
    entity.IsDeleted = true;
    entity.DeletedAt = DateTime.UtcNow;
    entity.RowVersion = rowVersion;

    // Assert
    entity.TenantId.Should().Be(tenantId);
    entity.IsDeleted.Should().BeTrue();
    entity.DeletedAt.Should().NotBeNull();
    entity.RowVersion.Should().BeEquivalentTo(rowVersion);
  }

  private class TestEntity : BaseEntity
  {
  }

  private class TestDomainEvent : IDomainEvent
  {
    public DateTime OccurredOn => DateTime.UtcNow;
  }
}
