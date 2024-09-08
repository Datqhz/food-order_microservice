using AutoFixture;
using FoodOrderApis.Common.Helpers;
using Microsoft.Extensions.Logging;
using Moq;
using OrderService.Data.Models;
using OrderService.Data.Requests;
using OrderService.Features.Commands.OrderDetailCommands.CreateOrderDetail;
using OrderService.Repositories;
using OrderService.Test.Extensions;

namespace OrderService.Test.Features.Commands.CreateOrderDetail;

public class CreateOrderDetailHandlerTests
{
    private readonly Mock<IUnitOfRepository> _unitOfRepositoryMock;
    private readonly Mock<ILogger<CreateOrderDetailHandler>> _loggerMock;
    private readonly Fixture _fixture;
    private readonly CancellationToken _cancellationToken;
    private readonly CreateOrderDetailHandler _handler;
        
    public CreateOrderDetailHandlerTests()
    {
        _unitOfRepositoryMock = new Mock<IUnitOfRepository>();
        _loggerMock = new Mock<ILogger<CreateOrderDetailHandler>>();
        _fixture = new Fixture().OmitOnRecursionBehavior();
        _cancellationToken = new CancellationToken();
        _handler = new CreateOrderDetailHandler(_unitOfRepositoryMock.Object, _loggerMock.Object);
    }

    [Test]
    public async Task Handle_ShouldReturn_StatusCreated()
    {
        // Arrange
        var request = new CreateOrderDetailInput
        {
            FoodId = 1,
            OrderId = 1,
            Quantity = 1,
            Price = 10000
        };
        var order = _fixture.Create<Order>();
        var food = _fixture.Create<Food>();
        _unitOfRepositoryMock.Setup(p => p.Order.GetById(It.IsAny<int>())).ReturnsAsync(order);
        _unitOfRepositoryMock.Setup(p => p.Food.GetById(It.IsAny<int>())).ReturnsAsync(food);
        _unitOfRepositoryMock.Setup(p => p.OrderDetail.Add(It.IsAny<OrderDetail>())).ReturnsAsync(new OrderDetail());

        var command = new CreateOrderDetailCommand
        {
            Payload = request
        };
        
        // Act 
        var result = await _handler.Handle(command, _cancellationToken);
        
        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo((int)ResponseStatusCode.Created));
    }
    
    [Test]
    public async Task Handle_ShouldReturn_StatusNotFound_Order()
    {
        // Arrange
        var request = new CreateOrderDetailInput
        {
            FoodId = 1,
            OrderId = 1,
            Quantity = 1,
            Price = 10000
        };
        _unitOfRepositoryMock.Setup(p => p.Order.GetById(It.IsAny<int>())).ReturnsAsync((Order)null);

        var command = new CreateOrderDetailCommand
        {
            Payload = request
        };
        
        // Act 
        var result = await _handler.Handle(command, _cancellationToken);
        
        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo((int)ResponseStatusCode.NotFound));
    }
    
    [Test]
    public async Task Handle_ShouldReturn_StatusNotFound_Food()
    {
        // Arrange
        var request = new CreateOrderDetailInput
        {
            FoodId = 1,
            OrderId = 1,
            Quantity = 1,
            Price = 10000
        };
        _unitOfRepositoryMock.Setup(p => p.Order.GetById(It.IsAny<int>())).ReturnsAsync(new Order());
        _unitOfRepositoryMock.Setup(p => p.Food.GetById(It.IsAny<int>())).ReturnsAsync((Food)null);

        var command = new CreateOrderDetailCommand
        {
            Payload = request
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
        var request = new CreateOrderDetailInput
        {
            FoodId = 1,
            OrderId = 1,
            Quantity = 1,
            Price = 10000
        };
        _unitOfRepositoryMock.Setup(p => p.Order.GetById(It.IsAny<int>())).ThrowsAsync(new Exception());

        var command = new CreateOrderDetailCommand
        {
            Payload = request
        };
        
        // Act 
        var result = await _handler.Handle(command, _cancellationToken);
        
        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo((int)ResponseStatusCode.InternalServerError));
    }
}

