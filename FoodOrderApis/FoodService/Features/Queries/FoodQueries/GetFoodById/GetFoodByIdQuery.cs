using FoodService.Data.Responses;
using MediatR;

namespace FoodService.Features.Queries.FoodQueries.GetFoodById;

public class GetFoodByIdQuery : IRequest<GetFoodByIdResponse>
{
    public int Id { get; set; }
}
