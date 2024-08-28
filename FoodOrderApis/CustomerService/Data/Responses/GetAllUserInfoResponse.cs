using CustomerService.Data.Models;
using FoodOrderApis.Common.Helpers;

namespace CustomerService.Data.Responses;

public class GetAllUserInfoResponse : BaseResponse
{
    public List<UserInfo> Data { get; set; }
}
