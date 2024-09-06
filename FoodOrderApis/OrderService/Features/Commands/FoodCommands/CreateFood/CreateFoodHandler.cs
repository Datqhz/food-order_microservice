using MediatR;
using OrderService.Data.Models;
using OrderService.Repositories;

namespace OrderService.Features.Commands.FoodCommands.CreateFood;

public class CreateFoodHandler : IRequestHandler<CreateFoodCommand>
{
    private readonly IUnitOfRepository _unitOfRepository;
    private readonly ILogger<CreateFoodHandler> _logger;

    public CreateFoodHandler(IUnitOfRepository unitOfRepository, ILogger<CreateFoodHandler> logger)
    {
        _unitOfRepository = unitOfRepository;
        _logger = logger;
    }
    public async Task Handle(CreateFoodCommand request, CancellationToken cancellationToken)
    {
        var functionName = nameof(CreateFoodHandler);
        try
        {
            _logger.LogInformation($"{functionName} - Start");
            var payload = request.Payload;
            var food = new Food
            {
                Id = payload.FoodId,
                Name = payload.Name,
                Describe = payload.Describe,
                ImageUrl = payload.ImageUrl
            };
            await _unitOfRepository.Food.Add(food);
            await _unitOfRepository.CompleteAsync();
            _logger.LogInformation($"{functionName} - End");
        }
        catch (Exception ex)
        {
            _logger.LogError($"{functionName} => Has error : Message = {ex.Message}");
        }
    }
}
