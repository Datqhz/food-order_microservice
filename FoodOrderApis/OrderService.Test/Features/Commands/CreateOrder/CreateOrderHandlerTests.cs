using AutoFixture;
using FoodOrderApis.Common.Helpers;
using Microsoft.Extensions.Logging;
using Moq;
using OrderService.Data.Models;
using OrderService.Data.Requests;
using OrderService.Features.Commands.OrderCommands.CreateOrder;
using OrderService.Repositories;
using OrderService.Test.Extensions;

namespace OrderService.Test.Features.Commands.CreateOrder;

public class CreateOrderHandlerTests
{
    private readonly Mock<IUnitOfRepository> _unitOfRepositoryMock; 
    private readonly Fixture _fixture;
    private readonly CancellationToken _cancellationToken;
    private readonly CreateOrderHandler _handler;
    private readonly Mock<ILogger<CreateOrderHandler>> _loggerMock;

    public CreateOrderHandlerTests()
    {
        _unitOfRepositoryMock = new Mock<IUnitOfRepository>();
        _fixture = new Fixture().OmitOnRecursionBehavior();
        _cancellationToken = new CancellationToken();   
        _loggerMock = new Mock<ILogger<CreateOrderHandler>>();
        _handler = new CreateOrderHandler(_unitOfRepositoryMock.Object, _loggerMock.Object);
    }

    [Test]
    public async Task Handle_ShouldReturn_StatusCreated()
    {
        // Arrange
        var input = _fixture.Create<CreateOrderInput>();
        var user = _fixture.Create<User>();
        
        _unitOfRepositoryMock.Setup(p => p.User.GetById(It.IsAny<string>())).ReturnsAsync(user);
        _unitOfRepositoryMock.Setup(p => p.Order.Add(It.IsAny<Order>())).ReturnsAsync(new Order());

        var command = new CreateOrderCommand
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
    public async Task Handle_ShouldReturn_StatusNotFound_User()
    {
        // Arrange
        var input = _fixture.Create<CreateOrderInput>();
        
        _unitOfRepositoryMock.Setup(p => p.User.GetById(It.IsAny<string>())).ReturnsAsync((User)null);
        var command = new CreateOrderCommand
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
        var input = _fixture.Create<CreateOrderInput>();
        
        _unitOfRepositoryMock.Setup(p => p.User.GetById(It.IsAny<string>())).ThrowsAsync(new Exception());
        var command = new CreateOrderCommand
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
