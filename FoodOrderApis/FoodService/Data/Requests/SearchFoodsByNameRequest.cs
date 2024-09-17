using FoodOrderApis.Common.Enums;

namespace FoodService.Data.Requests;

public class SearchFoodsByNameRequest
{
    public string Keyword { get; set; }
    public SortOption SortBy { get; set; } = SortOption.ByAlphabeticalAscending;
    public int? Page { get; set; }
    public int? MaxPerPage { get; set; }
}
