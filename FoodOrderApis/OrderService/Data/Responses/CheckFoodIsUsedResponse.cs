using FoodOrderApis.Common.Helpers;

namespace OrderService.Data.Responses;

public class CheckFoodIsUsedResponse : BaseResponse
{
    public bool Data { get; set; }
}
