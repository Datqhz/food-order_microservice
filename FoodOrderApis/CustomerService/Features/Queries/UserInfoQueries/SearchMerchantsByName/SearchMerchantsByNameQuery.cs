using CustomerService.Data.Requests;
using CustomerService.Data.Responses;
using MediatR;

namespace CustomerService.Features.Queries.UserInfoQueries.SearchMerchantsByName;

public class SearchMerchantsByNameQuery : IRequest<SearchMerchantsByNameResponse>
{
    public SearchMerchantsByNameRequest Payload { get; set; }
}
