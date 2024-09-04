using MediatR;

namespace OrderService.Features.Commands.FoodCommands.DeleteFood;

public class DeleteFoodCommand : IRequest
{
    public int FoodId { get; set; }
}
