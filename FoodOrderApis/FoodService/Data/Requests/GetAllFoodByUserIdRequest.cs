using FoodOrderApis.Common.Enums;

namespace FoodService.Data.Requests;

public class GetAllFoodByUserIdRequest
{
    public string UserId { get; set; }
    public int SortBy { get; set; } = (int)SortOption.ByAlphabeticalAscending;
}
