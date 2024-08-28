using FoodService.Data.Responses;
using MediatR;

namespace FoodService.Features.Queries.FoodQueries.GetAllFoodByUserId;

public class GetAllFoodByUserIdQuery : IRequest<GetAllFoodByUserIdResponse>
{
    public string UserId { get; set; }
}
