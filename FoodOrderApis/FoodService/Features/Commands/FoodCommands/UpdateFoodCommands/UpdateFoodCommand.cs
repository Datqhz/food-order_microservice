using FoodService.Data.Requests;
using FoodService.Data.Responses;
using MediatR;

namespace FoodService.Features.Commands.FoodCommands.UpdateFoodCommands;

public class UpdateFoodCommand : IRequest<UpdateFoodResponse>
{
    public UpdateFoodRequest Payload { get; set; }
}
