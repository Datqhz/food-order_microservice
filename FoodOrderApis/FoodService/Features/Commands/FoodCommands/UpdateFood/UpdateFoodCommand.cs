using FoodService.Data.Requests;
using FoodService.Data.Responses;
using MediatR;

namespace FoodService.Features.Commands.FoodCommands.UpdateFood;

public class UpdateFoodCommand : IRequest<UpdateFoodResponse>
{
    public UpdateFoodInput Payload { get; set; }
}
