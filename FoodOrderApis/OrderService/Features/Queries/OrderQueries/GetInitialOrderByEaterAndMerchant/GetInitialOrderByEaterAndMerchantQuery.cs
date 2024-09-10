using MediatR;
using OrderService.Data.Requests;
using OrderService.Data.Responses;

namespace OrderService.Features.Queries.OrderQueries.GetInitialOrderByEaterAndMerchant;

public class GetInitialOrderByEaterAndMerchantQuery : IRequest<GetInitialOrderByEaterAndMerchantResponse>
{
    public GetInitialOrderByEaterAndMerchantInput Payload { get; set; }
}
