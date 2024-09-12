using CustomerService.Data.Models;
using FoodOrderApis.Common.Helpers;
using FoodOrderApis.Common.Models.Dtos;

namespace CustomerService.Data.Responses;

public class GetAllMerchantByIdResponse : BaseResponse
{
    public List<UserInfo> Data { get; set; }
    public PagingDto Paging { get; set; }
}
