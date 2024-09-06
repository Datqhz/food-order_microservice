using System.Linq.Expressions;
using AutoFixture;
using FoodOrderApis.Common.Helpers;
using FoodService.Data.Models;
using FoodService.Features.Queries.FoodQueries.GetAllFoodByUserId;
using FoodService.Features.Queries.FoodQueries.GetFoodById;
using FoodService.Repositories;
using FoodService.Test.Extensions;
using Microsoft.Extensions.Logging;
using MockQueryable;
using Moq;

namespace FoodService.Test.Features.Queries.GetAllFoodByUserId;

public class GetAllFoodByUserIdHandlerTests
{
     private readonly Mock<IUnitOfRepository> _unitOfRepositoryMock;
    private readonly Fixture _fixture;
    private readonly CancellationToken _cancellationToken;
    private readonly GetAllFoodByUserIdHandler _handler;
    private readonly Mock<ILogger<GetAllFoodByUserIdHandler>> _logger;

    public GetAllFoodByUserIdHandlerTests()
    {
        _unitOfRepositoryMock = new Mock<IUnitOfRepository>();
        _fixture = new Fixture().OmitOnRecursionBehavior();
        _cancellationToken = new CancellationToken();
        _logger = new Mock<ILogger<GetAllFoodByUserIdHandler>>();
        _handler = new GetAllFoodByUserIdHandler(_unitOfRepositoryMock.Object, _logger.Object);
    }

    [Test]
    public async Task Handle_ShouldReturn_StatusOK()
    {
        var userId = _fixture.Create<Guid>().ToString();
        var foods = _fixture.Build<Food>()
            .With(x => x.UserId, userId)
            .CreateMany(10)
            .AsQueryable()
            .BuildMock();
        _unitOfRepositoryMock.Setup(p => p.Food.Where(It.IsAny<Expression<Func<Food, bool>>>())).Returns(foods);

        var query = new GetAllFoodByUserIdQuery {UserId = userId};
        
        // Act
        var result = await _handler.Handle(query, _cancellationToken);
        
        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo((int)ResponseStatusCode.OK));
    }

    
    [Test]
    public async Task Handle_ShouldReturn_StatusInternalServerError()
    {
        var userId = _fixture.Create<Guid>().ToString();
        _unitOfRepositoryMock.Setup(p => p.Food.Where(It.IsAny<Expression<Func<Food, bool>>>()))
            .Throws(new Exception());
        
        var query = new GetAllFoodByUserIdQuery { UserId  = userId };
        
        // Act
        var result = await _handler.Handle(query, _cancellationToken);
        
        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo((int)ResponseStatusCode.InternalServerError));
    }
}
