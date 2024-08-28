using FoodOrderApis.Common.Helpers;
using FoodService.Data.Models.Dtos;

namespace FoodService.Data.Responses;

public class GetAllFoodByUserIdResponse : BaseResponse
{
    public List<FoodDto> Data { get; set; }
}
