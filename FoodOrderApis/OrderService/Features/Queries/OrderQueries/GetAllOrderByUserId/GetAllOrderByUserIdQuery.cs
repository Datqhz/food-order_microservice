using MediatR;
using OrderService.Data.Responses;

namespace OrderService.Features.Queries.OrderQueries.GetAllOrderByUserId;

public class GetAllOrderByUserIdQuery : IRequest<GetAllOrderByUserIdResponse>
{
    public string? EaterId { get; set; }
    public string? MerchantId { get; set; }
}