using AuthServer.Data.Dtos;
using FoodOrderApis.Common.Helpers;

namespace AuthServer.Data.Responses;

public class GetAllRolesResponse : BaseResponse
{
    public List<RoleDto> Data { get; set; }
}