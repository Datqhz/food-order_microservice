using MediatR;
using OrderService.Repositories;

namespace OrderService.Features.Commands.FoodCommands.DeleteFood;

public class DeleteFoodHandler : IRequestHandler<DeleteFoodCommand>
{
    private readonly IUnitOfRepository _unitOfRepository;
    private readonly ILogger<DeleteFoodHandler> _logger;

    public DeleteFoodHandler(IUnitOfRepository unitOfRepository, ILogger<DeleteFoodHandler> logger)
    {
        _unitOfRepository = unitOfRepository;
        _logger = logger;
    }
    public async Task Handle(DeleteFoodCommand request, CancellationToken cancellationToken)
    {
        var functionName = nameof(DeleteFoodHandler);
        try
        {
            _logger.LogInformation($"{functionName} - Start");
            var food = await _unitOfRepository.Food.GetById(request.FoodId);
            bool result = _unitOfRepository.Food.Delete(food);
            if (result)
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
