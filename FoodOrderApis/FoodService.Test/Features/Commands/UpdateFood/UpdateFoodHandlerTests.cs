using System.Net;
using AutoFixture;
using FoodOrderApis.Common.Helpers;
using FoodOrderApis.Common.HttpContextCustom;
using FoodOrderApis.Common.MassTransit.Core;
using FoodService.Data.Models;
using FoodService.Data.Requests;
using FoodService.Features.Commands.FoodCommands.UpdateFoodCommands;
using FoodService.Repositories;
using FoodService.Test.Extensions;
using Microsoft.Extensions.Logging;
using Moq;

namespace FoodService.Test.Features.Commands.UpdateFood;

public class UpdateFoodHandlerTests
{
    private readonly Mock<IUnitOfRepository> _unitOfRepositoryMock;
    private readonly Mock<ISendEndpointCustomProvider> _sendEndpointMock;
    private readonly Fixture _fixture;
    private readonly CancellationToken _cancellationToken;
    private readonly UpdateFoodHandler _handler;
    private readonly Mock<ICustomHttpContextAccessor> _httpContextAccessorMock;
    private readonly Mock<ILogger<UpdateFoodHandler>> _loggerMock;

    public UpdateFoodHandlerTests()
    {
        _unitOfRepositoryMock = new Mock<IUnitOfRepository>();
        _sendEndpointMock = new Mock<ISendEndpointCustomProvider>();
        _fixture = new Fixture().OmitOnRecursionBehavior();
        _cancellationToken = new CancellationToken();
        _httpContextAccessorMock = new Mock<ICustomHttpContextAccessor>();
        _loggerMock = new Mock<ILogger<UpdateFoodHandler>>();
        _handler = new UpdateFoodHandler(_unitOfRepositoryMock.Object, _sendEndpointMock.Object, _httpContextAccessorMock.Object, _loggerMock.Object);
    }

    [Test]
    public async Task Handle_ShouldReturn_StatusOK()
    {
        // Arrange
        var food = _fixture.Build<Food>().Create();
        var input = _fixture.Build<UpdateFoodInput>().With(x => x.Id, food.Id).Create();
        _unitOfRepositoryMock.Setup(p => p.Food.GetById(input.Id)).ReturnsAsync(food);
        _unitOfRepositoryMock.Setup(p => p.Food.Update(It.IsAny<Food>())).Returns(true);
        _httpContextAccessorMock.Setup(p => p.GetCurrentUserId()).Returns(food.UserId);
        var command = new UpdateFoodCommand
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
        var input = _fixture.Build<UpdateFoodInput>().Create();
        _unitOfRepositoryMock.Setup(p => p.Food.GetById(It.IsAny<int>())).ReturnsAsync((Food)null);

        var command = new UpdateFoodCommand
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
    public async Task Handle_ShouldReturn_StatusForbidden()
    {
        // Arrange
        var currentUserId = _fixture.Create<Guid>().ToString();
        var food = _fixture.Build<Food>().Create();
        var input = _fixture.Build<UpdateFoodInput>().With(x => x.Id, food.Id).Create();
        _unitOfRepositoryMock.Setup(p => p.Food.GetById(input.Id)).ReturnsAsync(food); ;
        _httpContextAccessorMock.Setup(p => p.GetCurrentUserId()).Returns(currentUserId);
        var command = new UpdateFoodCommand
        {
            Payload = input
        };
        
        // Act 
        var result = await _handler.Handle(command, _cancellationToken);
        
        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo((int)ResponseStatusCode.Forbidden));
    }
    
    [Test]
    public async Task Handle_ShouldReturn_StatusInternalServerError()
    {
        // Arrange
        var input = _fixture.Build<UpdateFoodInput>().Create();
        _unitOfRepositoryMock.Setup(p => p.Food.GetById(It.IsAny<int>())).ThrowsAsync(new Exception());
        var command = new UpdateFoodCommand
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
