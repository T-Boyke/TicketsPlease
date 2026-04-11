using System.Security.Claims;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TicketsPlease.Application.Common.Interfaces;
using TicketsPlease.Domain.Entities;
using TicketsPlease.Web.Controllers;
using TicketsPlease.Web.Models.Account;

namespace TicketsPlease.UnitTests.Web.Controllers;

public class AccountControllerTests
{
    private readonly Mock<SignInManager<User>> _signInManagerMock;
    private readonly Mock<UserManager<User>> _userManagerMock;
    private readonly Mock<RoleManager<Role>> _roleManagerMock;
    private readonly Mock<IUserRepository> _userRepositoryMock = new();
    private readonly Mock<IOrganizationService> _organizationServiceMock = new();
    private readonly Mock<IDashboardService> _dashboardServiceMock = new();
    private readonly Mock<IFileStorageService> _fileStorageServiceMock = new();
    private readonly AccountController _controller;

    public AccountControllerTests()
    {
        var store = new Mock<IUserStore<User>>();
        _userManagerMock = new Mock<UserManager<User>>(store.Object, null!, null!, null!, null!, null!, null!, null!, null!);

        var contextAccessor = new Mock<IHttpContextAccessor>();
        var claimsFactory = new Mock<IUserClaimsPrincipalFactory<User>>();
        _signInManagerMock = new Mock<SignInManager<User>>(_userManagerMock.Object, contextAccessor.Object, claimsFactory.Object, null!, null!, null!, null!);

        var roleStore = new Mock<IRoleStore<Role>>();
        _roleManagerMock = new Mock<RoleManager<Role>>(roleStore.Object, null!, null!, null!, null!);

        _controller = new AccountController(
            _signInManagerMock.Object,
            _userManagerMock.Object,
            _roleManagerMock.Object,
            _userRepositoryMock.Object,
            _organizationServiceMock.Object,
            _dashboardServiceMock.Object,
            _fileStorageServiceMock.Object);
    }

    [Fact]
    public void Login_ReturnsViewResult()
    {
        // Act
        var result = _controller.Login();

        // Assert
        result.Should().BeOfType<ViewResult>();
    }

    [Fact]
    public async Task Logout_RedirectsToHome()
    {
        // Act
        var result = await _controller.Logout();

        // Assert
        var redirectResult = result.Should().BeOfType<RedirectToActionResult>().Subject;
        redirectResult.ActionName.Should().Be("Index");
        redirectResult.ControllerName.Should().Be("Home");
        _signInManagerMock.Verify(x => x.SignOutAsync(), Times.Once);
    }

    [Fact]
    public void Register_ReturnsViewResult()
    {
        // Act
        var result = _controller.Register();

        // Assert
        result.Should().BeOfType<ViewResult>();
    }

    [Fact]
    public void AccessDenied_ReturnsViewResult()
    {
        // Act
        var result = _controller.AccessDenied();

        // Assert
        result.Should().BeOfType<ViewResult>();
    }
}
