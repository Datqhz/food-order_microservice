using FoodOrderApis.Common.Enums;

namespace CustomerService.Data.Requests;

public class SearchMerchantByNameRequest
{
    public string Keyword { get; set; }
    public int SortBy { get; set; } = (int)SortOption.ByAlphabeticalAscending;
    public int? Page { get; set; }
    public int? MaxPerPage { get; set; }
}
