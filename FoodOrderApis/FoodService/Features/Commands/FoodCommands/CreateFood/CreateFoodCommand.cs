using FoodService.Data.Requests;
using FoodService.Data.Responses;
using MediatR;

namespace FoodService.Features.Commands.FoodCommands.CreateFood;

public class CreateFoodCommand : IRequest<CreateFoodResponse>
{
    public CreateFoodInput Payload { get; set; }
}
