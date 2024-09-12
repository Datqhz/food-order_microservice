using AutoFixture;
using FoodOrderApis.Common.Helpers;
using Microsoft.Extensions.Logging;
using Moq;
using OrderService.Data.Models;
using OrderService.Data.Requests;
using OrderService.Features.Commands.OrderDetailCommands.UpdateOrderDetail;
using OrderService.Repositories;
using OrderService.Test.Extensions;

namespace OrderService.Test.Features.Commands.UpdateOrderDetail;

public class UpdateOrderDetailHandlerTests
{
    private readonly Mock<IUnitOfRepository> _unitOfRepositoryMock;
    private readonly Mock<ILogger<UpdateOrderDetailHandler>> _loggerMock;
    private readonly Fixture _fixture;
    private readonly CancellationToken _cancellationToken;
    private readonly UpdateOrderDetailHandler _handler;
        
    public UpdateOrderDetailHandlerTests()
    {
        _unitOfRepositoryMock = new Mock<IUnitOfRepository>();
        _loggerMock = new Mock<ILogger<UpdateOrderDetailHandler>>();
        _fixture = new Fixture().OmitOnRecursionBehavior();
        _cancellationToken = new CancellationToken();
        _handler = new UpdateOrderDetailHandler(_unitOfRepositoryMock.Object, _loggerMock.Object);
    }

    [Test]
    public async Task Handle_ShouldReturn_StatusOK()
    {
        // Arrange
        var request = new UpdateOrderDetailRequest
        {
            OrderDetailId = 1,
            Quantity = 1,
            Price = 10000,
            Feature = 2
        };
        var orderDetail = _fixture.Create<OrderDetail>();
        _unitOfRepositoryMock.Setup(p => p.OrderDetail.GetById(It.IsAny<int>())).ReturnsAsync(orderDetail);
        _unitOfRepositoryMock.Setup(p => p.OrderDetail.Update(It.IsAny<OrderDetail>())).Returns(true);
        _unitOfRepositoryMock.Setup(p => p.OrderDetail.Delete(It.IsAny<OrderDetail>())).Returns(true);
        var command = new UpdateOrderDetailCommand
        {
            Payload = request
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
        var request = new UpdateOrderDetailRequest
        {
            OrderDetailId = 1,
            Quantity = 1,
            Price = 10000,
            Feature = 2
        };
        _unitOfRepositoryMock.Setup(p => p.OrderDetail.GetById(It.IsAny<int>())).ReturnsAsync((OrderDetail)null);

        var command = new UpdateOrderDetailCommand
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
        var request = new UpdateOrderDetailRequest
        {
            OrderDetailId = 1,
            Quantity = 1,
            Price = 10000,
            Feature = 2
        };
        _unitOfRepositoryMock.Setup(p => p.OrderDetail.GetById(It.IsAny<int>())).ThrowsAsync(new Exception());

        var command = new UpdateOrderDetailCommand
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

