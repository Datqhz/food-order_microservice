using AutoFixture;
using FoodOrderApis.Common.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using OrderService.Data.Models;
using OrderService.Data.Requests;
using OrderService.Features.Commands.OrderDetailCommands.ModifyMultipleOrderDetail;
using OrderService.Repositories;
using OrderService.Test.Extensions;

namespace OrderService.Test.Features.Commands.ModifyMultipleOrderDetails;
[TestFixture]
public class ModifyMultipleOrderDetailsHandlerTests
{
    private readonly Mock<IUnitOfRepository> _unitOfRepositoryMock;
    private readonly Fixture _fixture;
    private readonly CancellationToken _cancellationToken;
    private readonly ModifyMultipleOrderDetailHandler _handler;
    private readonly Mock<ILogger<ModifyMultipleOrderDetailHandler>> _loggerMock;

    public ModifyMultipleOrderDetailsHandlerTests()
    {
        _unitOfRepositoryMock = new Mock<IUnitOfRepository>();
        _fixture = new Fixture().OmitOnRecursionBehavior();
        _cancellationToken = new CancellationToken();
        _loggerMock = new Mock<ILogger<ModifyMultipleOrderDetailHandler>>();
        _handler = new ModifyMultipleOrderDetailHandler(_unitOfRepositoryMock.Object, _loggerMock.Object);
    }

    [Test]
    public async Task Handler_ShouldReturn_StatusOK()
    {
        // Arrange
        var request = new ModifyOrderDetailRequest
        {
            OrderDetailId = 1,
            Feature = 2,
            Price = 10000,
            Quantity = 1
        };
        var orderDetail = _fixture.Build<OrderDetail>()
            .With(x => x.Id, request.OrderDetailId)
            .Create();
        _unitOfRepositoryMock.Setup(p => p.OrderDetail.GetById(It.IsAny<int>())).ReturnsAsync(orderDetail);
        _unitOfRepositoryMock.Setup(p => p.OrderDetail.Update(It.IsAny<OrderDetail>())).Returns(true);
        _unitOfRepositoryMock.Setup(p => p.OrderDetail.Delete(It.IsAny<OrderDetail>())).Returns(true);

        var command = new ModifyMultipleOrderDetailCommand()
        {
            Payload = new List<ModifyOrderDetailRequest>(){request}
        };
        
        // Act 
        var result = await _handler.Handle(command, _cancellationToken);
        
        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo((int)ResponseStatusCode.OK));
    }
    
    [Test]
    public async Task Handler_ShouldReturn_StatusInternalServerError()
    {
        // Arrange
        var request = new ModifyOrderDetailRequest
        {
            OrderDetailId = 1,
            Feature = 2,
            Price = 10000,
            Quantity = 1
        };
        _unitOfRepositoryMock.Setup(p => p.OrderDetail.GetById(It.IsAny<int>())).ThrowsAsync(new Exception());

        var command = new ModifyMultipleOrderDetailCommand()
        {
            Payload = new List<ModifyOrderDetailRequest>(){request}
        };
        
        // Act 
        var result = await _handler.Handle(command, _cancellationToken);
        
        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo((int)ResponseStatusCode.InternalServerError));
    }
}