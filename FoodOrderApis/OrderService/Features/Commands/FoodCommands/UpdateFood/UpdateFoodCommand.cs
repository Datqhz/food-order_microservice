using MediatR;
using OrderService.Data.Requests;

namespace OrderService.Features.Commands.FoodCommands.UpdateFood;

public class UpdateFoodCommand : IRequest
{
    public ModifyFoodRequest Payload { get; set; }
}
