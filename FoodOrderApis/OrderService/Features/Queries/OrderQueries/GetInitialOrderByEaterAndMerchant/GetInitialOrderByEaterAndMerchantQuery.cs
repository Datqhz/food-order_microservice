using MediatR;
using OrderService.Data.Requests;
using OrderService.Data.Responses;

namespace OrderService.Features.Queries.OrderQueries.GetInitialOrderByEaterAndMerchant;

public class GetInitialOrderByEaterAndMerchantQuery : IRequest<GetInitialOrderByEaterAndMerchantResponse>
{
    public GetInitialOrderByEaterAndMerchantRequest Payload { get; set; }
}
