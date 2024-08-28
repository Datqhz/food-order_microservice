using FoodService.Data.Responses;
using MediatR;

namespace FoodService.Features.Commands.FoodCommands.DeleteFood;

public class DeleteFoodCommand : IRequest<DeleteFoodResponse>
{
    public int Id { get; set; }
}
