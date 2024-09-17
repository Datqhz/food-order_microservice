using FoodOrderApis.Common.Enums;

namespace CustomerService.Data.Requests;

public class GetAllUserInfoRequest
{
    public FilterUser GetBy { get; set; } = FilterUser.Eater;
    public SortOption SortBy { get; set; } = SortOption.ByAlphabeticalAscending;
}
