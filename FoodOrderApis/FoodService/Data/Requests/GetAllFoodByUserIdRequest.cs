using FoodOrderApis.Common.Enums;

namespace FoodService.Data.Requests;

public class GetAllFoodByUserIdRequest
{
    public string UserId { get; set; }
    public SortOption SortBy { get; set; } = SortOption.ByAlphabeticalAscending;
}
