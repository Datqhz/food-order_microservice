using MediatR;
using OrderService.Data.Responses;

namespace OrderService.Features.Queries.OrderDetailQueries.CheckFoodIsUsed;

public class CheckFoodIsUsedQuery : IRequest<CheckFoodIsUsedResponse>
{
    public int FoodId { get; set; }
}
