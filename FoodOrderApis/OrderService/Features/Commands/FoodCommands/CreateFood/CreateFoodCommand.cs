using MediatR;
using OrderService.Data.Requests;

namespace OrderService.Features.Commands.FoodCommands.CreateFood;

public class CreateFoodCommand : IRequest
{
    public ModifyFoodInput Payload { get; set; }
}
