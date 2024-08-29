using MediatR;
using OrderService.Data.Responses;

namespace OrderService.Features.Queries.OrderDetailQueries.GetAllOrderDetailByOrderId;

public class GetAllOrderDetailByOrderIdQuery : IRequest<GetAllOrderDetailByOrderIdResponse>
{
    public int OrderId { get; set; }
}
