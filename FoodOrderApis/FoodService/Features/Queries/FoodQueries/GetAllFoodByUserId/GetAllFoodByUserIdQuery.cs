using FoodService.Data.Requests;
using FoodService.Data.Responses;
using MediatR;

namespace FoodService.Features.Queries.FoodQueries.GetAllFoodByUserId;

public class GetAllFoodByUserIdQuery : IRequest<GetAllFoodByUserIdResponse>
{
    public GetAllFoodByUserIdRequest Payload { get; set; }
}
