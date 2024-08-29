using FoodOrderApis.Common.Helpers;
using OrderService.Data.Models.Dtos;

namespace OrderService.Data.Responses;

public class GetAllOrderByUserIdResponse : BaseResponse
{
    public List<OrderDto> Data;
}
