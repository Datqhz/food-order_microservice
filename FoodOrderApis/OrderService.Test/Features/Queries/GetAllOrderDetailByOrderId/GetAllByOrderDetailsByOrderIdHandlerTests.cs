using System.Linq.Expressions;
using AutoFixture;
using FoodOrderApis.Common.Helpers;
using Microsoft.Extensions.Logging;
using MockQueryable;
using Moq;
using OrderService.Data.Models;
using OrderService.Features.Queries.OrderDetailQueries.GetAllOrderDetailsByOrderId;
using OrderService.Features.Queries.OrderQueries.GetAllOrderByUserId;
using OrderService.Repositories;
using OrderService.Test.Extensions;

namespace OrderService.Test.Features.Queries.GetAllOrderDetailByOrderId;


[TestFixture]
public class GetAllByOrderDetailsByOrderIdHandlerTests
{
    private readonly Mock<IUnitOfRepository> _unitOfRepositoryMock;
    private readonly Mock<ILogger<GetAllOrderDetailsByOrderIdHandler>> _loggerMock;
    private readonly Fixture _fixture;
    private readonly GetAllOrderDetailsByOrderIdHandler _handler;
    private readonly CancellationToken _cancellationToken;

    public GetAllByOrderDetailsByOrderIdHandlerTests()
    {
        _unitOfRepositoryMock = new Mock<IUnitOfRepository>();
        _loggerMock = new Mock<ILogger<GetAllOrderDetailsByOrderIdHandler>>();
        _fixture = new Fixture().OmitOnRecursionBehavior();
        _cancellationToken = new CancellationToken();
        _handler = new GetAllOrderDetailsByOrderIdHandler(_unitOfRepositoryMock.Object, _loggerMock.Object);
    }

    [Test]
    public async Task Handle_ShouldReturn_StatusOK()
    {
        // Arrange
        var orderId = 1;
        var orderDetails = _fixture.Build<OrderDetail>()
            .With(order => order.OrderId, orderId)
            .CreateMany(10)
            .AsQueryable()
            .BuildMock();

        _unitOfRepositoryMock.Setup(p => p.OrderDetail.Where(It.IsAny<Expression<Func<OrderDetail, bool>>>()))
            .Returns(orderDetails);
        
        var query = new GetAllOrderDetailsByOrderIdQuery(){OrderId = orderId};
        
        // Act
        var result  = await _handler.Handle(query, _cancellationToken);
        
        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo((int) ResponseStatusCode.OK));
    }
    [Test]
    public async Task Handle_ShouldReturn_StatusInternalServerError()
    {
        // Arrange
        var orderId = 1;

        _unitOfRepositoryMock.Setup(p => p.OrderDetail.Where(It.IsAny<Expression<Func<OrderDetail, bool>>>()))
            .Throws(new Exception());
        
        var query = new GetAllOrderDetailsByOrderIdQuery(){OrderId = orderId};
        
        // Act
        var result  = await _handler.Handle(query, _cancellationToken);
        
        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo((int) ResponseStatusCode.InternalServerError));
    }
}