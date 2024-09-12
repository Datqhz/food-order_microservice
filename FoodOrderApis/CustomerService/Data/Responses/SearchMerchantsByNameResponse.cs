using CustomerService.Data.Models;
using FoodOrderApis.Common.Helpers;
using FoodOrderApis.Common.Models.Dtos;

namespace CustomerService.Data.Responses;

public class SearchMerchantsByNameResponse : BaseResponse
{
    public List<UserInfo> Data { get; set; } = new ();
    public PagingDto Paging { get; set; } = new();
}
