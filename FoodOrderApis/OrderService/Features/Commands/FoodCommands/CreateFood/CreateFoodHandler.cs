using MediatR;
using OrderService.Data.Models;
using OrderService.Repositories;

namespace OrderService.Features.Commands.FoodCommands.CreateFood;

public class CreateFoodHandler : IRequestHandler<CreateFoodCommand>
{
    private readonly IUnitOfRepository _unitOfRepository;

    public CreateFoodHandler(IUnitOfRepository unitOfRepository)
    {
        _unitOfRepository = unitOfRepository;
    }
    public async Task Handle(CreateFoodCommand request, CancellationToken cancellationToken)
    {
        try
        {
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
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}
