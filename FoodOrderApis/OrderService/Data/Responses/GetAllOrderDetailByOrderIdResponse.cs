using FoodOrderApis.Common.Helpers;
using OrderService.Data.Models.Dtos;

namespace OrderService.Data.Responses;

public class GetAllOrderDetailByOrderIdResponse : BaseResponse
{
    public List<OrderDetailDto> Data { get; set; }
}
