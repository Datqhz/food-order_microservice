namespace OrderService.Data.Requests;

public class GetAllOrderByUserIdInput
{
    public string? EaterId { get; set; }
    public string? MerchantId { get; set; }
}
