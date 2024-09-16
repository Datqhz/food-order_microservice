using FoodOrderApis.Common.Enums;

namespace CustomerService.Data.Requests;

public class GetAllUserInfoRequest
{
    public int GetBy { get; set; } = (int)FilterUser.Eater;
    public int SortBy { get; set; } = (int)SortOption.ByAlphabeticalAscending;
}
