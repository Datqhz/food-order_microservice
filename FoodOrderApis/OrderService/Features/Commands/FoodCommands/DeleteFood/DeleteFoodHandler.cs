using MediatR;
using OrderService.Repositories;

namespace OrderService.Features.Commands.FoodCommands.DeleteFood;

public class DeleteFoodHandler : IRequestHandler<DeleteFoodCommand>
{
    private readonly IUnitOfRepository _unitOfRepository;

    public DeleteFoodHandler(IUnitOfRepository unitOfRepository)
    {
        _unitOfRepository = unitOfRepository;
    }
    public async Task Handle(DeleteFoodCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var food = await _unitOfRepository.Food.GetById(request.FoodId);
            bool result = _unitOfRepository.Food.Delete(food);
            if (result)
            {
                await _unitOfRepository.CompleteAsync();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}
