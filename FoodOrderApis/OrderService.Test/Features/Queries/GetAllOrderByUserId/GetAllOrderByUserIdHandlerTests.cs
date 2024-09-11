using System.Linq.Expressions;
using AutoFixture;
using FoodOrderApis.Common.Helpers;
using Microsoft.Extensions.Logging;
using MockQueryable;
using Moq;
using OrderService.Data.Models;
using OrderService.Data.Requests;
using OrderService.Features.Queries.OrderQueries.GetAllOrderByUserId;
using OrderService.Repositories;
using OrderService.Test.Extensions;

namespace OrderService.Test.Features.Queries.GetAllOrderByUserId;

public class GetAllOrderByUserIdHandlerTests
{
    private readonly Mock<IUnitOfRepository> _unitOfRepositoryMock;
    private readonly Mock<ILogger<GetAllOrderByUserIdHandler>> _loggerMock;
    private readonly Fixture _fixture;
    private readonly CancellationToken _cancellationToken;
    private readonly GetAllOrderByUserIdHandler _handler;

    public GetAllOrderByUserIdHandlerTests()
    {
        _unitOfRepositoryMock = new Mock<IUnitOfRepository>();
        _loggerMock = new Mock<ILogger<GetAllOrderByUserIdHandler>>();
        _fixture = new Fixture().OmitOnRecursionBehavior();
        _cancellationToken = new CancellationToken();
        _handler = new GetAllOrderByUserIdHandler(_unitOfRepositoryMock.Object, _loggerMock.Object);
    }

    [Test]
    public async Task Handle_ShouldReturn_StatusOK_When_FindByEaterId()
    {
        
        // Arrange
        var eaterId = _fixture.Create<string>();
        var orders = _fixture.Build<Order>()
            .With(x => x.EaterId, eaterId)
            .CreateMany(10)
            .AsQueryable()
            .BuildMock();

        _unitOfRepositoryMock.Setup(p => p.Order.Where(It.IsAny<Expression<Func<Order, bool>>>())).Returns(orders);

        var query = new GetAllOrderByUserIdQuery
        {
            Payload = new GetAllOrderByUserIdInput
            {
                EaterId = eaterId
            }
        };
        
        // Act
        var result = await _handler.Handle(query, _cancellationToken);
        
        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo((int)ResponseStatusCode.OK));
    }
    
    [Test]
    public async Task Handle_ShouldReturn_StatusOK_When_FindByMerchantId()
    {
        
        // Arrange
        var merchantId = _fixture.Create<string>();
        var orders = _fixture.Build<Order>()
            .With(x => x.EaterId, merchantId)
            .CreateMany(10)
            .AsQueryable()
            .BuildMock();

        _unitOfRepositoryMock.Setup(p => p.Order.Where(It.IsAny<Expression<Func<Order, bool>>>())).Returns(orders);

        var query = new GetAllOrderByUserIdQuery
        {
            Payload = new GetAllOrderByUserIdInput
            {
                MerchantId = merchantId
            }
        };
        
        // Act
        var result = await _handler.Handle(query, _cancellationToken);
        
        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo((int)ResponseStatusCode.OK));
    }
    
    [Test]
    public async Task Handle_ShouldReturn_StatusInternalServerError()
    {
        
        // Arrange
        _unitOfRepositoryMock.Setup(p => p.Order.Where(It.IsAny<Expression<Func<Order, bool>>>())).Throws(new Exception());

        var query = new GetAllOrderByUserIdQuery
        {
            Payload = new GetAllOrderByUserIdInput
            {
                MerchantId = "aaaaa"
            }
        };
        
        // Act
        var result = await _handler.Handle(query, _cancellationToken);
        
        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo((int)ResponseStatusCode.InternalServerError));
    }
}
