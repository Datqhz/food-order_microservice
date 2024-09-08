using System.Linq.Expressions;
using AutoFixture;
using FoodOrderApis.Common.Helpers;
using Microsoft.Extensions.Logging;
using MockQueryable;
using Moq;
using OrderService.Data.Models;
using OrderService.Features.Queries.OrderDetailQueries.CheckFoodIsUsed;
using OrderService.Repositories;
using OrderService.Test.Extensions;

namespace OrderService.Test.Features.Queries.CheckFoodIsUsed;

public class CheckFoodIsUsedHandlerTests
{
    private readonly Mock<IUnitOfRepository> _unitOfRepositoryMock;
    private readonly Mock<ILogger<CheckFoodIsUsedHandler>> _loggerMock;
    private readonly Fixture _fixture;
    private readonly CancellationToken _cancellationToken;
    private readonly CheckFoodIsUsedHandler _handler;

    public CheckFoodIsUsedHandlerTests()
    {
        _unitOfRepositoryMock = new Mock<IUnitOfRepository>();
        _loggerMock = new Mock<ILogger<CheckFoodIsUsedHandler>>();
        _fixture = new Fixture().OmitOnRecursionBehavior();
        _cancellationToken = new CancellationToken();
        _handler = new CheckFoodIsUsedHandler(_unitOfRepositoryMock.Object, _loggerMock.Object);
    }

    [Test]
    public async Task Handle_ShouldReturn_StatusOK()
    {
        // Arrange
        var request = 1;
        var orderDetails = _fixture.Build<OrderDetail>()
            .With(x => x.OrderId, request)
            .CreateMany(10)
            .AsQueryable()
            .BuildMock();
        _unitOfRepositoryMock.Setup(p => p.OrderDetail.Where(It.IsAny<Expression<Func<OrderDetail, bool>>>()))
            .Returns(orderDetails);
        
        var query = new CheckFoodIsUsedQuery(){FoodId = request};
        
        // Act 
        var result = await _handler.Handle(query, _cancellationToken);
        
        // Assert
        Assert.That(result , Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo((int)ResponseStatusCode.OK));
    }
    
    [Test]
    public async Task Handle_ShouldReturn_StatusInternalServerError()
    {
        // Arrange
        var request = 1;
        _unitOfRepositoryMock.Setup(p => p.OrderDetail.Where(It.IsAny<Expression<Func<OrderDetail, bool>>>()))
            .Throws(new Exception());
        
        var query = new CheckFoodIsUsedQuery(){FoodId = request};
        
        // Act 
        var result = await _handler.Handle(query, _cancellationToken);
        
        // Assert
        Assert.That(result , Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo((int)ResponseStatusCode.InternalServerError));
    }
}