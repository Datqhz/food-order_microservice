using System.Linq.Expressions;
using AutoFixture;
using FoodOrderApis.Common.Helpers;
using FoodOrderApis.Common.MassTransit.Core;
using FoodService.Data.Models;
using FoodService.Data.Requests;
using FoodService.Features.Commands.FoodCommands.CreateFoodCommands;
using FoodService.Repositories;
using FoodService.Test.Extensions;
using MassTransit;
using MockQueryable;
using Moq;
using CreateFoodMessage = FoodOrderApis.Common.MassTransit.Contracts.CreateFood;

namespace FoodService.Test.Features.Commands.CreateFood;

[TestFixture]
public class CreateFoodHandlerTests
{
    private readonly Mock<IUnitOfRepository> _mockUnitOfRepository;
    private readonly Mock<ISendEndpointCustomProvider> _mockSendEndpoint;
    private readonly Fixture _fixture;
    private readonly CreateFoodHandler _handler;
    private readonly CancellationToken _cancellationToken;

    public CreateFoodHandlerTests()
    {
        _mockUnitOfRepository = new Mock<IUnitOfRepository>();
        _mockSendEndpoint = new Mock<ISendEndpointCustomProvider>();
        _handler = new CreateFoodHandler(_mockUnitOfRepository.Object, _mockSendEndpoint.Object);
        _cancellationToken = new CancellationToken();
        _fixture = new Fixture().OmitOnRecursionBehavior();
    }

    [Test]
    public async Task Handler_ShouldReturn_StatusCreated()
    {
        // Arrange
        var userId = _fixture.Create<Guid>().ToString();
        var user = new User
        {
            UserId = userId,
            DisplayName = "Yamato",
            PhoneNumber = "0987478774",
        };
        var food = new Food
        {
            Id = 1,
            Name = "Test",
            ImageUrl = "imageUrl",
            Describe = "a",
            Price = 20000,
            UserId = userId
        };

        var request = new CreateFoodInput
        {
            UserId = userId,
            Name = "Test",
            ImageUrl = "imageUrl",
            Describe = "a",
            Price = 20000,
        };
        var command = new CreateFoodCommand
        {
            Payload = request
        };
        var message = new CreateFoodMessage
        {
            Id = 1,
            Name = food.Name,
            Describe = food.Describe,
            ImageUrl = food.ImageUrl
        };
        _mockUnitOfRepository.Setup(x => x.User.GetById(userId)).ReturnsAsync(user);
        _mockUnitOfRepository.Setup(x => x.Food.Add(It.IsAny<Food>())).ReturnsAsync(food);
        //_mockSendEndpoint.Verify(x => x.SendMessage<CreateFoodMessage>(It.IsAny<CreateFoodMessage>(), It.IsAny<CancellationToken>(), "order-create-food"));
        
        // Act
        var actual = await _handler.Handle(command, _cancellationToken);
        
        //Assert
        Console.WriteLine(actual.ErrorMessage);
        Assert.IsNotNull(actual);
        Assert.That(actual.StatusCode, Is.EqualTo((int)ResponseStatusCode.Created));
    }
    
    [Test]
    public async Task Handler_ShouldReturn_StatusNotFound_UserNotFound()
    {
        // Arrange
        var userId = _fixture.Create<Guid>().ToString();

        var users = _fixture.Build<User>().CreateMany(0).AsQueryable().BuildMock();
        var request = _fixture.Build<CreateFoodInput>().With(p => p.UserId, userId).Create();
        var command = new CreateFoodCommand
        {
            Payload = request
        };
        
        _mockUnitOfRepository.Setup(x => x.User.Where(It.IsAny<Expression<Func<User, bool>>>())).Returns(users); ;
        
        // Actual
        var actual = await _handler.Handle(command, _cancellationToken);
        
        //Assert
        Console.WriteLine(actual.ErrorMessage);
        Assert.IsNotNull(actual);
        Assert.That(actual.StatusCode, Is.EqualTo((int)ResponseStatusCode.NotFound));
    }
    
    [Test]
    public async Task Handler_ShouldReturn_StatusInternalServerError()
    {
        // Arrange
        var request = _fixture.Build<CreateFoodInput>().Create();
        var command = new CreateFoodCommand
        {
            Payload = request
        };
        
        _mockUnitOfRepository.Setup(x => x.User.GetById(It.IsAny<string>())).Throws(new Exception());
        
        // Actual
        var actual = await _handler.Handle(command, _cancellationToken);
        
        //Assert
        Console.WriteLine(actual.ErrorMessage);
        Assert.IsNotNull(actual);
        Assert.That(actual.StatusCode, Is.EqualTo((int)ResponseStatusCode.InternalServerError));
    }

}
