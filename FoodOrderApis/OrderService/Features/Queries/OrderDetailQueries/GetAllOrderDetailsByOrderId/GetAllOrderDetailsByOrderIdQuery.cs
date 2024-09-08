using MediatR;
using OrderService.Data.Responses;

namespace OrderService.Features.Queries.OrderDetailQueries.GetAllOrderDetailsByOrderId;

public class GetAllOrderDetailsByOrderIdQuery : IRequest<GetAllOrderDetailByOrderIdResponse>
{
    public int OrderId { get; set; }
}
