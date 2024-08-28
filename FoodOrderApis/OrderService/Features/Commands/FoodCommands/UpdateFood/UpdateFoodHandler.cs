using MediatR;
using OrderService.Data.Models;
using OrderService.Repositories;
using OrderService.Repositories.Interfaces;

namespace OrderService.Features.Commands.FoodCommands.UpdateFood;

public class UpdateFoodHandler : IRequestHandler<UpdateFoodCommand>
{
    private readonly IUnitOfRepository _unitOfRepository;

    public UpdateFoodHandler(IUnitOfRepository unitOfRepository)
    {
        _unitOfRepository = unitOfRepository;
    }
    public async Task Handle(UpdateFoodCommand request, CancellationToken cancellationToken)
    {
        try
        {
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
            else
            {
                return;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}
