using System.Net;
using AutoFixture;
using CustomerService.Data.Models;
using CustomerService.Features.Commands.UserInfoCommands.DeleteUser;
using CustomerService.Repositories;
using CustomerService.Test.Extensions;
using FoodOrderApis.Common.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace CustomerService.Test.Features.Commands.DeleteUser;

[TestFixture]
public class DeleteUserHandlerTests
{
    private readonly Mock<IUnitOfRepository> _unitOfRepositoryMock; 
    private readonly Mock<ILogger<DeleteUserHandler>> _loggerMock;
    private readonly Fixture _fixture;
    private readonly DeleteUserHandler _handler;
    private readonly CancellationToken _cancellationToken;

    public DeleteUserHandlerTests()
    {
        _unitOfRepositoryMock = new Mock<IUnitOfRepository>();
        _loggerMock = new Mock<ILogger<DeleteUserHandler>>();
        _fixture = new Fixture().OmitOnRecursionBehavior();
        _cancellationToken = new CancellationToken();
        _handler = new DeleteUserHandler(_unitOfRepositoryMock.Object, _loggerMock.Object);
    }

    [Test]
    public async Task Handle_ShouldBeReturn_StatusOK()
    {
        // Arrange
        var user = _fixture.Build<UserInfo>().With(u => u.IsActive, true).Create();
        _unitOfRepositoryMock.Setup(p => p.User.GetById(user.Id)).ReturnsAsync(user);
        _unitOfRepositoryMock.Setup(p => p.User.Update(It.IsAny<UserInfo>())).Returns(true);

        var command = new DeleteUserCommand
        {
            UserId = user.Id,
        };

        // Act
        var result = await _handler.Handle(command, _cancellationToken);
        
        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo((int)ResponseStatusCode.OK));

    }
    [Test]
    public async Task Handle_ShouldBeReturn_StatusNotFound()
    {
        // Arrange
        _unitOfRepositoryMock.Setup(p => p.User.GetById(It.IsAny<string>())).ReturnsAsync((UserInfo)null);

        var command = new DeleteUserCommand
        {
            UserId = "aaa"
        };

        // Act
        var result = await _handler.Handle(command, _cancellationToken);
        
        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo((int)ResponseStatusCode.NotFound));

    }
    
    [Test]
    public async Task Handle_ShouldBeReturn_StatusInternalServerError()
    {
        // Arrange
        _unitOfRepositoryMock.Setup(p => p.User.GetById(It.IsAny<string>())).ThrowsAsync(new Exception());

        var command = new DeleteUserCommand
        {
            UserId = "aaa"
        };

        // Act
        var result = await _handler.Handle(command, _cancellationToken);
        
        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo((int)ResponseStatusCode.InternalServerError));

    }
}
