using AuthServer.Data.Models;
using AuthServer.Features.Commands.DeleteUser;
using AuthServer.Repositories;
using AuthServer.Test.Extensions;
using AutoFixture;
using FoodOrderApis.Common.Helpers;
using FoodOrderApis.Common.HttpContextCustom;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace AuthServer.Test.Features.Commands.DeleteUser;

public class DeleteUserHandlerTests
{
    private readonly Mock<IUnitOfRepository> _unitOfRepositoryMock;
    private readonly Fixture _fixture;
    private readonly DeleteUserHandler _handler;
    private readonly CancellationToken _cancellationToken;
    private readonly Mock<ICustomHttpContextAccessor> _httpContextAccessorMock;

    public DeleteUserHandlerTests()
    {
        _unitOfRepositoryMock = new Mock<IUnitOfRepository>();
        _fixture = new Fixture().OmitOnRecursionBehavior();
        _cancellationToken = new CancellationToken();
        _httpContextAccessorMock = new Mock<ICustomHttpContextAccessor>();
        _handler = new DeleteUserHandler(_unitOfRepositoryMock.Object, _httpContextAccessorMock.Object);
    }

    [Test]
    public async Task Handle_ShouReturn_StatusSuccess()
    {
        var user = _fixture.Build<User>().With(u => u.IsActive, true).Create();
        var userId = user.Id;
        
        _httpContextAccessorMock.Setup(p => p.GetCurrentUserId()).Returns(userId);
        _unitOfRepositoryMock.Setup(p => p.User.GetById(It.IsAny<string>())).ReturnsAsync(user);
        _unitOfRepositoryMock.Setup(p => p.User.Update(It.IsAny<User>())).Returns(true);

        var command = new DeleteUserCommand
        {
            UserId = userId
        };
        
        var act = await _handler.Handle(command, _cancellationToken);
        
        // Assert
        Assert.That(act, Is.Not.Null);
        Assert.That(act.StatusCode, Is.EqualTo((int)ResponseStatusCode.OK));
    }
    
    [Test]
    public async Task Handle_ShouReturn_StatusForbidden()
    {
        // Arrange
        var currentUserId = _fixture.Create<Guid>().ToString();
        var userIdRequest = _fixture.Create<Guid>().ToString();
        _httpContextAccessorMock.Setup(p => p.GetCurrentUserId()).Returns(currentUserId);

        var command = new DeleteUserCommand
        {
            UserId = userIdRequest
        };
        
        // Act 
        var act = await _handler.Handle(command, _cancellationToken);
        
        // Assert
        Assert.That(act, Is.Not.Null);
        Assert.That(act.StatusCode, Is.EqualTo((int)ResponseStatusCode.Forbidden));
    }
    
    [Test]
    public async Task Handle_ShouReturn_StatusNotFound()
    {
        // Arrange
        var userId = _fixture.Create<Guid>().ToString();
        _httpContextAccessorMock.Setup(p => p.GetCurrentUserId()).Returns(userId);
        _unitOfRepositoryMock.Setup(p => p.User.GetById(userId)).ReturnsAsync((User)null);
        
        var command = new DeleteUserCommand
        {
            UserId = userId
        };
        
        // Act 
        var act = await _handler.Handle(command, _cancellationToken);
        
        // Assert
        Assert.That(act, Is.Not.Null);
        Assert.That(act.StatusCode, Is.EqualTo((int)ResponseStatusCode.NotFound));
    }
    
    [Test]
    public async Task Handle_ShouReturn_StatusBadRequest()
    {
        // Arrange
        var user = _fixture.Build<User>().With(u => u.IsActive, false).Create();
        var userId = user.Id;
        
        _httpContextAccessorMock.Setup(p => p.GetCurrentUserId()).Returns(userId);
        _unitOfRepositoryMock.Setup(p => p.User.GetById(It.IsAny<string>())).ReturnsAsync(user);
        _unitOfRepositoryMock.Setup(p => p.User.Update(It.IsAny<User>())).Returns(true);

        var command = new DeleteUserCommand
        {
            UserId = userId
        };
        
        // Act
        var act = await _handler.Handle(command, _cancellationToken);
        
        // Assert
        Assert.That(act, Is.Not.Null);
        Assert.That(act.StatusCode, Is.EqualTo((int)ResponseStatusCode.BadRequest));
    }
    [Test]
    public async Task Handle_ShouReturn_StatusInternalServerError()
    {
        // Arrange
        var userId = "a";
        
        _httpContextAccessorMock.Setup(p => p.GetCurrentUserId()).Throws(new Exception("Error"));

        var command = new DeleteUserCommand
        {
            UserId = userId
        };
        
        // Act
        var act = await _handler.Handle(command, _cancellationToken);
        
        // Assert
        Console.WriteLine(act.ErrorMessage);
        Assert.That(act, Is.Not.Null);
        Assert.That(act.StatusCode, Is.EqualTo((int)ResponseStatusCode.InternalServerError));
    }
}
