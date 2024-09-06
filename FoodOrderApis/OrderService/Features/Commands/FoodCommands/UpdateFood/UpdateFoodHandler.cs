using MediatR;
using OrderService.Data.Models;
using OrderService.Repositories;
using OrderService.Repositories.Interfaces;

namespace OrderService.Features.Commands.FoodCommands.UpdateFood;

public class UpdateFoodHandler : IRequestHandler<UpdateFoodCommand>
{
    private readonly IUnitOfRepository _unitOfRepository;
    private readonly ILogger<UpdateFoodHandler> _logger;

    public UpdateFoodHandler(IUnitOfRepository unitOfRepository, ILogger<UpdateFoodHandler> logger)
    {
        _unitOfRepository = unitOfRepository;
        _logger = logger;
    }
    public async Task Handle(UpdateFoodCommand request, CancellationToken cancellationToken)
    {
        var functionName = nameof(UpdateFoodHandler);
        try
        {
            _logger.LogInformation($"{functionName} - Start");
            var payload = request.Payload;
            var updateFood = new Food
            {
                Id = payload.FoodId,
                Name = payload.Name,
                Describe = payload.Describe,
                ImageUrl = payload.ImageUrl,
            };
            var updateResult = _unitOfRepository.Food.Update(updateFood);
            if (updateResult)
            {
                await _unitOfRepository.CompleteAsync();
            }
            _logger.LogInformation($"{functionName} - End");
        }
        catch (Exception ex)
        {
            _logger.LogError($"{functionName} => Has error : Message = {ex.Message}");
        }
    }
}
