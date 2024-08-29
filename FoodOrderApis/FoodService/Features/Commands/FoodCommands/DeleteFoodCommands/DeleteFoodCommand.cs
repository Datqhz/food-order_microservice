using FoodService.Data.Responses;
using MediatR;

namespace FoodService.Features.Commands.FoodCommands.DeleteFoodCommands;

public class DeleteFoodCommand : IRequest<DeleteFoodResponse>
{
    public int Id { get; set; }
}
