using CustomerService.Data.Models;
using FoodOrderApis.Common.Helpers;

namespace CustomerService.Data.Responses;

public class GetAllMerchantByIdResponse : BaseResponse
{
    public List<UserInfo> Data { get; set; }
}
