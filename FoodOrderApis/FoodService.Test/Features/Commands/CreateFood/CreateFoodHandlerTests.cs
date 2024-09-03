using FoodService.Features.Commands.FoodCommands.CreateFoodCommands;
using FoodService.Repositories;
using MassTransit;
using Moq;

namespace FoodService.Test.Features.Commands.CreateFood;

[TestFixture]
public class CreateFoodHandlerTests
{
    private readonly Mock<IUnitOfRepository> _mockUnitOfRepository;
    private readonly Mock<IBusControl> _mockBus;

    public CreateFoodHandlerTests()
    {
        _mockUnitOfRepository = new Mock<IUnitOfRepository>();
        _mockBus = new Mock<IBusControl>();
    }

    [Test]
    public async Task Handler_ShouldReturnSuccess_WhenFoodCreated()
    {
        //_mockUnitOfRepository.Setup();
    }
    
}