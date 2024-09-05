using AutoFixture;
using CustomerService.Data.Models;
using CustomerService.Data.Requests;
using CustomerService.Features.Commands.UserInfoCommands.UpdateUser;
using CustomerService.Repositories;
using FoodOrderApis.Common.Helpers;
using FoodOrderApis.Common.HttpContextCustom;
using FoodOrderApis.Common.MassTransit.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace CustomerService.Test.Features.Commands.UpdateUser;

public class UpdateUserHandlerTests
{
    private readonly Mock<IUnitOfRepository> _unitOfRepositoryMock;
    private readonly Mock<ISendEndpointCustomProvider> _sendEndpointMock;
    private readonly Fixture _fixture;
    private readonly CancellationToken _cancellationToken;
    private readonly UpdateUserHandler _handler;
    private readonly Mock<ICustomHttpContextAccessor> _httpContextAccessorMock;

    public UpdateUserHandlerTests()
    {
        _unitOfRepositoryMock = new Mock<IUnitOfRepository>();
        _sendEndpointMock = new Mock<ISendEndpointCustomProvider>();
        _fixture = new Fixture();
        _cancellationToken = new CancellationToken();
        _httpContextAccessorMock = new Mock<ICustomHttpContextAccessor>();
        _handler = new UpdateUserHandler(_unitOfRepositoryMock.Object, _sendEndpointMock.Object, _httpContextAccessorMock.Object);
    }

    [Test]
    public async Task Handle_ShouldReturn_StatusOK()
    {
        // Arrange
        var userId = _fixture.Create<Guid>().ToString();
        var user = _fixture.Build<UserInfo>().With(x => x.Id, userId).Create();
        var input = _fixture.Build<UpdateUserInfoInput>().With(x => x.Id, userId).With(x => x.PhoneNumber, "0323121452").Create();
        
        _httpContextAccessorMock.Setup(p => p.GetCurrentUserId()).Returns(userId);
        _unitOfRepositoryMock.Setup(p => p.User.GetById(It.IsAny<string>())).ReturnsAsync(user);
        _unitOfRepositoryMock.Setup(p => p.User.Update(It.IsAny<UserInfo>())).Returns(true);

        var command = new UpdateUserCommand
        {
            Payload = input
        };
        
        // Act
        var result = await _handler.Handle(command, _cancellationToken);
        
        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo((int)ResponseStatusCode.OK));
    }
    
    [Test]
    public async Task Handle_ShouldReturn_StatusForbidden()
    {
        // Arrange
        var userId = _fixture.Create<Guid>().ToString();
        var input = _fixture.Build<UpdateUserInfoInput>().With(x => x.PhoneNumber, "0323121452").Create();
        
        _httpContextAccessorMock.Setup(p => p.GetCurrentUserId()).Returns(userId);

        var command = new UpdateUserCommand
        {
            Payload = input
        };
        
        // Act
        var result = await _handler.Handle(command, _cancellationToken);
        
        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo((int)ResponseStatusCode.Forbidden));
    }
    
    [Test]
    public async Task Handle_ShouldReturn_StatusNotFound()
    {
        // Arrange
        var userId = _fixture.Create<Guid>().ToString();
        var input = _fixture.Build<UpdateUserInfoInput>().With(x => x.Id, userId).With(x => x.PhoneNumber, "0323121452").Create();
        
        _httpContextAccessorMock.Setup(p => p.GetCurrentUserId()).Returns(userId);
        _unitOfRepositoryMock.Setup(p => p.User.GetById(It.IsAny<string>())).ReturnsAsync((UserInfo)null);

        var command = new UpdateUserCommand
        {
            Payload = input
        };
        
        // Act
        var result = await _handler.Handle(command, _cancellationToken);
        
        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo((int)ResponseStatusCode.NotFound));
    }
    
    [Test]
    public async Task Handle_ShouldReturn_StatusInternalServerError()
    {
        // Arrange
        var userId = _fixture.Create<Guid>().ToString();
        var input = _fixture.Build<UpdateUserInfoInput>().With(x => x.Id, userId).With(x => x.PhoneNumber, "0323121452").Create();
        
        _httpContextAccessorMock.Setup(p => p.GetCurrentUserId()).Throws(new Exception("Error"));

        var command = new UpdateUserCommand
        {
            Payload = input
        };
        
        // Act
        var result = await _handler.Handle(command, _cancellationToken);
        
        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo((int)ResponseStatusCode.InternalServerError));
    }
}
