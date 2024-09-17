using FoodOrderApis.Common.Enums;

namespace OrderService.Data.Requests;

public class GetAllOrderByUserIdRequest
{
    public string? EaterId { get; set; }
    public string? MerchantId { get; set; }
    public int OrderStatus { get; set; }
    public SortOption SortBy { get; set; } = SortOption.ByDateAscending;
}
