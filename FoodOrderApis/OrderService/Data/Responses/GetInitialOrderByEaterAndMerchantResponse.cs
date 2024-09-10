using FoodOrderApis.Common.Helpers;
using OrderService.Data.Models.Dtos;

namespace OrderService.Data.Responses;

public class GetInitialOrderByEaterAndMerchantResponse : BaseResponse
{
    public OrderDto Data { get; set; }
}
