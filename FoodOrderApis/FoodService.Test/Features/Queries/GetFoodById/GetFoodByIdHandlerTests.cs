using AutoFixture;
using FoodOrderApis.Common.Helpers;
using FoodService.Data.Models;
using FoodService.Data.Responses;
using FoodService.Features.Queries.FoodQueries.GetFoodById;
using FoodService.Repositories;
using FoodService.Test.Extensions;
using Moq;

namespace FoodService.Test.Features.Queries.GetFoodById;

public class GetFoodByIdHandlerTests
{
    private readonly Mock<IUnitOfRepository> _unitOfRepositoryMock;
    private readonly Fixture _fixture;
    private readonly CancellationToken _cancellationToken;
    private readonly GetFoodByIdHandler _handler;

    public GetFoodByIdHandlerTests()
    {
        _unitOfRepositoryMock = new Mock<IUnitOfRepository>();
        _fixture = new Fixture().OmitOnRecursionBehavior();
        _cancellationToken = new CancellationToken();
        _handler = new GetFoodByIdHandler(_unitOfRepositoryMock.Object);
    }

    [Test]
    public async Task Handle_ShouldReturn_StatusOK()
    {
        var food = _fixture.Create<Food>();
        _unitOfRepositoryMock.Setup(p => p.Food.GetById(It.IsAny<int>())).ReturnsAsync(food);
        
        var query = new GetFoodByIdQuery { Id = food.Id };
        
        // Act
        var result = await _handler.Handle(query, _cancellationToken);
        
        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo((int)ResponseStatusCode.OK));
    }
    
    [Test]
    public async Task Handle_ShouldReturn_StatusNotFound()
    {
        _unitOfRepositoryMock.Setup(p => p.Food.GetById(It.IsAny<int>())).ReturnsAsync((Food?)null);
        
        var query = new GetFoodByIdQuery { Id = 1 };
        
        // Act
        var result = await _handler.Handle(query, _cancellationToken);
        
        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo((int)ResponseStatusCode.NotFound));
    }
    
    [Test]
    public async Task Handle_ShouldReturn_StatusInternalServerError()
    {
        var food = _fixture.Create<Food>();
        _unitOfRepositoryMock.Setup(p => p.Food.GetById(It.IsAny<int>())).ThrowsAsync(new Exception());
        
        var query = new GetFoodByIdQuery { Id = food.Id };
        
        // Act
        var result = await _handler.Handle(query, _cancellationToken);
        
        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo((int)ResponseStatusCode.InternalServerError));
    }
}
