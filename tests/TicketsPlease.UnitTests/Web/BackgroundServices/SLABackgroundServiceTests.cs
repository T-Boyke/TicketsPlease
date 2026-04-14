using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Moq;
using TicketsPlease.Application.Common.Interfaces;
using TicketsPlease.Domain.Entities;
using TicketsPlease.Domain.Enums;
using TicketsPlease.Web.BackgroundServices;
using Xunit;

namespace TicketsPlease.UnitTests.Web.BackgroundServices;

public class SLABackgroundServiceTests
{
    private readonly Mock<IServiceProvider> _serviceProviderMock;
    private readonly Mock<IServiceScopeFactory> _serviceScopeFactoryMock;
    private readonly Mock<IServiceScope> _serviceScopeMock;
    private readonly Mock<IOrganizationRepository> _orgRepositoryMock;
    private readonly Mock<ITicketRepository> _ticketRepositoryMock;
    private readonly Mock<INotificationService> _notificationServiceMock;
    private readonly Mock<ILogger<SLABackgroundService>> _loggerMock;

    public SLABackgroundServiceTests()
    {
        _serviceProviderMock = new Mock<IServiceProvider>();
        _serviceScopeFactoryMock = new Mock<IServiceScopeFactory>();
        _serviceScopeMock = new Mock<IServiceScope>();
        
        _orgRepositoryMock = new Mock<IOrganizationRepository>();
        _ticketRepositoryMock = new Mock<ITicketRepository>();
        _notificationServiceMock = new Mock<INotificationService>();
        _loggerMock = new Mock<ILogger<SLABackgroundService>>();

        _serviceProviderMock.Setup(x => x.GetService(typeof(IServiceScopeFactory))).Returns(_serviceScopeFactoryMock.Object);
        _serviceScopeFactoryMock.Setup(x => x.CreateScope()).Returns(_serviceScopeMock.Object);
        _serviceScopeMock.Setup(x => x.ServiceProvider).Returns(_serviceProviderMock.Object);

        _serviceProviderMock.Setup(x => x.GetService(typeof(IOrganizationRepository))).Returns(_orgRepositoryMock.Object);
        _serviceProviderMock.Setup(x => x.GetService(typeof(ITicketRepository))).Returns(_ticketRepositoryMock.Object);
        _serviceProviderMock.Setup(x => x.GetService(typeof(INotificationService))).Returns(_notificationServiceMock.Object);
    }

    private static void SetPrivateProperty<T>(T instance, string propertyName, object? value)
    {
        var prop = typeof(T).GetProperty(propertyName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        if (prop != null && prop.CanWrite)
        {
            prop.SetValue(instance, value);
            return;
        }
        
        var field = typeof(T).GetField($"<{propertyName}>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance);
        if (field == null)
            field = typeof(T).GetField($"_{char.ToLower(propertyName[0])}{propertyName.Substring(1)}", BindingFlags.NonPublic | BindingFlags.Instance);
            
        if (field != null)
            field.SetValue(instance, value);
        else
            throw new Exception($"Could not set property or field for {propertyName}");
    }

    [Fact]
    public async Task ExecuteAsync_TriggersSlaBreach_WhenOutsideQuietHours()
    {
        // Arrange
        var orgId = Guid.NewGuid();
        var org = new Organization 
        { 
            Id = orgId, 
            Name = "Test Org", 
            IsActive = true,
            NotifyOnMedium = true
        };

        var ticket = new Ticket("Test Ticket", TicketType.Task, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "To Do", "127.0.0.1");
        SetPrivateProperty(ticket, "TenantId", orgId);
        
        var priority = new TicketPriority();
        SetPrivateProperty(priority, "Name", "Medium");
        SetPrivateProperty(ticket, "Priority", priority);
        SetPrivateProperty(ticket, "ResponseDeadline", DateTime.UtcNow.AddMinutes(-5));

        _orgRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Organization> { org });

        _ticketRepositoryMock.Setup(x => x.GetAllActiveAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Ticket> { ticket });

        var service = new SLABackgroundService(_serviceProviderMock.Object, _loggerMock.Object);
        
        using var cts = new CancellationTokenSource();
        cts.CancelAfter(TimeSpan.FromMilliseconds(50));

        // Act
        await service.StartAsync(cts.Token);
        await Task.Delay(100);
        await service.StopAsync(CancellationToken.None);

        // Assert
        _notificationServiceMock.Verify(x => 
            x.SendNotificationToAllAsync(It.Is<string>(s => s.Contains("SLA BREACH")), It.IsAny<string>()), 
            Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_SkipsSlaBreach_WhenInsideQuietHours()
    {
        // Arrange
        var orgId = Guid.NewGuid();
        
        var org = new Organization 
        { 
            Id = orgId, 
            Name = "Test Org", 
            IsActive = true,
            QuietHoursStart = TimeSpan.Zero,
            QuietHoursEnd = new TimeSpan(23, 59, 59),
            TimeZoneId = "UTC",
            NotifyOnMedium = true
        };

        var ticket = new Ticket("Test Ticket", TicketType.Task, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "To Do", "127.0.0.1");
        SetPrivateProperty(ticket, "TenantId", orgId);
        
        var priority = new TicketPriority();
        SetPrivateProperty(priority, "Name", "Medium");
        SetPrivateProperty(ticket, "Priority", priority);
        SetPrivateProperty(ticket, "ResponseDeadline", DateTime.UtcNow.AddMinutes(-5));

        _orgRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Organization> { org });

        _ticketRepositoryMock.Setup(x => x.GetAllActiveAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Ticket> { ticket });

        var service = new SLABackgroundService(_serviceProviderMock.Object, _loggerMock.Object);
        
        using var cts = new CancellationTokenSource();
        cts.CancelAfter(TimeSpan.FromMilliseconds(50));

        // Act
        await service.StartAsync(cts.Token);
        await Task.Delay(100);
        await service.StopAsync(CancellationToken.None);

        // Assert
        _notificationServiceMock.Verify(x => 
            x.SendNotificationToAllAsync(It.IsAny<string>(), It.IsAny<string>()), 
            Times.Never);
    }
}
