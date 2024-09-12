using CustomerService.Data.Requests;
using CustomerService.Data.Responses;
using MediatR;

namespace CustomerService.Features.Queries.UserInfoQueries.GetAllMerchant;

public class GetAllMerchantQuery : IRequest<GetAllMerchantByIdResponse>
{
    public GetAllMerchantRequest? Payload { get; set; }
}
