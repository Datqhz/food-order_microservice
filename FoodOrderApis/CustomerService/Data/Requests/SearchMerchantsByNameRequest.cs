using FoodOrderApis.Common.Enums;

namespace CustomerService.Data.Requests;

public class SearchMerchantsByNameRequest
{
    public string Keyword { get; set; }
    public SortOption SortBy { get; set; } = SortOption.ByAlphabeticalAscending;
    public int? Page { get; set; }
    public int? MaxPerPage { get; set; }
}
