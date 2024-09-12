using AuthServer.Data.Models;
using AuthServer.Data.Requests;
using AuthServer.Features.Commands.UpdateUser;
using AuthServer.Repositories;
using AuthServer.Test.Extensions;
using AutoFixture;
using FluentAssertions;
using FoodOrderApis.Common.Helpers;
using FoodOrderApis.Common.HttpContextCustom;
using FoodOrderApis.Common.MassTransit.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace AuthServer.Test.Features.Commands.UpdateUser;

public class UpdateUserHandlerTests
{
    private readonly Mock<IUnitOfRepository> _unitOfRepositoryMock;
    private readonly Mock<UserManager<User>> _userManagerMock;
    private readonly Mock<ICustomHttpContextAccessor> _httpContextAccessorMock;
    private readonly Fixture _fixture;
    private readonly UpdateUserHandler _handler;
    private readonly CancellationToken _cancellationToken;
    private readonly Mock<ILogger<UpdateUserHandler>> _loggerMock;
    public UpdateUserHandlerTests()
    {
        _unitOfRepositoryMock = new Mock<IUnitOfRepository>();
        _userManagerMock  = new Mock<UserManager<User>>(
            new Mock<IUserStore<User>>().Object,
            new Mock<IOptions<IdentityOptions>>().Object,
            new Mock<IPasswordHasher<User>>().Object,
            new IUserValidator<User>[0],
            new IPasswordValidator<User>[0],
            new Mock<ILookupNormalizer>().Object,
            new Mock<IdentityErrorDescriber>().Object,
            new Mock<IServiceProvider>().Object,
            new Mock<ILogger<UserManager<User>>>().Object);
        _httpContextAccessorMock = new Mock<ICustomHttpContextAccessor>();
        _fixture = new Fixture().OmitOnRecursionBehavior();
        _loggerMock = new Mock<ILogger<UpdateUserHandler>>();
        _handler = new UpdateUserHandler(_unitOfRepositoryMock.Object, _userManagerMock.Object, _httpContextAccessorMock.Object, _loggerMock.Object);
        _cancellationToken = new CancellationToken();
    }

    [Test]
    public async Task Handle_ShouldReturn_StatusOK()
    {
        var user = _fixture.Build<User>().Create();
        var payload = new UpdateUserRequest
        {
            UserId = user.Id,
            OldPassword = "String123@",
            NewPassword = "String1234@",
        };
        // Arrange
        _httpContextAccessorMock.Setup(p => p.GetCurrentUserId()).Returns(user.Id);
        _unitOfRepositoryMock.Setup(p => p.User.GetById(user.Id)).ReturnsAsync(user);
        _userManagerMock.Setup(p => p.CheckPasswordAsync(user, payload.OldPassword)).ReturnsAsync(true);
        _userManagerMock.Setup(p => p.UpdateAsync(It.IsAny<User>())).ReturnsAsync(IdentityResult.Success);

        var command = new UpdateUserCommand
        {
            Payload = payload
        };
        // Act
        var act = await _handler.Handle(command, _cancellationToken);
        
        // Assert
        Assert.That(act, Is.Not.Null);
        Assert.That(act.StatusCode, Is.EqualTo((int)ResponseStatusCode.OK) );
    }
    
    [Test]
    public async Task Handle_ShouldReturn_StatusForbidden()
    {
        var user = _fixture.Build<User>().Create();
        var payload = new UpdateUserRequest
        {
            UserId = user.Id,
            OldPassword = "String123@",
            NewPassword = "String1234@",
        };
        // Arrange
        _httpContextAccessorMock.Setup(p => p.GetCurrentUserId()).Returns("a");

        var command = new UpdateUserCommand
        {
            Payload = payload
        };
        // Act
        var act = await _handler.Handle(command, _cancellationToken);
        
        // Assert
        Assert.That(act, Is.Not.Null);
        Assert.That(act.StatusCode, Is.EqualTo((int)ResponseStatusCode.Forbidden) );
    }
    
    [Test]
    public async Task Handle_ShouldReturn_StatusNotFound()
    {
        var user = _fixture.Build<User>().Create();
        var payload = new UpdateUserRequest
        {
            UserId = user.Id,
            OldPassword = "String123@",
            NewPassword = "String1234@",
        };
        // Arrange
        _httpContextAccessorMock.Setup(p => p.GetCurrentUserId()).Returns(user.Id);
        _unitOfRepositoryMock.Setup(p => p.User.GetById(user.Id)).ReturnsAsync((User)null);

        var command = new UpdateUserCommand
        {
            Payload = payload
        };
        // Act
        var act = await _handler.Handle(command, _cancellationToken);
        
        // Assert
        Assert.That(act, Is.Not.Null);
        Assert.That(act.StatusCode, Is.EqualTo((int)ResponseStatusCode.NotFound) );
    }

    [Test] public async Task Handle_ShouldReturn_StatusInternalServerError()
    {
        var user = _fixture.Build<User>().Create();
        var payload = new UpdateUserRequest
        {
            UserId = user.Id,
            OldPassword = "String123@",
            NewPassword = "String1234@",
        };
        // Arrange
        _httpContextAccessorMock.Setup(p => p.GetCurrentUserId()).Throws(new Exception("Error"));

        var command = new UpdateUserCommand
        {
            Payload = payload
        };
        // Act
        var act = await _handler.Handle(command, _cancellationToken);
        
        // Assert
        Assert.That(act, Is.Not.Null);
        Assert.That(act.StatusCode, Is.EqualTo((int)ResponseStatusCode.InternalServerError) );
    }
}
