using FoodService.Data.Requests;
using FoodService.Data.Responses;
using MediatR;

namespace FoodService.Features.Commands.FoodCommands.CreateFoodCommands;

public class CreateFoodCommand : IRequest<CreateFoodResponse>
{
    public CreateFoodRequest Payload { get; set; }
}
