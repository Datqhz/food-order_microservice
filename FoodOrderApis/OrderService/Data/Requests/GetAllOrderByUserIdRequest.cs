namespace OrderService.Data.Requests;

public class GetAllOrderByUserIdRequest
{
    public string? EaterId { get; set; }
    public string? MerchantId { get; set; }
}
