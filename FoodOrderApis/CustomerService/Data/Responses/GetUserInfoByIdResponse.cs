using CustomerService.Data.Models;
using FoodOrderApis.Common.Helpers;

namespace CustomerService.Data.Responses;

public class GetUserInfoByIdResponse : BaseResponse
{
    public UserInfo Data { get; set; }
}
