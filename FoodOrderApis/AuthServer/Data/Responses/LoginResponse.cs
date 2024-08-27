
using FoodOrderApis.Common.Helpers;

namespace AuthServer.Data.Dtos.Responses;

public class LoginResponse : BaseResponse
{
    public LoginDto Data { get; set; } = new();
}