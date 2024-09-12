using AutoFixture;
using CustomerService.Data.Models;
using CustomerService.Data.Requests;
using CustomerService.Features.Commands.UserInfoCommands.CreateUser;
using CustomerService.Repositories;
using FoodOrderApis.Common.Helpers;
using Microsoft.Extensions.Logging;
using Moq;

namespace CustomerService.Test.Features.Commands.CreateUser;

public class CreateUserHandlerTests
{
    private readonly Mock<IUnitOfRepository> _unitOfRepositoryMock;
    private readonly Fixture _fixture;
    private readonly CancellationToken _cancellationToken;
    private readonly CreateUserHandler _handler;
    private readonly Mock<ILogger<CreateUserHandler>> _logger;

    public CreateUserHandlerTests()
    {
        _unitOfRepositoryMock = new Mock<IUnitOfRepository>();
        _fixture = new Fixture();
        _cancellationToken = new CancellationToken();
        _logger = new Mock<ILogger<CreateUserHandler>>();
        _handler = new CreateUserHandler(_unitOfRepositoryMock.Object, _logger.Object);
    }

    [Test]
    public async Task Handle_ShouldReturn_StatusCreated()
    {
        // Arrange
        var input = _fixture.Build<CreateUserInfoRequest>().With(x => x.PhoneNumber, "0323232323").Create();
        _unitOfRepositoryMock.Setup(p => p.User.Add(It.IsAny<UserInfo>())).ReturnsAsync(new UserInfo());

        var command = new CreateUserCommand
        {
            Payload = input
        };
        // Act 
        var result = await _handler.Handle(command, _cancellationToken);
        
        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo((int)ResponseStatusCode.Created));
    }
    
    [Test]
    public async Task Handle_ShouldReturn_StatusInternalServerError()
    {
        // Arrange
        var input = _fixture.Build<CreateUserInfoRequest>().With(x => x.PhoneNumber, "0323232323").Create();
        _unitOfRepositoryMock.Setup(p => p.User.Add(It.IsAny<UserInfo>())).ThrowsAsync(new Exception());

        var command = new CreateUserCommand
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
