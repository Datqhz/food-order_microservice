using System.Linq.Expressions;
using AutoFixture;
using FoodOrderApis.Common.Helpers;
using Microsoft.Extensions.Logging;
using MockQueryable;
using Moq;
using OrderService.Data.Models;
using OrderService.Data.Responses;
using OrderService.Features.Queries.OrderQueries.GetOrderById;
using OrderService.Repositories;
using OrderService.Test.Extensions;

namespace OrderService.Test.Features.Queries.GetOrderById;

public class GetOrderByIdHandlerTests
{
    private readonly Mock<IUnitOfRepository> _unitOfRepositoryMock;
    private readonly Mock<ILogger<GetOrderByIdHandler>> _loggerMock;
    private readonly Fixture _fixture;
    private readonly CancellationToken _cancellationToken;
    private readonly GetOrderByIdHandler _handler;

    public GetOrderByIdHandlerTests()
    {
        _unitOfRepositoryMock = new Mock<IUnitOfRepository>();
        _loggerMock = new Mock<ILogger<GetOrderByIdHandler>>();
        _fixture = new Fixture().OmitOnRecursionBehavior();
        _cancellationToken = new CancellationToken();
        _handler = new GetOrderByIdHandler(_unitOfRepositoryMock.Object, _loggerMock.Object);
    }

    [Test]
    public async Task Handle_ShouldReturn_StatusOK()
    {
        // Arrange
        var orderId = _fixture.Create<int>();
        var order = _fixture.Build<Order>()
            .With(x => x.Id, orderId)
            .CreateMany(1)
            .AsQueryable()
            .BuildMock();
        _unitOfRepositoryMock.Setup(x => x.Order.Where(It.IsAny<Expression<Func<Order, bool>>>())).Returns(order);
        
        var query = new GetOrderByIdQuery
        {
            Id = orderId
        };
        
        // Act
        var result = await _handler.Handle(query, _cancellationToken);
        
        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo((int) ResponseStatusCode.OK));
    }
    
    [Test]
    public async Task Handle_ShouldReturn_StatusInternalServerError()
    {
        // Arrange
        _unitOfRepositoryMock.Setup(x => x.Order.GetById(It.IsAny<int>())).ThrowsAsync(new Exception());
        
        var query = new GetOrderByIdQuery
        {
            Id = 1
        };
        
        // Act
        var result = await _handler.Handle(query, _cancellationToken);
        
        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo((int) ResponseStatusCode.InternalServerError));
    }
}
