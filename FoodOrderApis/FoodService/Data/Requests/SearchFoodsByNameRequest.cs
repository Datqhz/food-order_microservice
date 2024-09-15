using FoodOrderApis.Common.Enums;

namespace FoodService.Data.Requests;

public class SearchFoodsByNameRequest
{
    public string Keyword { get; set; }
    public int SortBy { get; set; } = (int)SortOption.ByAlphabeticalAscending;
    public int? Page { get; set; }
    public int? MaxPerPage { get; set; }
}
