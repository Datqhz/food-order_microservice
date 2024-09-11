using MediatR;
using OrderService.Data.Requests;
using OrderService.Data.Responses;

namespace OrderService.Features.Queries.OrderQueries.GetAllOrderByUserId;

public class GetAllOrderByUserIdQuery : IRequest<GetAllOrderByUserIdResponse>
{
    public GetAllOrderByUserIdInput Payload { get; set; }
}