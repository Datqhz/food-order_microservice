using FoodOrderApis.Common.Enums;

namespace OrderService.Data.Requests;

public class GetAllOrderByUserIdRequest
{
    public string? EaterId { get; set; }
    public string? MerchantId { get; set; }
    public int OrderStatus { get; set; }
    public int SortBy { get; set; } = (int)SortOption.ByDateAscending;
}
