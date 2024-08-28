using FoodOrderApis.Common.Helpers;
using FoodService.Data.Models.Dtos;

namespace FoodService.Data.Responses;

public class GetFoodByIdResponse : BaseResponse
{
    public FoodDto Data { get; set; }
}
