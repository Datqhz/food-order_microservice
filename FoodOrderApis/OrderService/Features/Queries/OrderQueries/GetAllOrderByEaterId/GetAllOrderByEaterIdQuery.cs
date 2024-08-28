using MediatR;
using OrderService.Data.Responses;

namespace OrderService.Features.Queries.OrderQueries.GetAllOrderByEaterId;

public class GetAllOrderByEaterIdQuery : IRequest<GetAllOrderByUserIdResponse>
{
    public int EaterId { get; set; }
}