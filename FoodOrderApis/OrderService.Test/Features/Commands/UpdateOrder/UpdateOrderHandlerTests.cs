using AutoFixture;
using FoodOrderApis.Common.Helpers;
using Moq;
using OrderService.Data.Models;
using OrderService.Data.Requests;
using OrderService.Data.Responses;
using OrderService.Features.Commands.OrderCommands.UpdateOrder;
using OrderService.Repositories;
using OrderService.Test.Extensions;

namespace OrderService.Test.Features.Commands.UpdateOrder;

[TestFixture]
public class UpdateOrderHandlerTests
{
    private readonly Mock<IUnitOfRepository> _unitOfRepositoryMock;
    private readonly Fixture _fixture;
    private readonly CancellationToken _cancellationToken;
    private readonly UpdateOrderHandler _handler;

    public UpdateOrderHandlerTests()
    {
        _unitOfRepositoryMock = new Mock<IUnitOfRepository>();
        _fixture = new Fixture().OmitOnRecursionBehavior();
        _cancellationToken = new CancellationToken();
        _handler = new UpdateOrderHandler(_unitOfRepositoryMock.Object);
    }

    [Test]
    public async Task Handle_ShouldReturn_StatusOK()
    {
        // Arrange
        var input = _fixture.Create<UpdateOrderInput>();
        var order = _fixture.Build<Order>().With(x => x.Id, input.OrderId).Create();
        _unitOfRepositoryMock.Setup(p => p.Order.GetById(It.IsAny<int>())).ReturnsAsync(order);
        _unitOfRepositoryMock.Setup(p => p.Order.Update(It.IsAny<Order>())).Returns(true);

        var command = new UpdateOrderCommand
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
    public async Task Handle_ShouldReturn_StatusNotFound()
    {
        // Arrange
        var input = _fixture.Create<UpdateOrderInput>();
        _unitOfRepositoryMock.Setup(p => p.Order.GetById(It.IsAny<int>())).ReturnsAsync((Order)null);

        var command = new UpdateOrderCommand
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
        var input = _fixture.Create<UpdateOrderInput>();
        _unitOfRepositoryMock.Setup(p => p.Order.GetById(It.IsAny<int>())).ThrowsAsync(new Exception());

        var command = new UpdateOrderCommand
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