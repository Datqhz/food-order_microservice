using MediatR;
using OrderService.Data.Responses;

namespace OrderService.Features.Queries.OrderQueries.GetOrderById;

public class GetOrderByIdQuery : IRequest<GetOrderByIdResponse>
{
    public int Id { get; set; }
}
